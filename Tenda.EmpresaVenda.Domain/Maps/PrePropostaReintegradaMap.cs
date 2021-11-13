using Europa.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class PrePropostaReintegradaMap : BaseClassMap<PrePropostaReintegrada>
    {
        public PrePropostaReintegradaMap()
        {
            Table("TBL_PRE_PROPOSTAS_REINTEGRADAS");
            Id(reg => reg.Id).Column("ID_PRE_PROPOSTA_REINTEGRADA").GeneratedBy.Sequence("SEQ_PRE_PROPOSTAS_REINTEGRADAs");
            Map(reg => reg.IdSuat).Column("ID_SUAT").Nullable();
            Map(reg => reg.IdUnidadeSuat).Column("ID_UNIDADE_SUAT").Nullable();
            Map(reg => reg.IdentificadorUnidadeSuat).Column("CD_IDENTIFICADOR_UNIDADE_SUAT").Nullable();
            Map(reg => reg.IdTorre).Column("ID_TORRE").Nullable();
            Map(reg => reg.ObservacaoTorre).Column("DS_TORRE").Nullable();
            Map(reg => reg.NomeTorre).Column("NM_TORRE").Nullable();
            Map(reg => reg.PassoAtualSuat).Column("NM_PASSO_ATUAL_SUAT").Nullable();
            Map(reg => reg.IdBreveLancamento).Column("ID_BREVE_LANCAMENTO").Nullable();
            References(reg => reg.PreProposta).Column("ID_PRE_PROPOSTA").ForeignKey("FK_PRE_PROPOSTA_REINTEGRADAS_X_PRE_PROSPOTA_01");
        }
    }
}
