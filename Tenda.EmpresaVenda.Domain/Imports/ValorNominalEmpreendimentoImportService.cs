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
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services.Models;

namespace Tenda.EmpresaVenda.Domain.Imports
{
    public class ValorNominalEmpreendimentoImportService
    {
        private ISession _session;
        private EmpreendimentoRepository _empreendimentoRepository;
        private ValorNominalRepository _valorNominalRepository;
        private CorretorRepository _corretorRepository;
        private NotificacaoRepository _notificacaoRepository;
        private EnderecoEmpreendimentoRepository _enderecoEmpreendimentoRepository;
        private EmpresaVendaRepository _empresaVendaRepository;

        #region Columns Index

        private static int columnIndex = 0;
        private static readonly int idxDivisao = ++columnIndex;
        private static readonly int idxNomeEmpreendimento = ++columnIndex;
        private static readonly int idxFaixaUmMeioDe = ++columnIndex;
        private static readonly int idxFaixaUmMeioAte = ++columnIndex;
        private static readonly int idxFaixaDoisDe = ++columnIndex;
        private static readonly int idxFaixaDoisAte = ++columnIndex;
        private static readonly int idxPNEDe = ++columnIndex;
        private static readonly int idxPNEAte = ++columnIndex;



        private static int idxStatus = ++columnIndex;
        private static int idxDetalhes = ++columnIndex;

        #endregion

        #region Field Size

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

            _empreendimentoRepository = new EmpreendimentoRepository()
            {
                _session = _session
            };
            _valorNominalRepository = new ValorNominalRepository()
            {
                _session = _session
            };
            _corretorRepository = new CorretorRepository()
            {
                _session = _session
            };
            _notificacaoRepository = new NotificacaoRepository()
            {
                _session = _session
            };

            _enderecoEmpreendimentoRepository = new EnderecoEmpreendimentoRepository()
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
            List<string> regionais = new List<string>();
            var dataHoje = DateTime.Now;

            if (validation)
            {
                var empreendimentosInMemory = _empreendimentoRepository.Queryable().ToList();
                var valorNominalInMemory = _valorNominalRepository.Queryable().ToList();
                var empreendimento = new Empreendimento();
                ITransaction transaction = _session.BeginTransaction();
                while (rowIndex < rowCount)
                {
                    try
                    {
                        rowIndex++;

                        var divisao = worksheet.Cells[rowIndex, idxDivisao].Value?.ToString().ToUpper().Trim();
                        #region dados obrigatorios

                        if (divisao.IsEmpty())
                        {
                            LogError(ref worksheet, ref importTask, rowIndex,
                                string.Format(GlobalMessages.Import_AtributoNaoInformado, GlobalMessages.Divisao));
                            continue;
                        }
                        else
                        {
                            empreendimento = empreendimentosInMemory.Where(reg => reg.Divisao.ToUpper().Equals(divisao.ToUpper())).FirstOrDefault();
                            if (empreendimento.IsEmpty())
                            {
                                LogError(ref worksheet, ref importTask, rowIndex, string.Format(GlobalMessages.EmpreendimentoNaoEncontrado));
                                continue;
                            }
                        }

                        #endregion


                        var valorNominalAntigo = valorNominalInMemory.Where(reg => reg.Empreendimento.Id.Equals(empreendimento.Id))
                                                            .Where(reg => reg.Situacao == SituacaoValorNominal.Ativo)
                                                           .FirstOrDefault();


                        if (valorNominalAntigo.HasValue())
                        {
                            valorNominalAntigo.Situacao = SituacaoValorNominal.Vencido;
                            valorNominalAntigo.TerminoVigencia = dataHoje;
                            _valorNominalRepository.Save(valorNominalAntigo);
                        }

                        #region Get Values


                        var valorNominal = new ValorNominal();

                        valorNominal.Empreendimento = new Empreendimento { Id = empreendimento.Id };
                        valorNominal.InicioVigencia = dataHoje;
                        valorNominal.Situacao = SituacaoValorNominal.Ativo;
                        valorNominal.FaixaUmMeioDe = Convert.ToDecimal(worksheet.Cells[rowIndex, idxFaixaUmMeioDe].Value?.ToString().Trim());
                        valorNominal.FaixaUmMeioAte = Convert.ToDecimal(worksheet.Cells[rowIndex, idxFaixaUmMeioAte].Value?.ToString().Trim());
                        valorNominal.FaixaDoisDe = Convert.ToDecimal(worksheet.Cells[rowIndex, idxFaixaDoisDe].Value?.ToString().Trim());
                        valorNominal.FaixaDoisAte = Convert.ToDecimal(worksheet.Cells[rowIndex, idxFaixaDoisAte].Value?.ToString().Trim());
                        valorNominal.PNEDe = Convert.ToDecimal(worksheet.Cells[rowIndex, idxPNEDe].Value?.ToString().Trim());
                        valorNominal.PNEAte = Convert.ToDecimal(worksheet.Cells[rowIndex, idxPNEAte].Value?.ToString().Trim());

                        #endregion

                        #region Validate dados

                        if (valorNominal.FaixaUmMeioDe > valorNominal.FaixaUmMeioAte)
                        {
                            LogError(ref worksheet, ref importTask, rowIndex, "O intervalo do valor nominal faixa 1,5 é inválido");
                            continue;
                        }

                        if (valorNominal.FaixaDoisDe > valorNominal.FaixaDoisAte)
                        {
                            LogError(ref worksheet, ref importTask, rowIndex, "O intervalo do valor nominal faixa 2 é inválido");
                            continue;

                        }
                        if (valorNominal.PNEDe > valorNominal.PNEAte)
                        {
                            LogError(ref worksheet, ref importTask, rowIndex, "O intervalo do valor nominal pne é inválido");
                            continue;
                        }
                        #endregion

                        _valorNominalRepository.Save(valorNominal);

                        if (valorNominalAntigo.HasValue())
                        {
                            valorNominalAntigo.Situacao = SituacaoValorNominal.Vencido;
                            valorNominalAntigo.TerminoVigencia = dataHoje;
                            _valorNominalRepository.Save(valorNominalAntigo);
                        }


                        var regional = _enderecoEmpreendimentoRepository.FindByEmpreendimento(empreendimento.Id).Estado;

                        if (regional.HasValue())
                        {
                            regionais.Add(regional);
                        }


                        importTask.CurrentLine = rowIndex;
                        if (rowIndex % 50 == 0)
                        {

                            transaction.Commit();
                            _session.Clear();
                            transaction = _session.BeginTransaction();
                        }


                        worksheet.Cells[rowIndex, idxStatus].Value = GlobalMessages.Sucesso;
                        worksheet.Cells[rowIndex, idxDetalhes].Value = String.Format(GlobalMessages.Integracao_Sucesso,
                         GlobalMessages.Empreendimento, divisao, valorNominal.Id);
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
                regionais = regionais.Distinct().ToList();
                var idsEmpresaVenda = _empresaVendaRepository.Queryable().Where(reg => regionais.Contains(reg.Estado))
                    .Where(reg => reg.Situacao == Tenda.Domain.Security.Enums.Situacao.Ativo)
                    .Select(x => x.Id).ToList();
                importTask.AppendLog("Enviando Notificações...");
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
            if (package.Workbook.Worksheets[1].Cells[1, idxDivisao].Value?.ToString().ToLower().Trim() != GlobalMessages.Divisao.ToLower())
            {
                return false;
            }
            else if (package.Workbook.Worksheets[1].Cells[1, idxNomeEmpreendimento].Value?.ToString().ToLower().Trim() != "nome empreendimento")
            {
                return false;
            }
            else if (package.Workbook.Worksheets[1].Cells[1, idxFaixaUmMeioDe].Value?.ToString().ToLower().Trim() != "valor n faixa 1,5 de")
            {
                return false;
            }
            else if (package.Workbook.Worksheets[1].Cells[1, idxFaixaDoisDe].Value?.ToString().ToLower().Trim() != "valor n faixa 2 de")
            {
                return false;
            }
            else if (package.Workbook.Worksheets[1].Cells[1, idxPNEDe].Value?.ToString().ToLower().Trim() != "valor n pne de")
            {
                return false;
            }
            return true;
        }

        private void EnviarNotificacao(List<long> idsEmpresaVenda)
        {

            var usuarios = _corretorRepository.ListarUsuariosAtivosDaEmpresaDeVendas(idsEmpresaVenda);

            foreach (var corretor in usuarios)
            {
                var notificacao = new Notificacao
                {
                    Titulo = GlobalMessages.ValorNominal_Titulo,
                    Conteudo = GlobalMessages.ValorNominal_Conteudo,
                    Usuario = corretor.Usuario,
                    EmpresaVenda = corretor.EmpresaVenda,
                    DestinoNotificacao = DestinoNotificacao.Portal,
                };

                _notificacaoRepository.Save(notificacao);
            }
        }
    }
}
