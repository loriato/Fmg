using Europa.Extensions;
using Europa.Resources;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services.Models;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class ConsultaAceiteContratoCorretagemService : BaseService
    {
        private ConsultaAceitesContratosCorretagemRepository _consultaAceitesContratosCorretagemRepository { get; set; }

        private ContratoCorretagemRepository _contratoCorretagemRepository { get; set; }

        public object GlobalMessage { get; private set; }

        public DataSourceResponse<ViewConsultaAceitesContratosCorretagem> Listar(DataSourceRequest request, FiltroConsultaAceitesContratosCorretagemDTO filtro)
        {
            return _consultaAceitesContratosCorretagemRepository.Consultar(request, filtro);
        }

        public byte[] ExportarConsulta(DataSourceRequest request, FiltroConsultaAceitesContratosCorretagemDTO filtro)
        {
            var consulta = _consultaAceitesContratosCorretagemRepository.Consultar(request, filtro).records.ToList();

            ExcelUtil excel = ExcelUtil.NewInstance(63)
                .NewSheet(DateTime.Now.ToString())
                .WithHeader(GetHeaderConsulta());

            foreach (var item in consulta)
            {
                excel
                    .CreateCellValue(item.CodigoFornecedor).Width(20)
                    .CreateCellValue(item.RazaoSocial).Width(20)
                    .CreateCellValue(item.NomeFantasia).Width(20)
                    .CreateCellValue(item.CNPJ).Width(20)
                    .CreateCellValue(item.InscricaoMunicipal).Width(20)
                    .CreateCellValue(item.CreciJuridico).Width(20)
                    .CreateCellValue(item.InscricaoEstadual).Width(20)
                    .CreateCellValue(item.LegislacaoFederal).Width(20)
                    .CreateCellValue(item.Simples).Width(20)
                    .CreateCellValue(item.SIMEI).Width(20)
                    .CreateMoneyCell(item.LucroPresumido).Width(20)
                    .CreateMoneyCell(item.LucroReal).Width(20)
                    .CreateCellValue(item.SituacaoEmpresaVenda).Width(20)
                    .CreateCellValue(item.Cep).Width(20)
                    .CreateCellValue(item.Cidade).Width(20)
                    .CreateCellValue(item.Estado).Width(20)
                    .CreateCellValue(item.Bairro).Width(20)
                    .CreateCellValue(item.Complemento).Width(20)
                    .CreateCellValue(item.Numero).Width(20)
                    .CreateCellValue(item.Logradouro).Width(20)
                    .CreateCellValue(item.NomeCorretor).Width(20)
                    .CreateCellValue(item.NomeLoja).Width(20)
                    .CreateCellValue(item.NomeResponsavelTecnico).Width(20)
                    .CreateCellValue(item.CategoriaEmpresaVenda).Width(20)
                    .CreateCellValue(item.NumeroRegistroCRECI).Width(20)
                    .CreateCellValue(item.SituacaoResponsavel).Width(20)
                    .CreateCellValue(item.TipoEmpresaVenda).Width(20)
                    .CreateCellValue(item.NomePessoaContato).Width(20)
                    .CreateCellValue(item.CorretorVisualizarClientes ? "Sim" : "Não").Width(20)
                    .CreateDateTimeCell(item.DtAceite).Width(20)
                    .CreateCellValue(item.NomeUsuario).Width(20)
                    .CreateCellValue(item.EmailUsuario).Width(20)
                    .CreateCellValue(item.LoginUsuario).Width(20)
                    .CreateCellValue(item.UsuarioTipoSituacao).Width(20)
                    .CreateCellValue(item.ContratoIdCriadoPor).Width(20)
                    .CreateCellValue(item.NomeUsuarioContratoCriadoPor).Width(20)
                    .CreateCellValue(item.NomeUsuarioContratoAtualizadoPor).Width(20)
                    .CreateDateTimeCell(item.ContratoDtAtualizadoEm).Width(20)
                    .CreateCellValue(item.ContentTypeDoubleCheck).Width(20)
                    .CreateCellValue(item.DoubleCheck).Width(20)
                    .CreateCellValue(item.CodigoToken).Width(20)
                    .CreateCellValue(item.Ativo).Width(20)
                    .CreateDateTimeCell(item.AcessoDtCriadoEm).Width(20)
                    .CreateDateTimeCell(item.InicioSessao).Width(20)
                    .CreateDateTimeCell(item.FimSessao).Width(20)
                    .CreateCellValue(item.TipoFormaEncerramento).Width(20)
                    .CreateCellValue(item.IpOrigem).Width(20)
                    .CreateCellValue(item.CodigoAutorizacao).Width(20)
                    .CreateCellValue(item.Servidor).Width(20)
                    .CreateCellValue(item.Navegador).Width(20)
                    .CreateCellValue(item.NomeSistema).Width(20)
                    .CreateCellValue(item.ArquivoIdCriadoPor).Width(20)
                    .CreateDateTimeCell(item.ArquivoDtCriadoEm).Width(20)
                    .CreateCellValue(item.ArquvioIdAtualizadoPor).Width(20)
                    .CreateDateTimeCell(item.ArquivoDtAtualizadoEm).Width(20)
                    .CreateCellValue(item.NomeArquivo).Width(20)
                    .CreateCellValue(item.CodigoHash).Width(20)
                    .CreateCellValue(item.ContentType).Width(20)
                    .CreateCellValue(item.ByThumbnail).Width(20)
                    .CreateCellValue(item.NrContentLength).Width(20)
                    .CreateCellValue(item.FileExtension).Width(20)
                    .CreateCellValue(item.Metadados).Width(20)
                    .CreateCellValue(item.FalhaExtMetadados).Width(20);



            }

            excel.Close();
            return excel.DownloadFile();
        }

        private string[] GetHeaderConsulta()
        {
            IList<string> header = new List<string> { 
            
            //    for(int i=0; i<=61; i++)
            //{
            //    header.Add(i.ToString());
            //}
            GlobalMessages.CodigoFornecedorSap,
            GlobalMessages.RazaoSocial,
            GlobalMessages.NomeFantasia,
            GlobalMessages.Cnpj,
            GlobalMessages.InscricaoMunicipal,
            GlobalMessages.CreciJuridico,
            GlobalMessages.InscricaoEstadual,
            GlobalMessages.LegislacaoFederal,
            GlobalMessages.Simples,
            GlobalMessages.Simei,
            GlobalMessages.LucroPresumido,
            GlobalMessages.LucroReal,
            "Situação da Empresa de Venda",//GlobalMessages.SituacaoEmpresaVenda,
            GlobalMessages.CEP,
            GlobalMessages.Cidade,
            GlobalMessages.Estado,
            GlobalMessages.Bairro,
            GlobalMessages.Complemento,
            GlobalMessages.Numero,
            GlobalMessages.Logradouro,
            GlobalMessages.NomeCorretor,
            GlobalMessages.Loja,
            GlobalMessages.ResponsavelTecnico,
            GlobalMessages.Categoria,
            GlobalMessages.NumeroRegistroCreci,
            "Situação do Responsável",//GlobalMessages.SituacaoResponsavel,
            GlobalMessages.Tipo,
            GlobalMessages.PessoaContato,
            GlobalMessages.CorretorVisualizarClientes,
            GlobalMessages.DataAceite,
            GlobalMessages.Usuario,
            GlobalMessages.Email,
            GlobalMessages.Login,
            "Situação do Usuário",//GlobalMessages.SituacaoUsuario,
            "Contrato Id Criado Por",
            "Nome Usuario Contrato Criado Por",
            "Nome Usuario Contrato Atualizado Por",
            "Contrato Atualizado Em",
            "Content Type Double Check",//GlobalMessages.ContentTypeDoubleCheck,
            "Double Check",//GlobalMessages.DoubleCheck,
            GlobalMessages.Token,
            GlobalMessages.Ativo,
            "Acesso Dt Criado Em",
            GlobalMessages.InicioSessao,
            GlobalMessages.FimSessao,
            GlobalMessages.FormaEncerramento,
            GlobalMessages.IpOrigem,
            "Autorização",//GlobalMessages.Autorizacao,
            GlobalMessages.Servidor,
            "Servidor",//GlobalMessages.Navegador,
            GlobalMessages.Sistema,
            "Arquivo Id Criado Por",
            "Arquivo Dt Criado Em",
            "Arquivo Id Atualizado Por",
            "Arquivo Dt Atulizado Em",
            GlobalMessages.NomeArquivo,
            GlobalMessages.HashArquivo,
            GlobalMessages.ContentType,
            "Bytes By Thumbnail",//GlobalMessages.BytesThumbnail,
            "Tamano",//GlobalMessages.Tamanho,
            GlobalMessages.Extensao,
            "Metadados",//GlobalMessages.Metadados,
            "Falha Ext Metadados",//GlobalMessages.FalhaExtMetadados
            };

            return header.ToArray();
        }

        public byte[] BaixarArquivo(long idArquivo)
        {
            var documentos = _contratoCorretagemRepository.Queryable()
                .Where(x => x.Id == idArquivo)
                .ToList();

            var outputMemStream = new MemoryStream();
            var zipOutputStream = new ZipOutputStream(outputMemStream);
            zipOutputStream.SetLevel(3); //0-9, 9 being the highest level of compression

            foreach (var doc in documentos)
            {
                var nomeArquivoMetadados = doc.Arquivo.Nome;

                var memoryStream = new MemoryStream(doc.Arquivo.Content);
                var entry = new ZipEntry(nomeArquivoMetadados);
                entry.IsUnicodeText = true;
                entry.DateTime = DateTime.Now;
                zipOutputStream.PutNextEntry(entry);
                StreamUtils.Copy(memoryStream, zipOutputStream, new byte[4096]);
                zipOutputStream.CloseEntry();

                if (doc.Arquivo.Metadados.IsEmpty() == false)
                {
                    var ms = new MemoryStream();
                    var metadadosStream = new StreamWriter(ms, new UnicodeEncoding());
                    metadadosStream.Write(doc.Arquivo.Metadados);
                    metadadosStream.Flush();
                    ms.Seek(0, SeekOrigin.Begin);

                    var metadadosEntry = new ZipEntry($"{nomeArquivoMetadados}_metadados.txt");
                    metadadosEntry.IsUnicodeText = true;
                    metadadosEntry.DateTime = DateTime.Now;
                    zipOutputStream.PutNextEntry(metadadosEntry);
                    StreamUtils.Copy(ms, zipOutputStream, new byte[4096]);
                    zipOutputStream.CloseEntry();

                    metadadosStream.Dispose();
                }
            }
            zipOutputStream.IsStreamOwner = false;
            zipOutputStream.Close();

            outputMemStream.Position = 0;

            return outputMemStream.ToArray();
        }

    }
}
