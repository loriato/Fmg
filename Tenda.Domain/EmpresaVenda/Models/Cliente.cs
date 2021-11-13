using Europa.Data.Model;
using System;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models
{

    public class Cliente : BaseEntity
    {
        public virtual TipoPessoa TipoPessoa { get; set; }
        public virtual string CpfCnpj { get; set; }
        public virtual string IdSap { get; set; }
        public virtual TipoSexo? TipoSexo { get; set; }
        public virtual string NomeCompleto { get; set; }
        public virtual DateTime? DataNascimento { get; set; }
        public virtual string TelefoneResidencial { get; set; }
        public virtual string TelefoneComercial { get; set; }
        public virtual string Email { get; set; }

        // Dados Pessoais
        public virtual int QuantidadeFilhos { get; set; }
        public virtual Profissao Profissao { get; set; }
        public virtual string Cargo { get; set; }
        public virtual TipoEscolaridade? Escolaridade { get; set; }
        public virtual TipoEstadoCivil? EstadoCivil { get; set; }
        public virtual TipoRegimeBens? RegimeBens { get; set; }
        public virtual TipoDocumentoEnum? TipoDocumento { get; set; }
        public virtual string NumeroDocumento { get; set; }
        public virtual string OrgaoEmissor { get; set; }
        public virtual string EstadoEmissor { get; set; }
        public virtual DateTime? DataEmissao { get; set; }
        public virtual string Nacionalidade { get; set; }
        public virtual string Naturalidade { get; set; }
        public virtual string Filiacao { get; set; }
        public virtual TipoResidencia? TipoResidencia { get; set; }
        public virtual decimal ValorAluguel { get; set; }
        public virtual string Empresa { get; set; }
        public virtual int? TempoDeEmpresa { get; set; }
        // Dados Financeiros
        public virtual TipoRenda? TipoRenda { get; set; }
        public virtual decimal RendaFormal { get; set; }
        public virtual string OrigemRendaFormal { get; set; }
        public virtual decimal RendaInformal { get; set; }
        public virtual string OrigemRendaInformal { get; set; }
        public virtual decimal RendaMensal { get; set; }
        public virtual decimal FGTS { get; set; }
        public virtual int? MesesFGTS { get; set; }
        public virtual bool PossuiFinanciamento { get; set; }
        public virtual decimal ValorFinanciamento { get; set; }
        public virtual string TipoFinanciamento { get; set; }
        public virtual DateTime? UltimaParcelaFinanciamento { get; set; }
        public virtual string PrimeiraReferencia { get; set; }
        public virtual string SegundaReferencia { get; set; }
        public virtual string TelefonePrimeiraReferencia { get; set; }
        public virtual string TelefoneSegundaReferencia { get; set; }
        public virtual string DescricaoReferencias { get; set; }

        // Informações Adicionais
        public virtual bool? PossuiVeiculo { get; set; }
        public virtual decimal ValorVeiculo { get; set; }
        public virtual bool? VeiculoFinanciado { get; set; }
        public virtual decimal ValorUltimaParcelaFinanciamentoVeiculo { get; set; }
        public virtual DateTime? DataUltimaParcelaFinanciamentoPaga { get; set; }
        public virtual bool? PossuiContaBanco { get; set; }
        public virtual Banco Banco { get; set; }
        public virtual decimal LimiteChequeEspecial { get; set; }
        public virtual bool? PossuiComprometimentoFinanceiro { get; set; }
        public virtual decimal ValorComprometimentoFinanceiro { get; set; }
        public virtual int PrestacoesVencer { get; set; }
        public virtual DateTime? DataUltimaPrestacaoPaga { get; set; }
        public virtual bool? PossuiCartaoCredito { get; set; }
        public virtual TipoBandeiraCartao? BandeiraCartaoCredito { get; set; }
        public virtual decimal LimiteCartaoCredito { get; set; }
        public virtual DateTime? DataAdmissao { get; set; }
        public virtual long Corretor { get; set; }
        public virtual string NomeCorretor { get; set; }
        public virtual DateTime? DataCriacao { get; set; }

        //Dados PJ
        public virtual string JuntaComercialEmpresa { get; set; }
        public virtual DateTime? DataRegistroEmpresa { get; set; }
        public virtual string NireEmpresa { get; set; }
        public virtual string RamoAtividadeEmpresa { get; set; }

        public virtual EmpresaVenda EmpresaVenda { get; set; }
        public virtual long IdSuat { get; set; }

        //Conecta
        public virtual string UuidLead { get; set; }
        public virtual string NomeLead { get; set; }
        public virtual string TelefoneLead { get; set; }
        public override string ChaveCandidata()
        {
            return NomeCompleto;
        }
    }
}
