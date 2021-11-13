using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using System;
using System.Collections.Concurrent;
using Tenda.Domain.Core.Services.Models;
using Tenda.Domain.Shared;
using Tenda.Domain.Shared.Log;
using Tenda.Domain.Tenda.CEP;

namespace Tenda.Domain.Core.Services
{
    public static class CepService
    {
        private static ConcurrentDictionary<string, CepDTO> _cache = new ConcurrentDictionary<string, CepDTO>();
        private const string _cepSeparator = "-";

        public static CepDTO ConsultaCEPWS(string cep)
        {
            CepDTO cepDto = null;
            try
            {
                var cleanCep = NormalizeCep(cep);
                if (cleanCep.Length != 8)
                {
                    return null;
                }

                if (_cache.TryGetValue(cleanCep, out cepDto))
                {
                    return cepDto;
                }

                BuscaCEPPortBindingQSService service = new BuscaCEPPortBindingQSService(ProjectProperties.EndpointCep);
                service.Timeout = 15000; //15 segundos
                endereco endereco = service.pesquisarEnderecoPorCEP(cleanCep);

                if (endereco == null)
                {
                    if (!cep.Contains(_cepSeparator))
                    {
                        cep = cep.Insert(5, _cepSeparator);
                    }
                    string erro = String.Format(GlobalMessages.CepNaoEncontrato, cep);
                    BusinessRuleException bre = new BusinessRuleException(erro);
                    bre.AddError(erro).Complete();
                    bre.ThrowIfHasError();
                }

                cepDto = new CepDTO(endereco);
                cepDto.cep = cepDto.cep.Insert(5, _cepSeparator);
                _cache.TryAdd(cleanCep, cepDto);
            }
            catch (BusinessRuleException)
            {
                throw;
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
                throw;
            }

            return cepDto;
        }

        private static string NormalizeCep(string cep)
        {
            if (cep.IsEmpty())
            {
                return null;
            }
            return cep.Replace(_cepSeparator, "");
        }

    }
}
