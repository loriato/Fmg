using Europa.Data.Model;
using System;
using Tenda.Domain.Security.Enums;

namespace Tenda.Domain.Security.Models
{
    public class Acesso : BaseEntity
    {
        public virtual DateTime InicioSessao { get; set; }
        public virtual DateTime? FimSessao { get; set; }
        public virtual string IpOrigem { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual Sistema Sistema { get; set; }
        public virtual FormaEncerramento FormaEncerramento { get; set; }
        public virtual string Autorizacao { get; set; }
        public virtual string Servidor { get; set; }
        public virtual string Navegador { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
