using Europa.Extensions;
using System;
using System.Security.Cryptography;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class PropostaFilaSUATService:BaseService
    {
        public PropostaFilaSUATRepository _propostaFilaSUATRepository { get; set; }

        public void AtualizarFilaUnificada(PropostaFilaSUAT proposta)
        {
            _session.Evict(proposta);
            proposta.Id = 0;

            proposta.CriadoPorSuat = proposta.CriadoPor;
            proposta.CriadoEmSuat = proposta.CriadoEm;
            proposta.AtualizadoPorSuat = proposta.AtualizadoPor;
            proposta.AtualizadoEmSuat = proposta.AtualizadoEm;                         

            var existe = _propostaFilaSUATRepository.FindByIdPropostaIdNode(proposta.IdProposta,proposta.IdNode);
            _session.Evict(existe);           

            if (!existe.IsEmpty())
            {
                proposta.Id = existe.Id;
                proposta.AtualizadoPor = ProjectProperties.IdUsuarioSistema;
                proposta.AtualizadoEm = DateTime.Now;
            }
            else
            {
                proposta.CriadoPor = ProjectProperties.IdUsuarioSistema;
                proposta.CriadoEm = DateTime.Now;
                proposta.AtualizadoPor = ProjectProperties.IdUsuarioSistema;
                proposta.AtualizadoEm = DateTime.Now;
            }

            _propostaFilaSUATRepository.Save(proposta);

        }
    }
}
