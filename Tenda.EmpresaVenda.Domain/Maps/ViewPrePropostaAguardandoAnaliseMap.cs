using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewPrePropostaAguardandoAnaliseMap : BaseClassMap<ViewPrePropostaAguardandoAnalise>
    {
        public ViewPrePropostaAguardandoAnaliseMap()
        {
            Table("VW_CONSOLIDADOS_PRE_PROPOSTA");
            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_CONSOLIDADO_PRE_PROPOSTA");
            Map(reg => reg.UF).Column("NM_ESTADO");

            Map(reg => reg.IdPreProposta).Column("ID_PRE_PROPOSTA");
            Map(reg => reg.CodigoPreProposta).Column("CD_PRE_PROPOSTA");
            Map(reg => reg.StatusPreProposta).Column("TP_SITUACAO_PRE_PROPOSTA").CustomType<EnumType<SituacaoProposta>>();
            Map(reg => reg.DataElaboracao).Column("DT_ELABORACAO");
            Map(reg => reg.HoraElaboracao).Column("HR_ELABORACAO").CustomType<Europa.Data.Conventions.TimeSpanType>();
            Map(reg => reg.SituacaoPrePropostaSuatEvs).Column("DS_SITUACAO_SUAT_EVS");
            Map(reg => reg.ParcelaAprovada).Column("VL_PARCELA_APROVADA");

            // proponentes
            Map(reg => reg.IdProponenteUm).Column("ID_PROPONENTE_UM");
            Map(reg => reg.ProponenteUm).Column("NM_PROPONENTE_UM");
            Map(reg => reg.CpfProponenteUm).Column("DS_CPF_PROPONENTE_UM");
            Map(reg => reg.IdProponenteDois).Column("ID_PROPONENTE_DOIS");
            Map(reg => reg.ProponenteDois).Column("NM_PROPONENTE_DOIS");
            Map(reg => reg.CpfProponenteDois).Column("DS_CPF_PROPONENTE_DOIS");

            // Diversos
            Map(reg => reg.IdBreveLancamento).Column("ID_BREVE_LANCAMENTO");
            Map(reg => reg.BreveLancamento).Column("NM_BREVE_LANCAMENTO");
            Map(reg => reg.IdEmpresaVenda).Column("ID_EMPRESA_VENDA");
            Map(reg => reg.EmpresaVenda).Column("NM_EMPRESA_VENDA");
            Map(reg => reg.AgenteViabilizador).Column("NM_VIABILIZADOR");
            Map(reg => reg.Corretor).Column("NM_CORRETOR");
            Map(reg => reg.IdCorretor).Column("ID_CORRETOR");
            Map(reg => reg.NomeElaborador).Column("NM_ELABORADOR");
            Map(reg => reg.IdViabilizador).Column("ID_VIABILIZADOR");
            // Ref. Envio p/ Fila(Aguardando Analise)
            Map(reg => reg.IdEnvioPreProposta).Column("ID_ENVIO");
            Map(reg => reg.DataEnvioPreProposta).Column("DT_ENVIO").Nullable();
            Map(reg => reg.HoraEnvioPreProposta).Column("HR_ENVIO").Nullable().CustomType<Europa.Data.Conventions.TimeSpanType>();
            Map(reg => reg.DataHoraUltimoEnvio).Column("DT_HR_ULTIMO_ENVIO").Nullable();

            // Ultima analise
            Map(reg => reg.IdAnalisePreProposta).Column("ID_ANALISE");
            Map(reg => reg.DataInicioAnalise).Column("DT_INICIO_ANALISE").Nullable();
            Map(reg => reg.HoraInicioAnalise).Column("HR_INICIO_ANALISE").Nullable().CustomType<Europa.Data.Conventions.TimeSpanType>();
            Map(reg => reg.HoraFimAnalise).Column("HR_FIM_ANALISE").Nullable().CustomType<Europa.Data.Conventions.TimeSpanType>();
            Map(reg => reg.NomeAssistenteAnalise).Column("NM_ASSISTENTE_ANALISE");
            Map(reg => reg.IdAssistenteAnalise).Column("ID_ASSISTENTE_ANALISE");

            Map(reg => reg.Contador).Column("NR_ESFORCO_ANALISE_MAIS_RECENTE");
            Map(reg => reg.ContadorTotal).Column("NR_ESFORCO_TOTAL");

            Map(reg => reg.PropostasAnteriores).Column("FL_PROPOSTA_ANTERIOR_CLIENTE");
            Map(reg => reg.NumeroPropostasAnteriores).Column("NR_PROPOSTAS_ANTERIORES");

            Map(reg => reg.MotivoPendencia).Column("DS_PENDENCIAS_ANALISE");
            Map(reg => reg.MotivoParecer).Column("DS_PENDENCIAS_PARECER");

            Map(reg => reg.IdAnaliseSicaq).Column("ID_ANALISE_SICAQ");
            Map(reg => reg.IdAnalistaSicaq).Column("ID_ANALISTA_SICAQ");
            Map(reg => reg.DataSicaq).Column("DT_SICAQ").Nullable();
            Map(reg => reg.HoraSicaq).Column("HR_SICAQ").Nullable().CustomType<Europa.Data.Conventions.TimeSpanType>();
            Map(reg => reg.NomeAnalistaSicaq).Column("NM_ANALISTA_SICAQ");
            Map(reg => reg.StatusSicaq).Column("TP_STATUS_SICAQ").CustomType<EnumType<StatusSicaq>>();

            Map(reg => reg.ContadorPrimeiraAnalise).Column("NR_ESFORCO_ANALISE_MAIS_ANTIGA");
            Map(reg => reg.DataInicioPrimeiraAnalise).Column("DT_INICIO_PRIMEIRA_ANALISE");
            Map(reg => reg.QuantidadeAnalise).Column("NR_QUANTIDADE_ANALISES");
            Map(reg => reg.HoraInicioPrimeiraAnalise).Column("HR_INICIO_PRIMEIRA_ANALISE").Nullable().CustomType<Europa.Data.Conventions.TimeSpanType>();
            Map(reg => reg.HoraFimPrimeiraAnalise).Column("HR_FIM_PRIMEIRA_ANALISE").Nullable().CustomType<Europa.Data.Conventions.TimeSpanType>();
            Map(reg => reg.DataTerminoPrimeiraAnalise).Column("DT_TERMINO_PRIMEIRA_ANALISE");
            Map(reg => reg.NomeUsuarioPrimeiraAnalise).Column("NM_USUARIO_PRIMEIRA_ANALISE");

            Map(reg => reg.DataInicioUltimaAnalise).Column("DT_INICIO_ULTIMO_ANALISE");
            Map(reg => reg.HoraInicioUltimaAnalise).Column("HR_INICIO_ULTIMO_ANALISE").Nullable().CustomType<Europa.Data.Conventions.TimeSpanType>();
            Map(reg => reg.HoraFimUltimaAnalise).Column("HR_FIM_ULTIMO_ANALISE").Nullable().CustomType<Europa.Data.Conventions.TimeSpanType>();
            Map(reg => reg.DataTerminoUltimaAnalise).Column("DT_TERMINO_ULTIMO_ANALISE");
            Map(reg => reg.NomeUsuarioUltimaAnalise).Column("NM_USUARIO_ULTIMO_ANALISE");

            Map(reg => reg.Observacao).Column("DS_OBSERVACAO");

            Map(reg => reg.IdAvalista).Column("ID_AVALISTA");
            Map(reg => reg.SituacaoDocumento).Column("TP_SITUACAO_DOCUMENTO").Nullable().CustomType<EnumType<SituacaoAprovacaoDocumento>>();
            Map(reg => reg.SituacaoAvalista).Column("TP_SITUACAO_AVALISTA").CustomType<EnumType<SituacaoAvalista>>().Nullable();

            //Sicaq Prévio
            Map(reg => reg.ParcelaAprovadaPrevio).Column("VL_PARCELA_APROVADA_PREVIO").Nullable();
            Map(reg => reg.StatusSicaqPrevio).Column("TP_STATUS_SICAQ_PREVIO").CustomType<EnumType<StatusSicaq>>();
            Map(reg => reg.DataSicaqPrevio).Column("DT_SICAQ_PREVIO").Nullable();
            Map(reg => reg.FaixaUmMeioPrevio).Column("FL_FAIXA_UM_MEIO_PREVIO").Nullable();

            //crz_grupo_cca_pre_proposta
            Map(reg => reg.IdCCAOrigem).Column("ID_CCA_ORIGEM");
            Map(reg => reg.IdCCADestino).Column("ID_CCA_DESTINO");
            Map(reg => reg.Regional).Column("NM_REGIONAL");
            Map(reg => reg.IdRegional).Column("ID_REGIONAL");
        }
    }
}
