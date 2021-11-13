using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Europa.Data.Model;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Models;


namespace Tenda.Domain.EmpresaVenda.Models
{
    public class ConsultaAceitesContratosCorretagem : BaseEntity
    {
        
        public virtual int IdAceiteContratoCorretagem { get; set; }
        #region EVS
        public virtual int IdEmpresaVenda { get; set; }
        public virtual string CodigoFornecedor { get; set; }
        public virtual string CentralVendas { get; set; } 
        public virtual string RazaoSocial { get; set; }
        public virtual string NomeFantasia { get; set; }
        public virtual string CNPJ { get; set; }
        public virtual string CreciJuridico { get; set; }
        public virtual string InscricaoMunicipal { get; set; }
        public virtual string InscricaoEstadual { get; set; }
        public virtual string LegislacaoFederal { get; set; }
        public virtual string Simples { get; set; }
        public virtual string SIMEI { get; set; }
        public virtual decimal LucroPresumido { get; set; }
        public virtual decimal LucroReal { get; set; }
        public virtual Core.Enums.Situacao SituacaoEmpresaVenda { get; set; }
        public virtual string FotoFachadaUrl { get; set; }
        public virtual string NomeResponsavelTecnico { get; set; }
        public virtual string CategoriaEmpresaVenda { get; set; }
        public virtual string NumeroRegistroCRECI { get; set; }
        public virtual string SituacaoResponsavel { get; set; }
        public virtual bool CorretorVisualizarClientes { get; set; }
        #endregion
        public virtual DateTime DtAceite { get; set; }
        public virtual int AceiteIdContratoCorretagem { get; set; }
        public virtual int IdAprovador { get; set; }
        public virtual int ContratoIdCriadoPor { get; set; }
        public virtual int ContratoIdAtualizadoPor { get; set; }
        public virtual DateTime ContratoDtAtualizadoEm { get; set; }
        public virtual string ContentTypeDoubleCheck { get; set; }
        public virtual string DoubleCheck { get; set; }
        public virtual int IdArquivoDoubleCheck { get; set; }
        public virtual int IdArquivo { get; set; }
        public virtual string Cep { get; set; }
        public virtual string Cidade { get; set; }
        public virtual string Estado { get; set; }
        public virtual string Pais { get; set; }
        public virtual string Bairro { get; set; }
        public virtual string Complemento { get; set; }
        public virtual string Numero { get; set; }
        public virtual string Logradouro { get; set; }
        public virtual int IdCorretor { get; set; }
        public virtual int IdFotoFachada { get; set; }
        public virtual int IdLoja { get; set; }
        public virtual string NmResponsavelTecnico { get; set; }
        public virtual string RegistroCreci { get; set; }
        public virtual string NmPessoaContato { get; set; }
        public virtual string TelefoneContato { get; set; }
        public virtual int TpEmpresaVenda { get; set; }
        public virtual int IdAcesso { get; set; }
        public virtual int IdUsuario { get; set; }
        public virtual string Email { get; set; }
        public virtual string NmUsuario { get; set; }
        public virtual SituacaoUsuario UsuarioTpSituacao { get; set; }
        public virtual int TokenAtivacao { get; set; }
        public virtual DateTime DtAtivacaoToken { get; set; }
        public virtual int IdTokenAceite { get; set; }
        public virtual string CdToken { get; set; }
        public virtual bool Ativo { get; set; }
        public virtual int IdContratoCorretagem { get; set; }
        public virtual int IdRegraComissao { get; set; }
        public virtual int AcessoIdCriadoPor { get; set; }
        public virtual DateTime AcessoDtCriadoEm { get; set; }
        public virtual int AcessoIdAtualizadoPor { get; set; }
        public virtual DateTime AcessoDtAtualizadoEm { get; set; }
        public virtual DateTime InicioSessao { get; set; }
        public virtual DateTime FimSessao { get; set; }
        public virtual int TpFormaEncerramento { get; set; }
        public virtual string IpOrigem { get; set; }
        public virtual string CdAutorizacao { get; set; }
        public virtual string Servidor { get; set; }
        public virtual string Navegador { get; set; }
        public virtual int IdSistema { get; set; }
        public virtual int ArquivoIdCriadoPor { get; set; }
        public virtual DateTime ArquivoDtCriadoEm { get; set; }
        public virtual int ArquvioIdAtualizadoPor { get; set; }
        public virtual DateTime ArquivoDtAtualizadoEm { get; set; }
        public virtual string NmArquivo { get; set; }
        public virtual string CdHash { get; set; }
        public virtual string ContentType { get; set; }
        //Bytea
        public virtual byte[] ByContent { get; set; }
        public virtual byte[] ByThumbnail { get; set; }
        public virtual int NrContentLength { get; set; }
        public virtual string FileExtension { get; set; }
        public virtual string Metadados { get; set; }
        public virtual bool FalhaExtMetadados { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
