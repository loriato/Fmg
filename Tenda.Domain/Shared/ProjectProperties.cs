using Europa.Extensions;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using Tenda.Domain.Security.Data;
using Tenda.Domain.Security.Models;
using Tenda.Domain.Shared.Log;
using Tenda.Domain.Shared.Models;

namespace Tenda.Domain.Shared
{
    public static class ProjectProperties
    {
        #region Implementação Inicial de Fila Personalziada

        //{"LojasIn":[],"LojasNotIn":[],"EmpreendimentosIn":[],"EmpreendimentosNotIn":[],"EstadosIn":[],"EstadosNotIn":[],"RegionaisIn":[],"RegionaisNotIn":[], "NodesIn":[], "StatusSicaqIn":[]}
        public static FilaPersonalizadaDTO FiltrosDeFila(string codIdentificador, int timeToLive = 1)
        {
            //FIXME: use GetJsonParameter instead

            if (codIdentificador == null)
            {
                return null;
            }

            var property = GetParameter(codIdentificador, timeToLive);
            if (property == null)
            {
                return null;
            }

            FilaPersonalizadaDTO dto = (FilaPersonalizadaDTO)new JavaScriptSerializer()
                .Deserialize(property.Value.ToString(), typeof(FilaPersonalizadaDTO));
            return dto;
        }

        #endregion

        #region ApiConfiguration

        public static class ApiConfiguration
        {
            private const string EnableRequestResponseLogProperty = "fl_enable_request_response_log";

            public static bool EnableRequestResponseLog => GetBoolProperty(EnableRequestResponseLogProperty);
        }

        #endregion

        #region External services configurations

        public static class EmailConfigurations
        {
            private const string EmailParaEnvioProperty = "ds_email_para_envio";

            private const string EmailAtendimentoUnificadoProperty = "ds_email_atendimento_unificado";

            private const string AutenticacaoEmailParaEnvioProperty = "ds_autenticacao_email_para_envio";

            private const string UseGmailProperty = "fl_usar_configuracoes_gmail";

            private const string DefaultSmtpHostProperty = "default_smtp_host";

            private const string DefaultSmtpPortProperty = "default_smtp_port";

            private const string FlagEnviarEmailProperty = "fl_enviar_email";


            private const string EmailEnvioDevModeProperty = "ds_email_envio_dev";
            public static string EmailParaEnvio => GetStringProperty(EmailParaEnvioProperty);
            public static string AutenticacaoEmailParaEnvio => GetStringProperty(AutenticacaoEmailParaEnvioProperty);
            public static bool UseGmail => GetBoolProperty(UseGmailProperty);
            public static string DefaultSmtpHost => GetStringProperty(DefaultSmtpHostProperty);
            public static int DefaultSmtpPort => GetIntProperty(DefaultSmtpPortProperty);
            public static bool FlagEnviarEmail => GetBoolProperty(FlagEnviarEmailProperty);
            public static string EmailEnvioDevMode => GetStringProperty(EmailEnvioDevModeProperty);
            public static string EmailAtendimentoUnificado => GetStringProperty(EmailAtendimentoUnificadoProperty);
        }

        #endregion

        #region Static Definitions

        public static string CsEmpresaVenda { get; } = "CS_WEB";
        public static string CsEmpresaVendaPortal { get; } = "CS_WEB_PORTAL";
        public static string CsSeguranca { get; } = "CS_SEG";

        public static string Codigo { get; } = "DES0000";
        public static string CodigoEmpresaVendaPortal { get; } = "POR1818";
        #endregion

        #region Properties Code Declaration

        private const string CurrentServerInfoProperty = "server_info";
        private const int CurrentTimeToLiveProperty = 60;
        private const int AlwaysLiveData = -1;

        // Integrações Diversas
        private const string EndpointCepProperty = "zeus_edp_consulta_cep";

        // Diretório usado para importação de arquivos de preço raso
        private const string DiretorioArquivosTemporariosProperty = "emvs_dir_arquivos_temporarios";
        private const string LoginViaActiveDirectoryProperty = "emvs_fl_login_via_active_directory";
        private const string SincronizarGruposActiveDirectoryProperty = "emvs_sincronizar_grupos_active_directory";

        private const string FonetizaProperty = "emvs_fl_fonetica_fonetiza";
        private const string HabilitarFoneticaProperty = "emvs_fl_habilitar_fonetica";
        private const string MidasPassthroughProperty = "emvs_fl_midas_passthrough";

        private const string SuatApiUsuarioEvsProperty = "emvs_suat_api_usuario_evs";
        private const string SuatApiEndpointProperty = "emvs_suat_api_endpoint";
        private const string SuatApiTokenProperty = "emvs_suat_api_token";

        private const string SuatBaseUrlProperty = "emvs_suat_url_base";
        private const string SuatUrlDetalharEmpreendimentoProperty = "emvs_suat_url_detalhar_empreendimento";
        private const string SuatUrlDetalharTorreProperty = "emvs_suat_url_detalhar_torre";
        private const string SuatUrlDetalharUnidadeProperty = "emvs_suat_url_detalhar_unidade";

        private const string TamanhoPaginaRoboConsolidadoPrePropostaProperty =
            "emvs_consolidacao_pre_proposta_bot_page_size";

        private const string TamanhoPaginaRoboAvaliacaoValidadeDocumentoProperty =
            "emvs_avaliacao_validade_documento_bot_page_size";

        private const string IdsDocumentosProponenteDataExpiracaoObrigatoriaProperty =
            "emvs_ids_documentos_proponente_data_expiracao_obrigatoria";

        private const string UsuarioSistemaProperty = "emvs_usuario_sistema";

        private const string MotivoDocumentoAnexadoOutroProponenteProperty = "emvs_motivo_anexado_outro_proponente";

        private const string VirtualDirectoryFileProperty = "emvs_virtual_directory_file";
        private const string VirtualDirectoryThumbnailProperty = "emvs_virtual_directory_thumbnail";

        private const string IdPerfilAdministradorProperty = "emvs_id_perfil_administrador";
        private const string IdPerfilViabilizadorProperty = "emvs_id_perfil_viabilizador";
        private const string IdPerfilDiretorPortalEvsProperty = "emvs_id_perfil_diretor_portal_evs";
        private const string IdPerfilViabilizadoHouseProperty = "emvs_id_perfil_viabilizador_house";
        private const string IdPerfilSupervisorCicloFinanceiroProperty = "emvs_id_perfil_supervisor_ciclo_financeiro";
        private const string IdPerfilCoordenadorCicloFinanceiroProperty = "emvs_id_perfil_coordenador_ciclo_financeiro";

        private const string IdPoliticaPrivacidadeProperty = "emvs_id_politica_privacidade";

        public const string SuatMidiaPropostaProperty = "emvs_suat_id_midia_proposta";

        private const string LinkAtivacaoUsuarioLogadoProperty = "link_ativacao_usuario_logado";

        private const string LinkAtivacaoUsuarioProperty = "link_ativacao_usuario";

        private const string QuantidadeHorasInvalidacaoTokenUsuarioProperty = "qtd_horas_invalidacao_token_usuario";

        private const string QuatidadeEmailsProperty = "qtd_emails";

        private const string IdUsuarioSistemaProperty = "id_usuario_sistema";

        private const string EvsBaseUrlProperty = "emvs_evs_url_base";
        private const string EvsAdminBaseUrlProperty = "emvs_evs_admin_url_base";
        private const string PortalHouseBaseUrlProperty = "emvs_evs_house_url_base";

        private const string SimuladorBaseUrlApiProperty = "emvs_simulador_url_base_api";
        private const string UsuarioApiSimuladorProperty = "emvs_usuario_api_simulador";
        private const string SimuladorApiTokenProperty = "emvs_simulador_api_token";

        private const string ConectaUrlBaseApiProperty = "emvs_conecta_url_base_api";
        private const string ConectaApiTokenProperty = "emvs_conecta_api_token";
        private const string ConectaUrlBaseProperty = "emvs_conecta_url_base";

        private const string MidasUrlZeusServiceEvsApiProperty = "emvs_midas_url_zeus_service_evs";
        private const string MidasTokenZeusServiceEvsApiProperty = "emvs_midas_token_zeus_service_evs";
        private const string MidasRegionaisListProperty = "emvs_ds_regionais_midas";

        private const string PhysicalDirectoryFileProperty = "emvs_physical_directory_file";

        private const string FilaDestaqueProperty = "emvs_codigo_fila_destaque";

        private const string DataBuscaBoletosPropostasProperty = "data_buscas_boletos_propostas";

        private const string DataBuscaAtualizacoesPropostasProperty = "data_busca_atualizacao_proposta";

        private const string DataBuscaAtualizacoesIdSapClienteProperty = "data_busca_atualizacao_sap_cliente";

        private const string DataInicioProgramaFidelidadeProperty = "data_inicio_programa_fidelidade";

        private const string DiasBuscaBoletosPropostasProperty = "dias_busca_boletos_propostas";

        private const string DiasBuscaAtualizacoesPropostasProperty = "dias_busca_atualizacao_proposta";

        private const string DiasBuscaAtualizacoesIdSapClienteProperty = "dias_busca_atualizacao_sap_cliente";

        private const string DiasExportartodosLeadProperty = "dias_exportar_todos_lead";

        private const string DiasBuscaIntegracaoEmpreendimentoSuatProperty =
            "dias_busca_integracao_empreendimento_suat";

        private const string DiasBuscaIntegracaoLojaSuatProperty = "dias_busca_integracao_loja_suat";

        private const string PageSizeRepasseJunixProperty = "emvs_page_size_repasse_junix";

        private const string FilasSUATProperty = "emvs_filas_suat";

        private const string AtivarRefreshViewRelatorioComissaoProperty = "emvs_ativar_refresh_vw_rel_comissao";

        private const string ExibirModalBannerShowProperty = "exibir_modal_banner_show";

        private const string AnoFiltroDataRepasseProperty = "ano_filtro_data_repasse";
        private const string DataBuscaVendaGeradaProperty = "data_busca_venda_gerada";

        private const string Planta3DLink4Property = "link_planta3d_faixa2_t4";
        private const string Planta3DLink7Property = "link_planta3d_faixa2eplus_t7";

        private const string UrlSimuladorProperty = "emvs_url_simulador";

        private const string TempoReenvioNovoTokenProperty = "tempo_reenvio_novo_token";

        private const string ZeusUrlBaseProperty = "emvs_zeus_url_base";
        private const string ZeusUrlGerarRequisicaoProperty = "emvs_zeus_url_gerar_requisicao";
        private const string ZeusUrlBuscarNumeroPedidoProperty = "emvs_zeus_url_buscar_numero_pedido";

        private const string IdsPerfisLoginEVSuspensaProperty = "emvs_ids_perfil_evs_suspensas";

        private const string AtributosClientePortalHouseProperty = "ds_atributos_cliente_portal_house";

        private const string IdPerfilAgenteVendasProperty = "emvs_id_perfil_agente_venda";
        private const string IdPerfilCoordenadorHouseProperty = "emvs_id_perfil_coordenador_house";
        private const string IdPerfilSupervisorHouseProperty = "emvs_id_perfil_supervisor_house";
        private const string IdPerfilSuperiorHouseProperty = "emvs_id_perfil_superior_house";

        private const string IndiqueUrlBaseApiProperty = "emvs_indique_url_base_api";
        private const string IndiqueApiTokenProperty = "emvs_indique_api_token";

        private const string InicioMidas = "data_inicio_midas";
        private const string ValidarCamposOrigemIndicacaoProperty = "validar_campos_origem_indicacao";
        #endregion

        #region Properties Declaration (To Use as Code Reference)
        public static string EndpointCep => GetStringProperty(EndpointCepProperty);
        public static string DiretorioArquivosTemporarios => GetStringProperty(DiretorioArquivosTemporariosProperty);
        public static bool LoginViaActiveDirectory => GetBoolProperty(LoginViaActiveDirectoryProperty);

        public static bool SincronizarGruposActiveDirectory =>
            GetBoolProperty(SincronizarGruposActiveDirectoryProperty);

        public static bool Fonetiza => GetBoolProperty(FonetizaProperty);
        public static bool HabilitarFonetica => GetBoolProperty(HabilitarFoneticaProperty);
        public static bool MidasPassthrough => GetBoolProperty(MidasPassthroughProperty);
        public static string SuatApiEndpoint => GetStringProperty(SuatApiEndpointProperty);
        public static string SuatApiToken => GetStringProperty(SuatApiTokenProperty);
        public static string SuatBaseUrl => GetStringProperty(SuatBaseUrlProperty);
        public static string SuatUrlDetalharEmpreendimento => GetStringProperty(SuatUrlDetalharEmpreendimentoProperty);
        public static string SuatUrlDetalharTorre => GetStringProperty(SuatUrlDetalharTorreProperty);
        public static string SuatUrlDetalharUnidade => GetStringProperty(SuatUrlDetalharUnidadeProperty);

        public static int TamanhoPaginaRoboConsolidadoPreProposta =>
            GetIntProperty(TamanhoPaginaRoboConsolidadoPrePropostaProperty);

        public static int TamanhoPaginaRoboAvaliacaoValidadeDocumento =>
            GetIntProperty(TamanhoPaginaRoboAvaliacaoValidadeDocumentoProperty);

        public static List<long> IdsDocumentosProponenteComDataExpiracaoObrigatoria =>
            (List<long>)GetJsonParameter(IdsDocumentosProponenteDataExpiracaoObrigatoriaProperty, typeof(List<long>));

        public static string UsuarioSistema => GetStringProperty(UsuarioSistemaProperty);

        public static long MotivoDocumentoAnexadoOutroProponente =>
            GetLongProperty(MotivoDocumentoAnexadoOutroProponenteProperty);

        public static VirtualDirectoryDto VirtualDirectoryFile =>
            (VirtualDirectoryDto)GetJsonParameter(VirtualDirectoryFileProperty, typeof(VirtualDirectoryDto));

        public static VirtualDirectoryDto VirtualDirectoryThumbnail =>
            (VirtualDirectoryDto)GetJsonParameter(VirtualDirectoryThumbnailProperty, typeof(VirtualDirectoryDto));

        public static long IdPerfilAdministrador => GetLongProperty(IdPerfilAdministradorProperty);
        public static long IdPerfilViabilizador => GetLongProperty(IdPerfilViabilizadorProperty);
        public static long IdPerfilDiretorPortalEvs => GetLongProperty(IdPerfilDiretorPortalEvsProperty);
        public static long IdPerfilViabilizadorHouse => GetLongProperty(IdPerfilViabilizadoHouseProperty);
        public static long IdPerfilSupervisorCicloFinanceiro => GetLongProperty(IdPerfilSupervisorCicloFinanceiroProperty);
        public static long IdPerfilCoordenadorCicloFinanceiro => GetLongProperty(IdPerfilCoordenadorCicloFinanceiroProperty);
        public static long IdPoliticaPrivacidade => GetLongProperty(IdPoliticaPrivacidadeProperty);

        public static long SuatMidiaProposta => GetLongProperty(SuatMidiaPropostaProperty);
        public static string LinkAtivacaoUsuario => GetStringProperty(LinkAtivacaoUsuarioProperty);
        public static string LinkAtivacaoUsuarioLogado => GetStringProperty(LinkAtivacaoUsuarioLogadoProperty);

        public static int QuantidadeHorasInvalidacaoTokenUsuario =>
            GetIntProperty(QuantidadeHorasInvalidacaoTokenUsuarioProperty);

        public static int QuatidadeEmails => GetIntProperty(QuatidadeEmailsProperty);
        public static long IdUsuarioSistema => GetLongProperty(IdUsuarioSistemaProperty);
        public static string EvsBaseUrl => GetStringProperty(EvsBaseUrlProperty);
        public static string EvsAdminBaseUrl => GetStringProperty(EvsAdminBaseUrlProperty);
        public static string PortalHouseBaseUrl => GetStringProperty(PortalHouseBaseUrlProperty);

        public static string SimuladorBaseUrlApi => GetStringProperty(SimuladorBaseUrlApiProperty);
        public static string SimuladorApiToken => GetStringProperty(SimuladorApiTokenProperty);
        public static LoginRequestDto UsuarioSimuladorApi => (LoginRequestDto)GetJsonParameter(UsuarioApiSimuladorProperty, typeof(LoginRequestDto));

        public static string ConectaUrlBaseApi => GetStringProperty(ConectaUrlBaseApiProperty);
        public static string ConectaApiToken => GetStringProperty(ConectaApiTokenProperty);
        public static string ConectaUrlBase => GetStringProperty(ConectaUrlBaseProperty);

        public static string MidasUrlZeusServiceEvsApi => GetStringProperty(MidasUrlZeusServiceEvsApiProperty);
        public static string MidasTokenZeusServiceEvsApi => GetStringProperty(MidasTokenZeusServiceEvsApiProperty);
        public static string MidasRegionaisList => GetStringProperty(MidasRegionaisListProperty);

        public static string FilaDestaque => GetStringProperty(FilaDestaqueProperty);

        public static PhysicalDirectoryFileDTO PhysicalDirectoryFile =>
            (PhysicalDirectoryFileDTO)GetJsonParameter(PhysicalDirectoryFileProperty,
                typeof(PhysicalDirectoryFileDTO));

        public static int PageSizeRepasseJunix => GetIntProperty(PageSizeRepasseJunixProperty);

        public static int DiasBuscaBoletosPropostas => GetIntProperty(DiasBuscaBoletosPropostasProperty);

        public static int DiasBuscaAtualizacoesPropostas => GetIntProperty(DiasBuscaAtualizacoesPropostasProperty);

        public static int DiasBuscaAtualizacoesIdSapCliente =>
            GetIntProperty(DiasBuscaAtualizacoesIdSapClienteProperty);

        public static double DiasExportarTodosLeads =>
            GetIntProperty(DiasExportartodosLeadProperty);

        public static int DiasBuscaIntegracaoEmpreendimentoSuat =>
            GetIntProperty(DiasBuscaIntegracaoEmpreendimentoSuatProperty);

        public static int DiasBuscaIntegracaoLojaSuat => GetIntProperty(DiasBuscaIntegracaoLojaSuatProperty);

        public static DateTime DataBuscaBoletosPropostas => GetDateTimeProperty(DataBuscaBoletosPropostasProperty);

        public static DateTime DataBuscaAtualizacoesPropostas =>
            GetDateTimeProperty(DataBuscaAtualizacoesPropostasProperty);

        public static DateTime DataBuscaAtualizacoesIdSapCliente =>
            GetDateTimeProperty(DataBuscaAtualizacoesIdSapClienteProperty);

        public static DateTime DataInicioProgramaFidelidade =>
            GetDateTimeProperty(DataInicioProgramaFidelidadeProperty);

        public static List<FilaAuxiliarDTO> FilasSUAT =>
            (List<FilaAuxiliarDTO>)GetJsonParameter(FilasSUATProperty, typeof(List<FilaAuxiliarDTO>), 1);

        public static long IdUsuarioEvsApiSuat => GetLongProperty(SuatApiUsuarioEvsProperty);

        public static bool AtivarRefreshViewRelatorioComissao =>
            GetBoolProperty(AtivarRefreshViewRelatorioComissaoProperty);

        public static bool ExibirModalBannerShow => GetBoolProperty(ExibirModalBannerShowProperty);

        public static int AnoFiltroDataRepasse => GetIntProperty(AnoFiltroDataRepasseProperty);
        public static DateTime DataBuscaVendaGerada => GetDateTimeProperty(DataBuscaVendaGeradaProperty);

        public static string Planta3DLink4 => GetStringProperty(Planta3DLink4Property);
        public static string Planta3DLink7 => GetStringProperty(Planta3DLink7Property);

        public static string UrlSimulador => GetStringProperty(UrlSimuladorProperty);

        public static int TempoReenvioNovoToken => GetIntProperty(TempoReenvioNovoTokenProperty);
        public static string ZeusUrlGerarRequisicao => GetStringProperty(ZeusUrlGerarRequisicaoProperty);
        public static string ZeusUrlBuscarNumeroPedido => GetStringProperty(ZeusUrlBuscarNumeroPedidoProperty);
        public static string ZeusUrlBase => GetStringProperty(ZeusUrlBaseProperty);
        public static List<long> IdsPerfisLoginEVSuspensa => (List<long>)GetJsonParameter(IdsPerfisLoginEVSuspensaProperty, typeof(List<long>));

        public static List<string> AtributosClientePortalHouse =>
                (List<string>)GetJsonParameter(AtributosClientePortalHouseProperty, typeof(List<string>));
        public static string IndiqueUrlBaseApi => GetStringProperty(IndiqueUrlBaseApiProperty);
        public static string IndiqueApiToken => GetStringProperty(IndiqueApiTokenProperty);



        public static long IdPerfilAgenteVenda => GetLongProperty(IdPerfilAgenteVendasProperty);
        public static long IdPerfilCoordenadorHouse => GetLongProperty(IdPerfilCoordenadorHouseProperty);
        public static long IdPerfilSupervisorHouse => GetLongProperty(IdPerfilSupervisorHouseProperty);

        public static DateTime DataInicioMidas => GetDateTimeProperty(InicioMidas);
        public static bool ValidarCamposOrigemIndicacao => GetBoolProperty(ValidarCamposOrigemIndicacaoProperty);



        public static long IdPerfilSuperiorHouse => GetLongProperty(IdPerfilSuperiorHouseProperty);
        #endregion

        #region IIS Properties Replacement

        private static IDictionary<string, ProjectPropertyDto> _projectProperties =
            new Dictionary<string, ProjectPropertyDto>();

        private static ProjectPropertyDto GetParameter(string key, int timeToLive)
        {
            ProjectPropertyDto property = null;

            if (timeToLive == 0)
            {
                timeToLive = CurrentTimeToLiveProperty;
            }

            if (_projectProperties.TryGetValue(key, out property))
            {
                // Update parameter strategy.
                TimeSpan difference = DateTime.Now - property.LastUpdate;
                if (difference.TotalMinutes < timeToLive && timeToLive != AlwaysLiveData)
                {
                    return property;
                }
            }

            Parametro parametro = FindByKey(key);
            if (parametro != null)
            {
                property = SetParameter(key, parametro.Valor);
            }

            return property;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static ProjectPropertyDto SetParameter(string key, object value)
        {
            ProjectPropertyDto property = new ProjectPropertyDto();
            property.Key = key;
            property.LastUpdate = DateTime.Now;
            property.Value = value;
            try
            {
                if (_projectProperties.ContainsKey(key))
                {
                    _projectProperties.Remove(key);
                }

                _projectProperties.Add(key, property);
            }
            catch (Exception e)
            {
                Exception exc = new Exception(key, e);
                ExceptionLogger.LogException(exc);
            }

            return property;
        }

        private static Parametro FindByKey(string key)
        {
            IStatelessSession session = null;
            Parametro parametro = null;
            try
            {
                session = NHibernateSession.StatelessSession(CsEmpresaVenda);
                parametro = session.Query<Parametro>()
                    .Where(reg => reg.Chave.ToLower() == key.ToLower())
                    .FirstOrDefault();
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
            }
            finally
            {
                session.CloseIfOpen();
            }

            return parametro;
        }

        private static bool GetBoolProperty(string key, int timeToLive = 0)
        {
            ProjectPropertyDto property = GetParameter(key, timeToLive);
            if (property == null)
            {
                return false;
            }

            return Convert.ToBoolean(property.Value.ToString());
        }

        private static string GetStringProperty(string key, int timeToLive = 0)
        {
            ProjectPropertyDto property = GetParameter(key, timeToLive);
            if (property == null)
            {
                return null;
            }

            return property.Value.ToString();
        }

        private static long GetLongProperty(string key, int timeToLive = 0)
        {
            ProjectPropertyDto property = GetParameter(key, timeToLive);
            if (property == null)
            {
                return 0;
            }

            return Convert.ToInt64(property.Value);
        }

        private static DateTime GetDateTimeProperty(string key, int timeToLive = 0)
        {
            ProjectPropertyDto property = GetParameter(key, timeToLive);
            try
            {
                if (property != null)
                {
                    return Convert.ToDateTime(property.Value);
                }
            }
            catch (FormatException exc)
            {
                ExceptionLogger.LogException(exc);
            }

            // Should Use MinValue or Null?
            return DateTime.MinValue;
        }

        private static int GetIntProperty(string key, int timeToLive = 0)
        {
            ProjectPropertyDto property = GetParameter(key, timeToLive);
            if (property == null)
            {
                return 0;
            }

            return Convert.ToInt32(property.Value);
        }

        public static object GetJsonParameter(string key, Type targetType, int timeToLive = 0)
        {
            ProjectPropertyDto property = GetParameter(key, timeToLive);
            if (property == null)
            {
                return null;
            }

            return new JavaScriptSerializer()
                .Deserialize(property.Value.ToString(), targetType);
        }

        public static void ClearPropertiesCache()
        {
            _projectProperties = new Dictionary<string, ProjectPropertyDto>();
            AddServerInfo();
        }

        public static IDictionary<string, ProjectPropertyDto> CurrentProperties()
        {
            var currentProperties = new Dictionary<string, ProjectPropertyDto>(_projectProperties);
            var removeForSecurity = currentProperties
                .Where(reg =>
                    reg.Key.ToLower().Contains("password") ||
                    reg.Key.ToLower().Contains("senha"))
                .Select(reg => reg.Key)
                .ToList();

            foreach (var keyToRemove in removeForSecurity)
            {
                currentProperties.Remove(keyToRemove);
            }

            return currentProperties;
        }

        private static void AddServerInfo()
        {
            Dictionary<string, string> serverParameters = new Dictionary<string, string>();
            serverParameters.Add("Enviroment.MachineName", Environment.MachineName);

            ProjectPropertyDto property = new ProjectPropertyDto();
            property.Key = CurrentServerInfoProperty;
            property.LastUpdate = DateTime.Now.AddDays(CurrentTimeToLiveProperty);
            property.Value = serverParameters;
            _projectProperties.Add(property.Key, property);
        }

        #endregion

        #region Olders Definitions (DO NOT CREATE NEW ONES HERE)

        private static readonly string IsProductionModeProperty = "IsProductionMode";
        public static bool IsProductionMode => GetBoolProperty(IsProductionModeProperty);


        public static void UpdateParameter(string key, string value)
        {
            IStatelessSession session = null;
            Parametro parametro = null;
            try
            {
                session = NHibernateSession.StatelessSession(CsEmpresaVenda);
                parametro = session.Query<Parametro>()
                    .Where(reg => reg.Chave.ToLower() == key.ToLower())
                    .FirstOrDefault();

                if (parametro != null)
                {
                    parametro.Valor = value;

                    session.Update(parametro);
                }
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
            }
            finally
            {
                session.CloseIfOpen();
            }
        }

        #endregion
    }
}