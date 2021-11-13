using Europa.Data.Model;
using System;
using Tenda.Domain.Core.Models;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class Notificacao : BaseEntity
    {
        public virtual UsuarioPortal Usuario { get; set; }
        public virtual EmpresaVenda EmpresaVenda { get; set; }
        public virtual string Titulo { get; set; }
        public virtual string Conteudo { get; set; }
        public virtual string Link { get; set; }
        public virtual DateTime? DataLeitura { get; set; }
        public virtual string NomeBotao { get; set; }
        public virtual TipoNotificacao TipoNotificacao{get;set;}
        public virtual DestinoNotificacao DestinoNotificacao { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
