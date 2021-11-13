namespace Tenda.EmpresaVenda.Domain.Services.Models.Indique
{
    public class CarteiraDto
    {
        public virtual LojaDto Loja { get; set; }
        public virtual UsuarioDto AgenteVenda { get; set; }
        public virtual UsuarioDto Supervisor { get; set; }
        public virtual UsuarioDto Coordenador { get; set; }
    }
}
