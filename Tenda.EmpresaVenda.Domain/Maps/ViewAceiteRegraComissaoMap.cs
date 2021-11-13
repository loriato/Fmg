using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewAceiteRegraComissaoMap : BaseClassMap<ViewAceiteRegraComissao>
    {
        public ViewAceiteRegraComissaoMap()
        {
            Table("VW_ACEITE_REGRA_COMISSAO");
            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_REGRA_COMISSAO_EVS");
            Map(reg => reg.IdEmpresaVenda).Column("ID_EMPRESA_VENDA");
            Map(reg => reg.NomeEmpresaVenda).Column("NM_EMPRESA_VENDA");
            Map(reg => reg.CnpjEmpresaVenda).Column("DS_CNPJ_EMPRESA_VENDA");
            Map(reg => reg.InicioVigenciaRegraComissao).Column("DT_INICIO_VIGENCIA").CustomType<DateTimeType>(); 
            Map(reg => reg.TerminoVigenciaRegraComissao).Column("DT_TERMINO_VIGENCIA").CustomType<DateTimeType>(); 
            Map(reg => reg.DataAceite).Column("DT_ACEITE").CustomType<DateTimeType>(); 
            Map(reg => reg.IdRegraComissao).Column("ID_REGRA_COMISSAO");
            Map(reg => reg.DescricaoRegraComissao).Column("DS_REGRA_COMISSAO");
            Map(reg => reg.SituacaoRegraComissao).Column("TP_SITUACAO").CustomType<EnumType<SituacaoRegraComissao>>();
            Map(reg => reg.IdAprovador).Column("ID_APROVADOR");
            Map(reg => reg.NomeAprovador).Column("NM_APROVADOR");
            Map(reg => reg.IdArquivo).Column("ID_ARQUIVO");
        }
    }
}
