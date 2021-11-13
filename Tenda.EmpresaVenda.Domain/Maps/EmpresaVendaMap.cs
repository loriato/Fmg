using Europa.Data.Model;
using FluentNHibernate.Mapping;
using NHibernate.Type;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class EmpresaVendaMap : ClassMap<Tenda.Domain.EmpresaVenda.Models.EmpresaVenda>
    {
        public EmpresaVendaMap()
        {
            Table("TBL_EMPRESAS_VENDAS");

            Id(reg => reg.Id).Column("ID_EMPRESA_VENDA").GeneratedBy.Sequence("SEQ_EMPRESAS_VENDAS");
            Map(x => x.CriadoPor).Column("ID_CRIADO_POR").Not.Update();
            Map(x => x.CriadoEm).Column("DT_CRIADO_EM").Not.Update().CustomType<DateTimeType>();
            Map(x => x.AtualizadoPor).Column("ID_ATUALIZADO_POR");
            Map(x => x.AtualizadoEm).Column("DT_ATUALIZADO_EM").CustomType<DateTimeType>();
            Map(reg => reg.CodigoFornecedor).Column("CD_FORNECEDOR_SAP").Nullable().Length(DatabaseStandardDefinitions.FiftyLength);
            Map(reg => reg.CentralVendas).Column("DS_CENTRAL_VENDA").Length(DatabaseStandardDefinitions.TwentyLength).Nullable();
            Map(reg => reg.RazaoSocial).Column("DS_RAZAO_SOCIAL").Length(DatabaseStandardDefinitions.TwoHundredFiftySixLength);
            Map(reg => reg.NomeFantasia).Column("NM_FANTASIA").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.CNPJ).Column("DS_CNPJ").Length(DatabaseStandardDefinitions.TwentyLength).Nullable();
            Map(reg => reg.InscricaoMunicipal).Column("DS_INSCRICAO_MUNICIPAL").Length(DatabaseStandardDefinitions.ThirtyLength).Nullable();
            Map(reg => reg.CreciJuridico).Column("DS_CRECI_JURIDICO").Length(DatabaseStandardDefinitions.FiftyLength).Nullable();
            Map(reg => reg.InscricaoEstadual).Column("DS_INSCRICAO_ESTADUAL").Length(DatabaseStandardDefinitions.ThirtyLength).Nullable();
            Map(reg => reg.LegislacaoFederal).Column("DS_LEGISLACAO_FEDERAL").Length(DatabaseStandardDefinitions.ThirtyLength).Nullable();
            Map(reg => reg.Simples).Column("DS_SIMPLES").Length(DatabaseStandardDefinitions.ThirtyLength).Nullable();
            Map(reg => reg.SIMEI).Column("DS_SIMEI").Length(DatabaseStandardDefinitions.ThirtyLength).Nullable();
            Map(reg => reg.LucroPresumido).Column("VL_LUCRO_PRESUMIDO").Nullable();
            Map(reg => reg.LucroReal).Column("VL_LUCRO_REAL").Nullable();
            Map(reg => reg.Situacao).Column("TP_SITUACAO").CustomType<EnumType<Situacao>>().Nullable(); 
            Map(reg => reg.Cep).Column("DS_CEP").Nullable().Length(DatabaseStandardDefinitions.CepLength);
            Map(reg => reg.Cidade).Column("DS_CIDADE").Nullable().Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.Estado).Column("DS_ESTADO").Nullable().Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.Pais).Column("DS_PAIS").Nullable().Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.Bairro).Column("DS_BAIRRO").Nullable().Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.Complemento).Column("DS_COMPLEMENTO").Nullable().Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.Numero).Column("DS_NUMERO").Nullable().Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.Logradouro).Column("DS_LOGRADOURO").Nullable().Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.NomeResponsavelTecnico).Column("NM_RESPONSAVEL_TECNICO").Nullable().Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.NumeroRegistroCRECI).Column("DS_REGISTRO_CRECI").Nullable().Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.SituacaoResponsavel).Column("DS_SITUACAO_RESPONSAVEL").Nullable().Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.Categoria).Column("DS_CATEGORIA").Nullable().Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength);
            Map(reg => reg.CorretorVisualizarClientes).Column("FL_CORRETOR_VISUALIZAR_CLIENTES").Nullable();
            Map(reg => reg.PessoaContato).Column("NM_PESSOA_CONTATO").Length(DatabaseStandardDefinitions.OneHundredTwentyEigthLength).Nullable();
            Map(reg => reg.TelefoneContato).Column("DS_TELEFONE_CONTATO").Nullable();
            Map(reg => reg.TipoEmpresaVenda).Column("TP_EMPRESA_VENDA").CustomType<EnumType<TipoEmpresaVenda>>();
            Map(reg => reg.ConsiderarUF).Column("TP_CONSIDERAR_UF").CustomType<EnumType<TipoSimNao>>();
            References(reg => reg.Loja).Column("ID_LOJA").ForeignKey("FK_EMPRESA_X_LOJA_01").Nullable();
            References(reg => reg.Corretor).Column("ID_CORRETOR").ForeignKey("FK_EMPRESA_X_CORRETOR_01").Nullable();
            References(reg => reg.FotoFachada).Column("ID_FOTO_FACHADA").ForeignKey("FK_EMPRESA_X_ARQUIVO_01").Nullable();
        }
    }
}
