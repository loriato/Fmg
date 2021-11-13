using Europa.Data;
using Europa.Extensions;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class EmpreendimentoRepository : NHibernateRepository<Empreendimento>
    {
        public EmpreendimentoRepository(ISession session) : base(session)
        {
        }

        public EmpreendimentoRepository()
        {
        }

        /// <summary>
        /// Usando como referência a lista de identificadores enviados,
        /// atualiza o flag Empreendimento.DisponivelParaVenda de todos contidos para  Verdadeiro e
        /// atualiza o flag Empreendimento.DisponivelParaVenda de todos NÃO contidos para FALSO
        /// </summary>
        /// <param name="idEmpreendimentos"></param>
        public int DisponibilizarParaVenda(List<long> idSuatEmpreendimentos, long idUsuario)
        {
            // No caso de vir 0, devo colocar todos como não disponíveis
            // Como o ID de BD começa em 1, estou passando número negativo para a função
            if (idSuatEmpreendimentos.IsEmpty())
            {
                idSuatEmpreendimentos = new List<long> { -1 };
            }

            var hql = new StringBuilder();
            hql.Append("UPDATE Empreendimento empr ");
            hql.Append(" SET empr.DisponivelParaVenda = :disponibilidade, ");
            hql.Append(" empr.AtualizadoEm = :atualizadoEm, ");
            hql.Append(" empr.AtualizadoPor = :usuario ");
            hql.Append(" WHERE empr.IdSuat #CONDITION# (:registros) ");


            string hqlNotIn = hql.Replace("#CONDITION#", "NOT IN").ToString();
            var query = Session.CreateQuery(hqlNotIn);
            query.SetParameter("disponibilidade", false);
            query.SetParameter("atualizadoEm", DateTime.Now);
            query.SetParameter("usuario", idUsuario);
            query.SetParameterList("registros", idSuatEmpreendimentos);

            string hqlIn = hql.Replace("#CONDITION#", "IN").ToString();

            query = Session.CreateQuery(hqlIn);
            query.SetParameter("disponibilidade", true);
            query.SetParameter("atualizadoEm", DateTime.Now);
            query.SetParameter("usuario", idUsuario);
            query.SetParameterList("registros", idSuatEmpreendimentos);
            return query.ExecuteUpdate();
        }

        public List<Empreendimento> DisponiveisParaCatalogo()
        {
            return Queryable()
                .Where(reg => reg.DisponivelCatalogo)
                .ToList();
        }

        public Empreendimento BuscarPorDivisao(string divisao)
        {
            return Queryable().SingleOrDefault(x => x.Divisao.ToUpper().Equals(divisao.ToUpper()));
        }

        public Empreendimento FindByIdSapEmpreendimento(string idSap)
        {
            return Queryable().Where(x => x.Divisao.ToUpper().Equals(idSap.ToUpper())).SingleOrDefault();
        }
    }
}