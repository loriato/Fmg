using Europa.Commons;
using Europa.Extensions;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class LayoutProgramaFidelidadeService : BaseService
    {
        public LayoutProgramaFidelidadeRepository _layoutProgramaFidelidadeRepository { get; set; }
        public ArquivoService _arquivoService { get; set; }
        public void SalvarLayout(LayoutProgramaFidelidadeDTO layout)
        {
            var bre = new BusinessRuleException();
            var validate = new LayoutProgramaFidelidadeValidator().Validate(layout);
            bre.WithFluentValidation(validate);
            bre.ThrowIfHasError();

            if (layout.IdLayoutProgramaFidelidade.IsEmpty())
            {
                var existente = _layoutProgramaFidelidadeRepository.LayoutAtivo();

                if (existente.HasValue())
                {
                    existente.Situacao = Situacao.Cancelado;
                    _layoutProgramaFidelidadeRepository.Save(existente);
                }
            }

            var novo = new LayoutProgramaFidelidade();

            if (layout.IdLayoutProgramaFidelidade.HasValue())
            {
                novo = _layoutProgramaFidelidadeRepository.FindById(layout.IdLayoutProgramaFidelidade);
            }

            if (novo.BannerParceiroExclusivo.IsEmpty())
            {
                novo.BannerParceiroExclusivo = _arquivoService.CreateFile(layout.FileBannerParceiroExclusivo, novo.BannerParceiroExclusivo, null);
            }

            if (novo.BannerPontos.IsEmpty())
            {
                novo.BannerPontos = _arquivoService.CreateFile(layout.FileBannerPontos, novo.BannerPontos, null);
            }

            if (layout.IdLayoutProgramaFidelidade.HasValue())
            {
                if (layout.FileBannerParceiroExclusivo.HasValue())
                {
                    novo.BannerParceiroExclusivo = _arquivoService.CreateFile(layout.FileBannerParceiroExclusivo, novo.BannerParceiroExclusivo, null);
                }

                if (layout.FileBannerPontos.HasValue())
                {
                    novo.BannerPontos = _arquivoService.CreateFile(layout.FileBannerPontos, novo.BannerPontos, null);
                }
            }

            novo.NomeParceiroExclusivo = layout.NomeParceiroExclusivo;
            novo.LinkParceiroExclusivo = layout.LinkParceiroExclusivo;
            novo.Situacao = Situacao.Ativo;

            _layoutProgramaFidelidadeRepository.Save(novo);
        }
    }
}
