using Europa.Extensions;
using System;
using Tenda.Domain.Core.Services;
using Tenda.EmpresaVenda.Domain.Integration.Zeus.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class RequisicaoCompraService : BaseService
    {
        //private const string CategoriaClassificacaoContabilCodigo = "ACCTASSCAT";
        private const string codigoMoedaCodigo = "CURRENCY";
        private const string tipoDocumentoCodigo = "DOC_TYPE";
        private const string codigoEntradaMercadoriasCodigo = "GR_IND";
        private const string codigoEntradaFaturasCodigo = "IR_IND";
        //private const string categoriaItemDocumentoCompraCodigo = "ITEM_CAT";
        private const string grupoMercadoriasCodigo = "MAT_GRP";
        private const string numeroMaterialCodigo = "MATERIAL";
        private const string numeroItemCodigo = "PREQ_ITEM";
        private const string grupoCompradoresCodigo = "PUR_GROUP";
        private const string quantidade = "QUANTITY";
        private const string unidadeMedidaCodigo = "UNIT";
        private const string nomeRequisitanteCodigo = "PREQ_NAME";
        private const string textoBreveCodigo = "SHORT_TEXT";
        //
        private const string categoriaClassificacaoContabil = "ACCTASSCAT";
        private const string tipoData = "DEL_DATCAT";
        private const string organizacaoCompras = "PURCH_ORG";
        private const string areaContabilidadeCustos = "CO_AREA";
        private const string numeroSecClasseContabil = "SERIAL_NO";
        private const string numeroContaRazao = "G_L_ACCT";

        public ParametroRequisicaoCompraRepository _parametroRequisicaoCompraRepository { get; set; }

        public void GetParametros(ItemRequisicaoDTO dto)
        {
            //dto.itemRequisicao.categoriaClassificacaoContabil = _parametroRequisicaoCompraRepository.BucarValorParametro(CategoriaClassificacaoContabilCodigo);
            dto.CodigoMoeda = _parametroRequisicaoCompraRepository.BucarValorParametro(codigoMoedaCodigo);
            dto.TipoDocumento = _parametroRequisicaoCompraRepository.BucarValorParametro(tipoDocumentoCodigo);
            dto.CodigoEntradaMercadorias = _parametroRequisicaoCompraRepository.BucarValorParametro(codigoEntradaMercadoriasCodigo);
            dto.CodigoEntradaFaturas = _parametroRequisicaoCompraRepository.BucarValorParametro(codigoEntradaFaturasCodigo);
            //dto.CategoriaItemDocumentoCompra = _parametroRequisicaoCompraRepository.BucarValorParametro(categoriaItemDocumentoCompraCodigo);
            dto.GrupoMercadorias = _parametroRequisicaoCompraRepository.BucarValorParametro(grupoMercadoriasCodigo);
            dto.NumeroMaterial = _parametroRequisicaoCompraRepository.BucarValorParametro(numeroMaterialCodigo);

            var numItem = _parametroRequisicaoCompraRepository.BucarValorParametro(numeroItemCodigo);
            dto.NumeroItem = numItem.IsEmpty() ? 0 : Convert.ToInt32(numItem);

            var quantItem = _parametroRequisicaoCompraRepository.BucarValorParametro(quantidade);
            dto.Quantidade = quantItem.IsEmpty() ? 0 : Convert.ToInt32(quantItem);

            var numSeqSegmentoClasseContabil = _parametroRequisicaoCompraRepository.BucarValorParametro(numeroSecClasseContabil);
            dto.NumeroSeqSegmentoClasseContabil = numSeqSegmentoClasseContabil.IsEmpty() ? 0 : Convert.ToInt32(numSeqSegmentoClasseContabil);

            dto.GrupoCompradores = _parametroRequisicaoCompraRepository.BucarValorParametro(grupoCompradoresCodigo);
            dto.UnidadeMedida = _parametroRequisicaoCompraRepository.BucarValorParametro(unidadeMedidaCodigo);
            dto.NomeRequisitante = _parametroRequisicaoCompraRepository.BucarValorParametro(nomeRequisitanteCodigo);
            dto.TextoBreve = _parametroRequisicaoCompraRepository.BucarValorParametro(textoBreveCodigo);
            dto.CategoriaClassificacaoContabil = _parametroRequisicaoCompraRepository.BucarValorParametro(categoriaClassificacaoContabil);
            dto.TipoData = _parametroRequisicaoCompraRepository.BucarValorParametro(tipoData);
            dto.OrganizacaoDeCompras = _parametroRequisicaoCompraRepository.BucarValorParametro(organizacaoCompras);
            dto.AreaContabilidadeCustos = _parametroRequisicaoCompraRepository.BucarValorParametro(areaContabilidadeCustos);
            dto.NumeroContaRazao = _parametroRequisicaoCompraRepository.BucarValorParametro(numeroContaRazao);
        }

    }
}
