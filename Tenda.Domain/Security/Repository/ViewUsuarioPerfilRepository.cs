using Europa.Data;
using Europa.Extensions;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Views;

namespace Tenda.Domain.Security.Repository
{
    public class ViewUsuarioPerfilRepository : NHibernateStatelessRepository<ViewUsuarioPerfil>
    {
        public PerfilRepository perfilRepository { get; set; }

        public DataSourceResponse<ViewUsuarioPerfil> Listar(DataSourceRequest request, string nameOrEmail, string[] tipos, string name, string login, string email, long? perfil, string codigoSistema)
        {
            var query = Queryable();

            if (request.filter.HasValue() && request.filter.FirstOrDefault().HasValue())
            {
                var filtroNome = request.filter.FirstOrDefault(reg => reg.column.ToLower() == "nomeusuario");
                if (filtroNome.HasValue())
                {
                    query = query.Where(x => x.NomeUsuario.ToLower().Contains(filtroNome.value.ToString().ToLower()));
                }
            }

            if (!nameOrEmail.IsEmpty())
            {
                query = query.Where(x => x.NomeUsuario.ToLower().StartsWith(nameOrEmail.ToLower()) || x.Login.ToLower().StartsWith(nameOrEmail.ToLower()) || x.Email.ToLower().StartsWith(nameOrEmail.ToLower()));
            }

            if (!tipos.IsNull())
            {
                if (tipos.Length != 0)
                {
                    if (tipos.Contains("Ativo"))
                    {
                        query = query.Where(x => x.Situacao == SituacaoUsuario.Ativo);
                    }
                    if (tipos.Contains("Suspenso"))
                    {
                        query = query.Where(x => x.Situacao == SituacaoUsuario.Suspenso);
                    }
                }
            }

            if (!name.IsEmpty())
            {
                query = query.Where(x => x.NomeUsuario.StartsWith(name));
            }

            if (!login.IsEmpty())
            {
                query = query.Where(x => x.Login.StartsWith(login));
            }

            if (!email.IsEmpty())
            {
                query = query.Where(x => x.Email.StartsWith(email));
            }

            if (perfil.HasValue)
            {
                var entidadePerfil = perfilRepository.FindById(perfil.Value);

                query = query.Where(x => x.Perfis.Contains(entidadePerfil.Nome));
            }

            if (!codigoSistema.IsEmpty())
            {
                query = query.Where(x => x.CodigoSistema.ToLower().Equals(codigoSistema.ToLower()));
            }

            return query.ToDataRequest(request);
        }

        public DataSourceResponse<ViewUsuarioPerfil> ListarTodos(DataSourceRequest request)
        {
            var query = Queryable();

            return query.ToDataRequest(request);
        }

        public List<ViewUsuarioPerfil> ListarUsuariosAtivos(DataSourceRequest request, string nameOrEmail, string[] tipos, string name, string login, string email, long? perfil, string codigoSistema)
        {
            var query = Listar(request, nameOrEmail,  tipos,  name,  login,  email,  perfil,  codigoSistema);
            return query.records.Where(x => x.Situacao == SituacaoUsuario.Ativo).ToList();
        }
    }
}
