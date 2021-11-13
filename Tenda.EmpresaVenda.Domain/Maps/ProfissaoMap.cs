using Europa.Data;
using Europa.Data.Model;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ProfissaoMap : BaseClassMap<Profissao>
    {
        public ProfissaoMap()
        {
            Table("TBL_PROFISSOES");
            Id(reg => reg.Id).Column("ID_PROFISSAO").GeneratedBy.Sequence("SEQ_PROFISSOES");
            Map(reg => reg.Nome).Column("NM_PROFISSAO").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.IdSap).Column("DS_ID_SAP").Length(DatabaseStandardDefinitions.TwentyLength);
        }
    }
}
