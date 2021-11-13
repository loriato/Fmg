using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.Core.Models;

namespace Tenda.Domain.Core.Maps
{
    public class AgendaMap : BaseClassMap<Agenda>
    {
        public AgendaMap()
        {
            Table("TBL_AGENDAS");
            // To Not Create Table
            SchemaAction.None();

            UseUnionSubclassForInheritanceMapping();

            Id(reg => reg.Id).Column("ID_AGENDA").GeneratedBy.Sequence("SEQ_AGENDAS");
            Map(reg => reg.DataCriacao).Column("DT_CRIACAO");
            Map(reg => reg.Dias).Column("NR_DIAS");
            Map(reg => reg.Situacao).Column("TP_SITUACAO").CustomType<Situacao>();
            Map(reg => reg.HoraInicio).Column("DT_HORA_INICIO").CustomType<TimeAsTimeSpanType>();
            Map(reg => reg.HoraFim).Column("DT_HORA_FIM").CustomType<TimeAsTimeSpanType>();
            Map(reg => reg.InicioAgenda).Column("DT_INICIO_AGENDA");
            Map(reg => reg.FimAgenda).Column("DT_FIM_AGENDA");
            Map(reg => reg.TempoSlot).Column("NR_TEMPO_SLOT");
            References(x => x.Criador).Column("ID_USUARIO_CRIADOR").ForeignKey("FK_AGENDAS_X_USUARIOS_PORTAL_01");

        }
    }
}
