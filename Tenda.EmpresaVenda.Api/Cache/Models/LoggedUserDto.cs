using System;
using Tenda.Domain.Core.Data;

namespace Tenda.EmpresaVenda.Api.Cache.Models
{
    public class LoggedUserDto
    {
        public DateTime CreatedAt { get; set; }
        public DateTime LastRequest { get; set; }
        public RequestStateApi RequestState { get; set; }

        public RequestStateApi RequestStateCopy()
        {
            var copy = new RequestStateApi();
            copy.Acesso = RequestState.Acesso;
            copy.UsuarioPortal = RequestState.UsuarioPortal;
            copy.Perfis = RequestState.Perfis;
            return copy;
        }
    }
}