using Europa.Extensions;
using Europa.Resources;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class FornecedorValidator : AbstractValidator<Fornecedor>
    {
        public FornecedorRepository _fornecedorRepository { get; set; }

        public FornecedorValidator(FornecedorRepository fornecedorRepository)
        {
            _fornecedorRepository = fornecedorRepository;
            RuleFor(forn => forn.CNPJ).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Cnpj));
            RuleFor(forn => forn).Must(forn => !CheckIfExistsCNPJ(forn)).WithName("CNPJ").WithMessage(string.Format(GlobalMessages.MsgErroRegistroExistente, GlobalMessages.Fornecedor, GlobalMessages.Cnpj));
            RuleFor(forn => forn).Must(forn => CheckIfCNPJIsValid(forn)).WithName("CNPJ").WithMessage(string.Format(GlobalMessages.InformeParametroValido, GlobalMessages.Cnpj));
            RuleFor(forn => forn.NomeFantasia).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.NomeFantasia));
            RuleFor(forn => forn.RazaoSocial).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.RazaoSocial));
        }

        public bool CheckIfExistsCNPJ(Fornecedor fornecedor)
        {
            if (fornecedor.CNPJ.IsEmpty())
            {
                return false;
            }
            return _fornecedorRepository.CheckIfExistsCNPJ(fornecedor);
        }

        public bool CheckIfCNPJIsValid(Fornecedor fornecedor)
        {
            if (fornecedor.CNPJ.IsEmpty())
            {
                return true;
            }
            return fornecedor.CNPJ.OnlyNumber().IsValidCNPJ();
        }
    }
}
