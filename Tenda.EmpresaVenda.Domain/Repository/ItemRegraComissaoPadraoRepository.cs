using Europa.Data;
using System.Collections.Generic;
using System.Linq;
using Europa.Extensions;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ItemRegraComissaoPadraoRepository : NHibernateRepository<ItemRegraComissaoPadrao>
    {
        public ItemRegraComissaoPadrao Buscar(long idRegraComissao, long idEmpreendimento)
        {
            return Queryable()
                .Where(reg => reg.Empreendimento.Id == idEmpreendimento)
                .Where(reg => reg.RegraComissaoPadrao.Id == idRegraComissao)
                .SingleOrDefault();
        }

        public ItemRegraComissaoPadrao Buscar(long idRegraComissao, long idEmpreendimento, TipoModalidadeComissao modalidade)
        {
            return Queryable()
                .Where(reg => reg.Empreendimento.Id == idEmpreendimento)
                .Where(reg => reg.RegraComissaoPadrao.Id == idRegraComissao)
                .Where(reg => reg.Modalidade == modalidade)
                .SingleOrDefault();
        }

        public List<ItemRegraComissaoPadrao> ItensDeRegra(long idRegraComissao)
        {
            return Queryable()
                .Where(reg => reg.RegraComissaoPadrao.Id == idRegraComissao)
                .ToList();
        }

        public List<ItemRegraComissaoPadrao> BuscarItens(long? idRegraComissao = null)
        {
            var query = Queryable();

            if (idRegraComissao.HasValue())
            {
                query = query.Where(reg => reg.RegraComissaoPadrao.Id == idRegraComissao);
            }

            return query.ToList();
        }

        public List<ItemRegraComissaoPadrao> BuscarItens(long idRegraComissaoPadrao, TipoModalidadeComissao modalidade)
        {
            return Queryable()
                    .Where(x => x.RegraComissaoPadrao.Id == idRegraComissaoPadrao)
                    .Where(x => x.Modalidade == modalidade)
                    .ToList();
        }
    }
}