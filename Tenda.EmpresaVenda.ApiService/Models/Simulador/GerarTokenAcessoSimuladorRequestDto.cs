using Europa.Cryptography;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.EmpresaVenda.ApiService.Models.Simulador
{
    public class GerarTokenAcessoSimuladorRequestDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string CorretorEmpresaVenda { get; set; }
        public string Autorizacao { get; set; }
        public string IdSapLoja { get; set; }

        public string Login { get; set; }
        public string Senha { get; set; }
        public bool SouCorretor { get; set; }
        public bool ShowError { get; set; }
        public string Token { get; set; }
        public long IdPreProposta { get; set; }
        public long IdCliente { get; set; }
        public TipoOrigemSimulacao OrigemSimulacao { get; set; }

        public GerarTokenAcessoSimuladorRequestDto() { }

        public GerarTokenAcessoSimuladorRequestDto(string login,string senha)
        {
            Username = SslAes256.EncryptString(login);
            Password = SslAes256.EncryptString(senha);
        }
    }
}
