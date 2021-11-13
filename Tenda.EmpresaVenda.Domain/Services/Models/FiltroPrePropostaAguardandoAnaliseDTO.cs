using System;
using System.Collections.Generic;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.Security.Models;

namespace Tenda.EmpresaVenda.Domain.Services.Models
{
    public class FiltroPrePropostaAguardandoAnaliseDTO
    {
        public List<long> Regional { get; set; }

        public List<string> UF { get; set; }
        public string CodigoPreProposta { get; set; }
        public SituacaoProposta[] SituacoesPreProposta { get; set; }
        public List<SituacaoProposta> SituacoesParaVisualizacao { get; set; }
        public string BreveLancamento { get; set; }
        public string EmpresaVendas { get; set; }
        public DateTime? DataElaboracaoDe { get; set; }
        public DateTime? DataElaboracaoAte { get; set; }
        public string Cpf { get; set; }
        public string Cliente { get; set; }
        public DateTime? DataUltimoEnvioDe { get; set; }
        public DateTime? DataUltimoEnvioAte { get; set; }
        public string Viabilizador { get; set; }
        public long IdBreveLancamento { get; set; }
        public long IdEmpresaVenda { get; set; }
        public long IdViabilizador { get; set; }
        public bool AvalistaPendente { get; set; }
        public List<long> IdsEvs { get; set; }
       // public List<string> Regionais { get; set; }
        public List<Perfil> Perfis { get; set; }
        public List<long> IdPrePropostasTransferidas { get; set; }
        public List<long> IdPrePropostasRemovidas { get; set; }
        public List<SituacaoAvalista> SituacoesAvalista { get; set; }

    }
}
