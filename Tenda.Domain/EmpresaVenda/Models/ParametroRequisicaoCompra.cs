using Europa.Data.Model;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class ParametroRequisicaoCompra : BaseEntity
    {
        public virtual string Codigo { get; set; }
        public virtual string Descricao { get; set; }
        public virtual string Valor { get; set; }
        public virtual string Tipo { get; set; }

        public override string ChaveCandidata()
        {
            return Codigo;
        }
    }
}
