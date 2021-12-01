using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.Fmg.Enums;
using Tenda.Domain.Fmg.Models.Views;

namespace Europa.Fmg.Domain.Maps
{
    public class ViewViaturaUsuarioMap : BaseClassMap<ViewViaturaUsuario>
    {
        public ViewViaturaUsuarioMap()
        {
            Table("VW_VIATURAS_USUARIOS");
            ReadOnly();
            SchemaAction.None();

            Id(x => x.Id).Column("ID_VIATURA_USUARIO");
            Map(x => x.IdUsuario).Column("ID_USUARIO");
            Map(x => x.IdViatura).Column("ID_VIATURA");
            Map(x => x.NomeUsuario).Column("NM_USUARIO");
            Map(x => x.Modelo).Column("DS_MODELO");
            Map(x => x.Placa).Column("DS_PLACA");
            Map(x => x.DataPedido).Column("DT_PEDIDO");
            Map(x => x.DataEntrega).Column("DT_ENTREGA");
            Map(x => x.QuilometragemNovo).Column("NR_QUILOMETRAGEM_NOVO");
            Map(x => x.QuilometragemAntigo).Column("NR_QUILOMETRAGEM_ANTIGO");
            Map(x => x.Situacao).Column("TP_SITUACAO").CustomType<EnumType<SituacaoViatura>>(); ;
        }
    }
}
