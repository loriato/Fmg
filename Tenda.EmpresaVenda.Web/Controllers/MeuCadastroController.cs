using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.Core.Models;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Web.Models;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    public class MeuCadastroController : BaseController
    {
        private EmpresaVendaRepository _empresaVendaRepository { get; set; }
        private StaticResourceService _staticResourceService { get; set; }

        [BaseAuthorize(true)]
        public ActionResult Index(long? id)
        {
            MeuCadastroDTO dto = new MeuCadastroDTO();
            dto.EmpresaVenda = new Tenda.Domain.EmpresaVenda.Models.EmpresaVenda();
            dto.EmpresaVenda.Corretor = new Corretor();
            if (!id.IsEmpty())
            {
                dto.EmpresaVenda = _empresaVendaRepository.FindById(id.Value);
                if (dto.EmpresaVenda == null)
                {
                    dto.EmpresaVenda = new Tenda.Domain.EmpresaVenda.Models.EmpresaVenda();
                }
                else
                {
                    dto.Sede = dto.EmpresaVenda.EnderecoCompleto();
                }
                if (!dto.EmpresaVenda.FotoFachada.IsEmpty())
                {
                    var imageName = _staticResourceService.LoadResource(dto.EmpresaVenda.FotoFachada.Id);
                    dto.Foto = _staticResourceService.CreateUrl(GetWebAppRoot(), imageName);
                }
            }

            return View(dto);
        }

        [BaseAuthorize(true)]
        public ActionResult Detalhar(long? id)
        {
            return RedirectToAction("index", new { id = id });
        }


    }
}