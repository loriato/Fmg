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
    public class ViewFilaUnificadaMap:BaseClassMap<ViewFilaUnificada>
    {
        public ViewFilaUnificadaMap()
        {
            Table("VW_FILA_UNIFICADA");
            ReadOnly();

            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_VW_FILA_UNIFICADA");

            Map(reg => reg.IdProposta).Column("ID_PROPOSTA");
            Map(reg => reg.CodigoProposta).Column("CD_PROPOSTA");
            Map(reg => reg.StatusPreProposta).Column("TP_STATUS").CustomType<EnumType<StatusProposta>>();
            Map(reg => reg.IdAvalista).Column("ID_AVALISTA");
            Map(reg => reg.DataElaboracao).Column("DT_ELABORACAO");
            Map(reg => reg.Regional).Column("NM_REGIONAL");
            Map(reg => reg.IdCliente).Column("ID_CLIENTE");
            Map(reg => reg.NomeCliente).Column("NM_CLIENTE");
            Map(reg => reg.CpfCnpjCliente).Column("DS_CPF_CNPJ");
            Map(reg => reg.IdEmpreendimento).Column("ID_EMPREENDIMENTO");
            Map(reg => reg.NomeEmpreendimento).Column("NM_EMPREENDIMENTO");
            Map(reg => reg.IdTorre).Column("ID_TORRE");
            Map(reg => reg.NomeTorre).Column("NM_TORRE");
            Map(reg => reg.IdUnidade).Column("ID_UNIDADE");
            Map(reg => reg.IdSapUnidade).Column("ID_SAP_UNIDADE");
            Map(reg => reg.NomeUnidade).Column("NM_UNIDADE");
            Map(reg => reg.DescricaoUnidade).Column("DS_UNIDADE");
            Map(reg => reg.StatusProposta).Column("DS_STATUS");
            Map(reg => reg.DataStatus).Column("DT_STATUS");
            Map(reg => reg.IdVendendor).Column("ID_VENDEDOR");
            Map(reg => reg.NomeViabilizador).Column("NM_VENDEDOR");
            Map(reg => reg.IdEmpresaVenda).Column("ID_EMPRESA_VENDA");
            Map(reg => reg.NomeEmpresaVenda).Column("NM_EMPRESA_VENDA");
            Map(reg => reg.IdLoja).Column("ID_LOJA");
            Map(reg => reg.NomeLoja).Column("NM_LOJA");
            Map(reg => reg.IdResponsavelPasso).Column("ID_RESPONSAVEL_PASSO");
            Map(reg => reg.NomeResponsavelPasso).Column("NM_RESPONSAVEL_PASSO");
            Map(reg => reg.IdProprietario).Column("ID_PROPRIETARIO");
            Map(reg => reg.NomeProprietario).Column("NM_PROPRIETARIO");
            Map(reg => reg.Origem).Column("TP_ORIGEM_PROPOSTA").CustomType<EnumType<TipoOrigemProposta>>();
            Map(reg => reg.SituacaoDocumento).Column("TP_SITUACAO_DOCUMENTO").CustomType<EnumType<SituacaoAprovacaoDocumento>>();
            Map(reg => reg.IdNode).Column("ID_NODE").Nullable();
            Map(reg => reg.StatusSicaq).Column("TP_STATUS_SICAQ").CustomType<EnumType<StatusSicaq>>().Nullable();
            Map(reg => reg.IdSapLoja).Column("ID_SAP_LOJA");

        }
    }
}
