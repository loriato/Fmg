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
    public class ClassificacaoMap : BaseClassMap<Classificacao> 
    {
        public ClassificacaoMap()
        {
            Table("TBL_CLASSIFICACOES");

            Id(reg => reg.Id).Column("ID_CLASSIFICACAO").GeneratedBy.Sequence("SEQ_CLASSIFICACOES");
            Map(reg => reg.Descricao).Column("DS_CLASSIFICACAO").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
        }
    }
}
