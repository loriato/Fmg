using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Web.Models;
using Tenda.EmpresaVenda.Web.Models.Application;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    public class PrePropostaWorkflowController : BaseController
    {
        private PrePropostaRepository _prePropostaRepository { get; set; }
        private PrePropostaService _prePropostaService { get; set; }
        private ViewHistoricoPrePropostaRepository _viewHistoricoPrePropostaRepository { get; set; }
        private EmpreendimentoRepository _empreendimentoRepository { get; set; }
        private ClienteRepository _clienteRepository { get; set; }
        private EmpreendimentoService _empreendimentoService { get; set; }
        private NotificacaoRepository _notificacaoRepository { get; set; }
        private UsuarioGrupoCCARepository _usuarioGrupoCCARepository { get; set; }


        public ActionResult Index(string id)
        {
            PreProposta preProposta = new PreProposta();
            if (id.IsEmpty() == false)
            {
                preProposta = _prePropostaRepository.BuscarPorCodigo(id);
                if (preProposta.IsEmpty())
                {
                    preProposta = new PreProposta();
                }
            }
            return View(preProposta);
        }

        [HttpGet]
        public ActionResult BuscarPorCodigo(string codigo)
        {
            var preProposta = _prePropostaRepository.BuscarPorCodigo(codigo);
            var result = new JsonResponse();
            if (preProposta != null)
            {
                result.Sucesso = true;
                result.Objeto = preProposta.Codigo;
            }
            else
            {
                result.Sucesso = false;
                result.Mensagens.Add(string.Format(GlobalMessages.PropostaNaoEncontrada, codigo));
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Historico(DataSourceRequest request, long idPreProposta)
        {
            var result = _viewHistoricoPrePropostaRepository.Listar(request, idPreProposta);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult Enviar(long idPreProposta)
        {
            var json = new JsonResponse();
            var validation = new BusinessRuleException();
            try
            {
                PreProposta preProposta = _prePropostaRepository.FindById(idPreProposta);
                _prePropostaService.ValidarRateioComissao(preProposta);
                _prePropostaService.MudarSituacaoProposta(preProposta, SituacaoProposta.AguardandoAnaliseSimplificada, SessionAttributes.Current().UsuarioPortal, null, SessionAttributes.Current().Perfis);
                _prePropostaService.EnviarEmailAguardandoAnalise(preProposta);
                json.Sucesso = true;
                json.Objeto = new PreProposta { Id = preProposta.Id, Codigo = preProposta.Codigo, SituacaoProposta = preProposta.SituacaoProposta };
                json.Mensagens.Add(string.Format(GlobalMessages.MsgAvancoEtapaPreProposta, SituacaoProposta.AguardandoAnaliseSimplificada.AsString()));
            }
            catch (BusinessRuleException ex)
            {
                CurrentSession().Transaction.Rollback();
                json.Mensagens.AddRange(ex.Errors);
                //json.Campos.AddRange(ex.ErrorsFields);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult Reenviar(long idPreProposta)
        {
            var json = new JsonResponse();
            var validation = new BusinessRuleException();
            try
            {
                PreProposta preProposta = _prePropostaRepository.FindById(idPreProposta);
                _prePropostaService.MudarSituacaoProposta(preProposta, SituacaoProposta.Retorno, SessionAttributes.Current().UsuarioPortal, null, SessionAttributes.Current().Perfis);
                json.Sucesso = true;
                json.Objeto = new PreProposta { Id = preProposta.Id, Codigo = preProposta.Codigo, SituacaoProposta = preProposta.SituacaoProposta };
                json.Mensagens.Add(string.Format(GlobalMessages.MsgAvancoEtapaPreProposta, SituacaoProposta.Retorno.AsString()));
            }
            catch (BusinessRuleException ex)
            {
                CurrentSession().Transaction.Rollback();
                json.Mensagens.AddRange(ex.Errors);
                //json.Campos.AddRange(ex.ErrorsFields);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult Revisar(long idPreProposta)
        {
            var json = new JsonResponse();
            var validation = new BusinessRuleException();
            try
            {
                PreProposta preProposta = _prePropostaRepository.FindById(idPreProposta);
                _prePropostaService.MudarSituacaoProposta(preProposta, SituacaoProposta.EmAnaliseSimplificada, SessionAttributes.Current().UsuarioPortal, null, SessionAttributes.Current().Perfis);
                json.Sucesso = true;
                json.Objeto = new PreProposta { Id = preProposta.Id, Codigo = preProposta.Codigo, SituacaoProposta = preProposta.SituacaoProposta };
                json.Mensagens.Add(string.Format(GlobalMessages.MsgAvancoEtapaPreProposta, SituacaoProposta.EmAnaliseSimplificada.AsString()));
            }
            catch (BusinessRuleException ex)
            {
                CurrentSession().Transaction.Rollback();
                json.Mensagens.AddRange(ex.Errors);
                //json.Campos.AddRange(ex.ErrorsFields);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult Analisar(long idPreProposta)
        {
            var json = new JsonResponse();
            var validation = new BusinessRuleException();
            try
            {
                PreProposta preProposta = _prePropostaRepository.FindById(idPreProposta);
                //preProposta.UltimoCCA = _usuarioGrupoCCARepository.ListarGruposPorUsuario(SessionAttributes.Current().UsuarioPortal.Id);
                _prePropostaService.MudarSituacaoProposta(preProposta, SituacaoProposta.EmAnaliseSimplificada, SessionAttributes.Current().UsuarioPortal, null, SessionAttributes.Current().Perfis);
                json.Sucesso = true;
                json.Objeto = new PreProposta { Id = preProposta.Id, Codigo = preProposta.Codigo, SituacaoProposta = preProposta.SituacaoProposta };
                json.Mensagens.Add(string.Format(GlobalMessages.MsgAvancoEtapaPreProposta, SituacaoProposta.EmAnaliseSimplificada.AsString()));
            }
            catch (BusinessRuleException ex)
            {
                CurrentSession().Transaction.Rollback();
                json.Mensagens.AddRange(ex.Errors);
                //json.Campos.AddRange(ex.ErrorsFields);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult Aprovar(long idPreProposta)
        {
            var json = new JsonResponse();
            var validation = new BusinessRuleException();
            try
            {
                PreProposta preProposta = _prePropostaRepository.FindById(idPreProposta);
                
                _prePropostaService.MudarSituacaoProposta(preProposta, SituacaoProposta.AnaliseSimplificadaAprovada, SessionAttributes.Current().UsuarioPortal, null, SessionAttributes.Current().Perfis);
                json.Sucesso = true;
                json.Objeto = new PreProposta { Id = preProposta.Id, Codigo = preProposta.Codigo, SituacaoProposta = preProposta.SituacaoProposta };
                json.Mensagens.Add(string.Format(GlobalMessages.MsgAvancoEtapaPreProposta, SituacaoProposta.AnaliseSimplificadaAprovada.AsString()));
            }
            catch (BusinessRuleException ex)
            {
                CurrentSession().Transaction.Rollback();
                json.Mensagens.AddRange(ex.Errors);
                //json.Campos.AddRange(ex.ErrorsFields);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult AguardandoIntegracao(long idPreProposta)
        {
            var json = new JsonResponse();
            var validation = new BusinessRuleException();
            try
            {
                PreProposta preProposta = _prePropostaRepository.FindById(idPreProposta);

                //caso a PPR esteja vindo pelo fluxo simplificado (Fluxo envido > aguardando integração) Sicaq previa é passado pro fixo
                if (preProposta.SituacaoProposta == SituacaoProposta.FluxoEnviado || preProposta.SituacaoProposta == SituacaoProposta.AguardandoFluxo)
                {
                    preProposta.DataSicaq = preProposta.DataSicaqPrevio;
                    preProposta.StatusSicaq = preProposta.StatusSicaqPrevio;
                    preProposta.ParcelaAprovada = preProposta.ParcelaAprovadaPrevio;
                    preProposta.FaixaUmMeio = preProposta.FaixaUmMeioPrevio;
                }

                _prePropostaService.MudarSituacaoProposta(preProposta, SituacaoProposta.AguardandoIntegracao, SessionAttributes.Current().UsuarioPortal, null, SessionAttributes.Current().Perfis);

                var notificacao = new Notificacao
                {
                    Titulo = string.Format(GlobalMessages.NotificacaoProposta_AguardandoIntegracaoTitulo, preProposta.Codigo, preProposta.Cliente.NomeCompleto),
                    Conteudo = GlobalMessages.NotificacaoProposta_AguardandoIntegracaoCorpo,
                    Usuario = preProposta.Viabilizador,
                    EmpresaVenda = preProposta.EmpresaVenda,
                    TipoNotificacao = TipoNotificacao.Comum,
                    DestinoNotificacao = DestinoNotificacao.Adm,
                };

                _notificacaoRepository.Save(notificacao);

                json.Sucesso = true;
                json.Objeto = new PreProposta { Id = preProposta.Id, Codigo = preProposta.Codigo, SituacaoProposta = preProposta.SituacaoProposta };
                json.Mensagens.Add(string.Format(GlobalMessages.MsgAvancoEtapaPreProposta, SituacaoProposta.AguardandoIntegracao.AsString()));
            }
            catch (BusinessRuleException ex)
            {
                CurrentSession().Transaction.Rollback();
                json.Mensagens.AddRange(ex.Errors);
                //json.Campos.AddRange(ex.ErrorsFields);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult AnaliseCompletaAprovada(long idPreProposta)
        {
            var json = new JsonResponse();
            var validation = new BusinessRuleException();
            try
            {
                PreProposta preProposta = _prePropostaRepository.FindById(idPreProposta);

                _prePropostaService.MudarSituacaoProposta(preProposta, SituacaoProposta.AnaliseCompletaAprovada, SessionAttributes.Current().UsuarioPortal, null, SessionAttributes.Current().Perfis);

                json.Sucesso = true;
                json.Objeto = new PreProposta { Id = preProposta.Id, Codigo = preProposta.Codigo, SituacaoProposta = preProposta.SituacaoProposta };
                json.Mensagens.Add(string.Format(GlobalMessages.MsgAvancoEtapaPreProposta, SituacaoProposta.AnaliseCompletaAprovada.AsString()));
            }
            catch (BusinessRuleException ex)
            {
                CurrentSession().Transaction.Rollback();
                json.Mensagens.AddRange(ex.Errors);
                //json.Campos.AddRange(ex.ErrorsFields);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult ReenviarAnaliseCompletaAprovada(ReenviarAnaliseCompletaDTO dto)
        {
            var json = new JsonResponse();
            var validation = new BusinessRuleException();
            try
            {

                PreProposta preProposta = _prePropostaRepository.FindById(dto.idPreProposta);
                preProposta.JustificativaReenvio = dto.Justificativa;
                _prePropostaService.ReenviarAnaliseCompletaAprovada(preProposta, SituacaoProposta.AnaliseCompletaAprovada, SessionAttributes.Current().UsuarioPortal, SessionAttributes.Current().Perfis);


                json.Sucesso = true;
                json.Objeto = new PreProposta { Id = preProposta.Id, Codigo = preProposta.Codigo, SituacaoProposta = preProposta.SituacaoProposta };
                json.Mensagens.Add(string.Format(GlobalMessages.MsgAlteracaoEtapaPreProposta, SituacaoProposta.AnaliseCompletaAprovada.AsString()));

            }
            catch (BusinessRuleException ex)
            {
                CurrentSession().Transaction.Rollback();
                json.Mensagens.AddRange(ex.Errors);
                //json.Campos.AddRange(ex.ErrorsFields);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult Pendenciar(long idPreProposta)
        {
            var json = new JsonResponse();
            var validation = new BusinessRuleException();
            try
            {
                PreProposta preProposta = _prePropostaRepository.FindById(idPreProposta);
                _prePropostaService.MudarSituacaoProposta(preProposta, SituacaoProposta.DocsInsuficientesSimplificado, SessionAttributes.Current().UsuarioPortal, null, SessionAttributes.Current().Perfis);
                json.Sucesso = true;
                json.Objeto = new PreProposta { Id = preProposta.Id, Codigo = preProposta.Codigo, SituacaoProposta = preProposta.SituacaoProposta };
                json.Mensagens.Add(string.Format(GlobalMessages.MsgAvancoEtapaPreProposta, SituacaoProposta.DocsInsuficientesSimplificado.AsString()));
            }
            catch (BusinessRuleException ex)
            {
                CurrentSession().Transaction.Rollback();
                json.Mensagens.AddRange(ex.Errors);
                //json.Campos.AddRange(ex.ErrorsFields);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult Finalizar(long idPreProposta)
        {
            var json = new JsonResponse();
            var validation = new BusinessRuleException();
            try
            {
                PreProposta preProposta = _prePropostaRepository.FindById(idPreProposta);
                _prePropostaService.MudarSituacaoProposta(preProposta, SituacaoProposta.AguardandoIntegracao, SessionAttributes.Current().UsuarioPortal, null, SessionAttributes.Current().Perfis);
                json.Sucesso = true;
                json.Objeto = new PreProposta { Id = preProposta.Id, Codigo = preProposta.Codigo, SituacaoProposta = preProposta.SituacaoProposta };
                json.Mensagens.Add(string.Format(GlobalMessages.MsgAvancoEtapaPreProposta, SituacaoProposta.AguardandoIntegracao.AsString()));
            }
            catch (BusinessRuleException ex)
            {
                CurrentSession().Transaction.Rollback();
                json.Mensagens.AddRange(ex.Errors);
                //json.Campos.AddRange(ex.ErrorsFields);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult AguardarFluxo(long idPreProposta)
        {
            var json = new JsonResponse();
            var validation = new BusinessRuleException();
            try
            {
                PreProposta preProposta = _prePropostaRepository.FindById(idPreProposta);
                _prePropostaService.MudarSituacaoProposta(preProposta, SituacaoProposta.AguardandoFluxo, SessionAttributes.Current().UsuarioPortal, null, SessionAttributes.Current().Perfis);
                json.Sucesso = true;
                json.Objeto = new PreProposta { Id = preProposta.Id, Codigo = preProposta.Codigo, SituacaoProposta = preProposta.SituacaoProposta };
                json.Mensagens.Add(string.Format(GlobalMessages.MsgAvancoEtapaPreProposta, SituacaoProposta.AguardandoFluxo.AsString()));
            }
            catch (BusinessRuleException ex)
            {
                CurrentSession().Transaction.Rollback();
                json.Mensagens.AddRange(ex.Errors);
                //json.Campos.AddRange(ex.ErrorsFields);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult Cancelar(long idPreProposta)
        {
            var json = new JsonResponse();
            var validation = new BusinessRuleException();
            try
            {
                PreProposta preProposta = _prePropostaRepository.FindById(idPreProposta);
                _prePropostaService.MudarSituacaoProposta(preProposta, SituacaoProposta.Cancelada, SessionAttributes.Current().UsuarioPortal, null, SessionAttributes.Current().Perfis);
                json.Sucesso = true;
                json.Objeto = new PreProposta { Id = preProposta.Id, Codigo = preProposta.Codigo, SituacaoProposta = preProposta.SituacaoProposta };
                json.Mensagens.Add(string.Format(GlobalMessages.MsgAvancoEtapaPreProposta, SituacaoProposta.Cancelada.AsString()));
            }
            catch (BusinessRuleException ex)
            {
                CurrentSession().Transaction.Rollback();
                json.Mensagens.AddRange(ex.Errors);
                //json.Campos.AddRange(ex.ErrorsFields);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult SavlarPreProposta(long idPreProposta, string observacao)
        {
            var json = new JsonResponse();
            var validation = new BusinessRuleException();
            try
            {
                PreProposta preProposta = _prePropostaRepository.FindById(idPreProposta);
                preProposta.Observacao = observacao;
                _prePropostaRepository.Save(preProposta);
                json.Sucesso = true;
                json.Objeto = new PreProposta { Id = preProposta.Id, Codigo = preProposta.Codigo, SituacaoProposta = preProposta.SituacaoProposta };
                json.Mensagens.Add(string.Format(GlobalMessages.MsgSalvoSucesso, preProposta.Codigo));
            }
            catch (BusinessRuleException ex)
            {
                CurrentSession().Transaction.Rollback();
                json.Mensagens.AddRange(ex.Errors);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult Retroceder(long idPreProposta)
        {
            var json = new JsonResponse();
            var validation = new BusinessRuleException();
            try
            {
                PreProposta preProposta = _prePropostaRepository.FindById(idPreProposta);
                _prePropostaService.RetrocederPreProposta(preProposta);
                json.Sucesso = true;
                json.Objeto = new PreProposta { Id = preProposta.Id, Codigo = preProposta.Codigo, SituacaoProposta = preProposta.SituacaoProposta };
                json.Mensagens.Add(string.Format(GlobalMessages.MsgAvancoEtapaPreProposta, preProposta.SituacaoProposta.AsString()));
            }
            catch (BusinessRuleException ex)
            {
                CurrentSession().Transaction.Rollback();
                json.Mensagens.AddRange(ex.Errors);
                //json.Campos.AddRange(ex.ErrorsFields);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult DesassociarUnidade(long idPreProposta)
        {
            var json = new JsonResponse();
            var preProposta = _prePropostaRepository.FindById(idPreProposta);
            _prePropostaService.DesassociarUnidade(preProposta);

            json.Sucesso = true;
            json.Mensagens.Add(string.Format(GlobalMessages.UnidadePrePropostaDesassociadaSucesso, preProposta.Codigo));

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult BuscarEmpreendimento(long idEmpreendimento)
        {
            var empreendimento = _empreendimentoRepository.FindById(idEmpreendimento);

            var empreendimentoDTO = new Empreendimento
            {
                Id = empreendimento.Id,
                DisponivelCatalogo = empreendimento.DisponivelCatalogo,
                DisponivelParaVenda = empreendimento.DisponivelParaVenda
            };

            return Json(empreendimentoDTO, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult SalvarEmpreendimento(Empreendimento empreendimentoDTO)
        {
            var json = new JsonResponse();
            try
            {
                var empreendimento = _empreendimentoRepository.FindById(empreendimentoDTO.Id);

                empreendimento.DisponivelCatalogo = empreendimentoDTO.DisponivelCatalogo;
                empreendimento.DisponivelParaVenda = empreendimentoDTO.DisponivelParaVenda;
                _empreendimentoRepository.Save(empreendimento);

                json.Sucesso = true;
                json.Mensagens.Add(GlobalMessages.SalvoSucesso);
            }
            catch (BusinessRuleException ex)
            {
                CurrentSession().Transaction.Rollback();
                json.Mensagens.AddRange(ex.Errors);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [Transaction(TransactionAttributeType.Required)]
        public JsonResult AtualizarSuatIdCliente(long idCliente, long idSuat)
        {
            var json = new JsonResponse();
            try
            {
                Cliente cliente = _clienteRepository.FindById(idCliente);
                cliente.IdSuat = idSuat;
                _clienteRepository.Save(cliente);
            }
            catch (BusinessRuleException ex)
            {
                CurrentSession().Transaction.Rollback();
                json.Mensagens.AddRange(ex.Errors);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }     

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult AnaliseAuditoria(long idPreProposta, int botao)
        {
            var json = new JsonResponse();
            try
            {
                var preProposta = _prePropostaRepository.FindById(idPreProposta);

                switch(botao)
                {
                    case 1:
                        _prePropostaService.MudarSituacaoProposta(preProposta, SituacaoProposta.AguardandoAuditoria, SessionAttributes.Current().UsuarioPortal, null, SessionAttributes.Current().Perfis);
                        json.Sucesso = true;
                        json.Objeto = new PreProposta { Id = preProposta.Id, Codigo = preProposta.Codigo, SituacaoProposta = preProposta.SituacaoProposta };
                        json.Mensagens.Add(string.Format(GlobalMessages.MsgAvancoEtapaPreProposta, SituacaoProposta.AguardandoAuditoria.AsString()));
                        break;
                    case 2:
                        _prePropostaService.MudarSituacaoProposta(preProposta, SituacaoProposta.EmAnaliseSimplificada, SessionAttributes.Current().UsuarioPortal, null, SessionAttributes.Current().Perfis);
                        json.Sucesso = true;
                        json.Objeto = new PreProposta { Id = preProposta.Id, Codigo = preProposta.Codigo, SituacaoProposta = preProposta.SituacaoProposta };
                        json.Mensagens.Add(string.Format(GlobalMessages.MsgAvancoEtapaPreProposta, SituacaoProposta.EmAnaliseSimplificada.AsString()));
                        break;
                    default:
                        json.Sucesso = false;
                        json.Mensagens.Add(GlobalMessages.SalvoInsucesso);
                        break;
                }
            }
            catch (BusinessRuleException ex)
            {
                CurrentSession().Transaction.Rollback();
                json.Mensagens.AddRange(ex.Errors);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult AguardandoAnaliseCompleta(long idPreProposta)
        {
            var json = new JsonResponse();
            var validation = new BusinessRuleException();
            try
            {
                PreProposta preProposta = _prePropostaRepository.FindById(idPreProposta);
                _prePropostaService.MudarSituacaoProposta(preProposta, SituacaoProposta.AguardandoAnaliseCompleta, SessionAttributes.Current().UsuarioPortal, null, SessionAttributes.Current().Perfis);
                _prePropostaService.EnviarEmailAguardandoAnalise(preProposta);
                json.Sucesso = true;
                json.Objeto = new PreProposta { Id = preProposta.Id, Codigo = preProposta.Codigo, SituacaoProposta = preProposta.SituacaoProposta };
                json.Mensagens.Add(string.Format(GlobalMessages.MsgAvancoEtapaPreProposta, SituacaoProposta.AguardandoAnaliseCompleta.AsString()));
            }
            catch (BusinessRuleException ex)
            {
                CurrentSession().Transaction.Rollback();
                json.Mensagens.AddRange(ex.Errors);
                //json.Campos.AddRange(ex.ErrorsFields);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult EmAnaliseCompleta(long idPreProposta)
        {
            var json = new JsonResponse();
            var validation = new BusinessRuleException();
            try
            {
                PreProposta preProposta = _prePropostaRepository.FindById(idPreProposta);
                //preProposta.UltimoCCA = _usuarioGrupoCCARepository.ListarGruposPorUsuario(SessionAttributes.Current().UsuarioPortal.Id);
                _prePropostaService.MudarSituacaoProposta(preProposta, SituacaoProposta.EmAnaliseCompleta, SessionAttributes.Current().UsuarioPortal, null, SessionAttributes.Current().Perfis);
                json.Sucesso = true;
                json.Objeto = new PreProposta { Id = preProposta.Id, Codigo = preProposta.Codigo, SituacaoProposta = preProposta.SituacaoProposta };
                json.Mensagens.Add(string.Format(GlobalMessages.MsgAvancoEtapaPreProposta, SituacaoProposta.EmAnaliseCompleta.AsString()));
            }
            catch (BusinessRuleException ex)
            {
                CurrentSession().Transaction.Rollback();
                json.Mensagens.AddRange(ex.Errors);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult DocsInsuficientesSimplificado(long idPreProposta)
        {
            var json = new JsonResponse();
            var validation = new BusinessRuleException();
            try
            {
                PreProposta preProposta = _prePropostaRepository.FindById(idPreProposta);
                _prePropostaService.MudarSituacaoProposta(preProposta, SituacaoProposta.DocsInsuficientesSimplificado, SessionAttributes.Current().UsuarioPortal, null, SessionAttributes.Current().Perfis);
                json.Sucesso = true;
                json.Objeto = new PreProposta { Id = preProposta.Id, Codigo = preProposta.Codigo, SituacaoProposta = preProposta.SituacaoProposta };
                json.Mensagens.Add(string.Format(GlobalMessages.MsgAvancoEtapaPreProposta, SituacaoProposta.DocsInsuficientesSimplificado.AsString()));
            }
            catch (BusinessRuleException ex)
            {
                CurrentSession().Transaction.Rollback();
                json.Mensagens.AddRange(ex.Errors);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult DocsInsuficientesCompleta(long idPreProposta)
        {
            var json = new JsonResponse();
            var validation = new BusinessRuleException();
            try
            {
                PreProposta preProposta = _prePropostaRepository.FindById(idPreProposta);
                _prePropostaService.MudarSituacaoProposta(preProposta, SituacaoProposta.DocsInsuficientesCompleta, SessionAttributes.Current().UsuarioPortal, null, SessionAttributes.Current().Perfis);
                json.Sucesso = true;
                json.Objeto = new PreProposta { Id = preProposta.Id, Codigo = preProposta.Codigo, SituacaoProposta = preProposta.SituacaoProposta };
                json.Mensagens.Add(string.Format(GlobalMessages.MsgAvancoEtapaPreProposta, SituacaoProposta.DocsInsuficientesCompleta.AsString()));
            }
            catch (BusinessRuleException ex)
            {
                CurrentSession().Transaction.Rollback();
                json.Mensagens.AddRange(ex.Errors);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult AguardandoAuditoria(long idPreProposta)
        {
            var json = new JsonResponse();
            var validation = new BusinessRuleException();
            try
            {
                PreProposta preProposta = _prePropostaRepository.FindById(idPreProposta);
                _prePropostaService.MudarSituacaoProposta(preProposta, SituacaoProposta.AguardandoAuditoria, SessionAttributes.Current().UsuarioPortal, null, SessionAttributes.Current().Perfis);
                json.Sucesso = true;
                json.Objeto = new PreProposta { Id = preProposta.Id, Codigo = preProposta.Codigo, SituacaoProposta = preProposta.SituacaoProposta };
                json.Mensagens.Add(string.Format(GlobalMessages.MsgAvancoEtapaPreProposta, SituacaoProposta.AguardandoAuditoria.AsString()));
            }
            catch (BusinessRuleException ex)
            {
                CurrentSession().Transaction.Rollback();
                json.Mensagens.AddRange(ex.Errors);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult FluxoEnviado(long idPreProposta)
        {
            var json = new JsonResponse();
            var validation = new BusinessRuleException();
            try
            {
                PreProposta preProposta = _prePropostaRepository.FindById(idPreProposta);
                _prePropostaService.MudarSituacaoProposta(preProposta, SituacaoProposta.FluxoEnviado, SessionAttributes.Current().UsuarioPortal, null, SessionAttributes.Current().Perfis);
                json.Sucesso = true;
                json.Objeto = new PreProposta { Id = preProposta.Id, Codigo = preProposta.Codigo, SituacaoProposta = preProposta.SituacaoProposta };
                json.Mensagens.Add(string.Format(GlobalMessages.MsgAvancoEtapaPreProposta, SituacaoProposta.FluxoEnviado.AsString()));
            }
            catch (BusinessRuleException ex)
            {
                CurrentSession().Transaction.Rollback();
                json.Mensagens.AddRange(ex.Errors);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult RetornoAuditoria(long idPreProposta)
        {
            var json = new JsonResponse();
            var validation = new BusinessRuleException();
            try
            {
                PreProposta preProposta = _prePropostaRepository.FindById(idPreProposta);
                var situacao = _prePropostaService.SituacaoAnterior(idPreProposta);
                _prePropostaService.MudarSituacaoProposta(preProposta, situacao.Value, SessionAttributes.Current().UsuarioPortal, null, SessionAttributes.Current().Perfis);
                json.Sucesso = true;
                json.Objeto = new PreProposta { Id = preProposta.Id, Codigo = preProposta.Codigo, SituacaoProposta = preProposta.SituacaoProposta };
                json.Mensagens.Add(string.Format(GlobalMessages.MsgAvancoEtapaPreProposta, situacao.AsString()));
            }
            catch (BusinessRuleException ex)
            {
                CurrentSession().Transaction.Rollback();
                json.Mensagens.AddRange(ex.Errors);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult DocsInsuficientes(long idPreProposta)
        {
            var json = new JsonResponse();
            var validation = new BusinessRuleException();
            try
            {
                PreProposta preProposta = _prePropostaRepository.FindById(idPreProposta);
                _prePropostaService.MudarSituacaoProposta(preProposta, SituacaoProposta.DocsInsuficientesSimplificado, SessionAttributes.Current().UsuarioPortal, null, SessionAttributes.Current().Perfis);
                json.Sucesso = true;
                json.Objeto = new PreProposta { Id = preProposta.Id, Codigo = preProposta.Codigo, SituacaoProposta = preProposta.SituacaoProposta };
                json.Mensagens.Add(string.Format(GlobalMessages.MsgAvancoEtapaPreProposta, SituacaoProposta.DocsInsuficientesSimplificado.AsString()));
            }
            catch (BusinessRuleException ex)
            {
                CurrentSession().Transaction.Rollback();
                json.Mensagens.AddRange(ex.Errors);
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

    }
}