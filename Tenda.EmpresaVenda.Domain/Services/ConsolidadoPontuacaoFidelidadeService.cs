using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class ConsolidadoPontuacaoFidelidadeService : BaseService
    {
        public EmpreendimentoRepository _empreendimentoRepository { get; set; }
        public LojaRepository _lojaRepository { get; set; }
        public EmpresaVendaRepository _empresaVendaRepository { get; set; }
        public ItemPontuacaoFidelidadeRepository _itemPontuacaoFidelidadeRepository { get; set; }
        public ConsolidadoPontuacaoFidelidadeRepository _consolidadoPontuacaoFidelidadeRepository { get; set; }
        public PropostaSuatRepository _propostaSuatRepository { get; set; }
        public PontuacaoFidelidadeEmpresaVendaRepository _pontuacaoFidelidadeEmpresaVendaRepository { get; set; }
        public ValorNominalRepository _valorNominalRepository { get; set; }
        public void ContabilizarPontuacao(PropostaSuat proposta)
        {
            var bre = new BusinessRuleException();

            if (proposta.IsEmpty())
            {
                bre.AddError(GlobalMessages.PropostaNaoIdentificada).Complete();
                bre.ThrowIfHasError();
            }

            if (!proposta.KitCompleto)
            {
                return;
            }

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

            //ITEM
            var itens = _itemPontuacaoFidelidadeRepository.BuscarItensNaData(empresaVenda.Id, empreendimento.Id, proposta.DataKitCompleto.Value);

            if (itens.IsEmpty())
            {
                itens = _itemPontuacaoFidelidadeRepository.BuscarItensVigentes(empresaVenda.Id, empreendimento.Id, proposta.DataKitCompleto.Value);
            }

            if (itens.IsEmpty())
            {
                bre.AddError(string.Format("Este empreendimento não possui item válido")).Complete();
                bre.ThrowIfHasError();
            }
            
            PontuacaoFidelidadeEmpresaVenda pontuacaoEmpresaVenda = null;
            var consolidado = _consolidadoPontuacaoFidelidadeRepository.FindByIdProposta(proposta.Id);
            
            if (consolidado.IsEmpty()) {
                consolidado = new ConsolidadoPontuacaoFidelidade
                {
                    IdProposta = proposta.Id,
                    IdEmpreendimento = empreendimento.Id,
                    IdEmpresaVenda = empresaVenda.Id
                };
            }
            else
            {
                if(!consolidado.Faturado && proposta.Faturado)
                {
                    pontuacaoEmpresaVenda = _pontuacaoFidelidadeEmpresaVendaRepository.FindByIdEmpresaVenda(empresaVenda.Id);
                    consolidado.SituacaoPontuacao = SituacaoPontuacao.Disponivel;
                    consolidado.Faturado = proposta.Faturado;

                    pontuacaoEmpresaVenda.PontuacaoDisponivel += consolidado.Pontuacao;
                    pontuacaoEmpresaVenda.PontuacaoIndisponivel -= consolidado.Pontuacao;

                    _pontuacaoFidelidadeEmpresaVendaRepository.Save(pontuacaoEmpresaVenda);

                    _consolidadoPontuacaoFidelidadeRepository.Save(consolidado);
                }

                return;
            }

            //Define a tipologia da pontuação
            consolidado = DefinirTipologia(consolidado, proposta);

            if (proposta.PreProposta.HasValue())
            {
                consolidado.IdPreProposta = proposta.PreProposta.Id;
            }

            consolidado.Faturado = proposta.Faturado;
                        
            if (proposta.Faturado)
            {
                consolidado.SituacaoPontuacao = SituacaoPontuacao.Disponivel;
            }
            else
            {
                consolidado.SituacaoPontuacao = SituacaoPontuacao.Indisponivel;
            }
            
            #region valor nominal
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
            #endregion

            if(itens.First().PontuacaoFidelidade.TipoPontuacaoFidelidade == TipoPontuacaoFidelidade.Normal)
            {
                consolidado = ContablzarPontuacaoPadrao(consolidado, proposta, itens.First(), valorNominal);
            }
            else
            {
                switch (itens.First().PontuacaoFidelidade.TipoCampanhaFidelidade)
                {
                    case TipoCampanhaFidelidade.PorVenda:
                        consolidado = ContabilizarCampanhaPorVenda(consolidado, proposta, itens.First(), valorNominal);
                        break;

                    case TipoCampanhaFidelidade.PorVendaMinima:
                        consolidado = ContabilizarCampanhaPorVendaMinima(consolidado, proposta, itens, valorNominal);
                        break;
                    case TipoCampanhaFidelidade.PorVendaMinimaEmpreendimento:
                        consolidado = ContabilizarCampanhaPorVendaMinimaEmpreendimento(consolidado, proposta, itens, valorNominal);
                        break;
                }
            }

            #region atualizando valores da EV

            pontuacaoEmpresaVenda = _pontuacaoFidelidadeEmpresaVendaRepository.FindByIdEmpresaVenda(empresaVenda.Id);

            if (pontuacaoEmpresaVenda.IsEmpty())
            {
                pontuacaoEmpresaVenda = new PontuacaoFidelidadeEmpresaVenda();
                pontuacaoEmpresaVenda.EmpresaVenda = new Tenda.Domain.EmpresaVenda.Models.EmpresaVenda { Id = empresaVenda.Id };                
            }

            pontuacaoEmpresaVenda.PontuacaoTotal += consolidado.Pontuacao;

            if (consolidado.SituacaoPontuacao == SituacaoPontuacao.Disponivel)
            {
                pontuacaoEmpresaVenda.PontuacaoDisponivel += consolidado.Pontuacao;
            }
            else
            {
                pontuacaoEmpresaVenda.PontuacaoIndisponivel += consolidado.Pontuacao;
            }

            _pontuacaoFidelidadeEmpresaVendaRepository.Save(pontuacaoEmpresaVenda);
            #endregion

            consolidado.DataPontuacao = proposta.DataKitCompleto.Value;
            
            _consolidadoPontuacaoFidelidadeRepository.Save(consolidado);
        }

        public ConsolidadoPontuacaoFidelidade DefinirTipologia(ConsolidadoPontuacaoFidelidade consolidado,PropostaSuat proposta)
        {
            if (proposta.Tipologia.ToUpper().Contains("PNE"))
            {
                consolidado.Tipologia = Tipologia.PNE;
            }
            else if (proposta.FaixaUmMeio)
            {
                consolidado.Tipologia = Tipologia.FaixaUmMeio;
            }
            else
            {
                consolidado.Tipologia = Tipologia.FaixaDois;
            }
            return consolidado;
        }
    
        public ConsolidadoPontuacaoFidelidade ContablzarPontuacaoPadrao(ConsolidadoPontuacaoFidelidade consolidado,PropostaSuat proposta,ItemPontuacaoFidelidade item,ValorNominal valorNominal)
        {
            var bre = new BusinessRuleException();
            if (item.Modalidade == TipoModalidadeProgramaFidelidade.Fixa)
            {
                switch (consolidado.Tipologia)
                {
                    case Tipologia.FaixaUmMeio:
                        consolidado.Pontuacao = item.PontuacaoFaixaUmMeio;
                        break;
                    case Tipologia.FaixaDois:
                        consolidado.Pontuacao = item.PontuacaoFaixaDois;
                        break;
                    default:
                        if (proposta.FaixaUmMeio)
                        {
                            consolidado.Pontuacao = item.PontuacaoFaixaUmMeio;
                        }
                        else
                        {
                            consolidado.Pontuacao = item.PontuacaoFaixaDois;
                        }
                        break;
                }                
            }
            else
            {
                if (valorNominal.IsEmpty())
                {
                    bre.AddError(string.Format("Não há tabela de valor nominal válida para o Empreendimento {0}", item.Empreendimento.Nome)).Complete();
                    bre.ThrowIfHasError();
                }

                consolidado.IdValorNominal = valorNominal.Id;

                switch (consolidado.Tipologia)
                {
                    case Tipologia.FaixaUmMeio:
                        if (proposta.ValorVGV < valorNominal.FaixaUmMeioDe)
                        {
                            consolidado.Pontuacao = item.PontuacaoFaixaUmMeioSeca;
                        }else if (proposta.ValorVGV > valorNominal.FaixaUmMeioAte)
                        {
                            consolidado.Pontuacao = item.PontuacaoFaixaUmMeioTurbinada;
                        }
                        else
                        {
                            consolidado.Pontuacao = item.PontuacaoFaixaUmMeioNormal;
                        }
                        break;
                    case Tipologia.FaixaDois:
                        if (proposta.ValorVGV < valorNominal.FaixaDoisDe)
                        {
                            consolidado.Pontuacao = item.PontuacaoFaixaDoisSeca;
                        }
                        else if (proposta.ValorVGV > valorNominal.FaixaDoisAte)
                        {
                            consolidado.Pontuacao = item.PontuacaoFaixaDoisTurbinada;
                        }
                        else
                        {
                            consolidado.Pontuacao = item.PontuacaoFaixaDoisNormal;
                        }
                        break;
                    case Tipologia.PNE:
                        if (proposta.ValorVGV < valorNominal.PNEDe)
                        {
                            consolidado.Pontuacao = item.PontuacaoPNESeca;
                        }
                        else if (proposta.ValorVGV > valorNominal.PNEAte)
                        {
                            consolidado.Pontuacao = item.PontuacaoPNETurbinada;
                        }
                        else
                        {
                            consolidado.Pontuacao = item.PontuacaoPNENormal;
                        }
                        break;
                    default:
                        bre.AddError("Tipologia inválida").Complete();
                        bre.ThrowIfHasError();
                        break;
                }
            }
            
            consolidado.IdItemPontuacaoFidelidade = item.Id;
            consolidado.IdPontuacaoFidelidade = item.PontuacaoFidelidade.Id;

            return consolidado;
        }
        public ConsolidadoPontuacaoFidelidade ContabilizarCampanhaPorVenda(ConsolidadoPontuacaoFidelidade consolidado,PropostaSuat proposta,ItemPontuacaoFidelidade item,ValorNominal valorNominal)
        {
            var bre = new BusinessRuleException();

            if (item.Modalidade == TipoModalidadeProgramaFidelidade.Fixa)
            {
                switch (consolidado.Tipologia)
                {
                    case Tipologia.FaixaUmMeio:
                        consolidado.Pontuacao = item.FatorUmMeio * item.PontuacaoPadraoUmMeio;
                        break;
                    case Tipologia.FaixaDois:
                        consolidado.Pontuacao = item.FatorDois * item.PontuacaoPadraoDois;
                        break;
                    default:
                        if (proposta.FaixaUmMeio)
                        {
                            consolidado.Pontuacao = item.PontuacaoPadraoUmMeio;
                        }
                        else
                        {
                            consolidado.Pontuacao = item.PontuacaoPadraoDois;
                        }
                        break;
                }                
            }
            else
            {
                if (valorNominal.IsEmpty())
                {
                    bre.AddError(string.Format("Não há tabela de valor nominal válida para o Empreendimento {0}", item.Empreendimento.Nome)).Complete();
                    bre.ThrowIfHasError();
                }

                consolidado.IdValorNominal = valorNominal.Id;

                switch (consolidado.Tipologia)
                {
                    case Tipologia.PNE:
                        if (proposta.ValorVGV < valorNominal.PNEDe)
                        {
                            consolidado.Pontuacao = item.FatorPNESeca * item.PontuacaoPNESeca;
                        }else if (proposta.ValorVGV > valorNominal.PNEAte)
                        {
                            consolidado.Pontuacao = item.FatorPNETurbinada * item.PontuacaoPNETurbinada;
                        }
                        else
                        {
                            consolidado.Pontuacao = item.FatorPNENormal * item.PontuacaoPNENormal;
                        }
                        break;
                    case Tipologia.FaixaUmMeio:
                        if (proposta.ValorVGV < valorNominal.FaixaUmMeioDe)
                        {
                            consolidado.Pontuacao = item.FatorUmMeioSeca * item.PontuacaoFaixaUmMeioSeca;
                        }
                        else if (proposta.ValorVGV > valorNominal.FaixaUmMeioAte)
                        {
                            consolidado.Pontuacao = item.FatorUmMeioTurbinada * item.PontuacaoFaixaUmMeioTurbinada;
                        }
                        else
                        {
                            consolidado.Pontuacao = item.FatorUmMeioNormal * item.PontuacaoFaixaUmMeioNormal;
                        }
                        break;
                    case Tipologia.FaixaDois:
                        if (proposta.ValorVGV < valorNominal.FaixaDoisDe)
                        {
                            consolidado.Pontuacao = item.FatorDoisSeca * item.PontuacaoFaixaDoisSeca;
                        }
                        else if (proposta.ValorVGV > valorNominal.FaixaDoisAte)
                        {
                            consolidado.Pontuacao = item.FatorDoisTurbinada * item.PontuacaoFaixaDoisTurbinada;
                        }
                        else
                        {
                            consolidado.Pontuacao = item.FatorDoisNormal * item.PontuacaoFaixaDoisNormal;
                        }
                        break;
                    default:
                        bre.AddError("Tipologia inválida").Complete();
                        bre.ThrowIfHasError();
                        break;
                }
            }

            consolidado.IdItemPontuacaoFidelidade = item.Id;
            consolidado.IdPontuacaoFidelidade = item.PontuacaoFidelidade.Id;

            return consolidado;
        }
        public ConsolidadoPontuacaoFidelidade ContabilizarCampanhaPorVendaMinima(ConsolidadoPontuacaoFidelidade consolidado, PropostaSuat proposta, List<ItemPontuacaoFidelidade> itens, ValorNominal valorNominal)
        {
            var bre = new BusinessRuleException();

            var divisao = _itemPontuacaoFidelidadeRepository.BuscarItens(itens.First().PontuacaoFidelidade.Id)
                .Select(x => x.Empreendimento.Divisao).ToList();

            var nPros = _propostaSuatRepository
                .NumeroDePropostaKitCompletoNoPeriodoPorEmpreendimento(itens.First().InicioVigencia.Value,
                itens.First().TerminoVigencia,
                divisao,
                proposta.IdSapLoja);

            var item = itens.Where(x => nPros >= x.QuantidadeMinima)
                .OrderByDescending(x => x.QuantidadeMinima).FirstOrDefault();

            if (item.HasValue())
            {
                if (item.Modalidade == TipoModalidadeProgramaFidelidade.Fixa)
                {
                    switch (consolidado.Tipologia)
                    {
                        case Tipologia.FaixaUmMeio:
                            consolidado.Pontuacao = item.PontuacaoPadraoUmMeio * item.FatorUmMeio;
                            break;
                        case Tipologia.FaixaDois:
                            consolidado.Pontuacao = item.PontuacaoPadraoDois * item.FatorDois;
                            break;
                        default:
                            if (proposta.FaixaUmMeio)
                            {
                                consolidado.Pontuacao = item.PontuacaoPadraoUmMeio * item.FatorUmMeio;
                            }
                            else
                            {
                                consolidado.Pontuacao = item.PontuacaoPadraoDois * item.FatorDois;
                            }
                            break;
                    }
                }
                else
                {
                    if (valorNominal.IsEmpty())
                    {
                        bre.AddError(string.Format("Não há tabela de valor nominal válida para o Empreendimento {0}", item.Empreendimento.Nome)).Complete();
                        bre.ThrowIfHasError();
                    }

                    consolidado.IdValorNominal = valorNominal.Id;

                    switch (consolidado.Tipologia)
                    {
                        case Tipologia.FaixaUmMeio:
                            if (proposta.ValorVGV < valorNominal.FaixaUmMeioDe)
                            {
                                consolidado.Pontuacao = item.PontuacaoFaixaUmMeioSeca*item.FatorUmMeioSeca;
                            }
                            else if (proposta.ValorVGV > valorNominal.FaixaUmMeioAte)
                            {
                                consolidado.Pontuacao = item.PontuacaoFaixaUmMeioTurbinada*item.FatorUmMeioTurbinada;
                            }
                            else
                            {
                                consolidado.Pontuacao = item.PontuacaoFaixaUmMeioNormal*item.FatorUmMeioNormal;
                            }
                            break;
                        case Tipologia.FaixaDois:
                            if (proposta.ValorVGV < valorNominal.FaixaDoisDe)
                            {
                                consolidado.Pontuacao = item.PontuacaoFaixaDoisSeca*item.FatorDoisSeca;
                            }
                            else if (proposta.ValorVGV > valorNominal.FaixaDoisAte)
                            {
                                consolidado.Pontuacao = item.PontuacaoFaixaDoisTurbinada*item.FatorDoisTurbinada;
                            }
                            else
                            {
                                consolidado.Pontuacao = item.PontuacaoFaixaDoisNormal*item.FatorDoisNormal;
                            }
                            break;
                        case Tipologia.PNE:
                            if (proposta.ValorVGV < valorNominal.PNEDe)
                            {
                                consolidado.Pontuacao = item.PontuacaoPNESeca*item.FatorPNESeca;
                            }
                            else if (proposta.ValorVGV > valorNominal.PNEAte)
                            {
                                consolidado.Pontuacao = item.PontuacaoPNETurbinada*item.FatorPNETurbinada;
                            }
                            else
                            {
                                consolidado.Pontuacao = item.PontuacaoPNENormal*item.FatorPNENormal;
                            }
                            break;
                        default:
                            bre.AddError("Tipologia inválida").Complete();
                            bre.ThrowIfHasError();
                            break;
                    }
                }

                consolidado.IdItemPontuacaoFidelidade = item.Id;
                consolidado.IdPontuacaoFidelidade = item.PontuacaoFidelidade.Id; 
                
                return consolidado;
            }

            item = itens.First();
            if (item.Modalidade == TipoModalidadeProgramaFidelidade.Fixa)
            {
                switch (consolidado.Tipologia)
                {
                    case Tipologia.FaixaUmMeio:
                        consolidado.Pontuacao = item.PontuacaoPadraoUmMeio;
                        break;
                    case Tipologia.FaixaDois:
                        consolidado.Pontuacao = item.PontuacaoPadraoDois;
                        break;
                    default:
                        if (proposta.FaixaUmMeio)
                        {
                            consolidado.Pontuacao = item.PontuacaoPadraoUmMeio;
                        }
                        else
                        {
                            consolidado.Pontuacao = item.PontuacaoPadraoDois;
                        }
                        break;
                }
            }
            else
            {
                if (valorNominal.IsEmpty())
                {
                    bre.AddError(string.Format("Não há tabela de valor nominal válida para o Empreendimento {0}", item.Empreendimento.Nome)).Complete();
                    bre.ThrowIfHasError();
                }

                consolidado.IdValorNominal = valorNominal.Id;

                switch (consolidado.Tipologia)
                {
                    case Tipologia.FaixaUmMeio:
                        if (proposta.ValorVGV < valorNominal.FaixaUmMeioDe)
                        {
                            consolidado.Pontuacao = item.PontuacaoFaixaUmMeioSeca;
                        }
                        else if (proposta.ValorVGV > valorNominal.FaixaUmMeioAte)
                        {
                            consolidado.Pontuacao = item.PontuacaoFaixaUmMeioTurbinada;
                        }
                        else
                        {
                            consolidado.Pontuacao = item.PontuacaoFaixaUmMeioNormal;
                        }
                        break;
                    case Tipologia.FaixaDois:
                        if (proposta.ValorVGV < valorNominal.FaixaDoisDe)
                        {
                            consolidado.Pontuacao = item.PontuacaoFaixaDoisSeca;
                        }
                        else if (proposta.ValorVGV > valorNominal.FaixaDoisAte)
                        {
                            consolidado.Pontuacao = item.PontuacaoFaixaDoisTurbinada;
                        }
                        else
                        {
                            consolidado.Pontuacao = item.PontuacaoFaixaDoisNormal;
                        }
                        break;
                    case Tipologia.PNE:
                        if (proposta.ValorVGV < valorNominal.PNEDe)
                        {
                            consolidado.Pontuacao = item.PontuacaoPNESeca;
                        }
                        else if (proposta.ValorVGV > valorNominal.PNEAte)
                        {
                            consolidado.Pontuacao = item.PontuacaoPNETurbinada;
                        }
                        else
                        {
                            consolidado.Pontuacao = item.PontuacaoPNENormal;
                        }
                        break;
                    default:
                        bre.AddError("Tipologia inválida").Complete();
                        bre.ThrowIfHasError();
                        break;
                }
            }

            consolidado.IdItemPontuacaoFidelidade = item.Id;
            consolidado.IdPontuacaoFidelidade = item.PontuacaoFidelidade.Id;

            return consolidado;
        } 
        public ConsolidadoPontuacaoFidelidade ContabilizarCampanhaPorVendaMinimaEmpreendimento(ConsolidadoPontuacaoFidelidade consolidado,PropostaSuat proposta,List<ItemPontuacaoFidelidade> itens,ValorNominal valorNominal)
        {
            var bre = new BusinessRuleException();
            var divisao = new List<string> { proposta.DivisaoEmpreendimento };

            var nPros = _propostaSuatRepository
                .NumeroDePropostaKitCompletoNoPeriodoPorEmpreendimento(itens.First().InicioVigencia.Value,
                itens.First().TerminoVigencia,
                divisao,
                proposta.IdSapLoja);

            var item = itens.Where(x => nPros >= x.QuantidadeMinima)
                .OrderByDescending(x => x.QuantidadeMinima).FirstOrDefault();

            if (item.HasValue())
            {
                if (item.Modalidade == TipoModalidadeProgramaFidelidade.Fixa)
                {
                    switch (consolidado.Tipologia)
                    {
                        case Tipologia.FaixaUmMeio:
                            consolidado.Pontuacao = item.PontuacaoPadraoUmMeio * item.FatorUmMeio;
                            break;
                        case Tipologia.FaixaDois:
                            consolidado.Pontuacao = item.PontuacaoPadraoDois * item.FatorDois;
                            break;
                        default:
                            if (proposta.FaixaUmMeio)
                            {
                                consolidado.Pontuacao = item.PontuacaoPadraoUmMeio * item.FatorUmMeio;
                            }
                            else
                            {
                                consolidado.Pontuacao = item.PontuacaoPadraoDois * item.FatorDois;
                            }
                            break;
                    }
                }
                else
                {
                    if (valorNominal.IsEmpty())
                    {
                        bre.AddError(string.Format("Não há tabela de valor nominal válida para o Empreendimento {0}", item.Empreendimento.Nome)).Complete();
                        bre.ThrowIfHasError();
                    }

                    consolidado.IdValorNominal = valorNominal.Id;

                    switch (consolidado.Tipologia)
                    {
                        case Tipologia.FaixaUmMeio:
                            if (proposta.ValorVGV < valorNominal.FaixaUmMeioDe)
                            {
                                consolidado.Pontuacao = item.PontuacaoFaixaUmMeioSeca * item.FatorUmMeioSeca;
                            }
                            else if (proposta.ValorVGV > valorNominal.FaixaUmMeioAte)
                            {
                                consolidado.Pontuacao = item.PontuacaoFaixaUmMeioTurbinada * item.FatorUmMeioTurbinada;
                            }
                            else
                            {
                                consolidado.Pontuacao = item.PontuacaoFaixaUmMeioNormal * item.FatorUmMeioNormal;
                            }
                            break;
                        case Tipologia.FaixaDois:
                            if (proposta.ValorVGV < valorNominal.FaixaDoisDe)
                            {
                                consolidado.Pontuacao = item.PontuacaoFaixaDoisSeca * item.FatorDoisSeca;
                            }
                            else if (proposta.ValorVGV > valorNominal.FaixaDoisAte)
                            {
                                consolidado.Pontuacao = item.PontuacaoFaixaDoisTurbinada * item.FatorDoisTurbinada;
                            }
                            else
                            {
                                consolidado.Pontuacao = item.PontuacaoFaixaDoisNormal * item.FatorDoisNormal;
                            }
                            break;
                        case Tipologia.PNE:
                            if (proposta.ValorVGV < valorNominal.PNEDe)
                            {
                                consolidado.Pontuacao = item.PontuacaoPNESeca * item.FatorPNESeca;
                            }
                            else if (proposta.ValorVGV > valorNominal.PNEAte)
                            {
                                consolidado.Pontuacao = item.PontuacaoPNETurbinada * item.FatorPNETurbinada;
                            }
                            else
                            {
                                consolidado.Pontuacao = item.PontuacaoPNENormal * item.FatorPNENormal;
                            }
                            break;
                        default:
                            bre.AddError("Tipologia inválida").Complete();
                            bre.ThrowIfHasError();
                            break;
                    }
                }

                consolidado.IdItemPontuacaoFidelidade = item.Id;
                consolidado.IdPontuacaoFidelidade = item.PontuacaoFidelidade.Id;

                return consolidado;
            }

            item = itens.First();
            if (item.Modalidade == TipoModalidadeProgramaFidelidade.Fixa)
            {
                switch (consolidado.Tipologia)
                {
                    case Tipologia.FaixaUmMeio:
                        consolidado.Pontuacao = item.PontuacaoPadraoUmMeio;
                        break;
                    case Tipologia.FaixaDois:
                        consolidado.Pontuacao = item.PontuacaoPadraoDois;
                        break;
                    default:
                        if (proposta.FaixaUmMeio)
                        {
                            consolidado.Pontuacao = item.PontuacaoPadraoUmMeio;
                        }
                        else
                        {
                            consolidado.Pontuacao = item.PontuacaoPadraoDois;
                        }
                        break;
                }
            }
            else
            {
                if (valorNominal.IsEmpty())
                {
                    bre.AddError(string.Format("Não há tabela de valor nominal válida para o Empreendimento {0}", item.Empreendimento.Nome)).Complete();
                    bre.ThrowIfHasError();
                }

                consolidado.IdValorNominal = valorNominal.Id;

                switch (consolidado.Tipologia)
                {
                    case Tipologia.FaixaUmMeio:
                        if (proposta.ValorVGV < valorNominal.FaixaUmMeioDe)
                        {
                            consolidado.Pontuacao = item.PontuacaoFaixaUmMeioSeca;
                        }
                        else if (proposta.ValorVGV > valorNominal.FaixaUmMeioAte)
                        {
                            consolidado.Pontuacao = item.PontuacaoFaixaUmMeioTurbinada;
                        }
                        else
                        {
                            consolidado.Pontuacao = item.PontuacaoFaixaUmMeioNormal;
                        }
                        break;
                    case Tipologia.FaixaDois:
                        if (proposta.ValorVGV < valorNominal.FaixaDoisDe)
                        {
                            consolidado.Pontuacao = item.PontuacaoFaixaDoisSeca;
                        }
                        else if (proposta.ValorVGV > valorNominal.FaixaDoisAte)
                        {
                            consolidado.Pontuacao = item.PontuacaoFaixaDoisTurbinada;
                        }
                        else
                        {
                            consolidado.Pontuacao = item.PontuacaoFaixaDoisNormal;
                        }
                        break;
                    case Tipologia.PNE:
                        if (proposta.ValorVGV < valorNominal.PNEDe)
                        {
                            consolidado.Pontuacao = item.PontuacaoPNESeca;
                        }
                        else if (proposta.ValorVGV > valorNominal.PNEAte)
                        {
                            consolidado.Pontuacao = item.PontuacaoPNETurbinada;
                        }
                        else
                        {
                            consolidado.Pontuacao = item.PontuacaoPNENormal;
                        }
                        break;
                    default:
                        bre.AddError("Tipologia inválida").Complete();
                        bre.ThrowIfHasError();
                        break;
                }
            }

            consolidado.IdItemPontuacaoFidelidade = item.Id;
            consolidado.IdPontuacaoFidelidade = item.PontuacaoFidelidade.Id;

            return consolidado;
        }
    }
}
