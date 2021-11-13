using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Repository.Models;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Portal.Models;
using Tenda.EmpresaVenda.Portal.Models.Application;
using Tenda.EmpresaVenda.Portal.Security;

namespace Tenda.EmpresaVenda.Portal.Controllers
{
    [BaseAuthorize("EVS21")]
    public class TabelaValorNominalController : BaseController
    {
        private ViewValorNominalEmpreendimentoRepository _viewValorNominalEmpreendimentoRepository { get; set; }
        private EmpreendimentoRepository _empreendimentoRepository { get; set; }
        public ActionResult Index(long? id)
        {
            var dto = new TabelaValorNominalDTO();
            Empreendimento empreendimento = null;
            if (id.HasValue())
            {
                empreendimento = _empreendimentoRepository.FindById(id.Value);
            }
            if (empreendimento.HasValue())
            {
                dto.Nome = empreendimento.Nome;
            }
            return View(dto);
        }

        public ActionResult Listar(DataSourceRequest request, FiltroValorNominalEmpreendimentoDTO filtro)
        {
            filtro.Estado = SessionAttributes.Current().EmpresaVenda.Estado;
            filtro.Situacao = SituacaoValorNominal.Ativo;
            var result = _viewValorNominalEmpreendimentoRepository.Listar(filtro);
            return Json(result.ToDataRequest(request), JsonRequestBehavior.AllowGet);

        }

        public FileContentResult ExportarTodos(DataSourceRequest request, FiltroValorNominalEmpreendimentoDTO filtro)
        {
            filtro.Estado = SessionAttributes.Current().EmpresaVenda.Estado;
            filtro.Situacao = SituacaoValorNominal.Ativo;

            var modifiedRequest = request;
            modifiedRequest.start = 0;
            modifiedRequest.pageSize = 0;

            byte[] file = Exportar(modifiedRequest, filtro);
            string nomeArquivo = GlobalMessages.ValorNominal;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }
        public FileContentResult ExportarPagina(DataSourceRequest request, FiltroValorNominalEmpreendimentoDTO filtro)
        {
            filtro.Estado = SessionAttributes.Current().EmpresaVenda.Estado;
            filtro.Situacao = SituacaoValorNominal.Ativo;

            byte[] file = Exportar(request, filtro);
            string nomeArquivo = GlobalMessages.ValorNominal;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }

        private byte[] Exportar(DataSourceRequest request, FiltroValorNominalEmpreendimentoDTO filtro)
        {
            var results = _viewValorNominalEmpreendimentoRepository.Listar(filtro).ToDataRequest(request);

            ExcelUtil excel = ExcelUtil.NewInstance(20)
                .NewSheet(DateTime.Now.ToString("yyyyMMddHHmmss"))
                .WithHeader(GetHeader());

            foreach (var model in results.records.ToList())
            {
                excel
                    .CreateCellValue(model.NomeEmpreendimento).Width(30)
                    .CreateDateTimeCell(model.InicioVigencia, DateTimeExtensions.PatternDateTimeSeconds).Width(20)
                    .CreateMoneyCell(model.FaixaUmMeioDe).Width(20)
                    .CreateMoneyCell(model.FaixaUmMeioAte).Width(20)
                    .CreateMoneyCell(model.FaixaDoisDe).Width(20)
                    .CreateMoneyCell(model.FaixaDoisAte).Width(20)
                    .CreateMoneyCell(model.PNEDe).Width(20)
                    .CreateMoneyCell(model.PNEAte).Width(20);
            }
            excel.Close();
            return excel.DownloadFile();
        }

        private string[] GetHeader()
        {
            IList<string> header = new List<string>
            {
                GlobalMessages.Nome,
                GlobalMessages.AtualizadoEm,
                GlobalMessages.ValorFaixaUmMeioDe,
                GlobalMessages.ValorFaixaUmMeioAte,
                GlobalMessages.ValorFaixaDoisDe,
                GlobalMessages.ValorFaixaDoisAte,
                GlobalMessages.ValorPNEDe,
                GlobalMessages.ValorPNEAte

            };
            return header.ToArray();
        }
    }
}