using Europa.Data;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.Core.Models;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Security.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class TokenAceiteRepository : NHibernateRepository<TokenAceite>
    {
        public TokenAceite TokenAtivoDe(UsuarioPortal usuario, ContratoCorretagem contratoCorretagem, string token)
        {
            return Queryable()
                .Where(reg => reg.Usuario.Id == usuario.Id)
                .Where(reg => reg.ContratoCorretagem.Id == contratoCorretagem.Id)
                .Where(reg => reg.Token == token)
                .Where(reg => reg.Ativo)
                .SingleOrDefault();
        }

        public List<TokenAceite> TokenAtivoDe(Usuario usuario, ContratoCorretagem contratoCorretagem)
        {
            return Queryable()
                .Where(reg => reg.Usuario.Id == usuario.Id)
                .Where(reg => reg.ContratoCorretagem.Id == contratoCorretagem.Id)
                .Where(reg => reg.Ativo)
                .ToList();
        }

        public TokenAceite TokenAtivoDe(UsuarioPortal usuario, RegraComissao regraComissao, string token)
        {
            return Queryable()
                .Where(reg => reg.Usuario.Id == usuario.Id)
                .Where(reg => reg.RegraComissao.Id == regraComissao.Id)
                .Where(reg => reg.Token == token)
                .Where(reg => reg.Ativo)
                .SingleOrDefault();
        }

        public TokenAceite TokenAtivoDe(UsuarioPortal usuario, RegraComissaoEvs regraComissaoEvs, string token)
        {
            return Queryable()
                .Where(reg => reg.Usuario.Id == usuario.Id)
                .Where(reg => reg.RegraComissao.Id == regraComissaoEvs.RegraComissao.Id)
                .Where(reg => reg.Token == token)
                .Where(reg => reg.Ativo)
                .SingleOrDefault();
        }

        public List<TokenAceite> TokenAtivoDe(Usuario usuario, RegraComissaoEvs regraComissaoEvs)
        {
            return Queryable()
                .Where(reg => reg.Usuario.Id == usuario.Id)
                .Where(reg => reg.RegraComissao.Id == regraComissaoEvs.RegraComissao.Id)
                .Where(reg => reg.Ativo)
                .ToList();
        }
    }
}
