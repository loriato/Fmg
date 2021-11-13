using Europa.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Models;
namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class NumeroPedidoSapRepository : NHibernateRepository<NumeroPedidoSap>
    {
        public string FindNumeroItemDocCompraPorRequisicaoCompra(string requisicaoCompra)
        {
            return Queryable().Where(x => x.NumeroRequisicaoCompra.Equals(requisicaoCompra)).Select(x => x.NumeroItemDocumentoCompra).FirstOrDefault();
        }
        public List<string> ListarNumeroItemDocumentoCompraPorPedidoSap(string pedidoSap)
        {
            return Queryable().Where(x => x.NumeroDocumentoCompra == pedidoSap).Select(x => x.NumeroItemDocumentoCompra).ToList();
        }
    }
}
