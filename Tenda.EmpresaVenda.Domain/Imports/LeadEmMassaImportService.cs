using Europa.Extensions;
using Europa.Resources;
using NHibernate;
using OfficeOpenXml;
using System;
using System.IO;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services.Models;


namespace Tenda.EmpresaVenda.Domain.Imports
{
    public class LeadEmMassaImportService
    {
        private ISession _session;
        private EmpresaVendaRepository _empresaVendaRepository;
        private LeadEmpresaVendaRepository _leadEmpresaVendaRepository;
        private LeadRepository _leadRepository;
        private long OrigemLead { get; set; }

        public LeadEmMassaImportService(long origem)
        {
            OrigemLead = origem;
        }


        #region Columns Index

        private static readonly int idxNome = 7;
        private static readonly int idxTelefone1 = 11;
        private static readonly int idxTelefone2 = 12;
        private static readonly int idxEmail = 13;
        private static readonly int idxLogradouro = 14;
        private static readonly int idxNumero = 15;
        private static readonly int idxComplemento = 16;
        private static readonly int idxBairro = 17;
        private static readonly int idxCep = 18;
        private static readonly int idxCidade = 19;
        private static readonly int idxEstado = 20;
        private static readonly int idxEmpresaVenda = 42;
        private static readonly int idxPacote = 43;

        private static int idxStatus;
        private static int idxDetalhes;

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
            _leadRepository = new LeadRepository()
            {
                _session = _session
            };
            _leadEmpresaVendaRepository = new LeadEmpresaVendaRepository()
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

            idxStatus = columnCount + 1;
            idxDetalhes = columnCount + 2;

            importTask.TotalLines = rowCount;

            // O indice para acessar o Cells[row,column] é baseado em 1
            int rowIndex = 1;

            // Sobreescrevendo cabeçalho
            package.Workbook.Worksheets[1].Cells[1, idxStatus].Value = "Status";
            package.Workbook.Worksheets[1].Cells[1, idxDetalhes].Value = "Detalhes";

            var validation = VerificarExcel(package);

            var DataAtual = DateTime.Now;
            var pacoteExistentes = _leadRepository.Queryable().Select(x => x.DescricaoPacote).Distinct().ToList();

            if (validation)
            {

                ITransaction transaction = _session.BeginTransaction();
                while (rowIndex < rowCount)
                {
                    try
                    {
                        rowIndex++;
                        var empresaVendaExcel = worksheet.Cells[rowIndex, idxEmpresaVenda].Value?.ToString().Trim();
                        var descricaoPacote = worksheet.Cells[rowIndex, idxPacote].Value?.ToString().Trim();

                        #region Dados Obrigatorios

                        if (empresaVendaExcel.IsEmpty())
                        {
                            LogError(ref worksheet, ref importTask, rowIndex,
                                string.Format(GlobalMessages.Import_AtributoNaoInformado, GlobalMessages.EmpresaVenda));
                            continue;
                        }

                        if (descricaoPacote.IsEmpty())
                        {
                            LogError(ref worksheet, ref importTask, rowIndex,
                                string.Format(GlobalMessages.Import_AtributoNaoInformado, GlobalMessages.Pacote));
                            continue;
                        }

                        if (pacoteExistentes.Where(x => x.ToUpper().Equals(descricaoPacote.ToUpper())).Any())
                        {
                            LogError(ref worksheet, ref importTask, rowIndex, GlobalMessages.MsgErroPacoteExistente);
                            continue;
                        }

                        #endregion

                        var empresaVenda = _empresaVendaRepository.Queryable().Where(x => x.NomeFantasia.ToLower() == empresaVendaExcel.ToLower())
                                                                .Select(x => x.Id)
                                                                .SingleOrDefault();
                        if (empresaVenda.IsEmpty())
                        {
                            LogError(ref worksheet, ref importTask, rowIndex,
                                string.Format(GlobalMessages.EmpresaVendaNaoEncontrada, empresaVendaExcel));
                            continue;
                        }


                        #region Get Values
                        var lead = new Lead();

                        lead.NomeCompleto = worksheet.Cells[rowIndex, idxNome].Value?.ToString();
                        lead.Telefone1 = worksheet.Cells[rowIndex, idxTelefone1].Value?.ToString()?.OnlyNumber();
                        lead.Telefone2 = worksheet.Cells[rowIndex, idxTelefone2].Value?.ToString()?.OnlyNumber();
                        lead.Email = worksheet.Cells[rowIndex, idxEmail].Value?.ToString();
                        lead.Cep = worksheet.Cells[rowIndex, idxCep].Value?.ToString()?.OnlyNumber();
                        lead.Logradouro = worksheet.Cells[rowIndex, idxLogradouro].Value?.ToString();
                        lead.Numero = worksheet.Cells[rowIndex, idxNumero].Value?.ToString();
                        lead.Complemento = worksheet.Cells[rowIndex, idxComplemento].Value?.ToString();
                        lead.Bairro = worksheet.Cells[rowIndex, idxBairro].Value?.ToString();
                        lead.Cidade = worksheet.Cells[rowIndex, idxCidade].Value?.ToString();
                        lead.Estado = worksheet.Cells[rowIndex, idxEstado].Value?.ToString();
                        lead.DataPacote = DataAtual;
                        lead.DescricaoPacote = descricaoPacote;
                        lead.OrigemLead = new OrigemLead { Id = OrigemLead };

                        #endregion

                        _leadRepository.Save(lead);

                        var leadEmprevenda = new LeadEmpresaVenda();
                        leadEmprevenda.EmpresaVenda = new Tenda.Domain.EmpresaVenda.Models.EmpresaVenda { Id = empresaVenda };
                        leadEmprevenda.Lead = new Lead { Id = lead.Id };
                        leadEmprevenda.Situacao = Tenda.Domain.EmpresaVenda.Enums.SituacaoLead.Contato;
                        _leadEmpresaVendaRepository.Save(leadEmprevenda);

                        importTask.CurrentLine = rowIndex;
                        if (rowIndex % 200 == 0)
                        {
                            transaction.Commit();
                            _session.Clear();
                            transaction = _session.BeginTransaction();
                        }


                        worksheet.Cells[rowIndex, idxStatus].Value = GlobalMessages.Sucesso;
                        worksheet.Cells[rowIndex, idxDetalhes].Value = String.Format(GlobalMessages.Integracao_Sucesso,
                         GlobalMessages.Lead, lead.NomeCompleto, lead.Id);
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
            if (package.Workbook.Worksheets[1].Cells[1, idxNome].Value?.ToString() != GlobalMessages.NomePreCliente)
            {
                return false;
            }
            else if (package.Workbook.Worksheets[1].Cells[1, idxTelefone1].Value?.ToString() != GlobalMessages.Telefone1)
            {
                return false;
            }
            else if (package.Workbook.Worksheets[1].Cells[1, idxTelefone2].Value?.ToString() != GlobalMessages.Telefone2)
            {
                return false;
            }
            else if (package.Workbook.Worksheets[1].Cells[1, idxEmail].Value?.ToString() != GlobalMessages.Email)
            {
                return false;
            }
            else if (package.Workbook.Worksheets[1].Cells[1, idxLogradouro].Value?.ToString() != GlobalMessages.Endereco)
            {
                return false;
            }
            else if (package.Workbook.Worksheets[1].Cells[1, idxBairro].Value?.ToString() != GlobalMessages.Bairro)
            {
                return false;
            }
            else if (package.Workbook.Worksheets[1].Cells[1, idxCep].Value?.ToString() != GlobalMessages.CEP)
            {
                return false;
            }
            else if (package.Workbook.Worksheets[1].Cells[1, idxCidade].Value?.ToString() != GlobalMessages.Cidade)
            {
                return false;
            }
            else if (package.Workbook.Worksheets[1].Cells[1, idxEstado].Value?.ToString() != GlobalMessages.UF)
            {
                return false;
            }
            else if (package.Workbook.Worksheets[1].Cells[1, idxNumero].Value?.ToString() != GlobalMessages.Numero)
            {
                return false;
            }
            else if (package.Workbook.Worksheets[1].Cells[1, idxComplemento].Value?.ToString() != GlobalMessages.Complemento)
            {
                return false;
            }
            else if (package.Workbook.Worksheets[1].Cells[1, idxPacote].Value?.ToString() != GlobalMessages.Pacote)
            {
                return false;
            }
            return true;
        }
    }
}