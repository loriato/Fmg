using Europa.Data;
using Europa.Extensions;
using NHibernate;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Domain.Repository.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewCorretorRepository : NHibernateRepository<ViewCorretor>
    {
        public RegionalEmpresaRepository _regionalEmpresaRepository { get; set; }
        public EmpresaVendaRepository _empresaVendaRepository { get; set; }
        public CorretorRepository _corretorRepository { get; set; }
        public ViewCorretorRepository(ISession session) : base(session)
        {
        }

        public IQueryable<ViewCorretor> Listar(FiltroCorretorDTO filtro)
        {

            var query = Queryable();

            if (filtro.IdEmpresaVenda.HasValue())
            {
                query = query.Where(reg => reg.IdEmpresaVenda == filtro.IdEmpresaVenda);
            }

            if (filtro.Nome.HasValue())
            {
                query = query.Where(reg =>
                    reg.Nome.ToLower().Contains(filtro.Nome.ToLower()) ||
                    reg.Apelido.ToLower().Contains(filtro.Nome.ToLower()));
            }

            if (filtro.CpfCnpjCreci.HasValue())
            {
                var cpfCnpjCreci = filtro.CpfCnpjCreci.ToUpper().OnlyNumber();
                query = query.Where(reg =>
                    reg.Cpf == cpfCnpjCreci ||
                    reg.Cnpj == cpfCnpjCreci ||
                    reg.Creci.ToUpper() == cpfCnpjCreci);
            }

            if (filtro.Situacao.HasValue())
            {
                query = query.Where(reg => filtro.Situacao.Contains(reg.Situacao) || reg.Situacao == null);
            }

            if (filtro.EmpresaVenda.HasValue())
            {
                query = query.Where(reg => reg.NomeEmpresaVenda.ToLower().Contains(filtro.EmpresaVenda.ToLower()));
            }

            if (filtro.Funcao.HasValue())
            {
                query = query.Where(reg => filtro.Funcao.Contains(reg.Funcao));
            }
            if (filtro.Perfil.HasValue())
            {
                query = query.Where(reg => reg.Perfis.ToLower().Contains(filtro.Perfil.ToLower()));
            }
            if (filtro.IdRegional.HasValue() && !filtro.IdRegional.Contains(0))
            {
                var list = _regionalEmpresaRepository.Queryable().Where(w => filtro.IdRegional.Contains(w.Regional.Id)).Select(s => s.EmpresaVenda.Id);
                query = query.Where(reg => list.Contains(reg.IdEmpresaVenda));
            }

            if (filtro.UF.HasValue() && !filtro.UF.Contains("") && !filtro.UF.Contains(null))
            {
                query = query.Where(reg => filtro.UF.Contains(reg.UF));
            }
            return query;


        }

        public DataSourceResponse<ViewCorretor> ListarDaEmpresaVenda(DataSourceRequest request, long idEmpresaVenda, long idCorretor)
        {
            var empresaVenda = _empresaVendaRepository.FindById(idEmpresaVenda);
            var corretor = _corretorRepository.FindById(idCorretor);
            var query = Queryable()
                .Where(reg => reg.IdEmpresaVenda == idEmpresaVenda);

            if (request.filter.FirstOrDefault() != null)
            {
                var filtro = request.filter.FirstOrDefault().column.ToString().ToLower();
                var queryTerm = request.filter.FirstOrDefault().value.ToString().ToLower();
                if (filtro.Equals("nome"))
                {
                    query = query.Where(x => x.Nome.ToLower().Contains(queryTerm));
                }
                if(corretor.Funcao == TipoFuncao.Corretor)
                {
                    if (empresaVenda.CorretorVisualizarClientes)
                    {
                        query = query.Where(x => x.Id == idCorretor);
                    }
                }
            }

            return query.ToDataRequest(request);
        }

        public DataSourceResponse<ViewCorretor> ListarGerenteDaEmpresaVenda(DataSourceRequest request, long idEmpresaVenda)
        {
            var query = Queryable()
                .Where(reg => reg.Funcao == TipoFuncao.Gerente)
                .Where(reg => reg.IdEmpresaVenda == idEmpresaVenda);

            if (request.filter.FirstOrDefault() != null)
            {
                var filtro = request.filter.FirstOrDefault().column.ToString().ToLower();
                var queryTerm = request.filter.FirstOrDefault().value.ToString().ToLower();
                if (filtro.Equals("nome"))
                {
                    query = query.Where(x => x.Nome.ToLower().Contains(queryTerm));
                }
            }

            return query.ToDataRequest(request);
        }

        public DataSourceResponse<ViewCorretor> Listar(DataSourceRequest request)
        {
            var query = Queryable();

            if (request.filter.FirstOrDefault() != null)
            {
                var filtro = request.filter.FirstOrDefault().column.ToString().ToLower();
                var queryTerm = request.filter.FirstOrDefault().value.ToString().ToLower();
                var idEmpresaVenda = request.filter[1].column.ToString().ToLower();
                var queryEV = request.filter[1].value.ToString().ToLower();
                if (filtro.Equals("nome"))
                {
                    query = query.Where(x => x.Nome.ToLower().Contains(queryTerm));
                }
                if (idEmpresaVenda.Equals("idempresavenda"))
                {
                    query = query.Where(x => x.IdEmpresaVenda.ToString() == queryEV);
                }
            }

            return query.ToDataRequest(request);
        }

        public DataSourceResponse<ViewCorretor> ListarProprietariosEmpresaVenda(DataSourceRequest request, long idEmpresaVenda)
        {
            var empresaVenda = _empresaVendaRepository.FindById(idEmpresaVenda);
            var query = Queryable().Where(reg => reg.IdEmpresaVenda == idEmpresaVenda);

            var nome = request.filter?.FirstOrDefault(reg => reg.column.ToLower() == "nome")?.value;
            if (!nome.IsEmpty())
            {
                query = query.Where(x => x.Nome.ToLower().Contains(nome.ToLower()));
            }

            return query.ToDataRequest(request);
        }
    }
}
