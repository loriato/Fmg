using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Rest;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.ApiService.Models.Cliente;
using Tenda.EmpresaVenda.ApiService.Models.Util;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Repository.Models;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class ClienteService : BaseService
    {
        private ClienteRepository _clienteRepository { get; set; }
        private EnderecoClienteRepository _enderecoClienteRepository { get; set; }
        private CorretorRepository _corretorRepository { get; set; }
        private FamiliarRepository _familiarRepository { get; set; }
        private EnderecoEmpresaRepository _enderecoEmpresaRepository { get; set; }
        private EnderecoClienteService _enderecoClienteService { get; set; }
        private EnderecoEmpresaService _enderecoEmpresaService { get; set; }
        private ConectaService _conectaService { get; set; }

        public Cliente Salvar(Cliente cliente, BusinessRuleException bre)
        {
            // Remove máscara
            cliente.CpfCnpj = cliente.CpfCnpj.OnlyNumber();
            cliente.TelefoneResidencial = cliente.TelefoneResidencial.OnlyNumber();
            cliente.TelefoneComercial = cliente.TelefoneComercial.OnlyNumber();
            cliente.TelefonePrimeiraReferencia = cliente.TelefonePrimeiraReferencia.OnlyNumber();
            cliente.TelefoneSegundaReferencia = cliente.TelefoneSegundaReferencia.OnlyNumber();
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

            // Realiza as validações de EmpresaVenda
            ValidationResult clieResult = new ClienteValidator(_clienteRepository).Validate(cliente);

            // Verifica se retornou algum erro
            bre.WithFluentValidation(clieResult);
            if (bre.ErrorsFields.Contains("Cliente.TelefoneResidencial"))
            {
                bre.ErrorsFields.Add("Cliente.TelefoneComercial");
                bre.ErrorsFields.Add("Cliente.Email");
            }
            if (clieResult.IsValid)
            {
                _clienteRepository.Save(cliente);
            }
            return cliente;
        }

        public bool ValidarDadosVenda(Cliente cliente, BusinessRuleException bre)
        {
            // Remove máscara
            cliente.CpfCnpj = cliente.CpfCnpj.OnlyNumber();
            if (cliente.Profissao.IsEmpty())
                cliente.Profissao = null;
            // Realiza as validações de EmpresaVenda
            ValidationResult clieResult = new ClienteDadosVendaValidator(_clienteRepository).Validate(cliente);

            // Verifica se retornou algum erro
            bre.WithFluentValidation(clieResult);
            if (bre.ErrorsFields.Contains("Cliente.TelefoneResidencial"))
            {
                bre.ErrorsFields.Add("Cliente.TelefoneComercial");
                bre.ErrorsFields.Add("Cliente.Email");
            }
            if (clieResult.IsValid)
            {
                return true;
            }
            return false;
        }

        public byte[] Exportar(DataSourceRequest request, ClienteExportarDTO filtro)
        {
            var results = _enderecoClienteRepository.ListarClientes(request, filtro);

            ExcelUtil excel = ExcelUtil.NewInstance(20)
                .NewSheet(DateTime.Now.ToString("yyyyMMddHHmmss"))
                .WithHeader(GetHeader());

            foreach (var model in results.records.ToList())
            {
                excel
                    .CreateCellValue(model.Cliente.EmpresaVenda.NomeFantasia).Width(30)
                    .CreateCellValue(model.Estado).Width(20)
                    .CreateCellValue(model.Cliente.NomeCompleto).Width(30)
                    .CreateCellValue(model.Cliente.CpfCnpj).Width(30)
                    .CreateCellValue(model.Cliente.TelefoneResidencial.ToPhoneFormat()).Width(30)
                    .CreateDateTimeCell(model.Cliente.DataCriacao).Width(30)
                    .CreateMoneyCell(model.Cliente.RendaFormal).Width(30)
                    .CreateMoneyCell(model.Cliente.RendaInformal).Width(30)
                    .CreateCellValue(model.Cliente.TipoSexo.ToString()).Width(30)
                    .CreateCellValue(model.Cliente.DataNascimento.HasValue() ? CalcularIdade(model.Cliente.DataNascimento.Value) : 0).Width(30)
                    .CreateCellValue(model.Cliente.EstadoCivil.HasValue() ? model.Cliente.EstadoCivil.Value.ToString() : "").Width(30);

            }
            excel.Close();
            return excel.DownloadFile();
        }

        private string[] GetHeader()
        {
            IList<string> header = new List<string>
            {
                GlobalMessages.EmpresaVenda,
                GlobalMessages.Regional,
                GlobalMessages.Nome,
                GlobalMessages.Cpf,
                GlobalMessages.Telefone,
                GlobalMessages.DataCriacao,
                GlobalMessages.RendaFormal,
                GlobalMessages.RendaInformal,
                GlobalMessages.Sexo,
                GlobalMessages.Idade,
                GlobalMessages.EstadoCivil,
            };
            return header.ToArray();
        }
        private static int CalcularIdade(DateTime date)
        {
            int age = 0;
            age = DateTime.Now.Year - date.Year;
            if (DateTime.Now.DayOfYear < date.DayOfYear)
                age = age - 1;

            return age;
        }

        public string MontarParametroMatrizOferta(long idCliente)
        {
            var bre = new BusinessRuleException();

            var parametro = "&";

            var cliente = _clienteRepository.FindById(idCliente);

            if (cliente.IsEmpty())
            {
                bre.AddError(string.Format(GlobalMessages.ErroRegistroInexistente,GlobalMessages.Cliente)).Complete();
                bre.ThrowIfHasError();
            }

            parametro += "Cpf=" + cliente.CpfCnpj.OnlyNumber();
            parametro += "&NomeCompleto=" + HttpUtility.UrlEncode(cliente.NomeCompleto);

            if (cliente.EstadoCivil.HasValue())
            {
                parametro += "&EstadoCivil=" + cliente.EstadoCivil.AsString();
            }

            if (cliente.TipoSexo.HasValue())
            {
                if (cliente.TipoSexo.Value == TipoSexo.Masculino)
                {
                    parametro += "&Genero=1";
                }
                else if (cliente.TipoSexo.Value == TipoSexo.Feminino)
                {
                    parametro += "&Genero=2";
                }
            }
            if (cliente.DataNascimento.HasValue())
            {
                parametro += "&DataNascimento=" + cliente.DataNascimento.Value.ToString("yyyy-MM-dd");
            }

            parametro += "&Telefone=" + cliente.TelefoneResidencial;
            parametro += "&Email=" + cliente.Email;

            var enderecoCliente = _enderecoClienteRepository.FindByCliente(cliente.Id);

            if (enderecoCliente.HasValue()) {
                parametro += "&Cep=" + enderecoCliente.Cep;
            }

            parametro += "&PossuiFilhos=" + cliente.QuantidadeFilhos.HasValue();

            if (cliente.QuantidadeFilhos.HasValue())
            {
                parametro += "&QtdFilhos=" + cliente.QuantidadeFilhos;
            }

            if (cliente.TipoResidencia.HasValue())
            {
                parametro += "&TipoResidencia=" + cliente.TipoResidencia.AsString();
            }

            var mesesFgts = cliente.MesesFGTS.HasValue() ? (cliente.MesesFGTS.Value >= 36 ? true : false) : false;
            parametro += "&PossuiTresAnosFgts=" + mesesFgts.ToString();

            parametro += "&Fgts=" + cliente.FGTS.ToString().Replace(',', '.');

            if (cliente.RendaMensal.HasValue())
            {
                parametro += "&RendaMensal=" + cliente.RendaMensal.ToString().Replace(',', '.');
            }

            return parametro;
        }

        public EntityDto MudarCorretor(MudarCorretorDTO dto)
        {
            var cliente = _clienteRepository.FindById(dto.IdCliente);
            var novoCorretor = _corretorRepository.FindById(dto.IdNovoCorretor);

            if (novoCorretor.IsNull())
            {
                throw new BusinessRuleException(GlobalMessages.NovoCorretorNaoEncontrado);
            }

            cliente.Corretor = novoCorretor.Id;
            _clienteRepository.Save(cliente);
            _session.Flush();

            var obj = new EntityDto()
            {
                Nome = novoCorretor.Nome
            };

            return obj;
        }

        public ClienteDto MontarClienteDto(long idCliente)
        {
            var apiEx = new ApiException();

            var clienteDto = new ClienteDto();

            var cliente = _clienteRepository.FindById(idCliente);

            if (cliente.IsEmpty())
            {
                return clienteDto;
            }

            clienteDto.IdCliente = idCliente;
            clienteDto.FromDomain(cliente);

            var enderecoCliente = _enderecoClienteRepository.BuscarSomenteEndereco(idCliente);

            if (enderecoCliente.HasValue())
            {
                clienteDto.EnderecoClienteDto.FromDomain(enderecoCliente);
            }

            var familiar = _familiarRepository.Queryable().Where(x => x.Cliente1.Id == idCliente).FirstOrDefault();

            if (familiar.HasValue())
            {
                clienteDto.DadosPessoaisDto.FamiliarDto.FromDomain(familiar);
            }
            else
            {
                familiar = _familiarRepository.Queryable().Where(x => x.Cliente2.Id == idCliente).FirstOrDefault();

                if (familiar.HasValue())
                {
                    familiar.Cliente2 = familiar.Cliente1;
                    familiar.Cliente1=cliente;

                    clienteDto.DadosPessoaisDto.FamiliarDto.FromDomain(familiar);
                }
            }

            var enderecoEmpresa = _enderecoEmpresaRepository.BuscarSomenteEndereco(idCliente);

            if (enderecoEmpresa.HasValue())
            {
                clienteDto.DadosProfissionaisDto.EnderecoEmpresaDto.FromDomain(enderecoEmpresa);
            }

            return clienteDto;
        }
        public Cliente SalvarCliente(ClienteDto clienteDto)
        {
            GenericFileLogUtil.DevLogWithDateOnBegin("Salvar cliente");
            var bre = new BusinessRuleException();

            ValidarCliente(clienteDto);

            var cliente = clienteDto.ToCliente();

            //Salvar o cliente
            cliente = Salvar(cliente, bre);

            bre.ThrowIfHasError();

            if (clienteDto.NovoCliente)
            {
                cliente = _conectaService.VincularNovoClienteConecta(clienteDto, cliente);
            }

            _clienteRepository.Save(cliente);

            clienteDto.IdCliente = cliente.Id;

            //Salvar Endereço Cliente
            var enderecoCliente = clienteDto.ToEnderecoCliente();
            enderecoCliente.Cliente = cliente;
            _enderecoClienteService.Salvar(enderecoCliente, bre);

            //Salvar endereço empresa
            var enderecoEmpresa = clienteDto.ToEnderecoEmpresa();
            enderecoEmpresa.Cliente = cliente;
            _enderecoEmpresaService.Salvar(enderecoEmpresa, bre);

            //Salvar Familiar
            SalvarFamiliar(clienteDto);
            return cliente;
        }
        private void ValidarCliente(ClienteDto dto)
        {
            var apiEx = new ApiException();

            if (dto.DadosPessoaisDto.EstadoCivil == TipoEstadoCivil.Casado && (!dto.DadosPessoaisDto.FamiliarDto.Cliente2.Id.HasValue() || !dto.DadosPessoaisDto.RegimeBens.HasValue))
            {
                apiEx.AddError(string.Format(GlobalMessages.CampoObrigatorio,dto.DadosPessoaisDto.FamiliarDto.Cliente2.Id.HasValue() ? GlobalMessages.RegimeBens : GlobalMessages.Conjuge));
            }
            if (dto.IdCliente == dto.DadosPessoaisDto.FamiliarDto.Cliente2.Id && dto.IdCliente != 0)
            {
                apiEx.AddError(GlobalMessages.ErrorConjuge);
            }
            if (dto.InformacoesGeraisDto.Email.HasValue())
            {
                //Solução Provisoria End Point do SUAT não verifica por completo
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match(dto.InformacoesGeraisDto.Email);
                if (!match.Success)
                {
                    apiEx.AddError(GlobalMessages.EmailInvalido);
                }
            }

            apiEx.ThrowIfHasError();
        }
        private void SalvarFamiliar(ClienteDto clienteDto)
        {
            if (clienteDto.DadosPessoaisDto.FamiliarDto.Cliente2.Id.HasValue())
            {
                ExluirFamiliar(clienteDto.IdCliente, clienteDto.DadosPessoaisDto.FamiliarDto.Cliente2.Id);

                var familiar = new Familiar();
                familiar.Cliente1 = new Cliente { Id = clienteDto.IdCliente };
                familiar.Cliente2 = new Cliente { Id = clienteDto.DadosPessoaisDto.FamiliarDto.Cliente2.Id };
                familiar.Familiaridade = TipoFamiliaridade.Conjuge;

                _familiarRepository.Save(familiar);

                AlterarRegimeBens(clienteDto.DadosPessoaisDto.FamiliarDto.Cliente2.Id, clienteDto.DadosPessoaisDto.RegimeBens);
            }
            else if (clienteDto.DadosPessoaisDto.EstadoCivil != TipoEstadoCivil.Casado)
            {
                // Caso ao salvar, não seja mais casado, altera os dados do antigo cônjuge para solteiro
                var familiar = _familiarRepository.BuscarConjugePorCliente(clienteDto.IdCliente);
                if (familiar.HasValue())
                {
                    Cliente conjuge = familiar.Cliente1.Id == clienteDto.IdCliente ? familiar.Cliente2 : familiar.Cliente1;
                    conjuge.EstadoCivil = TipoEstadoCivil.Solteiro;
                    conjuge.RegimeBens = null;
                    _clienteRepository.Save(conjuge);
                    ExluirFamiliar(conjuge.Id, clienteDto.IdCliente);
                }
            }
        }
        private void ExluirFamiliar(long idCli1, long IdCli2)
        {
            if (_familiarRepository.Queryable().Where(x => x.Cliente1.Id == idCli1 || x.Cliente2.Id == idCli1 || x.Cliente1.Id == IdCli2 || x.Cliente2.Id == IdCli2).Any())
            {
                var temp = _familiarRepository.Queryable().Where(x => x.Cliente1.Id == idCli1 || x.Cliente2.Id == idCli1 || x.Cliente1.Id == IdCli2 || x.Cliente2.Id == IdCli2).ToList();
                foreach (var familiar in temp)
                {
                    _familiarRepository.Delete(familiar);
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

    }
}
