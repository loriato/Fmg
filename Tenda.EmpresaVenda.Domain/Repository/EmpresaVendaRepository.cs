using Europa.Data;
using Europa.Extensions;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class EmpresaVendaRepository : NHibernateRepository<Tenda.Domain.EmpresaVenda.Models.EmpresaVenda>
    {
        public RegraComissaoEvsRepository _regraComissaoEvsRepository { get; set; }
        public IQueryable<Tenda.Domain.EmpresaVenda.Models.EmpresaVenda> Listar()
        {
            return Queryable();
        }

        public bool CheckIfExistsCNPJ(Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda)
        {
            return Queryable().Where(emve => emve.CNPJ == empresaVenda.CNPJ).Where(emve => emve.Id != empresaVenda.Id).Any();
        }

        public bool CheckIfExistsCNPJ(string CNPJ)
        {
           return Queryable().Where(emve => emve.CNPJ == CNPJ).Any();
        }
        public bool CheckIfExistsCNPJAtivaSuspensa(Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda)
        {
            return Queryable().Where(emve => emve.CNPJ == empresaVenda.CNPJ.OnlyNumber()).Where(emve => emve.Id != empresaVenda.Id
                                                                                   && (emve.Situacao == Tenda.Domain.Security.Enums.Situacao.Ativo
                                                                                   || emve.Situacao == Tenda.Domain.Security.Enums.Situacao.Suspenso))
                                                                                   .Any();
        }
        public bool CheckIfExistsCNPJCancelada(Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda)
        {
            return Queryable().Where(emve => emve.CNPJ == empresaVenda.CNPJ.OnlyNumber()).Where(emve => emve.Id != empresaVenda.Id
                                                                             && emve.Situacao == Tenda.Domain.Security.Enums.Situacao.Cancelado)
                                                                             .Any();
        }
        public bool CheckIfExistsCNPJPreCadastro(Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda)
        {
            return Queryable().Where(emve => emve.CNPJ == empresaVenda.CNPJ.OnlyNumber()).Where(emve => emve.Id != empresaVenda.Id
                                                                            && emve.Situacao == Tenda.Domain.Security.Enums.Situacao.PreCadastro)
                                                                            .Any();
        }
        public int AtivarEmLote(long usuarioAlteracao, List<long> registros)
        {
            return AlterarSituacaoEmLote(usuarioAlteracao, Situacao.Ativo, registros);
        }

        public int SuspenderEmLote(long usuarioAlteracao, List<long> registros)
        {
            return AlterarSituacaoEmLote(usuarioAlteracao, Situacao.Suspenso, registros);
        }

        public int CancelarEmLote(long usuarioAlteracao, List<long> registros)
        {
            return AlterarSituacaoEmLote(usuarioAlteracao, Situacao.Cancelado, registros);
        }

        public int AlterarSituacaoEmLote(long usuarioAlteracao, Situacao situacao, List<long> registros)
        {
            StringBuilder updateQuery = new StringBuilder();
            updateQuery.Append(" UPDATE EmpresaVenda emve ");
            updateQuery.Append(" SET emve.AtualizadoPor = :atualizadoPor ");
            updateQuery.Append(" , emve.AtualizadoEm = :atualizadoEm ");
            updateQuery.Append(" , emve.Situacao = :situacao ");
            updateQuery.Append(" WHERE emve.Situacao != :situacaoCancelado ");
            updateQuery.Append(" AND emve.Situacao != :situacao ");
            updateQuery.Append(" AND emve.Id in (:registros) ");

            IQuery query = Session.CreateQuery(updateQuery.ToString());
            query.SetParameter("atualizadoPor", usuarioAlteracao);
            query.SetParameter("atualizadoEm", DateTime.Now);
            query.SetParameter("situacao", situacao);
            query.SetParameter("situacaoCancelado", Situacao.Cancelado);
            query.SetParameterList("registros", registros);

            var updates = query.ExecuteUpdate();

            return updates;
        }

        public List<Tenda.Domain.EmpresaVenda.Models.EmpresaVenda> EmpresasDaRegional(string regional)
        {
            return Queryable()
                .Where(reg => reg.Estado.ToUpper() == regional.ToUpper())
                .ToList();
        }

        public List<string> RegionaisDasEmpresasVendas()
        {
            return Queryable().GroupBy(x => x.Estado).OrderBy(x => x.Key).Select(x => x.Key).ToList();
        }

        public Tenda.Domain.EmpresaVenda.Models.EmpresaVenda FindByIdLoja(long idLoja)
        {
            return Queryable().Where(x => x.Loja.Id == idLoja).SingleOrDefault();
        }

        public IQueryable<Tenda.Domain.EmpresaVenda.Models.EmpresaVenda> FindByIdLoja(List<long> idLoja)
        {
            return Queryable().Where(x => idLoja.Contains(x.Loja.Id));
        }
        public Tenda.Domain.EmpresaVenda.Models.EmpresaVenda FindByCNPJ(string CNPJ)
        {
            return Queryable().Where(emve => emve.CNPJ == CNPJ).FirstOrDefault();
        }
        public IQueryable<Tenda.Domain.EmpresaVenda.Models.EmpresaVenda> ComRegraComissaoEvs(List<long> idLoja)
        {
            var evs = FindByIdLoja(idLoja).Select(x => x.Id).ToList();
            return _regraComissaoEvsRepository.TemRegraComissao(evs);
        }

        public Tenda.Domain.EmpresaVenda.Models.EmpresaVenda FindByIdSapLoja(string idSap)
        {
            return Queryable().Where(x => x.Loja.SapId.ToUpper().Equals(idSap.ToUpper())).SingleOrDefault();
        }

        public List<Tenda.Domain.EmpresaVenda.Models.EmpresaVenda> FindByIdsEmpresaVenda(List<long> idsEmpresaVenda)
        {
            return Queryable().Where(x => idsEmpresaVenda.Contains(x.Id)).ToList();
        }

        public List<Tenda.Domain.EmpresaVenda.Models.EmpresaVenda> EmpresasTreeEv(string estado, string empresaVenda)
        {
            var query = Queryable().Where(x => x.Situacao == Tenda.Domain.Security.Enums.Situacao.Ativo);

            if (estado.HasValue())
            {
                query = query.Where(reg => reg.Estado.ToUpper() == estado.ToUpper());
            }
            if (empresaVenda.HasValue())
            {
                query = query.Where(reg => reg.NomeFantasia.ToLower().Contains(empresaVenda.ToLower()));
            }


            return query.ToList();

        }

        public Tenda.Domain.EmpresaVenda.Models.EmpresaVenda BuscarPorNomeFantasia(string NomeFantasia)
        {
            return Queryable().Where(x => x.NomeFantasia.ToLower() == NomeFantasia.ToLower()).SingleOrDefault();
        }

        public long BuscarPorCodigoFornecedor(string codigoFornecedor)
        {
            return Queryable().Where(x => x.CodigoFornecedor.ToUpper() == codigoFornecedor.ToUpper())
                                .Where(x => x.Situacao == Tenda.Domain.Security.Enums.Situacao.Ativo)
                                .Select(x => x.Id)
                              .FirstOrDefault();
        }

        public bool CheckIfCorretorIsDiretor(long idCorretor,long idEmpresaVenda)
        {
            return Queryable()
                .Where(x => x.Id == idEmpresaVenda)
                .Where(x => x.Corretor.Id == idCorretor)
                .Where(x => x.Corretor.Funcao == TipoFuncao.Diretor)
                .Any();
        }

        public bool ExisteLoja(string nome, long id)
        {
            var query = Queryable();

            var loja = query.Where(x => x.NomeFantasia.ToUpper().Trim().Equals(nome.ToUpper().Trim()))
                .Where(x => x.TipoEmpresaVenda == TipoEmpresaVenda.Loja)
                .FirstOrDefault(x => x.Id != id);

            return !loja.IsNull();
        }

        public DataSourceResponse<Tenda.Domain.EmpresaVenda.Models.EmpresaVenda> ListarPorTipoEmpresaVenda(DataSourceRequest request, TipoEmpresaVenda tipoEmpresaVenda)
        {
            var query = Queryable().Where(x => x.TipoEmpresaVenda == tipoEmpresaVenda);

            var nome = request.filter?.FirstOrDefault(reg => reg.column.ToLower() == "nomefantasia")?.value;
            if (!nome.IsEmpty())
            {
                query = query.Where(x => x.NomeFantasia.ToLower().Contains(nome.ToLower()));
            }

            return query.ToDataRequest(request);
        }

        public List<Tenda.Domain.EmpresaVenda.Models.EmpresaVenda> ListarEVSAtivas()
        {
            var query = Queryable();

            query = query.Where(x => x.Situacao == Tenda.Domain.Security.Enums.Situacao.Ativo)
                        .Where(x => x.TipoEmpresaVenda == TipoEmpresaVenda.EmpresaVenda);

            return query.ToList();
        }

        public string BuscarIdSapLojaPorIdEmpresaVenda(long idEmpresaVenda)
        {
            return Queryable()
                        .Where(x => x.Id == idEmpresaVenda)
                        .Select(x => x.Loja.SapId)
                        .SingleOrDefault();
        }
    }
}
