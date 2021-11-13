using Europa.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class RegionaisMap : BaseClassMap<Regionais>
    {
        public RegionaisMap()
        {
            Table("TBL_REGIONAIS");
            Id(reg => reg.Id).Column("ID_REGIONAL").GeneratedBy.Sequence("SEQ_REGIONAIS");
            Map(reg => reg.Nome).Column("DS_NOME");
        }
    }
}
