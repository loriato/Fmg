using Europa.Extensions;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.Core.Models;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Repository;

namespace Tenda.Domain.Core.Services
{
    public class UsuarioPortalService : BaseService
    {
        public ViewUsuarioPerfilRepository _viewUsuarioPerfilRepository { get; set; }
        public UsuarioPortalRepository _usuarioPortalRepository { get; set; }
        public UsuarioPerfilSistemaRepository _usuarioPerfilSistemaRepository { get; set; }
        public PerfilRepository _perfilRepository { get; set; }
        public SistemaRepository _sistemaRepository { get; set; }

        public void AlterarSitu(IList<long> ids, SituacaoUsuario situacao)
        {
            var usersToBeChanged = _usuarioPortalRepository.Queryable().Where(x => ids.Contains(x.Id));
            foreach (var usuarioPortal in usersToBeChanged)
            {
                usuarioPortal.Situacao = situacao;
                _usuarioPortalRepository.Save(usuarioPortal);
            }
            _usuarioPortalRepository.Flush();
        }

        public UsuarioPortal Salvar(UsuarioPortal usuario)
        {
            // remover mascaras
            usuario.Telefone = usuario.Telefone.OnlyNumber();
            usuario.Cpf = usuario.Cpf.OnlyNumber();
            usuario.Login = usuario.Cpf;
            usuario.Situacao = SituacaoUsuario.Ativo;

            _usuarioPortalRepository.Save(usuario);
            return usuario;
        }
        public DataSourceResponse<UsuarioPortal> ListarUsuariosAutocomplete(DataSourceRequest request)
        {
            var query = _usuarioPortalRepository.Queryable();
            if (request.filter.FirstOrDefault() != null)
            {
                var filtro = request.filter.FirstOrDefault()?.column.ToLower();
                var queryTerm = request.filter.FirstOrDefault()?.value.ToLower();
                if (filtro != null && filtro.Equals("nome"))
                {
                    query = query.Where(x => x.Nome.ToLower().Contains(queryTerm));
                }

            }

            return query.ToDataRequest(request);
        }
    }
}
