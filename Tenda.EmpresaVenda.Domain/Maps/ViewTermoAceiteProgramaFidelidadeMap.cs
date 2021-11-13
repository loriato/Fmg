using Europa.Data;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewTermoAceiteProgramaFidelidadeMap : BaseClassMap<ViewTermoAceiteProgramaFidelidade>
    {
        public ViewTermoAceiteProgramaFidelidadeMap()
        {
            Table("VW_TERMOS_ACEITE_PROGRAMA_FIDELIDADE");
            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_TERMO_ACEITE_PROGRAMA_FIDELIDADE");
            Map(reg => reg.NomeDoubleCheck).Column("NM_DOUBLE_CHECK");
            Map(reg => reg.IdArquivoDoubleCheck).Column("ID_ARQUIVO_DOUBLE_CHECK");
        }


    }
}
