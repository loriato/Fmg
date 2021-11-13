using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.Core.Services;
using Tenda.Domain.Security.Models;
using Tenda.Domain.Security.Repository;

namespace Tenda.Domain.Security.Services
{
    public class UnidadeFuncionalService : BaseService
    {
        public UnidadeFuncionalRepository _unidadeFuncionalRepository { get; set; }
        public HierarquiaModuloRepository _hierarquiaModuloRepository { get; set; }

        public IDictionary<string, string> ListarCodigoNomeUnidadesFuncionaisDoSistema(string codigoSistema)
        {
            return _unidadeFuncionalRepository.Queryable()
                .Where(uf => uf.Modulo.Sistema.Codigo.ToLower().Equals(codigoSistema.ToLower()))
                .OrderBy(uf => uf.Codigo)
                .ToDictionary(reg => reg.Codigo.ToUpper(), reg => reg.Nome);
        }

        public UnidadeFuncional GetCodigoESistema(string codigo, string codigoSistema)
        {
            return _unidadeFuncionalRepository
                .Queryable()
                .Where(uf => uf.Modulo.Sistema.Codigo.ToLower().Equals(codigoSistema.ToLower()))
                .FirstOrDefault(uf => uf.Codigo == codigo);
        }

        public DataSourceResponse<HierarquiaModulo> ListarHierarquia(DataSourceRequest request)
        {
            var results = _hierarquiaModuloRepository.Queryable();
            if (request.filter.FirstOrDefault() != null)
            {
                var filtro = request.filter.FirstOrDefault().column.ToString().ToLower();
                var queryTerm = request.filter.FirstOrDefault().value.ToString().ToLower();
                if (filtro.Equals("nome"))
                {
                    results = results.Where(x => x.Nome.ToLower().Contains(queryTerm));
                }
                var filterIdSistema = request.filter.FirstOrDefault(x => x.column.ToLower() == "idsistema");
                if (filterIdSistema.HasValue() && filterIdSistema.value.HasValue())
                {
                    results = results.Where(x => x.Sistema.Id.ToString() == filterIdSistema.value);
                }
            }
            return results.ToDataRequest(request);
        }

        public UnidadeFuncional Salvar(UnidadeFuncional novo)
        {
            var validation = new BusinessRuleException();
            Validate(validation, novo);
            validation.ThrowIfHasError();
            if (novo.Id.HasValue())
            {
                var atualizar = _unidadeFuncionalRepository.FindById(novo.Id);
                atualizar.Nome = novo.Nome;
                atualizar.Codigo = novo.Codigo;
                atualizar.EnderecoAcesso = novo.EnderecoAcesso;
                atualizar.ExibirMenu = novo.ExibirMenu;
                atualizar.Modulo = novo.Modulo;
                atualizar.Situacao = novo.Situacao;
                _unidadeFuncionalRepository.Save(atualizar);
                _unidadeFuncionalRepository.Flush();
                return atualizar;
            }
            else
            {
                _unidadeFuncionalRepository.Save(novo);
            }
            return novo;
        }
        public void Validate(BusinessRuleException validation, UnidadeFuncional novo)
        {
            if (novo.Nome.IsEmpty())
            {
                validation.AddError(GlobalMessages.CampoObrigatorio).WithParam(GlobalMessages.Nome).Complete();
            }
            if (novo.EnderecoAcesso.IsEmpty())
            {
                validation.AddError(GlobalMessages.CampoObrigatorio).WithParam(GlobalMessages.Endereco).Complete();
            }
            if (novo.Modulo.IsNull())
            {
                validation.AddError(GlobalMessages.CampoObrigatorio).WithParam(GlobalMessages.Hierarquia).Complete();
            }
            if (novo.Codigo.IsEmpty())
            {
                validation.AddError(GlobalMessages.CampoObrigatorio).WithParam(GlobalMessages.Codigo).Complete();
            }
            validation.ThrowIfHasError();
        }

        public DataSourceResponse<UnidadeFuncional> ListarUnidadeFuncionalAutoComplete(DataSourceRequest request)
        {
            var results = _unidadeFuncionalRepository.Queryable();
            if (request.filter.FirstOrDefault() != null)
            {
                var filtro = request.filter.FirstOrDefault().column.ToString().ToLower();
                var queryTerm = request.filter.FirstOrDefault().value.ToString().ToLower();
                if (filtro.Equals("nome"))
                {
                    results = results.Where(x => x.Nome.ToLower().Contains(queryTerm));
                }
            }
            return results.ToDataRequest(request);
        }

    }
}
