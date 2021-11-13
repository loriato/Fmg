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
    public class TipoCustoMap : BaseClassMap<TipoCusto>
    {
        public TipoCustoMap()
        {
            Table("TBL_TIPOS_CUSTO");

            Id(reg => reg.Id).Column("ID_TIPO_CUSTO").GeneratedBy.Sequence("SEQ_TIPOS_CUSTO");
            Map(reg => reg.Descricao).Column("DS_TIPO_CUSTO").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);

        }
    }
}
