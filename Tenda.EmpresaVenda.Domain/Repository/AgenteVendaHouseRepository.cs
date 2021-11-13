using Europa.Data;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class AgenteVendaHouseRepository:NHibernateRepository<AgenteVendaHouse>
    {
        public bool AgenteVendaValido(long idUsuario,long idLoja)
        {
            var query = Queryable()
                .Where(x => x.AgenteVenda.Usuario.Id == idUsuario)
                .Where(x => x.House.Id != idLoja)
                .Where(x => x.Situacao == SituacaoHierarquiaHouse.Ativo)
                .Where(x => x.Fim == null);

            return query.Any();
        }

        public AgenteVendaHouse BuscarVinculoAgenteVendaHouseAtivo(long idAgenteVenda)
        {
            var agenteVendaHouse=Queryable()
                .Where(x => x.AgenteVenda.Id == idAgenteVenda)
                .Where(x => x.Situacao == SituacaoHierarquiaHouse.Ativo)
                .SingleOrDefault();

            return agenteVendaHouse;
        }
    }
}
