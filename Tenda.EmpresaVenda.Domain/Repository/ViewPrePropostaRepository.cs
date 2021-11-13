using Europa.Data;
using Europa.Extensions;
using Europa.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.ApiService.Models.PreProposta;
using Tenda.EmpresaVenda.Domain.Services.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewPrePropostaRepository : NHibernateRepository<ViewPreProposta>
    {
        public CorretorRepository _corretorRepository { get; set; }
        public DataSourceResponse<ViewPreProposta> Listar(DataSourceRequest request, ConsultaPrePropostaDto dto, long? idCorretorVisualizador)
        {
            var query = Queryable();

            if (dto.Faturado.HasValue())
            {
                var faturado = dto.Faturado == 1;

                query = query.Where(x => x.Faturado==faturado);
            }

            if (dto.DataFaturadoDe.HasValue())
            {
                query = query.Where(x => x.DataFaturado != null)
                    .Where(x => x.DataFaturado.Value.Date >= dto.DataFaturadoDe.Value.Date);
            }

            if (dto.DataFaturadoAte.HasValue())
            {
                query = query.Where(x => x.DataFaturado != null)
                    .Where(x => x.DataFaturado.Value.Date <= dto.DataFaturadoAte.Value.Date);
            }

            if (dto.IsSupervisor || dto.IsCoordenador)
            {
                query = query.Where(x => dto.IdsViabilizadores.Contains(x.IdViabilizador));
            }

            //if (dto.ViabilizadorRestrito)
            //{
            //    query = query.Where(x => dto.IdsEvs.Contains(x.IdEmpresaVenda));
            //}

            if (dto.IdsPontoVenda.HasValue())
            {
                query = query.Where(x => dto.IdsPontoVenda.Contains(x.IdPontoVenda));
            }

            if (dto.DocCompleta.HasValue())
            {
                bool docCompleta = dto.DocCompleta == 1;
                query = query.Where(reg => reg.DocCompleta == docCompleta);
            }
            if (dto.ElaboracaoDe.HasValue)
            {
                query = query.Where(reg => reg.Elaboracao.Date >= dto.ElaboracaoDe.Value.Date);
            }
            if (dto.ElaboracaoAte.HasValue)
            {
                query = query.Where(reg => reg.Elaboracao.Date <= dto.ElaboracaoAte.Value.Date);
            }
            if (dto.IdBreveLancamento.HasValue())
            {
                query = query.Where(reg => reg.IdBreveLancamento == dto.IdBreveLancamento);
            }
            if (dto.IdCorretor.HasValue())
            {
                query = query.Where(reg => reg.IdCorretor == dto.IdCorretor);
            }
            if (dto.IdEmpresaVenda.HasValue() && !dto.IdEmpresaVenda.Contains(0))
            {
                query = query.Where(reg => dto.IdEmpresaVenda.Contains(reg.IdEmpresaVenda));
            }
            if (dto.IdPontoVenda.HasValue())
            {
                query = query.Where(reg => reg.IdPontoVenda == dto.IdPontoVenda);
            }
            if (dto.NomeCliente.HasValue())
            {
                query = query.Where(reg => reg.NomeCliente.ToLower().Contains(dto.NomeCliente.ToLower()));
            }
            if (dto.Regionais.HasValue() && !dto.Regionais.Contains(""))
            {
                query = query.Where(reg => dto.Regionais.Contains(reg.Regional));
            }
            if (dto.DataEnvioDe.HasValue)
            {
                query = query.Where(reg => reg.DataEnvio.Value.Date >= dto.DataEnvioDe.Value.Date);
            }
            if (dto.DataEnvioAte.HasValue)
            {
                query = query.Where(reg => reg.DataEnvio.Value.Date <= dto.DataEnvioAte.Value.Date);
            }
            if (dto.CodigoProposta.HasValue())
            {
                query = query.Where(reg => reg.Codigo.Equals(dto.CodigoProposta));
            }

            if (dto.Situacoes.HasValue())
            {
                query = query.Where(reg=>dto.Situacoes.Contains(reg.StatusAnalise));
            }

            if (dto.IdViabilizador.HasValue())
            {
                query = query.Where(reg=> reg.IdViabilizador == dto.IdViabilizador);
            }

            if (dto.IdStandVenda.HasValue())
            {
                query = query.Where(x => x.IdStandVenda == dto.IdStandVenda);
            }

            if (dto.PodeVisualizar)
            {
                query = query.Where(reg => reg.IdCorretor == dto.IdCorretorVisualizador);
            }
            
            if (dto.CPF.HasValue())
            {
                query = query.Where(x => x.CpfCnpj.Equals(dto.CPF));
            }

            if (dto.IdCliente.HasValue())
            {
                query = query.Where(x => x.IdCliente == dto.IdCliente);
            }
            if (dto.NomeCCA.HasValue())
            {
                query = query.Where(reg => reg.NomeCCA.ToUpper().Contains(dto.NomeCCA.ToUpper()));
            }
            if (dto.SituacaoAvalista.HasValue())
            {
                query = query.Where(reg => dto.SituacaoAvalista.Contains(reg.SituacaoAvalista));
            }
            if (dto.IdRegionais.HasValue() && !dto.IdRegionais.Contains(0))
            {
                query = query.Where(reg => dto.IdRegionais.Contains(reg.IdRegional));
            }
            if (dto.Estados.HasValue() && !dto.Estados.Contains(""))
            {
                dto.Estados = dto.Estados.Select(x => x.ToUpper()).ToList();
                query = query.Where(reg => dto.Estados.Contains(reg.UF.ToUpper()));
            }


            return query.ToDataRequest(request);
        }
        #region API Consulta Preproposta
        public DataSourceResponse<ViewPreProposta> ListarApi(FiltroConsultaPrePropostaDto filtro)
        {
            var query = Queryable();

            if (filtro.tipoStatusFiltro.HasValue())
            {
                if (filtro.tipoStatusFiltro == "A" && filtro.IdAgrupamentoProcessoPreProposta.HasValue() && filtro.CodigoSistema.HasValue())
                {
                    query = query.Where(w => (filtro.CodigoSistema == w.CodigoSistemaCorretor && filtro.IdAgrupamentoProcessoPreProposta.Contains(w.IdAgrupamentoPrePropostaCorretor)) ||
                                             (filtro.CodigoSistema == w.CodigoSistemaHouse && filtro.IdAgrupamentoProcessoPreProposta.Contains(w.IdAgrupamentoPrePropostaHouse)) ||
                                             (filtro.CodigoSistema == w.CodigoSistemaLoja && filtro.IdAgrupamentoProcessoPreProposta.Contains(w.IdAgrupamentoPrePropostaLoja)));
                }
                else {
                    query = query.Where(w => filtro.IdAgrupamentoProcessoPreProposta.Contains(w.IdStatusPreProposta) ||
                                             filtro.IdAgrupamentoProcessoPreProposta.Contains(w.IdStatusPreProposta) ||
                                             filtro.IdAgrupamentoProcessoPreProposta.Contains(w.IdStatusPreProposta));
                }
            }
            if (filtro.IdEmpresasVendas.HasValue())
            {
                query = query.Where(x => filtro.IdEmpresasVendas.Contains(x.IdEmpresaVenda));
            }
            if (filtro.IsCoordenadorHouse || filtro.IsSupervisorHouse|| filtro.IsAgenteHouse)
            {
                query = query.Where(x => filtro.IdAgentes.Contains(x.IdElaborador));
            }
            if (filtro.Faturado.HasValue())
            {
                var faturado = filtro.Faturado == 1;

                query = query.Where(x => x.Faturado == faturado);
            }

            if (filtro.DataFaturadoDe.HasValue())
            {
                query = query.Where(x => x.DataFaturado != null)
                    .Where(x => x.DataFaturado.Value.Date >= filtro.DataFaturadoDe.Value.Date);
            }

            if (filtro.DataFaturadoAte.HasValue())
            {
                query = query.Where(x => x.DataFaturado != null)
                    .Where(x => x.DataFaturado.Value.Date <= filtro.DataFaturadoAte.Value.Date);
            }

            if (filtro.IsSupervisor || filtro.IsCoordenador)
            {
                query = query.Where(x => filtro.IdsViabilizadores.Contains(x.IdViabilizador));
            }

            if (filtro.ViabilizadorRestrito)
            {
                query = query.Where(x => filtro.IdsEvs.Contains(x.IdEmpresaVenda));
            }

            if (filtro.DocCompleta.HasValue())
            {
                bool docCompleta = filtro.DocCompleta == 1;
                query = query.Where(reg => reg.DocCompleta == docCompleta);
            }
            if (filtro.ElaboracaoDe.HasValue)
            {
                query = query.Where(reg => reg.Elaboracao.Date >= filtro.ElaboracaoDe.Value.Date);
            }
            if (filtro.ElaboracaoAte.HasValue)
            {
                query = query.Where(reg => reg.Elaboracao.Date <= filtro.ElaboracaoAte.Value.Date);
            }
            if (filtro.IdBreveLancamento.HasValue())
            {
                query = query.Where(reg => reg.IdBreveLancamento == filtro.IdBreveLancamento);
            }
            if (filtro.IdCorretor.HasValue())
            {
                var corretor = _corretorRepository.FindById(filtro.IdCorretor);
                var funcao = corretor.Funcao;
                var podeVizualizar = corretor.EmpresaVenda.CorretorVisualizarClientes;
                if (funcao == Tenda.Domain.EmpresaVenda.Enums.TipoFuncao.Corretor)
                {
                    if (podeVizualizar)
                    {
                        query = query.Where(reg => reg.IdCorretor == filtro.IdCorretor);
                    }
                }
                query = query.Where(reg => reg.IdCorretor == filtro.IdCorretor);
            }
            if (filtro.IdEmpresaVenda.HasValue())
            {
                query = query.Where(reg => reg.IdEmpresaVenda == filtro.IdEmpresaVenda);
            }
            if (filtro.IdPontoVenda.HasValue())
            {
                query = query.Where(reg => reg.IdPontoVenda == filtro.IdPontoVenda);
            }
            if (filtro.NomeCliente.HasValue())
            {
                query = query.Where(reg => reg.NomeCliente.ToLower().Contains(filtro.NomeCliente.ToLower()));
            }
            if (filtro.Regionais.HasValue() && !filtro.Regionais.Contains(""))
            {
                query = query.Where(reg => filtro.Regionais.Contains(reg.Regional));
            }
            if (filtro.DataEnvioDe.HasValue)
            {
                query = query.Where(reg => reg.DataEnvio.Value.Date >= filtro.DataEnvioDe.Value.Date);
            }
            if (filtro.DataEnvioAte.HasValue)
            {
                query = query.Where(reg => reg.DataEnvio.Value.Date <= filtro.DataEnvioAte.Value.Date);
            }
            if (filtro.CodigoProposta.HasValue())
            {
                query = query.Where(reg => reg.Codigo.Equals(filtro.CodigoProposta.ToUpper().Trim()));
            }

            if (filtro.Situacoes.HasValue())
            {
                query = query.Where(reg => filtro.Situacoes.Contains(reg.StatusAnalise));
            }

            if (filtro.IdViabilizador.HasValue())
            {
                query = query.Where(reg => reg.IdViabilizador == filtro.IdViabilizador);
            }

            if (filtro.IdStandVenda.HasValue())
            {
                query = query.Where(x => x.IdStandVenda == filtro.IdStandVenda);
            }

            if (filtro.CPF.HasValue())
            {
                query = query.Where(x => x.CpfCnpj.Equals(filtro.CPF));
            }

            if (filtro.IdCliente.HasValue())
            {
                query = query.Where(x => x.IdCliente == filtro.IdCliente);
            }
            if (filtro.NomeCCA.HasValue())
            {
                query = query.Where(reg => reg.NomeCCA.ToUpper().Contains(filtro.NomeCCA.ToUpper()));
            }
            if (filtro.TipoEmpresaVenda.HasValue)
            {
                query = query.Where(x => x.TipoEmpresaVenda == filtro.TipoEmpresaVenda);
            }

            return query.ToDataRequest(filtro.DataSourceRequest);
        }

        public byte[] Exportar(FiltroConsultaPrePropostaDto filtro)
        {
            var results = ListarApi(filtro);

            ExcelUtil excel = ExcelUtil.NewInstance(23)
                .NewSheet(DateTime.Now.ToString("yyyyMMddHHmmss"))
                .WithHeader(GetHeader());

            const string empty = "";

            foreach (var model in results.records.ToList())
            {
                excel
                    .CreateCellValue(model.Codigo).Width(25)
                    .CreateCellValue(model.NomeCliente).Width(20)
                    .CreateCellValue(model.SituacaoPrePropostaSuatEvs == GlobalMessages.SituacaoProposta_AguardandoAuditoria ? GlobalMessages.EmAnalise : (model.SituacaoPrePropostaPortalHouse.IsEmpty()?model.SituacaoPrePropostaSuatEvs: model.SituacaoPrePropostaPortalHouse)).Width(40)
                    .CreateCellValue(model.NumeroDocumentosPendentes).Width(40)
                    .CreateCellValue(model.MotivoParecer).Width(100)
                    .CreateCellValue(model.MotivoPendencia).Width(100)
                    .CreateCellValue(model.NomePontoVenda).Width(40)
                    .CreateCellValue(model.NomeCorretor).Width(40)
                    .CreateCellValue(model.NomeViabilizador).Width(40)
                    .CreateCellValue(model.NomeElaborador).Width(40)
                    .CreateCellValue(model.NomeBreveLancamento).Width(40)
                    .CreateCellValue(model.Elaboracao.ToDate()).Width(20)
                    .CreateCellValue(model.DataEnvio.HasValue ? model.DataEnvio.Value.ToDate() : empty).Width(20)
                    .CreateCellValue(model.NomeAssistenteAnalise).Width(40)
                    .CreateCellValue(model.TipoRenda).Width(40)
                    .CreateMoneyCell(model.RendaApurada).Width(40)
                    .CreateMoneyCell(model.FgtsApurado).Width(100)
                    .CreateMoneyCell(model.Entrada).Width(15)
                    .CreateMoneyCell(model.PreChaves)
                    .CreateMoneyCell(model.PreChavesIntermediaria).Width(15)
                    .CreateMoneyCell(model.Fgts).Width(40)
                    .CreateMoneyCell(model.Subsidio).Width(40)
                    .CreateMoneyCell(model.Financiamento).Width(40)
                    .CreateMoneyCell(model.PosChaves).Width(40)
                    .CreateCellValue(model.StatusSicaq.AsString()).Width(20)
                    .CreateCellValue(model.NomeAnalistaSicaq).Width(40)
                    .CreateCellValue(model.DataSicaq.HasValue ? model.DataSicaq.Value.ToDate() : empty).Width(30)
                    .CreateMoneyCell(model.ParcelaAprovada).Width(45)
                    .CreateCellValue(model.OrigemCliente.AsString()).Width(40);
            }
            excel.Close();
            return excel.DownloadFile();
        }

        private string[] GetHeader()
        {
            IList<string> header = new List<string>
            {
                GlobalMessages.Codigo,
                GlobalMessages.Cliente,
                GlobalMessages.StatusAnalise,
                GlobalMessages.NumeroDocumentosPendentes,
                GlobalMessages.ParecerTenda,
                GlobalMessages.MotivoPendencia,
                GlobalMessages.PontoVenda,
                GlobalMessages.Corretor,
                GlobalMessages.Viabilizador,
                GlobalMessages.Elaborador,
                GlobalMessages.Produto,
                GlobalMessages.DataElaboracao,
                GlobalMessages.DataUltimoEnvio,
                GlobalMessages.AssistenteAnalise,
                GlobalMessages.TipoRenda,
                GlobalMessages.RendaFamiliar,
                GlobalMessages.FGTSApurado,
                GlobalMessages.Entrada,
                GlobalMessages.PreChaves,
                GlobalMessages.PreChavesIntermediaria,
                GlobalMessages.FGTS,
                GlobalMessages.Subsidio,
                GlobalMessages.Financiamento,
                GlobalMessages.PosChaves,
                GlobalMessages.StatusSicaq,
                GlobalMessages.AnalistaSicaq,
                GlobalMessages.DataHoraSicaq,
                GlobalMessages.ParcelaAprovadaDoSICAQ,
                GlobalMessages.OrigemCliente
            };
            return header.ToArray();
        }

#endregion

        public DataSourceResponse<ViewPreProposta> ListarCarteira(DataSourceRequest request, long? idViabilizador, List<long> idEmpresaVenda, long? idPontoVenda)
        {
            var query = Queryable();

            query = query.Where(reg => reg.StatusAnalise != Tenda.Domain.EmpresaVenda.Enums.SituacaoProposta.Integrada)
                        .Where(reg => reg.StatusAnalise != Tenda.Domain.EmpresaVenda.Enums.SituacaoProposta.Cancelada);
                      
            if (idViabilizador.HasValue())
            {
                query = query.Where(reg => reg.IdViabilizador == idViabilizador);
            }
            if (idEmpresaVenda.HasValue())
            {
                query = query.Where(reg => idEmpresaVenda.Contains(reg.IdEmpresaVenda));
            }

            if (idPontoVenda.HasValue())
            {
                query = query.Where(x => x.IdPontoVenda == idPontoVenda);
            }
            return query.ToDataRequest(request);
        }
    }
}
