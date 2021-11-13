using Europa.Extensions;
using Europa.Resources;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.EmpresaVenda.ApiService.Models.Lead;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class LeadApiValidator: AbstractValidator<LeadDto>
    {
        private LojaRepository _lojaRepository { get; set; }
        private EmpresaVendaRepository _empresaVendaRepository { get; set; }
        public LeadApiValidator()
        {
            RuleFor(x => x.IdSapCentralImobiliaria).NotEmpty()
                .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.IdSapCentralImobiliaria));
            RuleFor(x => x.IdSapCentralImobiliaria)
                .Must(ExisteLoja)
                .WithMessage(x => string.Format("Loja não Integrada - IDSAP: {0}", x.IdSapCentralImobiliaria));
            RuleFor(x => x.IdSapCentralImobiliaria)
                .Must(ExisteEmpresaVenda)
                .WithMessage(x=>string.Format("Loja - IDSAP: {0} não possui Empresa de Venda no Portal EV", x.IdSapCentralImobiliaria));
            
        }

        public bool ExisteLoja(string idSapCentralImobiliaria)
        {
            if (idSapCentralImobiliaria.IsEmpty())
            {
                return true;
            }

            var loja = _lojaRepository.FindByIdSap(idSapCentralImobiliaria);
            if (loja.IsEmpty())
            {
                return false;
            }
            
            return true;
        }

        public bool ExisteEmpresaVenda(string idSapCentralImobiliaria)
        {
            if (idSapCentralImobiliaria.IsEmpty())
            {
                return true;
            }

            var empresaVenda = _empresaVendaRepository.FindByIdSapLoja(idSapCentralImobiliaria);
            // EV
            if (empresaVenda.IsEmpty())
            {
                return false;
            }

            return true;

        }
    }
}
