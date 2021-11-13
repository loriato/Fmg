using Europa.Data.Model;
using System;
using Tenda.Domain.Security.Enums;

namespace Tenda.Domain.Security.Views
{
    public class ViewAcesso : BaseEntity
    {
        public virtual DateTime InicioSessao { get; set; }
        public virtual DateTime? FimSessao { get; set; }
        public virtual FormaEncerramento FormaEncerramento { get; set; }
        public virtual string IpOrigem { get; set; }
        public virtual long IdUsuario { get; set; }
        public virtual string NomeUsuario { get; set; }
        public virtual long IdSistema { get; set; }
        public virtual string NomeSistema { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}