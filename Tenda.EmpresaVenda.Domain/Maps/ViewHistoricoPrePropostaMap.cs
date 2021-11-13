using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    class ViewHistoricoPrePropostaMap : BaseClassMap<ViewHistoricoPreProposta>
    {
        public ViewHistoricoPrePropostaMap()
        {
            Table("VW_HISTORICO_PRE_PROPOSTA");
            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_HISTORICO_PRE_PROPOSTA");

            Map(view => view.IdPreProposta).Column("ID_PRE_PROPOSTA");
            Map(view => view.CodigoPreProposta).Column("CD_PRE_PROPOSTA");
            Map(view => view.IdAnterior).Column("ID_ANTERIOR");
            Map(view => view.IdResponsavelInicio).Column("ID_RESPONSAVEL_INICIO");
            Map(view => view.NomeResponsavelInicio).Column("NM_RESPONSAVEL_INICIO");
            Map(view => view.Inicio).Column("DT_INICIO");
            Map(view => view.SituacaoInicio).Column("TP_SITUACAO_INICIO").CustomType<EnumType<SituacaoProposta>>();
            Map(view => view.IdResponsavelTermino).Column("ID_RESPONSAVEL_TERMINO");
            Map(view => view.NomeResponsavelTermino).Column("NM_RESPONSAVEL_TERMINO");
            Map(view => view.Termino).Column("DT_TERMINO");
            Map(view => view.SituacaoTermino).Column("TP_SITUACAO_TERMINO").CustomType<EnumType<SituacaoProposta>>().Nullable();
            Map(view => view.Situacao).Column("TP_SITUACAO").CustomType<EnumType<Situacao>>();
            Map(view => view.NomePerfilCCAInicial).Column("NM_PERFIL_CCA_INICIAL");
            Map(view => view.NomePerfilCCAFinal).Column("NM_PERFIL_CCA_FINAL");
            Map(view => view.SituacaoInicioPortalHouse).Column("tp_situacao_inicio_house");
            Map(view => view.SituacaoTerminoPortalHouse).Column("tp_situacao_termino_house");


        }
    }
}
