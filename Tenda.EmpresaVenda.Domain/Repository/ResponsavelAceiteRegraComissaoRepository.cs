using Europa.Data;
using Europa.Extensions;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.ApiService.Models.RegraComissao;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ResponsavelAceiteRegraComissaoRepository : NHibernateRepository<ResponsavelAceiteRegraComissao>
    {
        public List<ResponsavelAceiteRegraComissao> FindResposaveisAtivos(long IdEmpresaVenda)
        {
            return Queryable().Where(x => x.EmpresaVenda.Id == IdEmpresaVenda && x.Situacao == Situacao.Ativo).ToList();
        }
    }
}
