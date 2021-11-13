using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Tenda.Domain.Core.Models;
using Tenda.Domain.Security;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Models;
using Tenda.Domain.Security.Repository;
using Tenda.Domain.Security.Services;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Web.Controllers;
using Tenda.EmpresaVenda.Web.Models;
using Tenda.EmpresaVenda.Web.Models.Application;
using Tenda.EmpresaVenda.Web.Models.PreferenciasUsuarioViewModel;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.Domain.Core.Services
{
    [BaseAuthorize("SEG03")]
    public class ViabilizadorController : BaseController
    {
        private ViewUsuarioPerfilRepository _viewUsuarioPerfilRepository { get; set; }
        private PerfilRepository _perfilRepository { get; set; }
        private UsuarioPortalRepository _usuarioPortalRepository { get; set; }
        private UsuarioPerfilSistemaRepository _usuarioPerfilSistemaRepository { get; set; }
        private SupervisorViabilizadorRepository _supervisorViabilizadorRepository { get; set; }

        public ViabilizadorController(ISession session) : base(session)
        {
        }

        [HttpGet]
        public JsonResult ListarViabilizadores(DataSourceRequest request)
        {
            var viabilizadores = new List<long>();

            if (SessionAttributes.Current().Perfis.Select(x => x.Id).Contains(ProjectProperties.IdPerfilSupervisorCicloFinanceiro))
            {
                viabilizadores = _supervisorViabilizadorRepository.ViabilizadorByIdSupervisor(SessionAttributes.Current().UsuarioPortal.Id);
            }

            var idPerfilViabilizador = ProjectProperties.IdPerfilViabilizador;
            var codSistema = ProjectProperties.CodigoEmpresaVenda;
            var response = _usuarioPerfilSistemaRepository.ListarUsuariosPerfilSistema(request, idPerfilViabilizador, codSistema,viabilizadores);

            

            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}