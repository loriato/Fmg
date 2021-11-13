using System.Linq;
using System.Web;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class ContratoCorretagemService : BaseService
    {
        private ContratoCorretagemRepository _contratoCorretagemRepository { get; set; }
        private ArquivoService _arquivoService { get; set; }

        public ContratoCorretagem SalvarContratoCorretagem(HttpPostedFileBase arquivo)
        {
            var arquivoSalvo = _arquivoService.CreateFile(arquivo, arquivo.FileName);
            var contrato = new ContratoCorretagem
            {
                Arquivo = arquivoSalvo,
                NomeDoubleCheck = arquivoSalvo.Nome,
                HashDoubleCheck = arquivoSalvo.Hash,
                IdArquivoDoubleCheck = arquivoSalvo.Id,
                ContentTypeDoubleCheck = arquivoSalvo.ContentType
            };

            _contratoCorretagemRepository.Save(contrato);

            return contrato;
        }

        public ContratoCorretagem ContratoVigente()
        {
            return _contratoCorretagemRepository.Queryable()
                .OrderByDescending(reg => reg.CriadoEm)
                .FirstOrDefault();
        }

    }
}
