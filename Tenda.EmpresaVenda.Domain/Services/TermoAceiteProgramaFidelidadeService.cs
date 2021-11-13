using System.Web;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class TermoAceiteProgramaFidelidadeService : BaseService
    {
        private TermoAceiteProgramaFidelidadeRepository _termoAceiteProgramaFidelidadeRepository { get; set; }
        private ArquivoService _arquivoService { get; set; }
        public TermoAceiteProgramaFidelidade SalvarTermoAceite(HttpPostedFileBase arquivo)
        {
            var arquivoSalvo = _arquivoService.CreateFile(arquivo, arquivo.FileName);
            var termoAceite = new TermoAceiteProgramaFidelidade
            {
                Arquivo = arquivoSalvo,
                NomeDoubleCheck = arquivoSalvo.Nome,
                HashDoubleCheck = arquivoSalvo.Hash,
                IdArquivoDoubleCheck = arquivoSalvo.Id,
                ContentTypeDoubleCheck = arquivoSalvo.ContentType
            };

            _termoAceiteProgramaFidelidadeRepository.Save(termoAceite);

            return termoAceite;
        }
    }
}
