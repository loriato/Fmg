using Europa.Extensions;
using Europa.Resources;
using FluentValidation;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class EmpresaVendaValidator : AbstractValidator<Tenda.Domain.EmpresaVenda.Models.EmpresaVenda>
    {
        public EmpresaVendaRepository _empresaVendaRepository { get; set; }

        public EmpresaVendaValidator(EmpresaVendaRepository empresaVendaRepository)
        {
            _empresaVendaRepository = empresaVendaRepository;

            // Dados Empresa Venda
            RuleFor(emve => emve.CNPJ).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Cnpj));
            RuleFor(emve => emve).Must(emve => !CheckIfExistsCNPJ(emve)).WithName("CNPJ").WithMessage(string.Format(GlobalMessages.MsgErroRegistroExistente, GlobalMessages.EmpresaVendas, GlobalMessages.Cnpj));
            RuleFor(emve => emve).Must(emve => CheckIfCNPJIsValid(emve)).WithName("CNPJ").WithMessage(string.Format(GlobalMessages.InformeParametroValido, GlobalMessages.Cnpj));
            RuleFor(emve => emve.RazaoSocial).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.RazaoSocial));
            RuleFor(emve => emve.NomeFantasia).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.NomeFantasia));
            RuleFor(emve => emve.Situacao).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Situacao));
            RuleFor(emve => emve.CodigoFornecedor).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.CodigoFornecedorSap));
            RuleFor(emve => emve.CentralVendas).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.CentralVendas)); // TODO: Será removido
            RuleFor(emve => emve.Loja.Id).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.CentralVendas));
            RuleFor(emve => emve.CreciJuridico).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.CreciJuridico));
            RuleFor(emve => emve.Simples).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Simples));
            RuleFor(emve => emve.SIMEI).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Simei));
            RuleFor(emve => emve.LucroPresumido).NotNull().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.LucroPresumido));
            RuleFor(emve => emve.LucroReal).NotNull().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.LucroReal));
            RuleFor(emve => emve.ConsiderarUF).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.ConsiderarUf));

            // Dados Endereço
            RuleFor(emve => emve.Cep).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.CEP));
            RuleFor(emve => emve.Logradouro).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Endereco));
            RuleFor(emve => emve.Numero).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Numero));
            RuleFor(emve => emve.Bairro).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Bairro));
            RuleFor(emve => emve.Cidade).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Cidade));
            RuleFor(emve => emve.Estado).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Estado));
        }

        public bool CheckIfExistsCNPJ(Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda)
        {
            if (empresaVenda.CNPJ.IsEmpty())
            {
                return false;
            }
            return _empresaVendaRepository.CheckIfExistsCNPJ(empresaVenda);
        }

        public bool CheckIfCNPJIsValid(Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda)
        {
            if (empresaVenda.CNPJ.IsEmpty())
            {
                return true;
            }
            return empresaVenda.CNPJ.OnlyNumber().IsValidCNPJ();
        }
    }
}
