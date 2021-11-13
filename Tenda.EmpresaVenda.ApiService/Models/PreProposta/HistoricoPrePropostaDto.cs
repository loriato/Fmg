using Europa.Extensions;
using System;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.ApiService.Models.PreProposta
{
    public class HistoricoPrePropostaDto
    {
        public long Id { get; set; }
        public long IdPreProposta { get; set; }
        public string CodigoPreProposta { get; set; }
        public DateTime Inicio { get; set; }
        public long IdResponsavelInicio { get; set; }
        public string NomeResponsavelInicio { get; set; }
        public SituacaoProposta SituacaoInicio { get; set; }
        public DateTime? Termino { get; set; }
        public long IdResponsavelTermino { get; set; }
        public string NomeResponsavelTermino { get; set; }
        public SituacaoProposta? SituacaoTermino { get; set; }
        public long IdAnterior { get; set; }
        public Situacao Situacao { get; set; }
        public string NomePerfilCCAInicial { get; set; }
        public string NomePerfilCCAFinal { get; set; }
        public string SituacaoInicioPortalHouse { get; set; }
        public string SituacaoTerminoPortalHouse { get; set; }

        public HistoricoPrePropostaDto FromDomain(ViewHistoricoPreProposta model)
        {
            Id = model.Id;
            IdPreProposta = model.IdPreProposta;
            CodigoPreProposta = model.CodigoPreProposta;
            Inicio = model.Inicio;
            IdResponsavelInicio = model.IdResponsavelInicio;
            NomeResponsavelInicio = model.NomeResponsavelInicio;
            SituacaoInicio = model.SituacaoInicio;
            Termino = model.Termino;
            IdResponsavelTermino = model.IdResponsavelTermino;
            NomeResponsavelTermino = model.NomeResponsavelTermino;
            SituacaoTermino = model.SituacaoTermino;
            IdAnterior = model.IdAnterior;
            Situacao = model.Situacao;
            NomePerfilCCAInicial = model.NomePerfilCCAInicial;
            NomePerfilCCAFinal = model.NomePerfilCCAFinal;
            SituacaoInicioPortalHouse = (model.SituacaoInicioPortalHouse.IsEmpty() ? model.SituacaoInicio.AsString() : model.SituacaoInicioPortalHouse) ;
            SituacaoTerminoPortalHouse = (model.SituacaoTerminoPortalHouse.IsEmpty() ? model.SituacaoTermino.AsString() : model.SituacaoTerminoPortalHouse);

            return this;
        }
    }
}
