using Europa.Fmg.Domain.Repository;
using Tenda.Domain.Core.Services;
using Tenda.Domain.Fmg.Models;
using Tenda.Domain.Security.Models;

namespace Europa.Fmg.Domain.Services
{
    public class ConsumoService : BaseService
    {
        private ConsumoRepository _consumoRepository { get; set; }
        private PedidoConsumoRepository _pedidoConsumoRepository { get; set; }

        public Consumo Salvar(Consumo model)
        {
            _consumoRepository.Save(model);
            return model;
        }
        public void RealizarPedido(Consumo consumo, Usuario usuario, long quantidade)
        {
            consumo.Total = consumo.Total - quantidade;
            _consumoRepository.Save(consumo);
            var pedidoCautela = new PedidoConsumo()
            {
                Usuario = usuario,
                Consumo = consumo,
                Quantidade = quantidade,
                Pedido = System.DateTime.Now
            };
            _pedidoConsumoRepository.Save(pedidoCautela);
        }
    }
}
