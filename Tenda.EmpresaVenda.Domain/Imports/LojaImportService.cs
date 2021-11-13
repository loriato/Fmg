using Europa.Extensions;
using Europa.Resources;
using NHibernate;
using OfficeOpenXml;
using System;
using System.IO;
using System.Linq;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services.Models;

namespace Tenda.Domain.EmpresaVenda.Imports
{
    public class LojaImportService
    {
        private ISession _session;

        private LojaRepository _lojaRepository;
        private RegionaisRepository _regionaisRepository { get; set; }


        private static int columnIndex = 0;
        private static readonly int idxNome = ++columnIndex;
        private static readonly int idxNomeFantasia = ++columnIndex;
        private static readonly int idxSapId = ++columnIndex;
        private static readonly int idxRegional = ++columnIndex;
        private static readonly int idxDataIntegracao = ++columnIndex;
        private static readonly int idxSituacao = ++columnIndex;
        private static readonly int idxStatus = ++columnIndex;
        private static readonly int idxDetalhes = ++columnIndex;

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
                string targetFileLog = string.Format("{0}{1}{2}-log.txt", diretorio, Path.DirectorySeparatorChar, importTask.TaskId);
                importTask.AppendLog(String.Format("Persistindo log em {0}, com identificador {1}", Environment.MachineName, importTask.TaskId));
                File.WriteAllText(targetFileLog, importTask.FullLog.ToString());

                if (_session != null && _session.IsOpen)
                {
                    _session.Close();
                }
            }
        }

        public void InternalProcess(string diretorio, ref ImportTaskDTO importTask)
        {

            _lojaRepository = new LojaRepository(_session);

            importTask.AppendLog("Execução de serviço iniciada");

            FileStream stream = File.Open(importTask.OriginalFilePath, FileMode.Open);

            ExcelPackage package = new ExcelPackage(stream);
            ExcelWorkbook workbook = package.Workbook;
            ExcelWorksheet worksheet = workbook.Worksheets.First();

            int rowCount = worksheet.Dimension.Rows;
            int colCount = worksheet.Dimension.Columns;

            importTask.TotalLines = rowCount;

            // O indice para acessar o Cells[row,column] é baseado em 1
            int rowIndex = 1;

            // Sobreescrevendo cabeçalho
            package.Workbook.Worksheets[1].Cells[1, idxStatus].Value = "Status";
            package.Workbook.Worksheets[1].Cells[1, idxDetalhes].Value = "Detalhes";

            LojaDto dto = new LojaDto();

            while (rowIndex < rowCount)
            {
                ITransaction transaction = null;
                try
                {
                    rowIndex++;

                    dto = new LojaDto();
                    dto.Nome = worksheet.Cells[rowIndex, idxNome].Value?.ToString();
                    dto.NomeFantasia = worksheet.Cells[rowIndex, idxNomeFantasia].Value?.ToString();
                    dto.SapId = worksheet.Cells[rowIndex, idxSapId].Value?.ToString();
                    dto.Regional[0] = _regionaisRepository.findByName(worksheet.Cells[rowIndex, idxRegional].Value?.ToString()).Id;

                    // Nome
                    if (dto.Nome.IsEmpty())
                    {
                        LogError(ref worksheet, ref importTask, rowIndex, string.Format(GlobalMessages.Import_AtributoNaoInformado, GlobalMessages.Nome));
                        continue;
                    }
                    // Nome Fantasia
                    if (dto.NomeFantasia.IsEmpty())
                    {
                        LogError(ref worksheet, ref importTask, rowIndex, string.Format(GlobalMessages.Import_AtributoNaoInformado, GlobalMessages.NomeFantasia));
                        continue;
                    }

                    // SAP id
                    if (worksheet.Cells[rowIndex, idxSapId].Value.IsEmpty())
                    {
                        LogError(ref worksheet, ref importTask, rowIndex, string.Format(GlobalMessages.Import_AtributoNaoInformado, GlobalMessages.IdSap));
                        continue;
                    }

                    // Regional
                    if (dto.Regional.IsEmpty())
                    {
                        LogError(ref worksheet, ref importTask, rowIndex, string.Format(GlobalMessages.Import_AtributoNaoInformado, GlobalMessages.Regional));
                        continue;
                    }

                    var loja = _lojaRepository.FindByIdSap(dto.SapId);

                    dto.SapId = dto.SapId.ToUpper().Trim();

                    if (loja.HasValue())
                    {
                        var regional = _regionaisRepository.findById(dto.Regional[0]);
                        if (regional.IsEmpty())
                        {
                            regional = new Regionais();
                            regional.Nome = loja.Regional.Nome;
                            _regionaisRepository.Save(regional);
                        }

                        loja.SapId = dto.SapId;
                        loja.Nome = dto.Nome;
                        loja.NomeFantasia = dto.NomeFantasia;
                        loja.Regional = regional;
                        loja.DataIntegracao = DateTime.Now;
                    }
                    else
                    {
                        loja = dto.ToModel();
                        loja.Situacao = Situacao.Ativo;
                        loja.DataIntegracao = DateTime.Now;
                    }

                    importTask.CurrentLine = rowIndex;

                    transaction = _session.BeginTransaction();

                    _lojaRepository.Save(loja);

                    transaction.Commit();

                    worksheet.Cells[rowIndex, idxStatus].Value = GlobalMessages.Sucesso;
                    worksheet.Cells[rowIndex, idxDetalhes].Value = String.Format(GlobalMessages.Integracao_Sucesso, GlobalMessages.Loja, loja.SapId, loja.Id);

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

            _session.Close();

            // Os menos 1 é para eliminar o cabeçalho
            importTask.AppendLog(string.Format("Processados {0} registros de {1}", rowIndex - 1, rowCount - 1));
            importTask.AppendLog(string.Format("{0} registros foram processados com sucesso", importTask.SuccessCount));
            importTask.AppendLog(string.Format("Ocorreram erros na importação de {0} registros", importTask.ErrorCount));

            importTask.AppendLog("Persistindo arquivo de retorno");
            package.Save();
            package.Stream.Position = 0;

            string targetFilePath = string.Format("{0}{1}{2}-target-{3}", diretorio, Path.DirectorySeparatorChar, importTask.TaskId, importTask.FileName);
            importTask.TargetFilePath = targetFilePath;
            using (FileStream file = new FileStream(targetFilePath, FileMode.Create, FileAccess.Write))
            {
                package.Stream.CopyTo(file);
            }
            importTask.AppendLog("Arquivo de retorno gerado com sucesso");
            importTask.AppendLog("Importação Finalizada");

            if (importTask.ErrorCount > 0)
            {
                importTask.AppendLog("Atenção! Ocorreram erros ao importar um ou mais registros. Faça o Download do arquivo de retorno para avaliar os problemas encontrados");
            }
            importTask.End = DateTime.Now;
        }

        private void LogError(ref ExcelWorksheet worksheet, ref ImportTaskDTO importTask, int rowIndex, string details)
        {
            importTask.IncrementError();
            worksheet.Cells[rowIndex, idxStatus].Value = GlobalMessages.Erro;
            worksheet.Cells[rowIndex, idxDetalhes].Value = details;
        }

    }
}
