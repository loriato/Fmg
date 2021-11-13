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
    public class EmAnaliseCompletaCanceladaRule:PrePropostaWorkflowBase
    {
        public EmAnaliseCompletaCanceladaRule()
        {
            Origem = SituacaoProposta.EmAnaliseCompleta;
            Destino = SituacaoProposta.Cancelada;
            Verbo = GlobalMessages.Cancelar;
        }

        public override bool Validate(PreProposta preProposta)
        {
            return true;
        }
    }
}
