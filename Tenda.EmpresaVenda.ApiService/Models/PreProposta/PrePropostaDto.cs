using Europa.Extensions;
using System;
using Tenda.Domain.Core.Models;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.ApiService.Models.BreveLancamento;
using Tenda.EmpresaVenda.ApiService.Models.Cliente;
using Tenda.EmpresaVenda.ApiService.Models.EmpresaVenda;
using Tenda.EmpresaVenda.ApiService.Models.EstadoCidade;
using Tenda.EmpresaVenda.ApiService.Models.PontoVenda;
using Tenda.EmpresaVenda.ApiService.Models.Util;

namespace Tenda.EmpresaVenda.ApiService.Models.PreProposta
{
    public class PrePropostaDto
    {
        public long Id { get; set; }
        public string Codigo { get; set; }

        /// <summary>
        /// O dono da pré-proposta. Mesmo conceito de vendedor do SUAT
        /// Ele pode ser alterado em alguns casos por Coordenadores e Gerentes da mesma EV
        /// </summary>
        public EntityDto Corretor { get; set; }
        /// <summary>
        /// Mesmo conceito do Agente de Vendas no SUAT, porém dentro do contexto da Empresa de Vendas
        /// Geralmente um Coordenador ou Gerente da EV. Não pode ser alterado.
        /// </summary>
        public EntityDto Elaborador { get; set; }
        public DadosClienteDto Cliente { get; set; }
        public EmpresaVendaDto EmpresaVenda { get; set; }
        public PontoVendaDto PontoVenda { get; set; }
        public BreveLancamentoDto BreveLancamento { get; set; }
        public EntityDto Viabilizador { get; set; }
        /// <summary>
        /// Status que vai representar o HardCoded da máquina de estados
        /// </summary>
        public SituacaoProposta? SituacaoProposta { get; set; }
        public DateTime? DataElaboracao { get; set; }
        public decimal Valor { get; set; }
        public decimal TotalDetalhamentoFinanceiro { get; set; }
        public decimal TotalItbiEmolumento { get; set; }
        public DateTime? NascimentoMaisVelho { get; set; }
        public decimal RendaBrutaFamiliar { get; set; }
        public decimal FgtsFamiliar { get; set; }
        public bool? ClienteCotista { get; set; }
        public bool? DocCompleta { get; set; }
        public bool? FatorSocial { get; set; }
        public bool? FaixaUmMeio { get; set; }
        public StatusSicaq StatusSicaq { get; set; }
        public DateTime? DataSicaq { get; set; }
        public long IdSuat { get; set; }
        public long IdUnidadeSuat { get; set; }
        public string IdentificadorUnidadeSuat { get; set; }
        public string Observacao { get; set; }
        public long IdOrigem { get; set; }
        public long IdTorre { get; set; }
        public string ObservacaoTorre { get; set; }
        public string NomeTorre { get; set; }
        public decimal ParcelaAprovada { get; set; }
        public decimal ParcelaAprovadaPrevio { get; set; }
        public decimal ParcelaSolicitada { get; set; }
        public string PassoAtualSuat { get; set; }
        public EntityDto Avalista { get; set; }
        public TipoOrigemCliente OrigemCliente { get; set; }
        public string UltimoCCA { get; set; }

        public bool IsBreveLancamento { get; set; }
        public EstadoCidadeDto Regiao { get; set; }
        public bool FaixaEv { get; set; }
        public bool? FaixaUmMeioPrevio { get; set; }
        public StatusSicaq StatusSicaqPrevio { get; set; }
        public DateTime? DataSicaqPrevio { get; set; }
        public string CpfIndicador { get; set; }
        public string NomeIndicador { get; set; }

        public PrePropostaDto FromDomain(Tenda.Domain.EmpresaVenda.Models.PreProposta model)
        {
            Id = model.Id;
            Codigo = model.Codigo;
            Corretor = new EntityDto(model.Corretor.Id, model.Corretor.Nome);
            Elaborador = model.Elaborador.HasValue() ? new EntityDto(model.Elaborador.Id, model.Elaborador.Nome) : null;
            Cliente = model.Cliente.HasValue() ? new DadosClienteDto().FromDomain(model.Cliente) : null;
            EmpresaVenda = new EmpresaVendaDto().FromDomain(model.EmpresaVenda);
            PontoVenda = model.PontoVenda.HasValue() ? new PontoVendaDto().FromDomain(model.PontoVenda) : null;
            BreveLancamento = model.BreveLancamento.HasValue() ? new BreveLancamentoDto().FromDomain(model.BreveLancamento) : null;
            Viabilizador = model.Viabilizador.HasValue() ? new EntityDto(model.Viabilizador.Id, model.Viabilizador.Nome) : null;
            SituacaoProposta = model.SituacaoProposta;
            DataElaboracao = model.DataElaboracao;
            Valor = model.Valor;
            TotalDetalhamentoFinanceiro = model.TotalDetalhamentoFinanceiro;
            TotalItbiEmolumento = model.TotalItbiEmolumento;
            NascimentoMaisVelho = model.NascimentoMaisVelho;
            RendaBrutaFamiliar = model.RendaBrutaFamiliar;
            FgtsFamiliar = model.FgtsFamiliar;
            ClienteCotista = model.ClienteCotista;
            DocCompleta = model.DocCompleta;
            FatorSocial = model.FatorSocial;
            FaixaUmMeio = model.FaixaUmMeio;
            StatusSicaq = model.StatusSicaq;
            DataSicaq = model.DataSicaq;
            IdSuat = model.IdSuat;
            IdUnidadeSuat = model.IdUnidadeSuat;
            IdentificadorUnidadeSuat = model.IdentificadorUnidadeSuat;
            Observacao = model.Observacao;
            IdOrigem = model.IdOrigem;
            IdTorre = model.IdTorre;
            ObservacaoTorre = model.ObservacaoTorre;
            NomeTorre = model.NomeTorre;
            ParcelaAprovada = model.ParcelaAprovada;
            ParcelaSolicitada = model.ParcelaSolicitada;
            PassoAtualSuat = model.PassoAtualSuat;
            Avalista = model.Avalista.HasValue() ? new EntityDto(model.Avalista.Id, model.Avalista.Nome) : null;
            OrigemCliente = model.OrigemCliente;
            UltimoCCA = model.UltimoCCA;
            //IsBreveLancamento = model.IsBreveLancamento;
            //Regiao = model.Regiao.HasValue() ? new EstadoCidadeDto().FromDomain(model.Regiao) : null;
            FaixaEv = model.FaixaEv;
            OrigemCliente = model.OrigemCliente;
            ParcelaAprovadaPrevio = model.ParcelaAprovadaPrevio;
            FaixaUmMeioPrevio = model.FaixaUmMeioPrevio;
            StatusSicaqPrevio = model.StatusSicaqPrevio;
            DataSicaqPrevio = model.DataSicaqPrevio;
            NomeIndicador = model.NomeIndicador;
            CpfIndicador = model.CpfIndicador;

            return this;
        }

        public Tenda.Domain.EmpresaVenda.Models.PreProposta ToDomain()
        {
            var model = new Tenda.Domain.EmpresaVenda.Models.PreProposta();
            model.Id = Id;
            model.Codigo = Codigo;
            model.Corretor = new Corretor { Id = Corretor.Id, Nome = Corretor.Nome };
            model.Elaborador = new Corretor { Id = Elaborador.Id, Nome = Elaborador.Nome };
            model.Cliente = Cliente.HasValue() ? Cliente.ToDomain() : null;
            model.EmpresaVenda = EmpresaVenda.ToDomain();
            model.PontoVenda = PontoVenda.HasValue() ? PontoVenda.ToDomain() : null;
            model.BreveLancamento = BreveLancamento.HasValue() ? BreveLancamento.ToDomain() : null;
            model.Viabilizador = Viabilizador.HasValue() ? new UsuarioPortal { Id = Viabilizador.Id, Nome = Viabilizador.Nome } : null;
            model.SituacaoProposta = SituacaoProposta;
            model.DataElaboracao = DataElaboracao;
            model.Valor = Valor;
            model.TotalDetalhamentoFinanceiro = TotalDetalhamentoFinanceiro;
            model.TotalItbiEmolumento = TotalItbiEmolumento;
            model.NascimentoMaisVelho = NascimentoMaisVelho;
            model.RendaBrutaFamiliar = RendaBrutaFamiliar;
            model.FgtsFamiliar = FgtsFamiliar;
            model.ClienteCotista = ClienteCotista;
            model.DocCompleta = DocCompleta;
            model.FatorSocial = FatorSocial;
            model.FaixaUmMeio = FaixaUmMeio;
            model.StatusSicaq = StatusSicaq;
            model.DataSicaq = DataSicaq;
            model.IdSuat = IdSuat;
            model.IdUnidadeSuat = IdUnidadeSuat;
            model.IdentificadorUnidadeSuat = IdentificadorUnidadeSuat;
            model.Observacao = Observacao;
            model.IdOrigem = IdOrigem;
            model.IdTorre = IdTorre;
            model.ObservacaoTorre = ObservacaoTorre;
            model.NomeTorre = NomeTorre;
            model.ParcelaAprovada = ParcelaAprovada;
            model.ParcelaSolicitada = ParcelaSolicitada;
            model.PassoAtualSuat = PassoAtualSuat;
            model.Avalista = Avalista.HasValue() ? new Tenda.Domain.EmpresaVenda.Models.Avalista { Id = Avalista.Id, Nome = Avalista.Nome } : null;
            model.OrigemCliente = model.OrigemCliente;
            model.UltimoCCA = model.UltimoCCA;
            //model.IsBreveLancamento = model.IsBreveLancamento;
            //model.Regiao = Regiao.HasValue() ? Regiao.ToDomain() : null;
            model.FaixaEv = FaixaEv;
            model.OrigemCliente = OrigemCliente;
            model.ParcelaAprovadaPrevio = ParcelaAprovadaPrevio;
            model.NomeIndicador = NomeIndicador;
            model.CpfIndicador = CpfIndicador;

            return model;
        }
    }
}
