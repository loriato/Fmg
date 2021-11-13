namespace Tenda.Domain.Security.Services.Models
{
    public class LoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Server { get; set; }
        public string UserAgent { get; set; }
        public string ClienteIpAddress { get; set; }
        public string CodigoSistema { get; set; }
        public bool LoginViaActiveDirectory { get; set; }
        public bool SincronizarGruposActiveDirectory { get; set; }

    }
}
