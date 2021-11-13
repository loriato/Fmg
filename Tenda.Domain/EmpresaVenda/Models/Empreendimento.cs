using Europa.Data.Model;
using System;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models
{

    /// <summary>
    /// Representa o cadastro de determinado empreendimento Tenda, que será disponibilizado para venda
    /// </summary>
    public class Empreendimento : BaseEntity
    {
        /// <summary>
        /// Nome do Empreendimento
        /// </summary>
        public virtual string Nome { get; set; }
        /// <summary>
        /// Código identificador do Empreendimento no SUAT
        /// </summary>
        public virtual long IdSuat { get; set; }
        /// <summary>
        /// Código identificador do Empreendimento no SAP
        /// </summary>
        public virtual string Divisao { get; set; }
        /// <summary>
        /// Informação de origem SUAT, que informa se aquele empreendimento possui alguma unidade a venda, respeitando as regras e configurações SUAT/SAP
        /// </summary>
        public virtual bool DisponivelParaVenda { get; set; }
        /// <summary>
        /// Utilizado para dizer se o empreendimento em questão vai estar visível para as empresas de vendas visualizarem no BOOK
        /// </summary>
        public virtual bool DisponivelCatalogo { get; set; }
        /// <summary>
        /// Informações diversas para serem utilizadas como apoio na operação do sistema
        /// </summary>
        public virtual string Informacoes { get; set; }
        public virtual string Regional { get; set; }
        public virtual String CodigoEmpresa { get; set; }
        public virtual String NomeEmpresa { get; set; }
        public virtual String CNPJ { get; set; }
        public virtual String Mancha { get; set; }
        public virtual String RegistroIncorporacao { get; set; }
        public virtual DateTime? DataLancamento { get; set; }
        public virtual DateTime? PrevisaoEntrega { get; set; }
        public virtual DateTime? DataEntrega { get; set; }
        public virtual bool PriorizarRegraComissao { get; set; }
        public virtual string FichaTecnica { get; set; }
        public virtual TipoModalidadeComissao ModalidadeComissao { get; set; }
        public virtual TipoModalidadeProgramaFidelidade ModalidadeProgramaFidelidade { get; set; }

        public virtual Regionais RegionalObjeto { get; set; }

        public override string ChaveCandidata()
        {
            return Nome;
        }
    }
}
