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
    public class ClassificacaoValidator : AbstractValidator<Classificacao>
    {
        public ClassificacaoRepository _classificacaoRepository { get; set; }

        public ClassificacaoValidator(ClassificacaoRepository classificacaoRepository)
        {
            _classificacaoRepository = classificacaoRepository;
            RuleFor(pntv => pntv.Descricao).NotEmpty().WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Descricao));
            RuleFor(pntv => pntv).Must(pntv => !CheckIfExistsClassificao(pntv)).WithName("Descricao").WithMessage(string.Format(GlobalMessages.MsgErroRegistroInformada, GlobalMessages.Classificacao, GlobalMessages.Descricao));
        }
        public bool CheckIfExistsClassificao(Classificacao model)
        {
            if (model.Descricao.IsEmpty())
            {
                return false;
            }
            return _classificacaoRepository.CheckIfExistsClassificacao(model);
        }
    }
}
