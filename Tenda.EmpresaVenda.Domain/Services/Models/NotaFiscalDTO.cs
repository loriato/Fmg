using System.Collections.Generic;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Services.Models
{
    public class NotaFiscalDTO
    {
        public virtual List<ViewNotaFiscalPagamento> NotasFiscais { get; set; }

        //Empresa de Venda
        public virtual string CnpjEmpresaVenda { get; set; }
        public virtual string RazaoSocialEmpresaVenda { get; set; }
        public virtual string LogradouroEmpresaVenda { get; set; }
        public virtual string NumeroEmpresaVenda { get; set; }
        public virtual string BairroEmpresaVenda { get; set; }
        public virtual string CidadeEmpresaVenda { get; set; }
        public virtual string CepEmpresaVenda { get; set; }
        public virtual string RegionalEmpresaVenda { get; set; }

        //Fornecedor
        public virtual string CnpjEstabelecimento { get; set; }
        public virtual string RazaoSocialEstabelecimento { get; set; }
        public virtual string LogradouroEstabelecimento { get; set; }
        public virtual string NumeroEstabelecimento { get; set; }
        public virtual string BairroEstabelecimento { get; set; }
        public virtual string CidadeEstabelecimento { get; set; }
        public virtual string CepEstabelecimento { get; set; }
        public virtual string RegionalEstabelecimento { get; set; }

        //Sap
        public virtual string PedidoSap { get; set; }
        public virtual string CodigoFornecedorSap { get; set; }

        public virtual string Total { get; set; }
    }
}
