using System;

namespace Tenda.EmpresaVenda.ApiService.Models.Cliente
{
    public class DadosProfissionaisDto
    {
        public string Empresa { get; set; }
        public int? TempoDeEmpresa { get; set; }
        public DateTime? DataAdmissao { get; set; }
        public EnderecoDto EnderecoEmpresaDto { get; set; }

        public DadosProfissionaisDto()
        {
            EnderecoEmpresaDto = new EnderecoDto();
        }
        public void FromDomain(Tenda.Domain.EmpresaVenda.Models.Cliente cliente)
        {
            Empresa = cliente.Empresa;
            TempoDeEmpresa = cliente.TempoDeEmpresa;
            DataAdmissao = cliente.DataAdmissao;
        }
    }
}
