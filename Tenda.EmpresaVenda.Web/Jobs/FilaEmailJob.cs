using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;

namespace Tenda.EmpresaVenda.Web.Jobs
{
    public class FilaEmailJob : BaseJob
    {
        private FilaEmailRepository _filaEmailRepository { get; set; }
        private FilaEmailService _filaEmailService { get; set; }
        private ArquivoRepository _arquivoRepository { get; set; }


        protected override void Init()
        {
            _filaEmailRepository = new FilaEmailRepository();
            _filaEmailRepository._session = _session;

            _arquivoRepository = new ArquivoRepository();
            _arquivoRepository._session = _session;

            _filaEmailService = new FilaEmailService();
            _filaEmailService._session = _session;
            _filaEmailService._arquivoRepository = _arquivoRepository;

        }

        public override void Process()
        {
            var pendentes = _filaEmailRepository.BuscarEmailPendente(ProjectProperties.QuatidadeEmails);

            if (pendentes.IsEmpty())
            {
                WriteLog(TipoLog.Informacao, "Não há E-mails pendentes");
                return;
            }

            var totalEmails = pendentes.Count();
            var count = 0;

            WriteLog(TipoLog.Informacao, string.Format("Total de e-mails a serem processados: {0}", totalEmails));

            var regex = new Regex(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$");

            foreach (var email in pendentes)
            {
                var transaction = _session.BeginTransaction();
                try
                {
                    var match = regex.Match(email.Destinatario);

                    if (!match.Success)
                    {
                        throw new Exception(string.Format("E-mail inválido: {0}", email.Destinatario));
                    }

                    var novo = _filaEmailService.CriarEmailMessage(email);

                    EmailService.EnviarEmail(novo);

                    email.SituacaoEnvio = SituacaoEnvioFila.EnviadoComSucesso;
                    email.DataEnvio = DateTime.Now;

                    count++;

                }catch(Exception ex)
                {
                    email.AlterarSituacaoParaErroDeAcordoComTentativas(ex.Message);

                    WriteLog(TipoLog.Erro, ex.Message);

                }
                finally
                {

                    email.NumeroTentativas++;
                    email.AtualizadoEm = DateTime.Now;
                    _filaEmailRepository.Save(email);
                }

                if (transaction.IsActive)
                {
                    transaction.Commit();
                }
            }

            WriteLog(TipoLog.Informacao, string.Format("{0} de {1} enviados com sucesso", count, totalEmails));

        }
        
    }
}