using Europa.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class DocumentoMidasMap : BaseClassMap<DocumentoMidas>
    {
        public DocumentoMidasMap()
        {
            Table("TBL_DOCUMENTOS_MIDAS");
            Id(reg => reg.Id).Column("ID_DOCUMENTOS_MIDAS").GeneratedBy.Sequence("SEQ_DOCUMENTOS_MIDAS");
            Map(reg => reg.Number).Column("NUMBER");
            Map(reg => reg.DocumentType).Column("TYPE").Nullable();
            Map(reg => reg.AccessKey).Column("ACCESS_KEY").Nullable();
            Map(reg => reg.Serie).Column("SERIE").Nullable();
            Map(reg => reg.VerificationCode).Column("VERIFICATION_CODE").Nullable();
            Map(reg => reg.DateIssue).Column("DATE_ISSUE").Nullable();
            Map(reg => reg.MunicipalCode).Column("MUNICIPAL_CODE").Nullable();
            Map(reg => reg.ServiceValue).Column("SERVICE_VALUE").Nullable();
            Map(reg => reg.TotalValue).Column("TOTAL_VALUE").Nullable();
            Map(reg => reg.DueDate).Column("DUE_DATE").Nullable();
        }
    }
}
