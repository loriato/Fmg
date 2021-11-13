using Europa.Data;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewClienteDuplicadoMap:BaseClassMap<ViewClienteDuplicado>
    {
        public ViewClienteDuplicadoMap()
        {
            Table("VW_CLIENTE_DUPLICADO");

            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_VW_CLIENTE_DUPLICADO");

            Map(reg => reg.IdCliente).Column("ID_CLIENTE");
            Map(reg => reg.NomeCliente).Column("NM_CLIENTE");
            Map(reg => reg.CPF).Column("DS_CPF_CNPJ");

            Map(reg => reg.IdViabilizador).Column("ID_VIABILIZADOR");
            Map(reg => reg.NomeViabilizador).Column("NM_VIABILIZADOR");

            Map(reg => reg.IdEmpresaVenda).Column("ID_EMPRESA_VENDA");
            Map(reg => reg.NomeEmpresaVenda).Column("NM_EMPRESA_VENDA");

            Map(reg => reg.IdSupervisor).Column("ID_SUPERVISOR");
            Map(reg => reg.NomeSupervisor).Column("NM_SUPERVISOR");

            Map(reg => reg.IdCoordenadorSupervisor).Column("ID_COORDENADOR_SUPERVISOR");
            Map(reg => reg.NomeCoordenadorSupervisor).Column("NM_COORDENADOR_SUPERVISOR");

            Map(reg => reg.IdCoordenadorViabilizador).Column("ID_COORDENADOR_VIABILIZADOR");
            Map(reg => reg.NomeCoordenadorViabilizador).Column("NM_COORDENADOR_VIABILIZADOR");
        }
    }
}
