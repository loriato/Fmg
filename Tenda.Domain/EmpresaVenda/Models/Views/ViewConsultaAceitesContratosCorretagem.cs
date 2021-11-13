using Europa.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewConsultaAceitesContratosCorretagem : BaseEntity
    {
        public virtual int IdAceiteContratoCorretagem { get; set; }
        public virtual int IdEmpresaVenda { get; set; }
        public virtual string CodigoFornecedor { get; set; }
        public virtual string CentralVendas { get; set; }
        public virtual string RazaoSocial { get; set; }
        public virtual string NomeFantasia { get; set; }
        public virtual string CNPJ { get; set; }
        public virtual string InscricaoMunicipal { get; set; }
        public virtual string CreciJuridico { get; set; }
        public virtual string InscricaoEstadual { get; set; }
        public virtual string LegislacaoFederal { get; set; }
        public virtual string Simples { get; set; }
        public virtual string SIMEI { get; set; }
        public virtual decimal LucroPresumido { get; set; }
        public virtual decimal LucroReal { get; set; }
        public virtual Core.Enums.Situacao SituacaoEmpresaVenda { get; set; }
        public virtual string Cep { get; set; }
        public virtual string Cidade { get; set; }
        public virtual string Estado { get; set; }
        public virtual string Bairro { get; set; }
        public virtual string Complemento { get; set; }
        public virtual string Numero { get; set; }
        public virtual string Logradouro { get; set; }
        public virtual string NomeCorretor { get; set; }
        public virtual string NomeLoja { get; set; }
        public virtual string NomeResponsavelTecnico { get; set; }
        public virtual string CategoriaEmpresaVenda { get; set; }
        public virtual string NumeroRegistroCRECI { get; set; }
        public virtual string SituacaoResponsavel { get; set; }

        public virtual TipoEmpresaVenda TipoEmpresaVenda { get; set; }

        public virtual string NomePessoaContato { get; set; }
        public virtual bool CorretorVisualizarClientes { get; set; }
        public virtual int AceiteIdContratoCorretagem { get; set; }
        public virtual DateTime DtAceite { get; set; }
        public virtual string NomeUsuario { get; set; }
        public virtual string EmailUsuario { get; set; }
        public virtual string LoginUsuario { get; set; }
        public virtual SituacaoUsuario UsuarioTipoSituacao { get; set; }
        public virtual int ContratoIdCriadoPor { get; set; }
        public virtual string NomeUsuarioContratoCriadoPor { get; set; }
        public virtual string NomeUsuarioContratoAtualizadoPor { get; set; }
        public virtual DateTime ContratoDtAtualizadoEm { get; set; }
        public virtual string ContentTypeDoubleCheck { get; set; }
        public virtual string DoubleCheck { get; set; }
        public virtual int IdArquivo { get; set; }
        public virtual string CodigoToken { get; set; }
        public virtual bool Ativo { get; set; }
        public virtual DateTime AcessoDtCriadoEm { get; set; }
        public virtual DateTime InicioSessao { get; set; }
        public virtual DateTime FimSessao { get; set; }
        public virtual FormaEncerramento TipoFormaEncerramento { get; set; }
        public virtual string IpOrigem { get; set; }
        public virtual string CodigoAutorizacao { get; set; }
        public virtual string Servidor { get; set; }
        public virtual string Navegador { get; set; }
        public virtual string NomeSistema { get; set; }
        public virtual int ArquivoIdCriadoPor { get; set; }
        public virtual DateTime ArquivoDtCriadoEm { get; set; }
        public virtual int ArquvioIdAtualizadoPor { get; set; }
        public virtual DateTime ArquivoDtAtualizadoEm { get; set; }
        public virtual string NomeArquivo { get; set; }
        public virtual string CodigoHash { get; set; }
        public virtual string ContentType { get; set; }
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