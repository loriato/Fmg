using Europa.Data;
using Europa.Extensions;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class EnderecoBreveLancamentoRepository : NHibernateRepository<EnderecoBreveLancamento>
    {
        public RegionalEmpresaRepository _regionalEmpresaRepository { get; set; }
        public EnderecoBreveLancamento FindByBreveLancamento(long idBreveLancamento)
        {
            return Queryable().Where(x => x.BreveLancamento.Id == idBreveLancamento).FirstOrDefault();
        }

        public Dictionary<long, EnderecoBreveLancamento> EnderecosDeBrevesLancamentos(List<long> idsBrevesLancamentos, Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda)
        {
            var query = Queryable();

            if (empresaVenda.HasValue())
            {
                var regionaisEmpresa = _regionalEmpresaRepository.ListarRegionaisPorEmpresa(empresaVenda.Id).Select(s => s.Regional).ToList();
                query = query.Where(w => (empresaVenda.ConsiderarUF == Tenda.Domain.EmpresaVenda.Enums.TipoSimNao.Sim && regionaisEmpresa.Contains(w.BreveLancamento.Regional) && w.Estado == empresaVenda.Estado) ||
                                         (empresaVenda.ConsiderarUF == Tenda.Domain.EmpresaVenda.Enums.TipoSimNao.Nao && regionaisEmpresa.Contains(w.BreveLancamento.Regional)));
            }

            return query
                .Where(reg => idsBrevesLancamentos.Contains(reg.BreveLancamento.Id))
                .ToDictionary(reg => reg.BreveLancamento.Id, reg => reg);
        }
        public IQueryable<EnderecoBreveLancamento> EnderecosDeBrevesLancamentosQueryable(List<long> idsBrevesLancamentos, Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda)
        {
            var query = Queryable();

            if (empresaVenda.HasValue())
            {
                var regionais = _regionalEmpresaRepository.ListarRegionaisPorEmpresa(empresaVenda.Id).Select(s => s.Regional).ToList();
                query = query.Where(reg => regionais.Contains(reg.BreveLancamento.Regional));
            }

            return query
                .Where(reg => idsBrevesLancamentos.Contains(reg.BreveLancamento.Id));
        }

        public IQueryable<EnderecoBreveLancamento> BuscarPorEstado(string estado)
        {
            return Queryable().Where(reg => reg.Estado == estado);
        }


        /// <summary>
        /// Atenção! Retorna apenas ID e Nome na Projeção
        /// </summary>
        /// <param name="request"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        public DataSourceResponse<BreveLancamento> DisponiveisParaRegional(DataSourceRequest request, Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda)
        {
            var query = Queryable();
            if (empresaVenda.HasValue())
            {
                var regionaisEmpresa = _regionalEmpresaRepository.ListarRegionaisPorEmpresa(empresaVenda.Id).Select(s=>s.Regional).ToList();
                query = query.Where(w => (empresaVenda.ConsiderarUF == Tenda.Domain.EmpresaVenda.Enums.TipoSimNao.Sim && regionaisEmpresa.Contains(w.BreveLancamento.Regional) && w.Estado == empresaVenda.Estado) ||
                                         (empresaVenda.ConsiderarUF == Tenda.Domain.EmpresaVenda.Enums.TipoSimNao.Nao && regionaisEmpresa.Contains(w.BreveLancamento.Regional)));
            }

            // Autocomplete 
            if (request.filter.FirstOrDefault() != null)
            {
                var filtro = request.filter.FirstOrDefault().column.ToString().ToLower();
                var queryTerm = request.filter.FirstOrDefault().value.ToString().ToLower();
                if (filtro.Equals("nome"))
                {
                    query = query.Where(x => x.BreveLancamento.Nome.ToLower().Contains(queryTerm));
                }
            }

            return query
                .OrderBy(reg => reg.BreveLancamento.Nome)
                .Select(reg => new BreveLancamento()
                {
                    Id = reg.BreveLancamento.Id,
                    Nome = reg.BreveLancamento.Nome
                })
            .ToDataRequest(request);
        }

        /// <summary>
        /// Atenção! Retorna apenas ID e Nome na Projeção
        /// </summary>
        /// <param name="request"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        public DataSourceResponse<BreveLancamento> DisponiveisNoCatalogoParaRegional(DataSourceRequest request, Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda)
        {
            var query = Queryable()
                .Where(reg => reg.BreveLancamento.DisponivelCatalogo);

            if (empresaVenda.HasValue())
            {
                var regionaisEmpresa = _regionalEmpresaRepository.ListarRegionaisPorEmpresa(empresaVenda.Id).Select(s => s.Regional).ToList();
                query = query.Where(w => (empresaVenda.ConsiderarUF == Tenda.Domain.EmpresaVenda.Enums.TipoSimNao.Sim && regionaisEmpresa.Contains(w.BreveLancamento.Regional) && w.Estado == empresaVenda.Estado) ||
                                         (empresaVenda.ConsiderarUF == Tenda.Domain.EmpresaVenda.Enums.TipoSimNao.Nao && regionaisEmpresa.Contains(w.BreveLancamento.Regional)));
            }

            // Autocomplete 
            if (request.filter.FirstOrDefault() != null)
            {
                var filtro = request.filter.FirstOrDefault().column.ToString().ToLower();
                var queryTerm = request.filter.FirstOrDefault().value.ToString().ToLower();
                if (filtro.Equals("nome"))
                {
                    query = query.Where(x => x.BreveLancamento.Nome.ToLower().Contains(queryTerm));
                }
            }

            return query
                .OrderBy(reg => reg.BreveLancamento.Nome)
                .Select(reg => new BreveLancamento()
                {
                    Id = reg.BreveLancamento.Id,
                    Nome = reg.BreveLancamento.Nome
                })
            .ToDataRequest(request);
        }

        /// <summary>
        /// Atenção! Retorna apenas ID e Nome na Projeção
        /// </summary>
        /// <param name="request"></param>
        /// <param name="estado"></param>
        /// <returns></returns>
        public DataSourceResponse<BreveLancamento> DisponiveisParaRegionalSemEmpreendimento(DataSourceRequest request, Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda)
        {
            var query = Queryable()
                .Where(reg => reg.BreveLancamento.Empreendimento == null);

            if (empresaVenda.HasValue())
            {
                var regionaisEmpresa = _regionalEmpresaRepository.ListarRegionaisPorEmpresa(empresaVenda.Id).Select(s => s.Regional).ToList();
                query = query.Where(w => (empresaVenda.ConsiderarUF == Tenda.Domain.EmpresaVenda.Enums.TipoSimNao.Sim && regionaisEmpresa.Contains(w.BreveLancamento.Regional) && w.Estado == empresaVenda.Estado) ||
                                         (empresaVenda.ConsiderarUF == Tenda.Domain.EmpresaVenda.Enums.TipoSimNao.Nao && regionaisEmpresa.Contains(w.BreveLancamento.Regional)));
            }

            // Autocomplete 
            if (request.filter.FirstOrDefault() != null)
            {
                var filtro = request.filter.FirstOrDefault().column.ToString().ToLower();
                var queryTerm = request.filter.FirstOrDefault().value.ToString().ToLower();
                if (filtro.Equals("nome"))
                {
                    query = query.Where(x => x.BreveLancamento.Nome.ToLower().Contains(queryTerm));
                }
            }

            return query.Select(reg => new BreveLancamento()
            {
                Id = reg.BreveLancamento.Id,
                Nome = reg.BreveLancamento.Nome
            })
            .ToDataRequest(request);
        }
    }
}