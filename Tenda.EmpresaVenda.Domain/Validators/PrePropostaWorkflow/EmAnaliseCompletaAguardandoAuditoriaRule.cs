using Europa.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Validators.PrePropostaWorkflow
{
    public class EmAnaliseCompletaAguardandoAuditoriaRule:PrePropostaWorkflowBase
    {
        public EmAnaliseCompletaAguardandoAuditoriaRule()
        {
            Origem = SituacaoProposta.EmAnaliseCompleta;
            Destino = SituacaoProposta.AguardandoAuditoria;
            Verbo = GlobalMessages.Auditar;
        }

        public override bool Validate(PreProposta preProposta)
        {
            return true;
        }
    }
}
