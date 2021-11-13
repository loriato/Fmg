using System;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.ApiService.Models.Financeiro
{
    public class NotaFiscalRequestDto
    {
        public virtual string Chave { get; set; }
        public virtual string EmpresaVenda { get; set; }
        public virtual string Proposta { get; set; }
        public virtual string ParcelaPagamento { get; set; }
        public virtual DateTime? DataComissao { get; set; }
        public virtual string Regional { get; set; }
        public virtual string NumeroPedido { get; set; }
        public virtual string DescricaoNotaFiscal { get; set; }
        public virtual string CnpjEmpresaVenda { get; set; }
        public virtual byte[] NotaFiscal { get; set; }

        public virtual bool Aprovado { get; set; }
        public virtual string MotivoRecusa { get; set; }
        public virtual string MIRO { get; set; }
        public virtual string MIGO { get; set; }
        public virtual DateTime DataPrevisaoPagamento { get; set; }

        public EnvioNotaFiscal FromDoamin(NotaFiscalRequestDto notaFiscalRequestDto)
        {
            var envioNotaFiscal = new EnvioNotaFiscal();

            envioNotaFiscal.Chave = notaFiscalRequestDto.Chave;
            envioNotaFiscal.EmpresaVenda = notaFiscalRequestDto.EmpresaVenda;
            envioNotaFiscal.Proposta = notaFiscalRequestDto.Proposta;
            envioNotaFiscal.ParcelaPagamento = notaFiscalRequestDto.ParcelaPagamento;
            envioNotaFiscal.DataComissao = notaFiscalRequestDto.DataComissao;
            envioNotaFiscal.Regional = notaFiscalRequestDto.Regional;
            envioNotaFiscal.NumeroPedido = notaFiscalRequestDto.NumeroPedido;
            envioNotaFiscal.DescricaoNotaFiscal = notaFiscalRequestDto.DescricaoNotaFiscal;
            envioNotaFiscal.CnpjEmpresaVenda = notaFiscalRequestDto.CnpjEmpresaVenda;
            envioNotaFiscal.NotaFiscal = notaFiscalRequestDto.NotaFiscal;

            return envioNotaFiscal;
        }
    }
}
