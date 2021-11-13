using Europa.Extensions;
using Europa.Resources;
using NHibernate;
using OfficeOpenXml;
using System;
using System.IO;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services.Models;

namespace Tenda.Domain.EmpresaVenda.Imports
{
    public class EmpreendimentoImportService
    {
        private ISession _session;
        private EmpreendimentoRepository _empreendimentoRepository;
        private EnderecoEmpreendimentoRepository _enderecoEmpreendimentoRepository;
        private RegionaisRepository _regionaisRepository;


        #region Columns Index

        private static int columnIndex = 0;
        private static readonly int idxRegional = ++columnIndex;
        private static readonly int idxCodigoEmpresa = ++columnIndex;
        private static readonly int idxDivisao = ++columnIndex;
        private static readonly int idxNome = ++columnIndex;
        private static readonly int idxCnpj = ++columnIndex;
        private static readonly int idxNomeEmpresa = ++columnIndex;
        private static readonly int idxRegistroIncorporacao = ++columnIndex;
        private static readonly int idxCep = ++columnIndex;
        private static readonly int idxLogradouro = ++columnIndex;
        private static readonly int idxNumero = ++columnIndex;
        private static readonly int idxComplemento = ++columnIndex;
        private static readonly int idxBairro = ++columnIndex;
        private static readonly int idxCidade = ++columnIndex;
        private static readonly int idxEstado = ++columnIndex;
        private static readonly int idxMancha = ++columnIndex;
        private static readonly int idxDataLancamento = ++columnIndex;
        private static readonly int idxPrevisaoEntrega = ++columnIndex;
        private static readonly int idxDataEntrega = ++columnIndex;
        private static readonly int idxStatus = ++columnIndex;
        private static readonly int idxDetalhes = ++columnIndex;

        #endregion

        #region Field Size

        private const int MaxSizeDivisao = 20;
        private const int MaxSizeNome = 128;
        private const int MaxSizeRegional = 20;
        public static int MaxSizeCodEmpresa = 10;
        private const int MaxSizeNomeEmpresa = 128;
        private const int MaxSizeCnpj = 20;
        private const int MaxSizeRegistroIncorporacao = 128;
        private const int MaxSizeMancha = 256;

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
            _enderecoEmpreendimentoRepository = new EnderecoEmpreendimentoRepository()
            {
                _session = _session
            };

            var regionaisList = _regionaisRepository.getAll();

            importTask.AppendLog("Execução de serviço iniciada");

            FileStream stream = File.Open(importTask.OriginalFilePath, FileMode.Open);

            ExcelPackage package = new ExcelPackage(stream);
            ExcelWorkbook workbook = package.Workbook;
            ExcelWorksheet worksheet = workbook.Worksheets.First();

            int rowCount = worksheet.Dimension.Rows;

            importTask.TotalLines = rowCount;

            // O indice para acessar o Cells[row,column] é baseado em 1
            int rowIndex = 1;

            // Sobreescrevendo cabeçalho
            package.Workbook.Worksheets[1].Cells[1, idxStatus].Value = "Status";
            package.Workbook.Worksheets[1].Cells[1, idxDetalhes].Value = "Detalhes";

            while (rowIndex < rowCount)
            {
                ITransaction transaction = null;
                try
                {
                    rowIndex++;

                    var divisao = worksheet.Cells[rowIndex, idxDivisao].Value?.ToString().ToUpper().Trim();
                    if (!divisao.HasValue())
                    {
                        LogError(ref worksheet, ref importTask, rowIndex,
                            string.Format(GlobalMessages.Import_AtributoNaoInformado, GlobalMessages.Divisao));
                        continue;
                    }

                    if (divisao.HasValue() && divisao.Length > MaxSizeDivisao)
                    {
                        LogError(ref worksheet, ref importTask, rowIndex,
                            string.Format(GlobalMessages.Import_AtributoInvalido, GlobalMessages.Divisao));
                        continue;
                    }

                    var empreendimento = _empreendimentoRepository.BuscarPorDivisao(divisao);
                    var enderecoEmpreendimento = new EnderecoEmpreendimento();
                    if (empreendimento == null)
                    {
                        empreendimento = new Empreendimento();
                    }
                    else
                    {
                        _session.Evict(empreendimento);
                        enderecoEmpreendimento =
                            _enderecoEmpreendimentoRepository.FindByEmpreendimento(empreendimento.Id);
                        if (enderecoEmpreendimento == null)
                        {
                            enderecoEmpreendimento = new EnderecoEmpreendimento();
                        }
                        else
                        {
                            _session.Evict(enderecoEmpreendimento);
                        }
                    }

                    #region Get Values

                    empreendimento.Divisao = divisao;
                    empreendimento.Nome = worksheet.Cells[rowIndex, idxNome].Value?.ToString();
                    empreendimento.Regional = worksheet.Cells[rowIndex, idxRegional].Value?.ToString();
                    empreendimento.RegionalObjeto = regionaisList.Where(x => x.Nome == empreendimento.Regional).SingleOrDefault();
                    empreendimento.CodigoEmpresa = worksheet.Cells[rowIndex, idxCodigoEmpresa].Value?.ToString();
                    empreendimento.NomeEmpresa = worksheet.Cells[rowIndex, idxNomeEmpresa].Value?.ToString();
                    empreendimento.CNPJ = worksheet.Cells[rowIndex, idxCnpj].Value?.ToString()?.OnlyNumber();
                    empreendimento.RegistroIncorporacao =
                        worksheet.Cells[rowIndex, idxRegistroIncorporacao].Value?.ToString();
                    var dataLancamento = worksheet.Cells[rowIndex, idxDataLancamento].Value;
                    var dataEntrega = worksheet.Cells[rowIndex, idxDataEntrega].Value;
                    var previsaoEntrega = worksheet.Cells[rowIndex, idxPrevisaoEntrega].Value;
                    empreendimento.Mancha = worksheet.Cells[rowIndex, idxMancha].Value?.ToString();
                    empreendimento.ModalidadeComissao = empreendimento.ModalidadeComissao.HasValue() ? empreendimento.ModalidadeComissao : TipoModalidadeComissao.Fixa;
                    empreendimento.ModalidadeProgramaFidelidade = empreendimento.ModalidadeProgramaFidelidade.HasValue() ? empreendimento.ModalidadeProgramaFidelidade : TipoModalidadeProgramaFidelidade.Fixa;
                    enderecoEmpreendimento.Cep = worksheet.Cells[rowIndex, idxCep].Value?.ToString()?.OnlyNumber();
                    enderecoEmpreendimento.Logradouro = worksheet.Cells[rowIndex, idxLogradouro].Value?.ToString();
                    enderecoEmpreendimento.Numero = worksheet.Cells[rowIndex, idxNumero].Value?.ToString();
                    enderecoEmpreendimento.Complemento = worksheet.Cells[rowIndex, idxComplemento].Value?.ToString();
                    enderecoEmpreendimento.Bairro = worksheet.Cells[rowIndex, idxBairro].Value?.ToString();
                    enderecoEmpreendimento.Cidade = worksheet.Cells[rowIndex, idxCidade].Value?.ToString();
                    enderecoEmpreendimento.Estado = worksheet.Cells[rowIndex, idxEstado].Value?.ToString();

                    #endregion

                    #region Validate Values

                    if (!empreendimento.Nome.HasValue())
                    {
                        LogError(ref worksheet, ref importTask, rowIndex,
                            string.Format(GlobalMessages.Import_AtributoNaoInformado,
                                GlobalMessages.NomeEmpreendimento));
                        continue;
                    }

                    if (empreendimento.Nome.HasValue() && empreendimento.Nome.Length > MaxSizeNome)
                    {
                        LogError(ref worksheet, ref importTask, rowIndex,
                            string.Format(GlobalMessages.Import_AtributoInvalido, GlobalMessages.NomeEmpreendimento));
                        continue;
                    }

                    if (!empreendimento.Regional.HasValue())
                    {
                        LogError(ref worksheet, ref importTask, rowIndex,
                            string.Format(GlobalMessages.Import_AtributoNaoInformado, GlobalMessages.Regional));
                        continue;
                    }

                    if (empreendimento.Regional.HasValue() && empreendimento.RegionalObjeto.Nome.Length > MaxSizeRegional)
                    {
                        LogError(ref worksheet, ref importTask, rowIndex,
                            string.Format(GlobalMessages.Import_AtributoInvalido, GlobalMessages.Regional));
                        continue;
                    }

                    if (!empreendimento.CodigoEmpresa.HasValue())
                    {
                        LogError(ref worksheet, ref importTask, rowIndex,
                            string.Format(GlobalMessages.Import_AtributoNaoInformado, GlobalMessages.CodigoEmpresa));
                        continue;
                    }

                    if (empreendimento.CodigoEmpresa.HasValue() &&
                        empreendimento.CodigoEmpresa.Length > MaxSizeCodEmpresa)
                    {
                        LogError(ref worksheet, ref importTask, rowIndex,
                            string.Format(GlobalMessages.Import_AtributoInvalido, GlobalMessages.CodigoEmpresa));
                        continue;
                    }

                    if (empreendimento.NomeEmpresa.HasValue() &&
                        empreendimento.NomeEmpresa.Length > MaxSizeNomeEmpresa)
                    {
                        LogError(ref worksheet, ref importTask, rowIndex,
                            string.Format(GlobalMessages.Import_AtributoInvalido, GlobalMessages.NomeEmpresa));
                        continue;
                    }

                    if (empreendimento.CNPJ.HasValue() && empreendimento.CNPJ.Length > MaxSizeCnpj)
                    {
                        LogError(ref worksheet, ref importTask, rowIndex,
                            string.Format(GlobalMessages.Import_AtributoInvalido, GlobalMessages.Cnpj));
                        continue;
                    }

                    if (empreendimento.RegistroIncorporacao.HasValue() && empreendimento.RegistroIncorporacao.Length >
                        MaxSizeRegistroIncorporacao)
                    {
                        LogError(ref worksheet, ref importTask, rowIndex,
                            string.Format(GlobalMessages.Import_AtributoInvalido,
                                GlobalMessages.RegistroIncorporacao));
                        continue;
                    }

                    if (dataLancamento.HasValue())
                    {
                        long dataLancamentoAux = 0;
                        if (dataLancamento is DateTime)
                        {
                            empreendimento.DataLancamento = (DateTime)dataLancamento;
                        }
                        else if (dataLancamento is long ||
                                 long.TryParse(dataLancamento.ToString(), out dataLancamentoAux))
                        {
                            empreendimento.DataLancamento = DateTime.FromOADate(dataLancamentoAux);
                        }
                        else
                        {
                            LogError(ref worksheet, ref importTask, rowIndex,
                                string.Format(GlobalMessages.Import_AtributoInvalido, GlobalMessages.DataLancamento));
                            continue;
                        }
                    }

                    if (dataEntrega.HasValue())
                    {
                        long dataEntregaAux = 0;
                        if (dataEntrega is DateTime)
                        {
                            empreendimento.DataEntrega = (DateTime)dataEntrega;
                        }
                        else if (dataEntrega is long ||
                                 long.TryParse(dataEntrega.ToString(), out dataEntregaAux))
                        {
                            empreendimento.DataEntrega = DateTime.FromOADate(dataEntregaAux);
                        }
                        else
                        {
                            LogError(ref worksheet, ref importTask, rowIndex,
                                string.Format(GlobalMessages.Import_AtributoInvalido, GlobalMessages.DataEntrega));
                            continue;
                        }
                    }

                    if (previsaoEntrega.HasValue())
                    {
                        long previsaoEntregaAux = 0;
                        if (previsaoEntrega is DateTime)
                        {
                            empreendimento.PrevisaoEntrega = (DateTime)previsaoEntrega;
                        }
                        else if (previsaoEntrega is long ||
                                 long.TryParse(previsaoEntrega.ToString(), out previsaoEntregaAux))
                        {
                            empreendimento.PrevisaoEntrega = DateTime.FromOADate(previsaoEntregaAux);
                        }
                        else
                        {
                            LogError(ref worksheet, ref importTask, rowIndex,
                                string.Format(GlobalMessages.Import_AtributoInvalido, GlobalMessages.PrevisaoEntrega));
                            continue;
                        }
                    }

                    if (empreendimento.Mancha.HasValue() && empreendimento.Mancha.Length > MaxSizeMancha)
                    {
                        LogError(ref worksheet, ref importTask, rowIndex,
                            string.Format(GlobalMessages.Import_AtributoInvalido, GlobalMessages.Mancha));
                        continue;
                    }

                    if (!enderecoEmpreendimento.Cep.HasValue())
                    {
                        LogError(ref worksheet, ref importTask, rowIndex,
                            string.Format(GlobalMessages.Import_AtributoNaoInformado,
                                GlobalMessages.CEP));
                        continue;
                    }

                    if (!enderecoEmpreendimento.Logradouro.HasValue())
                    {
                        LogError(ref worksheet, ref importTask, rowIndex,
                            string.Format(GlobalMessages.Import_AtributoNaoInformado,
                                GlobalMessages.Logradouro));
                        continue;
                    }

                    if (!enderecoEmpreendimento.Numero.HasValue())
                    {
                        LogError(ref worksheet, ref importTask, rowIndex,
                            string.Format(GlobalMessages.Import_AtributoNaoInformado,
                                GlobalMessages.Numero));
                        continue;
                    }

                    if (!enderecoEmpreendimento.Bairro.HasValue())
                    {
                        LogError(ref worksheet, ref importTask, rowIndex,
                            string.Format(GlobalMessages.Import_AtributoNaoInformado,
                                GlobalMessages.Bairro));
                        continue;
                    }

                    if (!enderecoEmpreendimento.Cidade.HasValue())
                    {
                        LogError(ref worksheet, ref importTask, rowIndex,
                            string.Format(GlobalMessages.Import_AtributoNaoInformado,
                                GlobalMessages.Cidade));
                        continue;
                    }

                    if (!enderecoEmpreendimento.Estado.HasValue())
                    {
                        LogError(ref worksheet, ref importTask, rowIndex,
                            string.Format(GlobalMessages.Import_AtributoNaoInformado,
                                GlobalMessages.Estado));
                        continue;
                    }

                    #endregion

                    transaction = _session.BeginTransaction();

                    _empreendimentoRepository.Save(empreendimento);

                    enderecoEmpreendimento.Empreendimento = empreendimento;
                    _enderecoEmpreendimentoRepository.Save(enderecoEmpreendimento);

                    importTask.CurrentLine = rowIndex;

                    transaction.Commit();

                    worksheet.Cells[rowIndex, idxStatus].Value = GlobalMessages.Sucesso;
                    worksheet.Cells[rowIndex, idxDetalhes].Value = String.Format(GlobalMessages.Integracao_Sucesso,
                        GlobalMessages.Empreendimento, empreendimento.Divisao, empreendimento.Id);
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

            _session.Flush();
            _session.Close();

            // Os menos 1 é para eliminar o cabeçalho
            importTask.AppendLog(string.Format("Processados {0} registros de {1}", rowIndex - 1, rowCount - 1));
            importTask.AppendLog(string.Format("{0} registros foram processados com sucesso", importTask.SuccessCount));
            importTask.AppendLog(string.Format("Ocorreram erros na importação de {0} registros",
                importTask.ErrorCount));

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
                    "Atenção! Ocorreram erros ao importar um mais registros. Faça o Download do arquivo de retorno para avaliar os problemas encontrados");
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