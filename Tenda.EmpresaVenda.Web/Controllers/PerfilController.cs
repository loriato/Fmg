using Europa.Commons;
using Europa.Extensions;
using Europa.Web;
using NHibernate;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Tenda.Domain.Core.Models;
using Tenda.Domain.Security;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Models;
using Tenda.Domain.Security.Repository;
using Tenda.Domain.Security.Repository.Models;
using Tenda.Domain.Security.Services;
using Tenda.Domain.Security.Services.Models;
using Tenda.Domain.Security.Views;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Web.Models;
using Tenda.EmpresaVenda.Web.Models.Application;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("SEG11")]
    public class PerfilController : BaseController
    {
        private PerfilService _perfilService { get; set; }
        private PermissaoService _permissaoService { get; set; }
        private FuncionalidadeService _funcionalidadeService { get; set; }
        private UsuarioPerfilSistemaRepository _usuarioPerfilSistemaRepository { get; set; }
        private PerfilRepository _perfilRepository { get; set; }
        private SistemaRepository _sistemaRepository { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult ListaPerfis(DataSourceRequest request, string nome, bool? buscaSuspensos)
        {
            var result = _perfilService.ListarPerfisDataTable(request, nome, buscaSuspensos);
            var resultadosSelecionados = result.records.Select(x => new Perfil
            {
                Id = x.Id,
                Nome = x.Nome,
                Situacao = x.Situacao,
                AtualizadoEm = x.AtualizadoEm,
                AtualizadoPor = x.AtualizadoPor,
                CriadoEm = x.CriadoEm,
                CriadoPor = x.CriadoPor,
                ExibePortal = x.ExibePortal,
                ResponsavelCriacao = new Usuario
                {
                    Id = x.ResponsavelCriacao.Id,
                    Nome = x.ResponsavelCriacao.Nome
                }
            });
            result.records = resultadosSelecionados.ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("SEG04", "Visualizar")]
        public JsonResult ListaFuncionalidade(DataSourceRequest request, string nome, string nomeUF, long? perfil, long idSistema)
        {
            DataSourceResponse<ViewPerfilPermissao> result = new DataSourceResponse<ViewPerfilPermissao>();
            if (perfil.HasValue)
            {
                Sistema sistema = _sistemaRepository.FindById(idSistema);
                result = _permissaoService.ListarFuncionalidades(request, nome, nomeUF, perfil.Value, sistema.Codigo);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("SEG11", "Incluir")]
        [Transaction(TransactionAttributeType.Required)]
        private JsonResult Salvar(Perfil perfil)
        {
            if (perfil.Id == 0)
            {
                perfil.ResponsavelCriacao = SessionAttributes.Current().UsuarioPortal;
                perfil.Situacao = Situacao.Ativo;
            }
            JsonResponse result = new JsonResponse();
            try
            {
                var novo = _perfilService.CriarPerfil(perfil);
                result.Objeto = perfil;
                result.Sucesso = true;
            }
            catch (BusinessRuleException e)
            {
                result.Mensagens.AddRange(e.Errors);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        [BaseAuthorize("SEG11", "Incluir")]
        public JsonResult IncluirPerfil(Perfil perfil)
        {
            return Salvar(perfil);
        }

        [BaseAuthorize("SEG11", "Alterar")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult AlterarPerfil(Perfil perfil)
        {
            return Salvar(perfil);
        }

        [BaseAuthorize("SEG11", "Suspender")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult SuspenderPerfil(long id)
        {
            return AlterarSituacao(id, Situacao.Suspenso);
        }

        [BaseAuthorize("SEG11", "Cancelar")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult CancelarPerfil(long id)
        {
            return AlterarSituacao(id, Situacao.Cancelado);
        }

        [BaseAuthorize("SEG11", "Reativar")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult ReativarPerfil(long id)
        {
            return AlterarSituacao(id, Situacao.Ativo);
        }

        private JsonResult AlterarSituacao(long id, Situacao situacao)
        {
            var resposta = new JsonResponse();

            try
            {
                _perfilService.AlterarSituacaoById(id, situacao);
                resposta.Sucesso = true;
            }
            catch (BusinessRuleException bre)
            {
                resposta.Mensagens.AddRange(bre.Errors);
            }

            return Json(resposta);
        }


        [BaseAuthorize("SEG04", "Incluir")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult AssociarPermissao(long perfil, long funcionalidade)
        {
            JsonResponse result = new JsonResponse();
            try
            {
                _permissaoService.Salvar(new Permissao
                {
                    Funcionalidade = new Funcionalidade { Id = funcionalidade },
                    Perfil = new Perfil { Id = perfil }
                });
                result.Sucesso = true;
            }
            catch (Exception e)
            {
                result.Mensagens.Add(e.Message);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("SEG04", "Excluir")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult RemoverPermissao(long perfil, long funcionalidade)
        {
            JsonResponse result = new JsonResponse();
            try
            {
                _permissaoService.Remover(funcionalidade, perfil);
                result.Sucesso = true;
            }
            catch (Exception e)
            {
                result.Mensagens.Add(e.Message);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("SEG04", "Atualizar")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult AlterarLogFuncionalidade(long funcionalidade, bool logar)
        {
            JsonResponse result = new JsonResponse();
            try
            {
                result.Objeto = _funcionalidadeService.AlterarStatusLog(funcionalidade, logar);
                result.Sucesso = true;
            }
            catch (Exception e)
            {
                result.Mensagens.Add(e.Message);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarPerfisAutocomplete(DataSourceRequest request)
        {
            var result = _perfilService.ListarPerfisAutocomplete(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarPerfisUsuarioLogado()
        {
            var idUsuarioLogado = SessionAttributes.Current().UsuarioPortal.Id;
            var perfis = _usuarioPerfilSistemaRepository.ListarPerfisUsuarioSistema(idUsuarioLogado, ProjectProperties.CodigoEmpresaVenda);

            var result = new PerfilUsuarioViewModel
            {
                IdUsuario = idUsuarioLogado
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BuscarPerfil(DataSourceRequest request, long id = 0)
        {
            var result = _perfilRepository.FindById(id);
            if (result.IsEmpty())
            {
                result = new Perfil();
                result.Nome = "";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarPerfisPortalAutocomplete(DataSourceRequest request)
        {
            var result = _perfilService.ListarPerfisPortalAutocomplete(request);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListaPerfisSelecionados(DataSourceRequest request, FiltroPerfilDTO filtro)
        {
            var result = _perfilService.ListaPerfisSelecionados(request, filtro);
            if(result.records.HasValue())
            {
                var resultadosSelecionados = result.records.Select(x => new Perfil
                {
                    Id = x.Id,
                    Nome = x.Nome
                });
                result.records = resultadosSelecionados.ToList();
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}