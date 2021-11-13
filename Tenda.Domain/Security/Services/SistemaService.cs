using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using System.Linq;
using Tenda.Domain.Core.Services;
using Tenda.Domain.Security.Models;
using Tenda.Domain.Security.Repository;

namespace Tenda.Domain.Security.Services
{
    public class SistemaService : BaseService
    {
        public SistemaRepository _sistemaRepository { get; set; }
        public ParametroSistemaRepository _parametroSistemaRepository { get; set; }

        public Sistema FindByCodigo(string codigoSistema)
        {
            return _sistemaRepository.Queryable()
                .Where(reg => reg.Codigo == codigoSistema)
                .SingleOrDefault();
        }

        public ParametroSistema FindByCodigoSistema(string codigoSistema)
        {
            return _parametroSistemaRepository.Queryable()
               .Where(reg => reg.Sistema.Codigo == codigoSistema)
               .SingleOrDefault();
        }

        public Sistema FindById(long Id)
        {
            return _sistemaRepository.Queryable().FirstOrDefault(x => x.Id == Id);
        }

        public IQueryable<Sistema> ListarTodos()
        {
            var result = _sistemaRepository.Queryable();
            return result;
        }

        public Sistema Salvar(Sistema sistema)
        {
            var bre = new BusinessRuleException();
            var entidadePorCodigo = FindByCodigo(sistema.Codigo);

            if (entidadePorCodigo != null && sistema.Id != entidadePorCodigo.Id)
            {
                bre.Errors.Add(string.Format(GlobalMessages.ImpossivelIncluirSistema, sistema.Nome, sistema.Codigo));
            }
            else
            {
                var entidade = _sistemaRepository.FindById(sistema.Id);

                if (entidade != null)
                {
                    entidade.Codigo = sistema.Codigo;
                    entidade.EnderecoAcesso = sistema.EnderecoAcesso;
                    entidade.Nome = sistema.Nome;
                    entidade.Situacao = sistema.Situacao;
                }
                else
                {
                    entidade = sistema;
                }
                

                _sistemaRepository.Save(entidade);
            }

            bre.ThrowIfHasError();
            return sistema;
        }
        public IQueryable<Sistema> Listar(DataSourceRequest request)
        {
            var query = _sistemaRepository.Queryable();
            if(request.filter!=null && request.filter.Count>0)
            {
                foreach (var filtro in request.filter)
                    if (filtro.column.ToLower() == "nome")
                        query = query.Where(w => w.Nome.ToLower().Contains(filtro.value.ToLower()));
            }
            return query;
        }
    }
}
