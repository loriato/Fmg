using Europa.Extensions;
using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using Tenda.Domain.Shared;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public static class EmailService
    {
        private static SmtpClient DeveloperMailClient()
        {
            // cria o objeto que envia de fato o e-mail
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("fswvix.europa@geen.com.br", "hkzi eyug issn ghtn");
            return client;
        }

        private static SmtpClient TendaMailClient()
        {
            return new SmtpClient
            {
                Host = ProjectProperties.EmailConfigurations.DefaultSmtpHost,
                Port = ProjectProperties.EmailConfigurations.DefaultSmtpPort,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
            };
        }

        private static SmtpClient ConfigurarSmtp()
        {
            if (ProjectProperties.EmailConfigurations.EmailParaEnvio.IsNull() || ProjectProperties.EmailConfigurations.AutenticacaoEmailParaEnvio.IsNull())
            {
                throw new MissingFieldException("Os parametros para envio de email não estão devidamente configurados.");
            }

            if (ProjectProperties.EmailConfigurations.UseGmail)
            {
                return DeveloperMailClient();
            }
            return TendaMailClient();
        }

        public static MailMessage CriarEmail(string email, string titulo, string corpo, MailPriority prioridadeEnvio = MailPriority.High)
        {
            var objEmail = new MailMessage
            {
                Sender = new MailAddress(ProjectProperties.EmailConfigurations.EmailParaEnvio, string.Empty),
                From = new MailAddress(ProjectProperties.EmailConfigurations.EmailParaEnvio, string.Empty)
            };

            if (ProjectProperties.EmailConfigurations.FlagEnviarEmail)
            {
                if (objEmail.HasValue()) objEmail.To.Add(email);
            }
            else
            {
                var Emails = ProjectProperties.EmailConfigurations.EmailEnvioDevMode.Split(';');
                foreach (var Email in Emails)
                {
                    if (objEmail.IsEmpty()) continue;
                    objEmail.To.Add(Email);
                }
            }

            objEmail.Priority = prioridadeEnvio;
            objEmail.IsBodyHtml = true;
            objEmail.Subject = titulo;
            objEmail.Body = corpo;

            objEmail.SubjectEncoding = Encoding.UTF8;
            objEmail.BodyEncoding = Encoding.UTF8;
            objEmail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            return objEmail;
        }

        public static void EnviarEmail(MailMessage mail)
        {
            var smtpClient = ConfigurarSmtp();
            smtpClient.Send(mail);
            mail.Attachments.Dispose();
            mail.Dispose();
            smtpClient.Dispose();
        }
    }
}
