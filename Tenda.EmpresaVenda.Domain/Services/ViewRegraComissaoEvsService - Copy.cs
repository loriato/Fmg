using Europa.Extensions;
using Europa.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.Core.Services;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services.Models;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class ViewRegraComissaoEvsService : BaseService
    {
        private ViewRegraComissaoEvsRepository _viewRegraComissaoEvsRepository { get; set; }
        public byte[] Exportar(DataSourceRequest request, FiltroRegraComissaoDTO filtro)
        {
            var resultados = _viewRegraComissaoEvsRepository.Listar(request, filtro).records.ToList();
            
            ExcelUtil excel = ExcelUtil.NewInstance(25)
                .NewSheet(DateTime.Now.ToString("yyyyMMddHHmmss"))
                .WithHeader(GetHeader());

            foreach (var model in resultados)
            {   
                excel
                    .CreateCellValue(model.Regional).Width(10)
                    .CreateCellValue(model.NomeEmpresaVenda).Width(50)
                    .CreateCellValue(model.Cnpj.ToCNPJFormat()).Width(20)
                    .CreateCellValue(model.Descricao).Width(50)
                    .CreateCellValue(model.InicioVigencia.IsEmpty()?"": model.InicioVigencia.Value.ToDate()).Width(20)
                    .CreateCellValue(model.TerminoVigencia.IsEmpty() ? "" : model.TerminoVigencia.Value.ToDate()).Width(20)
                    .CreateCellValue(model.DataAceite.IsEmpty() ? "" : model.DataAceite.Value.ToDate()).Width(20)
                    .CreateCellValue(model.Aprovador).Width(50)
                    .CreateCellValue(model.SituacaoEvs.AsString()).Width(30);
            }

            excel.Close();
            return excel.DownloadFile();
        }

        private string[] GetHeader()
        {
            IList<string> header = new List<string>
            {
                GlobalMessages.Regional,
                GlobalMessages.EmpresaVenda,
                GlobalMessages.Cnpj,
                GlobalMessages.Descricao,
                GlobalMessages.InicioVigencia,
                GlobalMessages.TerminoVigencia,
                GlobalMessages.DataAceite,
                GlobalMessages.Aprovador,
                GlobalMessages.Situacao
            };
            return header.ToArray();
        }
    }
}
