using Europa.Data;
using Europa.Extensions;
using NHibernate;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.Security.Repository
{
    public class UsuarioPerfilSistemaRepository : NHibernateRepository<UsuarioPerfilSistema>
    {
        public UsuarioPerfilSistemaRepository(ISession session) : base(session) { }

        public List<Perfil> ListProfilesByUserSkuAndSystemCode(long userSku, string systemCode)
        {
            return Queryable().Where(x => x.Usuario.Id == userSku && x.Sistema.Codigo == systemCode)
                .Select(x => x.Perfil).ToList();
        }

        public List<Perfil> ListarPerfisUsuarioSistema(long idUsuario, string codigoSistema)
        {
            return Queryable()
                .Where(x => x.Usuario.Id == idUsuario && x.Sistema.Codigo == codigoSistema)
                .Select(x => x.Perfil)
                .ToList();
        }
        public List<UsuarioPerfilSistema> Listar(long idUsuario, long idSistema)
        {
            return Queryable()
                .Where(x => x.Usuario.Id == idUsuario)
                .Where(x => x.Sistema.Id == idSistema)
                .ToList();
        }

        public List<Perfil> PerfisAtivosUsuarioSistema(long idUsuario, long idSistema)
        {
            return Queryable()
                .Where(x => x.Usuario.Id == idUsuario)
                .Where(x => x.Sistema.Id == idSistema)
                .Where(x => x.Perfil.Situacao == Situacao.Ativo)
                .Select(x => x.Perfil)
                .ToList();
        }
        public DataSourceResponse<Usuario> ListarUsuariosPerfilSistema(DataSourceRequest request, long? idPerfil, string codigoSistema,List<long> viabilizadores)
        {
            var query = Queryable()
                .Where(x => x.Sistema.Codigo == codigoSistema);

            if (viabilizadores.HasValue())
            {
                query = query.Where(x => viabilizadores.Contains(x.Usuario.Id));
            }
            if (idPerfil != null)
            {
                query = query.Where(x => x.Perfil.Id == idPerfil);
            }

            if (request.filter.FirstOrDefault() != null)
            {
                string filtro = request.filter.FirstOrDefault().column.ToString().ToLower();
                string queryTerm = request.filter.FirstOrDefault().value.ToString().ToLower();
                if (filtro.Equals("nome"))
                {
                    query = query.Where(x => x.Usuario.Nome.ToLower().Contains(queryTerm));
                }

                var exceto = request.filter.Where(x => x.column.Equals("exceto_viabilizador")).FirstOrDefault();

                if (exceto.HasValue() && exceto.value.HasValue())
                {
                    query = query.Where(x => x.Usuario.Id != long.Parse(exceto.value));
                }
            }

            return query.Select(x => x.Usuario).ToDataRequest(request);
        }
        public Usuario ListarPerfils(long idUsuario, long perfil)
        {
            return Queryable()
                .Where(x => x.Perfil.Id == perfil)
                .Where(x => x.Usuario.Id == idUsuario)
                .Select(x => x.Usuario)
                .FirstOrDefault();
        }

        public bool UsuarioPertenceAPerfil(long idUsuario, long idPerfil, long idSistema)
        {
            return Queryable()
                .Where(x => x.Sistema.Id == idSistema)
                .Where(x => x.Perfil.Id == idPerfil)
                .Where(x => x.Usuario.Id == idUsuario)
                .Any();
        }
    }
}