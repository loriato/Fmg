using Europa.Commons;
using Europa.Extensions;
using System;
using System.IO;
using System.Net.Mail;
using System.Text;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class FilaEmailService : BaseService
    {
        public FilaEmailRepository _filaEmailRepository { get; set; }
        public FilaEmailValidator _filaEmailValidator { get; set; }
        public ArquivoRepository _arquivoRepository { get; set; }

        public FilaEmail PushEmail(FilaEmail email)
        {
            var bre = new BusinessRuleException();

            var validate = _filaEmailValidator.Validate(email);
            bre.WithFluentValidation(validate);
            bre.ThrowIfHasError();

            _filaEmailRepository.Save(email);

            return email;
        }

        public MailMessage CriarEmailMessage(FilaEmail email)
        {
            var objEmail = new MailMessage
            {
                Sender = new MailAddress(ProjectProperties.EmailConfigurations.EmailParaEnvio, string.Empty),
                From = new MailAddress(ProjectProperties.EmailConfigurations.EmailParaEnvio, string.Empty)
            };

            if (ProjectProperties.EmailConfigurations.FlagEnviarEmail)
            {
                var destinatarios = email.Destinatario.Split(';');
                foreach (var destinatario in destinatarios)
                {
                    objEmail.To.Add(destinatario);
                }
            }
            else
            {
                var emailsDev = ProjectProperties.EmailConfigurations.EmailEnvioDevMode.Split(';');
                foreach (var emailDev in emailsDev)
                {
                    objEmail.To.Add(emailDev);
                }
            }

            if (email.IdAnexos.HasValue())
            {
                string[] idsArquivo = email.IdAnexos.Split(';');
                foreach (string id in idsArquivo)
                {
                    var file = _arquivoRepository.FindById(Convert.ToInt64(id));
                    var contents = new MemoryStream(file.Content);
                    var attach = new Attachment(contents, file.Nome);
                    objEmail.Attachments.Add(attach);
                }
            }


            objEmail.Priority = MailPriority.High;
            objEmail.IsBodyHtml = true;
            objEmail.Subject = email.Titulo;
            objEmail.Body = email.Mensagem;
            objEmail.SubjectEncoding = Encoding.UTF8;
            objEmail.BodyEncoding = Encoding.UTF8;
            objEmail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            return objEmail;
        }
    }
}
