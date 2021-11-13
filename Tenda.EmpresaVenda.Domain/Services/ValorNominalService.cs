using Europa.Extensions;
using Europa.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.Core.Services;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Repository.Models;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class ValorNominalService : BaseService
    {
        private ViewValorNominalEmpreendimentoRepository _viewValorNominalEmpreendimentoRepository { get; set; }
        public byte[] Exportar(DataSourceRequest request, FiltroValorNominalEmpreendimentoDTO filtro)
        {
            var results = _viewValorNominalEmpreendimentoRepository.Listar(filtro).ToDataRequest(request);

            ExcelUtil excel = ExcelUtil.NewInstance(20)
                .NewSheet(DateTime.Now.ToString("yyyyMMddHHmmss"))
                .WithHeader(GetHeader());

            foreach (var model in results.records.ToList())
            {
                excel
                    .CreateCellValue(model.Divisao).Width(20)
                    .CreateCellValue(model.NomeEmpreendimento).Width(30)
                    .CreateCellValue(model.Estado).Width(20)
                    .CreateMoneyCell(model.FaixaUmMeioDe).Width(20)
                    .CreateMoneyCell(model.FaixaUmMeioAte).Width(20)
                    .CreateMoneyCell(model.FaixaDoisDe).Width(20)
                    .CreateMoneyCell(model.FaixaDoisAte).Width(20)
                    .CreateMoneyCell(model.PNEDe).Width(20)
                    .CreateMoneyCell(model.PNEAte).Width(20)
                    .CreateDateTimeCell(model.InicioVigencia, DateTimeExtensions.PatternDateTimeSeconds).Width(20)
                    .CreateDateTimeCell(model.TerminoVigencia, DateTimeExtensions.PatternDateTimeSeconds).Width(20)
                    .CreateCellValue(model.Situacao.AsString()).Width(20);
            }
            excel.Close();
            return excel.DownloadFile();
        }

        private string[] GetHeader()
        {
            IList<string> header = new List<string>
            {
                GlobalMessages.Divisao,
                GlobalMessages.Nome,
                GlobalMessages.Estado,
                GlobalMessages.FaixaUmMeioDe,
                GlobalMessages.FaixaUmMeioAte,
                GlobalMessages.FaixaDoisDe,
                GlobalMessages.FaixaDoisAte,
                GlobalMessages.PNEDe,
                GlobalMessages.PNEAte,
                GlobalMessages.InicioVigencia,
                GlobalMessages.TerminoVigencia,
                GlobalMessages.Situacao
            };
            return header.ToArray();
        }
    }
}
