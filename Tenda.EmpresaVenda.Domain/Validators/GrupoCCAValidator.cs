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
    public class GrupoCCAValidator:AbstractValidator<GrupoCCA>
    {
        private GrupoCCARepository _grupoCCARepository { get; set; }

        public GrupoCCAValidator(GrupoCCARepository grupoCCARepository)
        {
            _grupoCCARepository = grupoCCARepository;

            RuleFor(x => x.Descricao).NotEmpty().WithName("Descricao").WithMessage(string.Format(GlobalMessages.CampoObrigatorio,GlobalMessages.Descricao));
            RuleFor(x => x).Must(x => ValidarGrupo(x)).WithMessage(string.Format(GlobalMessages.RegistroExistenteComMesmoNome));
        }

        public bool ValidarGrupo(GrupoCCA grupo)
        {
            if (grupo.IsEmpty())
            {
                return true;
            }

            if (grupo.Descricao.IsEmpty())
            {
                return true;
            }

            return !_grupoCCARepository.ValidarGrupo(grupo.Id,grupo.Descricao);

        }
    }
}
