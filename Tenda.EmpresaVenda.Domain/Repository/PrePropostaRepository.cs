using Europa.Data;
using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class PrePropostaRepository : NHibernateRepository<PreProposta>
    {
        public string BuscarCodigoPreProposta()
        {
            return Session.CreateSQLQuery("select nextval('seq_codigo_pre_propostas');").UniqueResult().ToString();
        }

        public List<PreProposta> BuscarPrePropostasParaConsolidar(DateTime dataCorte)
        {
            return Queryable()
                .Where(x => x.AtualizadoEm >= dataCorte)
                .OrderBy(x => x.AtualizadoEm)
                .ThenBy(x => x.Id)
                .ToList();
        }

        public int QuantidadePrePropostaAnteriorPorCliente(long idCliente, DateTime dataProposta)
        {
            return Queryable()
                .Where(x => x.Cliente.Id == idCliente)
                .Where(x => x.DataElaboracao < dataProposta)
                .Count();
        }

        public PreProposta BuscarPorCodigo(string codigo)
        {
            return Queryable()
                .FirstOrDefault(reg => reg.Codigo == codigo);
        }

        public IQueryable<PreProposta> BuscarPrePropostasFinalizadas()
        {
            return Queryable()
                .Where(x => x.SituacaoProposta == SituacaoProposta.AnaliseSimplificadaAprovada ||
                            x.SituacaoProposta == SituacaoProposta.Retorno);
        }

        public List<PreProposta> BuscarFinalizadasPorBreveLancamento(BreveLancamento breveLancamento)
        {
            return Queryable()
                .Where(x => x.SituacaoProposta == SituacaoProposta.AguardandoIntegracao)
                .Where(x => x.BreveLancamento.Id == breveLancamento.Id).ToList();
        }

        public IQueryable<PreProposta> BuscarPrePropostasIntegradas()
        {
            return Queryable().Where(x => x.IdSuat > 0);
        }

        public PreProposta BuscarPorIdSuat(long idSuat)
        {
            return Queryable().Where(x => x.IdSuat == idSuat).SingleOrDefault();
        }

        public List<Avalista> ListarDaPreProposta(long idPrePrposta, long? idAvalista)
        {
            var query = Queryable();

            query = query.Where(x => x.Id == idPrePrposta);

            if (idAvalista.HasValue())
            {
                query = query.Where(x => x.Avalista.Id == idAvalista);
            }

            return query.Select(x => x.Avalista).ToList();
        }

        public List<PreProposta> BuscarPrePropostasAtivas(List<long> idsPreProposta)
        {
            return Queryable().Where(x => idsPreProposta.Contains(x.Id))
                               .Where(x => x.SituacaoProposta != SituacaoProposta.Integrada)
                               .Where(x => x.SituacaoProposta != SituacaoProposta.Cancelada).ToList();
        }

        public List<long> ListarIdsPrePropostaDeViabilizadorComEvs(long? idViabilizador, List<long> idEmpresaVenda, long? idPontoVenda)
        {
            var query = Queryable();

            if (idViabilizador.HasValue())
            {
                query = query.Where(x => x.Viabilizador.Id == idViabilizador);
            }

            if (idEmpresaVenda.HasValue())
            {
                query = query.Where(x => idEmpresaVenda.Contains(x.EmpresaVenda.Id));
            }

            if (idPontoVenda.HasValue())
            {
                query = query.Where(x => x.PontoVenda.Id == idPontoVenda );
            }

            return query.Select(x => x.Id).ToList(); ;
        }

        public IQueryable<PreProposta> ListarAutoComplete(DataSourceRequest request, long? idEmpresaVenda)
        {

            var results = Queryable();
            if (!idEmpresaVenda.IsEmpty())
            {
                results = results.Where(reg => reg.EmpresaVenda.Id == idEmpresaVenda);
            }

            if (request.HasValue() && request.filter.FirstOrDefault() != null)
            {
                foreach (var filtro in request.filter)
                {
                    if (filtro.column.ToLower().Equals("codigo"))
                    {
                        results = results.Where(x => x.Codigo.ToString().ToLower().Contains(filtro.value.ToLower()));
                    }
                    if (filtro.column.ToLower().Equals("idempresavenda"))
                    {
                        results = results.Where(x => x.EmpresaVenda.Id == Convert.ToInt64(filtro.value));
                    }

                }
            }
            return results;
        }

        public bool PontoVendaPossuiPreProposta(List<long> idsPontoVenda)
        {
            return Queryable()
                        .Where(x => idsPontoVenda.Contains(x.PontoVenda.Id))
                        .Any();
        }


    }
}