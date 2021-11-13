using Europa.Data;
using Europa.Data.Model;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class RegraComissaoEvsMap : BaseClassMap<RegraComissaoEvs>
    {
        public RegraComissaoEvsMap()
        {
            Table("TBL_REGRAS_COMISSAO_EVS");

            Id(reg => reg.Id).Column("ID_REGRA_COMISSAO_EVS").GeneratedBy.Sequence("SEQ_REGRAS_COMISSAO_EVS");
            Map(reg => reg.Descricao).Column("DS_REGRA_COMISSAO").Length(DatabaseStandardDefinitions.TwoHundredFiftySixLength);
            Map(reg => reg.Situacao).Column("TP_SITUACAO").CustomType<EnumType<SituacaoRegraComissao>>();
            Map(reg => reg.InicioVigencia).Column("DT_INICIO_VIGENCIA").Nullable();
            Map(reg => reg.TerminoVigencia).Column("DT_TERMINO_VIGENCIA").Nullable();
            Map(reg => reg.HashDoubleCheck).Column("DS_HASH_DOUBLE_CHECK");
            Map(reg => reg.IdArquivoDoubleCheck).Column("ID_ARQUIVO_DOUBLE_CHECK");
            Map(reg => reg.NomeDoubleCheck).Column("NM_ARQUIVO_DOUBLE_CHECK");
            Map(reg => reg.ContentTypeDoubleCheck).Column("DS_CONTENT_TYPE_DOUBLE_CHECK");
            References(reg => reg.Arquivo).Column("ID_ARQUIVO").ForeignKey("FK_REGRA_COMISSAO_EVS_X_ARQUIVO_01");
            Map(reg => reg.Regional).Column("DS_REGIONAL").Not.Update();
            References(reg => reg.EmpresaVenda).Column("ID_EMPRESA_VENDA").ForeignKey("FK_REGRA_COMISSAO_EVS_X_EMPRESA_VENDA_01").Not.Update();
            References(reg => reg.RegraComissao).Column("ID_REGRA_COMISSAO").ForeignKey("FK_REGRA_COMISSAO_EVS_X_REGRA_COMISSAO_01");
            Map(reg => reg.Tipo).Column("TP_REGRA_COMISSAO").CustomType<EnumType<TipoRegraComissao>>();
            Map(reg => reg.Codigo).Column("CD_REGRA_COMISSAO").Nullable();
        }
    }
}
