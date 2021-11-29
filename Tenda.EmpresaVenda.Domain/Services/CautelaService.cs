using Europa.Fmg.Domain.Repository;
using Tenda.Domain.Core.Services;
using Tenda.Domain.Fmg.Models;
using Tenda.Domain.Security.Models;

namespace Europa.Fmg.Domain.Services
{
    public class CautelaService : BaseService
    {
        private CautelaRepository _cautelaRepository { get; set; }
        private PedidoCautelaRepository _pedidocautelaRepository { get; set; }
        public Cautela Salvar(Cautela model)
        {
            _cautelaRepository.Save(model);
            return model;
        }
        public void RealizarPedido(Cautela cautela, Usuario usuario, long quantidade)
        {
            cautela.Total = cautela.Total - quantidade;
            _cautelaRepository.Save(cautela);
            var pedidoCautela = new PedidoCautela()
            {
                Usuario = usuario,
                Cautela = cautela,
                Quantidade = quantidade,
                Pedido = System.DateTime.Now
            };
            _pedidocautelaRepository.Save(pedidoCautela);
        }
    }

}
