using Europa.Extensions;
using System;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.EmpresaVenda.ApiService.Models.Util;

namespace Tenda.EmpresaVenda.ApiService.Models.Cliente
{
    public class DadosPessoaisDto
    {
        public DateTime? DataNascimento { get; set; }
        public int Idade { get; set; }
        public int QuantidadeFilhos { get; set; }
        public TipoEstadoCivil? EstadoCivil { get; set; }
        public TipoRegimeBens? RegimeBens { get; set; }
        public EntityDto Profissao { get; set; }
        public string Cargo { get; set; }
        public string NumeroDocumento { get; set; }
        public string OrgaoEmissor { get; set; }
        public string EstadoEmissor { get; set; }
        public DateTime? DataEmissao { get; set; }
        public string Nacionalidade { get; set; }
        public string Naturalidade { get; set; }
        public string Filiacao { get; set; }
        public TipoResidencia? TipoResidencia { get; set; }
        public TipoEscolaridade? Escolaridade { get; set; }
        public TipoDocumentoEnum? TipoDocumento { get; set; }
        public FamiliarDto FamiliarDto { get; set; }
        public DadosPessoaisDto()
        {
            Profissao = new EntityDto();
            FamiliarDto = new FamiliarDto();
        }

        public void FromDomain(Tenda.Domain.EmpresaVenda.Models.Cliente cliente)
        {
            DataNascimento = cliente.DataNascimento;
            //Idade
            QuantidadeFilhos = cliente.QuantidadeFilhos;
            EstadoCivil = cliente.EstadoCivil;
            RegimeBens = cliente.RegimeBens;

            if (cliente.Profissao.HasValue())
            {
                Profissao.Id = cliente.Profissao.Id;
                Profissao.Nome = cliente.Profissao.Nome;
            }

            NumeroDocumento = cliente.NumeroDocumento;
            OrgaoEmissor = cliente.OrgaoEmissor;
            EstadoEmissor = cliente.EstadoEmissor;
            DataEmissao = cliente.DataEmissao;
            Nacionalidade = cliente.Nacionalidade;
            Naturalidade = cliente.Naturalidade;
            Filiacao = cliente.Filiacao;
            TipoResidencia = cliente.TipoResidencia;
            Cargo = cliente.Cargo;
            Escolaridade = cliente.Escolaridade;
            TipoDocumento = cliente.TipoDocumento;
        }
    }
}
