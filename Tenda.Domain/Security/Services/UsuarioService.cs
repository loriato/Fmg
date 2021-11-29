using Europa.Extensions;
using System;
using System.Text;
using Tenda.Domain.Core.Services;
using Tenda.Domain.Security.Repository;

namespace Tenda.Domain.Security.Services
{
    public class UsuarioService : BaseService
    {
        public UsuarioRepository _usuarioRepository { get; set; }

        public object UserListDataTable(DataSourceRequest request)
        {
            var result = _usuarioRepository.Queryable();
            return result.ToDataRequest(request);
        }

        public int InvalidarTokenUsuarios(int quantidadeHorasCorte, long idUsuario)
        {
            var dataParametro = DateTime.Now.AddHours(-quantidadeHorasCorte);
            var queryString = new StringBuilder();
            queryString.Append(" UPDATE Usuario usu SET usu.TokenAtivacao = NULL, usu.DataAtivacaoToken = NULL, ");
            queryString.Append(" usu.AtualizadoPor = :idUsuarioAtualizacao, usu.AtualizadoEm = :dataAtualizacao ");
            queryString.Append(" WHERE usu.DataAtivacaoToken < :dataCorte ");

            var query = _session.CreateQuery(queryString.ToString());
            query.SetParameter("dataCorte", dataParametro);
            query.SetParameter("idUsuarioAtualizacao", idUsuario);
            query.SetParameter("dataAtualizacao", DateTime.Now);

            return query.ExecuteUpdate();
        }
    }
}
