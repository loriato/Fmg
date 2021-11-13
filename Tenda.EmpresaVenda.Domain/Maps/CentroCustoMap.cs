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
    public class CentroCustoMap : BaseClassMap<CentroCusto>
    {
        public CentroCustoMap()
        {
            Table("TBL_CENTROS_CUSTO");

            Id(reg => reg.Id).Column("ID_CENTRO_CUSTO").GeneratedBy.Sequence("SEQ_CENTROS_CUSTO");
            Map(reg => reg.Descricao).Column("DS_CENTRO_CUSTO").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.Codigo).Column("CD_CENTRO_CUSTO");
        }
    }
}
