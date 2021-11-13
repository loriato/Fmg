using Europa.Data;
using Europa.Data.Model;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class EmpreendimentoMap : BaseClassMap<Empreendimento>
    {
        public EmpreendimentoMap()
        {
            Table("TBL_EMPREENDIMENTOS");
            Id(reg => reg.Id).Column("ID_EMPREENDIMENTO").GeneratedBy.Sequence("SEQ_EMPREENDIMENTOS");
            Map(reg => reg.Nome).Column("NM_EMPREENDIMENTO").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.IdSuat).Column("ID_SUAT");
            Map(reg => reg.Divisao).Column("CD_DIVISAO").Length(DatabaseStandardDefinitions.TwentyLength);
            Map(reg => reg.DisponivelParaVenda).Column("FL_DISPONIVEL_PARA_VENDA");
            Map(reg => reg.DisponivelCatalogo).Column("FL_DISPONIVEL_CATALOGO");
            Map(reg => reg.Informacoes).Column("DS_INFORMACOES").Nullable();
            Map(reg => reg.Regional).Column("DS_REGIONAL").Length(DatabaseStandardDefinitions.TwentyLength);
            Map(reg => reg.NomeEmpresa).Column("NM_EMPRESA").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength).Nullable();
            Map(reg => reg.CNPJ).Column("DS_CNPJ").Length(DatabaseStandardDefinitions.TwentyLength).Nullable();
            Map(reg => reg.RegistroIncorporacao).Column("DS_REGISTRO_INCORPORACAO").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength).Nullable();
            Map(reg => reg.DataLancamento).Column("DT_LANCAMENTO").Nullable();
            Map(reg => reg.PrevisaoEntrega).Column("DT_PREVISAO_ENTREGA").Nullable();
            Map(reg => reg.DataEntrega).Column("DT_ENTREGA").Nullable();
            Map(reg => reg.CodigoEmpresa).Column("CD_EMPRESA").Length(DatabaseStandardDefinitions.TenLength);
            Map(reg => reg.Mancha).Column("DS_MANCHA").Length(DatabaseStandardDefinitions.TwoHundredFiftySixLength).Nullable();
            Map(reg => reg.PriorizarRegraComissao).Column("FL_PRIORIZAR_REGRA_COMISSAO");
            Map(reg => reg.FichaTecnica).Column("DS_FICHA_TECNICA").Nullable();
            Map(reg => reg.ModalidadeComissao).Column("TP_MODALIDADE_COMISSAO").CustomType<EnumType<TipoModalidadeComissao>>().Nullable();
            Map(reg => reg.ModalidadeProgramaFidelidade).Column("TP_MODALIDADE_PROGRAMA_FIDELIDADE").CustomType<EnumType<TipoModalidadeProgramaFidelidade>>().Nullable();
            References(reg => reg.RegionalObjeto).Column("ID_REGIONAL").ForeignKey("FK_TBL_EMPREENDIMENTOS_X_TBL_REGIONAIS").Nullable();
        }
    }
}