using Europa.Extensions;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tenda.Domain.Security.Data;
using Tenda.Domain.Security.Models;
using Tenda.Domain.Security.Repository;

namespace Tenda.Domain.Shared.Log
{
    public static class SecurityLogger
    {
        private static IDictionary<string, IDictionary<string, IDictionary<string, Funcionalidade>>> _sistemaTree;

        private static bool _inicialized;

        public static void Logar(string sistema, long idAcesso, long idUsuario, string unidadeFuncional, string funcionalidade, string entidade, long id, string conteudo)
        {
            if (!_inicialized)
            {
                Init();
            }

            long shouldLog = NeedLogActionSecurityTree(sistema, unidadeFuncional, funcionalidade);
            if (shouldLog != 0)
            {
                DateTime timestamp = DateTime.Now;

                LogAcao log = new LogAcao();
                log.CriadoEm = timestamp;
                log.CriadoPor = idUsuario;
                log.AtualizadoEm = timestamp;
                log.AtualizadoPor = idUsuario;
                log.ComPermissao = true;
                log.Entidade = entidade;
                log.ChavePrimaria = id;
                log.Conteudo = conteudo;
                log.Funcionalidade = new Funcionalidade { Id = shouldLog };
                log.Acesso = new Acesso { Id = idAcesso };
                log.Momento = timestamp;

                IStatelessSession session = NHibernateSession.StatelessSession(ProjectProperties.CsEmpresaVenda);
                session.Insert(log);
                session.CloseIfOpen();
            }
        }

        public static void Init()
        {
            TearUpSecurityTree();
            _inicialized = true;
        }

        #region SecurityTree

        private static void TearUpSecurityTree()
        {
            ISession session = NHibernateSession.Session(ProjectProperties.CsEmpresaVenda);

            SistemaRepository sistemaRepository = new SistemaRepository(session);
            UnidadeFuncionalRepository unidadeFuncionalRepository = new UnidadeFuncionalRepository(session);
            FuncionalidadeRepository funcionalidadeRepository = new FuncionalidadeRepository(session);

            List<Sistema> sistemas = sistemaRepository.Queryable()
                .OrderBy(sist => sist.Codigo)
                .ToList();

            IDictionary<string, IDictionary<string, IDictionary<string, Funcionalidade>>> sistemaDictionary
                = new Dictionary<string, IDictionary<string, IDictionary<string, Funcionalidade>>>();

            foreach (Sistema sistema in sistemas)
            {
                var unidadesFuncionais = unidadeFuncionalRepository.Queryable()
                    .Where(unfu => unfu.Modulo.Sistema.Id == sistema.Id)
                    .OrderBy(unfu => unfu.Codigo)
                    .ToList();

                IDictionary<string, IDictionary<string, Funcionalidade>> unidadeFuncionalDictionary
                        = new Dictionary<string, IDictionary<string, Funcionalidade>>();

                foreach (UnidadeFuncional unidadeFuncional in unidadesFuncionais)
                {
                    var funcionalidades = funcionalidadeRepository.Queryable()
                        .Where(func => func.UnidadeFuncional.Id == unidadeFuncional.Id)
                        .OrderBy(func => func.Comando)
                        .ToDictionary(func => func.Comando, func => func, StringComparer.OrdinalIgnoreCase);

                    unidadeFuncionalDictionary.Add(unidadeFuncional.Codigo, funcionalidades);
                }

                //Ajustar
                sistemaDictionary.Add(sistema.Codigo.ToUpper(), unidadeFuncionalDictionary);

            }

            _sistemaTree = sistemaDictionary;

            session.CloseIfOpen();
        }

        private static string PrintSecurityTree()
        {
            StringBuilder builder = new StringBuilder();

            foreach (var sistema in _sistemaTree)
            {
                builder.AppendLine(sistema.Key);
                foreach (var unidadeFuncional in sistema.Value)
                {
                    builder.Append("\t").Append(unidadeFuncional.Key).AppendLine();

                    foreach (var funcionalidade in unidadeFuncional.Value)
                    {
                        builder.Append("\t").Append(unidadeFuncional.Key).Append("\t").Append(funcionalidade.Key)
                            .Append(" -> ").Append(funcionalidade.Value).AppendLine();
                    }
                }

                builder.AppendLine(sistema.Key);
            }

            return builder.ToString();
        }

        private static long NeedLogActionSecurityTree(string sistema, string unidadeFuncional, string funcionalidade)
        {
            IDictionary<string, IDictionary<string, Funcionalidade>> unidadeFuncionalTree;
            if (!_sistemaTree.TryGetValue(sistema, out unidadeFuncionalTree))
            {
                // Sistema não encontrado!
                return -1;
            }

            IDictionary<string, Funcionalidade> funcionalidadeTree;
            if (!unidadeFuncionalTree.TryGetValue(unidadeFuncional, out funcionalidadeTree))
            {
                // Unidade Funcional não encontrada!
                return -1;
            }

            Funcionalidade funcObject;
            if (!funcionalidadeTree.TryGetValue(funcionalidade, out funcObject))
            {
                // Funcionalidade não encontrada!
                return -1;
            }

            return funcObject.Logar ? funcObject.Id : 0;
        }

        #endregion
    }
}
