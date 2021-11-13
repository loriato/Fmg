using Europa.Commons;
using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class AvalistaService:BaseService
    {
        public AvalistaRepository _avalistaRepository { get; set; }
        public void SalvarAvalista(Avalista avalista)
        {
            // Remove máscara
            avalista.CPF = avalista.CPF.OnlyNumber();
            avalista.ContatoGerente = avalista.ContatoGerente.OnlyNumber();

            var bre = new BusinessRuleException();

            var result = new AvalistaValidator(_avalistaRepository).Validate(avalista);

            bre.WithFluentValidation(result);
            bre.ThrowIfHasError();

            _avalistaRepository.Save(avalista);
        }
    }
}
