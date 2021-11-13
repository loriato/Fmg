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
    public class StatusSuatStatusEvsMap : BaseClassMap<StatusSuatStatusEvs>
    {
        public StatusSuatStatusEvsMap()
        {
            Table("TBL_STATUS_SUAT_STATUS_EVS");

            Id(reg => reg.Id).Column("ID_STATUS_SUAT_STATUS_EVS").GeneratedBy.Sequence("SEQ_STATUS_SUAT_STATUS_EVS");
            Map(reg => reg.DescricaoEvs).Column("DS_EVS").Nullable().Length(DatabaseStandardDefinitions.FiftyLength);
            Map(reg => reg.DescricaoSuat).Column("DS_SUAT").Nullable().Length(DatabaseStandardDefinitions.FiftyLength);
        
        }
    }
}
