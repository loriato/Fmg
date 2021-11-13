using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using NHibernate;
using Tenda.Domain.Core.Services;
using Tenda.Domain.Security.Models;
using Tenda.Domain.Security.Repository;

namespace Tenda.Domain.Security.Services
{
    public class ParametroService : BaseService
    {
        private readonly ParametroRepository _repositorioParametro;

        public ParametroService(ISession session, ParametroRepository repositorioParametro) : base(session)
        {
            _repositorioParametro = repositorioParametro;
        }

        public DataSourceResponse<Parametro> ListarDatatable(DataSourceRequest request, string chave, long? idSistema)
        {
            var results = _repositorioParametro.BuscarParametros(chave, idSistema);
            return results.ToDataRequest(request);
        }

        public void Salvar(Parametro parametro)
        {
            _repositorioParametro.Save(parametro);
        }

        public void Excluir(long id)
        {
            var exc = new BusinessRuleException();
            var parametro = _repositorioParametro.GetById(id);
            if (parametro != null)
            {
                try
                {
                    _repositorioParametro.Delete(parametro);
                }
                catch (BusinessRuleException ex)
                {
                    exc.Errors.AddRange(ex.Errors);
                }
            }
            else
            {
                exc.AddError(GlobalMessages.ErroNenhumParametroEncontrado).Complete();
            }
            exc.ThrowIfHasError();
        }

        public void ValidarCampos(Parametro parametro, long? idSistema)
        {
            var ex = new BusinessRuleException();

            if (parametro.Chave.IsEmpty())
            {
                ex.AddError(GlobalMessages.ParametroObrigatorio).WithParam(GlobalMessages.Chave).Complete();
            }
            if (parametro.Valor.IsNull())
            {
                ex.AddError(GlobalMessages.ParametroObrigatorio).WithParam(GlobalMessages.Valor).Complete();
            }
            if (!idSistema.HasValue || idSistema < 1)
            {
                ex.AddError(GlobalMessages.ParametroObrigatorio).WithParam(GlobalMessages.Sistema).Complete();
            }
            ex.ThrowIfHasError();

            var hasChave = _repositorioParametro.HasChave(parametro, idSistema.Value);
            if (hasChave)
            {
                ex.AddError(GlobalMessages.ErroParametroJaPossuiChave).Complete();
            }
            ex.ThrowIfHasError();
        }

    }
}
