using System;
using System.Collections.Generic;

namespace Tenda.EmpresaVenda.Api.Cache.Models
{
    public class PermissionDto
    {
        public long IdPerfil { get; set; }
        public DateTime LastUpdate { get; set; }
        public Dictionary<string, List<string>> Permission { get; set; }
        public string CodigoSistema { get; set; }

        public PermissionDto()
        {

        }

        public PermissionDto(long profileSku, Dictionary<string, List<string>> permission)
        {
            IdPerfil = profileSku;
            LastUpdate = DateTime.Now;
            Permission = permission;
        }
    }
}