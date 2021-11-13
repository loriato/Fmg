using Europa.Data;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class OrigemLeadMap : BaseClassMap<OrigemLead>
    {
        public OrigemLeadMap()
        {
            Table("TBL_ORIGENS_LEADS");

            Id(reg => reg.Id).Column("ID_ORIGEM_LEAD").GeneratedBy.Sequence("SEQ_ORIGEM_LEAD");

            Map(reg => reg.Nome).Column("NM_ORIGEM_LEAD").Not.Nullable();
            Map(reg => reg.Situacao).Column("TP_SITUACAO").CustomType<EnumType<Situacao>>().Not.Nullable();

            References(reg => reg.Usuario).Column("ID_USUARIO").ForeignKey("FK_ORIGEM_LEAD_X_USUARIO_01").Not.Nullable();
        }
    }
}
