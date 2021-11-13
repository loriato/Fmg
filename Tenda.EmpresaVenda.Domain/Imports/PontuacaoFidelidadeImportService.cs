using Europa.Extensions;
using Europa.Resources;
using NHibernate;
using OfficeOpenXml;
using System;
using System.IO;
using System.Linq;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services.Models;

namespace Tenda.EmpresaVenda.Domain.Imports
{
    public class PontuacaoFidelidadeImportService
    {
        private ISession _session;

        private PropostaSuatRepository _propostaSuatRepository { get; set; }

        #region Columns Index

        private static int columnIndex = 0;
        private static readonly int idxPropostaIdentificada = ++columnIndex;

        private static int idxObservacoes = 6;

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
            _propostaSuatRepository = new PropostaSuatRepository
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

            package.Workbook.Worksheets[1].Cells[1, idxObservacoes].Value = GlobalMessages.Observacoes;


            var validation = VerificarExcel(package);

            if (validation)
            {
                var propostasSuatInMemory = _propostaSuatRepository.Queryable().Where(x => x.PropostaIdentificada != null).ToList();

                ITransaction transaction = _session.BeginTransaction();
                while (rowIndex < rowCount)
                {
                    try
                    {
                        rowIndex++;

                        var propostaIdentificada = worksheet.Cells[rowIndex, idxPropostaIdentificada].Value?.ToString().ToUpper().Trim();

                        #region dados obrigatorios

                        if (propostaIdentificada.IsEmpty())
                        {
                            LogError(ref worksheet, ref importTask, rowIndex,
                                string.Format(GlobalMessages.Import_AtributoNaoInformado, GlobalMessages.PIPNI));
                            continue;
                        }

                        #endregion

                        propostaIdentificada = propostaIdentificada.PadLeft(10, '0');

                        var proposta = propostasSuatInMemory.Where(x => x.PropostaIdentificada.PadLeft(10, '0').Equals(propostaIdentificada))
                            .SingleOrDefault();

                        if (proposta.IsEmpty())
                        {
                            LogError(ref worksheet, ref importTask, rowIndex,
                                string.Format(GlobalMessages.DadoInexistente, GlobalMessages.PIPNI));
                            continue;
                        }

                        if (!proposta.Faturado)
                        {
                            proposta.DataFaturado = DateTime.Now;
                        }
                        proposta.Faturado = true;                        

                        proposta.AtualizadoEm = DateTime.Now;
                        _propostaSuatRepository.Save(proposta);

                        importTask.CurrentLine = rowIndex;
                        if (rowIndex % 50 == 0)
                        {

                            transaction.Commit();
                            _session.Clear();
                            transaction = _session.BeginTransaction();
                        }

                        worksheet.Cells[rowIndex, idxObservacoes].Value = String.Format(GlobalMessages.Integracao_Sucesso,
                         GlobalMessages.Proposta, proposta.CodigoProposta, proposta.Id);
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

        private void LogError(ref ExcelWorksheet worksheet, ref ImportTaskDTO importTask, int rowIndex, string v)
        {
            importTask.IncrementError();
            worksheet.Cells[rowIndex, idxObservacoes].Value = v;
        }

        public bool VerificarExcel(ExcelPackage package)
        {
            if (package.Workbook.Worksheets[1].Cells[1, idxPropostaIdentificada].Value?.ToString() != GlobalMessages.PIPNI)
            {
                return false;
            }

            return true;
        }
    }
}
