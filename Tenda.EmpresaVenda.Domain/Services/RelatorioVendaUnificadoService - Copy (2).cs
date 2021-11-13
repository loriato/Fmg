using Europa.Extensions;
using Europa.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services.Models;

namespace Tenda.EmpresaVenda.Domain.Services
{

    public class RelatorioVendaUnificadoService:BaseService
    {
        private ViewRelatorioVendaUnificadoRepository _viewRelatorioVendaUnificadoRepository { get; set; }

        #region Relatorio Venda Unificado

        private string[] GetHeaderRelatorioVendaUnificada()
        {
            IList<string> header = new List<string>
            {
                GlobalMessages.Proposta,
                GlobalMessages.PreProposta,
                GlobalMessages.StatusPRO,
                GlobalMessages.Empresa,
                GlobalMessages.Divisao,
                GlobalMessages.Empreendimento,
                GlobalMessages.Regional,
                GlobalMessages.UF,
                GlobalMessages.CodigoFornecedor,
                GlobalMessages.Central,
                GlobalMessages.PontoVenda,
                GlobalMessages.Cliente,
                GlobalMessages.Viabilizador,
                GlobalMessages.Supervisor,
                GlobalMessages.Coordenador,
                GlobalMessages.DataVenda,
                GlobalMessages.DataProcessado+"*",
                GlobalMessages.Tipologia,
                GlobalMessages.CriterioPagamentoLinha,
                GlobalMessages.ComissaoAcordada,
                GlobalMessages.ValorSemPremiada,
                GlobalMessages.ComissaoPagar,
                GlobalMessages.StatusRepasse,
                GlobalMessages.DataRepasse,
                GlobalMessages.StatusConformidade,
                GlobalMessages.DataConformidade,
                GlobalMessages.Aptidao,
                GlobalMessages.SituacaoNotaFiscal,
                GlobalMessages.PrevisaoPagamento,
                GlobalMessages.Pago+"?",
                GlobalMessages.NumeroPedido,
                GlobalMessages.DataPedido,
                GlobalMessages.NumeroNotaFiscal,
                GlobalMessages.DataNotaFiscalEnviada,
                GlobalMessages.DataNotaFiscalRecebida,
                GlobalMessages.DataNotaFiscalAprovadaRecusada,
                GlobalMessages.DataFaturamento
            };
            return header.ToArray();
        }
        public string RenderFaixa(ViewRelatorioVendaUnificado model)
        {
            if (model.Tipologia.HasValue())
            {
                return model.Tipologia.AsString();
            }

            if (model.FlagFaixaUmMeio)
            {
                return Tipologia.FaixaUmMeio.AsString();
            }

            return Tipologia.FaixaDois.AsString();
        }
        public string RegraPagamento(string regraPagamento, TipoPagamento tipo)
        {
            switch (tipo)
            {
                case TipoPagamento.KitCompleto:
                    return regraPagamento + "% Kit Completo";

                case TipoPagamento.Repasse:
                    return regraPagamento + "% Repasse";

                case TipoPagamento.Conformidade:
                    return regraPagamento + "% Conformidade";

                default:
                    break;
            }
            return "";
        }
        public string RenderComissaoAPagar(ViewRelatorioVendaUnificado model)
        {
            if (model.Tipologia.HasValue())
            {
                switch (model.Tipologia)
                {
                    case Tipologia.PNE:
                        return model.ComissaoPagarPNE + "%";
                    case Tipologia.FaixaUmMeio:
                        return model.ComissaoPagarUmMeio + "%";
                    case Tipologia.FaixaDois:
                        return model.ComissaoPagarDois + "%";
                }
            }

            if (model.FlagFaixaUmMeio)
            {
                return model.ComissaoPagarUmMeio + "%";
            }
            return model.ComissaoPagarDois + "%";
        }
        public decimal RenderValorAPagar(ViewRelatorioVendaUnificado model)
        {
            switch (model.Tipologia)
            {
                case Tipologia.PNE:
                    return model.ValorVGV.Value * model.ComissaoPagarPNE.Value / 100;
                case Tipologia.FaixaUmMeio:
                    return model.ValorVGV.Value * model.ComissaoPagarUmMeio / 100;
                case Tipologia.FaixaDois:
                    return model.ValorVGV.Value * model.ComissaoPagarDois / 100;
            }
            return 0;
        }
        public string RenderAptidao(ViewRelatorioVendaUnificado model)
        {
            if (model.TipoPagamento == TipoPagamento.KitCompleto && model.Faturado && model.DataKitCompleto.HasValue())
            {
                return GlobalMessages.Sim;
            }

            if (model.TipoPagamento == TipoPagamento.Repasse && model.DataRepasse.HasValue())
            {
                return GlobalMessages.Sim;
            }

            if (model.TipoPagamento == TipoPagamento.Conformidade && model.DataConformidade.HasValue())
            {
                return GlobalMessages.Sim;
            }

            return GlobalMessages.Nao;
        }
        public string RenderDataNotaFiscalAprovadaRecusada(ViewRelatorioVendaUnificado model)
        {
            switch (model.SituacaoNotaFiscal)
            {
                case SituacaoNotaFiscal.Aprovado:
                    return model.DataNotaFiscalAprovada.HasValue()? model.DataNotaFiscalAprovada.Value.ToDate():"";
                case SituacaoNotaFiscal.Reprovado:
                    return model.DataNotaFiscalReprovada.HasValue()?model.DataNotaFiscalReprovada.Value.ToDate():"";
                default:
                    return "";
            }
        }
        public byte[] ExportarRelatorioVendaUnificado(DataSourceRequest request, RelatorioComissaoDTO filtro)
        {
            var vendas = _viewRelatorioVendaUnificadoRepository.ListarRelatorioVendaUnificado(request, filtro).records.ToList();

            ExcelUtil excel = ExcelUtil.NewInstance(23)
                .NewSheet(DateTime.Now.ToString("yyyyMMddHHmmss"))
                .WithHeader(GetHeaderRelatorioVendaUnificada());

            const string empty = "";

            foreach (var model in vendas)
            {
                excel
                    .CreateCellValue(model.CodigoProposta).Width(20)
                    .CreateCellValue(model.CodigoPreProposta).Width(20)
                    .CreateCellValue(model.PassoAtual).Width(20)
                    .CreateCellValue(model.CodigoEmpresa).Width(20)
                    .CreateCellValue(model.DivisaoEmpreendimento).Width(20)
                    .CreateCellValue(model.NomeEmpreendimento).Width(40)
                    .CreateCellValue(model.Regional).Width(20)
                    .CreateCellValue(model.Estado).Width(20)
                    .CreateCellValue(model.CodigoFornecedorSap).Width(20)
                    .CreateCellValue(model.CentralVenda).Width(40)
                    .CreateCellValue(model.NomePontoVenda).Width(40)
                    .CreateCellValue(model.NomeCliente).Width(40)
                    .CreateCellValue(model.NomeViabilizador).Width(40)
                    .CreateCellValue(model.NomeSupervisor).Width(40)
                    .CreateCellValue(model.NomeCoordenador).Width(40)
                    .CreateCellValue(model.DataVenda.HasValue ? model.DataVenda.Value.ToDate() : empty).Width(20)
                    .CreateCellValue(model.DataComissao.HasValue ? model.DataComissao.Value.ToDate() : empty).Width(20)
                    .CreateCellValue(RenderFaixa(model)).Width(20)
                    .CreateCellValue(RegraPagamento(model.RegraPagamento, model.TipoPagamento)).Width(20)
                    .CreateCellValue(RenderComissaoAPagar(model)).Width(20)
                    .CreateMoneyCell(model.ValorVGV).Width(30)
                    .CreateMoneyCell(RenderValorAPagar(model))
                    .CreateCellValue(model.StatusRepasse).Width(20)
                    .CreateCellValue(model.DataRepasse.HasValue ? model.DataRepasse.Value.ToDate() : empty).Width(25)
                    .CreateCellValue(model.EmConformidade == true ? "Sim" : "Não").Width(20)
                    .CreateCellValue(model.DataConformidade.HasValue ? model.DataConformidade.Value.ToDate() : empty).Width(25)
                    .CreateCellValue(RenderAptidao(model))
                    .CreateCellValue(model.SituacaoNotaFiscal.AsString())
                    .CreateCellValue(model.DataPrevisaoPagamento.HasValue ? model.DataPrevisaoPagamento.Value.ToDate() : empty).Width(25)
                    .CreateCellValue(model.Pago == true ? "Sim" : "Não").Width(20)
                    .CreateCellValue(model.PedidoSap).Width(20)
                    .CreateCellValue(model.DataPedidoSap.HasValue ? model.DataPedidoSap.Value.ToDate() : empty).Width(25)
                    .CreateCellValue(model.NotaFiscal).Width(20)
                    .CreateCellValue(model.DataNotaFiscalEnviada.HasValue ? model.DataNotaFiscalEnviada.Value.ToDate() : empty).Width(25)
                    .CreateCellValue(model.DataNotaFiscalRecebida.HasValue ? model.DataNotaFiscalRecebida.Value.ToDate() : empty).Width(25)
                    .CreateCellValue(RenderDataNotaFiscalAprovadaRecusada(model))
                    .CreateCellValue(model.DataFaturado.HasValue ? model.DataFaturado.Value.ToDate() : empty).Width(25);
            }

            excel.Close();
            return excel.DownloadFile();
        }

        #endregion
    }
}
