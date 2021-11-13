using Europa.Data;
using Europa.Data.Model;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class AvalistaMap : BaseClassMap<Avalista>
    {
        public AvalistaMap()
        {
            Table("TBL_AVALISTA");

            Id(reg => reg.Id).Column("ID_AVALISTA").GeneratedBy.Sequence("SEQ_AVALISTAS");

            Map(reg => reg.Nome).Column("NM_AVALISTA").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength).Not.Nullable();
            Map(reg => reg.Email).Column("DS_EMAIL").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength).Nullable();
            Map(reg => reg.RG).Column("DS_RG").Length(DatabaseStandardDefinitions.CpfLength).Nullable();
            Map(reg => reg.OrgaoExpedicao).Column("DS_ORGAO_EXPEDICAO").Length(DatabaseStandardDefinitions.TwentyLength).Nullable();
            Map(reg => reg.CPF).Column("DS_CPF").Length(DatabaseStandardDefinitions.CpfLength).Not.Nullable();
            Map(reg => reg.Profissao).Column("DS_PROFISSAO").Length(DatabaseStandardDefinitions.FiftyLength).Nullable();
            Map(reg => reg.Empresa).Column("DS_EMPRESA").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength).Nullable();
            Map(reg => reg.TempoEmpresa).Column("VL_TEMPO_EMPRESA").Nullable();
            Map(reg => reg.RendaDeclarada).Column("VL_RENDA_DECLARADA").Nullable();
            Map(reg => reg.OutrasRendas).Column("VL_OUTRAS_RENDAS").Nullable();
            Map(reg => reg.Banco).Column("DS_BANCO").Length(DatabaseStandardDefinitions.FiftyLength).Nullable();
            Map(reg => reg.ContatoGerente).Column("DS_CONTATO_GERENTE").Length(DatabaseStandardDefinitions.CellphoneLength).Nullable();
            Map(reg => reg.ValorTotalBens).Column("VL_TOTAL_BENS").Nullable();
            Map(reg => reg.PossuiImovel).Column("FL_POSSUI_IMOVEL").Nullable();
            Map(reg => reg.PossuiVeiculo).Column("FL_POSSUI_VEICULO").Nullable();
            Map(reg => reg.SituacaoAvalista).Column("TP_SITUACAO").CustomType<EnumType<SituacaoAvalista>>().Nullable();
        }
    }
}
