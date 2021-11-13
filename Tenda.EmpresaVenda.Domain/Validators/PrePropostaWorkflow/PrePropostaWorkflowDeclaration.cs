using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.EmpresaVenda.Domain.Validators.PrePropostaWorkflow
{
    public static class PrePropostaWorkflowDeclaration
    {
        private static List<PrePropostaWorkflowBase> _allRules;

        public static List<PrePropostaWorkflowBase> AllRules
        {
            get
            {
                if (_allRules == null)
                {
                    _allRules = new List<PrePropostaWorkflowBase>();
                    lock (_allRules)
                    {
                        _allRules.Add(new EmElaboracaoEmElaboracaoRule());
                        _allRules.Add(new AguardandoAnaliseSimplificadaEmAnaliseSimplificadaRule());
                        _allRules.Add(new AnaliseSimplificadaAprovadaCanceladaRule());
                        _allRules.Add(new AnaliseSimplificadaAprovadaDocsInsuficientesSimplificadoRule());
                        _allRules.Add(new EmAnaliseSimplificadaAnaliseSimplificadaAprovadaRule());
                        _allRules.Add(new EmAnaliseSimplificadaDocsInsuficientesSimplificadoRule());
                        _allRules.Add(new EmAnaliseSimplificadaCanceladaRule());
                        _allRules.Add(new EmAnaliseSimplificadaAguardandoAnaliseSimplificadaRule());
                        _allRules.Add(new EmElaboracaoAguardandoAnaliseSimplificadaRule());
                        _allRules.Add(new EmElaboracaoCanceladaRule());
                        _allRules.Add(new DocsInsuficientesSimplificadoCanceladaRule());
                        _allRules.Add(new AguardandoFluxoFluxoEnviadoRule());
                        _allRules.Add(new FluxoEnviadoAguardandoFluxoRule());                        
                        _allRules.Add(new CondicionadaCanceladaRule());
                        _allRules.Add(new AnaliseSimplificadaAprovadaAguardandoFluxoRule());
                        _allRules.Add(new AguardandoFluxoCanceladaRule());
                        _allRules.Add(new SicaqComErroCanceladaRule());
                        _allRules.Add(new FluxoEnviadoCanceladaRule());
                        _allRules.Add(new EmAnaliseCompletaAguardandoIntegracaoRule());
                        _allRules.Add(new EmAnaliseCompletaAguardandoAnaliseCompletaRule());
                        _allRules.Add(new DocsInsuficientesSimplificadoAguardandoAnaliseSimplificadaRule());
                        _allRules.Add(new AguardandoIntegracaoIntegradaRule());
                        _allRules.Add(new EmAnaliseSimplificadaAguardandoAuditoriaRule());
                        _allRules.Add(new AguardandoAuditoriaEmAnaliseSimplificadaRule());
                        _allRules.Add(new FluxoEnvidadoAguardandoAnaliseCompletaRule());
                        _allRules.Add(new AguardandoAnaliseCompletaEmAnaliseCompletaRule());
                        _allRules.Add(new EmAnaliseCompletaAguardandoAuditoriaRule());
                        _allRules.Add(new AguardandoAuditoriaEmAnaliseCompletaRule());
                        _allRules.Add(new EmAnaliseCompletaDocsInsuficientesCompletaRule());
                        _allRules.Add(new DocsInsuficientesCompletaAguardandoAnaliseCompletaRule());
                        _allRules.Add(new DocsInsuficientesCompletaCanceladaRule());
                        _allRules.Add(new EmAnaliseCompletaCanceladaRule());
                        _allRules.Add(new IntegradaAguardandoIntegracaoRule());
                        _allRules.Add(new AnaliseSimplificadaAprovadaEmAnaliseSimplificadaRule());
                        _allRules.Add(new EmAnaliseCompletaAnaliseCompletaAprovadaRule());
                        _allRules.Add(new AnaliseCompletaAprovadaCanceladaRule());
                        _allRules.Add(new AnaliseCompletaAprovadaSICAQComErroRule());
                        _allRules.Add(new AnaliseCompletaAprovadaReprovadaRule());
                        _allRules.Add(new SICAQComErroAnaliseCompletaAprovadaRule());
                        _allRules.Add(new AnaliseCompletaAprovadaCondicionadaRule());
                        _allRules.Add(new AnaliseCompletaAprovadaAguardandoIntegracaoRule());
                        _allRules.Add(new SICAQComErroAnaliseSimplificadaAprovadaRule());
                        _allRules.Add(new AguardandoIntegracaoAnaliseCompletaAprovadaRule());
                        _allRules.Add(new AnaliseCompletaAprovadaEmAnaliseCompletaRule());
                        _allRules.Add(new AnaliseSimplificadaAprovadaCondicionadaRule()); 
                        _allRules.Add(new AnaliseSimplificadaAprovadaSICAQComErroRule()); 
                        _allRules.Add(new AnaliseSimplificadaAprovadaReprovadaRule());
                        _allRules.Add(new AguardandoFluxoAguardandoAnaliseCompletaRule());
                        _allRules.Add(new ReprovadaAnaliseSimplificadaAprovadaRule());
                        _allRules.Add(new ReprovadaAnaliseCompletaAprovadaRule());
                        _allRules.Add(new CondicionadaAnaliseSimplificadaAprovadaRule());
                        _allRules.Add(new CondicionadaAnaliseCompletaAprovadaRule());
                        _allRules.Add(new FluxoEnvidadoAguardandoIntegracaoRule());
                        _allRules.Add(new AguardandoFluxoAguardandoIntegracaoRule());

                        #region transições desativadas
                        /*
                        _allRules.Add(new PreAnaliseAprovadaAguardandoIntegracaoRule());//não existe
                        _allRules.Add(new PendenciaRetornoRule());//não existe
                        _allRules.Add(new RetornoEmPreAnaliseRule());//não existe                        
                        _allRules.Add(new PreAnaliseAprovadaEmPreAnaliseRule());//não existe
                        */
                        #endregion
                    }
                }

                return _allRules;
            }
        }

        public static List<PrePropostaWorkflowBase> RuleFor(SituacaoProposta origem, SituacaoProposta destino)
        {
            return AllRules.AsQueryable()
                .Where(reg => reg.Origem == origem)
                .Where(reg => reg.Destino == destino)
                .Select(reg => reg.CloneAndCasting())
                .ToList();
        }

        public static Dictionary<string, SituacaoProposta> ProximosPassos(SituacaoProposta origem)
        {
            return AllRules.AsQueryable()
               .Where(reg => reg.Origem == origem)
               .ToDictionary(reg => reg.Verbo, reg => reg.Destino);
        }


    }
}
