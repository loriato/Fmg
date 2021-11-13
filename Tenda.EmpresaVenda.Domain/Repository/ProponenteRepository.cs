using System.Collections.Generic;
using System.Linq;
using Europa.Data;
using NHibernate.Linq;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ProponenteRepository : NHibernateRepository<Proponente>
    {
        public List<Proponente> ProponentesDaPreProposta(long idPreProposta)
        {
            return Queryable()
                .Where(reg => reg.PreProposta.Id == idPreProposta)
                .ToList();
        }

        public int SomatorioParticipacaoProponentes(long idPreProposta)
        {
            var possuiProponente = Queryable()
                .Where(reg => reg.PreProposta.Id == idPreProposta)
                .Any();

            if (!possuiProponente) { return 0; }

            return Queryable()
                .Where(reg => reg.PreProposta.Id == idPreProposta)
                .Sum(reg => reg.Participacao);
        }

        public Proponente BuscarSegundoProponenteDaPreProposta(long idPreProposta)
        {
            return Queryable()
                // Filtrando pela proposta
                .Where(reg => reg.PreProposta.Id == idPreProposta)
                // Eliminando o cliente principal)
                .Where(reg => reg.PreProposta.Cliente.Id != reg.Cliente.Id)
                .OrderByDescending(x => x.CriadoEm)
                .FirstOrDefault();
        }

        public Proponente BuscarPorCpfClienteIndique(string cpf)
        {
            return Queryable()
                .Where(x => x.Cliente.CpfCnpj == cpf && x.PreProposta.SituacaoProposta != SituacaoProposta.Cancelada)
                .Fetch(x => x.Cliente)
                .Fetch(x => x.PreProposta)
                .OrderByDescending(x => x.PreProposta.DataElaboracao)
                .FirstOrDefault();
        }

        public IEnumerable<Proponente> BuscarPrePropostasClientesPorCpfIndique(string[] cpfs)
        {
            return Queryable()
                .Where(x => cpfs.Contains(x.Cliente.CpfCnpj))
                .Fetch(x => x.Cliente)
                .Fetch(x => x.PreProposta)
                .OrderByDescending(x => x.PreProposta.DataElaboracao)
                .ToList();
        }
    }
}