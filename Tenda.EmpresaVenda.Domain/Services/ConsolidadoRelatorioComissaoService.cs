using Europa.Commons;
using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class ConsolidadoRelatorioComissaoService : BaseService
    {
        public PrePropostaRepository _prePropostaRepository { get; set; }
        public EmpreendimentoRepository _empreendimentoRepository { get; set; }
        public LojaRepository _lojaRepository { get; set; }
        public EmpresaVendaRepository _empresaVendaRepository { get; set; }
        public ConsolidadoRelatorioComissaoRepository _consolidadoRelatorioComissaoRepository { get; set; }
        public RegraComissaoEvsRepository _regraComissaoEvsRepository { get; set; }
        public AceiteRegraComissaoEvsRepository _aceiteRegraComissaoEvsRepository { get; set; }
        public ItemRegraComissaoRepository _itemRegraComissaoRepository { get; set; }
        public SinteseStatusContratoJunixRepository _sinteseStatusContratoJunixRepository { get; set; }
        public StatusConformidadeRepository _statusConformidadeRepository { get; set; }
        public FaseStatusContratoJunixRepository _faseStatusContratoJunixRepository { get; set; }
        public PagamentoRepository _pagamentoRepository { get; set; }
        public RateioComissaoRepository _rateioComissaoRepository { get; set; }
        public ValorNominalRepository _valorNominalRepository { get; set; }

        public void ProcessarProposta(PropostaSuat proposta)
        {
            var bre = new BusinessRuleException();

            //var consolidado = _consolidadoRelatorioComissaoRepository.FindByIdProposta(proposta.Id);


            var empreendimento = _empreendimentoRepository.BuscarPorDivisao(proposta.IdSapEmpreendimento);
            // EMPREENDIMENTO
            if (empreendimento.IsEmpty())
            {
                bre.AddError(string.Format("Empreendimento não integrado")).Complete();
                bre.ThrowIfHasError();
            }

            var loja = _lojaRepository.FindByIdSap(proposta.IdSapLoja);
            //LOJA
            if (loja.IsEmpty())
            {
                bre.AddError(string.Format("Loja não Integrada - IDSAP: {0}", proposta.IdSapLoja)).Complete();
                bre.ThrowIfHasError();
            }

            var empresaVenda = _empresaVendaRepository.FindByIdLoja(loja.Id);
            // EV
            if (empresaVenda.IsEmpty())
            {
                bre.AddError(string.Format("Loja não possui Empresa de Venda no Portal EV")).Complete();
                bre.ThrowIfHasError();
            }

            var consolidado = new ConsolidadoRelatorioComissao();
            var rateio = new RateioComissao();
            var empresaVendaRateio = new Tenda.Domain.EmpresaVenda.Models.EmpresaVenda();

            //Busca os consolidados referentes a proposta
            var consolidados = _consolidadoRelatorioComissaoRepository.Queryable()
                .Where(x => x.IdProposta == proposta.Id)
                .Where(x => x.Situacao == Situacao.Ativo)
                .ToList();

            var evs = new List<long> { empresaVenda.Id };

            if (consolidados.HasValue())
            {
                //verifica se os consolidados pertencem a EV encontrada
                consolidado = consolidados.Where(x => x.IdEmpresaVenda == empresaVenda.Id).SingleOrDefault();

                /*Buscando rateio para a EV
                     */
                empresaVendaRateio = BuscarRateio(proposta, empresaVenda, empreendimento);

                if (empresaVendaRateio.HasValue())
                {
                    evs.Add(empresaVendaRateio.Id);
                }
                /*caso o consolidado seja encontrado, verifica-se se há consolidado ligado a proposta 
                 * que não pertença ao rateio
                 */
                consolidados = consolidados.Where(x => !evs.Contains(x.IdEmpresaVenda))
                    .ToList();


                //lista dos pagamentos pagos
                var evsComPagamento = _pagamentoRepository.Queryable()
                    .Where(x => x.Proposta.Id==proposta.Id)
                    .Where(x => x.Pago == true)
                    .Select(x => x.EmpresaVenda.Id)
                    .Distinct()
                    .ToList();

                /*Remove os consolidados com pagamentos pagos
                    */
                consolidados = consolidados
                    .Where(x => !evsComPagamento.Contains(x.IdEmpresaVenda))
                    .ToList();

                /*Suspende os consolidados que não possuírem pagamento
                    */
                foreach (var con in consolidados)
                {
                    con.Situacao = Situacao.Suspenso;
                    con.AtualizadoEm = DateTime.Now;
                    con.AtualizadoPor = ProjectProperties.IdUsuarioSistema;
                    _consolidadoRelatorioComissaoRepository.Save(con);
                }
                
            }

            //caso não exista consolidado ligando a proposta a EV ligada ao ID SAP LOJA da proposta
            if (consolidado.IsEmpty())
            {
                consolidado = new ConsolidadoRelatorioComissao();
                consolidado.IdProposta = proposta.Id;
                consolidado.Situacao = Situacao.Ativo;
            }

            ItemRegraComissao item = null;
            RegraComissaoEvs regraComissaoEvs = null;

            //busca regra de comissão aceita com data de venda dentro do periodo
            regraComissaoEvs = _aceiteRegraComissaoEvsRepository.BuscarRegraComAceiteNaData(empresaVenda.Id, proposta.DataVenda.Value);
            if (!regraComissaoEvs.IsEmpty())
            {
                item = _itemRegraComissaoRepository.Buscar(regraComissaoEvs.RegraComissao.Id, empresaVenda.Id, empreendimento.Id);
            }

            //Busca regras de comissão com aceite e termino de vigencia aberto
            if (item.IsEmpty())
            {
                var listaRegras = _aceiteRegraComissaoEvsRepository.BuscarRegraAbertaComAceite(empresaVenda.Id, proposta.DataVenda.Value);
                if (!listaRegras.IsEmpty())
                {
                    foreach (var regra in listaRegras)
                    {
                        item = _itemRegraComissaoRepository.Buscar(regra.RegraComissao.Id, empresaVenda.Id, empreendimento.Id);
                        if (!item.IsEmpty())
                        {
                            regraComissaoEvs = regra;
                            break;
                        }
                    }
                }
            }

            //busca o primeiro aceite após a data de venda
            if (item.IsEmpty())
            {
                var lista = _aceiteRegraComissaoEvsRepository.BuscarPorEvEData(empresaVenda.Id, proposta.DataVenda.Value);
                foreach (var regra in lista)
                {
                    item = _itemRegraComissaoRepository.Buscar(regra.RegraComissao.Id, empresaVenda.Id, empreendimento.Id);
                    if (!item.IsEmpty())
                    {
                        regraComissaoEvs = regra;
                        break;
                    }
                }
            }
            // Separar até aqui

            if (regraComissaoEvs.IsEmpty())
            {
                bre.AddError("Não há Regra de Comissão aceita para a Data de Venda").Complete();
                bre.ThrowIfHasError();
            }

            if (item.IsEmpty())
            {
                bre.AddError("Não há Item de Regra de Comissão para o Empreendimento").Complete();
                bre.ThrowIfHasError();
            }

            //neste ponto já encontrou a regra de comissão correta

            consolidado.IdEmpreendimento = empreendimento.Id;
            consolidado.IdLoja = loja.Id;
            consolidado.IdEmpresaVenda = empresaVenda.Id;

            consolidado.StatusConformidade = proposta.StatusConformidade.IsEmpty() ? "" : "Status Genérico";
            consolidado.StatusContrato = proposta.StatusContrato.IsEmpty() ? "" : proposta.StatusContrato;
            consolidado.Sintese = proposta.Sintese.IsEmpty() ? "" : "Sintese Genérica";
            consolidado.Fase = proposta.Fase.IsEmpty() ? "" : "Fase Genérica";

            consolidado = DeterminarSintese(consolidado, proposta.Sintese, proposta.Fase);

            consolidado = DeterminarConformidade(consolidado, proposta.StatusConformidade);

            //PRÉ-PROPOSTA
            consolidado.IdPreProposta = proposta.PreProposta.IsEmpty() ? 0 : proposta.PreProposta.Id;

            consolidado.IdItemRegraComissao = item.Id;
            consolidado.IdRegraComissaoEvs = regraComissaoEvs.Id;
            consolidado.IdRegraComissao = regraComissaoEvs.RegraComissao.Id;

            /**
             *Regra de pagamento 
             */

            consolidado = RegraDePagamento(consolidado, empreendimento, item, proposta);

            consolidado.UltimaModificacao = proposta.DataPassoAtual.HasValue() ? proposta.DataPassoAtual.Value : proposta.AtualizadoEm;
            _consolidadoRelatorioComissaoRepository.Save(consolidado);

            #region Rateio de Comissão
            //Realizar consolidado para rateio
            rateio = _rateioComissaoRepository.BuscarRateioNaData(proposta.DataVenda.Value, empresaVenda.Id, empreendimento.Id);
            if (rateio.HasValue())
            {
                empresaVendaRateio = BuscarEmpresaRateio(rateio, empresaVenda.Id);
            }
            //Buscar Contrante ativo
            if (rateio.IsEmpty())
            {
                rateio = _rateioComissaoRepository.BuscarRateioAtivo(proposta.DataVenda.Value, empresaVenda.Id, empreendimento.Id);

                if (rateio.HasValue())
                {
                    empresaVendaRateio = BuscarEmpresaRateio(rateio, empresaVenda.Id);
                }
            }

            if (rateio.IsEmpty())
            {
                rateio = _rateioComissaoRepository.BuscarRateioNaDataTodosEmpreendimento(proposta.DataVenda.Value, empresaVenda.Id);

                if (rateio.HasValue())
                {
                    empresaVendaRateio = BuscarEmpresaRateio(rateio, empresaVenda.Id);
                }
            }

            if (rateio.IsEmpty())
            {
                rateio = _rateioComissaoRepository.BuscarRateioAtivoTodosEmpreendimento(proposta.DataVenda.Value, empresaVenda.Id);

                if (rateio.HasValue())
                {
                    empresaVendaRateio = BuscarEmpresaRateio(rateio, empresaVenda.Id);
                }
            }

            if (empresaVendaRateio.HasValue())
            {
                //Salvando a referência do rateio
                consolidado.IdRateio = rateio.Id;
                RateioComissao(consolidado, empresaVendaRateio, proposta, empreendimento);
            }
            #endregion
        }

        private Tenda.Domain.EmpresaVenda.Models.EmpresaVenda BuscarEmpresaRateio(RateioComissao rateio, long Id)
        {
            var empresaVendaRateio = new Tenda.Domain.EmpresaVenda.Models.EmpresaVenda();
            if (rateio.Contratante.Id == Id)
            {
                empresaVendaRateio = rateio.Contratada;
            }
            else
            {
                empresaVendaRateio = rateio.Contratante;
            }
            return empresaVendaRateio;
        }

        public ConsolidadoRelatorioComissao RegraDePagamento(ConsolidadoRelatorioComissao consolidado, Empreendimento empreendimento, ItemRegraComissao item, PropostaSuat proposta)
        {
            var bre = new BusinessRuleException();

            if (item.TipoModalidadeComissao == TipoModalidadeComissao.Nominal)
            {
                var valorNominal = _valorNominalRepository.Queryable()
                .Where(x => x.Empreendimento.Id == consolidado.IdEmpreendimento)
                .Where(x => x.InicioVigencia.Value.Date <= proposta.DataVenda.Value.Date)
                .Where(x => x.TerminoVigencia.Value.Date >= proposta.DataVenda.Value.Date)
                .FirstOrDefault();

                if (valorNominal.IsEmpty())
                {
                    valorNominal = _valorNominalRepository.Queryable()
                .Where(x => x.Empreendimento.Id == consolidado.IdEmpreendimento)
                .Where(x => x.InicioVigencia.Value.Date <= proposta.DataVenda.Value.Date)
                .Where(x => x.TerminoVigencia.Value.Date == null)
                .FirstOrDefault();
                }

                if (valorNominal.IsEmpty())
                {
                    bre.AddError(string.Format("Não há tabela de valor nominal válida para o Empreendimento {0}", empreendimento.Nome)).Complete();
                    bre.ThrowIfHasError();
                }

                consolidado.IdValorNominal = valorNominal.Id;

                if (proposta.Tipologia.ToUpper().Contains("PNE"))
                {
                    consolidado.Tipologia = Tipologia.PNE;
                    double porcentagem = 0;

                    if (proposta.ValorVGV < valorNominal.PNEDe)
                    {
                        porcentagem = item.MenorValorNominalPNE;
                    }
                    else if (proposta.ValorVGV > valorNominal.PNEAte)
                    {
                        porcentagem = item.MaiorValorNominalPNE;
                    }
                    else
                    {
                        porcentagem = item.IgualValorNominalPNE;
                    }

                    consolidado.ConformidadePNE = Convert.ToDecimal(item.ValorConformidade * porcentagem / 100);
                    consolidado.RepassePNE = Convert.ToDecimal(item.ValorRepasse * porcentagem / 100);
                    consolidado.KitCompletoPNE = Convert.ToDecimal(item.ValorKitCompleto * porcentagem / 100);
                    consolidado.Faixa = Convert.ToDecimal(porcentagem);
                }
                else if (proposta.FaixaUmMeio)
                {
                    consolidado.Tipologia = Tipologia.FaixaUmMeio;
                    double porcentagem = 0;

                    if (proposta.ValorVGV < valorNominal.FaixaUmMeioDe)
                    {
                        porcentagem = item.MenorValorNominalUmMeio;
                    }
                    else if (proposta.ValorVGV > valorNominal.FaixaUmMeioAte)
                    {
                        porcentagem = item.MaiorValorNominalUmMeio;
                    }
                    else
                    {
                        porcentagem = item.IgualValorNominalUmMeio;
                    }

                    consolidado.ConformidadeUmMeio = Convert.ToDecimal(item.ValorConformidade * porcentagem / 100);
                    consolidado.RepasseUmMeio = Convert.ToDecimal(item.ValorRepasse * porcentagem / 100);
                    consolidado.KitCompletoUmMeio = Convert.ToDecimal(item.ValorKitCompleto * porcentagem / 100);
                    consolidado.Faixa = Convert.ToDecimal(porcentagem);
                }
                else
                {
                    consolidado.Tipologia = Tipologia.FaixaDois;
                    double porcentagem = 0;

                    if (proposta.ValorVGV < valorNominal.FaixaDoisDe)
                    {
                        porcentagem = item.MenorValorNominalDois;
                    }
                    else if (proposta.ValorVGV > valorNominal.FaixaDoisAte)
                    {
                        porcentagem = item.MaiorValorNominalDois;
                    }
                    else
                    {
                        porcentagem = item.IgualValorNominalDois;
                    }

                    consolidado.ConformidadeDois = Convert.ToDecimal(item.ValorConformidade * porcentagem / 100);
                    consolidado.RepasseDois = Convert.ToDecimal(item.ValorRepasse * porcentagem / 100);
                    consolidado.KitCompletoDois = Convert.ToDecimal(item.ValorKitCompleto * porcentagem / 100);
                    consolidado.Faixa = Convert.ToDecimal(porcentagem);
                }
            }
            else if (item.TipoModalidadeComissao == TipoModalidadeComissao.Fixa)
            {
                consolidado.Tipologia = proposta.FaixaUmMeio ? Tipologia.FaixaUmMeio : Tipologia.FaixaDois;
                //valores de pagamento
                consolidado.ConformidadeUmMeio = Convert.ToDecimal(item.FaixaUmMeio * item.ValorConformidade / 100);
                consolidado.ConformidadeDois = Convert.ToDecimal(item.FaixaDois * item.ValorConformidade / 100);


                consolidado.KitCompletoUmMeio = Convert.ToDecimal(item.FaixaUmMeio * item.ValorKitCompleto / 100);
                consolidado.KitCompletoDois = Convert.ToDecimal(item.FaixaDois * item.ValorKitCompleto / 100);

                consolidado.RepasseUmMeio = Convert.ToDecimal(item.FaixaUmMeio * item.ValorRepasse / 100);
                consolidado.RepasseDois = Convert.ToDecimal(item.FaixaDois * item.ValorRepasse / 100);

            }

            consolidado = ValoresAPagar(consolidado, proposta);
            consolidado = Pagamento(consolidado);

            return consolidado;
        }

        public ConsolidadoRelatorioComissao DeterminarSintese(ConsolidadoRelatorioComissao consolidado, string sintese, string fase)
        {
            if (!sintese.IsEmpty() && !fase.IsEmpty())
            {
                var resultado = _sinteseStatusContratoJunixRepository.FindBySinteseEFase(sintese, fase);

                if (resultado.HasValue())
                {
                    consolidado.Sintese = resultado.Sintese;
                    consolidado.Fase = resultado.FaseJunix.Fase;
                    consolidado.StatusContrato = resultado.StatusContrato;
                    consolidado.IdSinteseStatusContratoJunix = resultado.Id;

                    consolidado.EmReversao = consolidado.StatusContrato.ToUpper().Equals("EM REVERSÃO");
                }
                else
                {
                    consolidado.EmReversao = false;
                }
            }
            else
            {
                consolidado.EmReversao = false;
            }

            return consolidado;
        }

        public ConsolidadoRelatorioComissao DeterminarConformidade(ConsolidadoRelatorioComissao consolidado, string conformidade)
        {
            if (!conformidade.IsEmpty())
            {
                var resultado = _statusConformidadeRepository.FindByConformidade(conformidade);

                if (!resultado.IsEmpty())
                {
                    consolidado.StatusConformidade = resultado.DescricaoEvs;
                    consolidado.IdStatusConformidade = resultado.Id;
                }
            }

            return consolidado;
        }

        public ConsolidadoRelatorioComissao ValoresAPagar(ConsolidadoRelatorioComissao consolidado, PropostaSuat proposta)
        {
            switch (consolidado.Tipologia)
            {
                case Tipologia.PNE:
                    consolidado.KitCompletoAPagar = proposta.ValorVGV * consolidado.KitCompletoPNE / 100;
                    consolidado.RepasseAPagar = proposta.ValorVGV * consolidado.RepassePNE / 100;
                    consolidado.ConformidadeAPagar = proposta.ValorVGV * consolidado.ConformidadePNE / 100;
                    break;
                case Tipologia.FaixaUmMeio:
                    consolidado.KitCompletoAPagar = proposta.ValorVGV * consolidado.KitCompletoUmMeio / 100;
                    consolidado.RepasseAPagar = proposta.ValorVGV * consolidado.RepasseUmMeio / 100;
                    consolidado.ConformidadeAPagar = proposta.ValorVGV * consolidado.ConformidadeUmMeio / 100;
                    break;
                case Tipologia.FaixaDois:
                    consolidado.KitCompletoAPagar = proposta.ValorVGV * consolidado.KitCompletoDois / 100;
                    consolidado.RepasseAPagar = proposta.ValorVGV * consolidado.RepasseDois / 100;
                    consolidado.ConformidadeAPagar = proposta.ValorVGV * consolidado.ConformidadeDois / 100;
                    break;
            }

            //if (proposta.FaixaUmMeio)
            //{
            //    consolidado.KitCompletoAPagar = proposta.ValorVGV * consolidado.KitCompletoUmMeio / 100;
            //    consolidado.RepasseAPagar = proposta.ValorVGV * consolidado.RepasseUmMeio / 100;
            //    consolidado.ConformidadeAPagar = proposta.ValorVGV * consolidado.ConformidadeUmMeio / 100;
            //}
            //else
            //{
            //    consolidado.KitCompletoAPagar = proposta.ValorVGV * consolidado.KitCompletoDois / 100;
            //    consolidado.RepasseAPagar = proposta.ValorVGV * consolidado.RepasseDois / 100;
            //    consolidado.ConformidadeAPagar = proposta.ValorVGV * consolidado.ConformidadeDois / 100;
            //}

            return consolidado;
        }

        public ConsolidadoRelatorioComissao Pagamento(ConsolidadoRelatorioComissao consolidado)
        {
            consolidado.IdPagamentoKitCompleto = _pagamentoRepository.BuscarIdPagamentoAtivo(consolidado.IdProposta, TipoPagamento.KitCompleto, consolidado.IdEmpresaVenda);
            consolidado.IdPagamentoRepasse = _pagamentoRepository.BuscarIdPagamentoAtivo(consolidado.IdProposta, TipoPagamento.Repasse, consolidado.IdEmpresaVenda);
            consolidado.IdPagamentoConformidade = _pagamentoRepository.BuscarIdPagamentoAtivo(consolidado.IdProposta, TipoPagamento.Conformidade, consolidado.IdEmpresaVenda);

            return consolidado;
        }

        private void RateioComissao(ConsolidadoRelatorioComissao consolidadoRelatorioComissao, Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda, PropostaSuat proposta, Empreendimento empreendimento)
        {
            var bre = new BusinessRuleException();

            var consolidado = _consolidadoRelatorioComissaoRepository.Queryable().Where(x => x.IdProposta == proposta.Id)
                                                                      .Where(x => x.IdEmpresaVenda == empresaVenda.Id).FirstOrDefault();
            if (consolidado.IsEmpty())
            {
                consolidado = new ConsolidadoRelatorioComissao();
                consolidado.IdProposta = consolidadoRelatorioComissao.IdProposta;
                consolidado.IdRateio = consolidadoRelatorioComissao.IdRateio;
                consolidado.Situacao = Situacao.Ativo;
            }

            var loja = _lojaRepository.FindByIdSap(proposta.IdSapLoja);

            ItemRegraComissao item = null;
            RegraComissaoEvs regraComissaoEvs = null;

            //busca regra de comissão aceita com data de venda dentro do periodo
            regraComissaoEvs = _aceiteRegraComissaoEvsRepository.BuscarRegraComAceiteNaData(empresaVenda.Id, proposta.DataVenda.Value);
            if (!regraComissaoEvs.IsEmpty())
            {
                item = _itemRegraComissaoRepository.Buscar(regraComissaoEvs.RegraComissao.Id, empresaVenda.Id, consolidadoRelatorioComissao.IdEmpreendimento);
            }

            //Busca regras de comissão com aceite e termino de vigencia aberto
            if (item.IsEmpty())
            {
                var listaRegras = _aceiteRegraComissaoEvsRepository.BuscarRegraAbertaComAceite(empresaVenda.Id, proposta.DataVenda.Value);
                if (!listaRegras.IsEmpty())
                {
                    foreach (var regra in listaRegras)
                    {
                        item = _itemRegraComissaoRepository.Buscar(regra.RegraComissao.Id, empresaVenda.Id, consolidadoRelatorioComissao.IdEmpreendimento);
                        if (!item.IsEmpty())
                        {
                            regraComissaoEvs = regra;
                            break;
                        }
                    }
                }
            }

            //busca o primeiro aceite após a data de venda
            if (item.IsEmpty())
            {
                var lista = _aceiteRegraComissaoEvsRepository.BuscarPorEvEData(empresaVenda.Id, proposta.DataVenda.Value);
                foreach (var regra in lista)
                {
                    item = _itemRegraComissaoRepository.Buscar(regra.RegraComissao.Id, empresaVenda.Id, consolidadoRelatorioComissao.IdEmpreendimento);
                    if (!item.IsEmpty())
                    {
                        regraComissaoEvs = regra;
                        break;
                    }
                }
            }
            // Separar até aqui

            if (regraComissaoEvs.IsEmpty())
            {
                bre.AddError("Não há Regra de Comissão aceita para a Data de Venda").Complete();
                bre.ThrowIfHasError();
            }

            if (item.IsEmpty())
            {
                bre.AddError("Não há Item de Regra de Comissão para o Empreendimento").Complete();
                bre.ThrowIfHasError();
            }

            consolidado.IdEmpreendimento = consolidadoRelatorioComissao.IdEmpreendimento;
            consolidado.IdLoja = consolidadoRelatorioComissao.IdLoja;
            consolidado.IdEmpresaVenda = empresaVenda.Id;

            consolidado.StatusConformidade = proposta.StatusConformidade.IsEmpty() ? "" : "Status Genérico";
            consolidado.StatusContrato = proposta.StatusContrato.IsEmpty() ? "" : proposta.StatusContrato;
            consolidado.Sintese = proposta.Sintese.IsEmpty() ? "" : "Sintese Genérica";
            consolidado.Fase = proposta.Fase.IsEmpty() ? "" : "Fase Genérica";

            consolidado = DeterminarSintese(consolidado, proposta.Sintese, proposta.Fase);

            consolidado = DeterminarConformidade(consolidado, proposta.StatusConformidade);

            //PRÉ-PROPOSTA
            consolidado.IdPreProposta = proposta.PreProposta.IsEmpty() ? 0 : proposta.PreProposta.Id;

            consolidado.IdItemRegraComissao = item.Id;
            consolidado.IdRegraComissaoEvs = regraComissaoEvs.Id;
            consolidado.IdRegraComissao = regraComissaoEvs.RegraComissao.Id;

            consolidado = RegraDePagamento(consolidado, empreendimento, item, proposta);

            consolidado.UltimaModificacao = proposta.DataPassoAtual.HasValue() ? proposta.DataPassoAtual.Value : new DateTime(0101, 01, 01, 01, 01, 01);
            _consolidadoRelatorioComissaoRepository.Save(consolidado);
        }
    
        private Tenda.Domain.EmpresaVenda.Models.EmpresaVenda BuscarRateio(PropostaSuat proposta, 
            Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda, Empreendimento empreendimento)
        {
            var rateio = new RateioComissao();
            var empresaVendaRateio = new Tenda.Domain.EmpresaVenda.Models.EmpresaVenda();

            //Realizar consolidado para rateio
            rateio = _rateioComissaoRepository.BuscarRateioNaData(proposta.DataVenda.Value, empresaVenda.Id, empreendimento.Id);
            if (rateio.HasValue())
            {
                empresaVendaRateio = BuscarEmpresaRateio(rateio, empresaVenda.Id);
            }
            //Buscar Contrante ativo
            if (rateio.IsEmpty())
            {
                rateio = _rateioComissaoRepository.BuscarRateioAtivo(proposta.DataVenda.Value, empresaVenda.Id, empreendimento.Id);

                if (rateio.HasValue())
                {
                    empresaVendaRateio = BuscarEmpresaRateio(rateio, empresaVenda.Id);
                }
            }

            if (rateio.IsEmpty())
            {
                rateio = _rateioComissaoRepository.BuscarRateioNaDataTodosEmpreendimento(proposta.DataVenda.Value, empresaVenda.Id);

                if (rateio.HasValue())
                {
                    empresaVendaRateio = BuscarEmpresaRateio(rateio, empresaVenda.Id);
                }
            }

            if (rateio.IsEmpty())
            {
                rateio = _rateioComissaoRepository.BuscarRateioAtivoTodosEmpreendimento(proposta.DataVenda.Value, empresaVenda.Id);

                if (rateio.HasValue())
                {
                    empresaVendaRateio = BuscarEmpresaRateio(rateio, empresaVenda.Id);
                }
            }

            //if (empresaVendaRateio.HasValue())
            //{
            //    //Salvando a referência do rateio
            //    consolidado.IdRateio = rateio.Id;
            //    RateioComissao(consolidado, empresaVendaRateio, proposta, empreendimento);
            //}
            return empresaVendaRateio;
        }
    }
}
