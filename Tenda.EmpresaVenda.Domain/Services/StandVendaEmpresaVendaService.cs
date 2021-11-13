using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.Core.Models;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services.Models;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class StandVendaEmpresaVendaService : BaseService
    {
        private StandVendaEmpresaVendaRepository _standVendaEmpresaVendaRepository { get; set; }
        private PontoVendaService _pontoVendaService { get; set; }
        private StandVendaRepository _standVendaRepository { get; set; }
        private PontoVendaRepository _pontoVendaRepository { get; set; }
        public void OnJoinStandEmpresaVenda(StandVendaDTO standEmpresaVendaDTO)
        {            

            standEmpresaVendaDTO = CriarPontoVendaByStandVenda(standEmpresaVendaDTO); 

            if (standEmpresaVendaDTO.IdStandVendaEmpresaVenda.HasValue())
            {
                return;
            }

            var standEmpresaVenda = new StandVendaEmpresaVenda();

            standEmpresaVenda.PontoVenda = new PontoVenda { Id = standEmpresaVendaDTO.IdPontoVenda };
            standEmpresaVenda.EmpresaVenda = new Tenda.Domain.EmpresaVenda.Models.EmpresaVenda { Id = standEmpresaVendaDTO.IdEmpresaVenda };
            standEmpresaVenda.StandVenda = new StandVenda { Id = standEmpresaVendaDTO.IdStandVenda };

            _standVendaEmpresaVendaRepository.Save(standEmpresaVenda);

        }

        public StandVendaDTO CriarPontoVendaByStandVenda(StandVendaDTO standEmpresaVendaDTO)
        {
            if (standEmpresaVendaDTO.IdPontoVenda.HasValue())
            {
                var ponto = _pontoVendaRepository.FindById(standEmpresaVendaDTO.IdPontoVenda);

                if (standEmpresaVendaDTO.Situacao != Situacao.Ativo)
                {
                    ponto.Situacao = Situacao.Ativo;
                }
                else
                {
                    ponto.Situacao = Situacao.Suspenso;
                }

                _pontoVendaRepository.Save(ponto);

                standEmpresaVendaDTO.Situacao = ponto.Situacao;

                return standEmpresaVendaDTO;
            }

            var bre = new BusinessRuleException();

            var pontoVenda = new PontoVenda();
            pontoVenda.Nome = standEmpresaVendaDTO.NomeStandVenda;
            pontoVenda.Situacao = Situacao.Ativo;
            pontoVenda.EmpresaVenda = new Tenda.Domain.EmpresaVenda.Models.EmpresaVenda { Id = standEmpresaVendaDTO.IdEmpresaVenda };
            pontoVenda.IniciativaTenda = true;

            if (standEmpresaVendaDTO.IdGerente.HasValue())
            {
                pontoVenda.Gerente = new Corretor { Id = standEmpresaVendaDTO.IdGerente };
            }

            if (standEmpresaVendaDTO.IdViabilizador.IsEmpty())
            {
                standEmpresaVendaDTO.IdViabilizador = standEmpresaVendaDTO.IdUsuario;
            }

            pontoVenda.Viabilizador = new UsuarioPortal { Id = standEmpresaVendaDTO.IdViabilizador };

            _pontoVendaService.Salvar(pontoVenda, bre);

            standEmpresaVendaDTO.IdPontoVenda = pontoVenda.Id;
            standEmpresaVendaDTO.Situacao = pontoVenda.Situacao;

            return standEmpresaVendaDTO;
        }
    }
}
