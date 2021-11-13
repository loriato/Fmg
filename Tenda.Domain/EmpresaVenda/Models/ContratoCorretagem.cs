using Europa.Data.Model;

namespace Tenda.Domain.EmpresaVenda.Models
{
    /// <summary>
    /// Para reforçar os requisitos de auditoria, os campos do contrato de corretagem nunca poderão ser atualizados
    /// </summary>
    public class ContratoCorretagem : BaseEntity
    {
        public virtual Arquivo Arquivo { get; set; }
        public virtual string NomeDoubleCheck { get; set; }
        public virtual string HashDoubleCheck { get; set; }
        public virtual long IdArquivoDoubleCheck { get; set; }
        public virtual string ContentTypeDoubleCheck { get; set; }

        public override string ChaveCandidata()
        {
            throw new System.NotImplementedException();
        }
    }
}
