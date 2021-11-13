using Europa.Data.Model;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.Core.Models;

namespace Tenda.Domain.EmpresaVenda.Models
{
    /// <summary>
    /// Ponto de Venda (PDV) de determinada empresa de vendas.
    /// A respponsabilidade do cadastro é Tenda.
    /// </summary>
    public class PontoVenda : BaseEntity
    {
        public virtual string Nome { get; set; }
        public virtual Situacao Situacao { get; set; }

        /// <summary>
        /// Diz se o ponto de venda é 'bancado' ou mantido pela Tenda. Essa segregação é importante para trazer informações
        /// </summary>
        public virtual bool IniciativaTenda { get; set; }

        /// <summary>
        /// Apenas usuários do Tipo Gerente podem ser associados aqui
        /// </summary>
        public virtual Corretor Gerente { get; set; }

        public virtual EmpresaVenda EmpresaVenda { get; set; }
        public virtual UsuarioPortal Viabilizador { get; set; }

        public override string ChaveCandidata()
        {
            throw new System.NotImplementedException();
        }

        public PontoVenda()
        {
        }

        /// <summary>
        /// Cria o registro de Matriz para uma Empresa de Venda
        /// </summary>
        /// <param name="empresaVenda"></param>
        public PontoVenda(EmpresaVenda empresaVenda)
        {
            Nome = "Matriz";
            Situacao = Situacao.Ativo;
            IniciativaTenda = false;
            EmpresaVenda = empresaVenda;
        }
    }
}