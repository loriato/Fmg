using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using OfficeOpenXml;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Shared;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class EnderecoFornecedorService:BaseService
    {
        private static int idxStatus = 10;
        private static int idxDetalhes = 11;

        #region Columns Index
        private static int idxCodigo = 1;
        private static int idxCnpj = 2;
        private static int idxRazaoSocial = 3;

        private static int idxLogradouro = 4;
        private static int idxNumero = 5;
        private static int idxBairro = 6;
        private static int idxCep = 7;
        private static int idxCidade = 8;
        private static int idxEstado = 9;
        #endregion

        public EnderecoFornecedorValidator _enderecoFornecedorValidator { get; set; }
        public EnderecoFornecedorRepository _enderecoFornecedorRepository { get; set; }
                
        public EnderecoFornecedor Salvar(EnderecoFornecedor enderecoFornecedor)
        {
            var bre = new BusinessRuleException();

            var validate = _enderecoFornecedorValidator.Validate(enderecoFornecedor);
            bre.WithFluentValidation(validate);
            bre.ThrowIfHasError();

            enderecoFornecedor.Cep = enderecoFornecedor.Cep.OnlyNumber();
            enderecoFornecedor.Cnpj = enderecoFornecedor.Cnpj.OnlyNumber();
            _enderecoFornecedorRepository.Save(enderecoFornecedor);

            return enderecoFornecedor;
        }

        public ImportTaskDTO UploadEnderecoFornecedor(HttpPostedFileBase file)
        {
            var importTask = new ImportTaskDTO();
            var bre = new BusinessRuleException();

            try
            {
                ExcelPackage package = new ExcelPackage(file.InputStream);
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

                while (rowIndex < rowCount)
                {
                    try
                    {
                        rowIndex++;

                        var codigo = worksheet.Cells[rowIndex, idxCodigo].Value?.ToString();
                        var estado = worksheet.Cells[rowIndex, idxEstado].Value?.ToString();

                        if (estado.IsEmpty())
                        {
                            bre.AddError(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Estado)).Complete();
                            bre.ThrowIfHasError();
                        }

                        var enderecoFornecedor = _enderecoFornecedorRepository.BuscarEnderecoPorCodigoERegional(codigo, estado);

                        if (enderecoFornecedor.IsEmpty())
                        {
                            enderecoFornecedor = new EnderecoFornecedor();
                        }

                        #region Dados do Fornecedor
                        enderecoFornecedor.CodigoFornecedor = codigo;
                        enderecoFornecedor.Cnpj = worksheet.Cells[rowIndex, idxCnpj].Value?.ToString();
                        enderecoFornecedor.RazaoSocial = worksheet.Cells[rowIndex, idxRazaoSocial].Value?.ToString();
                        #endregion

                        #region Dados de Endereço
                        enderecoFornecedor.Logradouro = worksheet.Cells[rowIndex, idxLogradouro].Value?.ToString();
                        enderecoFornecedor.Numero = worksheet.Cells[rowIndex, idxNumero].Value?.ToString();
                        enderecoFornecedor.Bairro = worksheet.Cells[rowIndex, idxBairro].Value?.ToString();
                        enderecoFornecedor.Cep = worksheet.Cells[rowIndex, idxCep].Value?.ToString();
                        enderecoFornecedor.Cidade = worksheet.Cells[rowIndex, idxCidade].Value?.ToString();
                        enderecoFornecedor.Estado = estado;
                        #endregion

                        Salvar(enderecoFornecedor);

                        importTask.SuccessCount++;
                        worksheet.Cells[rowIndex, idxStatus].Value = GlobalMessages.Sucesso;
                    }
                    catch (BusinessRuleException ex)
                    {
                        var msg = "";
                        foreach (var erro in ex.Errors)
                        {
                            importTask.AppendLog(String.Format("Linha ({0}) - {1}", rowIndex, erro));
                            msg += erro + "; ";
                        }
                        worksheet.Cells[rowIndex, idxStatus].Value = GlobalMessages.Erro;
                        worksheet.Cells[rowIndex, idxDetalhes].Value = msg;
                        importTask.IncrementError();
                    }
                }

                if (importTask.ErrorCount > 0)
                {
                    package.Save();
                    package.Stream.Position = 0;
                    var targetFilePath = ProjectProperties.VirtualDirectoryFile.PhysicalPath + "erros-" + file.FileName;
                    using (FileStream saida = new FileStream(targetFilePath, FileMode.Create, FileAccess.Write))
                    {
                        package.Stream.CopyTo(saida);
                    }

                    importTask.TargetFilePath = "erros-" + file.FileName;

                }

            }
            catch(Exception ex)
            {
                importTask.Error = ex.Message;
                importTask.End = DateTime.Now;
                ExceptionLogger.LogException(ex);
            }

            return importTask;

        }

        public bool VerificarExcel(ExcelPackage package)
        {
            return true;
        }
    }
}
