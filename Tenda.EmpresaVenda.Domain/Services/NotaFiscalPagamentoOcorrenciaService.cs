using Europa.Extensions;
using System.Collections.Generic;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class NotaFiscalPagamentoOcorrenciaService : BaseService
    {
        public NotaFiscalPagamentoOcorrenciaRepository _notaFiscalPagamentoOcorrenciaRepository { get; set; }
        public OcorrenciasMidasRepository _ocorrenciasMidasRepository { get; set; }
    }
}
