using System;
using System.Collections.Generic;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.ConsultarNumeroPedidoZeus;
using Tenda.EmpresaVenda.Domain.GerarRequisicaoDeCompraZeus;
using Tenda.EmpresaVenda.Domain.Integration.Zeus.Models;

namespace Tenda.EmpresaVenda.Domain.Integration.Zeus
{
    public static class ZeusService
    {

        #region GerarRequisicao
        public static RequisicaoCompraResponseItemDTO[] GerarRC(ItemRequisicaoDTO itemRequisicao, ContabilizacaoRequisicaoDTO contabilizacaoRequisicao, DadosAuditoriaDTO dadosAuditoriaDTO)
        {
            var url = ProjectProperties.ZeusUrlGerarRequisicao;

            var service = new GerarRequisicaoCompra(url);

            var dadosAuditoria = new GerarRequisicaoDeCompraZeus.dadosAuditoria
            {
                dataTransacao = dadosAuditoriaDTO.DataTransacao,
                usuario = dadosAuditoriaDTO.Usuario,
                sistema = dadosAuditoriaDTO.Sistema
            };

            service.AuditoriaHeader = dadosAuditoria;

            List<requisicaoCompraRequestItem> compraRequestItems = new List<requisicaoCompraRequestItem>();

            var itemDTO = MontarRequisicaoCompraRequestItem(itemRequisicao, contabilizacaoRequisicao);

            compraRequestItems.Add(itemDTO);

            var result = service.CallGerarRequisicaoCompra(compraRequestItems.ToArray());

            List<RequisicaoCompraResponseItemDTO> requisicaoCompraResponseItemDTO = new List<RequisicaoCompraResponseItemDTO>();
            foreach (var item in result)
            {
                var requisicaoCompraDTO = new RequisicaoCompraResponseItemDTO();
                requisicaoCompraDTO.numero = item.numero;
                requisicaoCompraDTO.texto = item.texto;
                requisicaoCompraDTO.status = item.status;

                requisicaoCompraResponseItemDTO.Add(requisicaoCompraDTO);
            }
            return requisicaoCompraResponseItemDTO.ToArray();
        }

        private static requisicaoCompraRequestItem MontarRequisicaoCompraRequestItem(ItemRequisicaoDTO itemRequisicaoDTO, ContabilizacaoRequisicaoDTO contabilizacaoRequisicaoDTO)
        {
            var itemRequisicao = new itemRequisicao();
            itemRequisicao.numeroItem = itemRequisicaoDTO.NumeroItem;
            itemRequisicao.tipoDocumento = itemRequisicaoDTO.TipoDocumento;
            itemRequisicao.grupoCompradores = itemRequisicaoDTO.GrupoCompradores;
            itemRequisicao.nomeRequisitante = itemRequisicaoDTO.NomeRequisitante;
            itemRequisicao.dataSolicitacao = itemRequisicaoDTO.DataSolicitacao;
            itemRequisicao.textoBreve = itemRequisicaoDTO.TextoBreve;
            itemRequisicao.numeroMaterial = itemRequisicaoDTO.NumeroMaterial;
            itemRequisicao.centroDeCusto = itemRequisicaoDTO.CentroDeCusto;
            itemRequisicao.grupoMercadorias = itemRequisicaoDTO.GrupoMercadorias;
            itemRequisicao.quantidade = itemRequisicaoDTO.Quantidade;
            itemRequisicao.unidadeMedida = itemRequisicaoDTO.UnidadeMedida;
            itemRequisicao.tipoData = itemRequisicaoDTO.TipoData;
            itemRequisicao.dataRemessaItem = itemRequisicaoDTO.DataRemessaItem;
            itemRequisicao.dataLiberacao = itemRequisicaoDTO.DataLiberacao;
            itemRequisicao.preco = itemRequisicaoDTO.Preco;
            itemRequisicao.precoUnidade = itemRequisicaoDTO.PrecoUnidade;
            itemRequisicao.categoriaItemDocumentoCompra = itemRequisicaoDTO.CategoriaItemDocumentoCompra;
            itemRequisicao.categoriaClassificacaoContabil = itemRequisicaoDTO.CategoriaClassificacaoContabil;
            itemRequisicao.codigoEntradaMercadorias = itemRequisicaoDTO.CodigoEntradaMercadorias;
            itemRequisicao.codigoEntradaFaturas = itemRequisicaoDTO.CodigoEntradaFaturas;
            itemRequisicao.fornecedorPretendido = itemRequisicaoDTO.FornecedorPretendido;
            itemRequisicao.organizacaoDeCompras = itemRequisicaoDTO.OrganizacaoDeCompras;
            itemRequisicao.codigoMoeda = itemRequisicaoDTO.CodigoMoeda;

            var contabilizacaoRequisicao = new contabilizacaoRequisicao();
            contabilizacaoRequisicao.numeroContaRazao = itemRequisicaoDTO.NumeroContaRazao;
            contabilizacaoRequisicao.areaContabilidadeCustos = itemRequisicaoDTO.AreaContabilidadeCustos;
            contabilizacaoRequisicao.numeroItem = contabilizacaoRequisicaoDTO.NumeroItem;
            contabilizacaoRequisicao.numeroSeqSegmentoClasseContabil = itemRequisicaoDTO.NumeroSeqSegmentoClasseContabil;
            contabilizacaoRequisicao.quantidade = contabilizacaoRequisicaoDTO.Quantidade;
            contabilizacaoRequisicao.divisao = contabilizacaoRequisicaoDTO.Divisao;
            contabilizacaoRequisicao.centroCusto = contabilizacaoRequisicaoDTO.CentroCusto;
            contabilizacaoRequisicao.numeroOrdem = contabilizacaoRequisicaoDTO.NumeroOrdem;
            contabilizacaoRequisicao.centroDeLucro = contabilizacaoRequisicaoDTO.CentroDeLucro;



            var item = new requisicaoCompraRequestItem();
            item.designacaoContaRequisicao = contabilizacaoRequisicao;
            item.itemRequisicao = itemRequisicao;

            return item;
        }
        #endregion

        #region BuscarPedido
        public static List<PedidoDTO> BuscarPedido(List<string> requisicaoCompras)
        {
            var url = ProjectProperties.ZeusUrlBuscarNumeroPedido;

            var service = new ConsultarNumeroDePedido(url);
            service.AuditoriaHeader = new ConsultarNumeroPedidoZeus.dadosAuditoria()
            {
                dataTransacao = DateTime.Now,
                usuario = "Robo Atualizacao Sap Numero Pedido",
                sistema = "EmpresaVenda",
            };


            var result = service.ConsultarNumeroPedido(requisicaoCompras.ToArray());
            List<PedidoDTO> pedido = new List<PedidoDTO>();
            foreach (var item in result)
            {
                PedidoDTO dto = new PedidoDTO
                {
                    Mandante = item.Mandante,
                    CodigoLiberacaoDocumentoCompra = item.CodigoLiberacaoDocumentoCompra,
                    NumeroItemDocumentoCompra = item.NumeroItemDocumentoCompra,
                    NumeroDocumentoCompra = item.NumeroDocumentoCompra,
                    NumeroItemRequisicaoCompra = item.NumeroItemRequisicaoCompra,
                    NumeroRequisicaoCompra = item.NumeroRequisicaoCompra,
                    Data = item.Data,
                    Status = item.Status,
                    LinhaTexto = item.LinhaTexto
                };

                pedido.Add(dto);

            }
            return pedido;
        }
        #endregion
    }
}
