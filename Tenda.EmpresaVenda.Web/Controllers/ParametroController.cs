using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Models;
using Tenda.Domain.Security.Repository;
using Tenda.Domain.Security.Services;
using Tenda.EmpresaVenda.Web.Models;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    public class ParametroController : BaseController
    {
        private ParametroService _parametroService { get; set; }
        private SistemaService _sistemaService { get; set; }
        private ParametroRepository _parametroRepository { get; set; }

        [BaseAuthorize("SEG10", "Visualizar")]
        public ActionResult Index()
        {
            return View();
        }


        [BaseAuthorize("SEG10", "ExportarPagina")]
        public ActionResult ExportarPagina(DataSourceRequest request, string chave, long? idSistema)
        {
            byte[] file = Exportar(request, chave, idSistema);
            string nomeArquivo = GlobalMessages.Parametros;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }
        [BaseAuthorize("SEG10", "ExportarTodos")]
        public ActionResult ExportarTodos(DataSourceRequest request, string chave, long? idSistema)
        {
            request.start = 0;
            request.pageSize = 0;

            byte[] file = Exportar(request, chave, idSistema);
            string nomeArquivo = GlobalMessages.Parametros;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }

        private byte[] Exportar(DataSourceRequest request, string chave, long? idSistema)
        {
            ExcelUtil excel = ExcelUtil.NewInstance(20)
                .NewSheet(DateTime.Now.ToString("yyyyMMddHHmmss"))
                .WithHeader(GetHeader());
            var list = _parametroRepository.BuscarParametros(chave, idSistema);
            var rFinal = new List<ParametroDto>() { };
            string dateCol = "dd/MM/yyyy HH:mm";
            foreach (var item in list.ToDataRequest(request).records.ToList())
            {
                rFinal.Add(new ParametroDto()
                {
                    Id = item.Id,
                    Sistema = item.Sistema,
                    Chave = item.Chave,
                    TipoParametro = item.TipoParametro,
                    Valor = item.Valor,
                    Descricao = item.Descricao,
                    Detalhe = ReturnDetalhe(item.TipoParametro, item.Valor)
                });
            }
            foreach (var model in rFinal.ToList())
            {
                excel.CreateCellValue(model.Sistema.Nome).Width(30)
                    .CreateCellValue(model.Chave).Width(40)
                    .CreateCellValue(model.TipoParametro).Width(40)
                    .CreateCellValue(model.Valor).Width(30)
                    .CreateCellValue(model.Detalhe).Width(40)
                    .CreateCellValue(model.Descricao).Width(20);
            }
            excel.Close();
            return excel.DownloadFile();
        }

        public string[] GetHeader()
        {
            IList<string> header = new List<string>
            {
                GlobalMessages.Sistema,
                GlobalMessages.Chave,
                GlobalMessages.TipoParametro,
                GlobalMessages.Valor,
                GlobalMessages.Detalhe,
                GlobalMessages.Descricao
            };
            return header.ToArray();
        }

        [BaseAuthorize("SEG10", "Visualizar")]
        public ActionResult ListarDatatable(DataSourceRequest request, string chave, long? idSistema)
        {
            var parametros = _parametroRepository.BuscarParametros(chave, idSistema);
            var dreq = parametros.ToDataRequest(request);
            var rFinal = new List<ParametroDto>() { };
            foreach (var item in dreq.records.ToList())
            {
                rFinal.Add(new ParametroDto()
                {
                    Id = item.Id,
                    Sistema = item.Sistema,
                    Chave = item.Chave,
                    TipoParametro = item.TipoParametro,
                    Valor = item.Valor,
                    Descricao = item.Descricao,
                    Detalhe = ReturnDetalhe(item.TipoParametro, item.Valor)
                });
            }

            var result = new DataSourceResponse<ParametroDto> { records = rFinal, total = dreq.total, filtered = dreq.total };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("SEG10", "Visualizar")]
        public ActionResult ListarSistemas()
        {
            var list = _sistemaService.ListarTodos().ToList();
            var aux = list.Select(x => new SelectListItem
            {
                Text = x.Nome,
                Value = x.Id.ToString()
            });
            return PartialView("_SistemaDropDownList", aux);
        }

        [BaseAuthorize("SEG10", "Incluir")]
        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult IncluirParametro(Parametro parametro, long? idSistema)
        {
            var result = Salvar(parametro, idSistema);
            return result;
        }

        [BaseAuthorize("SEG10", "Atualizar")]
        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult AlterarParametro(Parametro parametro, long? idSistema)
        {
            var result = Salvar(parametro, idSistema);
            return result;
        }

        [BaseAuthorize("SEG10", "Excluir")]
        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult ExcluirParametro(long? id)
        {
            var json = new JsonResponse();
            if (!id.HasValue)
            {
                json.Mensagens.Add(GlobalMessages.Erro);
                return Json(json);
            }
            try
            {
                _parametroService.Excluir(id.Value);
                json.Sucesso = true;
            }
            catch (BusinessRuleException ex)
            {
                json.Mensagens.AddRange(ex.Errors);
            }
            return Json(json);
        }

        private ActionResult Salvar(Parametro parametro, long? idSistema)
        {
            var json = new JsonResponse();
            try
            {
                _parametroService.ValidarCampos(parametro, idSistema);
            }
            catch (BusinessRuleException ex)
            {
                json.Mensagens.AddRange(ex.Errors);
                return Json(json);
            }

            parametro.Sistema = new Sistema
            {
                Id = idSistema.Value
            };
            parametro.Sistema = _sistemaService.FindById(parametro.Sistema.Id);
            _parametroService.Salvar(parametro);
            json.Sucesso = true;
            json.Objeto = parametro;
            return Json(json);
        }

        private string ReturnDetalhe(TipoParametro tipo, string valor)
        {
            var detalhe = GlobalMessages.TipoParametro_Generico;
            switch (tipo)
            {
            }
            return detalhe;
        }
    }
}