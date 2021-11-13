using Europa.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class TransferenciaCarteiraMap : BaseClassMap<TransferenciaCarteira>
    {
        public TransferenciaCarteiraMap()
        {
            Table("TBL_TRANSFERENCIAS_CARTEIRA");
            Id(reg => reg.Id).Column("ID_TRANSFERENCIA_CARTEIRA").GeneratedBy.Sequence("SEQ_TRANSFERENCIAS_CARTEIRA");
            References(reg => reg.PreProposta).Column("ID_PRE_PROPOSTA").ForeignKey("FK_CARTEIRA_X_PRE_PROPOSTA_01").Not.Update();
            References(reg => reg.ViabilizadorOrigem).Column("ID_VIABILIZADOR_ORIGEM").ForeignKey("FK_CARTEIRA_X_VIABILIZADOR_01").Not.Update();
            References(reg => reg.ViabilizadorDestino).Column("ID_VIABILIZADOR_DESTINO").ForeignKey("FK_CARTEIRA_X_VIABLIZADOR_01").Not.Update();
          
        }
    }
}
