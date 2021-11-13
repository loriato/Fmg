using System.Collections.Generic;
using System.Linq;
using Europa.Data;
using NHibernate;
using Tenda.Domain.Security.Models;
namespace Tenda.Domain.Security.Repository
{
    public class PermissaoRepository : NHibernateRepository<Permissao>
    {
        public PermissaoRepository(ISession session) : base(session)
        {
        }
        public Dictionary<string, List<string>> Permissoes(string systemCode, List<long> profilesSku)
        {
            return Queryable()
                .Where(reg => profilesSku.Contains(reg.Perfil.Id))
                .Where(reg => reg.Funcionalidade.UnidadeFuncional.Modulo.Sistema.Codigo == systemCode)
                .GroupBy(reg => reg.Funcionalidade.UnidadeFuncional.Codigo,
                    reg => reg.Funcionalidade.Comando,
                    (key, g) => new
                    {
                        Codigo = key.ToUpper(),
                        Comandos = g.ToList()
                    }
                )
                .ToDictionary(reg => reg.Codigo, reg => reg.Comandos);
        }

        public Dictionary<string, List<string>> Permissoes(string systemCode, long profileSku)
        {
            return Queryable()
                .Where(reg => reg.Perfil.Id == profileSku)
                .Where(reg => reg.Funcionalidade.UnidadeFuncional.Modulo.Sistema.Codigo == systemCode)
                .GroupBy(reg => reg.Funcionalidade.UnidadeFuncional.Codigo,
                    reg => reg.Funcionalidade.Comando,
                    (key, g) => new
                    {
                        Codigo = key.ToUpper(),
                        Comandos = g.ToList()
                    }
                )
                .ToDictionary(reg => reg.Codigo, reg => reg.Comandos);
        }

    }
}