using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Integration;
using Tenda.EmpresaVenda.Domain.Integration.Suat;
using Tenda.EmpresaVenda.Domain.Integration.Suat.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Repository.Models;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Web.Models;
using Tenda.EmpresaVenda.Web.Models.Application;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("EVS05")]
    public class ClienteController : BaseController
    {
        private FamiliarRepository _familiarRepository { get; set; }
        private ClienteRepository _clienteRepository { get; set; }
        private ClienteService _clienteService { get; set; }
        private EnderecoClienteRepository _enderecoClienteRepository { get; set; }
        private EnderecoEmpresaRepository _enderecoEmpresaRepository { get; set; }
        private BancoRepository _bancoRepository { get; set; }
        private CorretorRepository _corretorRepository { get; set; }

        // GET: Cliente
        [BaseAuthorize("EVS05", "Visualizar")]
        public ActionResult Index(long? id)
        {
            var model = GetModel(id);
            if (!id.IsEmpty() && model.Cliente.IsEmpty())
            {
                RouteData.Values.Remove("id");
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult ListarClientes(DataSourceRequest request, string nome, string email, string telefone, string cpfCnpj)
        {
            var result = _clienteRepository.ListarClientes(nome, email, telefone.OnlyNumber(), cpfCnpj.OnlyNumber(), null,null);
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

        [HttpPost]
        [BaseAuthorize("EVS05", "ValidarDadosIntegracao")]
        public JsonResult ValidarDadosIntegracao(long idCliente)
        {
            SuatService service = new SuatService();
            var dto = FromClienteDTO(idCliente);
            var retorno = new MensagemRetornoPropostaDTO();
            try
            {
                var result = service.ValidarCliente(dto);
                retorno.Mensagens.AddRange(result.Mensagens);
                retorno.Sucesso = result.Sucesso;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessRuleException ex)
            {
                retorno.Mensagens.AddRange(ex.Errors);
                retorno.Campos.AddRange(ex.ErrorsFields);
                retorno.Sucesso = false;
            }
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("EVS05", "IncluirClienteSUAT")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult IncluirClienteSUAT(long idCliente)
        {
            SuatService service = new SuatService();
            var dto = FromClienteDTO(idCliente);
            var retorno = new MensagemRetornoPropostaDTO();
            try
            {
                var result = service.IncluirCliente(dto);
                var clienteRetorno = result.Objeto;
                if (!clienteRetorno.IsEmpty() && !clienteRetorno.IdSuat.IsEmpty())
                {
                    var cliente = _clienteRepository.FindById(clienteRetorno.Id);
                    cliente.IdSuat = clienteRetorno.IdSuat;
                    _clienteRepository.Save(cliente);
                }
                retorno.Mensagens.AddRange(result.Mensagens);
                retorno.Sucesso = result.Sucesso;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessRuleException ex)
            {
                retorno.Mensagens.AddRange(ex.Errors);
                retorno.Campos.AddRange(ex.ErrorsFields);
                retorno.Sucesso = false;
                GenericFileLogUtil.DevLogWithDateOnBegin(
                    string.Format("Cliente: {0} ID: {1} - Telefone Residencial: {2}, Telefone Comercial: {3} - CPF: {4} Erro na integração do cliente",
                    dto.NomeCompleto.ToString(), dto.Id, dto.TelefoneResidencial, dto.TelefoneComercial, dto.CpfCnpj.ToString()));
            }
            return Json(retorno, JsonRequestBehavior.AllowGet);
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

        // Transforma o ClienteDTO (da tela) em ClienteSuatDTO (que a integração utiliza)
        private ClienteSuatDTO FromClienteDTO(long idCliente)
        {
            var cliente = _clienteRepository.FindById(idCliente);
            var enderecoCliente = _enderecoClienteRepository.FindByCliente(idCliente);
            if (enderecoCliente != null)
            {
                enderecoCliente.Cliente = null;
            }
            var enderecoEmpresa = _enderecoEmpresaRepository.FindByCliente(idCliente);
            if (enderecoEmpresa != null)
            {
                enderecoEmpresa.Cliente = null;
            }
            var familiar = _familiarRepository.BuscarConjugePorCliente(idCliente);
            cliente.EmpresaVenda = null;
            ClienteSuatDTO clienteSuatDto = new ClienteSuatDTO().FromModel(cliente, enderecoCliente, enderecoEmpresa);
            if (!familiar.IsEmpty())
            {
                clienteSuatDto.IdConjuge = familiar.Cliente1.Id == idCliente ? familiar.Cliente2.IdSuat : familiar.Cliente1.IdSuat;
            }
            return clienteSuatDto;
        }

        [HttpPost]
        [BaseAuthorize("EVS05", "BuscarIdSapCliente")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult BuscarIdSapCliente(long idCliente)
        {
            JsonResponse res = new JsonResponse();
            SuatService service = new SuatService();
            var idSuat = new List<long>();

            try
            {
                res.Sucesso = false;
                var cliente = _clienteRepository.FindById(idCliente);

                if (cliente.IdSuat.IsEmpty())
                {
                    res.Mensagens.Add(GlobalMessages.ClienteSemIdSap);
                }
                else
                {
                    idSuat.Add(cliente.IdSuat);
                     var idSap = service.BuscarIdsSapClientes(idSuat, null).Select(x => x.IdSap).FirstOrDefault();

                    if (idSap.IsNull())
                    {
                        res.Mensagens.Add(GlobalMessages.ClienteSemIdSap);
                    }
                    else
                    {
                        cliente.IdSap = idSap;
                        _clienteRepository.Save(cliente);

                        res.Sucesso = true;
                        res.Objeto = idSap;
                        res.Mensagens.Add(GlobalMessages.IdSapAtualizado);
                    }

                }
            }
            catch (Exception e)
            {
                res.Sucesso = false;
                res.Mensagens.Add(string.Format("Erro ao buscar IdSap do Cliente: {0}", e.Message));
            }

            return Json(res, JsonRequestBehavior.AllowGet);

        }

        [BaseAuthorize("EVS05", "Exportar")]
        public FileContentResult Exportar(DataSourceRequest request, ClienteExportarDTO filtro)
        {
            var modifiedRequest = request;
            modifiedRequest.start = 0;
            modifiedRequest.pageSize = 0;
            byte[] file = _clienteService.Exportar(modifiedRequest, filtro);
            string nomeArquivo = GlobalMessages.Clientes;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(file, MimeMappingWrapper.Xlsx, $"{nomeArquivo}_{date}.xlsx");
        }
    }
}