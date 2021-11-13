using Europa.Commons;
using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.EmpresaVenda.Domain.Integration.Suat;
using Tenda.EmpresaVenda.Domain.Integration.Suat.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Portal.Controllers;
using Tenda.EmpresaVenda.Portal.Models.Application;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    public class BreveLancamentoController : BaseController
    {
        private BreveLancamentoRepository _breveLancamentoRepository { get; set; }
        private EnderecoBreveLancamentoRepository _enderecoBreveLancamentoRepository { get; set; }
        private EmpresaVendaRepository _empresaVendaRepository { get; set; }
        private EmpreendimentoRepository _empreendimentoRepository { get; set; }

        [HttpGet]
        public JsonResult Listar(DataSourceRequest request)
        {
            var result = _breveLancamentoRepository.DisponiveisParaCatalogo().AsQueryable();
            return Json(result.ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarDaRegional(DataSourceRequest request)
        {
            var empresaVenda = _empresaVendaRepository.FindById(SessionAttributes.Current().EmpresaVendaId);

            var result = _enderecoBreveLancamentoRepository.DisponiveisParaRegional(request, empresaVenda);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarDaRegionalSemEmpreendimento(DataSourceRequest request)
        {
            var empresaVenda = _empresaVendaRepository.FindById(SessionAttributes.Current().EmpresaVendaId);

            var result = _enderecoBreveLancamentoRepository.DisponiveisParaRegionalSemEmpreendimento(request, empresaVenda);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListarTorre(DataSourceRequest request)
        {
            //FIX-ME: Verificar com o Conecta a solução adequada
            if (request.HasValue())
            {
                request.pageSize = 100;
            }

            var filtroBreveLancamento = request.filter.FirstOrDefault(x => x.column.Equals("idBreveLancamento"));
            if (filtroBreveLancamento.IsNull())
            {
                return Json(new List<TorreDTO>().AsQueryable().ToDataRequest(request), JsonRequestBehavior.AllowGet);
            }

            var breveLancamento = _breveLancamentoRepository.FindById(Convert.ToInt32(filtroBreveLancamento.value));
            if (breveLancamento == null || breveLancamento.Empreendimento == null)
            {
                var listTorre = new List<TorreDTO>();
                var torreDto = new TorreDTO();
                torreDto.IdTorre = -1;
                torreDto.NomeTorre = "TORRE INEXISTENTE";
                listTorre.Add(torreDto);
                return Json(listTorre.AsQueryable().ToDataRequest(request), JsonRequestBehavior.AllowGet);
            }

            var results = ConsultaEstoqueService.EstoqueTorre(request, breveLancamento.Empreendimento.Divisao);
            if (results.records.IsEmpty())
            {
                var listTorre = new List<TorreDTO>();
                var torreDto = new TorreDTO();
                torreDto.IdTorre = -1;
                torreDto.NomeTorre = "TORRE INEXISTENTE";
                listTorre.Add(torreDto);
                results = listTorre.AsQueryable().ToDataRequest(request);
            }
            else
            {
                var filtroNomeTorre = request.filter.FirstOrDefault(x => x.column.Equals("NomeTorre"));
                if (filtroNomeTorre.HasValue())
                {
                    results = results.records.Where(x => x.NomeTorre.ToLower().Contains(filtroNomeTorre.value.ToLower())).AsQueryable().ToDataRequest(request);
                }
            }
            return Json(results, JsonRequestBehavior.AllowGet);
        }
    }
}