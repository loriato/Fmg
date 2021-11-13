using Europa.Extensions;
using System;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.ApiService.Models.AgenteVenda;

namespace Tenda.EmpresaVenda.ApiService.Models.Cliente
{
    public class ClienteDto
    {
        //Refatorando tela de Cliente

        //Informações Gerais
        public InformacoesGeraisDto InformacoesGeraisDto { get; set; }

        //Endereço Cliente
        public EnderecoDto EnderecoClienteDto { get; set; }

        //Dados Pessoais
        public DadosPessoaisDto DadosPessoaisDto { get; set; }

        //Dados Profissionais
        public DadosProfissionaisDto DadosProfissionaisDto { get; set; }

        //Dados Profissionais
        public DadosFinanceirosDto DadosFinanceirosDto { get; set; }

        //Referências
        public ReferenciasDto ReferenciasDto { get; set; }

        //Agente de Venda
        public AgenteVendaDto AgenteVendaDto { get; set; }

        //Usados no simulador
        public string CpfCnpj { get; set; }
        public string Nome { get; set; }
        public string TelefoneResidencial { get; set; }
        public string TelefoneCelular { get; set; }
        public string EmailPrincipal { get; set; }
        public TipoPessoa TipoPessoa { get; set; }
        public DateTime DataNascimento { get; set; }
        public TipoEstadoCivil EstadoCivil { get; set; }
        public TipoSexo Genero { get; set; }
        public string Cep { get; set; }

        //integração com o conecta
        public long IdCliente { get; set; }
        public string OrigemContato { get; set; }
        public string Uuid { get; set; }
        public string NomeLead { get; set; }
        public string TelefoneLead { get; set; }
        public string Vendedor { get; set; }
        public bool NovoCliente { get; set; }
        public ClienteDto()
        {
            InformacoesGeraisDto = new InformacoesGeraisDto();
            EnderecoClienteDto = new EnderecoDto();
            DadosPessoaisDto = new DadosPessoaisDto();
            DadosProfissionaisDto = new DadosProfissionaisDto();
            DadosFinanceirosDto = new DadosFinanceirosDto();
            ReferenciasDto = new ReferenciasDto();
            AgenteVendaDto = new AgenteVendaDto();
        }

        public Tenda.Domain.EmpresaVenda.Models.Cliente ToCliente()
        {
            var cliente = new Tenda.Domain.EmpresaVenda.Models.Cliente();

            cliente.Id = IdCliente;

            //Informações Gerais
            cliente.NomeCompleto = InformacoesGeraisDto.NomeCompleto;
            cliente.CpfCnpj = InformacoesGeraisDto.CpfCnpj.OnlyNumber();
            cliente.TipoSexo = InformacoesGeraisDto.TipoSexo;
            cliente.TelefoneResidencial = InformacoesGeraisDto.TelefoneResidencial;
            cliente.TelefoneComercial = InformacoesGeraisDto.TelefoneComercial;
            cliente.Email = InformacoesGeraisDto.Email;
            cliente.UuidLead = InformacoesGeraisDto.UuidLead;
            cliente.TelefoneLead = InformacoesGeraisDto.TelefoneLead;
            cliente.NomeLead = InformacoesGeraisDto.NomeLead;

            //Dados Pessoais
            cliente.DataNascimento = DadosPessoaisDto.DataNascimento;
            cliente.QuantidadeFilhos = DadosPessoaisDto.QuantidadeFilhos;
            cliente.EstadoCivil = DadosPessoaisDto.EstadoCivil;
            cliente.RegimeBens = DadosPessoaisDto.RegimeBens;

            if (DadosPessoaisDto.Profissao.HasValue()&&DadosPessoaisDto.Profissao.Id.HasValue())
            {
                cliente.Profissao = new Profissao { Id = DadosPessoaisDto.Profissao.Id };
            }

            cliente.NumeroDocumento = DadosPessoaisDto.NumeroDocumento;
            cliente.OrgaoEmissor = DadosPessoaisDto.OrgaoEmissor;
            cliente.EstadoEmissor = DadosPessoaisDto.EstadoEmissor;
            cliente.DataEmissao = DadosPessoaisDto.DataEmissao;
            cliente.Nacionalidade = DadosPessoaisDto.Nacionalidade;
            cliente.Naturalidade = DadosPessoaisDto.Naturalidade;
            cliente.Filiacao = DadosPessoaisDto.Filiacao;
            cliente.TipoResidencia = DadosPessoaisDto.TipoResidencia;
            cliente.Cargo = DadosPessoaisDto.Cargo;
            cliente.Escolaridade = DadosPessoaisDto.Escolaridade;
            cliente.TipoDocumento = DadosPessoaisDto.TipoDocumento;

            //Dados Financeiros
            cliente.TipoRenda = DadosFinanceirosDto.TipoRenda;
            cliente.RendaFormal = DadosFinanceirosDto.RendaFormal;
            cliente.RendaInformal = DadosFinanceirosDto.RendaInformal;
            cliente.RendaMensal = DadosFinanceirosDto.RendaMensal;
            cliente.FGTS = DadosFinanceirosDto.FGTS;
            cliente.MesesFGTS = DadosFinanceirosDto.MesesFGTS;

            //Dados Profissionais
            cliente.Empresa = DadosProfissionaisDto.Empresa;
            cliente.TempoDeEmpresa = DadosProfissionaisDto.TempoDeEmpresa;
            cliente.DataAdmissao = DadosProfissionaisDto.DataAdmissao;

            //Referências
            cliente.PrimeiraReferencia = ReferenciasDto.PrimeiraReferencia;
            cliente.TelefonePrimeiraReferencia = ReferenciasDto.TelefonePrimeiraReferencia;
            cliente.SegundaReferencia = ReferenciasDto.SegundaReferencia;
            cliente.TelefoneSegundaReferencia = ReferenciasDto.TelefoneSegundaReferencia;
            cliente.DescricaoReferencias = ReferenciasDto.DescricaoReferencias;

            //Dados de controle
            cliente.Corretor = AgenteVendaDto.Id;
            cliente.EmpresaVenda = new Tenda.Domain.EmpresaVenda.Models.EmpresaVenda { Id = AgenteVendaDto.IdLoja };

            return cliente;
        }

        public EnderecoCliente ToEnderecoCliente()
        {
            var enderecoCliente = new EnderecoCliente();

            enderecoCliente.Id = EnderecoClienteDto.Id;
            enderecoCliente.Cep = EnderecoClienteDto.Cep;
            enderecoCliente.Logradouro = EnderecoClienteDto.Logradouro;
            enderecoCliente.Numero = EnderecoClienteDto.Numero;
            enderecoCliente.Complemento = EnderecoClienteDto.Complemento;
            enderecoCliente.Bairro = EnderecoClienteDto.Bairro;
            enderecoCliente.Estado = EnderecoClienteDto.Estado;
            enderecoCliente.Cidade = EnderecoClienteDto.Cidade;
            enderecoCliente.Pais = "BR";

            return enderecoCliente;
        }

        public EnderecoEmpresa ToEnderecoEmpresa()
        {
            var enderecoEmpresa = new EnderecoEmpresa();

            enderecoEmpresa.Id = DadosProfissionaisDto.EnderecoEmpresaDto.Id;
            enderecoEmpresa.Cep = DadosProfissionaisDto.EnderecoEmpresaDto.Cep;
            enderecoEmpresa.Logradouro = DadosProfissionaisDto.EnderecoEmpresaDto.Logradouro;
            enderecoEmpresa.Numero = DadosProfissionaisDto.EnderecoEmpresaDto.Numero;
            enderecoEmpresa.Complemento = DadosProfissionaisDto.EnderecoEmpresaDto.Complemento;
            enderecoEmpresa.Bairro = DadosProfissionaisDto.EnderecoEmpresaDto.Bairro;
            enderecoEmpresa.Estado = DadosProfissionaisDto.EnderecoEmpresaDto.Estado;
            enderecoEmpresa.Cidade = DadosProfissionaisDto.EnderecoEmpresaDto.Cidade;
            enderecoEmpresa.Pais = "BR";

            return enderecoEmpresa;
        }

        public void FromDomain(Tenda.Domain.EmpresaVenda.Models.Cliente cliente)
        {
            InformacoesGeraisDto.FromDomain(cliente);
            DadosPessoaisDto.FromDomain(cliente);
            DadosProfissionaisDto.FromDomain(cliente);
            DadosFinanceirosDto.FromDomain(cliente);
            ReferenciasDto.FromDomain(cliente);
            AgenteVendaDto.FromDomain(cliente);
        }
                
        //novo comentário
    }

}
