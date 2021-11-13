using Europa.Data;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewHistoricoRegraComissaoMap:BaseClassMap<ViewHistoricoRegraComissao>
    {
        public ViewHistoricoRegraComissaoMap()
        {
            Table("VW_HISTORICO_REGRA_COMISSAO");
            ReadOnly();

            Id(reg => reg.Id).Column("ID_HISTORICO_REGRA_COMISSAO");

            Map(reg => reg.Descricao).Column("DS_REGRA_COMISSAO");
            Map(reg => reg.Situacao).Column("TP_SITUACAO").CustomType<EnumType<SituacaoRegraComissao>>();
            Map(reg => reg.ResponsavelInicio).Column("NM_RESPONSAVEL_INICIO");
            Map(reg => reg.DataInicio).Column("DT_INICIO");
            Map(reg => reg.ResponsavelTermino).Column("NM_RESPONSAVEL_TERMINO").Nullable();
            Map(reg => reg.DataTermino).Column("DT_TERMINO").Nullable();
            Map(reg => reg.Status).Column("TP_STATUS").CustomType<EnumType<Situacao>>();
            Map(reg => reg.EmpresaVenda).Column("NM_EMPRESA_VENDA");
            Map(reg => reg.TipoRegraComissao).Column("TP_REGRA_COMISSAO").CustomType<EnumType<TipoRegraComissao>>();
        }
    }
}
