using Europa.Resources;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebPages;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class TipoCustoValidator : AbstractValidator<TipoCusto>
    {
        public TipoCustoRepository _tipoCustoRepository { get; set; }

        public TipoCustoValidator(TipoCustoRepository tipoCustoRepository)
        {
            _tipoCustoRepository = tipoCustoRepository;
            RuleFor(tpc => tpc.Descricao).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Descricao));
            RuleFor(tpc => tpc).Must(tpc => !CheckIfExistsTipoCusto(tpc)).WithName("Descricao").WithMessage(string.Format(GlobalMessages.MsgErroRegistroInformada, GlobalMessages.TipoCusto, GlobalMessages.Descricao));
        }
        public bool CheckIfExistsTipoCusto(TipoCusto model)
        {
            if (model.Descricao.IsEmpty())
            {
                return false;
            }
            return _tipoCustoRepository.CheckIfExistsTipoCusto(model);
        }
    }
}
