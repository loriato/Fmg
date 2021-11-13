using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("GEC02")]
    public class NovaEvController : BaseController
    {

        private EmpresaVendaRepository _empresaVendaRepository { get; set; }
        private CorretorRepository _corretorRepository { get; set; }
        private ViewPreEmpresaVendaRepository _viewPreEmpresaVendaRepository { get; set; }
        private EmpresaVendaService _empresaVendaService { get; set; }
        private CorretorService _corretorService { get; set; }
        private PontoVendaService _pontoVendaService { get; set; }
        private RegraComissaoEvsRepository _regraComissaoEvsRepository { get; set; }
        private RegionalEmpresaService _regionalEmpresaService { get; set; }

        [BaseAuthorize("GEC02", "Visualizar")]
        public ActionResult Index()
        {
            return View();
        }

        [BaseAuthorize("GEC02", "Visualizar")]
        public ActionResult Listar(DataSourceRequest request, string empresaVenda, string cnpj, string creci)
        {
            var results = _viewPreEmpresaVendaRepository.Listar(empresaVenda, cnpj, creci);
            return Json(results.ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("GEC02", "Incluir")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult Incluir(Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda, Corretor corretor, List<long> idsRegionais)
        {
            return Salvar(empresaVenda, corretor, idsRegionais);
        }

        [BaseAuthorize("GEC02", "Atualizar")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult Atualizar(Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda, Corretor corretor, List<long> idsRegionais)
        {
            return Salvar(empresaVenda, corretor, idsRegionais);
        }

        private JsonResult Salvar(Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda, Corretor corretor, List<long> idsRegionais)
        {
            var json = new JsonResponse();
            var validation = new BusinessRuleException();
            try
            {
                empresaVenda.CentralVendas = "Pendente Remoção";
                var situacao = empresaVenda.Situacao;
                empresaVenda.Situacao = Tenda.Domain.Security.Enums.Situacao.Ativo;

                if (situacao == Tenda.Domain.Security.Enums.Situacao.PreCadastro)
                {
                    var bre = new BusinessRuleException();

                    if (ProjectProperties.IdPerfilDiretorPortalEvs.IsEmpty())
                    {
                        bre.AddError(GlobalMessages.ParametroInexistente).WithParams(GlobalMessages.PerfilDiretorPortalEvs).Complete();
                    }
                    bre.ThrowIfHasError();
                }

                if (idsRegionais.IsEmpty())
                {
                    validation.AddError(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Regionais)).AddField("autocomplete_regional_empresa").Complete();
                }

                _empresaVendaService.Salvar(empresaVenda, validation);

                _regionalEmpresaService.Salvar(empresaVenda, idsRegionais);


                if (situacao == Tenda.Domain.Security.Enums.Situacao.PreCadastro)
                {
                    var pontoVenda = new PontoVenda(empresaVenda);
                    _pontoVendaService.Salvar(pontoVenda, validation);
                }

                corretor.EmpresaVenda = empresaVenda;
                // Para preencher automaticamente os campos que não estão no formulário
                corretor.GerarCorretorCompleto();
                _corretorService.SalvarViaNovaEv(corretor, validation);
                empresaVenda.Corretor = corretor;

                validation.ThrowIfHasError();
                _empresaVendaService.Salvar(empresaVenda, validation);

                json.Mensagens.Add(string.Format(GlobalMessages.MsgSucessoSalvarEmpresaVendaRepresentante,
                    empresaVenda.RazaoSocial, corretor.Nome));
                json.Sucesso = true;
            }
            catch (BusinessRuleException ex)
            {
                CurrentSession().Transaction.Rollback();
                json.Mensagens.AddRange(ex.Errors);
                json.Campos.AddRange(ex.ErrorsFields);
                json.Sucesso = false;
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [BaseAuthorize("GEC02", "Visualizar")]
        public ActionResult BuscarEmpresaVenda(long idEmpresaVenda, long idCorretor)
        {
            var jsonResponse = new JsonResponse();
            try
            {
                var empresaVenda = _empresaVendaRepository.FindById(idEmpresaVenda);
                var corretor = _corretorRepository.FindById(idCorretor);

                var result = new
                {
                    htmlEmpresaVenda = RenderRazorViewToString("_FormularioEmpresaVenda", empresaVenda, false),
                    htmlEnderecoEmpresaVenda = RenderRazorViewToString("_FormularioEndereco", empresaVenda, false),
                    htmlDadosTributarios = RenderRazorViewToString("_FormularioDadosTributarios", empresaVenda, false),
                    htmlRepresentanteLegal = RenderRazorViewToString("_FormularioDadosRepresentanteLegal", corretor, false),
                    htmlResponsavelTecnico = RenderRazorViewToString("_FormularioResponsavelTecnico", empresaVenda, false),

                };
                jsonResponse.Objeto = result;
                jsonResponse.Sucesso = true;
                jsonResponse.Mensagens.Add(GlobalMessages.MsgUploadFotoSucesso);
            }
            catch (BusinessRuleException bre)
            {
                jsonResponse.Mensagens.AddRange(bre.Errors);
                jsonResponse.Sucesso = false;
            }

            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }

        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Cancelar(long idEmpresaVenda, string motivo)
        {
            var jsonResponse = new JsonResponse();
            var bre = new BusinessRuleException();
            try
            {
                if (motivo.IsEmpty())
                {
                    bre.AddError(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Motivo))
                    .AddField("Motivo").Complete();
                }
                bre.ThrowIfHasError();

                var empresaVenda = _empresaVendaRepository.FindById(idEmpresaVenda);
                _empresaVendaService.CancelarEV(empresaVenda, motivo);

                jsonResponse.Sucesso = true;
                jsonResponse.Mensagens.Add(string.Format(GlobalMessages.EmailReprovacaoEv, empresaVenda.RazaoSocial));

            }
            catch (BusinessRuleException ex)
            {
                CurrentSession().Transaction.Rollback();
                jsonResponse.Sucesso = false;
                jsonResponse.Mensagens.AddRange(ex.Errors);
                jsonResponse.Campos.AddRange(ex.ErrorsFields);
            }

            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }
    }
}