using Europa.Cryptography;
using Europa.Extensions;
using Europa.Resources;
using Europa.Rest;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Tenda.Domain.Core.Models;
using Tenda.Domain.Core.Services;
using Tenda.Domain.Core.Services.Models;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Repository;
using Tenda.Domain.Security.Services.Models;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.ApiService.Models.Cliente;
using Tenda.EmpresaVenda.Domain.Integration.Conecta;
using Tenda.EmpresaVenda.Domain.Integration.Conecta.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class ConectaService:BaseService
    {
        public const string cpfConecta = "cpf";
        public const string cepConecta = "endereco";
        public const string dataNascimentoConecta = "datanascimento";
        public const string dtNascimento = "dtnascimento";
        public const string bairroConecta = "enderecobairro";
        public const string telefoneAdicionalConecta = "telefoneadicional";
        public const string complementoEnderecoConecta = "enderecocomplemento";
        public const string numeroEnderecoConecta = "endereconumero";
        public const string sexoConecta = "sexo";

        private UsuarioPortalRepository _usuarioPortalRepository { get; set; }
        private ClienteRepository _clienteRepository { get; set; }
        private ConectaApiService _conectaApiService { get; set; }
        private CorretorRepository _corretorRepository { get; set; }
        private LeadConectaDtoValidator _leadConectaDtoValidator { get; set; }
        private ClienteLeadConectaValidator _clienteLeadConectaValidator { get; set; }
        private EnderecoClienteRepository _enderecoClienteRepository { get; set; }
        public LoginDto ParametroAcesso(UsuarioPortal usuario)
        {
            var loginDto = new LoginDto
            {
                Username = SslAes256.EncryptString(usuario.Login),
                Password = SslAes256.EncryptString(usuario.Senha)
            };

            return loginDto;
        }

        public LoginDto ParametroAcesso(string login)
        {
            var apiEx = new ApiException();

            var usuario = _usuarioPortalRepository.UsuarioPorLogin(login);

            if (usuario.IsEmpty())
            {
                apiEx.AddError(string.Format("Usuário não autorizado"));
                apiEx.ThrowIfHasError();
            }

            usuario.Senha = usuario.Senha.IsEmpty() ? "AD_HOUSE" : usuario.Senha;

            var loginDto = new LoginDto
            {
                Username = SslAes256.EncryptString(usuario.Login),
                Password = SslAes256.EncryptString(usuario.Senha)
            };

            return loginDto;
        }

        public string UrlConectaKanban(string tokenAcesso)
        {
            var apiEx = new ApiException();

            if (tokenAcesso.IsEmpty() || tokenAcesso.Equals("ERROR"))
            {
                apiEx.AddError("Usuário sem acesso ao conecta, realize login novamente");
                apiEx.ThrowIfHasError();
            }

            var url = ProjectProperties.ConectaUrlBase+"integracao/landing?token="+ HttpUtility.UrlEncode(tokenAcesso)+ "&returnUrl=kanban"; ;

            return url;
        }

        public LeadConectaDto MontarLeadConecta(ClienteDto clienteDto)
        {
            var apiEx = new ApiException();

            var cliente = clienteDto.ToCliente();

            if (cliente.IsEmpty())
            {
                cliente = _clienteRepository.FindById(clienteDto.IdCliente);
            }

            if (cliente.IsEmpty())
            {
                apiEx.AddError(string.Format(GlobalMessages.DadoInexistente, GlobalMessages.Cliente));
                apiEx.ThrowIfHasError();
            }

            var leadConectaDto = new LeadConectaDto();

            leadConectaDto.Nome = cliente.NomeCompleto;
            leadConectaDto.Email = cliente.Email;
            leadConectaDto.Telefone = cliente.TelefoneResidencial.OnlyNumber();
            leadConectaDto.Cpf = cliente.CpfCnpj.OnlyNumber();
            leadConectaDto.OrigemContato = clienteDto.OrigemContato;

            return leadConectaDto;
        }

        public Cliente VincularLeadConectaClienteEv(ClienteDto clienteDto)
        {
            var apiEx = new ApiException();

            var cliente = _clienteRepository.FindById(clienteDto.IdCliente);

            if (cliente.IsEmpty())
            {
                cliente = clienteDto.ToCliente();
            }

            if (cliente.IsEmpty())
            {
                apiEx.AddError(string.Format(GlobalMessages.DadoInexistente, GlobalMessages.Cliente));
                apiEx.ThrowIfHasError();
            }

            cliente.UuidLead = clienteDto.Uuid;
            cliente.TelefoneLead = clienteDto.TelefoneLead.IsEmpty()?cliente.TelefoneResidencial.OnlyNumber() : clienteDto.TelefoneLead.OnlyNumber();
            cliente.NomeLead = clienteDto.TelefoneLead.IsEmpty() ? cliente.NomeCompleto : clienteDto.NomeLead;

            _clienteRepository.Save(cliente);

            return cliente;
        }

        public Cliente VincularNovoClienteConecta(ClienteDto clienteDto,Cliente cliente)
        {
            if (clienteDto.InformacoesGeraisDto.UuidLead.IsEmpty())
            {
                var leadConectaDto = new LeadConectaDto();

                leadConectaDto.Nome = cliente.NomeCompleto;
                leadConectaDto.Email = cliente.Email;
                leadConectaDto.Telefone = cliente.TelefoneResidencial.OnlyNumber();
                leadConectaDto.Cpf = cliente.CpfCnpj.OnlyNumber();
                leadConectaDto.OrigemContato = clienteDto.OrigemContato;

                leadConectaDto.Vendedor = clienteDto.Vendedor;

                //gera o lead dentro do conecta
                var response = _conectaApiService.CriarLeadConecta(leadConectaDto);

                //referência do lead no conecta
                clienteDto.Uuid = response.Data.ToString();

                //vincula o lead conecta ao cliente no portal
                cliente.UuidLead = clienteDto.Uuid;
                cliente.TelefoneLead = clienteDto.TelefoneLead.IsEmpty() ? cliente.TelefoneResidencial.OnlyNumber() : clienteDto.TelefoneLead.OnlyNumber();
                cliente.NomeLead = clienteDto.TelefoneLead.IsEmpty() ? cliente.NomeCompleto : clienteDto.NomeLead;

                var atributos = MontarAtributosLead(clienteDto);

                response = _conectaApiService.AtualizarAtributosLead(clienteDto.Uuid, atributos);
                
            }
            return cliente;
        }

        public Cliente IntegrarLeadEmpresaVenda(LeadConectaDto leadConectaDto)
        {
            var apiEx = new ApiException();

            var corretor = _corretorRepository.Queryable()
                .Where(x => x.Usuario.Login.Equals(leadConectaDto.Vendedor))
                .SingleOrDefault();

            if (corretor.IsEmpty())
            {
                apiEx.AddError(string.Format("Vendedor {0} inexistente", leadConectaDto.Vendedor));
                apiEx.ThrowIfHasError();
            }

            var empresaVenda = corretor.EmpresaVenda;

            if (empresaVenda.IsEmpty())
            {
                apiEx.AddError(string.Format("Vendedor {0} não possui Loja vinculada",leadConectaDto.Vendedor));
                apiEx.ThrowIfHasError();
            }
            if (empresaVenda.Situacao != Situacao.Ativo)
            {
                apiEx.AddError(string.Format("Loja {0} {1}",empresaVenda.NomeFantasia,empresaVenda.Situacao.AsString()));
                apiEx.ThrowIfHasError();
            }

            leadConectaDto = ValidarAtributosDinamicos(leadConectaDto);
                        
            var validate = _leadConectaDtoValidator.Validate(leadConectaDto);
            apiEx.WithFluentValidation(validate);
            apiEx.ThrowIfHasError();

            var cliente = _clienteRepository.Queryable()
                .Where(x => x.CpfCnpj.Equals(leadConectaDto.Cpf.OnlyNumber()))
                .Where(x => x.EmpresaVenda.Id == empresaVenda.Id)
                .Where(x => x.Corretor == corretor.Id)
                .SingleOrDefault();

            if (cliente.HasValue())
            {
                if (cliente.UuidLead.HasValue() && cliente.UuidLead.Equals(leadConectaDto.Uuid))
                {
                    apiEx.AddError(string.Format("O lead já está vinculado a este cliente"));
                    apiEx.ThrowIfHasError();
                }
            }

            if (cliente.IsEmpty())
            {
                cliente = new Cliente();

                cliente.Corretor = corretor.Id;

                cliente.EmpresaVenda = empresaVenda;

                cliente.NomeCompleto = leadConectaDto.Nome;
                cliente.TelefoneResidencial = leadConectaDto.Telefone.OnlyNumber();
                cliente.Email = leadConectaDto.Email;
                cliente.CpfCnpj = leadConectaDto.Cpf.OnlyNumber();

                //lead
                cliente.UuidLead = leadConectaDto.Uuid;
                cliente.TelefoneLead = leadConectaDto.TelefoneLead.OnlyNumber();
                cliente.NomeLead = leadConectaDto.NomeLead;

                //Cliente
                cliente.PossuiVeiculo = false;
                cliente.PossuiContaBanco = false;
                cliente.PossuiCartaoCredito = false;
                cliente.PossuiComprometimentoFinanceiro = false;

                if (cliente.Profissao.IsEmpty())
                {
                    cliente.Profissao = null;
                }
                if (cliente.Banco.IsEmpty())
                {
                    cliente.Banco = null;
                }

                cliente.TipoPessoa = TipoPessoa.Fisica;

                //atributos dinamicos
                cliente.DataNascimento = leadConectaDto.DataNascimento;
                cliente.TelefoneComercial = leadConectaDto.TelefoneAdicional.OnlyNumber();
                cliente.TipoSexo = leadConectaDto.Sexo;

                validate = _clienteLeadConectaValidator.Validate(cliente);
                apiEx.WithFluentValidation(validate);
                apiEx.ThrowIfHasError();

                if (leadConectaDto.Cep.HasValue())
                {
                    var endereco = CepService.ConsultaCEPWS(leadConectaDto.Cep);

                    if (endereco.HasValue())
                    {
                        _clienteRepository.Save(cliente);

                        var enderecoCliente = NormalizarEndereco(endereco);

                        if (leadConectaDto.Complemento.HasValue())
                        {
                            enderecoCliente.Complemento = leadConectaDto.Complemento;
                        }

                        if (leadConectaDto.Numero.HasValue())
                        {
                            enderecoCliente.Numero = leadConectaDto.Numero;
                        }

                        enderecoCliente.Cliente = cliente;

                        _enderecoClienteRepository.Save(enderecoCliente);
                    }
                }

            }
            else
            {
                cliente.NomeLead = leadConectaDto.NomeLead;
                cliente.TelefoneLead = leadConectaDto.TelefoneLead.OnlyNumber();
            }

            cliente.UuidLead = leadConectaDto.Uuid;              

            _clienteRepository.Save(cliente);

            return cliente;
        }

        public AtributosDinamicosLeadDto MontarAtributosLead(ClienteDto clienteDto)
        {
            var cliente = clienteDto.ToCliente();
            var novo = new AtributosDinamicosLeadDto();

            novo.Atributos.GetOrAdd(cpfConecta, cliente.CpfCnpj.ToCPFFormat());

            if (cliente.DataNascimento.HasValue())
            {
                novo.Atributos.GetOrAdd(dataNascimentoConecta, cliente.DataNascimento.Value.ToString("dd/MM/yyyy"));
                novo.Atributos.GetOrAdd(dtNascimento, cliente.DataNascimento.Value.ToString("dd/MM/yyyy"));
            }

            if (cliente.TelefoneComercial.HasValue())
            {
                novo.Atributos.GetOrAdd(telefoneAdicionalConecta, cliente.TelefoneComercial.OnlyNumber());
            }

            if (cliente.TipoSexo.HasValue())
            {
                novo.Atributos.GetOrAdd(sexoConecta, cliente.TipoSexo.AsString());
            }

            var enderecoCliente = clienteDto.ToEnderecoCliente();

            if (enderecoCliente.HasValue())
            {
                if (enderecoCliente.Cep.HasValue())
                {
                    novo.Atributos.GetOrAdd(cepConecta, enderecoCliente.Cep.ToCEPFormat());
                }

                if (enderecoCliente.Bairro.HasValue())
                {
                    novo.Atributos.GetOrAdd(bairroConecta, enderecoCliente.Bairro);
                }

                if (enderecoCliente.Complemento.HasValue())
                {
                    novo.Atributos.GetOrAdd(complementoEnderecoConecta, enderecoCliente.Complemento);
                }

                if (enderecoCliente.Numero.HasValue())
                {
                    novo.Atributos.GetOrAdd(numeroEnderecoConecta, enderecoCliente.Numero);
                }
            }

            return novo;
        }
        public LeadConectaResponseDto BuscarLeadConecta(FiltroLeadConectaDto filtro)
        {
            var apiEx = new ApiException();

            if (filtro.Telefone.IsEmpty())
            {
                apiEx.AddError(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.TelefoneLead));
                apiEx.ThrowIfHasError();
            }
            filtro.Telefone = filtro.Telefone.OnlyNumber();
            //if (filtro.NomeCompleto.IsEmpty())
            //{
            //    apiEx.AddError(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.NomeCompleto));
            //    apiEx.ThrowIfHasError();
            //}            
            var result = _conectaApiService.BuscarLeadConecta(filtro);
            return result;
        }
        public List<LeadConectaResponseDto> ListarLeadConectaNomeCompleto(FiltroLeadConectaDto filtro)
        {
            var apiEx = new ApiException();
            
            if (filtro.Nome.IsEmpty())
            {
                apiEx.AddError(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.NomeCompleto));
                apiEx.ThrowIfHasError();
            }
            filtro.Nome = filtro.Nome.Trim();
            var result = _conectaApiService.ListarLeadConectaNomeCompleto(filtro);
            return result;
        }
        public LeadConectaDto ValidarAtributosDinamicos(LeadConectaDto leadConectaDto)
        {
            if (leadConectaDto.Atributos.IsEmpty())
            {
                return leadConectaDto;
            }

            var atributosClientePortalHouse = ProjectProperties.AtributosClientePortalHouse;
            var cultureInfo = new CultureInfo("pt-BR");

            //preenchendo atributos de respostas apenas com os dados requisitados no parametro e existente no lead
            foreach (var field in atributosClientePortalHouse)
            {
                if (!leadConectaDto.Atributos.ContainsKey(field) || leadConectaDto.Atributos[field] == null) continue;

                if (field.Equals(cpfConecta))
                {
                    leadConectaDto.Cpf = leadConectaDto.Atributos[field];
                }else if (field.Equals(cepConecta))
                {
                    leadConectaDto.Cep = leadConectaDto.Atributos[field];
                }else if (field.Equals(dataNascimentoConecta))
                {
                    leadConectaDto.DataNascimento= DateTime.Parse(leadConectaDto.Atributos[field], cultureInfo);
                }
                else if (field.Equals(dtNascimento))
                {                    
                    leadConectaDto.DataNascimento = DateTime.Parse(leadConectaDto.Atributos[field],cultureInfo);
                }
                else if (field.Equals(bairroConecta))
                {
                    leadConectaDto.Bairro = leadConectaDto.Atributos[field];
                }
                else if (field.Equals(telefoneAdicionalConecta))
                {
                    leadConectaDto.TelefoneAdicional = leadConectaDto.Atributos[field];
                }else if (field.Equals(complementoEnderecoConecta))
                {
                    leadConectaDto.Complemento = leadConectaDto.Atributos[field];
                }else if (field.Equals(numeroEnderecoConecta))
                {
                    leadConectaDto.Numero = leadConectaDto.Atributos[field];
                }else if (field.Equals(sexoConecta))
                {
                    leadConectaDto.Sexo = (TipoSexo)Enum.Parse(typeof(TipoSexo), leadConectaDto.Atributos[field], true);
                }

            }

            return leadConectaDto;
        }
        public EnderecoCliente NormalizarEndereco(CepDTO endereco)
        {
            var enderecoCliente = new EnderecoCliente();

            enderecoCliente.Cep = endereco.cep.OnlyNumber();
            enderecoCliente.Logradouro = endereco.logradouroAbrev;
            enderecoCliente.Bairro = endereco.bairro;
            enderecoCliente.Cidade = endereco.localidade;
            enderecoCliente.Estado = endereco.uf;

            return enderecoCliente;
        }
    }
}
