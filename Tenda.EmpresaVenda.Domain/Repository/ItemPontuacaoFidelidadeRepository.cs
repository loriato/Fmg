using Europa.Data;
using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ItemPontuacaoFidelidadeRepository : NHibernateRepository<ItemPontuacaoFidelidade>
    {
        public List<ItemPontuacaoFidelidade> ItensDePontuacaoFidelidade(long idPontuacaoFidelidade)
        {
            return Queryable()
                .Where(reg => reg.PontuacaoFidelidade.Id == idPontuacaoFidelidade)
                .ToList();
        }

        public List<ItemPontuacaoFidelidade> BuscarItens(long? idPontuacaoFidelidade = null)
        {
            var query = Queryable();

            if (idPontuacaoFidelidade.HasValue())
            {
                query = query.Where(reg => reg.PontuacaoFidelidade.Id == idPontuacaoFidelidade);
            }

            return query.ToList();
        }

        public List<ItemPontuacaoFidelidade> BuscarItens(long idPontuacaoFidelidade,long idEmpresaVenda)
        {
            return Queryable()
                .Where(x => x.PontuacaoFidelidade.Id == idPontuacaoFidelidade)
                .Where(x => x.EmpresaVenda.Id == idEmpresaVenda)
                .ToList();
        }

        public List<ItemPontuacaoFidelidade> EmpreendimentosComNormalAtiva(List<long> empreendimentos)
        {
            return Queryable()
                .Where(x => empreendimentos.Contains(x.Empreendimento.Id))
                .Where(x => x.PontuacaoFidelidade.TipoPontuacaoFidelidade == TipoPontuacaoFidelidade.Normal)
                .Where(x => x.Situacao == SituacaoPontuacaoFidelidade.Ativo)
                .ToList();
        }

        public List<ItemPontuacaoFidelidade> ItensDePontuacaoFidelidade(List<long> idsPontuacao)
        {
            return Queryable()
                .Where(x => idsPontuacao.Contains(x.PontuacaoFidelidade.Id))
                .ToList();
        }

        public ItemPontuacaoFidelidade BuscarItemAtivoPorEmpreendimentoEmpresaVenda(long idEmpreendimento, long idEmpresaVenda, TipoModalidadeProgramaFidelidade modalidade)
        {
            return Queryable()
                    .Where(x => x.Empreendimento.Id == idEmpreendimento)
                    .Where(x => x.EmpresaVenda.Id == idEmpresaVenda)
                    .Where(x => x.Situacao == SituacaoPontuacaoFidelidade.Ativo)
                    .Where(x => x.Modalidade == modalidade)
                    .SingleOrDefault();
        }

        public ItemPontuacaoFidelidade BuscarItemAtivoPorEmpreendimentoEmpresaVendaQuant(long idEmpreendimento, long idEmpresaVenda, TipoModalidadeProgramaFidelidade modalidade, long quant)
        {
            return Queryable()
                    .Where(x => x.Empreendimento.Id == idEmpreendimento)
                    .Where(x => x.EmpresaVenda.Id == idEmpresaVenda)
                    .Where(x => x.Situacao == SituacaoPontuacaoFidelidade.Ativo)
                    .Where(x => x.Modalidade == modalidade)
                    .Where(x => x.QuantidadeMinima == quant)
                    .FirstOrDefault();
        }
        public List<ItemPontuacaoFidelidade> ItensNormaisAtivosESuspensos(
            List<Empreendimento> empreendimentos, List<long> empresaVendas,
            TipoPontuacaoFidelidade tipoPontuacao)
        {
            var situacoes = new List<SituacaoPontuacaoFidelidade> { SituacaoPontuacaoFidelidade.Ativo, SituacaoPontuacaoFidelidade.Suspenso };
            return Queryable()
                .Where(x => empreendimentos.Contains(x.Empreendimento))
                .Where(x => empresaVendas.Contains(x.EmpresaVenda.Id))
                .Where(x => x.PontuacaoFidelidade.TipoPontuacaoFidelidade == tipoPontuacao)
                .Where(x => situacoes.Contains(x.Situacao))
                .ToList();
        }

        public ItemPontuacaoFidelidade BuscarItemNaData(long idEmpresaVenda, long idEmpreendimento, DateTime data)
        {
            return Queryable()
                .Where(x => x.EmpresaVenda.Id == idEmpresaVenda)
                .Where(x => x.Empreendimento.Id == idEmpreendimento)
                .Where(x => x.Situacao != SituacaoPontuacaoFidelidade.Rascunho)
                .Where(x => x.Situacao != SituacaoPontuacaoFidelidade.AguardandoLiberacao)
                .Where(x => data.Date >= x.InicioVigencia.Value.Date)
                .Where(x => data.Date <= x.TerminoVigencia.Value.Date)
                .OrderByDescending(x => x.PontuacaoFidelidade.TipoPontuacaoFidelidade)
                .FirstOrDefault();
        }

        public List<ItemPontuacaoFidelidade> BuscarItensNaData(long idEmpresaVenda, long idEmpreendimento, DateTime data)
        {
            return Queryable()
                .Where(x => x.EmpresaVenda.Id == idEmpresaVenda)
                .Where(x => x.Empreendimento.Id == idEmpreendimento)
                .Where(x => x.Situacao != SituacaoPontuacaoFidelidade.Rascunho)
                .Where(x => x.Situacao != SituacaoPontuacaoFidelidade.AguardandoLiberacao)
                .Where(x => data.Date >= x.InicioVigencia.Value.Date)
                .Where(x => data.Date <= x.TerminoVigencia.Value.Date)
                .OrderByDescending(x => x.PontuacaoFidelidade.TipoPontuacaoFidelidade)
                .OrderByDescending(x => x.QuantidadeMinima)
                .ToList();
        }

        public ItemPontuacaoFidelidade BuscarItemVigente(long idEmpresaVenda, long idEmpreendimento, DateTime data)
        {
            return Queryable()
                .Where(x => x.EmpresaVenda.Id == idEmpresaVenda)
                .Where(x => x.Empreendimento.Id == idEmpreendimento)
                .Where(x => x.Situacao != SituacaoPontuacaoFidelidade.Rascunho)
                .Where(x => x.Situacao != SituacaoPontuacaoFidelidade.AguardandoLiberacao)
                .Where(x => data.Date >= x.InicioVigencia.Value.Date)
                .Where(x => x.TerminoVigencia == null)
                .OrderByDescending(x => x.InicioVigencia)
                .FirstOrDefault();
        }

        public List<ItemPontuacaoFidelidade> BuscarItensVigentes(long idEmpresaVenda, long idEmpreendimento, DateTime data)
        {
            return Queryable()
                .Where(x => x.EmpresaVenda.Id == idEmpresaVenda)
                .Where(x => x.Empreendimento.Id == idEmpreendimento)
                .Where(x => x.Situacao != SituacaoPontuacaoFidelidade.Rascunho)
                .Where(x => x.Situacao != SituacaoPontuacaoFidelidade.AguardandoLiberacao)
                .Where(x => data.Date >= x.InicioVigencia.Value.Date)
                .Where(x => x.TerminoVigencia == null)
                .OrderByDescending(x => x.InicioVigencia)
                .OrderByDescending(x => x.PontuacaoFidelidade.TipoPontuacaoFidelidade)
                .OrderByDescending(x => x.QuantidadeMinima)
                .ToList();
        }

        public override void Save(ItemPontuacaoFidelidade entity)
        {
            if (entity.Id.IsEmpty())
            {
                _session.SaveOrUpdate(entity);
            }

            var sequence = entity.Id.ToString();
            entity.Codigo = "IPF" + sequence.PadLeft(8, '0');

            if (entity.PontuacaoFidelidade.TipoPontuacaoFidelidade == TipoPontuacaoFidelidade.Campanha)
            {
                entity.Codigo = "IPFC" + sequence.PadLeft(8, '0');
            }

            _session.SaveOrUpdate(entity);

        }

        public List<ItemPontuacaoFidelidade> BuscarItemAtivoPorEmpresaVenda(long idEmpresaVenda)
        {
            return Queryable()
                    .Where(x => x.EmpresaVenda.Id == idEmpresaVenda)
                    .Where(x => x.Situacao == SituacaoPontuacaoFidelidade.Ativo)
                    .ToList();
        }

    }
}
