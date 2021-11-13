using Europa.Extensions;
using Europa.Resources;
using FluentValidation;
using System;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class ClienteValidator : AbstractValidator<Cliente>
    {
        public ClienteRepository _clienteRepository { get; set; }

        public ClienteValidator(ClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;

            When(x => x.CpfCnpj.IsEmpty(), () =>
            {
                RuleFor(clie => clie.CpfCnpj).NotEmpty().OverridePropertyName("Cliente.CpfCnpj")
                .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.CpfCnpj));
            }).Otherwise(() =>
            {
                RuleFor(clie => clie).Must(clie => !CheckIfExistsCpfCnpj(clie)).OverridePropertyName("Cliente.CpfCnpj")
                .WithMessage(string.Format(GlobalMessages.MsgErroRegistroExistenteEmpresaVenda, GlobalMessages.Cliente, GlobalMessages.CpfCnpj));
                RuleFor(clie => clie).Must(clie => CheckIfIsValidCpfCnpj(clie)).OverridePropertyName("Cliente.CpfCnpj")
                    .WithMessage(string.Format(GlobalMessages.DadoInvalido, GlobalMessages.CpfCnpj));
            });
                        
            RuleFor(clie => clie.NomeCompleto).NotEmpty().OverridePropertyName("Cliente.NomeCompleto")
                .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Nome));
            //RuleFor(clie => clie.TipoSexo).NotEmpty().OverridePropertyName("Cliente.TipoSexo")
            //.WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Sexo));

            //RuleFor(clie => clie).Must(clie => CheckContacts(clie)).OverridePropertyName("Cliente.TelefoneResidencial")
            //    .WithMessage(GlobalMessages.MsgErroContato);

            When(x => x.TelefoneResidencial.IsEmpty(), () =>
            {
                RuleFor(x => x.TelefoneResidencial).NotEmpty().OverridePropertyName("Cliente.TelefoneResidencial")
                    .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.TelefonePrincipal));
            }).Otherwise(() =>
            {
                RuleFor(x => x.TelefoneResidencial)
                    .Must(x => ValidarTelefone(x)).OverridePropertyName("Cliente.TelefoneResidencial")
                    .WithMessage(string.Format(GlobalMessages.CampoInvalido, GlobalMessages.TelefonePrincipal, GlobalMessages.Numero));

            });

            When(x => x.Email.IsEmpty(), () =>
            {
                RuleFor(clie => clie.Email).NotEmpty()
                .OverridePropertyName("Cliente.Email")
                .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Email));
            }).Otherwise(() =>
            {
                RuleFor(clie => clie.Email).EmailAddress().OverridePropertyName("Cliente.Email")
                .WithMessage(GlobalMessages.EmailInvalido);
            });
            
            //RuleFor(clie => clie.DataAdmissao).NotEmpty().OverridePropertyName("Cliente.DataAdmissao")
                //.WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.DataAdmissao));

            RuleFor(clie => clie).Must(clie => ValidarDataFutura(clie.DataNascimento)).When(clie => !clie.DataNascimento.IsEmpty()).OverridePropertyName("Cliente.DataNascimento")
                .WithMessage(string.Format(GlobalMessages.MsgCampoDataFuturo, GlobalMessages.DataNascimento));
            RuleFor(clie => clie).Must(clie => ValidarDataFutura(clie.DataEmissao)).When(clie => !clie.DataEmissao.IsEmpty()).OverridePropertyName("Cliente.DataEmissao")
                .WithMessage(string.Format(GlobalMessages.MsgCampoDataFuturo, GlobalMessages.DataEmissao));
            RuleFor(clie => clie).Must(clie => ValidarDataFutura(clie.DataAdmissao)).When(clie => !clie.DataAdmissao.IsEmpty()).OverridePropertyName("Cliente.DataAdmissao")
                .WithMessage(string.Format(GlobalMessages.MsgCampoDataFuturo, GlobalMessages.DataAdmissao));
            RuleFor(clie => clie).Must(clie => ValidarDataFutura(clie.DataUltimaParcelaFinanciamentoPaga)).When(clie => !clie.DataUltimaParcelaFinanciamentoPaga.IsEmpty()).OverridePropertyName("Cliente.DataUltimaParcelaFinanciamentoPaga")
                .WithMessage(string.Format(GlobalMessages.MsgCampoDataFuturo, GlobalMessages.DataUltimaParcelaFinanciamentoPaga));
            RuleFor(clie => clie).Must(clie => ValidarDataFutura(clie.DataUltimaPrestacaoPaga)).When(clie => !clie.DataUltimaPrestacaoPaga.IsEmpty()).OverridePropertyName("Cliente.DataUltimaPrestacaoPaga")
                .WithMessage(string.Format(GlobalMessages.MsgCampoDataFuturo, GlobalMessages.DataUltimaPrestacaoPaga));

            // Validação caso veículo seja financiado
            RuleFor(clie => clie.ValorUltimaParcelaFinanciamentoVeiculo).NotEmpty().When(clie => clie.VeiculoFinanciado.HasValue && clie.VeiculoFinanciado.Value)
                .OverridePropertyName("Cliente.ValorUltimaParcelaFinanciamentoVeiculo").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.ValorUltimaParcelaFinanciamentoVeiculo));
            RuleFor(clie => clie.DataUltimaParcelaFinanciamentoPaga).NotEmpty().When(clie => clie.VeiculoFinanciado.HasValue && clie.VeiculoFinanciado.Value)
                .OverridePropertyName("Cliente.DataUltimaParcelaFinanciamentoPaga").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.DataUltimaParcelaFinanciamentoPaga));

        }

        public bool ValidarDataFutura(DateTime? data)
        {
            if (data.IsEmpty() || data.Value.Date > DateTime.Today)
            {
                return false;
            }
            return true;
        }

        public bool CheckIfExistsCpfCnpj(Cliente cliente)
        {
            if (cliente.CpfCnpj.IsEmpty())
            {
                return true;
            }
            return _clienteRepository.CheckIfExistsCpfCnpj(cliente);
        }

        public bool CheckIfIsValidCpfCnpj(Cliente cliente)
        {
            if (cliente.CpfCnpj.IsEmpty())
            {
                return true;
            }
            return cliente.CpfCnpj.IsValidCPF() || cliente.CpfCnpj.IsValidCNPJ();
        }

        public bool CheckContacts(Cliente cliente)
        {
            if (cliente.TelefoneResidencial.IsEmpty() && cliente.TelefoneComercial.IsEmpty())
            {
                return false;
            }
            return true;
        }

        public bool ValidarTelefone(string telefone)
        {
            if (telefone.IsEmpty())
            {
                return true;
            }

            return telefone.OnlyNumber().HasValue();
        }
    }
}
