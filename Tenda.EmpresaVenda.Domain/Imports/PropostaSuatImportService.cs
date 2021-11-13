using Europa.Commons;
using Europa.Data.Model;
using Europa.Extensions;
using Europa.Resources;
using NHibernate;
using OfficeOpenXml;
using System;
using System.IO;
using System.Linq;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Shared;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services.Models;

namespace Tenda.EmpresaVenda.Domain.Imports
{
    public class PropostaSuatImportService : BaseService
    {
        private PropostaSuatRepository _propostaSuatRepository;
        private ArquivoRepository _arquivoRepository;
        private ImportacaoJunixRepository _importacaoJunixRepositoy;
        public CorretorRepository _corretorRepository;
        public NotificacaoRepository _notificacaoRepository;
        public PlanoPagamentoRepository _planoPagamentoRepository;

        private static readonly int idxCodigoCliente = 17;
        private static readonly int idxFase = 21;
        private static readonly int idxSintese = 22;//V
        private static readonly int idxObervacao = 23;
        private static readonly int idxDataRepasse = 30; //DataComissionamento
        private static readonly int idxDataRegistro = 37; //MatriculaAnexada
        private static readonly int idxStatusConformidade = 68;//LoginValidacaoAssinatura
        private static readonly int idxDataConformidade = 80; //DataConformidade CB
        private static readonly int idxProposta = 70; //PRO BR

        private static int idxStatus;
        private static int idxDetalhes;

        public void Process(ISession session, string diretorio, ref ImportTaskDTO importTask)
        {
            try
            {
                _session = session;
                FileInfo file = new FileInfo(importTask.OriginalFilePath);
                //InternalProcess(file, ref importTask);
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

        public void InternalProcess(Arquivo file, ref ImportTaskDTO importTask)
        {
            _propostaSuatRepository = new PropostaSuatRepository()
            {
                _session = _session
            };

            _arquivoRepository = new ArquivoRepository()
            {
                _session = _session
            };

            _importacaoJunixRepositoy = new ImportacaoJunixRepository()
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
            _planoPagamentoRepository = new PlanoPagamentoRepository()
            {
                _session = _session
            };
            var importacaoJunix = _importacaoJunixRepositoy.Queryable().Where(x => x.Arquivo.Id == file.Id).FirstOrDefault();

            importTask.AppendLog("Execução de serviço iniciada");

            MemoryStream ms = new MemoryStream();
            ms.Write(file.Content, 0, file.ContentLength);
            ms.Position = 0;

            ExcelPackage package = new ExcelPackage(ms);
            ExcelWorkbook workbook = package.Workbook;
            ExcelWorksheet worksheet = workbook.Worksheets.First();

            int rowCount = worksheet.Dimension.Rows;
            int columnCount = worksheet.Dimension.Columns;

            idxStatus = columnCount + 1;
            idxDetalhes = columnCount + 2;

            importTask.TotalLines = rowCount - 1;

            // O indice para acessar o Cells[row,column] é baseado em 1
            int rowIndex = 1;
            int pageSize = ProjectProperties.PageSizeRepasseJunix;
            if (pageSize.IsEmpty()) { pageSize = 50; }

            // Sobreescrevendo cabeçalho
            package.Workbook.Worksheets[1].Cells[1, idxStatus].Value = "Status";
            package.Workbook.Worksheets[1].Cells[1, idxDetalhes].Value = "Detalhes";

            PropostaSuatDTO dto = new PropostaSuatDTO();

            var propostasSuatInMemory = _propostaSuatRepository.Queryable().ToList();
            var BaseAno = ProjectProperties.AnoFiltroDataRepasse.HasValue() ? ProjectProperties.AnoFiltroDataRepasse : 2019;

            ITransaction transaction = _session.BeginTransaction();
            while (rowIndex < rowCount)
            {
                try
                {
                    rowIndex++;

                    importTask.CurrentLine = rowIndex - 1;

                    dto = new PropostaSuatDTO();
                    dto.CodigoCliente = worksheet.Cells[rowIndex, idxCodigoCliente].Value.HasValue() ? long.Parse(worksheet.Cells[rowIndex, idxCodigoCliente].Value?.ToString()) : 0;
                    dto.Fase = worksheet.Cells[rowIndex, idxFase].Value.IsEmpty() ? " " : worksheet.Cells[rowIndex, idxFase].Value?.ToString();
                    dto.Sintese = worksheet.Cells[rowIndex, idxSintese].Value.IsEmpty() ? " " : worksheet.Cells[rowIndex, idxSintese].Value.ToString();
                    dto.Observacao = worksheet.Cells[rowIndex, idxObervacao].Value.IsEmpty() ? " " : worksheet.Cells[rowIndex, idxObervacao].Value.ToString();
                    if (worksheet.Cells[rowIndex, idxDataRepasse].Value.HasValue())
                    {
                        dto.DataRepasse = DateTime.Parse(worksheet.Cells[rowIndex, idxDataRepasse].Value?.ToString());
                    }
                    if (worksheet.Cells[rowIndex, idxDataRegistro].Value.HasValue())
                    {
                        dto.DataRegistro = DateTime.Parse(worksheet.Cells[rowIndex, idxDataRegistro].Value?.ToString());

                    }
                    dto.StatusConformidade = worksheet.Cells[rowIndex, idxStatusConformidade].Value.IsEmpty() ? " " : worksheet.Cells[rowIndex, idxStatusConformidade].Value?.ToString();
                    if (worksheet.Cells[rowIndex, idxDataConformidade].Value.HasValue())
                    {
                        dto.DataConformidade = DateTime.Parse(worksheet.Cells[rowIndex, idxDataConformidade].Value?.ToString());
                    }
                    dto.Proposta = worksheet.Cells[rowIndex, idxProposta].Value.IsEmpty() ? " " : worksheet.Cells[rowIndex, idxProposta].Value.ToString();

                    // Proposta
                    if (dto.Proposta.IsEmpty())
                    {
                        LogError(ref worksheet, ref importTask, rowIndex, string.Format(GlobalMessages.Import_AtributoNaoInformado, GlobalMessages.Proposta));
                        continue;
                    }

                    // Garantindo a máscara de proposta (sanetização de dados)
                    dto.Proposta = dto.Proposta.ToUpper().Trim();

                    var proposta = propostasSuatInMemory.Where(reg => reg.CodigoProposta == dto.Proposta).FirstOrDefault();
                    if (proposta.HasValue())
                    {
                        proposta.CodigoCliente = dto.CodigoCliente.IsEmpty() ? proposta.CodigoCliente : dto.CodigoCliente;
                        proposta.Fase = dto.Fase.IsEmpty() ? proposta.Fase : dto.Fase;
                        proposta.Sintese = dto.Sintese.IsEmpty() ? proposta.Sintese : dto.Sintese;
                        proposta.Observacao = dto.Observacao.IsEmpty() ? proposta.Observacao : dto.Observacao;
                        if (!proposta.Observacao.IsEmpty() && proposta.Observacao.Length > DatabaseStandardDefinitions.FourThousandLength)
                        {
                            proposta.Observacao = proposta.Observacao.Substring(0, DatabaseStandardDefinitions.FourThousandLength - 10) + "-truncado";
                        }

                        if (proposta.DataRepasse.IsEmpty())
                        {
                            if (dto.DataRepasse.HasValue())
                            {
                                if (dto.DataRepasse.Value.Year > BaseAno)
                                {

                                    proposta.DataRepasse = DateTime.Now;
                                }
                                else
                                {
                                    proposta.DataRepasse = dto.DataRepasse;
                                }
                            }
                        }
                        //
                        proposta.DataRepasseJunix = proposta.DataRepasseJunix.IsEmpty() ? dto.DataRepasse : proposta.DataRepasseJunix;

                        proposta.StatusRepasse = proposta.DataRepasse.IsEmpty() ? "Não" : "Sim";
                        proposta.DataRegistro = dto.DataRegistro.IsEmpty() ? proposta.DataRegistro : dto.DataRegistro;
                        proposta.StatusConformidade = dto.StatusConformidade.IsEmpty() ? proposta.StatusConformidade : dto.StatusConformidade;
                        if (proposta.PreProposta.HasValue())
                        {
                            if (_planoPagamentoRepository.RegraAvalistaPreChaves(proposta.PreProposta.Id) && proposta.DataConformidade.IsEmpty())
                            {
                                proposta.DataConformidade = proposta.DataRepasse;
                            }
                            else
                            {
                                if (!proposta.DataConformidade.HasValue())
                                {
                                    if (dto.DataConformidade.HasValue() && dto.DataConformidade.Value.Year > BaseAno)
                                    {

                                        proposta.DataConformidadeJunix = dto.DataConformidade;
                                        proposta.DataConformidade = DateTime.Now;
                                    }
                                    else
                                    {
                                        proposta.DataConformidadeJunix = dto.DataConformidade;
                                        proposta.DataConformidade = dto.DataConformidade;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (!proposta.DataConformidade.HasValue())
                            {
                                if (dto.DataConformidade.HasValue() && dto.DataConformidade.Value.Year > BaseAno)
                                {

                                    proposta.DataConformidadeJunix = dto.DataConformidade;
                                    proposta.DataConformidade = DateTime.Now;
                                }
                                else
                                {
                                    proposta.DataConformidadeJunix = dto.DataConformidade;
                                    proposta.DataConformidade = dto.DataConformidade;
                                }
                            }
                        }

                    }
                    else
                    {
                        LogError(ref worksheet, ref importTask, rowIndex, string.Format(GlobalMessages.PropostaNaoEncontrada, dto.Proposta));
                        continue;
                    }

                    proposta.AtualizadoPor = ProjectProperties.IdUsuarioSistema;
                    proposta.AtualizadoEm = DateTime.Now;

                    _propostaSuatRepository.Save(proposta);

                    if (!proposta.StatusConformidade.IsEmpty() && !proposta.PreProposta.IsEmpty() && proposta.StatusConformidade.ToLower().Equals(GlobalMessages.StatusConformidade_Sem_Docs_Avalista.ToLower()))
                    {
                        EnviarNotificacao(proposta.PreProposta);
                    }

                    if (rowIndex % pageSize == 0)
                    {
                        importacaoJunix.RegistroAtual = rowIndex - 1;
                        _importacaoJunixRepositoy.Save(importacaoJunix);
                        transaction.Commit();
                        transaction = _session.BeginTransaction();
                    }

                    worksheet.Cells[rowIndex, idxStatus].Value = GlobalMessages.Sucesso;
                    worksheet.Cells[rowIndex, idxDetalhes].Value = String.Format(GlobalMessages.Integracao_Sucesso, GlobalMessages.Proposta, proposta.CodigoProposta, proposta.Id);

                    importTask.SuccessCount++;
                }

                catch (FormatException fex)
                {
                    LogError(ref worksheet, ref importTask, rowIndex, fex.Message);
                    continue;
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
            importacaoJunix.RegistroAtual = rowIndex - 1;
            _importacaoJunixRepositoy.Save(importacaoJunix);

            // Garantindo commit do Bloco
            if (transaction != null && transaction.IsActive)
            {
                transaction.Commit();
            }

            transaction = _session.BeginTransaction();

            // Os menos 1 é para eliminar o cabeçalho
            importTask.AppendLog(string.Format("Processados {0} registros de {1}", rowIndex - 1, rowCount - 1));
            importTask.AppendLog(string.Format("{0} registros foram processados com sucesso", importTask.SuccessCount));
            importTask.AppendLog(string.Format("Ocorreram erros na importação de {0} registros", importTask.ErrorCount));

            importTask.AppendLog("Persistindo arquivo de retorno");
            package.Save();
            package.Stream.Position = 0;
            ms.Close();

            using (var ms2 = new MemoryStream())
            {
                package.Stream.CopyTo(ms2);
                file.Content = ms2.ToArray();
            }
            file.Hash = HashUtil.SHA256(file.Content);

            _arquivoRepository.Save(file);

            importTask.AppendLog("Arquivo de retorno gerado com sucesso");
            importTask.AppendLog("Importação Finalizada");

            if (importTask.ErrorCount > 0)
            {
                importTask.AppendLog("Atenção! Ocorreram erros ao importar um ou mais registros. Faça o Download do arquivo de retorno para avaliar os problemas encontrados");
            }
            importTask.End = DateTime.Now;

            transaction.Commit();
        }

        private void LogError(ref ExcelWorksheet worksheet, ref ImportTaskDTO importTask, int rowIndex, string details)
        {
            importTask.IncrementError();
            worksheet.Cells[rowIndex, idxStatus].Value = GlobalMessages.Erro;
            worksheet.Cells[rowIndex, idxDetalhes].Value = details;
        }

        public void EnviarNotificacao(PreProposta preProposta)
        {
            var idEmpresaVenda = preProposta.EmpresaVenda.Id;
            var ppr = preProposta.Codigo;
            var nomeCliente = preProposta.Cliente.NomeCompleto;

            var diretores = _corretorRepository.ListarDiretoresAtivosDaEmpresaDeVendas(idEmpresaVenda);
            foreach (var corretor in diretores)
            {
                var notificacao = new Notificacao
                {
                    Titulo = string.Format(GlobalMessages.Notificacao_SemDocsAvalista_Titulo, ppr, nomeCliente),
                    Conteudo = GlobalMessages.Notificacao_SemDocsAvalista_Conteudo,
                    Usuario = corretor.Usuario,
                    EmpresaVenda = corretor.EmpresaVenda,
                    DestinoNotificacao = Tenda.Domain.EmpresaVenda.Enums.DestinoNotificacao.Portal,
                };
                _notificacaoRepository.Save(notificacao);
            }
        }
    }
}

