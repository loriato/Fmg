using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.ApiService.Models.Juridico;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Portal.Controllers
{
    public class JuridicoController : BaseController
    {
        private StaticResourceService _staticResourceService { get; set; }
        private DocumentoJuridicoRepository _documentoJuridicoRepository { get; set; }
        
        public ActionResult Index()
        {
            var juridicoDto = new JuridicoDto();

            var idDocumentoJuridico = _documentoJuridicoRepository.Queryable()
                .OrderByDescending(x => x.Id)
                .Select(x => x.Arquivo.Id)
                .First();

            var resourcePath = _staticResourceService.LoadResource(idDocumentoJuridico);
            var urlJuridico = _staticResourceService.CreateUrl(GetWebAppRoot(), resourcePath);
            juridicoDto.Url = urlJuridico;

            return View(juridicoDto);
        }
    }
}
