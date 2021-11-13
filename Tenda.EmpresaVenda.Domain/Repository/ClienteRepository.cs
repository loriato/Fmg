using Europa.Data;
using Europa.Extensions;
using System;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ClienteRepository : NHibernateRepository<Cliente>
    {
        public EmpresaVendaRepository _empresaVendaRepository  { get; set; }
        public CorretorRepository _corretorRepository { get; set; }
        public bool CheckIfExistsCpfCnpj(Cliente cliente)
        {
            return Queryable()
                .Where(clie => clie.Id != cliente.Id)
                .Where(clie => clie.CpfCnpj == cliente.CpfCnpj)
                .Where(clie => clie.EmpresaVenda.Id == cliente.EmpresaVenda.Id)
                .Any();
        }

        public IQueryable<Cliente> ListarClientes(string nome, string email, string telefone, string cpfCnpj, long? idEmpresaVenda, long? idCorretor)
        {

            var result = Queryable();

            if (!nome.IsEmpty())
                result = result.Where(x => x.NomeCompleto.ToLower().StartsWith(nome.ToLower()));
            if (!email.IsEmpty())
                result = result.Where(x => x.Email.Contains(email));
            if (!telefone.IsEmpty())
                result = result.Where(x => x.TelefoneResidencial == telefone || x.TelefoneComercial == telefone);
            if (!cpfCnpj.IsEmpty())
                result = result.Where(x => x.CpfCnpj.Equals(cpfCnpj));
            if (idEmpresaVenda.HasValue && idEmpresaVenda > 0)
                result = result.Where(x => x.EmpresaVenda.Id == idEmpresaVenda);
            if (idEmpresaVenda.HasValue() && idEmpresaVenda > 0)
            {   
                var corretor = _corretorRepository.FindById(idCorretor.Value);
                var funcao = corretor.Funcao;
                var podeVizualizar = corretor.EmpresaVenda.CorretorVisualizarClientes;
                if (funcao == Tenda.Domain.EmpresaVenda.Enums.TipoFuncao.Corretor)
                {
                    if (podeVizualizar)
                    {
                        result = result.Where(x => x.Corretor == idCorretor
                                        || x.Corretor == 0
                                        || x.Corretor == null);
                    }
                }
            }

            return result;
        }
        public Cliente BuscarPorIdSuat(long idSuat)
        {
            return Queryable().FirstOrDefault(x => x.IdSuat == idSuat);
        }
    }
}
