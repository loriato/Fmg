using Europa.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class OcorrenciasMidasMap :  BaseClassMap<OcorrenciasMidas>
    {
        public OcorrenciasMidasMap()
        {
            Table("TBL_OCORRENCIAS_MIDAS"); 
            Id(reg => reg.Id).Column("ID_OCORRENCIAS_MIDAS").GeneratedBy.Sequence("SEQ_OCORRENCIAS_MIDAS");
            Map(reg => reg.OccurenceId).Column("ID_OCCURRENCE");
            Map(reg => reg.TaxIdTaker).Column("ID_TAXTAKER");
            Map(reg => reg.TaxIdProvider).Column("ID_TAXPROVIDER");
            Map(reg => reg.CanBeCommissioned).Column("FL_CAN_BE_COMMISSIONED");
            References(reg => reg.Document).Column("ID_DOCUMENT");

        }
    }
}
