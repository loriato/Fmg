using Europa.Extensions;
using NHibernate;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tenda.Domain.Core.Services;
using Tenda.Domain.Security.Models;
using Tenda.Domain.Security.Repository;
using Tenda.Domain.Security.Views;

namespace Tenda.Domain.Security.Services
{
    public class PermissaoService : BaseService
    {
        public UsuarioPerfilSistemaRepository _repositorioUPS { get; set; }
        public PermissaoRepository _repositorioPermissao { get; set; }
        public FuncionalidadeRepository _repositorioFuncionalidade { get; set; }
        public ViewPerfilPermissaoRepository _repositorioPerfilPermissao { get; set; }

        public bool VerificarPermissao(string comandoFuncionalidade, string codigoUnidadeFuncional, long idUsuario, string codigoSistema)
        {
            if (comandoFuncionalidade.IsEmpty())
            {
                throw new ArgumentNullException("comandoFuncionalidade");
            }
            if (codigoUnidadeFuncional.IsEmpty())
            {
                throw new ArgumentNullException("codigoUnidadeFuncional");
            }
            if (idUsuario.IsEmpty())
            {
                throw new ArgumentNullException("idUsuario");
            }
            if (codigoSistema.IsEmpty())
            {
                throw new ArgumentNullException("codigoSistema");
            }



            return _repositorioUPS.Queryable()
                .Where(reg => reg.Sistema.Codigo == codigoSistema)
                .Where(reg => reg.Usuario.Id == idUsuario)
                .Any(reg => _repositorioPermissao.Queryable()
                                .Where(perm => perm.Perfil.Id == reg.Perfil.Id)
                                .Where(perm => perm.Funcionalidade.UnidadeFuncional.Codigo == codigoUnidadeFuncional)
                                .Where(perm => perm.Funcionalidade.Comando == comandoFuncionalidade)
                                .Any()
            );
        }

        public IDictionary<string, List<string>> Permissoes(string codigoSistema, long idPerfil)
        {
            return Permissoes(codigoSistema, ToPerfilList(idPerfil));

        }

        public Permissao Salvar(Permissao permissao)
        {
            _repositorioPermissao.Save(permissao);
            return permissao;
        }

        public void Remover(Permissao permissao)
        {
            _repositorioPermissao.Delete(permissao);
        }

        public void Remover(long funcionalidade, long perfil)
        {
            _repositorioPermissao.Delete(_repositorioPermissao.Queryable().SingleOrDefault(x => x.Perfil.Id.Equals(perfil) && x.Funcionalidade.Id.Equals(funcionalidade)));
            _repositorioPermissao.Flush();
        }

        public IDictionary<string, List<string>> Permissoes(string codigoSistema, List<long> perfis)
        {
            if (codigoSistema.IsNull())
            {
                throw new ArgumentNullException("codigoSistema");
            }
            if (!perfis.Any())
            {
                throw new ArgumentNullException("perfis");
            }

            var permissoes = _repositorioPermissao.Queryable()
                .Where(reg => perfis.Contains(reg.Perfil.Id))
                .Where(reg => reg.Funcionalidade.UnidadeFuncional.Modulo.Sistema.Codigo == codigoSistema)
                .GroupBy(reg => reg.Funcionalidade.UnidadeFuncional.Codigo,
                         reg => reg.Funcionalidade.Comando,
                         (key, g) => new
                         {
                             Codigo = key.ToUpper(),
                             Comandos = g.ToList()
                         }
                )
                .ToDictionary(reg => reg.Codigo, reg => reg.Comandos);

            return permissoes;
        }

        private List<long> ToPerfilList(long idPerfil)
        {
            List<long> perfis = new List<long>();
            perfis.Add(idPerfil);
            return perfis;
        }

        /*  Hermann Miertschink Neto
         *      FIXME: Diversos. Infelizmente sem tempo pra corrigir 
         *         - Usando a consulta SQL na mão e não via View. Não consegui resolver a consulta e deixar ela como uma view. Alguma junção estava dando problemas e não exibia o resultado esperado
         *         - Estou filtrando todos os resultados e depois paginando eles via Estrutura do DataSourceResponse. Ou Seja, filtro vários e jogo outros fora.
         *  */
        public DataSourceResponse<ViewPerfilPermissao> ListarFuncionalidades(DataSourceRequest request, string nome, string nomeUF, long perfil, string codSistema)
        {
            StringBuilder sqlQuery = new StringBuilder();
            sqlQuery.Append("SELECT ");
            sqlQuery.Append(" TBL_UNIDADES_FUNCIONAIS.CD_UNIDADE_FUNCIONAL AS \"CodigoUF\", ");
            sqlQuery.Append(" TBL_UNIDADES_FUNCIONAIS.NM_UNIDADE_FUNCIONAL AS \"NomeUF\", ");
            sqlQuery.Append(" TBL_FUNCIONALIDADES.ID_FUNCIONALIDADE AS \"Id\", ");
            sqlQuery.Append(" TBL_FUNCIONALIDADES.NM_FUNCIONALIDADE AS \"NomeFuncionalidade\", ");
            sqlQuery.Append(" TBL_FUNCIONALIDADES.FL_LOGAR as \"NumLogar\", ");
            sqlQuery.Append(" (SELECT COUNT(TBL_PERMISSOES.ID_PERMISSAO) FROM TBL_PERMISSOES ");
            sqlQuery.Append(" WHERE TBL_PERMISSOES.ID_FUNCIONALIDADE = TBL_FUNCIONALIDADES.ID_FUNCIONALIDADE ");
            sqlQuery.Append(" AND TBL_PERMISSOES.ID_PERFIL = :idPerfil) as \"NumPermitida\" ");
            sqlQuery.Append(" FROM TBL_FUNCIONALIDADES ");
            sqlQuery.Append(" INNER JOIN TBL_UNIDADES_FUNCIONAIS ON TBL_UNIDADES_FUNCIONAIS.ID_UNIDADE_FUNCIONAL = TBL_FUNCIONALIDADES.ID_UNIDADE_FUNCIONAL ");
            sqlQuery.Append(" WHERE 1 = 1 ");
            if (!nomeUF.IsEmpty())
            {
                sqlQuery.Append(" AND (LOWER(TBL_UNIDADES_FUNCIONAIS.NM_UNIDADE_FUNCIONAL) like LOWER(:nomeOuCodigoUf) OR LOWER(TBL_UNIDADES_FUNCIONAIS.CD_UNIDADE_FUNCIONAL) like LOWER(:nomeOuCodigoUf)) ");
            }
            if (!nome.IsEmpty())
            {
                sqlQuery.Append(" AND (LOWER(TBL_FUNCIONALIDADES.NM_FUNCIONALIDADE) like LOWER(:nomeFuncionalidade)) ");
            }

            sqlQuery.Append(" AND EXISTS (SELECT HM.ID_HIERARQUIA_MODULO FROM TBL_HIERARQUIAS_MODULO HM" +
                            " JOIN TBL_SISTEMAS SIS ON SIS.ID_SISTEMA = HM.ID_SISTEMA " +
                            " WHERE SIS.CD_SISTEMA = :codSistema AND TBL_UNIDADES_FUNCIONAIS.ID_HIERARQUIA_MODULO = HM.ID_HIERARQUIA_MODULO) ");

            IQuery query = Session.CreateSQLQuery(sqlQuery.ToString())
                    .AddScalar("Id", NHibernateUtil.Int64)
                    .AddScalar("CodigoUF", NHibernateUtil.AnsiString)
                    .AddScalar("NomeUF", NHibernateUtil.AnsiString)
                    .AddScalar("NomeFuncionalidade", NHibernateUtil.AnsiString)
                    .AddScalar("NumLogar", NHibernateUtil.Int32)
                    .AddScalar("NumPermitida", NHibernateUtil.Int32)
                  .SetResultTransformer(Transformers.AliasToBean<ViewPerfilPermissao>())
                  .SetInt64("idPerfil", perfil);

            if (!nomeUF.IsEmpty())
            {
                query = query.SetString("nomeOuCodigoUf", "%" + nomeUF + "%");
            }
            if (!nome.IsEmpty())
            {
                query = query.SetString("nomeFuncionalidade", "%" + nome + "%");
            }
            query = query.SetString("codSistema", codSistema);

            return query.List<ViewPerfilPermissao>().AsQueryable().ToDataRequest(request);
        }

    }
}