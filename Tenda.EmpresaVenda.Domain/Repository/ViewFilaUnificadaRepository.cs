using Europa.Data;
using Europa.Extensions;
using Europa.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.Domain.Shared;
using Tenda.Domain.Shared.Models;
using Tenda.EmpresaVenda.Domain.Repository.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewFilaUnificadaRepository:NHibernateRepository<ViewFilaUnificada>
    {
        public LojaRepository _lojaRepository { get; set; }
        public EmpreendimentoRepository _empreendimentoRepository { get; set; }
        public DataSourceResponse<ViewFilaUnificada> Listar(DataSourceRequest request, FiltroFilaUnificadaDTO filtro)
        {
            var query = Queryable().Where(x => x.IdEmpresaVenda == null || filtro.IdsEvs.Contains(x.IdEmpresaVenda.Value));

            var situacao = new List<string>
            {
                "Aguardando Análise",
                "Doc. Em Aval. de Alçada",
                "Prop. Docs. Enviados"
            };

            query = query.Where(x => situacao.Contains(x.StatusProposta));

            if (filtro.Filas.HasValue())
            {
                var filas = ProjectProperties.FilasSUAT.Where(x => filtro.Filas.Contains(x.IdFila));
                
                foreach(var fila in filas)
                {
                    if (!fila.CodigoIdentificador.IsEmpty())
                    {
                        var filaPersonalizadaDTO = ProjectProperties.FiltrosDeFila(fila.CodigoIdentificador, 1);
                        if (!filaPersonalizadaDTO.IsNull())
                        {
                            query = AplicarFiltrosParametrizados(query, filaPersonalizadaDTO);
                        }                        
                    }
                    else
                    {
                        query = query.Where(x => filtro.IdsNodes.Contains(x.IdNode));
                    }
                }
            }

            if (!filtro.Regional.IsEmpty() && !filtro.Regional.Equals("Todos"))
            {
                query = query.Where(x => x.Regional.Contains(filtro.Regional));
            }
            if (!filtro.CodigoProposta.IsEmpty())
            {
                filtro.CodigoProposta = filtro.CodigoProposta.ToUpper();
                query = query.Where(x => x.CodigoProposta.Contains(filtro.CodigoProposta));
            }
            if (filtro.DataUltimoEnvioDe.HasValue)
            {
                query = query.Where(x => x.DataStatus.Value.Date >= filtro.DataUltimoEnvioDe.Value.Date);
            }
            if (filtro.DataUltimoEnvioAte.HasValue)
            {
                query = query.Where(x => x.DataStatus.Value.Date <= filtro.DataUltimoEnvioAte.Value.Date);
            }
            if (!filtro.NomeViabilizador.IsEmpty())
            {
                filtro.NomeViabilizador = filtro.NomeViabilizador.ToLower();
                query = query.Where(reg => reg.NomeViabilizador.ToLower().Contains(filtro.NomeViabilizador));
            }
            if (!filtro.NomeCliente.IsEmpty())
            {
                query = query.Where(x => x.NomeCliente.ToLower().Contains(filtro.NomeCliente.ToLower()));
            }
            if (!filtro.CpfCnpj.IsEmpty())
            {
                filtro.CpfCnpj = filtro.CpfCnpj.OnlyNumber();
                query = query.Where(x => x.CpfCnpjCliente.Contains(filtro.CpfCnpj));
            }
            if (!filtro.NomeEmpreendimento.IsEmpty())
            {
                query = query.Where(x => x.NomeEmpreendimento.ToLower().Contains(filtro.NomeEmpreendimento.ToLower()));
            }
            if (filtro.IdEmpresaVendas.HasValue())
            {
                query = query.Where(reg => reg.IdEmpresaVenda == filtro.IdEmpresaVendas);
            }            
            if (filtro.DataElaboracaoDe.HasValue)
            {
                query = query.Where(x => x.DataElaboracao.Value.Date >= filtro.DataElaboracaoDe.Value.Date);
            }
            if (filtro.DataElaboracaoAte.HasValue)
            {
                query = query.Where(x => x.DataElaboracao.Value.Date <= filtro.DataElaboracaoAte.Value.Date);
            }
            if (filtro.AvalistaPendente)
            {
                query = query
                    .Where(reg => reg.IdAvalista > 0)
                    .Where(reg => reg.SituacaoDocumento != SituacaoAprovacaoDocumento.Aprovado);
            }

            query = OrdenacaoEspecial(query);
            
            return query.ToDataRequest(request);
        }
        /*
         * Este método ordena a lista de modo que
         * a cada 3 propostas, apresentar 1 proposta
         * da fila "Análise Gerar Venda"
         * */
        private IQueryable<ViewFilaUnificada> OrdenacaoEspecial(IQueryable<ViewFilaUnificada> query)
        {
            var filtro = ProjectProperties.FiltrosDeFila(ProjectProperties.FilaDestaque, 1);
            var saida = new List<ViewFilaUnificada>();

            var destaques = AplicarFiltrosParametrizados(query, filtro);
            var lista = query.Where(x=>!destaques.Contains(x));

            while (lista.HasValue())
            {
                saida.AddRange(lista.Take(3));
                lista = lista.Where(x => !saida.Contains(x));

                saida.AddRange(destaques.Take(1));
                destaques = destaques.Where(x => !saida.Contains(x));
            }

            if (destaques.HasValue())
            {
                saida.AddRange(destaques);
            }
            
            return saida.AsQueryable();
        }

        private IQueryable<ViewFilaUnificada> AplicarFiltrosParametrizados(IQueryable<ViewFilaUnificada> queryBase, FilaPersonalizadaDTO dtoFilaPersonalizada)
        {
            if (!dtoFilaPersonalizada.LojasIn.IsEmpty())
            {
                queryBase = queryBase.Where(x => dtoFilaPersonalizada.LojasIn.Contains(x.IdSapLoja));
            }
            //LojasNotIn
            if (!dtoFilaPersonalizada.LojasNotIn.IsEmpty())
            {
                queryBase = queryBase.Where(x => !dtoFilaPersonalizada.LojasNotIn.Contains(x.IdSapLoja));
            }
            //EmpreendimentosIn
            if (!dtoFilaPersonalizada.EmpreendimentosIn.IsEmpty())
            {
                var identificadorEmpreendimentos = _empreendimentoRepository.Queryable()
                    .Where(x => dtoFilaPersonalizada.EmpreendimentosIn.Contains(x.Divisao))
                    .Select(x => x.IdSuat).ToList();
                queryBase = queryBase.Where(x => identificadorEmpreendimentos.Contains(x.IdEmpreendimento));
            }
            //EmpreendimentosNotIn
            if (!dtoFilaPersonalizada.EmpreendimentosNotIn.IsEmpty())
            {
                var identificadorEmpreendimentos = _empreendimentoRepository.Queryable()
                   .Where(x => dtoFilaPersonalizada.EmpreendimentosNotIn.Contains(x.Divisao))
                   .Select(x => x.IdSuat).ToList();
                queryBase = queryBase.Where(x => !identificadorEmpreendimentos.Contains(x.IdEmpreendimento));
            }
            //EstadosIn
            if (!dtoFilaPersonalizada.EstadosIn.IsEmpty())
            {
                queryBase = queryBase.Where(x => dtoFilaPersonalizada.EstadosIn.Contains(x.Regional));
            }
            //EstadosNotIn
            if (!dtoFilaPersonalizada.EstadosNotIn.IsEmpty())
            {
                queryBase = queryBase.Where(x => !dtoFilaPersonalizada.EstadosNotIn.Contains(x.Regional));
            }
            //EstadosIn
            if (!dtoFilaPersonalizada.RegionaisIn.IsEmpty())
            {
                queryBase = queryBase.Where(x => dtoFilaPersonalizada.RegionaisIn.Contains(x.Regional));
            }
            //EstadosNotIn
            if (!dtoFilaPersonalizada.RegionaisNotIn.IsEmpty())
            {
                queryBase = queryBase.Where(x => !dtoFilaPersonalizada.RegionaisNotIn.Contains(x.Regional));
            }
            //NodesIn
            if (!dtoFilaPersonalizada.NodesIn.IsEmpty())
            {
                queryBase = queryBase.Where(x => dtoFilaPersonalizada.NodesIn.Contains(x.IdNode));
            }
            //NodesIn
            if (!dtoFilaPersonalizada.StatusSicaqIn.IsEmpty())
            {
                queryBase = queryBase.Where(x => dtoFilaPersonalizada.StatusSicaqIn.Contains(x.StatusSicaq));
            }

            return queryBase;
        }

        public byte[] Exportar(DataSourceRequest request, FiltroFilaUnificadaDTO filtro)
        {
            var results = Listar(request, filtro);

            ExcelUtil excel = ExcelUtil.NewInstance(25)
                .NewSheet(DateTime.Now.ToString("yyyyMMddHHmmss"))
                .WithHeader(GetHeader());

            foreach (var model in results.records.ToList())
            {
                excel
                    .CreateCellValue(model.CodigoProposta).Width(40)
                    .CreateCellValue(model.Regional).Width(15)
                    .CreateCellValue(model.StatusProposta).Width(40)
                    .CreateCellValue(model.DataStatus.HasValue ? model.DataStatus.Value.ToString() : "").Width(40)
                    .CreateCellValue(model.DataElaboracao.HasValue ? model.DataElaboracao.Value.ToString() : "").Width(40)
                    .CreateCellValue(model.NomeCliente).Width(40)
                    .CreateCellValue(model.CpfCnpjCliente.ToCPFFormat()).Width(30)
                    .CreateCellValue(model.NomeEmpresaVenda).Width(40)
                    .CreateCellValue(model.NomeLoja).Width(40)
                    .CreateCellValue(model.NomeEmpreendimento).Width(40)
                    .CreateCellValue(model.NomeTorre).Width(40)
                    .CreateCellValue(model.NomeUnidade).Width(40)
                    .CreateCellValue(model.NomeProprietario).Width(40)
                    .CreateCellValue(model.NomeResponsavelPasso).Width(40)
                    .CreateCellValue(model.NomeViabilizador).Width(40);
            }
            excel.Close();
            return excel.DownloadFile();
        }

        private string[] GetHeader()
        {
            IList<string> header = new List<string>
            {
                GlobalMessages.Codigo,
                GlobalMessages.Regional,
                GlobalMessages.Situacao,
                GlobalMessages.DataHoraUltimoEnvio,
                GlobalMessages.DataDaElaboracao,
                GlobalMessages.Cliente,
                GlobalMessages.Cpf,
                GlobalMessages.EmpresaVenda,
                GlobalMessages.Loja,
                GlobalMessages.Empreendimento,
                GlobalMessages.Torre,
                GlobalMessages.Unidade,
                GlobalMessages.Proprietario,
                GlobalMessages.ResponsavelPasso,
                GlobalMessages.Viabilizador
            };
            return header.ToArray();
        }

        private string[] GetHeaderOperador()
        {
            IList<string> header = new List<string>
            {
                GlobalMessages.Regional,
                GlobalMessages.DataHoraUltimoEnvio,
                GlobalMessages.DataDaElaboracao,
                GlobalMessages.Cliente,
                GlobalMessages.Cpf,
                GlobalMessages.Empreendimento,
                GlobalMessages.Torre,
                GlobalMessages.Unidade
            };
            return header.ToArray();
        }

        public byte[] ExportarOperador(DataSourceRequest request, FiltroFilaUnificadaDTO filtro)
        {
            var results = Listar(request, filtro);

            ExcelUtil excel = ExcelUtil.NewInstance(25)
                .NewSheet(DateTime.Now.ToString("yyyyMMddHHmmss"))
                .WithHeader(GetHeaderOperador());

            foreach (var model in results.records.ToList())
            {
                excel
                    .CreateCellValue(model.Regional).Width(15)
                    .CreateCellValue(model.DataStatus.HasValue ? model.DataStatus.Value.ToString() : "").Width(40)
                    .CreateCellValue(model.DataElaboracao.HasValue ? model.DataElaboracao.Value.ToString() : "").Width(40)
                    .CreateCellValue(model.NomeCliente).Width(40)
                    .CreateCellValue(model.CpfCnpjCliente.ToCPFFormat()).Width(30)
                    .CreateCellValue(model.NomeEmpreendimento).Width(40)
                    .CreateCellValue(model.NomeTorre).Width(40)
                    .CreateCellValue(model.NomeUnidade).Width(40);
            }
            excel.Close();
            return excel.DownloadFile();
        }
    }
}
