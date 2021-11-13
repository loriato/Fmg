using Europa.Data.Model;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class PontuacaoFidelidadeEmpresaVenda : BaseEntity
    {
        public virtual EmpresaVenda EmpresaVenda { get; set; }
        public virtual decimal PontuacaoTotal { get; set; }
        public virtual decimal PontuacaoIndisponivel { get; set; }
        public virtual decimal PontuacaoDisponivel { get; set; }
        public virtual decimal PontuacaoResgatada { get; set; }
        public virtual decimal PontuacaoSolicitada { get; set; }


        public override string ChaveCandidata()
        {
            return EmpresaVenda.NomeFantasia;
        }
    }
}
