using Europa.Data;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class LeadEmpresaVendaRepository : NHibernateRepository<LeadEmpresaVenda>
    {
        public bool EmpresaVendaPossuiLead(long id)
        {
            return Queryable().Where(x => x.EmpresaVenda.Id == id).Any();
        }

        public List<LeadEmpresaVenda> FindByIds(List<long> ids)
        {
            return Queryable()
                .Where(x => ids.Contains(x.Id))
                .ToList();
        }

        public bool CorretorPossuiLead(long idCorretor)
        {
            return Queryable()
                .Where(x => x.Corretor.Id == idCorretor)
                .Any();
        }

        public LeadEmpresaVenda UltimoLeadCorretor(long idCorretor)
        {
            return Queryable()
                .Where(x => x.Corretor.Id == idCorretor)
                .Where(x => x.Lead.Liberar == true)
                .OrderByDescending(x => x.CriadoEm)
                .FirstOrDefault();
        }

        public LeadEmpresaVenda UltimoLeadEv(long idEmpresaVenda)
        {
            return Queryable()
                .Where(x => x.EmpresaVenda.Id == idEmpresaVenda)
                .Where(x => x.Lead.Liberar == true)
                .OrderByDescending(x => x.CriadoEm)
                .FirstOrDefault();
        }

        public List<LeadEmpresaVenda> BuscarPorPacote(string pacote)
        {
            return Queryable()
                .Where(x => x.Lead.DescricaoPacote.ToUpper().Equals(pacote.ToUpper())).ToList();
        }

        public List<long> BuscarIdsEvPorPacote(List<string> pacote)
        {
            return Queryable()
               .Where(x => pacote.Contains(x.Lead.DescricaoPacote))
               .Select(x => x.EmpresaVenda.Id).Distinct().ToList();
        }
        public List<long> BuscarIdsPorEvPacote(long idEmpresaVenda, string pacote)
        {
            return Queryable()
                .Where(x => x.Lead.DescricaoPacote.ToUpper().Equals(pacote.ToUpper()))
                .Where(x => x.EmpresaVenda.Id == idEmpresaVenda)
                .Select(x => x.Id)
                .ToList();
        }
    }
}
