using Europa.Extensions;
using Europa.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Repository.Models;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class PropostaSuatService : BaseService
    {
        private PropostaSuatRepository _propostaSuatRepository { get; set; }
        private ViewPropostaFaturadaRepository _viewPropostaFaturadaRepository { get; set; }

        public void SolicitarAdiantamento(List<ViewRelatorioComissao> model)
        {
            foreach (var item in model)
            {
                var proposta = _propostaSuatRepository.FindById(item.IdProposta);
                if (item.TipoPagamento == TipoPagamento.Conformidade)
                {
                    proposta.AdiantamentoConformidade = StatusAdiantamentoPagamento.Solicitado;
                }
                else if (item.TipoPagamento == TipoPagamento.Repasse)
                {
                    proposta.AdiantamentoRepasse = StatusAdiantamentoPagamento.Solicitado;
                }
                _propostaSuatRepository.Save(proposta);
            }
        }

        public int AlterarStatus(List<ViewRelatorioComissao> model, StatusAdiantamentoPagamento status)
        {
            var num = 0;
            foreach (var item in model)
            {
                if (item.AdiantamentoPagamento == StatusAdiantamentoPagamento.Solicitado)

                {
                    num++;
                    var proposta = _propostaSuatRepository.FindById(item.IdProposta);

                    if (item.TipoPagamento == TipoPagamento.Conformidade)
                    {
                        proposta.AdiantamentoConformidade = status;
                        if (status == StatusAdiantamentoPagamento.Aprovado)
                        {
                            proposta.DataConformidade = DateTime.Now;
                        }

                    }
                    else if (item.TipoPagamento == TipoPagamento.Repasse)
                    {
                        proposta.AdiantamentoRepasse = status;
                        if (status == StatusAdiantamentoPagamento.Aprovado)
                        {
                            proposta.DataRepasse = DateTime.Now;
                        }
                    }
                    _propostaSuatRepository.Save(proposta);
                }

            }
            return num;

        }

        #region Proposta Faturada
        private string[] GetHeaderPropostaFaturada()
        {
            IList<string> header = new List<string>
            {
                GlobalMessages.UF,
                GlobalMessages.Proposta,
                GlobalMessages.Faturado,
                GlobalMessages.DataFaturamento
            };
            return header.ToArray();
        }

        public byte[] ExportarPropostaFaturada(DataSourceRequest request, FiltroPropostaDTO filtro)
        {
            var results = _viewPropostaFaturadaRepository.Listar(request,filtro);

            ExcelUtil excel = ExcelUtil.NewInstance(23)
                .NewSheet(DateTime.Now.ToString("yyyyMMddHHmmss"))
                .WithHeader(GetHeaderPropostaFaturada());

            foreach (var model in results.records.ToList())
            {
                excel
                    .CreateCellValue(model.Regional).Width(20)
                    .CreateCellValue(model.CodigoProposta).Width(20)
                    .CreateCellValue(model.Faturado ? GlobalMessages.Sim : GlobalMessages.Nao).Width(20)
                    .CreateDateTimeCell(model.DataFaturado).Width(20);
            }
            excel.Close();
            return excel.DownloadFile();
        }
        #endregion
    }
}
