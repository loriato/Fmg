using Europa.Extensions;
using Europa.Resources;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class PreCadastroValidator : AbstractValidator<Tenda.Domain.EmpresaVenda.Models.EmpresaVenda>
    {
        public EmpresaVendaRepository _empresaVendaRepository { get; set; }

        public PreCadastroValidator(EmpresaVendaRepository empresaVendaRepository)
        {
            _empresaVendaRepository = empresaVendaRepository;

            // Dados Empresa Venda
            RuleFor(emve => emve.RazaoSocial).NotEmpty().OverridePropertyName("EmpresaVenda.RazaoSocial").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.RazaoSocial + " [" + GlobalMessages.DadosCadastrais + "]"));
            RuleFor(emve => emve.NomeFantasia).NotEmpty().OverridePropertyName("EmpresaVenda.NomeFantasia").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.NomeFantasia + " [" + GlobalMessages.DadosCadastrais + "]"));
            RuleFor(emve => emve.CNPJ).NotEmpty().OverridePropertyName("EmpresaVenda.CNPJ").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Cnpj + " [" + GlobalMessages.DadosCadastrais + "]"));
            RuleFor(emve => emve).Must(emve => !CheckIfExistsCNPJAtivoSuspensa(emve)).OverridePropertyName("EmpresaVenda.CNPJ").WithMessage(emve => string.Format(GlobalMessages.MsgErroCNPJAtivoSuspenso,emve.CNPJ));
            RuleFor(emve => emve).Must(emve => !CheckIfExistsCNPJCancelada(emve)).OverridePropertyName("EmpresaVenda.CNPJ").WithMessage(emve => string.Format(GlobalMessages.MsgErroCNPJCancelada, emve.CNPJ));
            RuleFor(emve => emve).Must(emve => !CheckIfExistsCNPJPreCadastro(emve)).OverridePropertyName("EmpresaVenda.CNPJ").WithMessage(emve => string.Format(GlobalMessages.MsgErroCNPJPreCadastro, emve.CNPJ));
            RuleFor(emve => emve).Must(emve => CheckIfCNPJIsValid(emve)).OverridePropertyName("EmpresaVenda.CNPJ").WithMessage(string.Format(GlobalMessages.InformeParametroValido, GlobalMessages.Cnpj + " [" + GlobalMessages.DadosCadastrais + "]"));
            RuleFor(emve => emve.Situacao).NotEmpty().OverridePropertyName("EmpresaVenda.Situacao").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Situacao));
            RuleFor(emve => emve.CentralVendas).NotEmpty().OverridePropertyName("EmpresaVenda.CentralVendas").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.CentralVendas)); // TODO: Será removido      
            RuleFor(emve => emve.CreciJuridico).NotEmpty().OverridePropertyName("EmpresaVenda.CreciJuridico").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.CreciJuridico + " [" + GlobalMessages.DadosCadastrais + "]"));
            RuleFor(emve => emve.InscricaoMunicipal).NotEmpty().OverridePropertyName("EmpresaVenda.InscricaoMunicipal").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.InscricaoMunicipal + " [" + GlobalMessages.DadosCadastrais + "]"));
            RuleFor(emve => emve.InscricaoEstadual).NotEmpty().OverridePropertyName("EmpresaVenda.InscricaoEstadual").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.InscricaoEstadual + " [" + GlobalMessages.DadosCadastrais + "]"));


            RuleFor(emve => emve.Cep).NotEmpty().OverridePropertyName("EmpresaVenda.Cep").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.CEP + " [" + GlobalMessages.DadosCadastrais + "]"));
            RuleFor(emve => emve.Logradouro).NotEmpty().OverridePropertyName("EmpresaVenda.Logradouro").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Endereco + " [" + GlobalMessages.DadosCadastrais + "]"));
            RuleFor(emve => emve.Numero).NotEmpty().OverridePropertyName("EmpresaVenda.Numero").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Numero + " [" + GlobalMessages.DadosCadastrais + "]"));
            RuleFor(emve => emve.Bairro).NotEmpty().OverridePropertyName("EmpresaVenda.Bairro").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Bairro + " [" + GlobalMessages.DadosCadastrais + "]"));
            RuleFor(emve => emve.Cidade).NotEmpty().OverridePropertyName("EmpresaVenda.Cidade").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Cidade + " [" + GlobalMessages.DadosCadastrais + "]"));
            RuleFor(emve => emve.Estado).NotEmpty().OverridePropertyName("EmpresaVenda.Estado").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Estado + " [" + GlobalMessages.DadosCadastrais + "]"));

            RuleFor(emve => emve.NomeResponsavelTecnico).NotEmpty().OverridePropertyName("EmpresaVenda.NomeResponsavelTecnico").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Nome + " [" + GlobalMessages.ResponsavelTecnico + "]"));
            RuleFor(emve => emve.SituacaoResponsavel).NotEmpty().OverridePropertyName("EmpresaVenda.SituacaoResponsavel").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Situacao + " [" + GlobalMessages.ResponsavelTecnico + "]"));
            RuleFor(emve => emve.NumeroRegistroCRECI).NotEmpty().OverridePropertyName("EmpresaVenda.NumeroRegistroCRECI").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.NumeroRegistroCreci + " [" + GlobalMessages.ResponsavelTecnico + "]"));
            RuleFor(emve => emve.Categoria).NotEmpty().OverridePropertyName("EmpresaVenda.Categoria").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Categoria + " [" + GlobalMessages.ResponsavelTecnico + "]"));

            RuleFor(emve => emve.Simples).NotEmpty().OverridePropertyName("EmpresaVenda.Simples").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Simples + " [" + GlobalMessages.DadosTributarios + "]"));
            RuleFor(emve => emve.SIMEI).NotEmpty().OverridePropertyName("EmpresaVenda.SIMEI").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Simei + " [" + GlobalMessages.DadosTributarios + "]"));
            RuleFor(emve => emve.LucroPresumido).NotEmpty().OverridePropertyName("EmpresaVenda.LucroPresumido").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.LucroPresumido + " [" + GlobalMessages.DadosTributarios + "]"));
            RuleFor(emve => emve.LucroReal).NotEmpty().OverridePropertyName("EmpresaVenda.LucroReal").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.LucroReal + " [" + GlobalMessages.DadosTributarios + "]"));
        }

        public bool CheckIfExistsCNPJAtivoSuspensa(Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda)
        {
            if (empresaVenda.CNPJ.IsEmpty())
            {
                return false;
            }
            return _empresaVendaRepository.CheckIfExistsCNPJAtivaSuspensa(empresaVenda);
        }

        public bool CheckIfCNPJIsValid(Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda)
        {
            if (empresaVenda.CNPJ.IsEmpty())
            {
                return true;
            }
            return empresaVenda.CNPJ.OnlyNumber().IsValidCNPJ();
        }
        public bool CheckIfExistsCNPJPreCadastro(Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda)
        {
            if (empresaVenda.CNPJ.IsEmpty())
            {
                return false;
            }
            return _empresaVendaRepository.CheckIfExistsCNPJPreCadastro(empresaVenda);
        }
        public bool CheckIfExistsCNPJCancelada(Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda)
        {
            if (empresaVenda.CNPJ.IsEmpty())
            {
                return false;
            }
            return _empresaVendaRepository.CheckIfExistsCNPJCancelada(empresaVenda);
        }

    }
}
