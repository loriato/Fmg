using Europa.Data;
using Europa.Extensions;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Domain.Repository.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewEmpreendimentoExportacaoRepository : NHibernateRepository<ViewEmpreendimentoExportacao>
    {
        public IQueryable<ViewEmpreendimentoExportacao> ListarView(FiltroEmpreendimentoDTO dto)
        {
            var query = Queryable();
            
            if (dto.Nome.HasValue())
            {
                query = query.Where(x => x.Nome.ToLower().Contains(dto.Nome.ToLower()));
            }

            if (dto.Cidade.HasValue())
            {
                query = query.Where(x => x.Cidade.ToLower().Contains(dto.Cidade.ToLower()));
            }

            if (dto.Estados.HasValue())
            {
                query = query.Where(x => dto.Estados.Any(y => y.ToLower() == x.Estado.ToLower()));
            }
            
            if (dto.DisponibilizaCatalogo.HasValue())
            {
                bool filterValue = dto.DisponibilizaCatalogo == 1;
                query = query.Where(reg => reg.DisponibilizarCatalogo == filterValue);
            }

            if (dto.DisponivelVenda.HasValue())
            {
                bool filterValue = dto.DisponivelVenda == 1;
                query = query.Where(reg => reg.DisponivelVenda == filterValue);
            }
            if(dto.Regionais.HasValue() && !dto.IdRegionais.Contains(0))
            {
                query = query.Where(reg => dto.IdRegionais.Contains(reg.IdRegional));
            }

            return query;
        }
    }
}
