using Europa.Data;
using Europa.Extensions;
using System;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Domain.Repository.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewEmpreendimentoEnderecoRepository : NHibernateRepository<ViewEmpreendimentoEndereco>
    {
        public IQueryable<ViewEmpreendimentoEndereco> Listar(FiltroEmpreendimentoDTO filtro)
        {
            var query = Queryable();

            if (filtro.Nome.HasValue())
            {
                filtro.Nome = filtro.Nome.ToLower();
                query = query.Where(reg => reg.Nome.ToLower().Contains(filtro.Nome) || reg.Divisao.ToLower().Equals(filtro.Nome));
            }

            if (filtro.Cidade.HasValue())
            {
                filtro.Cidade = filtro.Cidade.ToLower();
                query = query.Where(reg => reg.Cidade.ToLower().Contains(filtro.Cidade));
            }

            if (filtro.Estados.HasValue())
            {
                filtro.Estados = filtro.Estados.Select(reg => reg.ToUpper()).ToArray();
                query = query.Where(reg => filtro.Estados.Contains(reg.Estado.ToUpper()));
            }

            if (filtro.DisponibilizaCatalogo.HasValue())
            {
                bool filterValue = filtro.DisponibilizaCatalogo == 1;
                if (filterValue)
                {
                    query = query.Where(reg => reg.DisponibilizarCatalogo == filterValue);
                }
                else
                {
                    query = query.Where(reg => reg.DisponibilizarCatalogo != true);
                }

            }

            if (filtro.DisponivelVenda.HasValue())
            {
                bool filterValue = filtro.DisponivelVenda == 1;
                query = query.Where(reg => reg.DisponivelVenda == filterValue);
            }

            if (filtro.ModalidadeComissao.HasValue())
            {
                query = query.Where(reg => reg.ModalidadeComissao == filtro.ModalidadeComissao);
            }
            if (filtro.ModalidadeProgramaFidelidade.HasValue())
            {
                query = query.Where(reg => reg.ModalidadeProgramaFidelidade == filtro.ModalidadeProgramaFidelidade);
            }

            if(filtro.IdRegionais.HasValue() && !filtro.IdRegionais.Contains(0))
            {
                query = query.Where(reg => filtro.IdRegionais.Contains(reg.IdRegional));
            }

            return query;
        }
    }
}
