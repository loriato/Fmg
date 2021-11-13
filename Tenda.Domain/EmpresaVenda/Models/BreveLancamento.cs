using Europa.Data.Model;

namespace Tenda.Domain.EmpresaVenda.Models
{
    /// <summary>
    /// Representa o cadastro de determinado Breve Lançamento Tenda, um breve lançamento será umemprendimento.
    /// Ele é usado para agilizar aas vendas enquanto o empreendimento não existe propriamente no SAP/SUAT
    /// </summary>
    public class BreveLancamento : BaseEntity
    {
        /// <summary>
        /// Nome do Empreendimento
        /// </summary>
        public virtual string Nome { get; set; }
        /// <summary>
        /// Código identificador do Empreendimento no SUAT
        /// </summary>
        public virtual bool DisponivelCatalogo { get; set; }
        /// <summary>
        /// Informações diversas para serem utilizadas como apoio na operação do sistema
        /// </summary>
        public virtual string Informacoes { get; set; }
        /// <summary>
        /// Empreendimento futuro
        /// </summary>
        public virtual Empreendimento Empreendimento { get; set; }
        public virtual string FichaTecnica { get; set; }

        public virtual Regionais Regional { get; set; }

        public virtual int? Sequencia { get; set; }

        public override string ChaveCandidata()
        {
            return Nome;
        }
    }
}
