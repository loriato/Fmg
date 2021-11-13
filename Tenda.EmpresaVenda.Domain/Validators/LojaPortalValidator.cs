using Europa.Resources;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebPages;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class LojaPortalValidator : AbstractValidator<Tenda.Domain.EmpresaVenda.Models.EmpresaVenda>
    {
        private EmpresaVendaRepository EmpresaVendaRepository { get; set; }
        private RegionalEmpresaRepository _regionalEmpresaRepository { get; set; }
        public LojaPortalValidator()
        {
            RuleFor(x => x.NomeFantasia).NotEmpty()
                .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Nome));
            RuleFor(x => x.RazaoSocial).NotEmpty()
                .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.NomeComercial));
            RuleFor(x => x).Must(x => !EmpresaVendaRepository.ExisteLoja(x.NomeFantasia, x.Id)).When(x => !x.NomeFantasia.IsEmpty())
                .WithMessage(x =>
                    string.Format(GlobalMessages.RegistroNaoUnico, GlobalMessages.Loja, x.NomeFantasia));
            RuleFor(x => x.PessoaContato).NotEmpty()
                .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.PessoaContato));
            RuleFor(x => x.TelefoneContato).NotEmpty()
                .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Telefone));
            RuleFor(x => x.Loja.Id).NotEmpty()
                .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.CentralVendas));
            RuleFor(x => x.Loja.Id).NotEmpty()
    .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.CentralVendas));

            //endereco
            RuleFor(x => x.Cep).NotEmpty()
                .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.CEP));
            RuleFor(x => x.Logradouro).NotEmpty()
                .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Endereco));
            RuleFor(x => x.Numero).NotEmpty()
                .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Numero));
            RuleFor(x => x.Bairro).NotEmpty()
                .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Bairro));
            RuleFor(x => x.Cidade).NotEmpty()
                .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Cidade));
            RuleFor(x => x.Estado).NotEmpty()
                .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Estado));
            RuleFor(x => x.ConsiderarUF).NotEmpty()
                .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.ConsiderarUf));


        }

    }
}
