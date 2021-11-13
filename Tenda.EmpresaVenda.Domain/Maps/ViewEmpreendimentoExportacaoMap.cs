using Europa.Data;
using NHibernate.Type;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewEmpreendimentoExportacaoMap : BaseClassMap<ViewEmpreendimentoExportacao>
    {
        public ViewEmpreendimentoExportacaoMap()
        {
            Table("VW_EMPREENDIMENTOS_EXPORTACAO");
            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_EMPREENDIMENTO");
            Map(reg => reg.Nome).Column("NM_EMPREENDIMENTO");
            Map(reg => reg.Divisao).Column("CD_DIVISAO");
            Map(reg => reg.Regional).Column("DS_REGIONAL");
            Map(reg => reg.NomeEmpresa).Column("NM_EMPRESA");
            Map(reg => reg.CNPJ).Column("DS_CNPJ");
            Map(reg => reg.RegistroIncorporacao).Column("DS_REGISTRO_INCORPORACAO");
            Map(reg => reg.DataLancamento).Column("DT_LANCAMENTO");
            Map(reg => reg.PrevisaoEntrega).Column("DT_PREVISAO_ENTREGA");
            Map(reg => reg.DataEntrega).Column("DT_ENTREGA");
            Map(reg => reg.CodigoEmpresa).Column("CD_EMPRESA");
            Map(reg => reg.Mancha).Column("DS_MANCHA");
            Map(reg => reg.DisponivelVenda).Column("FL_DISPONIVEL_PARA_VENDA");
            Map(reg => reg.DisponibilizarCatalogo).Column("FL_DISPONIVEL_CATALOGO");
            
            Map(reg => reg.Cep).Column("DS_CEP");
            Map(reg => reg.Logradouro).Column("DS_LOGRADOURO");
            Map(reg => reg.Numero).Column("DS_NUMERO");
            Map(reg => reg.Complemento).Column("DS_COMPLEMENTO");
            Map(reg => reg.Bairro).Column("DS_BAIRRO");
            Map(reg => reg.Cidade).Column("DS_CIDADE");
            Map(reg => reg.Estado).Column("DS_ESTADO");
            Map(reg => reg.IdRegional).Column("ID_REGIONAL");
        }
    }
}
