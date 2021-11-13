using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ConsultaAceitesContratosCorretagemMap : BaseClassMap<ViewConsultaAceitesContratosCorretagem>
    {
        public ConsultaAceitesContratosCorretagemMap()
        {
            Table("VW_CONSULTAS_ACEITES_CONTRATOS");
            ReadOnly();
            SchemaAction.None();
            Id(reg => reg.Id).Column("ID_VW_CONSULTAS_ACEITES_CONTRATOS");
            Map(reg => reg.IdEmpresaVenda).Column("ID_EMPRESA_VENDA");
            Map(reg => reg.CodigoFornecedor).Column("CD_FORNECEDOR_SAP");
            Map(reg => reg.CentralVendas).Column("DS_CENTRAL_VENDA");
            Map(reg => reg.RazaoSocial).Column("DS_RAZAO_SOCIAL");
            Map(reg => reg.NomeFantasia).Column("NM_FANTASIA");
            Map(reg => reg.CNPJ).Column("DS_CNPJ");
            Map(reg => reg.InscricaoMunicipal).Column("DS_INSCRICAO_MUNICIPAL");
            Map(reg => reg.CreciJuridico).Column("DS_CRECI_JURIDICO");
            Map(reg => reg.InscricaoEstadual).Column("DS_INSCRICAO_ESTADUAL");
            Map(reg => reg.LegislacaoFederal).Column("DS_LEGISLACAO_FEDERAL");
            Map(reg => reg.Simples).Column("DS_SIMPLES");
            Map(reg => reg.SIMEI).Column("DS_SIMEI");
            Map(reg => reg.LucroPresumido).Column("VL_LUCRO_PRESUMIDO");
            Map(reg => reg.LucroReal).Column("VL_LUCRO_REAL");
            Map(reg => reg.SituacaoEmpresaVenda).Column("TP_SITUACAO_EMPRESA_VENDA").CustomType<EnumType<Tenda.Domain.Core.Enums.Situacao>>();
            Map(reg => reg.Cep).Column("DS_CEP");
            Map(reg => reg.Cidade).Column("DS_CIDADE");
            Map(reg => reg.Estado).Column("DS_ESTADO");
            Map(reg => reg.Bairro).Column("DS_BAIRRO");
            Map(reg => reg.Complemento).Column("DS_COMPLEMENTO");
            Map(reg => reg.Numero).Column("DS_NUMERO");
            Map(reg => reg.Logradouro).Column("DS_LOGRADOURO");
            Map(reg => reg.NomeCorretor).Column("NM_CORRETOR");
            Map(reg => reg.NomeLoja).Column("NM_LOJA");
            Map(reg => reg.NomeResponsavelTecnico).Column("NM_RESPONSAVEL_TECNICO");
            Map(reg => reg.CategoriaEmpresaVenda).Column("DS_CATEGORIA");
            Map(reg => reg.NumeroRegistroCRECI).Column("DS_REGISTRO_CRECI");
            Map(reg => reg.SituacaoResponsavel).Column("DS_SITUACAO_RESPONSAVEL");
            Map(reg => reg.TipoEmpresaVenda).Column("TP_EMPRESA_VENDA").CustomType<EnumType<TipoEmpresaVenda>>();
            Map(reg => reg.NomePessoaContato).Column("NM_PESSOA_CONTATO");
            Map(reg => reg.CorretorVisualizarClientes).Column("FL_CORRETOR_VISUALIZAR_CLIENTES");
            Map(reg => reg.AceiteIdContratoCorretagem).Column("ID_ACEITE_CONTRATO_CORRETAGEM");
            Map(reg => reg.DtAceite).Column("DT_ACEITE");
            Map(reg => reg.NomeUsuario).Column("NM_USUARIO_TOKEN_ACEITE");
            Map(reg => reg.EmailUsuario).Column("DS_EMAIL");
            Map(reg => reg.LoginUsuario).Column("DS_LOGIN");
            Map(reg => reg.UsuarioTipoSituacao).Column("TP_SITUACAO_USUARIO").CustomType<EnumType<SituacaoUsuario>>(); ;
            Map(reg => reg.NomeUsuarioContratoCriadoPor).Column("NM_USUARIO_CONTRATO_CRIADO_POR");
            Map(reg => reg.NomeUsuarioContratoAtualizadoPor).Column("NM_USUARIO_CONTRATO_ATUALIZADO_POR");
            Map(reg => reg.ContentTypeDoubleCheck).Column("DS_CONTENT_TYPE_DOUBLE_CHECK");
            Map(reg => reg.DoubleCheck).Column("NM_DOUBLE_CHECK");
            Map(reg => reg.IdArquivo).Column("ID_ARQUIVO");
            Map(reg => reg.CodigoToken).Column("CD_TOKEN");
            Map(reg => reg.Ativo).Column("FL_ATIVO");
            Map(reg => reg.AcessoDtCriadoEm).Column("DT_CRIADO_EM_ACESSO");
            Map(reg => reg.InicioSessao).Column("DT_INICIO_SESSAO");
            Map(reg => reg.FimSessao).Column("DT_FIM_SESSAO");
            Map(reg => reg.TipoFormaEncerramento).Column("TP_FORMA_ENCERRAMENTO").CustomType<EnumType<FormaEncerramento>>(); ;
            Map(reg => reg.IpOrigem).Column("DS_IP_ORIGEM");
            Map(reg => reg.CodigoAutorizacao).Column("CD_AUTORIZACAO");
            Map(reg => reg.Servidor).Column("DS_SERVIDOR");
            Map(reg => reg.Navegador).Column("DS_NAVEGADOR");
            Map(reg => reg.NomeSistema).Column("NM_SISTEMA");
            Map(reg => reg.ArquivoIdCriadoPor).Column("ID_CRIADO_POR_ARQUIVO");
            Map(reg => reg.ArquivoDtCriadoEm).Column("DT_CRIADO_EM_ARQUIVO");
            Map(reg => reg.ArquvioIdAtualizadoPor).Column("ID_ATUALIZADO_POR_ARQUVIO");
            Map(reg => reg.ArquivoDtAtualizadoEm).Column("DT_ATUALIZADO_EM_ARQUIVO");
            Map(reg => reg.NomeArquivo).Column("NM_ARQUIVO");
            Map(reg => reg.CodigoHash).Column("CD_HASH");
            Map(reg => reg.ContentType).Column("DS_CONTENT_TYPE");
            Map(reg => reg.ByThumbnail).Column("BY_THUMBNAIL");
            Map(reg => reg.NrContentLength).Column("NR_CONTENT_LENGTH");
            Map(reg => reg.FileExtension).Column("DS_FILE_EXTENSION");
            Map(reg => reg.Metadados).Column("DS_METADADOS");
            Map(reg => reg.FalhaExtMetadados).Column("FL_FALHA_EXT_METADADOS");
        }
    }
}