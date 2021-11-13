using System;
using Europa.Extensions;
using Tenda.Domain.Core.Models;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Integration.Suat.Models;

namespace Tenda.EmpresaVenda.Domain.Integration
{
    public class ClienteSuatDTO
    {
        public long Id { get; set; }
        public long IdSuat { get; set; }
        public long IdConjuge { get; set; }
        public virtual TipoPessoa TipoPessoa { get; set; }
        public virtual string NomeCompleto { get; set; }
        public virtual string CpfCnpj { get; set; }
        public virtual TipoSexo TipoSexo { get; set; }
        public virtual string TelefoneResidencial { get; set; }
        public virtual string TelefoneComercial { get; set; }
        public virtual string EmailPrincipal { get; set; }
        public virtual Endereco Endereco { get; set; }

        // Dados Pesoais
        public virtual DateTime DataNascimento { get; set; }
        public virtual TipoEstadoCivil EstadoCivil { get; set; }
        public virtual TipoRegimeBens? RegimeBens { get; set; }
        public virtual int QuantidadeFilhos { get; set; }
        public virtual ProfissaoDTO Profissao { get; set; }
        public virtual string Cargo { get; set; }
        public virtual TipoEscolaridade Escolaridade { get; set; }
        public virtual TipoDocumentoEnum TipoDocumento { get; set; }
        public virtual string NumeroDocumento { get; set; }
        public virtual string OrgaoEmissor { get; set; }
        public virtual string EstadoEmissor { get; set; }
        public virtual DateTime? DataEmissao { get; set; }
        public virtual string Nacionalidade { get; set; }
        public virtual string Naturalidade { get; set; }
        public virtual string Filiacao { get; set; }
        public virtual TipoResidencia TipoResidencia { get; set; }
        public virtual decimal? ValorAluguel { get; set; }

        // Dados Profissionais
        public virtual string Empresa { get; set; }
        public virtual int? TempoDeEmpresa { get; set; }
        public virtual Endereco EnderecoEmpresa { get; set; }

        // Dados Financeiros
        public virtual TipoRenda TipoRenda { get; set; }
        public virtual decimal RendaFormal { get; set; }
        public virtual string OrigemRendaFormal { get; set; }
        public virtual decimal? RendaInformal { get; set; }
        public virtual string OrigemRendaInformal { get; set; }
        public virtual decimal RendaMensal { get; set; }
        public virtual decimal FGTS { get; set; }
        public virtual int? MesesFGTS { get; set; }
        public virtual bool PossuiFinanciamento { get; set; }
        public virtual decimal ValorFinanciamento { get; set; }
        public virtual string TipoFinanciamento { get; set; }
        public virtual DateTime? UltimaParcela { get; set; }

        // Informações Complementares
        public virtual bool PossuiVeiculo { get; set; }
        public virtual decimal ValorVeiculo { get; set; }
        public virtual bool PossuiContaBanco { get; set; }
        public virtual string Banco { get; set; }
        public virtual bool PossuiComprometimentoFinanceiro { get; set; }
        public virtual DateTime? DataUltimaPrestacaoPaga { get; set; }
        public virtual decimal ValorComprometimentoFinanceiro { get; set; }

        // Referências
        public virtual string PrimeiraReferencia { get; set; }
        public virtual string SegundaReferencia { get; set; }
        public virtual string TelefonePrimeiraReferencia { get; set; }
        public virtual string TelefoneSegundaReferencia { get; set; }
        public virtual string DescricaoReferencias { get; set; }

        public ClienteSuatDTO()
        {

        }

        public ClienteSuatDTO FromModel(Cliente cliente, Endereco enderecoCliente, Endereco enderecoEmpresa, long idConjuge = 0)
        {
            ClienteSuatDTO suat = new ClienteSuatDTO();
            suat.Id = cliente.Id;
            suat.IdSuat = cliente.IdSuat;
            suat.IdConjuge = idConjuge;
            suat.TipoPessoa = cliente.TipoPessoa;
            suat.NomeCompleto = cliente.NomeCompleto;
            suat.CpfCnpj = cliente.CpfCnpj.OnlyNumber();
            suat.TipoSexo = cliente.TipoSexo.HasValue ? cliente.TipoSexo.Value : 0;
            suat.TelefoneResidencial = cliente.TelefoneResidencial;
            suat.TelefoneComercial = cliente.TelefoneComercial;
            suat.EmailPrincipal = cliente.Email;
            suat.Endereco = enderecoCliente;


            // Dados Pesoais
            suat.DataNascimento = cliente.DataNascimento.HasValue ? cliente.DataNascimento.Value : new DateTime();
            suat.EstadoCivil = cliente.EstadoCivil.HasValue ? cliente.EstadoCivil.Value : 0;
            suat.RegimeBens = cliente.RegimeBens.HasValue ? cliente.RegimeBens.Value : 0;
            suat.QuantidadeFilhos = cliente.QuantidadeFilhos;
            suat.Profissao = cliente.Profissao == null ? null : new ProfissaoDTO(cliente.Profissao);
            suat.Cargo = cliente.Cargo;
            suat.Escolaridade = cliente.Escolaridade.HasValue ? cliente.Escolaridade.Value : 0;
            suat.TipoDocumento = cliente.TipoDocumento.HasValue ? cliente.TipoDocumento.Value : 0;
            suat.NumeroDocumento = cliente.NumeroDocumento;
            suat.OrgaoEmissor = cliente.OrgaoEmissor;
            suat.EstadoEmissor = cliente.EstadoEmissor;
            suat.DataEmissao = cliente.DataEmissao;
            suat.Nacionalidade = cliente.Nacionalidade;
            suat.Naturalidade = cliente.Naturalidade;
            suat.Filiacao = cliente.Filiacao;
            suat.TipoResidencia = cliente.TipoResidencia.HasValue ? cliente.TipoResidencia.Value : 0;
            suat.ValorAluguel = cliente.ValorAluguel;

            // Dados Profissionais
            suat.Empresa = cliente.Empresa;
            suat.TempoDeEmpresa = cliente.TempoDeEmpresa;
            // Endereço Empresa
            suat.EnderecoEmpresa = enderecoEmpresa;

            // Dados Financeiros
            suat.TipoRenda = cliente.TipoRenda.HasValue ? cliente.TipoRenda.Value : 0;
            suat.RendaFormal = cliente.RendaFormal;
            suat.OrigemRendaFormal = cliente.OrigemRendaFormal;
            suat.RendaInformal = cliente.RendaInformal;
            suat.OrigemRendaInformal = cliente.OrigemRendaInformal;
            suat.RendaMensal = cliente.RendaMensal;
            suat.FGTS = cliente.FGTS;
            suat.MesesFGTS = cliente.MesesFGTS;
            suat.PossuiFinanciamento = cliente.PossuiFinanciamento;
            suat.ValorFinanciamento = cliente.ValorFinanciamento;
            suat.TipoFinanciamento = cliente.TipoFinanciamento;
            suat.UltimaParcela = cliente.UltimaParcelaFinanciamento;

            // Informações Complementares
            suat.PossuiVeiculo = cliente.PossuiVeiculo.HasValue ? cliente.PossuiVeiculo.Value : false;
            suat.PossuiContaBanco = cliente.PossuiContaBanco.HasValue ? cliente.PossuiContaBanco.Value : false;
            suat.Banco = cliente.Banco?.Nome;
            suat.PossuiComprometimentoFinanceiro = cliente.PossuiComprometimentoFinanceiro.HasValue ? cliente.PossuiComprometimentoFinanceiro.Value : false;
            suat.ValorComprometimentoFinanceiro = cliente.ValorComprometimentoFinanceiro;
            suat.DataUltimaPrestacaoPaga = cliente.DataUltimaPrestacaoPaga;

            // Referências
            suat.PrimeiraReferencia = cliente.PrimeiraReferencia;
            suat.SegundaReferencia = cliente.SegundaReferencia;
            suat.TelefonePrimeiraReferencia = cliente.TelefonePrimeiraReferencia;
            suat.TelefoneSegundaReferencia = cliente.TelefoneSegundaReferencia;
            suat.DescricaoReferencias = cliente.DescricaoReferencias;

            return suat;
        }
    }
}
