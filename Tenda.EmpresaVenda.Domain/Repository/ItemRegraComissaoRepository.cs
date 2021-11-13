using Europa.Data;
using System.Collections.Generic;
using System.Linq;
using Europa.Extensions;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ItemRegraComissaoRepository : NHibernateRepository<ItemRegraComissao>
    {
        public ItemRegraComissao Buscar(long idRegraComissao, long idEmpresaVenda, long idEmpreendimento)
        {
            return Queryable()
                .Where(reg => reg.Empreendimento.Id == idEmpreendimento)
                .Where(reg => reg.EmpresaVenda.Id == idEmpresaVenda)
                .Where(reg => reg.RegraComisao.Id == idRegraComissao)
                .SingleOrDefault();
        }

        public ItemRegraComissao Buscar(long idRegraComissao, long idEmpresaVenda, long idEmpreendimento,TipoModalidadeComissao modalidade)
        {
            return Queryable()
                .Where(reg => reg.Empreendimento.Id == idEmpreendimento)
                .Where(reg => reg.EmpresaVenda.Id == idEmpresaVenda)
                .Where(reg => reg.RegraComisao.Id == idRegraComissao)
                .Where(reg => reg.TipoModalidadeComissao == modalidade)
                .SingleOrDefault();
        }


        public List<ItemRegraComissao> ItensDeRegra(long idRegraComissao)
        {
            return Queryable()
                .Where(reg => reg.RegraComisao.Id == idRegraComissao)
                .ToList();
        }

        public List<ItemRegraComissao> BuscarItens(long? idRegraComissao = null, long? idEmpresaVendas = null, TipoModalidadeComissao? modalidade = null)
        {
            var query = Queryable();

            if (idRegraComissao.HasValue())
            {
                query = query.Where(reg => reg.RegraComisao.Id == idRegraComissao);
            }
            if (idEmpresaVendas.HasValue())
            {
                query = query.Where(reg => reg.EmpresaVenda.Id == idEmpresaVendas);
            }
            if (modalidade.HasValue())
            {
                query = query.Where(x => x.TipoModalidadeComissao == modalidade);
            }


            return query.ToList();
        }

        public ItemRegraComissao Buscar(long idEmpresaVenda, long idEmpreendimento)
        {
            return Queryable()
                .Where(reg => reg.Empreendimento.Id == idEmpreendimento)
                .Where(reg => reg.EmpresaVenda.Id == idEmpresaVenda)
                .FirstOrDefault();
        }

        public List<ItemRegraComissao> Buscar(long idRegraComissao,TipoModalidadeComissao modalidade)
        {
            return Queryable()
                    .Where(x => x.RegraComisao.Id == idRegraComissao)
                    .Where(x => x.TipoModalidadeComissao == modalidade)
                    .ToList();
        }
    }
}