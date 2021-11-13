using Europa.Data;
using Europa.Extensions;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Domain.Services.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewRegraComissaoEvsRepository : NHibernateRepository<ViewRegraComissaoEvs>
    {
        public DataSourceResponse<ViewRegraComissaoEvs> Listar(DataSourceRequest request, FiltroRegraComissaoDTO filtro)
        {
            var query = Queryable();

            bool todos = ValidarSituacao(filtro.Situacoes);

            if (!filtro.Situacoes.IsEmpty() && todos)
            {
                query = query.Where(reg => filtro.Situacoes.Contains(reg.SituacaoEvs));
            }

            if (!filtro.IdEmpresaVenda.IsEmpty())
            {
                query = query.Where(x=>x.IdEmpresaVenda == filtro.IdEmpresaVenda);
            }

            if (!filtro.Descricao.IsEmpty())
            {
                query = query.Where(reg => reg.Descricao.ToLower().Contains(filtro.Descricao.ToLower()));
            }
            
            if (!filtro.Regionais.IsEmpty() && !filtro.Regionais.Contains("Todos"))
            {
                query = query.Where(reg => filtro.Regionais.Contains(reg.Regional));
            }
            if (!filtro.VigenteEm.IsEmpty())
            {
                query = query.Where(reg => reg.InicioVigencia.Value.Date <= filtro.VigenteEm.Value.Date)
                    .Where(reg => reg.TerminoVigencia.Value.Date >= filtro.VigenteEm.Value.Date || !reg.TerminoVigencia.HasValue);
            }

            if (!filtro.Tipos.IsEmpty())
            {
                query = query.Where(reg => filtro.Tipos.Contains(reg.TipoRegraComissao));
            }

            if (!filtro.CodigoRegra.IsEmpty())
            {
                filtro.CodigoRegra = filtro.CodigoRegra.ToLower();
                query = query.Where(x => x.Codigo.ToLower().Contains(filtro.CodigoRegra));
            }

            return query.ToDataRequest(request);
        }

        public bool ValidarSituacao(List<SituacaoRegraComissao> Situacoes)
        {
            List<int> list = new List<int> { 
                (int)SituacaoRegraComissao.Rascunho,
                (int)SituacaoRegraComissao.Ativo,
                (int)SituacaoRegraComissao.Vencido,
                (int)SituacaoRegraComissao.SuspensoPorCampanha,
                (int)SituacaoRegraComissao.AguardandoLiberacao
            };

            foreach(var sit in Situacoes)
            {
                if(!list.Contains((int)sit))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
