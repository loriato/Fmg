using Europa.Data.Model;
using System;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.Security.Enums;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewCorretor : BaseEntity
    {
        public virtual string Nome { get; set; }
        public virtual string Apelido { get; set; }
        public virtual string Rg { get; set; }
        public virtual string Cpf { get; set; }
        public virtual string Cnpj { get; set; }
        public virtual string Creci { get; set; }
        public virtual string Telefone { get; set; }
        public virtual string Email { get; set; }
        public virtual TipoFuncao Funcao { get; set; }
        public virtual DateTime DataCredenciamento { get; set; }
        public virtual SituacaoUsuario Situacao { get; set; }
        public virtual long IdEmpresaVenda { get; set; }
        public virtual string NomeEmpresaVenda { get; set; }
        public virtual string Perfis { get; set; }
        public virtual string Regional { get; set; }
        public virtual string UF { get; set; }
        public virtual string IdRegional { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
