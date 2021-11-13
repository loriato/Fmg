using Europa.Data.Model;
using Europa.Extensions;
using System;
using System.Linq;
using Tenda.Domain.Core.Models;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class Corretor : Endereco
    {
        public virtual string Codigo { get; set; }
        public virtual string Nome { get; set; }
        public virtual string Email { get; set; }
        public virtual string Apelido { get; set; }
        public virtual string RG { get; set; }
        public virtual string CPF { get; set; }
        public virtual string CNPJ { get; set; }
        public virtual DateTime DataNascimento { get; set; }
        public virtual string Creci { get; set; }
        public virtual string Telefone { get; set; }
        public virtual TipoFuncao Funcao { get; set; }
        public virtual DateTime DataCredenciamento { get; set; }
        public virtual EmpresaVenda EmpresaVenda { get; set; }
        public virtual UsuarioPortal Usuario { get; set; }
        public virtual string Nacionalidade { get; set; }
        public virtual TipoCorretor TipoCorretor { get; set; }

        public override string ChaveCandidata()
        {
            return Nome;
        }

        public virtual Corretor GerarCorretorCompleto()
        {
            Funcao = TipoFuncao.Diretor;
            if (Apelido.IsEmpty())
            {
                Apelido = Nome.IsEmpty() ? "" : Nome.Split(' ').First();
            }
            if (DataCredenciamento.IsEmpty())
            {
                DataCredenciamento = DateTime.Now;
            }
            return this;
        }

    }
}
