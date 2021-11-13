using Europa.Commons;
using Europa.Resources;
using NHibernate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Shared;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;

namespace Tenda.EmpresaVenda.Web.Jobs
{
    public class SalvaArquivoRepasseJunixJob : BaseJob
    {

        private StaticResourceService _staticResourceService { get; set; }
        private ArquivoRepository _arquivoRepository { get; set; }
        private ImportacaoJunixService _importacaoJunixService { get; set; }
        private ArquivoService _arquivoService { get; set; }

        protected override void Init()
        {
            _staticResourceService = new StaticResourceService();
            _staticResourceService._session = _session;

            _arquivoRepository = new ArquivoRepository();
            _arquivoRepository._session = _session;

            _arquivoService = new ArquivoService();
            _arquivoService._session = _session;

            _importacaoJunixService = new ImportacaoJunixService();
            _importacaoJunixService._session = _session;
            _importacaoJunixService._importacaoJunixRepository = new ImportacaoJunixRepository();
            _importacaoJunixService._importacaoJunixRepository._session = _session;
        }
        public override void Process()
        {
            var arquivos = _staticResourceService.SearchFiles();

            var totalDeArquivos = arquivos.Count;

            var indiceAtual = 0;

            WriteLog(TipoLog.Informacao, String.Format(GlobalMessages.TotalASerProcessado, totalDeArquivos));

            ITransaction transaction = null;

            foreach (var arq in arquivos)
            {
                transaction = _session.BeginTransaction();
                try
                {
                    indiceAtual++;

                    var file = new Arquivo();
                    file.Nome = arq.Name;
                    file.ContentType = arq.Extension;
                    file.FileExtension = MimeMappingWrapper.GetDefaultExtension(arq.Extension);
                    file.Content = File.ReadAllBytes(arq.FullName);
                    file.ContentLength = file.Content.Length;
                    file.Hash = HashUtil.SHA256(file.Content);

                    _arquivoRepository.Save(file);
                    long rowCount = _arquivoService.TotalLinhas(file);

                    var importacao = new ImportacaoJunix()
                    {
                        CriadoPor = ProjectProperties.IdUsuarioSistema,
                        CriadoEm = DateTime.Now,
                        AtualizadoPor = ProjectProperties.IdUsuarioSistema,
                        AtualizadoEm = DateTime.Now,
                        Situacao = SituacaoArquivo.AguardandoProcessamento,
                        Origem = TipoOrigem.Robo,
                        Arquivo = file,
                        TotalRegistros = rowCount
                    };

                    _importacaoJunixService.Salvar(importacao);

                    WriteLog(TipoLog.Informacao, string.Format(GlobalMessages.ArquivoEnviadoSucesso, file.Nome));
                    WriteLog(TipoLog.Informacao, string.Format(GlobalMessages.ItensProcessados, indiceAtual, totalDeArquivos));

                    transaction.Commit();

                    arq.Delete();
                }
                catch (BusinessRuleException bre)
                {
                    // FIXME: Quando der erro, significa que uma regra obrigatória do arquivo foi violada, sendo assim
                    // o arquivo deveria ser movido para uma pasta de 'erros de processamento'.
                    // Da forma que está atualmente, mesmo dando erro, na proxima execução ele será avaliado novamente.
                    ExceptionLogger.LogException(bre);
                    transaction.Rollback();
                    foreach (var erro in bre.Errors)
                    {
                        WriteLog(TipoLog.Erro, String.Format(GlobalMessages.ErroEm, arq.Name, erro));
                    }
                }
                catch (Exception err)
                {
                    ExceptionLogger.LogException(err);
                    // FIXME: Quando der erro, significa que uma regra obrigatória do arquivo foi violada, sendo assim
                    // o arquivo deveria ser movido para uma pasta de 'erros de processamento'.
                    // Da forma que está atualmente, mesmo dando erro, na proxima execução ele será avaliado novamente.
                }
            }

        }
    }
}