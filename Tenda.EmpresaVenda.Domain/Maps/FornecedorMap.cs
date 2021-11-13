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
    public class FornecedorMap : BaseClassMap<Fornecedor>
    {
        public FornecedorMap()
        {
            Table("TBL_FORNECEDORES");

            Id(reg => reg.Id).Column("ID_FORNECEDOR").GeneratedBy.Sequence("SEQ_FORNECEDORES");
            Map(reg => reg.NomeFantasia).Column("NM_FANTASIA").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.RazaoSocial).Column("DS_RAZAO_SOCIAL").Length(DatabaseStandardDefinitions.TwoHundredFiftySixLength);
            Map(reg => reg.CNPJ).Column("DS_CNPJ").Length(DatabaseStandardDefinitions.TwentyLength);

        }
    }
}
