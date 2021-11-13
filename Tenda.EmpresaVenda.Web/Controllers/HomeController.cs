﻿using Europa.Data;
using Europa.Resources;
using NHibernate;
using System.Linq;
using System.Web.Mvc;
using Tenda.EmpresaVenda.Web.Models.Application;
using Tenda.Domain.Security;
using Tenda.Domain.Security.Models;
using Tenda.Domain.Security.Repository;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController()
        {
        }

        public UnidadeFuncionalRepository _unidadeFuncionalRepository { get; set; }

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AcessoNegado(string uf, string funcionalidade, bool isAjax = false)
        {
            var unidade =
                _unidadeFuncionalRepository.Queryable()
                    .FirstOrDefault(x => x.Codigo == uf && x.Modulo.Sistema.Codigo == ApplicationInfo.CodigoSistema);
            var msg = string.Format(GlobalMessages.MsgAcessoNegado,
                string.Join(", ", SessionAttributes.Current().Perfis.Select(x => x.Nome)),
                unidade.Nome, funcionalidade);
            if (isAjax)
            {
                return new JavaScriptResult { Script = "Europa.AccessDenied('" + msg + "')" };
            }
            return View(model: msg);
        }

        public ActionResult SemAcesso()
        {
            return View();
        }
    }
}