using Europa.Data;
using Europa.Extensions;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Services.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class PontoVendaRepository : NHibernateRepository<PontoVenda>
    {
        private StandVendaEmpresaVendaRepository _standVendaEmpresaVendaRepository { get; set; }
        public IQueryable<PontoVenda> Listar(PontoVendaDto filtro)
        {
            var query = Queryable();
            if (filtro.Nome.HasValue())
            {
                query = query.Where(x => x.Nome.ToLower().Contains(filtro.Nome.ToLower()));
            }
            if (filtro.Situacao.HasValue())
            {
                query = query.Where(x => filtro.Situacao.Contains((int)x.Situacao));
            }
            if (filtro.IniciativaTenda.HasValue())
            {
                query = query.Where(x => x.IniciativaTenda == filtro.IniciativaTenda);
            }
            if (filtro.idEmpresaVenda.HasValue())
            {
                query = query.Where(x => x.EmpresaVenda.Id == filtro.idEmpresaVenda);
            }
            if (filtro.idGerente.HasValue())
            {
                query = query.Where(x => x.Gerente.Id == filtro.idGerente);
            }
            if (filtro.IdPontosVenda.HasValue())
            {
                query = query.Where(x => filtro.IdPontosVenda.Contains(x.Id));
            }
            return query;
        }

        public DataSourceResponse<PontoVenda> ListarDaEmpresaVenda(DataSourceRequest request, long idEmpresaVenda)
        {
            var query = Queryable()
                .Where(reg => reg.EmpresaVenda.Id == idEmpresaVenda);

            var filtroStand = request.filter.Where(x => x.column.ToLower().Equals("situacaostand")).SingleOrDefault();
            if (filtroStand.value.HasValue())
            {
                var queryAux = _standVendaEmpresaVendaRepository.Queryable()
                    .Where(x => x.EmpresaVenda.Id == idEmpresaVenda)
                    .Where(x => x.PontoVenda.Situacao != Situacao.Ativo)
                    .Select(x => x.PontoVenda);

                query = query.Where(x => !queryAux.Contains(x));
            }


            if (request.filter.FirstOrDefault() != null)
            {
                var filtro = request.filter.FirstOrDefault().column.ToString().ToLower();
                var queryTerm = request.filter.FirstOrDefault().value.ToString().ToLower();
                if (filtro.Equals("nome"))
                {
                    query = query.Where(x => x.Nome.ToLower().Contains(queryTerm));
                }
            }

            query = query.Select(reg => new PontoVenda()
            {
                Id = reg.Id,
                Nome = reg.Nome
            });

            return query.ToDataRequest(request);
        }

        public bool CheckIfExistsNomeWithEmpresaVenda(PontoVenda model)
        {
            return Queryable().Where(pntv => pntv.Id != model.Id)
                              .Where(pntv => pntv.Nome.ToLower().Equals(model.Nome.ToLower()))
                              .Where(pntv => pntv.EmpresaVenda.Id == model.EmpresaVenda.Id)
                              .Any();
        }

        public int AlterarSituacaoEmLote(Situacao novaSituacao, long[] ids, long usuarioAlteracao)
        {
            StringBuilder updateQuery = new StringBuilder();
            updateQuery.Append(" UPDATE PontoVenda pntv ");
            updateQuery.Append(" SET pntv.AtualizadoPor = :atualizadoPor ");
            updateQuery.Append(" , pntv.AtualizadoEm = :atualizadoEm ");
            updateQuery.Append(" , pntv.Situacao = :situacao ");
            updateQuery.Append(" WHERE pntv.Situacao != :situacaoCancelado ");
            updateQuery.Append(" AND pntv.Id in (:registros) ");

            IQuery query = Session.CreateQuery(updateQuery.ToString());
            query.SetParameter("atualizadoPor", usuarioAlteracao);
            query.SetParameter("atualizadoEm", DateTime.Now);
            query.SetParameter("situacao", novaSituacao);
            query.SetParameter("situacaoCancelado", Situacao.Cancelado);
            query.SetParameterList("registros", ids);

            return query.ExecuteUpdate();
        }

        public IQueryable<PontoVenda> BuscarPorEmpresaVenda(long idEmpresaVenda)
        {
            return Queryable().Where(pont => pont.EmpresaVenda.Id == idEmpresaVenda).OrderBy(pont => pont.Nome);
        }


        public IQueryable<PontoVenda> BuscarAtivosPorEmpresaVenda(long idEmpresaVenda)
        {
            return Queryable()
                   .Where(pont => pont.Situacao == Situacao.Ativo)
                .Where(pont => pont.EmpresaVenda.Id == idEmpresaVenda)
                .OrderBy(pont => pont.Nome);
        }

        public List<long> BuscarViabilizadoresPontoVenda(List<long> idEmpresaVenda)
        {
            return Queryable()
                    .Where(pont => pont.Situacao == Situacao.Ativo)
                    .Where(pont => idEmpresaVenda.Contains(pont.EmpresaVenda.Id))
                    .Where(pont => pont.Viabilizador != null)
                    .Select(pont => pont.Viabilizador.Id)
                    .ToList();
        }

        public List<long> BuscarIdEmpresaVendaPorViabilizador(long idViabilizador)
        {
            return Queryable()
                .Where(x => x.Viabilizador.Id == idViabilizador)
                .Select(x => x.EmpresaVenda.Id)
                .ToList();
        }
        public PontoVenda BuscarPontoVendaPorViabilizador(long idViabilizador)
        {
            return Queryable()
                .Where(x => x.Viabilizador.Id == idViabilizador)
                .ToList().FirstOrDefault();
        }
        public List<PontoVenda> BuscarPontosVendaPorViabilizador(long idViabilizador)
        {
            return Queryable()
                .Where(x => x.Viabilizador.Id == idViabilizador)
                .ToList().ToList();
        }

        public List<PontoVenda> BuscarPontosVendaPorViabilizador(List<long> idViabilizador)
        {
            return Queryable()
                .Where(x => idViabilizador.Contains(x.Viabilizador.Id))
                .ToList().ToList();
        }

        public bool HasPontoVenda(long idEmpresaVenda)
        {
            return Queryable()
                .Where(x => x.EmpresaVenda.Id ==idEmpresaVenda)
                .Any();
        }
    }
    
}
