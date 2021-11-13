using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Integration;
using Tenda.EmpresaVenda.Domain.Integration.Suat;
using Tenda.EmpresaVenda.Domain.Integration.Suat.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Repository.Models;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Portal.Models;
using Tenda.EmpresaVenda.Portal.Models.Application;
using Tenda.EmpresaVenda.Portal.Security;

namespace Tenda.EmpresaVenda.Portal.Controllers
{
    [BaseAuthorize("EVS05")]
    public class ClienteController : BaseController
    {
        private ClienteRepository _clienteRepository { get; set; }
        private EnderecoClienteRepository _enderecoClienteRepository { get; set; }
        private EnderecoEmpresaRepository _enderecoEmpresaRepository { get; set; }
        private FamiliarRepository _familiarRepository { get; set; }
        private BancoRepository _bancoRepository { get; set; }
        private ClienteService _clienteService { get; set; }
        private EnderecoClienteService _enderecoClienteService { get; set; }
        private EnderecoEmpresaService _enderecoEmpresaService { get; set; }
        private CorretorRepository _corretorRepository { get; set; }

        [BaseAuthorize("EVS05", "Visualizar")]
        public ActionResult Index(long? id)
        {
            var model = GetModel(id);
            if (!id.IsEmpty() && (model.Cliente.IsEmpty() || model.Cliente.EmpresaVenda?.Id != SessionAttributes.Current().EmpresaVendaId))
            {
                RouteData.Values.Remove("id");
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [BaseAuthorize("EVS05", "Incluir")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult Incluir(ClienteDTO dto)
        {
            return Salvar(dto, true);
        }

        [BaseAuthorize("EVS05", "Atualizar")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult Atualizar(ClienteDTO dto)
        {
            return Salvar(dto, false);
        }

        [BaseAuthorize("EVS05", "ValidarDadosVenda")]
        public JsonResult ValidarDadosVenda(ClienteDTO dto)
        {
            var json = new JsonResponse();
            var bre = new BusinessRuleException();
            try
            {
                // Validar o cliente
                _clienteService.ValidarDadosVenda(dto.Cliente, bre);
                // Validar o endereço
                _enderecoClienteService.ValidarEndereco(dto.EnderecoCliente, bre);

                bre.ThrowIfHasError();
                json.Objeto = dto;
                json.Mensagens.Add(GlobalMessages.MsgClienteValido);
                json.Sucesso = true;
            }
            catch (BusinessRuleException ex)
            {
                json.Mensagens.AddRange(ex.Errors);
                json.Campos.AddRange(ex.ErrorsFields);
                json.Sucesso = false;
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("EVS05", "ValidarDadosVenda")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult ValidarDadosIntegracao(ClienteDTO dto)
        {
            SuatService service = new SuatService();
            var retorno = new MensagemRetornoPropostaDTO();
            var json = new JsonResponse();
            var bre = new BusinessRuleException();
            try
            {
                var dadosIntegracao = new ClienteSuatDTO().FromModel(dto.Cliente, dto.EnderecoCliente, dto.EnderecoEmpresa);
                var result = service.ValidarCliente(dadosIntegracao);

                if (dto.Cliente.Id.IsEmpty())
                {
                    Validar(bre, dto);
                    bre.ThrowIfHasError();
                    // Define a Empresa de Venda do usuário logado como a do cliente
                    dto.Cliente.EmpresaVenda = new Tenda.Domain.EmpresaVenda.Models.EmpresaVenda { Id = SessionAttributes.Current().EmpresaVendaId };

                    // Salva o cliente
                    _clienteService.Salvar(dto.Cliente, bre);

                    // Salva o endereço do cliente
                    dto.EnderecoCliente.Cliente = dto.Cliente;
                    _enderecoClienteService.Salvar(dto.EnderecoCliente, bre);

                    // Salva o endereço da empresa
                    dto.EnderecoEmpresa.Cliente = dto.Cliente;
                    _enderecoEmpresaService.Salvar(dto.EnderecoEmpresa, bre);
                    if (dto.Familiar.Cliente2.Id.HasValue())
                    {
                        ExluirFamiliar(dto.Cliente.Id, dto.Familiar.Cliente2.Id);
                        dto.Familiar.Cliente1 = dto.Cliente;
                        dto.Familiar.Familiaridade = Tenda.Domain.EmpresaVenda.Enums.TipoFamiliaridade.Conjuge;
                        _familiarRepository.Save(dto.Familiar);
                        AlterarRegimeBens(dto.Familiar.Cliente2.Id, dto.Cliente.RegimeBens);
                    }
                    else if (dto.Cliente.EstadoCivil != TipoEstadoCivil.Casado)
                    {
                        // Caso ao salvar, não seja mais casado, altera os dados do antigo cônjuge para solteiro
                        var familiar = _familiarRepository.BuscarConjugePorCliente(dto.Cliente.Id);
                        if (familiar.HasValue())
                        {
                            Cliente conjuge = familiar.Cliente1.Id == dto.Cliente.Id ? familiar.Cliente2 : familiar.Cliente1;
                            conjuge.EstadoCivil = TipoEstadoCivil.Solteiro;
                            conjuge.RegimeBens = null;
                            _clienteRepository.Save(conjuge);
                            ExluirFamiliar(conjuge.Id, dto.Cliente.Id);
                        }
                    }
                }
                bre.ThrowIfHasError();
                json.Objeto = dto;
                json.Mensagens.AddRange(result.Mensagens);
                json.Sucesso = result.Sucesso;
                return Json(json, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessRuleException ex)
            {
                json.Mensagens.AddRange(ex.Errors);
                json.Campos.AddRange(ex.ErrorsFields);
                json.Sucesso = false;
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ListarClientes(DataSourceRequest request, string nome, string email, string telefone, string cpfCnpj)
        {
            var idEmpresaVenda = SessionAttributes.Current().EmpresaVendaId;
            var idCorretor = SessionAttributes.Current().Corretor.Id;
            var result = _clienteRepository.ListarClientes(nome, email, telefone.OnlyNumber(), cpfCnpj.OnlyNumber(), idEmpresaVenda, idCorretor);
            result = result.Select(x => new Cliente
            {
                Id = x.Id,
                NomeCompleto = x.NomeCompleto,
                CpfCnpj = x.CpfCnpj,
                Email = x.Email,
                TelefoneResidencial = x.TelefoneResidencial,
                TelefoneComercial = x.TelefoneComercial
            });
            return Json(result.ToDataRequest(request), JsonRequestBehavior.AllowGet);
        }


        private JsonResult Salvar(ClienteDTO dto, bool inclusao)
        {
            var json = new JsonResponse();
            var bre = new BusinessRuleException();
            try
            {
                Validar(bre, dto);
                bre.ThrowIfHasError();

                //Define o Corretor que criou esse cliente
                var idCorretor = SessionAttributes.Current().Corretor.Id;
                dto.Cliente.Corretor = idCorretor;

                // Define a Empresa de Venda do usuário logado como a do cliente
                dto.Cliente.EmpresaVenda = new Tenda.Domain.EmpresaVenda.Models.EmpresaVenda { Id = SessionAttributes.Current().EmpresaVendaId };
                // Define a Data de Criação do Cliente
                dto.Cliente.DataCriacao = DateTime.Now;
                // Salva o cliente
                _clienteService.Salvar(dto.Cliente, bre);

                // Salva o endereço do cliente
                dto.EnderecoCliente.Cliente = dto.Cliente;
                _enderecoClienteService.Salvar(dto.EnderecoCliente, bre);

                // Salva o endereço da empresa
                dto.EnderecoEmpresa.Cliente = dto.Cliente;
                _enderecoEmpresaService.Salvar(dto.EnderecoEmpresa, bre);
                if (dto.Familiar.Cliente2.Id.HasValue())
                {
                    ExluirFamiliar(dto.Cliente.Id, dto.Familiar.Cliente2.Id);
                    dto.Familiar.Cliente1 = dto.Cliente;
                    dto.Familiar.Familiaridade = Tenda.Domain.EmpresaVenda.Enums.TipoFamiliaridade.Conjuge;
                    _familiarRepository.Save(dto.Familiar);
                    AlterarRegimeBens(dto.Familiar.Cliente2.Id, dto.Cliente.RegimeBens);
                }
                else if (dto.Cliente.EstadoCivil != TipoEstadoCivil.Casado)
                {
                    // Caso ao salvar, não seja mais casado, altera os dados do antigo cônjuge para solteiro
                    var familiar = _familiarRepository.BuscarConjugePorCliente(dto.Cliente.Id);
                    if (familiar.HasValue())
                    {
                        Cliente conjuge = familiar.Cliente1.Id == dto.Cliente.Id ? familiar.Cliente2 : familiar.Cliente1;
                        conjuge.EstadoCivil = TipoEstadoCivil.Solteiro;
                        conjuge.RegimeBens = null;
                        _clienteRepository.Save(conjuge);
                        ExluirFamiliar(conjuge.Id, dto.Cliente.Id);
                    }
                }
                bre.ThrowIfHasError();
                json.Objeto = RenderRazorViewToString("_Conteudo", dto, false);
                json.Mensagens.Add(string.Format(GlobalMessages.RegistroSalvo, dto.Cliente.ChaveCandidata(), inclusao ? GlobalMessages.Incluido.ToLower() : GlobalMessages.Atualizado.ToLower()));
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

        private void Validar(BusinessRuleException ex, ClienteDTO dto)
        {
            if (dto.Cliente.EstadoCivil == Tenda.Domain.EmpresaVenda.Enums.TipoEstadoCivil.Casado && (!dto.Familiar.Cliente2.Id.HasValue() || !dto.Cliente.RegimeBens.HasValue))
            {
                ex.AddError(GlobalMessages.CampoObrigatorio)
                .WithParam(dto.Familiar.Cliente2.Id.HasValue() ? GlobalMessages.RegimeBens : GlobalMessages.Conjuge)
                .Complete();
            }
            if (dto.Cliente.Id == dto.Familiar.Cliente2.Id && dto.Cliente.Id != 0)
            {
                ex.AddError(GlobalMessages.ErrorConjuge).Complete();
            }
            if (dto.Cliente.Email.HasValue())
            {
                //Solução Provisoria End Point do SUAT não verifica por completo
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match(dto.Cliente.Email);
                if (!match.Success)
                {
                    ex.AddError(GlobalMessages.EmailInvalido).Complete();
                }
            }
        }

        private void AlterarRegimeBens(long idCli, TipoRegimeBens? tipo)
        {
            var temp = _clienteRepository.FindById(idCli);
            temp.RegimeBens = tipo;
            temp.EstadoCivil = TipoEstadoCivil.Casado;
            _clienteRepository.Save(temp);
        }

        private void ExluirFamiliar(long idCli1, long IdCli2)
        {
            if (_familiarRepository.Queryable().Where(x => x.Cliente1.Id == idCli1 || x.Cliente2.Id == idCli1 || x.Cliente1.Id == IdCli2 || x.Cliente2.Id == IdCli2).Any())
            {
                var temp = _familiarRepository.Queryable().Where(x => x.Cliente1.Id == idCli1 || x.Cliente2.Id == idCli1 || x.Cliente1.Id == IdCli2 || x.Cliente2.Id == IdCli2).ToList();
                foreach (Familiar obj in temp)
                {
                    _familiarRepository.Delete(obj);
                }
            }
        }

        private ClienteDTO GetModel(long? id)
        {
            var model = new ClienteDTO();
            model.Cliente = new Cliente();
            model.EnderecoCliente = new EnderecoCliente();
            model.EnderecoEmpresa = new EnderecoEmpresa();
            model.Cliente.PossuiFinanciamento = false;
            if (id.IsEmpty())
                return model;

            model.Cliente = _clienteRepository.FindById(id.Value);

            if (model.Cliente.IsEmpty())
                return new ClienteDTO();

            var corretor = _corretorRepository.FindById(model.Cliente.Corretor);
            if (!corretor.IsNull())
            {
                model.Cliente.NomeCorretor = corretor.Nome;
            }

            var endereco = _enderecoClienteRepository.FindByCliente(id.Value);
            if (!endereco.IsEmpty())
            {
                model.EnderecoCliente = endereco;
                model.EnderecoCliente.Pais = "BR";
            }

            var enderecoEmpresa = _enderecoEmpresaRepository.FindByCliente(id.Value);
            if (!enderecoEmpresa.IsEmpty())
            {
                model.EnderecoEmpresa = enderecoEmpresa;
                model.EnderecoEmpresa.Pais = "BR";
            }

            var familiar = _familiarRepository.Queryable().Where(x => x.Cliente1.Id == id.Value).FirstOrDefault();
            if (!familiar.IsEmpty())
            {
                model.Familiar = familiar;
            }
            else if (_familiarRepository.Queryable().Where(x => x.Cliente2.Id == id.Value).Any())
            {
                model.Familiar = new Familiar();
                familiar = _familiarRepository.Queryable().Where(x => x.Cliente2.Id == id.Value).FirstOrDefault();
                model.Familiar.Cliente1 = familiar.Cliente2;
                model.Familiar.Cliente2 = familiar.Cliente1;
                model.Familiar.Id = familiar.Id;
            }
            return model;
        }

        public ActionResult RenderBancos()
        {
            var results = _bancoRepository.Listar();

            var list = results.OrderBy(x => x.Codigo).Select(x => new SelectListItem
            {
                Text = x.Codigo + " - " + x.Sigla + " - " + x.Nome,
                Value = x.Id.ToString()
            });
            return PartialView("_BancoDropdownList", list);
        }

        public JsonResult MudarCorretor(MudarCorretorDTO model)
        {
            var response = new JsonResponse();

            try
            {
                Validar(model);

                var novoCorretor = _clienteService.MudarCorretor(model);
                response.Mensagens.Add(string.Format(GlobalMessages.ClienteAlteradoNovoProprietario, novoCorretor.Nome));
                response.Sucesso = true;

            }catch(BusinessRuleException ex)
            {
                response.Mensagens.AddRange(ex.Errors);
                response.Sucesso = false;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public void Validar(MudarCorretorDTO model)
        {
            if (model.IdNovoCorretor.IsEmpty())
            {
                throw new BusinessRuleException(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Corretor));
            };

            if (model.IdNovoCorretor == model.IdCorretorAtual)
            {
                throw new BusinessRuleException(GlobalMessages.SelecioneCorretorDiferente);
            }
        }
    }
}
