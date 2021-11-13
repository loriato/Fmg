using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.Domain.Integration.Suat.Models;

namespace Tenda.EmpresaVenda.Domain.Integration.Suat
{
    public static class ConsultaEstoqueService
    {
        private static List<EstoqueEmpreendimentoDTO> _estoqueEmpreendimento;
        private static List<EstoqueUnidadeDTO> _estoqueUnidade;
        private static List<TorreDTO> _estoqueTorre;

        public static DataSourceResponse<EstoqueEmpreendimentoDTO> EstoqueEmpreendimento(DataSourceRequest request, string divisao)
        {
            try
            {
                SuatService service = new SuatService();
                _estoqueEmpreendimento = service.EstoqueEmpreendimento(divisao);
                return _estoqueEmpreendimento.AsQueryable().ToDataRequest(request);
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
                return new List<EstoqueEmpreendimentoDTO>().AsQueryable().ToDataRequest(request);
            }
        }

        public static DataSourceResponse<TorreDTO> EstoqueTorre(DataSourceRequest request, string divisao)
        {
            SuatService service = new SuatService();
            _estoqueTorre = service.EstoqueTorre(divisao);
            return _estoqueTorre.AsQueryable().ToDataRequest(request);
        }

        public static DataSourceResponse<EstoqueUnidadeDTO> EstoqueUnidade(DataSourceRequest request, string divisao, string caracteristicas, DateTime previsaoEntrega, long idTorre)
        {
            SuatService service = new SuatService();
            _estoqueUnidade = service.EstoqueUnidade(divisao, caracteristicas, previsaoEntrega, idTorre);
            return _estoqueUnidade.AsQueryable().ToDataRequest(request);
        }
    }
}
