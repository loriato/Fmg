using Europa.Data;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class DocumentoAvalistaRepository: NHibernateRepository<DocumentoAvalista>
    {
        public DocumentoAvalista BuscarDoTipoParaAvalista(long idPreProposta, long idAvalista, long idTipoDocumento)
        {
            return Queryable()
                .Where(reg => reg.PreProposta.Id == idPreProposta)
                .Where(reg => reg.Avalista.Id == idAvalista)
                .Where(reg => reg.TipoDocumento.Id == idTipoDocumento)
                .SingleOrDefault();
        }

        public List<DocumentoAvalista> BuscarDocumentosComArquivosAnexadosPorIdPrePropostaEIdAvalista(long idPreProposta,long idAvalista)
        {
            return Queryable()
                .Where(x => x.PreProposta.Id == idPreProposta)
                .Where(x=>x.Avalista.Id == idAvalista)
                .Where(x => x.Arquivo != null)
                .ToList();
        }

        public DocumentoAvalista BuscarPOrIdPrePropostaEIdAvalista(long idPreProposta, long idAvalista)
        {
            return Queryable()
                .Where(reg => reg.PreProposta.Id == idPreProposta)
                .Where(reg => reg.Avalista.Id == idAvalista)
                .SingleOrDefault();
        }
        public List<DocumentoAvalista> BuscarDocumentosParaAnalise(long idPreProposta, long idAvalista)
        {
            return Queryable()
                .Where(x => x.PreProposta.Id == idPreProposta)
                .Where(x => x.Avalista.Id == idAvalista)
                .ToList();
        }
        public List<DocumentoAvalista> BuscarDocumentosAvalista(long idPreProposta, long idAvalista)
        {
            return Queryable()
                .Where(x=>x.PreProposta.Id == idPreProposta)
                .Where(x=>x.Avalista.Id == idAvalista)
                .ToList();
        }
        public List<DocumentoAvalista> BuscarDocumentosNaoAnalisados(long idPreProposta, long idAvalista)
        {
            return Queryable()
                .Where(x => x.PreProposta.Id == idPreProposta)
                .Where(x => x.Avalista.Id == idAvalista)
                .Where(x => x.Situacao != SituacaoAprovacaoDocumentoAvalista.Pendente)
                .Where(x => x.Situacao != SituacaoAprovacaoDocumentoAvalista.PreAprovado)
                .ToList();
        }
    }
}
