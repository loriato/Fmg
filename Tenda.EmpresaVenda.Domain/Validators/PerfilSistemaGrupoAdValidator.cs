using Europa.Extensions;
using Europa.Resources;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.Security.Models;
using Tenda.Domain.Security.Repository;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class PerfilSistemaGrupoAdValidator : AbstractValidator<PerfilSistemaGrupoActiveDirectory>
    {
        private PerfilSistemaGrupoActiveDiretoryRepository _perfilSistemaGrupoADRepository { get; set; }

        public PerfilSistemaGrupoAdValidator(PerfilSistemaGrupoActiveDiretoryRepository perfilSisGrupoAdRepo)
        {
            _perfilSistemaGrupoADRepository = perfilSisGrupoAdRepo;

            RuleFor(psgad => psgad.Perfil).NotEmpty().OverridePropertyName("NomePerfil").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Perfil));
            RuleFor(psgad => psgad.Sistema).NotEmpty().OverridePropertyName("Sistema").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Sistema));
            RuleFor(psgad => psgad.GrupoActiveDirectory).NotEmpty().OverridePropertyName("GrupoActiveDirectory").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.GrupoAd));

            RuleFor(psgad => psgad).Must(psgad => !VerificarUnicidade(psgad)).OverridePropertyName("GrupoActiveDirectory").WithMessage(string.Format(GlobalMessages.MsgErroRegistroExistente, GlobalMessages.AssociacoesPerfilSistemaGrupoAd, GlobalMessages.GrupoAd));
        }

        public bool VerificarUnicidade(PerfilSistemaGrupoActiveDirectory model)
        {
            if (model.GrupoActiveDirectory.IsEmpty()) return true;

            model.GrupoActiveDirectory = model.GrupoActiveDirectory.ToUpper();
            return _perfilSistemaGrupoADRepository.VerificarUnicidade(model.Id, model.GrupoActiveDirectory);
        }
    }
}
