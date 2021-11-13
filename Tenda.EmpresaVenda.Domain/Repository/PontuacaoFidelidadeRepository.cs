using Europa.Data;
using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class PontuacaoFidelidadeRepository : NHibernateRepository<PontuacaoFidelidade>
    {
        public PontuacaoFidelidade NormalAtivoRegional(string regional)
        {
            return Queryable()
                .Where(x => x.Regional.Equals(regional))
                .Where(x => x.TipoPontuacaoFidelidade == TipoPontuacaoFidelidade.Normal)
                .Where(x => x.Situacao == SituacaoPontuacaoFidelidade.Ativo)
                .SingleOrDefault();
        }

        public List<PontuacaoFidelidade> CampanhasVencidas()
        {
            return Queryable()
                    .Where(x => x.TipoPontuacaoFidelidade == TipoPontuacaoFidelidade.Campanha)
                    .Where(x => x.Situacao == SituacaoPontuacaoFidelidade.Ativo)
                    .Where(x => x.TerminoVigencia.Value.Date < DateTime.Now.Date)
                    .ToList();
        }

        public List<PontuacaoFidelidade> CampanhasAguardando()
        {
            return Queryable()
                    .Where(x => x.TipoPontuacaoFidelidade == TipoPontuacaoFidelidade.Campanha)
                    .Where(x => x.Situacao == SituacaoPontuacaoFidelidade.AguardandoLiberacao)
                    .Where(x => x.InicioVigencia.Value.Date <= DateTime.Now.Date)
                    .ToList();
        }

        public override void Save(PontuacaoFidelidade entity)
        {
            if (entity.Id.IsEmpty())
            {
                _session.SaveOrUpdate(entity);
            }

            if (entity.TipoPontuacaoFidelidade == TipoPontuacaoFidelidade.Campanha)
            {
                var sequence = entity.Id.ToString();
                entity.Codigo = "PFC" + sequence.PadLeft(8, '0');
            }
            else
            {
                var sequence = entity.Id.ToString();
                entity.Codigo = "PF" + sequence.PadLeft(8, '0');
            }

            _session.SaveOrUpdate(entity);

        }
    }
}
