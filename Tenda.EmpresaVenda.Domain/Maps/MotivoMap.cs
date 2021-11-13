using Europa.Data;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class MotivoMap : BaseClassMap<Motivo>
    {
        public MotivoMap()
        {
            Table("TBL_MOTIVOS");
            Id(reg => reg.Id).Column("ID_MOTIVO").GeneratedBy.Sequence("SEQ_MOTIVO");
            Map(reg => reg.Descricao).Column("DS_MOTIVO");
            Map(reg => reg.TipoMotivo).Column("TP_MOTIVO").CustomType<EnumType<TipoMotivo>>();
            Map(reg => reg.Situacao).Column("TP_SITUACAO").CustomType<EnumType<Situacao>>();
        }
    }
}
