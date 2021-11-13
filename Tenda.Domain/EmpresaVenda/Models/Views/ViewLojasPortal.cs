using Europa.Data.Model;
using System;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.Security.Enums;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewLojasPortal : BaseEntity
    {
        public virtual string Nome { get; set; }
        public virtual string NomeComercial { get; set; }
        public virtual string PessoaContato { get; set; }
        public virtual string TelefoneContato { get; set; }
        public virtual string Cidade { get; set; }
        public virtual string Logradouro { get; set; }
        public virtual string Bairro { get; set; }
        public virtual string Cep { get; set; }
        public virtual string Numero { get; set; }
        public virtual string Complemento { get; set; }
        public virtual string Estado { get; set; }
        public virtual long IdCentralVenda { get; set; }
        public virtual string NomeCentralVenda { get; set; }
        public virtual Situacao Situacao { get; set; }
        public virtual TipoSimNao ConsiderarUF { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
