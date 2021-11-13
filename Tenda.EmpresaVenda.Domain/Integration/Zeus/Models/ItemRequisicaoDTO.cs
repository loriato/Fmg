using System;

namespace Tenda.EmpresaVenda.Domain.Integration.Zeus.Models
{
    public class ItemRequisicaoDTO
    {
        public string Numero { get; set; }

        public string TipoDocumento { get; set; }

        public int NumeroItem { get; set; }

        public string GrupoCompradores { get; set; }

        public string NomeRequisitante { get; set; }

        public DateTime DataSolicitacao { get; set; }

        public string TextoBreve { get; set; }

        public string NumeroMaterial { get; set; }

        public string CentroDeCusto { get; set; }

        public string GrupoMercadorias { get; set; }

        public int Quantidade { get; set; }

        public string UnidadeMedida { get; set; }

        public string TipoData { get; set; }

        public DateTime DataRemessaItem { get; set; }

        public DateTime DataLiberacao { get; set; }

        public double Preco { get; set; }

        public double PrecoUnidade { get; set; }

        public string CategoriaItemDocumentoCompra { get; set; }

        public string CategoriaClassificacaoContabil { get; set; }

        public string CodigoEntradaMercadorias { get; set; }

        public string CodigoEntradaFaturas { get; set; }

        public string FornecedorPretendido { get; set; }

        public string OrganizacaoDeCompras { get; set; }

        public string CodigoMoeda { get; set; }
        public string AreaContabilidadeCustos { get; set; }

        public int NumeroSeqSegmentoClasseContabil { get; set; }

        public string NumeroContaRazao { get; set; }
    }
}
