using Europa.Extensions;
using Europa.Web.Menu;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.Core.Services;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Models;
using Tenda.Domain.Security.Repository;

namespace Tenda.Domain.Security.Services
{
    public class MenuService : BaseService
    {
        public MenuService(ISession Session) : base(Session)
        {
        }

        public MenuItem MontarMenuPorUsuario(string codigoSistema, long idUsuario)
        {
            UsuarioPerfilSistema usuarioPerfilSistema = new UsuarioPerfilSistemaRepository(Session)
                .Queryable()
                .Where(reg => reg.Usuario.Id == idUsuario)
                .SingleOrDefault(reg => reg.Sistema.Codigo == codigoSistema);

            if (usuarioPerfilSistema == null)
            {
                throw new Exception("O usuário não possui perfil para acessar este sistema");
            }

            return MontarMenuPerfil(codigoSistema, usuarioPerfilSistema.Perfil.Id);
        }

        public MenuItem MontarMenuPerfil(string codigoSistema, long idPerfil)
        {
            return MontarMenuPerfil(codigoSistema, ToPerfilList(idPerfil));
        }

        public MenuItem MontarMenuPerfil(string codigoSistema, List<long> perfis)
        {
            HierarquiaModulo moduloNivelZero = ListarModuloNivelZero(codigoSistema);

            MenuItem menu = null;
            if (moduloNivelZero != null)
            {
                menu = MontarMenuModulo(0, codigoSistema, perfis, moduloNivelZero.Id, moduloNivelZero.Nome, moduloNivelZero.Ordem);
                menu = MontarMenuUnidadeFuncional(0, menu, codigoSistema, perfis, moduloNivelZero.Id);
            }

            menu.Filhos = menu.Filhos.OrderBy(x => x.Ordem).ToList();

            return menu;
        }

        private MenuItem MontarMenuModulo(int nivel, string codigoSistema, List<long> perfis, long idModulo, string nomeModulo, int ordem)
        {
            IDictionary<long, HierarquiaModulo> modulos = ListarModulosOutrosNiveis(codigoSistema, perfis, idModulo);

            MenuItem menuItem = new MenuItem(nomeModulo, ordem);

            nivel++;
            MenuItem menuItemAux;
            foreach (var moduloFilho in modulos)
            {
                menuItemAux = null;
                menuItemAux = MontarMenuModulo(nivel, codigoSistema, perfis, moduloFilho.Key, moduloFilho.Value.Nome, moduloFilho.Value.Ordem);
                menuItemAux = MontarMenuUnidadeFuncional(nivel, menuItemAux, codigoSistema, perfis, moduloFilho.Key);
                menuItem.AddFilho(menuItemAux);
            }

            return menuItem;
        }

        private MenuItem MontarMenuUnidadeFuncional(int nivel, MenuItem menuItem, string codigoSistema, List<long> perfis, long idModulo)
        {
            IList<UnidadeFuncional> unidadesFuncionais = ListarUnidadesFuncionais(codigoSistema, perfis, idModulo);

            if (unidadesFuncionais == null || unidadesFuncionais.IsEmpty())
            {
                return menuItem;
            }

            MenuItem menuItemAux;
            foreach (UnidadeFuncional ufInstance in unidadesFuncionais)
            {
                menuItemAux = new MenuItem(ufInstance.Codigo, ufInstance.Nome, ufInstance.EnderecoAcesso, ufInstance.Ordem);
                menuItem.AddFilho(menuItemAux);
            }

            return menuItem;
        }

        private HierarquiaModulo ListarModuloNivelZero(string codigoSistema)
        {
            return new HierarquiaModuloRepository(Session)
                .Queryable()
                .Where(h => h.Sistema.Codigo == codigoSistema)
                .Where(h => h.Situacao == Situacao.Ativo)
                .First(h => h.Pai == null);
        }

        private IDictionary<long, HierarquiaModulo> ListarModulosOutrosNiveis(string codigoSistema, long idPerfil, long idModulo)
        {
            return ListarModulosOutrosNiveis(codigoSistema, ToPerfilList(idPerfil), idModulo);
        }

        private IDictionary<long, HierarquiaModulo> ListarModulosOutrosNiveis(string codigoSistema, List<long> perfis, long idModulo)
        {
            var RepositoryPermissao = new PermissaoRepository(Session);

            var resultadoModulo = new HierarquiaModuloRepository(Session).Queryable()
                .Where(h => h.Sistema.Codigo == codigoSistema)
                .Where(h => h.Situacao == Situacao.Ativo)
                .Where(h => h.Pai.Id == idModulo)
                .OrderBy(h => h.Ordem)
                .Where(h => RepositoryPermissao.Queryable()
                    .Where(p => perfis.Contains(p.Perfil.Id))
                    .Where(p => p.Funcionalidade.UnidadeFuncional.Situacao == Situacao.Ativo)
                    .Where(p => p.Funcionalidade.UnidadeFuncional.ExibirMenu)
                    .Any(p => p.Funcionalidade.UnidadeFuncional.Modulo.Id == h.Id)
                )
                .ToDictionary(h => h.Id, h => h);

            var modulosPerm = RepositoryPermissao.Queryable()
                    .Where(p => perfis.Contains(p.Perfil.Id))
                    .Where(p => p.Funcionalidade.UnidadeFuncional.Situacao == Situacao.Ativo)
                    .Where(p => p.Funcionalidade.UnidadeFuncional.ExibirMenu)
                    .Select(p => p.Funcionalidade.UnidadeFuncional.Modulo).ToList();

            modulosPerm = modulosPerm.Where(x => x.Pai != null)
                .GroupBy(x => x.Id).Select(x => x.ElementAt(0)).ToList();

            foreach (var modulo in modulosPerm)
            {
                HierarquiaModulo encontrado = null;
                HierarquiaModulo aux = modulo;
                while (aux != null && encontrado == null)
                {
                    if (!aux.Pai.IsEmpty() && aux.Pai.Id == idModulo)
                    {
                        encontrado = aux;
                        aux = null;
                    }
                    else
                    {
                        aux = aux.Pai;
                    }
                }
                if (!encontrado.IsEmpty())
                {
                    if (!resultadoModulo.ContainsKey(encontrado.Id))
                    {
                        resultadoModulo.Add(encontrado.Id, encontrado);
                    }
                }
            }


            return resultadoModulo;
        }

        private IList<UnidadeFuncional> ListarUnidadesFuncionais(string codigoSistema, List<long> perfis, long idModulo)
        {
            var repositoryPermissao = new PermissaoRepository(Session);

            var resultadoUnidadeFuncional = new UnidadeFuncionalRepository(Session).Queryable()
                .Where(u => u.Modulo.Sistema.Codigo == codigoSistema)
                .Where(u => u.Modulo.Situacao == Situacao.Ativo)
                .Where(u => u.Situacao == Situacao.Ativo)
                .Where(u => u.ExibirMenu)
                .Where(u => u.Modulo.Id == idModulo);

            //Ordenando por nome no ADM
            if(codigoSistema == "DES1818")
            {
                resultadoUnidadeFuncional = resultadoUnidadeFuncional.OrderBy(u => u.Nome)
                .Where(u => repositoryPermissao.Queryable()
                    .Where(p => perfis.Contains(p.Perfil.Id))
                    .Any(p => p.Funcionalidade.UnidadeFuncional.Id == u.Id)
                ); 
            }
            else
            {
                resultadoUnidadeFuncional = resultadoUnidadeFuncional.OrderBy(u => u.Ordem)
                .Where(u => repositoryPermissao.Queryable()
                    .Where(p => perfis.Contains(p.Perfil.Id))
                    .Any(p => p.Funcionalidade.UnidadeFuncional.Id == u.Id)
                );
            }
                

            return resultadoUnidadeFuncional.ToList();
        }

        private List<long> ToPerfilList(long idPerfil)
        {
            List<long> perfis = new List<long>();
            perfis.Add(idPerfil);
            return perfis;
        }
    }
}
