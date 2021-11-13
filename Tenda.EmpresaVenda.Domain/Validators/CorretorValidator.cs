using Europa.Extensions;
using Europa.Resources;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Security.Enums;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class CorretorValidator : AbstractValidator<Corretor>
    {
        public CorretorRepository _corretorRepository { get; set; }
        public EmpresaVendaRepository _empresaVendaRepository { get; set; }

        public CorretorValidator(CorretorRepository corretorRepository)
        {
            _corretorRepository = corretorRepository;
            _empresaVendaRepository = new EmpresaVendaRepository();
            _empresaVendaRepository._session = _corretorRepository._session;

            RuleFor(corr => corr.CPF).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Cpf));
            RuleFor(corr => corr.CPF).Must(cpf => CheckIfCPFIsValid(cpf)).WithName("CPF").WithMessage(string.Format(GlobalMessages.InformeParametroValido, GlobalMessages.Cpf));
            RuleFor(corr => corr).Must(corr => CheckIfOtherCpfInEV(corr)).WithName("CPF").WithMessage(string.Format(GlobalMessages.MsgErroCorretorExistente));
            RuleFor(corr => corr.CNPJ).Must(cnpj => CheckIfCNPJIsValid(cnpj)).WithMessage(string.Format(GlobalMessages.InformeParametroValido, GlobalMessages.Cnpj));
            RuleFor(corr => corr.Nome).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.NomeCompleto));
            RuleFor(corr => corr.RG).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.RG));
            RuleFor(corr => corr.Email).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Email));
            RuleFor(corr => corr.Email).Must(email => CheckIfEmailIsValid(email)).WithName("Email").WithMessage(string.Format(GlobalMessages.InformeParametroValido, GlobalMessages.Email));
            RuleFor(corr => corr.Telefone).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Telefone));
            RuleFor(corr => corr.Creci).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.CreciJuridico));
            RuleFor(corr => corr.DataNascimento).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.DataNascimento));
            RuleFor(corr => corr.DataNascimento).Must(dtNascimento => CheckDataNascimento(dtNascimento)).WithMessage(GlobalMessages.DataNascimentoFuturo);
            RuleFor(corr => corr.DataCredenciamento).Must(dtCredenciamento => CheckDataCredenciamento(dtCredenciamento)).WithMessage(GlobalMessages.MsgDataCredenciamentoFuturo);
            RuleFor(corr => corr.DataCredenciamento).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.DataDeCredenciamento));
            RuleFor(corr => corr.Apelido).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.NomeGuerra));
            RuleFor(corr => corr).Must(x => CheckDiretor(x)).WithMessage("Apenas o representante legal poderá ter a função de diretor");
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

        public bool CheckDiretor(Corretor corretor)
        {
            var representante = _empresaVendaRepository.CheckIfCorretorIsDiretor(corretor.Id,corretor.EmpresaVenda.Id);
            
            var temRepresentante = _empresaVendaRepository.Queryable()
                .Where(x=>x.Id==corretor.EmpresaVenda.Id)
                .Where(x => x.Corretor != null)
                .Any();

            if(representante && corretor.Funcao != TipoFuncao.Diretor)
            {
                return false;
            }

            if(corretor.Id==0 && temRepresentante && corretor.Funcao == TipoFuncao.Diretor)
            {
                return false;
            }

            if(!representante && corretor.Id != 0 && corretor.Funcao == TipoFuncao.Diretor)
            {
                return false;
            }

            return true;
        }
    }
}
