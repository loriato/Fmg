using Europa.Data;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    class GrupoCCAPrePropostaMap : BaseClassMap<GrupoCCAPreProposta>
    {
            public GrupoCCAPrePropostaMap()
            {
            
                Table("CRZ_GRUPO_CCA_PRE_PROPOSTA");
                Id(reg => reg.Id).Column("ID_CRZ_GRUPO_CCA_PRE_PROPOSTA").GeneratedBy.Sequence("SEQ_CRZ_GRUPO_CCA_PRE_PROPOSTA");
                //Map(reg => reg.IdCCAOrigem).Column("ID_CCA_ORIGEM");
                //Map(reg => reg.IdCCADestino).Column("ID_CCA_DESTINO");
                Map(reg => reg.IdPreProposta).Column("ID_PRE_PROPOSTA");
                Map(reg => reg.Situacao).Column("TP_SITUACAO").CustomType<EnumType<Tenda.Domain.Core.Enums.Situacao>>();
                References(reg => reg.GrupoCCAOrigem).Column("ID_CCA_ORIGEM").ForeignKey("FK_CRZ_GRUPO_CCA_PRE_PROPOSTA_X_GRUPO_CCA_ORIGEM");
                References(reg => reg.GrupoCCADestino).Column("ID_CCA_DESTINO").ForeignKey("FK_CRZ_GRUPO_CCA_PRE_PROPOSTA_X_GRUPO_CCA_DESTINO");

        }

    }
}
