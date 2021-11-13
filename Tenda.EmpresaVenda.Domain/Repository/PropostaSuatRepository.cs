using Europa.Data;
using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class PropostaSuatRepository : NHibernateRepository<PropostaSuat>
    {
        public ItemRegraComissaoRepository _itemRegraComissaoRepository { get; set; }
        public LojaRepository _lojaRepository { get; set; }
        public EmpresaVendaRepository _empresaVendaRepository { get; set; }
        public PagamentoRepository _pagamentoRepository { get; set; }
        public PropostaSuatRepository()
        {
        }
        public PropostaSuat FindByIdSuat(long idSuat)
        {
            return Queryable().SingleOrDefault(reg => reg.IdSuat == idSuat);
        }

        public PropostaSuat FindByCodigoProposta(string codigoProposta)
        {
            return Queryable().SingleOrDefault(x => x.CodigoProposta == codigoProposta);
        }

        public PropostaSuat FindByIdPreProposta(long idPreProposta)
        {
            return Queryable().Where(x => x.PreProposta.Id == idPreProposta).FirstOrDefault();
        }

        public List<PropostaSuat> BuscarPropostasParaConsolidar(DateTime dataCorte)
        {
            var status = new List<string>() { "KIT COMPLETO", "KIT ENVIADO", "KIT PENDENTE", "KIT REENVIADO", "VENDA GERADA", "AGUARDANDO ASSINATURA DIGITAL", "MARCADA PARA ASSINATURA DIGITAL", "PROP. CANCELADA" };

            var query = Queryable()
                .Where(x => x.IdSapEmpreendimento != null)
                .Where(x => x.IdSapLoja != null)
                .Where(x => x.DataVenda != null);

            var atualizado = query.Where(x => x.AtualizadoEm >= dataCorte).ToList();
            var passo = query.Where(x => x.DataPassoAtual != null)
                .Where(x => x.DataPassoAtual.Value >= dataCorte).ToList();

            var uniao = atualizado.Union(passo);

            var listByStatus = uniao.Where(x => status.Contains(x.PassoAtual.ToUpper())).ToList();
            var listByKitCompleto = uniao.Where(x => x.KitCompleto).ToList();

            var propostas = listByStatus.Union(listByKitCompleto).OrderBy(x => x.AtualizadoEm).ToList();

            //Busca propostas com novos pagamentos
            var novosPagamentos = _pagamentoRepository.Queryable()
                .Where(x => x.AtualizadoEm >= dataCorte)
                .Select(x => x.Proposta)
                .Distinct()
                .ToList();

            var result = propostas.Union(novosPagamentos).OrderBy(x => x.AtualizadoEm).ToList();

            return result;
        }

        public long NumeroDePropostaKitCompletoNoPeriodoPorEmpreendimento(DateTime inicio,DateTime? fim,List<string> divisao,string idSapLoja)
        {
            var query = Queryable()
                    .Where(x => x.KitCompleto)
                    .Where(x => divisao.Contains(x.DivisaoEmpreendimento))
                    .Where(x=>x.IdSapLoja.ToUpper().Equals(idSapLoja.ToUpper()))
                    .Where(x => x.DataKitCompleto >= inicio);

            if (fim.HasValue())
            {
                query = query.Where(x => x.DataKitCompleto <= fim);
            }
                    
            return query.ToList().Count;
        }

        /**
         * Lista as propostas kit completo a partir de uma data de corte
         * */
        public List<PropostaSuat> PropostasKitCompletoDataCorte(DateTime? dataCorte)
        {
            var query = Queryable()
                .Where(x => x.KitCompleto);

            if (dataCorte.HasValue())
            {
                query = query.Where(x => x.DataKitCompleto != null)
                    .Where(x => x.DataKitCompleto.Value.Date >= dataCorte.Value.Date);
            }

            return query.ToList();
        }
    }
}
