using Europa.Data;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewCargarelatorioRepasseJunixMap : BaseClassMap<ViewCargarelatorioRepasseJunix>
    {
        public ViewCargarelatorioRepasseJunixMap()
        {
            Table("VW_CARGA_RELATORIO_REPASSE_JUNIX");
            ReadOnly();
            SchemaAction.None();

            Id(reg=> reg.Id).Column("ID_IMPORTACAO_JUNIX");
            Map(reg => reg.Situacao).Column("TP_SITUACAO").CustomType<EnumType<SituacaoArquivo>>();
            Map(reg => reg.Origem).Column("TP_ORIGEM").CustomType<EnumType<TipoOrigem>>();
            Map(reg => reg.DataInicio).Column("DT_INICIO_IMPORTACAO");
            Map(reg => reg.DataFim).Column("DT_FIM_IMPORTACAO");
            Map(reg => reg.IdArquivo).Column("ID_ARQUIVO");
            Map(reg => reg.NomeArquivo).Column("NM_ARQUIVO");
            Map(reg => reg.IdExecucao).Column("ID_EXECUCAO");
            Map(reg => reg.NomeUsuario).Column("NM_USUARIO");
            Map(reg => reg.RegistroAtual).Column("NR_REGISTRO_ATUAL");
            Map(reg => reg.TotalRegistros).Column("QT_TOTAL_REGISTROS");

        }
    }
}
