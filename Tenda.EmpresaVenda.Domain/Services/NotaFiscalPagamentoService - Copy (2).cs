using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Tenda.Domain.Core.Models;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.ApiService.Models.Financeiro;
using Tenda.EmpresaVenda.Domain.Integration.Zeus.Midas.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Repository.Models;
using Tenda.EmpresaVenda.Domain.Services.Models;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class NotaFiscalPagamentoService : BaseService
    {
        public NotaFiscalPagamentoRepository _notaFiscalPagamentoRepository { get; set; }
        public ViewNotaFiscalPagamentoRepository _viewNotaFiscalPagamentoRepository { get; set; }
        private NotaFiscalPagamentoPdfService _notaFiscalPagamentoPdfService { get; set; }
        private EmpresaVendaRepository _empresaVendaRepository { get; set; }
        private EmpreendimentoRepository _empreendimentoRepository { get; set; }
        private EnderecoFornecedorRepository _enderecoFornecdorRepository { get; set; }
        private EnderecoEstabelecimentoRepository _enderecoEstabelecimentoRepository { get; set; }
        private ValorNominalRepository _valorNominalRepository { get; set; }
        private PagamentoRepository _pagamentoRepository { get; set; }
        public NotaFiscalPagamentoOcorrenciaRepository _notaFiscalPagamentoOcorrenciaRepository { get; set; }

        public NotaFiscalPagamento SalvarPorFinanceiroPortal(NotaFiscalPagamento notaFiscalPagamento, string notaFiscal, Arquivo arquivo)
        {
            notaFiscalPagamento.DataEnviado = DateTime.Now;
            if(notaFiscalPagamento.Situacao != SituacaoNotaFiscal.AguardandoAvaliacao)
            {
                notaFiscalPagamento.Situacao = SituacaoNotaFiscal.AguardandoProcessamento;
            }
            notaFiscalPagamento.RevisaoNF += 1;
            notaFiscalPagamento.NotaFiscal = notaFiscal.TrimStart('0');
            notaFiscalPagamento.Arquivo = arquivo;

            _notaFiscalPagamentoRepository.Save(notaFiscalPagamento);

            return notaFiscalPagamento;
        }

        public byte[] Exportar(DataSourceRequest request, FiltroPagamentoDTO filtro)
        {
            var results = _viewNotaFiscalPagamentoRepository.ListarComGrupo(filtro);

            ExcelUtil excel = ExcelUtil.NewInstance(20)
                .NewSheet(DateTime.Now.ToString("yyyyMMddHHmmss"))
                .WithHeader(GetHeader());

            foreach (var grupo in results.ToDataRequest(request).records.ToList())
            {
                foreach (var model in grupo.Filhos)
                {
                    excel
                   .CreateCellValue(model.CodigoProposta).Width(20)
                   .CreateCellValue(RegraPagamento(model.RegraPagamento, model.TipoPagamento)).Width(30)
                   .CreateCellValue(model.PedidoSap).Width(30)
                   .CreateCellValue(model.NomeCliente).Width(30)
                   .CreateCellValue(model.NotaFiscal).Width(30)
                   .CreateCellValue(model.RevisaoNF.ToString()).Width(30)
                   .CreateCellValue(RenderSituacaoNotaFiscal(model.PassoAtual,model.SituacaoNotaFiscal,model.EmReversao)).Width(30)
                   .CreateCellValue(model.Motivo).Width(60)
                   .CreateDateTimeCell(model.DataPrevisaoPagamento).Width(30)
                   .CreateCellValue(model.Pago ? GlobalMessages.Sim : GlobalMessages.Nao).Width(20)
                   ;
                }


            }
            excel.Close();
            return excel.DownloadFile();
        }

        private string[] GetHeader()
        {
            IList<string> header = new List<string>
            {
                GlobalMessages.Proposta,
                GlobalMessages.ParcelaPagamento,
                GlobalMessages.NumeroPedido,
                GlobalMessages.Cliente,
                GlobalMessages.NotaFiscal,
                GlobalMessages.RevisaoNF,
                GlobalMessages.Situacao,
                GlobalMessages.MotivoRecusa,
                GlobalMessages.PrevisaoPagamento,
                GlobalMessages.Pago
            };
            return header.ToArray();
        }

        private string RegraPagamento(string regraPagamento, TipoPagamento tipo)
        {
            switch (tipo)
            {
                case TipoPagamento.KitCompleto:
                    return regraPagamento + "% Kit Completo";
                    break;
                case TipoPagamento.Repasse:
                    return regraPagamento + "% Repasse";
                    break;
                case TipoPagamento.Conformidade:
                    return regraPagamento + "% Conformidade";
                    break;
                default:
                    break;
            }
            return "";
        }

        public void MudarSituacao(NotaFiscalPagamento notaFiscalPagamento, SituacaoNotaFiscal novaSituacao)
        {
            GenericFileLogUtil.DevLogWithDateOnBegin(string.Format("Nota fiscal: {0} | {1} | {2}", notaFiscalPagamento.Id, notaFiscalPagamento.NotaFiscal, notaFiscalPagamento.Situacao.AsString()));

            notaFiscalPagamento.Situacao = novaSituacao;
            switch (novaSituacao)
            {
                case SituacaoNotaFiscal.AguardandoProcessamento:
                    notaFiscalPagamento.DataRecebido = DateTime.Now;
                    break;
                case SituacaoNotaFiscal.Aprovado:
                    notaFiscalPagamento.DataAprovado = DateTime.Now;
                    break;
                case SituacaoNotaFiscal.Reprovado:
                    notaFiscalPagamento.DataReprovado = DateTime.Now;
                    break;
            }

            _notaFiscalPagamentoRepository.Save(notaFiscalPagamento);

            GenericFileLogUtil.DevLogWithDateOnBegin(string.Format("Nota fiscal: {0} | {1} | {2}", notaFiscalPagamento.Id, notaFiscalPagamento.NotaFiscal, notaFiscalPagamento.Situacao.AsString()));

        }


        public byte[] ExportarTodosDocumentos(List<long> idsNotaFiscaisPagamento, bool permissaoReceber)
        {
            var notaFiscalPagamentos = _notaFiscalPagamentoRepository.BuscarNotasFicaisPagamentosPorId(idsNotaFiscaisPagamento);

            var outputMemStream = new MemoryStream();
            var zipOutputStream = new ZipOutputStream(outputMemStream);
            zipOutputStream.SetLevel(3); //0-9, 9 being the highest level of compression

            foreach (var notaFiscal in notaFiscalPagamentos)
            {
                GenericFileLogUtil.DevLogWithDateOnBegin(string.Format("Alterar Situação da nota fiscal: {0}", notaFiscal.NotaFiscal));
                GenericFileLogUtil.DevLogWithDateOnBegin(string.Format("Nota fiscal: {0} | {1} | {2}", notaFiscal.Id, notaFiscal.NotaFiscal, notaFiscal.Situacao.AsString()));

                //if (notaFiscal.Situacao == SituacaoNotaFiscal.AguardandoProcessamento && permissaoReceber)
                //{
                //    MudarSituacao(notaFiscal, SituacaoNotaFiscal.AguardandoAvaliacao);
                //}

                var memoryStream = new MemoryStream(notaFiscal.Arquivo.Content);
                var entry = new ZipEntry(notaFiscal.Arquivo.Nome);
                entry.IsUnicodeText = true;
                entry.DateTime = DateTime.Now;
                zipOutputStream.PutNextEntry(entry);
                StreamUtils.Copy(memoryStream, zipOutputStream, new byte[4096]);
                zipOutputStream.CloseEntry();

            }
            zipOutputStream.IsStreamOwner = false;
            zipOutputStream.Close();

            outputMemStream.Position = 0;

            return outputMemStream.ToArray();
        }

        public Arquivo GerarPdf(NotaFiscalPagamentoPdfDTO dto, byte[] logo)
        {
            var files = _notaFiscalPagamentoPdfService.CriarPdf(dto, logo);
            var file = files[0];

            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            var fileName = $"DadosNotaFiscal{date}.pdf";

            Arquivo arquivo = new Arquivo();
            arquivo.Nome = fileName;
            arquivo.ContentType = "application/pdf";
            arquivo.Content = file;

            return arquivo;
        }

        public byte[] ExportarRecebimentoNotaFiscal(DataSourceRequest request, FiltroPagamentoDTO filtro)
        {
            var results = _viewNotaFiscalPagamentoRepository.ListarComGrupo(filtro);

            IList<string> header = new List<string>
            {
                GlobalMessages.EmpresaVenda,
                GlobalMessages.Proposta,
                GlobalMessages.ParcelaPagamento,
                GlobalMessages.DataComissao,
                GlobalMessages.UF,
                GlobalMessages.Regional,
                GlobalMessages.NumeroPedido,
                GlobalMessages.NumeroNotaFiscalP,
                GlobalMessages.RevisaoNF,
                GlobalMessages.Situacao,
                GlobalMessages.MotivoRecusa,
                GlobalMessages.PrevisaoPagamento,
                GlobalMessages.Pago
            };

            ExcelUtil excel = ExcelUtil.NewInstance(20)
                .NewSheet(DateTime.Now.ToString("yyyyMMddHHmmss"))
                .WithHeader(header.ToArray());

            foreach (var grupo in results.ToDataRequest(request).records.ToList())
            {
                foreach (var model in grupo.Filhos)
                {
                    excel
                   .CreateCellValue(model.NomeFantasia).Width(20)
                   .CreateCellValue(model.CodigoProposta).Width(20)
                   .CreateCellValue(RegraPagamento(model.RegraPagamento, model.TipoPagamento)).Width(30)
                   .CreateCellValue(model.DataComissao.HasValue() ? model.DataComissao.Value.ToDate() : "").Width(20)
                   .CreateCellValue(model.Estado).Width(20)
                   .CreateCellValue(model.Regional).Width(20)
                   .CreateCellValue(model.PedidoSap).Width(30)
                   .CreateCellValue(model.NotaFiscal).Width(30)
                   .CreateCellValue(model.RevisaoNF.ToString()).Width(30)
                   .CreateCellValue(RenderSituacaoNotaFiscal(model.PassoAtual, model.SituacaoNotaFiscal, model.EmReversao)).Width(30)
                   .CreateCellValue(model.Motivo).Width(60)
                   .CreateDateTimeCell(model.DataPrevisaoPagamento).Width(30)
                   .CreateCellValue(model.Pago ? GlobalMessages.Sim : GlobalMessages.Nao).Width(20)
                   ;
                }


            }
            excel.Close();
            return excel.DownloadFile();
        }

        public NotaFiscalDTO ExibirNotaFiscal(NotaFiscalPagamentoPdfDTO filtro)
        {
            var notaFiscal = new NotaFiscalDTO();

            notaFiscal.NotasFiscais = _viewNotaFiscalPagamentoRepository.BuscarPagamentos(filtro.IdEmpresaVenda, filtro.PedidoSap);

            foreach(var nota in notaFiscal.NotasFiscais)
            {
                nota.SubTabela = SubTabela(nota);
            }

            var empresaVenda = _empresaVendaRepository.FindById(filtro.IdEmpresaVenda);

            notaFiscal.CnpjEmpresaVenda = empresaVenda.CNPJ.ToCNPJFormat();
            notaFiscal.RazaoSocialEmpresaVenda = empresaVenda.RazaoSocial;
            notaFiscal.LogradouroEmpresaVenda = empresaVenda.Logradouro;
            notaFiscal.NumeroEmpresaVenda = empresaVenda.Numero;
            notaFiscal.BairroEmpresaVenda = empresaVenda.Bairro;
            notaFiscal.CidadeEmpresaVenda = empresaVenda.Cidade;
            notaFiscal.CepEmpresaVenda = empresaVenda.Cep;
            notaFiscal.RegionalEmpresaVenda = empresaVenda.Estado;

            var empreendimento = _empreendimentoRepository.FindById(filtro.IdEmpreendimento);

            var endereco = _enderecoFornecdorRepository.Queryable()
                .Where(x => x.CodigoFornecedor.ToUpper().Equals(empreendimento.CodigoEmpresa.ToUpper()))
                .Where(x => x.Estado.Equals(empresaVenda.Estado))
                .SingleOrDefault();

            if (endereco.HasValue())
            {
                notaFiscal.CnpjEstabelecimento = endereco.Cnpj.ToCNPJFormat();
                notaFiscal.RazaoSocialEstabelecimento = endereco.RazaoSocial;
                notaFiscal.LogradouroEstabelecimento = endereco.Logradouro;
                notaFiscal.NumeroEstabelecimento = endereco.Numero;
                notaFiscal.BairroEstabelecimento = endereco.Bairro;
                notaFiscal.CidadeEstabelecimento = endereco.Cidade;
                notaFiscal.CepEstabelecimento = endereco.Cep.ToCEPFormat();
                notaFiscal.RegionalEstabelecimento = endereco.Estado;
            }

            if (endereco.IsEmpty())
            {
                var estabelecimento =_enderecoEstabelecimentoRepository.FindByEmpreendimento(empreendimento.Id);

                if (estabelecimento.HasValue())
                {
                    notaFiscal.CnpjEstabelecimento = empreendimento.CNPJ.ToCNPJFormat();
                    notaFiscal.RazaoSocialEstabelecimento = empreendimento.NomeEmpresa;
                    notaFiscal.LogradouroEstabelecimento = estabelecimento.Logradouro;
                    notaFiscal.NumeroEstabelecimento = estabelecimento.Numero;
                    notaFiscal.BairroEstabelecimento = estabelecimento.Bairro;
                    notaFiscal.CidadeEstabelecimento = estabelecimento.Cidade;
                    notaFiscal.CepEstabelecimento = estabelecimento.Cep.ToCEPFormat();
                    notaFiscal.RegionalEstabelecimento = estabelecimento.Estado;
                }
            }

            notaFiscal.PedidoSap = filtro.PedidoSap;
            notaFiscal.CodigoFornecedorSap = empreendimento.CodigoEmpresa;

            var total = notaFiscal.NotasFiscais.Sum(x => x.ValorAPagar);
            notaFiscal.Total = string.Format(new CultureInfo("pt-BR"), "{0:C}", total);

            return notaFiscal;
        }

        public TipoSubtabelaComissao SubTabela(ViewNotaFiscalPagamento item)
        {
            var subTabela = TipoSubtabelaComissao.Seca;

            var valorNominal = _valorNominalRepository.Queryable()
                .Where(x => x.Empreendimento.Id == item.IdEmpreendimento)
                .Where(x => x.InicioVigencia.Value.Date <= item.DataVenda.Value.Date)
                .Where(x => x.TerminoVigencia.Value.Date >= item.DataVenda.Value.Date)
                .FirstOrDefault();

            if (valorNominal.IsEmpty())
            {
                valorNominal = _valorNominalRepository.Queryable()
            .Where(x => x.Empreendimento.Id == item.IdEmpreendimento)
            .Where(x => x.InicioVigencia.Value.Date <= item.DataVenda.Value.Date)
            .Where(x => x.TerminoVigencia.Value.Date == null)
            .FirstOrDefault();
            }

            if (valorNominal.IsEmpty())
            {
                return subTabela;
            }

            switch (item.Tipologia)
            {
                case Tipologia.PNE:
                    if (item.ValorVgv < valorNominal.PNEDe)
                    {
                        return TipoSubtabelaComissao.Seca;
                    }
                    else if (item.ValorVgv > valorNominal.PNEAte)
                    {
                        return TipoSubtabelaComissao.Turbinada;
                    }
                    else
                    {
                        return TipoSubtabelaComissao.Normal;
                    }
                    break;
                case Tipologia.FaixaUmMeio:
                    if (item.ValorVgv < valorNominal.FaixaUmMeioDe)
                    {
                        return TipoSubtabelaComissao.Seca;
                    }
                    else if (item.ValorVgv > valorNominal.FaixaUmMeioAte)
                    {
                        return TipoSubtabelaComissao.Turbinada;
                    }
                    else
                    {
                        return TipoSubtabelaComissao.Normal;
                    }
                    break;
                case Tipologia.FaixaDois:
                    if (item.ValorVgv < valorNominal.FaixaDoisDe)
                    {
                        return TipoSubtabelaComissao.Seca;
                    }
                    else if (item.ValorVgv > valorNominal.FaixaDoisDe)
                    {
                        return TipoSubtabelaComissao.Turbinada;
                    }
                    else
                    {
                        return TipoSubtabelaComissao.Normal;
                    }
                    break;
            }

            return subTabela;
        }

        public NotaFiscalPagamento AtualizarNotaFiscal(NotaFiscalRequestDto notaFiscalRequestDto)
        {
            var bre = new BusinessRuleException();

            if (notaFiscalRequestDto.Chave.IsEmpty())
            {
                bre.AddError(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Chave)).Complete();
                bre.ThrowIfHasError();
            }

            var pagamento = _pagamentoRepository.Queryable()
                .Where(x => x.NotaFiscalPagamento != null)
                .Where(x => x.NotaFiscalPagamento.Chave.Equals(notaFiscalRequestDto.Chave))
                .SingleOrDefault();

            if (pagamento.IsEmpty())
            {
                bre.AddError(string.Format("Chave {0} inválida", notaFiscalRequestDto.Chave)).Complete();
                bre.ThrowIfHasError();
            }

            if (notaFiscalRequestDto.Aprovado)
            {

                pagamento.DataPrevisaoPagamento = notaFiscalRequestDto.DataPrevisaoPagamento;
                pagamento.MIRO = notaFiscalRequestDto.MIRO;
                pagamento.MIGO = notaFiscalRequestDto.MIGO;

                pagamento.NotaFiscalPagamento.Situacao = SituacaoNotaFiscal.PreAprovado;
                pagamento.NotaFiscalPagamento.DataAprovado = DateTime.Now;

                _pagamentoRepository.Save(pagamento);

                return pagamento.NotaFiscalPagamento;
            }

            if (notaFiscalRequestDto.MotivoRecusa.IsEmpty())
            {
                bre.AddError(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.MotivoRecusa)).Complete();
                bre.ThrowIfHasError();
            }

            pagamento.NotaFiscalPagamento.Situacao = SituacaoNotaFiscal.Reprovado;
            pagamento.NotaFiscalPagamento.RevisaoNF += 1;
            pagamento.NotaFiscalPagamento.Motivo = notaFiscalRequestDto.MotivoRecusa;
            pagamento.NotaFiscalPagamento.DataReprovado = DateTime.Now;

            _pagamentoRepository.Save(pagamento);

            return pagamento.NotaFiscalPagamento;
        }

        public string RenderSituacaoNotaFiscal(string passoAtual, SituacaoNotaFiscal situacao, bool emRevercao)
        {
            if (passoAtual == "Prop. Cancelada" || emRevercao == true)
            {
                return SituacaoNotaFiscal.Distratado.ToString();
            }
            if (situacao != SituacaoNotaFiscal.PendenteEnvio)
            {
                return situacao.ToString();
            }

            return SituacaoNotaFiscal.PendenteEnvio.ToString();
        }

        public void MudarSituacaoMidasAguardandoAvaliacao(ViewNotaFiscalPagamento notaFiscalPagamento)
        {
            var notaFiscal = _notaFiscalPagamentoRepository.FindById(notaFiscalPagamento.IdNotaFiscalPagamento.Value);
            notaFiscal.Situacao = SituacaoNotaFiscal.AguardandoAvaliacao;
            _notaFiscalPagamentoRepository.Save(notaFiscal);
            
        }
        public void MudarSituacaoMidasIntegracaoAprovada(string occurenceId)
        {
            long occurenceIdParse = long.Parse(occurenceId);
            var crz = _notaFiscalPagamentoOcorrenciaRepository.FindByOccurrenceId(occurenceIdParse);
            var notaFiscal =_notaFiscalPagamentoRepository.FindById(crz.NotaFiscalPagamento.Id);
            MudarSituacao(notaFiscal, SituacaoNotaFiscal.PreAprovado);
        }

        public NotaFiscalPagamento AtualizarNumeroNf(long idNf, string numero)
        {
            var nota = _notaFiscalPagamentoRepository.FindById(idNf);
            nota.NotaFiscal = numero.TrimStart('0');
            _notaFiscalPagamentoRepository.Save(nota);

            return nota;
        }
    }
}