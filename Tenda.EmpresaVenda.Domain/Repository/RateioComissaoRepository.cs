using Europa.Data;
using System;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class RateioComissaoRepository : NHibernateRepository<RateioComissao>
    {
        public IQueryable<RateioComissao> Listar()
        {
            return Queryable();
        }

        public bool CheckExistsRateioContratadaVigente(long idContratada)
        {
            return Queryable().Where(x => x.Contratada.Id == idContratada || x.Contratante.Id == idContratada)
                                .Where(x => x.Situacao == SituacaoRateioComissao.Ativo)
                                .Any();
        }
        public bool CheckExistsRateioContratanteVigente(long idContratante)
        {
            return Queryable().Where(x => x.Contratante.Id == idContratante || x.Contratada.Id == idContratante)
                                .Where(x => x.Situacao == SituacaoRateioComissao.Ativo)
                                .Any();
        }

        public RateioComissao BuscarRateioNaData(DateTime dataVenda, long idEmpresaVenda, long IdEmpreendimento)
        {
            return Queryable().Where(x => x.Contratada.Id == idEmpresaVenda || x.Contratante.Id == idEmpresaVenda)
                               .Where(x => x.Empreendimento.Id == IdEmpreendimento)
                               .Where(X => X.InicioVigencia <= dataVenda)
                               .Where(x => x.TerminoVigencia >= dataVenda)
                               .FirstOrDefault();
        }

        public RateioComissao BuscarRateioAtivo(DateTime dataVenda, long idEmpresaVenda, long IdEmpreendimento)
        {
            return Queryable().Where(x => x.Contratada.Id == idEmpresaVenda || x.Contratante.Id == idEmpresaVenda)
                               .Where(x => x.Empreendimento.Id == IdEmpreendimento)
                               .Where(X => X.InicioVigencia <= dataVenda)
                               .Where(x => x.TerminoVigencia == null)
                               .FirstOrDefault();
        }
        public RateioComissao BuscarRateioNaDataTodosEmpreendimento(DateTime dataVenda, long idEmpresaVenda)
        {
            return Queryable().Where(x => x.Contratada.Id == idEmpresaVenda || x.Contratante.Id == idEmpresaVenda)
                               .Where(x => x.Empreendimento == null)
                               .Where(X => X.InicioVigencia <= dataVenda)
                               .Where(x => x.TerminoVigencia >= dataVenda)
                               .FirstOrDefault();
        }
        public RateioComissao BuscarRateioAtivoTodosEmpreendimento(DateTime dataVenda, long idEmpresaVenda)
        {
            return Queryable().Where(x => x.Contratada.Id == idEmpresaVenda || x.Contratante.Id == idEmpresaVenda)
                               .Where(X => X.InicioVigencia <= dataVenda)
                               .Where(x => x.TerminoVigencia == null)
                               .Where(x => x.Empreendimento == null)
                               .FirstOrDefault();
        }
    }
}
