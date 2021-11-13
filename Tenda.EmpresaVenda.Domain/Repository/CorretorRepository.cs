using Europa.Data;
using Europa.Extensions;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Security.Enums;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class CorretorRepository : NHibernateRepository<Corretor>
    {
        public IQueryable<Corretor> Listar()
        {
            return Queryable();
        }

        public bool CheckIfExistsCPF(Corretor corretor)
        {
            return Queryable().Where(corr => corr.CPF == corretor.CPF).Where(corr => corr.Id != corretor.Id).Any();
        }

        public bool CheckCpfActiveOrSuspendedBroker(Corretor corretor)
        {
            return Queryable().Where(x => x.CPF.Contains(corretor.CPF)).Where(x => x.Usuario.Situacao == SituacaoUsuario.Ativo || x.Usuario.Situacao == SituacaoUsuario.Suspenso).Where(x => x.Id != corretor.Id).Any();
        }

        public IQueryable<Corretor> Listar(string nome, string cpfCnpjCreci, SituacaoUsuario[] situacao)
        {
            var query = Queryable();

            if (nome.HasValue())
            {
                nome = nome.ToLower();
                query = query.Where(reg =>
                    reg.Nome.ToLower().Contains(nome) ||
                    reg.Apelido.ToLower().Contains(nome));
            }

            if (cpfCnpjCreci.HasValue())
            {
                cpfCnpjCreci = cpfCnpjCreci.ToUpper();
                query = query.Where(reg =>
                    reg.CPF == cpfCnpjCreci ||
                    reg.CNPJ == cpfCnpjCreci ||
                    reg.Creci.ToUpper() == cpfCnpjCreci);
            }

            return query;
        }

        public Corretor FindByIdUsuario(long idUsuario)
        {
            return Queryable()
                .Where(reg => reg.Usuario.Id == idUsuario)
                .SingleOrDefault();
        }

        public Corretor FindbyTokenAtivacao(string tokenAtivacao)
        {
            return Queryable()
                .Where(reg => reg.Usuario.TokenAtivacao == tokenAtivacao)
                .Single();
        }

        public Corretor FindByEmail(string email)
        {
            return Queryable()
                .Where(reg => reg.Usuario.Email.ToLower().Equals(email.ToLower()))
                .SingleOrDefault();
        }

        public List<Corretor> ListarAtivos()
        {
            return Queryable()
                .Where(reg => reg.Usuario.Situacao == SituacaoUsuario.Ativo)
                .ToList();
        }

        public List<Corretor> ListarDiretoresAtivosDaRegional(string regional)
        {
            return Queryable()
                .Where(reg => reg.Usuario.Situacao == SituacaoUsuario.Ativo)
                .Where(reg => reg.EmpresaVenda.Estado == regional)
                .Where(reg => reg.Funcao == TipoFuncao.Diretor)
                .ToList();
        }

        public List<Corretor> ListarDiretoresAtivosDaEmpresaDeVendas(List<long> empresaVendas)
        {
            var diretores = Queryable()
                .Where(reg => reg.Usuario.Situacao == SituacaoUsuario.Ativo)
                .Where(reg => empresaVendas.Contains(reg.EmpresaVenda.Id))
                .Where(reg => reg.Funcao == TipoFuncao.Diretor)
                .ToList();

            return diretores;
        }

        public List<Corretor> ListarCorretoresCpf(string cpf)
        {
            return Queryable()
                .Where(x => x.CPF.Contains(cpf))
                .ToList();
        }

        public List<Corretor> ListarDiretoresAtivosDaEmpresaDeVendas(long idEmpresaVenda)
        {
            var diretores = Queryable()
                .Where(reg => reg.Usuario.Situacao == SituacaoUsuario.Ativo)
                .Where(reg => idEmpresaVenda == reg.EmpresaVenda.Id)
                .Where(reg => reg.Funcao == TipoFuncao.Diretor)
                .ToList();

            return diretores;
        }

        public List<long> ListarEmailsDiretoresEmpresaDeVendas(List<long> idEmpresaVenda)
        {
            return Queryable().Where(x => x.Usuario.Situacao == SituacaoUsuario.Ativo)
                                .Where(x => idEmpresaVenda.Contains(x.EmpresaVenda.Id))
                                .Where(x => x.Funcao == TipoFuncao.Diretor)
                                .Select(x => x.Usuario.Id)
                                .ToList();
        }

        public List<Corretor> ListarUsuariosAtivosDaEmpresaDeVendas(List<long> empresaVendas)
        {
            var diretores = Queryable()
                .Where(reg => reg.Usuario.Situacao == SituacaoUsuario.Ativo)
                .Where(reg => empresaVendas.Contains(reg.EmpresaVenda.Id))
                .ToList();

            return diretores;
        }

        public List<Corretor> ListarTodosDiretoresAtivos()
        {
            return Queryable().Where(reg => reg.Usuario.Situacao == SituacaoUsuario.Ativo)
                .Where(x => x.Funcao == TipoFuncao.Diretor)
                .ToList();
        }

        public bool CorretorValido (long idUsuario, long idLoja)
        {
            var query = Queryable();

            var isValid = !query.Where(x => x.Usuario.Id == idUsuario)
                .Where(x => x.EmpresaVenda != null)
                .Where(x => x.EmpresaVenda.Id != idLoja)
                .Where(x => x.EmpresaVenda.TipoEmpresaVenda == TipoEmpresaVenda.Loja).Any();


            return isValid;
        }

        public bool HasLoja(long idUsuario)
        {
            var query = Queryable();

            var isValid = query.Where(x => x.EmpresaVenda != null)
                .Where(x => x.EmpresaVenda.TipoEmpresaVenda == TipoEmpresaVenda.Loja)
                .Where(x => x.Usuario.Id == idUsuario)
                .Any();

            return isValid;
        }

        public DataSourceResponse<Corretor> ListarPorTipoCorretor(DataSourceRequest request, TipoCorretor tipoCorretor)
        {
            var query = Queryable().Where(x => x.TipoCorretor == tipoCorretor);

            var nome = request.filter?.FirstOrDefault(reg => reg.column.ToLower() == "nome")?.value;
            if (!nome.IsEmpty())
            {
                query = query.Where(x => x.Nome.ToLower().Contains(nome.ToLower()));
            }

            return query.ToDataRequest(request);
        }
    }
}
