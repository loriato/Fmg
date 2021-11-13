using Europa.Data;
using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class RegraComissaoRepository : NHibernateRepository<RegraComissao>
    {
        public RegraComissao BuscarRegraVigente(string regional)
        {
            return Queryable()
                .Where(reg => reg.Situacao == SituacaoRegraComissao.Ativo)
                .Where(reg => reg.Regional == regional)
                .SingleOrDefault();
        }

        public List<RegraComissao> CampanhasAguardandoLiberacao()
        {
            var lista = Queryable()
                .Where(reg => reg.Situacao == SituacaoRegraComissao.AguardandoLiberacao)
                .Where(reg => reg.Tipo == TipoRegraComissao.Campanha)
                .Where(reg => reg.InicioVigencia.Value.Date <= DateTime.Now.Date);

            return lista.ToList();
        }

        public List<RegraComissao> CampanhasAguardandoInativacao()
        {
            var lista = Queryable()
                .Where(reg => reg.Tipo == TipoRegraComissao.Campanha)
                .Where(reg => reg.Situacao == SituacaoRegraComissao.Ativo || reg.Situacao == SituacaoRegraComissao.AguardandoLiberacao)
                .Where(reg => reg.TerminoVigencia.Value < DateTime.Now);
            
            return lista.ToList();
        }

        public override void Save(RegraComissao entity)
        {
            if (entity.Id.IsEmpty())
            {
                _session.SaveOrUpdate(entity);
            }

            if (entity.Tipo == TipoRegraComissao.Campanha)
            {
                //Criando código da Regra de Comissao
                var sequence = entity.Id.ToString();
                entity.Codigo = "C" + sequence.PadLeft(6, '0');
            }
            else
            {
                //Criando código da Regra de Comissao
                var sequence = entity.Id.ToString();
                entity.Codigo = "R" + sequence.PadLeft(6, '0');
            }

            _session.SaveOrUpdate(entity);

        }
    }
}
