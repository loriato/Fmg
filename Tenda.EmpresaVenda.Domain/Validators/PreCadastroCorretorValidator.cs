using Europa.Extensions;
using Europa.Resources;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Security.Enums;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    class PreCadastroCorretorValidator : AbstractValidator<Corretor>
    {
        public CorretorRepository _corretorRepository { get; set; }

        public PreCadastroCorretorValidator(CorretorRepository corretorRepository)
        {
            _corretorRepository = corretorRepository;

            RuleFor(corr => corr.CPF).NotEmpty().OverridePropertyName("Corretor.CPF").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Cpf + " [" + GlobalMessages.RepresentanteLegal+ "]"));
            RuleFor(corr => corr.CPF).Must(cpf => CheckIfCPFIsValid(cpf)).OverridePropertyName("Corretor.CPF").WithMessage(string.Format(GlobalMessages.InformeParametroValido, GlobalMessages.Cpf + " [" + GlobalMessages.RepresentanteLegal + "]"));
            RuleFor(corr => corr).Must(corr => CheckIfOtherCpfInEV(corr)).OverridePropertyName("Corretor.CPF").WithMessage(string.Format(GlobalMessages.MsgErroCorretorExistente));
            RuleFor(corr => corr.Nome).NotEmpty().OverridePropertyName("Corretor.Nome").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Nome + " [" + GlobalMessages.RepresentanteLegal + "]"));
            RuleFor(corr => corr.RG).NotEmpty().OverridePropertyName("Corretor.RG").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.RG + " [" + GlobalMessages.RepresentanteLegal + "]"));
            RuleFor(corr => corr.Email).NotEmpty().OverridePropertyName("Corretor.Email").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Email + " [" + GlobalMessages.RepresentanteLegal + "]"));
            RuleFor(corr => corr.Email).Must(email => CheckIfEmailIsValid(email)).OverridePropertyName("Corretor.Email").WithMessage(string.Format(GlobalMessages.InformeParametroValido, GlobalMessages.Email + " [" + GlobalMessages.RepresentanteLegal + "]"));
            RuleFor(corr => corr.Telefone).NotEmpty().OverridePropertyName("Corretor.Telefone").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Telefone + " [" + GlobalMessages.RepresentanteLegal + "]"));
            RuleFor(corr => corr.Creci).NotEmpty().OverridePropertyName("Corretor.Creci").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.CreciFisico + " [" + GlobalMessages.RepresentanteLegal + "]"));
            RuleFor(corr => corr.DataNascimento).NotEmpty().OverridePropertyName("Corretor.DataNascimento").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.DataNascimento + " [" + GlobalMessages.RepresentanteLegal + "]"));
            RuleFor(corr => corr.DataNascimento).Must(dtNascimento => CheckDataNascimento(dtNascimento)).OverridePropertyName("Corretor.DataNascimento").WithMessage(GlobalMessages.DataNascimentoFuturo);
            RuleFor(corr => corr.DataCredenciamento).Must(dtCredenciamento => CheckDataCredenciamento(dtCredenciamento)).OverridePropertyName("Corretor.DataCredenciamento").WithMessage(GlobalMessages.MsgDataCredenciamentoFuturo);
            RuleFor(corr => corr.DataCredenciamento).NotEmpty().OverridePropertyName("Corretor.DataCredenciamento").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.DataDeCredenciamento + " [" + GlobalMessages.RepresentanteLegal + "]"));
            RuleFor(corr => corr.Nacionalidade).NotEmpty().OverridePropertyName("Corretor.Nacionalidade").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Nacionalidade + " [" + GlobalMessages.RepresentanteLegal + "]"));


            RuleFor(corr => corr.Cep).NotEmpty().OverridePropertyName("Corretor.Cep").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.CEP + " [" + GlobalMessages.RepresentanteLegal + "]"));
            RuleFor(corr => corr.Logradouro).NotEmpty().OverridePropertyName("Corretor.Logradouro").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Endereco + " [" + GlobalMessages.RepresentanteLegal + "]"));
            RuleFor(corr => corr.Numero).NotEmpty().OverridePropertyName("Corretor.Numero").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Numero + " [" + GlobalMessages.RepresentanteLegal + "]"));
            RuleFor(corr => corr.Bairro).NotEmpty().OverridePropertyName("Corretor.Bairro").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Bairro + " [" + GlobalMessages.RepresentanteLegal + "]"));
            RuleFor(corr => corr.Cidade).NotEmpty().OverridePropertyName("Corretor.Cidade").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Cidade + " [" + GlobalMessages.RepresentanteLegal + "]"));
            RuleFor(corr => corr.Estado).NotEmpty().OverridePropertyName("Corretor.Estado").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Estado + " [" + GlobalMessages.RepresentanteLegal + "]"));
        }

        public bool CheckIfExistsCPF(Corretor corretor)
        {
            if (corretor.CPF.IsEmpty())
            {
                return false;
            }
            return _corretorRepository.CheckIfExistsCPF(corretor);
        }

        public bool CheckIfCPFIsValid(string cpf)
        {
            if (cpf.IsEmpty())
            {
                return true;
            }
            return cpf.OnlyNumber().IsValidCPF();
        }

        public bool CheckIfCNPJIsValid(string cnpj)
        {
            if (cnpj.IsEmpty())
            {
                return true;
            }
            return cnpj.OnlyNumber().IsValidCNPJ();
        }

        public bool CheckIfEmailIsValid(string email)
        {
            if (email.IsEmpty())
            {
                return true;
            }
            return email.IsValidEmail();
        }

        public bool CheckDataNascimento(DateTime dtNascimento)
        {
            if (dtNascimento.IsEmpty())
            {
                return true;
            }
            return dtNascimento.Date.CompareTo(DateTime.Today) <= 0;
        }

        public bool CheckDataCredenciamento(DateTime dtCredenciamento)
        {
            if (dtCredenciamento.IsEmpty())
            {
                return true;
            }
            return dtCredenciamento.Date.CompareTo(DateTime.Today) <= 0;
        }

        public bool CheckCpfActiveOrSuspendedBroker(Corretor corretor)
        {
            return _corretorRepository.CheckCpfActiveOrSuspendedBroker(corretor);
        }

        public bool CheckIfOtherCpfInEV(Corretor corretor)
        {
            if (corretor.CPF.IsEmpty())
            {
                return true;
            }
            if (corretor.Id == 0 && !CheckIfExistsCPF(corretor)) //create sales company
            {
                return true;
            }
            if (corretor.Id != 0 && !CheckIfExistsCPF(corretor)) //update
            {
                return true;
            }
            if (corretor.Id != 0 && CheckIfExistsCPF(corretor)) //update
            {
                var cpfExistenteUsuarioAtivoOuSuspenso = CheckCpfActiveOrSuspendedBroker(corretor);
                if (cpfExistenteUsuarioAtivoOuSuspenso == true)
                    return false;
                return true;
            }
            else //insert
            {
                var possoSalvarCorretor = true;
                List<Corretor> antigosCorretoresComMesmoCpf = _corretorRepository.ListarCorretoresCpf(corretor.CPF);
                foreach (var antigoCorretor in antigosCorretoresComMesmoCpf)
                {
                    //Posso ter Corretores sem usuario por causa do pre cadastro
                    if (!antigoCorretor.Usuario.IsEmpty())
                    {
                        if (antigoCorretor.Usuario.Situacao == SituacaoUsuario.Ativo || antigoCorretor.Usuario.Situacao == SituacaoUsuario.Suspenso)
                        {
                            possoSalvarCorretor = false;
                            break;
                        }
                    }
                }
                if (possoSalvarCorretor && !corretor.Usuario.IsEmpty())
                {
                    corretor.Usuario.Situacao = SituacaoUsuario.Ativo;
                }
                return possoSalvarCorretor;
            }
        }
    }
}
