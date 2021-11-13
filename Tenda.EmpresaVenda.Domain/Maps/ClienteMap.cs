using Europa.Data;
using Europa.Data.Model;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ClienteMap : BaseClassMap<Cliente>
    {
        public ClienteMap()
        {
            Table("TBL_CLIENTES");
            Id(reg => reg.Id).Column("ID_CLIENTE").GeneratedBy.Sequence("SEQ_CLIENTES");
            Map(reg => reg.TipoPessoa).Column("TP_PESSOA").CustomType<EnumType<TipoPessoa>>();
            Map(reg => reg.CpfCnpj).Column("DS_CPF_CNPJ").Length(DatabaseStandardDefinitions.CnpjLength).Nullable();
            Map(reg => reg.TipoSexo).Column("TP_SEXO").CustomType<TipoSexo>().Nullable();
            Map(reg => reg.NomeCompleto).Column("NM_CLIENTE").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.DataNascimento).Column("DT_NASCIMENTO").Nullable();
            Map(reg => reg.TelefoneResidencial).Column("DS_TELEFONE_RESIDENCIAL").Nullable();
            Map(reg => reg.TelefoneComercial).Column("DS_TELEFONE_COMERCIAL").Nullable();
            Map(reg => reg.Email).Column("DS_EMAIL").Nullable();

            // Dados Pessoais
            Map(reg => reg.QuantidadeFilhos).Column("NR_FILHOS");
            References(reg => reg.Profissao).Column("ID_PROFISSAO").ForeignKey("FK_CLIENTE_X_PROFISSAO_01").Nullable();
            Map(reg => reg.Cargo).Column("DS_CARGO").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength).Nullable();
            Map(reg => reg.Escolaridade).Column("TP_ESCOLARIDADE").CustomType<TipoEscolaridade>().Nullable();
            Map(reg => reg.EstadoCivil).Column("TP_ESTADO_CIVIL").CustomType<TipoEstadoCivil>().Nullable();
            Map(reg => reg.RegimeBens).Column("TP_REGIME_BENS").CustomType<TipoRegimeBens>().Nullable();
            Map(reg => reg.TipoDocumento).Column("TP_DOCUMENTO").CustomType<TipoDocumentoEnum>().Nullable();
            Map(reg => reg.NumeroDocumento).Column("NR_DOCUMENTO").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength).Nullable();
            Map(reg => reg.OrgaoEmissor).Column("DS_ORGAO_EMISSOR").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength).Nullable();
            Map(reg => reg.EstadoEmissor).Column("DS_ESTADO_EMISSOR").Length(DatabaseStandardDefinitions.TenLength).Nullable();
            Map(reg => reg.DataEmissao).Column("DT_EMISSAO").Nullable();
            Map(reg => reg.Nacionalidade).Column("DS_NACIONALIDADE").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength).Nullable();
            Map(reg => reg.Naturalidade).Column("DS_NATURALIDADE").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength).Nullable();
            Map(reg => reg.Filiacao).Column("DS_FILIACAO").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength).Nullable();
            Map(reg => reg.TipoResidencia).Column("TP_RESIDENCIA").CustomType<TipoResidencia>().Nullable();
            Map(reg => reg.ValorAluguel).Column("VL_ALUGUEL");
            Map(reg => reg.Empresa).Column("NM_EMPRESA").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength).Nullable();
            Map(reg => reg.TempoDeEmpresa).Column("NR_TEMPO_DE_EMPRESA").Nullable();

            // Dados Financeiros
            Map(reg => reg.TipoRenda).Column("TP_RENDA").CustomType<TipoRenda>().Nullable();
            Map(reg => reg.RendaFormal).Column("VL_RENDA_FORMAL").Nullable();
            Map(reg => reg.OrigemRendaFormal).Column("DS_ORIGEM_RENDA_FORMAL").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength).Nullable();
            Map(reg => reg.RendaInformal).Column("VL_RENDA_INFORMAL").Nullable();
            Map(reg => reg.OrigemRendaInformal).Column("DS_ORIGEM_RENDA_INFORMAL").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength).Nullable();
            Map(reg => reg.RendaMensal).Column("VL_RENDA_MENSAL").Nullable();
            Map(reg => reg.FGTS).Column("VL_FGTS").Nullable();
            Map(reg => reg.MesesFGTS).Column("VL_MESES_FGTS").Nullable();
            Map(reg => reg.PossuiFinanciamento).Column("FL_POSSUI_FINANCIAMENTO");
            Map(reg => reg.ValorFinanciamento).Column("VL_FINANCIAMENTO").Nullable();
            Map(reg => reg.TipoFinanciamento).Column("DS_TIPO_FINANCIAMENTO").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength).Nullable();
            Map(reg => reg.UltimaParcelaFinanciamento).Column("DT_ULTIMA_PARCELA_FINANCIAMENTO").CustomType<DateType>().Nullable();
            Map(reg => reg.PrimeiraReferencia).Column("DS_PRIMEIRA_REFERENCIA").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength).Nullable();
            Map(reg => reg.SegundaReferencia).Column("DS_SEGUNDA_REFERENCIA").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength).Nullable();
            Map(reg => reg.TelefonePrimeiraReferencia).Column("DS_TELEFONE_PRIMEIRA_REFERENCIA").Length(DatabaseStandardDefinitions.CellphoneLength).Nullable();
            Map(reg => reg.TelefoneSegundaReferencia).Column("DS_TELEFONE_SEGUNDA_REFERENCIA").Length(DatabaseStandardDefinitions.CellphoneLength).Nullable();
            Map(reg => reg.DescricaoReferencias).Column("DS_REFERENCIAS").Length(DatabaseStandardDefinitions.FourThousandLength).Nullable();

            // Informações Adicionais
            Map(reg => reg.PossuiVeiculo).Column("FL_POSSUI_VEICULO").Nullable();
            Map(reg => reg.ValorVeiculo).Column("VL_VEICULO").Default("0");
            Map(reg => reg.VeiculoFinanciado).Column("FL_VEICULO_FINANCIADO").Nullable();
            Map(reg => reg.ValorUltimaParcelaFinanciamentoVeiculo).Column("VL_ULTIMA_PARC_FINANC_VEICULO").Default("0");
            Map(reg => reg.DataUltimaParcelaFinanciamentoPaga).Column("DT_ULTIMA_PARC_FINANC_PAGA").Nullable();
            Map(reg => reg.PossuiContaBanco).Column("FL_POSSUI_CONTA_BANCO").Nullable();
            References(reg => reg.Banco).Column("ID_BANCO").ForeignKey("FK_CLIENTE_X_BANCO_01");
            Map(reg => reg.LimiteChequeEspecial).Column("VL_LIMITE_CHEQUE_ESPECIAL").Default("0");
            Map(reg => reg.PossuiComprometimentoFinanceiro).Column("FL_POSSUI_COMPRO_FINANCEIRO").Nullable();
            Map(reg => reg.ValorComprometimentoFinanceiro).Column("VL_COMPROMETIMENTO_FINANCEIRO").Default("0");
            Map(reg => reg.PrestacoesVencer).Column("NR_PRESTACOES_VENCER").Default("0");
            Map(reg => reg.DataUltimaPrestacaoPaga).Column("DT_ULTIMA_PRESTACAO_PAGA").Nullable();
            Map(reg => reg.PossuiCartaoCredito).Column("FL_POSSUI_CARTAO_CREDITO").Nullable();
            Map(reg => reg.BandeiraCartaoCredito).Column("TP_BANDEIRA_CARTAO_CREDITO").CustomType<TipoBandeiraCartao>().Nullable();
            Map(reg => reg.LimiteCartaoCredito).Column("VL_LIMITE_CARTAO_CREDITO").Default("0");
            Map(reg => reg.DataAdmissao).Column("DT_ADMISSAO").Nullable();
            Map(reg => reg.Corretor).Column("ID_CORRETOR").Nullable();
            Map(reg => reg.DataCriacao).Column("DT_CRIACAO").Nullable();

            //Dados PJ
            Map(reg => reg.JuntaComercialEmpresa).Column("DS_JUNTA_COMERCIAL_EMPRESA").Length(DatabaseStandardDefinitions.FiftyLength).Nullable();
            Map(reg => reg.DataRegistroEmpresa).Column("DT_REGISTRO").Nullable();
            Map(reg => reg.NireEmpresa).Column("DS_NIRE_EMPRESA").Length(DatabaseStandardDefinitions.FiftyLength).Nullable();
            Map(reg => reg.RamoAtividadeEmpresa).Column("DS_RAMO_ATIVIDADE_EMPRESA").Length(DatabaseStandardDefinitions.FiftyLength).Nullable();

            References(reg => reg.EmpresaVenda).Column("ID_EMPRESA_VENDA").ForeignKey("FK_CLIENTE_X_EMPRESA_VENDA_01").Not.Update();
            Map(reg => reg.IdSuat).Column("ID_SUAT");
            Map(reg => reg.IdSap).Column("DS_ID_SAP").Nullable().Length(DatabaseStandardDefinitions.TwentyLength);

            //Conecta
            Map(reg => reg.UuidLead).Column("CD_UUID_LEAD").Nullable();
            Map(reg => reg.NomeLead).Column("NM_LEAD").Nullable();
            Map(reg => reg.TelefoneLead).Column("DS_TELEFONE_LEAD").Nullable();

        }
    }
}
