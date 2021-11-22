using Europa.Fmg.Domain.Repository;
using Tenda.Domain.Core.Services;
using Tenda.Domain.Fmg.Models;

namespace Europa.Fmg.Domain.Services
{
    public class ViaturaService : BaseService
    {
        private ViaturaRepository _viaturaRepository { get; set; }
        public Viatura Salvar(Viatura viatura)
        {
            viatura.Placa = viatura.Placa.ToUpper();
            _viaturaRepository.Save(viatura);
            return viatura;
        }
    }
}
