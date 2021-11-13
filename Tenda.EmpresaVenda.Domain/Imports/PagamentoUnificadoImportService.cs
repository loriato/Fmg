using Europa.Extensions;
using Europa.Resources;
using NHibernate;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Shared;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services.Models;

namespace Tenda.EmpresaVenda.Domain.Imports
{
    public class PagamentoUnificadoImportService
    {
        private ISession _session;
        private PagamentoRepository _pagamentoRepository;
        private ViewRelatorioVendaUnificadoRepository _viewRelatorioVendaUnificadoRepository;
        private NotificacaoRepository _notificacaoRepository;
        private CorretorRepository _corretorRepository;
        private EmpresaVendaRepository _empresaVendaRepository;

        #region Columns Index

        private static int columnIndex = 0;
        private static readonly int idxProposta = ++columnIndex;
        private static readonly int idxCodigoFornecedor = ++columnIndex;
        private static readonly int idxParcelaPagamento = ++columnIndex;
        private static readonly int idxRC = ++columnIndex;
        private static readonly int idxChamadoPedido = ++columnIndex;
        private static readonly int idxNumeroPedido = ++columnIndex; //Pedido Sap
        private static readonly int idxMIR7 = ++columnIndex;
        private static readonly int idxNF = ++columnIndex;
        private static readonly int idxChamadoPagamento = ++columnIndex;
        private static readonly int idxDataPrevistaPagamento = ++columnIndex;
        private static readonly int idxPago = ++columnIndex;
        private static readonly int idxPriorizarArquivo = ++columnIndex;


        private static int idxStatus = ++columnIndex;
        private static int idxDetalhes = ++columnIndex;

        #endregion

        #region Field Size

        private const int MaxSizeNF = 12;
        private const int MaxSizeTen = 10;
        private const int MaxSizeNumeroPedido = 255;
        private const int MaxSizeRC = 25;

        #endregion

        public void Process(ISession session, string diretorio, ref ImportTaskDTO importTask)
        {
            try
            {
                _session = session;
                InternalProcess(diretorio, ref importTask);
            }
            catch (Exception e)
            {
                importTask.Error = e.Message;
                importTask.End = DateTime.Now;
                ExceptionLogger.LogException(e);
            }
            finally
            {
                string targetFileLog = string.Format("{0}{1}{2}-log.txt", diretorio, Path.DirectorySeparatorChar,
                    importTask.TaskId);
                importTask.AppendLog(String.Format("Persistindo log em {0}, com identificador {1}",
                    Environment.MachineName, importTask.TaskId));
                File.WriteAllText(targetFileLog, importTask.FullLog.ToString());

                if (_session != null && _session.IsOpen)
                {
                    _session.Close();
                }
            }
        }

        public void InternalProcess(string diretorio, ref ImportTaskDTO importTask)
        {

            _pagamentoRepository = new PagamentoRepository()
            {
                _session = _session
            };
            _viewRelatorioVendaUnificadoRepository = new ViewRelatorioVendaUnificadoRepository()
            {
                _session = _session
            };
            _notificacaoRepository = new NotificacaoRepository()
            {
                _session = _session
            };
            _corretorRepository = new CorretorRepository()
            {
                _session = _session
            };
            _empresaVendaRepository = new EmpresaVendaRepository()
            {
                _session = _session
            };

            importTask.AppendLog("Execução de serviço iniciada");

            FileStream stream = File.Open(importTask.OriginalFilePath, FileMode.Open);

            ExcelPackage package = new ExcelPackage(stream);
            ExcelWorkbook workbook = package.Workbook;
            ExcelWorksheet worksheet = workbook.Worksheets.First();

            int rowCount = worksheet.Dimension.Rows;
            int columnCount = worksheet.Dimension.Columns;

            importTask.TotalLines = rowCount;

            // O indice para acessar o Cells[row,column] é baseado em 1
            int rowIndex = 1;

            // Sobreescrevendo cabeçalho
            package.Workbook.Worksheets[1].Cells[1, idxStatus].Value = "Status";
            package.Workbook.Worksheets[1].Cells[1, idxDetalhes].Value = "Detalhes";

            var validation = VerificarExcel(package);
            List<long> idsEmpresaVenda = new List<long>();

            if (validation)
            {
                var viewPagamentoInMemory = _viewRelatorioVendaUnificadoRepository.Queryable().Where(x => x.Faturado == true).ToList();

                var tipoPagamento = new TipoPagamento();

                ITransaction transaction = _session.BeginTransaction();
                while (rowIndex < rowCount)
                {
                    try
                    {
                        rowIndex++;

                        var proposta = worksheet.Cells[rowIndex, idxProposta].Value?.ToString().ToUpper().Trim();
                        var parcelaPagamento = worksheet.Cells[rowIndex, idxParcelaPagamento].Value?.ToString().ToUpper().Trim();
                        var pago = worksheet.Cells[rowIndex, idxPago].Value?.ToString().ToUpper().Trim();
                        var codigoFornecedor = worksheet.Cells[rowIndex, idxCodigoFornecedor].Value?.ToString().ToUpper().Trim();
                        var priorizarArquivo = worksheet.Cells[rowIndex, idxPriorizarArquivo].Value?.ToString().ToUpper().Trim() == GlobalMessages.Sim.ToUpper() ? true : false;
                        #region dados obrigatorios

                        if (proposta.IsEmpty())
                        {
                            LogError(ref worksheet, ref importTask, rowIndex,
                                string.Format(GlobalMessages.Import_AtributoNaoInformado, GlobalMessages.Proposta));
                            continue;
                        }

                        if (codigoFornecedor.IsEmpty())
                        {
                            LogError(ref worksheet, ref importTask, rowIndex,
                              string.Format(GlobalMessages.Import_AtributoNaoInformado, GlobalMessages.CodigoFornecedor));
                            continue;
                        }
                        var idEmpresaVenda = _empresaVendaRepository.BuscarPorCodigoFornecedor(codigoFornecedor);

                        if (idEmpresaVenda.IsEmpty())
                        {
                            LogError(ref worksheet, ref importTask, rowIndex,
                                string.Format(GlobalMessages.Import_AtributoInvalido, GlobalMessages.CodigoFornecedor));
                            continue;
                        }
                        var propostaRelacionadaFornecedor = _viewRelatorioVendaUnificadoRepository.RelacionarEmpresaVendaProposta(idEmpresaVenda, proposta);

                        if (propostaRelacionadaFornecedor.IsEmpty())
                        {
                            LogError(ref worksheet, ref importTask, rowIndex,
                                string.Format(GlobalMessages.Import_AtributoNaoRelacionado, GlobalMessages.Proposta, GlobalMessages.Fornecedor));
                            continue;
                        }

                        if (parcelaPagamento.IsEmpty())
                        {
                            LogError(ref worksheet, ref importTask, rowIndex,
                                string.Format(GlobalMessages.Import_AtributoNaoInformado, GlobalMessages.ParcelaPagamento));
                            continue;
                        }

                        if (parcelaPagamento.ToUpper().Equals(GlobalMessages.TipoPagamento_KitCompleto.ToUpper()))
                        {
                            tipoPagamento = TipoPagamento.KitCompleto;
                        }
                        else if (parcelaPagamento.ToUpper().Equals(GlobalMessages.TipoPagamento_Repasse.ToUpper()))
                        {
                            tipoPagamento = TipoPagamento.Repasse;

                        }
                        else if (parcelaPagamento.ToUpper().Equals(GlobalMessages.TipoPagamento_Conformidade.ToUpper()))
                        {
                            tipoPagamento = TipoPagamento.Conformidade;
                        }

                        if (tipoPagamento.IsEmpty())
                        {
                            LogError(ref worksheet, ref importTask, rowIndex,
                                string.Format(GlobalMessages.Import_AtributoInvalido, GlobalMessages.ParcelaPagamento));
                            continue;
                        }

                        if (pago.IsEmpty())
                        {
                            LogError(ref worksheet, ref importTask, rowIndex,
                                string.Format(GlobalMessages.Import_AtributoNaoInformado, GlobalMessages.Pago));
                            continue;
                        }

                        if (pago.ToUpper().Trim() != "NÃO" && pago.ToUpper().Trim() != "NAO" && pago.ToUpper().Trim() != "SIM")
                        {
                            LogError(ref worksheet, ref importTask, rowIndex,
                               string.Format(GlobalMessages.Import_AtributoInvalido, GlobalMessages.Pago));
                            continue;
                        }

                        #endregion

                        var viewPagamento = viewPagamentoInMemory.Where(reg => reg.CodigoProposta == proposta)
                                                                 .Where(reg => reg.TipoPagamento == tipoPagamento)
                                                                 .Where(reg => reg.IdEmpresaVenda == idEmpresaVenda)
                                                                 .FirstOrDefault();

                        if (viewPagamento.IsEmpty())
                        {
                            LogError(ref worksheet, ref importTask, rowIndex, string.Format(GlobalMessages.PropostaNaoEncontrada, proposta));
                            continue;
                        }
                        else if (viewPagamento.EmReversao)
                        {
                            LogError(ref worksheet, ref importTask, rowIndex, "O pagamento está cancelado.");
                            continue;
                        }

                        var pagamentoAntigo = _pagamentoRepository.Queryable().Where(reg => reg.Proposta.CodigoProposta == proposta)
                                                           .Where(reg => reg.TipoPagamento == tipoPagamento)
                                                           .Where(reg => reg.Situacao == Tenda.Domain.Core.Enums.Situacao.Ativo)
                                                           .Where(reg => reg.EmpresaVenda.Id == idEmpresaVenda)
                                                           .FirstOrDefault();


                        if (pagamentoAntigo.HasValue())
                        {
                            pagamentoAntigo.Situacao = Tenda.Domain.Core.Enums.Situacao.Cancelado;
                            _pagamentoRepository.Save(pagamentoAntigo);
                        }

                        #region Get Values


                        var pagamento = new Pagamento();

                        if (pagamentoAntigo.HasValue())
                        {
                            if (pagamentoAntigo.NotaFiscalPagamento.HasValue())
                            {
                                pagamento.NotaFiscalPagamento = pagamentoAntigo.NotaFiscalPagamento;
                            }
                            pagamento.ReciboCompra = pagamentoAntigo.ReciboCompra;
                            pagamento.StatusIntegracaoSap = pagamentoAntigo.StatusIntegracaoSap;
                            pagamento.PedidoSap = pagamentoAntigo.PedidoSap;
                            pagamento.DataRequisicaoCompra = pagamentoAntigo.DataRequisicaoCompra;
                            pagamento.DataPedidoSap = pagamentoAntigo.DataPedidoSap;
                        }

                        pagamento.Proposta = new PropostaSuat { Id = viewPagamento.IdProposta };
                        pagamento.TipoPagamento = tipoPagamento;
                        pagamento.Situacao = Tenda.Domain.Core.Enums.Situacao.Ativo;
                        pagamento.ChamadoPedido = worksheet.Cells[rowIndex, idxChamadoPedido].Value?.ToString().Trim();
                        pagamento.NotaFiscal = worksheet.Cells[rowIndex, idxNF].Value?.ToString().Trim();
                        pagamento.MIR7 = worksheet.Cells[rowIndex, idxMIR7].Value?.ToString()?.OnlyNumber().Trim();
                        pagamento.ChamadoPagamento = worksheet.Cells[rowIndex, idxChamadoPagamento].Value?.ToString().Trim();
                        if (worksheet.Cells[rowIndex, idxDataPrevistaPagamento].Value.HasValue())
                        {
                            pagamento.DataPrevisaoPagamento = DateTime.Parse(worksheet.Cells[rowIndex, idxDataPrevistaPagamento].Value?.ToString());
                        }
                        pagamento.Pago = pago == GlobalMessages.Sim.ToUpper() ? true : false;
                        pagamento.EmpresaVenda = new Tenda.Domain.EmpresaVenda.Models.EmpresaVenda { Id = viewPagamento.IdEmpresaVenda };
                        pagamento.ValorPagamento = viewPagamento.ValorAPagar;

                        if (priorizarArquivo)
                        {

                            var pedidoSap = worksheet.Cells[rowIndex, idxNumeroPedido].Value?.ToString().Trim();

                            if (pedidoSap.HasValue() && pagamento.PedidoSap.IsEmpty())
                            {
                                pagamento.DataPedidoSap = DateTime.Now;
                            }
                            else if (pedidoSap.HasValue() && pagamento.PedidoSap.HasValue() && !pedidoSap.Equals(pagamento.PedidoSap))
                            {
                                pagamento.DataPedidoSap = DateTime.Now;
                            }
                            else if (pedidoSap.HasValue() && pedidoSap.Equals(pagamento.PedidoSap) && pagamento.DataPedidoSap.IsEmpty())
                            {
                                pagamento.DataPedidoSap = DateTime.Now;
                            }

                            var rc = worksheet.Cells[rowIndex, idxRC].Value?.ToString().Trim();

                            if (rc.HasValue() && pagamento.ReciboCompra.IsEmpty())
                            {
                                pagamento.DataRequisicaoCompra = DateTime.Now;
                            }

                            pagamento.ReciboCompra = rc;
                            pagamento.PedidoSap = pedidoSap;
                        }
                        #endregion

                        #region Validate dados


                        if (pagamento.ReciboCompra.HasValue() && pagamento.ReciboCompra.Length > MaxSizeRC && priorizarArquivo)
                        {
                            LogError(ref worksheet, ref importTask, rowIndex,
                                string.Format(GlobalMessages.Import_AtributoInvalido,
                                    GlobalMessages.RC));
                            continue;
                        }
                        if (pagamento.ChamadoPedido.HasValue() && pagamento.ChamadoPedido.Length > MaxSizeTen)
                        {
                            LogError(ref worksheet, ref importTask, rowIndex,
                                string.Format(GlobalMessages.Import_AtributoInvalido,
                                    GlobalMessages.ChamadoPedido));
                            continue;
                        }
                        if (pagamento.PedidoSap.HasValue() && pagamento.PedidoSap.Length > MaxSizeNumeroPedido)
                        {
                            LogError(ref worksheet, ref importTask, rowIndex,
                                string.Format(GlobalMessages.Import_AtributoInvalido,
                                    GlobalMessages.NumeroPedido));
                            continue;
                        }
                        if (pagamento.NotaFiscal.HasValue() && pagamento.NotaFiscal.Length > MaxSizeNF)
                        {
                            LogError(ref worksheet, ref importTask, rowIndex,
                                string.Format(GlobalMessages.Import_AtributoInvalido,
                                    GlobalMessages.NF));
                            continue;
                        }
                        if (pagamento.MIR7.HasValue() && pagamento.MIR7.Length > MaxSizeTen)
                        {
                            LogError(ref worksheet, ref importTask, rowIndex,
                                string.Format(GlobalMessages.Import_AtributoInvalido,
                                    GlobalMessages.MIR7));
                            continue;
                        }
                        #endregion

                        if (pagamentoAntigo.HasValue())
                        {
                            if (pagamentoAntigo.PedidoSap.IsEmpty() && pagamento.PedidoSap.HasValue())
                            {

                                idsEmpresaVenda.Add(pagamentoAntigo.EmpresaVenda.Id);
                            }
                        }
                        else
                        {
                            if (pagamento.PedidoSap.HasValue())
                            {
                                idsEmpresaVenda.Add(pagamento.EmpresaVenda.Id);
                            }
                        }

                        _pagamentoRepository.Save(pagamento);

                        importTask.CurrentLine = rowIndex;

                        transaction.Commit();
                        _session.Clear();
                        transaction = _session.BeginTransaction();

                        worksheet.Cells[rowIndex, idxStatus].Value = GlobalMessages.Sucesso;
                        worksheet.Cells[rowIndex, idxDetalhes].Value = String.Format(GlobalMessages.Integracao_Sucesso,
                         GlobalMessages.Pagamento, pagamento.Proposta.CodigoProposta, pagamento.Id);
                        importTask.SuccessCount++;
                    }
                    catch (Exception e)
                    {
                        LogError(ref worksheet, ref importTask, rowIndex, e.Message);
                        if (transaction != null && transaction.IsActive)
                        {
                            transaction.Rollback();
                        }

                        ExceptionLogger.LogException(e);
                    }
                }
                if (transaction.IsActive)
                {
                    transaction.Commit();
                }

                transaction = _session.BeginTransaction();
                idsEmpresaVenda = idsEmpresaVenda.Distinct().ToList();
                EnviarNotificacao(idsEmpresaVenda);
                transaction.Commit();

            }

            _session.Flush();
            _session.Close();

            // Os menos 1 é para eliminar o cabeçalho
            importTask.AppendLog(string.Format("Processados {0} registros de {1}", rowIndex - 1, rowCount - 1));
            importTask.AppendLog(string.Format("{0} registros foram processados com sucesso", importTask.SuccessCount));
            if (validation)
            {
                importTask.AppendLog(string.Format("Ocorreram erros na importação de {0} registros",
                importTask.ErrorCount));
            }
            else
            {
                importTask.AppendLog("O arquivo de Importação é inválido");
            }
            importTask.AppendLog("Persistindo arquivo de retorno");
            package.Save();
            package.Stream.Position = 0;

            string targetFilePath = string.Format("{0}{1}{2}-target-{3}", diretorio, Path.DirectorySeparatorChar,
                importTask.TaskId, importTask.FileName);
            importTask.TargetFilePath = targetFilePath;
            using (FileStream file = new FileStream(targetFilePath, FileMode.Create, FileAccess.Write))
            {
                package.Stream.CopyTo(file);
            }

            importTask.AppendLog("Arquivo de retorno gerado com sucesso");
            importTask.AppendLog("Importação Finalizada");

            if (importTask.ErrorCount > 0)
            {
                importTask.AppendLog(
                    "Atenção! Ocorreram erros ao importar um ou mais registros. Faça o Download do arquivo de retorno para avaliar os problemas encontrados");
            }

            importTask.End = DateTime.Now;
        }

        private void LogError(ref ExcelWorksheet worksheet, ref ImportTaskDTO importTask, int rowIndex, string details)
        {
            importTask.IncrementError();
            worksheet.Cells[rowIndex, idxStatus].Value = GlobalMessages.Erro;
            worksheet.Cells[rowIndex, idxDetalhes].Value = details;
        }


        public bool VerificarExcel(ExcelPackage package)
        {
            if (package.Workbook.Worksheets[1].Cells[1, idxProposta].Value?.ToString() != GlobalMessages.Proposta)
            {
                return false;
            }
            else if (package.Workbook.Worksheets[1].Cells[1, idxParcelaPagamento].Value?.ToString() != GlobalMessages.ParcelaPagamento)
            {
                return false;
            }
            else if (package.Workbook.Worksheets[1].Cells[1, idxRC].Value?.ToString() != GlobalMessages.RC)
            {
                return false;
            }
            else if (package.Workbook.Worksheets[1].Cells[1, idxChamadoPedido].Value?.ToString() != GlobalMessages.ChamadoPedido)
            {
                return false;
            }
            else if (package.Workbook.Worksheets[1].Cells[1, idxMIR7].Value?.ToString() != GlobalMessages.MIR7)
            {
                return false;
            }
            else if (package.Workbook.Worksheets[1].Cells[1, idxNF].Value?.ToString() != GlobalMessages.NF)
            {
                return false;
            }
            else if (package.Workbook.Worksheets[1].Cells[1, idxPago].Value?.ToString() != GlobalMessages.Pago)
            {
                return false;
            }
            return true;
        }

        private void EnviarNotificacao(List<long> idsEmpresaVenda)
        {

            var diretores = _corretorRepository.ListarDiretoresAtivosDaEmpresaDeVendas(idsEmpresaVenda);

            foreach (var corretor in diretores)
            {
                var notificacao = new Notificacao
                {
                    Titulo = GlobalMessages.NotificacaoNotaFiscal_Titulo,
                    Conteudo = GlobalMessages.NotificacaoNotaFiscal_Conteudo,
                    Usuario = corretor.Usuario,
                    EmpresaVenda = corretor.EmpresaVenda,
                    TipoNotificacao = TipoNotificacao.Lead,
                    Link = ProjectProperties.EvsBaseUrl + "/financeiro",
                    NomeBotao = GlobalMessages.IrParaFinanceiro,
                    DestinoNotificacao = DestinoNotificacao.Portal,
                };

                _notificacaoRepository.Save(notificacao);
            }
        }
    }
}
