using Europa.Data;
using Europa.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class StatusConformidadeMap : BaseClassMap<StatusConformidade>
    {
        public StatusConformidadeMap()
        {
            Table("TBL_STATUS_CONFORMIDADE");

            Id(reg => reg.Id).Column("ID_STATUS_CONFORMIDADE").GeneratedBy.Sequence("SEQ_STATUS_CONFORMIDADE");
            Map(reg => reg.DescricaoEvs).Column("DS_EVS").Not.Nullable().Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.DescricaoJunix).Column("DS_JUNIX").Not.Nullable().Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
        }
    }
}
