using Europa.Data;
using NHibernate.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.Domain.Security.Enums;

namespace Tenda.EmpresaVenda.Domain.Maps
{
    public class ViewPreEmpresaVendaMap : BaseClassMap<ViewPreEmpresaVenda>
    {
        public ViewPreEmpresaVendaMap()
        {
            Table("VW_PRE_EMPRESAS_VENDAS");
            ReadOnly();
            SchemaAction.None();

            Id(reg => reg.Id).Column("ID_EMPRESA_VENDA");

            Map(view => view.NomeFantasia).Column("NM_FANTASIA");
            Map(view => view.RazaoSocial).Column("DS_RAZAO_SOCIAL");
            Map(view => view.CNPJ).Column("DS_CNPJ");
            Map(view => view.CentralVenda).Column("DS_CENTRAL_VENDA"); // TODO: Será retirado
            Map(view => view.IdLoja).Column("ID_LOJA");
            Map(view => view.NomeLoja).Column("NM_LOJA");
            Map(view => view.CreciJuridico).Column("DS_CRECI_JURIDICO");
            Map(view => view.IdCorretor).Column("ID_CORRETOR");
            Map(view => view.NomeCorretor).Column("NM_CORRETOR");
            Map(view => view.Telefone).Column("DS_TELEFONE");
            Map(view => view.Email).Column("DS_EMAIL");
            Map(view => view.Situacao).Column("TP_SITUACAO").CustomType<EnumType<SituacaoUsuario>>();

            Map(view => view.CodigoFornecedorSap).Column("CD_FORNECEDOR_SAP");
            Map(view => view.InscricaoMunicipal).Column("DS_INSCRICAO_MUNICIPAL");
            Map(view => view.InscricaoEstadual).Column("DS_INSCRICAO_ESTADUAL");
            Map(view => view.LegislacaoFederal).Column("DS_LEGISLACAO_FEDERAL");
            Map(view => view.Simples).Column("DS_SIMPLES");
            Map(view => view.Simei).Column("DS_SIMEI");
            Map(view => view.CEP).Column("DS_CEP");
            Map(view => view.LucroPresumido).Column("VL_LUCRO_PRESUMIDO");
            Map(view => view.LucroReal).Column("VL_LUCRO_REAL");
            Map(view => view.Estado).Column("DS_ESTADO");
            Map(view => view.Cidade).Column("DS_CIDADE");
            Map(view => view.Complemento).Column("DS_COMPLEMENTO");
            Map(view => view.Bairro).Column("DS_BAIRRO");
            Map(view => view.Numero).Column("DS_NUMERO");
            Map(view => view.Endereco).Column("DS_LOGRADOURO");
            Map(view => view.RG).Column("DS_RG");
            Map(view => view.CreciFisico).Column("DS_CRECI");
            Map(view => view.CPF).Column("DS_CPF");
            Map(view => view.CnpjCorretor).Column("DS_CNPJ_CORRETOR");
            Map(view => view.DataNascimento).Column("DT_NASCIMENTO");
        }
    }
}
