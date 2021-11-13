using System.Collections.Generic;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Shared;

namespace Tenda.EmpresaVenda.Domain.Integration.Suat.Models
{
    public class PropostaDTO
    {
        public long Id { get; set; }
        public long IdSuat { get; set; }
        public long Unidade { get; set; }
        public long Midia { get; set; }
        public string Codigo { get; set; }
        public string Corretor { get; set; }
        public string Vendedor { get; set; }
        public string Proprietario { get; set; }
        public ClienteSuatDTO Cliente { get; set; }
        // A loja associada a Empresa de Venda
        public string CentralImobiliaria { get; set; }
        public bool? FatorSocial { get; set; }
        public bool? FaixaUmMeio { get; set; }
        public string ProtocoloGA { get; set; }
        public bool SicaqEnquadrado { get; set; }
        public List<PlanoPagamentoDTO> PlanosPagamento { get; set; }
        public List<ProponenteDTO> Proponentes { get; set; }
        public List<ClienteSuatDTO> Clientes { get; set; }
        public List<ClienteSuatDTO> Conjuges { get; set; }

        public string SituacaoSuat { get; set; }

        public PropostaDTO()
        {
        }

        public PropostaDTO(PreProposta preProposta, ClienteSuatDTO cliente, List<PlanoPagamento> planosPagamento, List<Proponente> proponentes)
        {
            Id = preProposta.IdSuat;
            IdSuat = preProposta.IdSuat;
            Midia = ProjectProperties.SuatMidiaProposta;
            Corretor = preProposta.PontoVenda.Viabilizador.Login;
            Vendedor = preProposta.PontoVenda.Viabilizador.Login;
            Proprietario = preProposta.PontoVenda.Viabilizador.Login;
            Cliente = cliente;
            CentralImobiliaria = preProposta.EmpresaVenda.Loja.SapId;
            FatorSocial = preProposta.FatorSocial.Value;
            FaixaUmMeio = preProposta.FaixaUmMeio.Value;
            ProtocoloGA = preProposta.Codigo;
            Unidade = preProposta.IdUnidadeSuat;
            PlanosPagamento = new List<PlanoPagamentoDTO>();
            foreach (var plano in planosPagamento)
            {
                PlanosPagamento.Add(new PlanoPagamentoDTO(plano));
            }
            Proponentes = new List<ProponenteDTO>();
            foreach (var prop in proponentes)
            {
                Proponentes.Add(new ProponenteDTO(prop));
            }
            Clientes = new List<ClienteSuatDTO>();
            Conjuges = new List<ClienteSuatDTO>();
            SituacaoSuat = preProposta.PassoAtualSuat;
        }
    }
}
