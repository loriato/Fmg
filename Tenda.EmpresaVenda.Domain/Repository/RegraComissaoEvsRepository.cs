using Europa.Data;
using System.Collections.Generic;
using System.Linq;
using Europa.Extensions;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using NHibernate;
using System;
using Europa.Commons;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class RegraComissaoEvsRepository : NHibernateRepository<RegraComissaoEvs>
    {
        public RegraComissaoEvsRepository(ISession session) : base(session)
        {
        }

        public IQueryable<RegraComissaoEvs> BuscarPorRegraComissao(long idRegraComissao)
        {
            return Queryable().Where(x => x.RegraComissao.Id == idRegraComissao);
        }
        public IQueryable<RegraComissaoEvs> BuscarAtivos(long? idRegraComissao = null, long? idEmpresaVenda = null)
        {
            var query = Queryable().Where(x => x.Situacao == SituacaoRegraComissao.Ativo);

            if (idRegraComissao.HasValue())
            {
                query = query.Where(x => x.RegraComissao.Id == idRegraComissao.Value);
            }

            if (idEmpresaVenda.HasValue())
            {
                query = query.Where(x => x.EmpresaVenda.Id == idEmpresaVenda.Value);
            }
            
            return query;
        }
        public RegraComissaoEvs Buscar(long idEmpresaVenda, long idRegraComissao)
        {
            return Queryable().Where(x => x.EmpresaVenda.Id == idEmpresaVenda)
                .Where(x => x.RegraComissao.Id == idRegraComissao)
                .SingleOrDefault();
        }

        //Retorna filhos da nova regra que estejam ativos
        public List<RegraComissaoEvs> BuscarFilhosAtivos(long idRegraComissao)
        {
            var filhos = Queryable()
                .Where(x => x.RegraComissao.Id == idRegraComissao)
                .Where(x => x.Situacao == SituacaoRegraComissao.Ativo);

            return filhos.ToList();
        }

        //Retorna filhos da nova regra que estejam ativos
        public bool VerificarFilhosAtivos(long idRegraComissao)
        {
            return Queryable()
                .Where(x => x.RegraComissao.Id == idRegraComissao)
                .Where(x => x.Situacao == SituacaoRegraComissao.Ativo).Any();
        }

        public bool VerificarCamapnhasAtivas(long idRegraComissao)
        {
            return Queryable()
                .Where(x => x.RegraComissao.Id == idRegraComissao)
                .Where(x=>x.Tipo == TipoRegraComissao.Campanha)                
                .Where(x => x.Situacao == SituacaoRegraComissao.Ativo)
                .Where(x => x.TerminoVigencia < DateTime.Now)
                .Any();
        }

        public bool ExistemCampanhasAguardandoLiberacao(long idRegraComissao)
        {
            var lista = Queryable()
                .Where(x => x.RegraComissao.Id == idRegraComissao)
                .Where(x => x.Situacao == SituacaoRegraComissao.AguardandoLiberacao)
                .Where(x => x.Tipo == TipoRegraComissao.Campanha)
                .Where(x => x.InicioVigencia.Value <= DateTime.Now);

            return lista.Any();
        }

        public bool ExistemRegrasEvsSuspensas(long idRegraComissao)
        {
            var lista = Queryable()
                .Where(x => x.RegraComissao.Id == idRegraComissao)
                .Where(x => x.Situacao == SituacaoRegraComissao.SuspensoPorCampanha);

            return lista.Any();
        }

        //Retorna filhos da nova regra a serem ativados
        public List<RegraComissaoEvs> BuscarRegrasRascunho(long idRegraComissao)
        {
            var filhos = Queryable()
                .Where(x => x.RegraComissao.Id == idRegraComissao)
                .Where(x => x.Situacao == SituacaoRegraComissao.Rascunho);

            return filhos.ToList();
        }

        public List<RegraComissaoEvs> BuscarCampanhasAtivas(long idRegraComissao)
        {
            var lista = Queryable()
                .Where(x => x.RegraComissao.Id == idRegraComissao)
                .Where(x => x.Tipo == TipoRegraComissao.Campanha)
                .Where(x => x.Situacao == SituacaoRegraComissao.Ativo);
            
            return lista.ToList();
        }

        public List<RegraComissaoEvs> BuscarCampanhasAguardadoLiberacao(long idRegraComissao)
        {
            var lista = Queryable()
                .Where(x => x.RegraComissao.Id == idRegraComissao)
                .Where(x => x.Situacao == SituacaoRegraComissao.AguardandoLiberacao)
                .Where(x => x.Tipo == TipoRegraComissao.Campanha)
                .Where(x => x.InicioVigencia.Value <= DateTime.Now);

            return lista.ToList();
                
        }

        public List<RegraComissaoEvs> BuscarRegrasEvsAtivas(List<long> idsEvsRegraAtiva)
        {
            var filhos = Queryable()
                .Where(x => x.Situacao == SituacaoRegraComissao.Ativo)
                .Where(x => idsEvsRegraAtiva.Contains(x.EmpresaVenda.Id));                

            return filhos.ToList();
        }

        public List<RegraComissaoEvs> BuscarRegrasEvsSuspensas(List<long> idsEvsRegraAtiva)
        {
            var lista = Queryable()
                .Where(x => x.Situacao == SituacaoRegraComissao.SuspensoPorCampanha)
                .Where(x => idsEvsRegraAtiva.Contains(x.EmpresaVenda.Id));

            return lista.ToList();
        }

        public RegraComissaoEvs BuscarRegrasEvsSuspensas(long idEv)
        {
            return Queryable()
                .Where(x => x.Situacao == SituacaoRegraComissao.SuspensoPorCampanha)
                .Where(x => x.EmpresaVenda.Id == idEv)
                .SingleOrDefault();
        }

        public List<RegraComissaoEvs> BuscarCampanhasEvsAtivas(List<long> idsEvsRegraAtiva)
        {
            return Queryable()
                .Where(x=>x.Tipo == TipoRegraComissao.Campanha)
                .Where(x => x.Situacao == SituacaoRegraComissao.Ativo)
                .Where(x => idsEvsRegraAtiva.Contains(x.EmpresaVenda.Id))
                .ToList();
        }

        public List<RegraComissaoEvs> BuscarCampanhasEvsAguardando(List<long> idsEvsRegraAtiva)
        {
            return Queryable()
                .Where(x => x.Tipo == TipoRegraComissao.Campanha)
                .Where(x => x.Situacao == SituacaoRegraComissao.AguardandoLiberacao)
                .Where(x => idsEvsRegraAtiva.Contains(x.EmpresaVenda.Id))
                .ToList();
        }
        public List<RegraComissaoEvs> BuscarRegrasEvsInativas(List<long> idsEvsRegraAtiva)
        {
            var filhos = Queryable()
                .Where(x => x.Situacao == SituacaoRegraComissao.Vencido)
                .Where(x => idsEvsRegraAtiva.Contains(x.EmpresaVenda.Id));

            return filhos.ToList();
        }
        public RegraComissaoEvs BuscarCampanhaAguardandoInativacao( long idEmpresaVenda)
        {
            return Queryable()
                .Where(reg => reg.EmpresaVenda.Id == idEmpresaVenda)
                .Where(reg => reg.Tipo == TipoRegraComissao.Campanha)
                .Where(reg => reg.Situacao == SituacaoRegraComissao.Ativo)
                .Where(reg => reg.TerminoVigencia.Value < DateTime.Now)
                .SingleOrDefault();
        }
        public RegraComissaoEvs BuscarRegraEvsVigente(long idEmpresaVenda)
        {
            return Queryable()
                .Where(reg => reg.Situacao == SituacaoRegraComissao.Ativo)
                .Where(reg => reg.EmpresaVenda.Id == idEmpresaVenda)
                .SingleOrDefault();
        }

        public List<RegraComissao> BuscarCampanhaAguardandoLiberacaoEv(long idEmpresaVenda)
        {
            return Queryable()
                .Where(x=>x.EmpresaVenda.Id == idEmpresaVenda)
                .Where(x=>x.Tipo == TipoRegraComissao.Campanha)
                .Where(x=>x.Situacao == SituacaoRegraComissao.AguardandoLiberacao)
                .Where(x=>x.InicioVigencia.Value.Date <= DateTime.Now.Date)
                .Select(x=>x.RegraComissao)
                .ToList();
        }

        public bool TemRegraComissao(long idEmpresaVenda)
        {
            return Queryable()
                .Where(x => x.EmpresaVenda.Id == idEmpresaVenda)
                .Where(x => x.Situacao == SituacaoRegraComissao.Ativo || x.Situacao == SituacaoRegraComissao.SuspensoPorCampanha)
                .Any();
        }
        
        public List<RegraComissao> CampanhasComErro()
        {
            var lista = Queryable()
                .Where(x => x.Tipo == TipoRegraComissao.Campanha)
                .Where(x => x.Situacao == SituacaoRegraComissao.AguardandoLiberacao)
                .Where(x => x.RegraComissao.Situacao == SituacaoRegraComissao.Vencido)
                .Where(x=>x.RegraComissao.Tipo == TipoRegraComissao.Campanha)
                .Select(x=>x.RegraComissao);

            return lista.ToList();
        }

        public override void Save(RegraComissaoEvs entity)
        {
            if (entity.Id.IsEmpty())
            {
                _session.SaveOrUpdate(entity);               
            }

            if (entity.Codigo.IsEmpty())
            {
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
                    GenericFileLogUtil.DevLogWithDateOnBegin(
                        "Save>" +
                        "ID REGRA COMISSAO EVS: " + entity.Id +
                        " | ID USUARIO: " + entity.CriadoPor +
                        " | CRIADO EM: " + entity.CriadoEm +
                        " | ID EV: " + entity.EmpresaVenda.Id +
                        " | EV: " + entity.EmpresaVenda.NomeFantasia +
                        " | ID REGRA COMISSAO: " + entity.RegraComissao.Id +
                        " | REGIONAL: " + entity.Regional +
                        " | DESCRICAO: " + entity.Descricao +
                        " | CODIGO: " + entity.Codigo +
                        " | SITUACAO: " + entity.Situacao.AsString() +
                        " | TIPO: " + entity.Tipo.AsString() +
                        " | DATA DO EVENTO: " + DateTime.Now
                        );
                }
            }

            _session.SaveOrUpdate(entity);
            
        }

        public IQueryable<Tenda.Domain.EmpresaVenda.Models.EmpresaVenda> TemRegraComissao(List<long> idsEmpresaVenda)
        {
            return Queryable()
                .Where(x => idsEmpresaVenda.Contains(x.EmpresaVenda.Id))
                .Where(x => x.Situacao != SituacaoRegraComissao.Rascunho)
                .Select(x => x.EmpresaVenda);
        }

    }
}
