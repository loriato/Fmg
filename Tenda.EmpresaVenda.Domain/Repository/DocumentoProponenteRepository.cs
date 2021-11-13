using Europa.Data;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class DocumentoProponenteRepository : NHibernateRepository<DocumentoProponente>
    {
        public bool DocumentoDoTipoParaProponente(long idPreProposta, long idProponente, long idTipoDocumento)
        {
            return Queryable()
                .Where(reg => reg.PreProposta.Id == idPreProposta)
                .Where(reg => reg.Proponente.Id == idProponente)
                .Where(reg => reg.TipoDocumento.Id == idTipoDocumento)
                .Any();
        }

        public DocumentoProponente BuscarDoTipoParaProponente(long idPreProposta, long idProponente, long idTipoDocumento)
        {
            return Queryable()
                .Where(reg => reg.PreProposta.Id == idPreProposta)
                .Where(reg => reg.Proponente.Id == idProponente)
                .Where(reg => reg.TipoDocumento.Id == idTipoDocumento)
                .SingleOrDefault();
        }

        public List<DocumentoProponente> BuscarDocumentosReprovadosPorIdPreProposta(long idPreProposta)
        {
            return Queryable()
                .Where(x => x.PreProposta.Id == idPreProposta)
                .Where(x => x.Situacao == SituacaoAprovacaoDocumento.Pendente)
                .ToList();
        }

        public Dictionary<PreProposta, List<DocumentoProponente>> BuscarDocumentosPrePropostasFinalizadas()
        {
            var dataAtual = DateTime.Now.Date;

            var query = Queryable()
                .Where(x => x.PreProposta.SituacaoProposta == SituacaoProposta.AnaliseSimplificadaAprovada||
                x.PreProposta.SituacaoProposta==SituacaoProposta.EmAnaliseCompleta
                || x.PreProposta.SituacaoProposta == SituacaoProposta.Retorno)
                .Where(x => x.DataExpiracao.HasValue)
                .Where(x => x.DataExpiracao.Value.Date < dataAtual)
                .Fetch(x => x.PreProposta)
                .ToList();

            return query
                .GroupBy(x => x.PreProposta)
                .ToDictionary(x => x.Key, x => x.ToList());
        }

        public List<DocumentoProponente> BuscarDocumentosComArquivosAnexadosPorIdPreProposta(long idPreProposta)
        {
            return Queryable()
                .Where(x => x.PreProposta.Id == idPreProposta)
                .Where(x => x.Arquivo != null)
                .ToList();
        }

        public List<DocumentoProponente> BuscarDocumentosPorIdPrePropostaIdProponente(long idPreProposta, long idProponente)
        {
            return Queryable()
                .Where(x => x.PreProposta.Id == idPreProposta)
                .Where(x => x.Proponente.Id == idProponente)
                .ToList();
        }

        public List<DocumentoProponente> BuscarDocumentosPorPreProposta(long idPreProposta)
        {
            return Queryable()
                .Where(x => x.PreProposta.Id == idPreProposta)
                .ToList();
        }

        /// <summary>
        /// Verifica se existe algum outro proponente com determinado tipo de documento anexado
        /// </summary>
        /// <param name="idPreProposta"></param>
        /// <param name="idProponente"></param>
        /// <param name="idTipoDocumento"></param>
        /// <returns></returns>
        public bool PossuiDocumentoAnexadoEmOutroProponente(long idPreProposta, long idProponente, long idTipoDocumento)
        {
            return Queryable()
                .Where(reg => reg.PreProposta.Id == idPreProposta)
                .Where(reg => reg.TipoDocumento.Id == idTipoDocumento)
                .Where(reg => reg.Proponente.Id != idProponente)
                .Where(reg => reg.Situacao == SituacaoAprovacaoDocumento.Anexado)
                .Any();
        }

        /// <summary>
        /// Busca todos os documentos do mesmo tipo 
        /// </summary>
        /// <param name="idPreProposta"></param>
        /// <param name="idProponente"></param>
        /// <param name="idTipoDocumento"></param>
        /// <returns></returns>
        public List<DocumentoProponente> DocumentosAssociadosDeOutroProponente(long idPreProposta, long idProponente, long idTipoDocumento, long idDocumentoProponente, string motivo)
        {
            return Queryable()
                .Where(reg => reg.PreProposta.Id == idPreProposta)
                .Where(reg => reg.TipoDocumento.Id == idTipoDocumento)
                .Where(reg => reg.Proponente.Id != idProponente)
                .Where(reg => reg.Id != idDocumentoProponente)
                .Where(reg => reg.Motivo.Equals(motivo))
                .ToList();
        }
        public List<DocumentoProponente> BuscarDocumentosPorIdProponente(long idProponente)
        {
            return Queryable()
                .Where(x => x.Proponente.Id == idProponente)
                .ToList();
        }
        public bool HasDocsUnaproved(PreProposta preproposta)
        {
            return Queryable()
                .Where(x => x.PreProposta.Id == preproposta.Id)
                .Where(x => SituacaoAprovacaoDocumento.Aprovado != x.Situacao)
                .Any();
        }
    }
}