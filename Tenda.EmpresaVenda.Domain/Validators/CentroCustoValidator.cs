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
    public class CentroCustoValidator : AbstractValidator<CentroCusto>
    {
        public CentroCustoRepository _centroCustoRepository { get; set; }

        public CentroCustoValidator(CentroCustoRepository centroCustoRepository)
        {
            _centroCustoRepository = centroCustoRepository;
            RuleFor(cod => cod.Descricao).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Descricao));
            RuleFor(cod => cod.Codigo).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Codigo));
            RuleFor(cod => cod).Must(cod => !CheckIfExistsTipoCusto(cod)).WithName("Codigo").WithMessage(string.Format(GlobalMessages.MsgErroRegistroInformada, GlobalMessages.CentroCusto, GlobalMessages.Codigo));
       
        }
        public bool CheckIfExistsTipoCusto(CentroCusto model)
        {
            if (model.Codigo.IsEmpty())
            {
                return false;
            }
            return _centroCustoRepository.CheckIfExistsCodigo(model);
        }
    }
}
