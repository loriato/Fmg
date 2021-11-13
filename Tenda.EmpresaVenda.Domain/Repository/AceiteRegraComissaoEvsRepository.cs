using Europa.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class AceiteRegraComissaoEvsRepository:NHibernateRepository<AceiteRegraComissaoEvs>
    {
        public bool BuscarAceiteParaRegraEvsAndEmpresaVenda(long idRegraComissaoEvs, long idEmpresaVenda)
        {
            return Queryable()
                .Where(reg => reg.RegraComissaoEvs.Id == idRegraComissaoEvs)
                .Where(reg => reg.EmpresaVenda.Id == idEmpresaVenda)
                .Any();
        }
        public AceiteRegraComissaoEvs SegundoAceite(long idEmpresaVenda)
        {
            return Queryable()
                .Where(reg => reg.EmpresaVenda.Id == idEmpresaVenda)
                .OrderByDescending(reg => reg.DataAceite)
                .Where(x => x.RegraComissaoEvs.Tipo != TipoRegraComissao.Campanha)
                .FirstOrDefault();
        }
        public AceiteRegraComissaoEvs AceiteMaisRecente(long idEmpresaVenda)
        {
            return Queryable()
                .Where(reg => reg.EmpresaVenda.Id == idEmpresaVenda)
                .OrderByDescending(reg => reg.DataAceite)
                .FirstOrDefault();
        }

        public IQueryable<AceiteRegraComissaoEvs> ListarTodosAceites(long idEmpresaVenda)
        {
            return Queryable()
               .Where(reg => reg.EmpresaVenda.Id == idEmpresaVenda)
               .OrderByDescending(reg => reg.DataAceite);
        }

        public List<RegraComissaoEvs> BuscarPorEvEData(long idEmpresaVenda, DateTime data)
        {
            return Queryable()
                .Where(x => x.EmpresaVenda.Id == idEmpresaVenda)
                .Where(x => x.RegraComissaoEvs.InicioVigencia >= data)
                .OrderBy(x => x.DataAceite)
                .Select(x => x.RegraComissaoEvs)
                .ToList();
        }

        public IQueryable<RegraComissaoEvs> BuscarRegraAbertaComAceite(long idEmpresaVenda,DateTime dataVenda)
        {
            return Queryable()
                .Where(x => x.EmpresaVenda.Id == idEmpresaVenda)
                .Where(x => dataVenda >= x.RegraComissaoEvs.InicioVigencia)
                .Where(x => x.RegraComissaoEvs.TerminoVigencia == null)
                .OrderByDescending(x => x.RegraComissaoEvs.InicioVigencia)
                .Select(x => x.RegraComissaoEvs);
        }

        public RegraComissaoEvs BuscarRegraComAceiteNaData(long idEmpresaVenda, DateTime dataVenda)
        {
            return Queryable()
                .Where(x => x.EmpresaVenda.Id == idEmpresaVenda)
                .Where(x => dataVenda >= x.RegraComissaoEvs.InicioVigencia)
                .Where(x => dataVenda <= x.RegraComissaoEvs.TerminoVigencia)
                .OrderByDescending(x => x.RegraComissaoEvs.Tipo)
                .Select(x => x.RegraComissaoEvs)
                .FirstOrDefault();
        }
    }
}
