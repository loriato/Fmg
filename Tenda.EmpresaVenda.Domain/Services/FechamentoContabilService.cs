using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using NHibernate.Exceptions;
using System.Collections.Generic;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.Commons;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class FechamentoContabilService : BaseService
    {
        public FechamentoContabilRepository _fechamentoContabilRepository { get; set; }
        public EmpresaVendaRepository _empresaVendaRepositoy { get; set; }
        public FechamentoContabilValidator _fechamentoContabilValidator { get; set; }
        public CorretorRepository _corretorRepository { get; set; }
        public NotificacaoRepository _notificacaoRepository { get; set; }
        public FilaEmailService _filaEmailService { get; set; }

        public FechamentoContabil ExcluirFechamentoContabil(long idFechamento)
        {
            var bre = new BusinessRuleException();

            var fechamento = _fechamentoContabilRepository.FindById(idFechamento);

            if (fechamento.IsEmpty())
            {
                bre.AddError(string.Format(GlobalMessages.ErroRegistroInexistente, GlobalMessages.FechamentoContabil)).Complete();
            }
            bre.ThrowIfHasError();

            try
            {
                _fechamentoContabilRepository.Delete(fechamento);
            }
            catch (GenericADOException gex)
            {
                bre.AddError(gex.Message);
            }

            bre.ThrowIfHasError();

            return fechamento;
        }
        public FechamentoContabil SalvarFechamento(FechamentoContabilDto fechamentoDto)
        {
            var bre = new BusinessRuleException();

            var validate = _fechamentoContabilValidator.Validate(fechamentoDto);
            bre.WithFluentValidation(validate);
            bre.ThrowIfHasError();

            var fechamento = new FechamentoContabil();
            fechamento.InicioFechamento = fechamentoDto.InicioFechamento;
            fechamento.TerminoFechamento = fechamentoDto.TerminoFechamento;
            fechamento.Descricao = fechamentoDto.Descricao;
            fechamento.QuantidadeDiasLembrete = fechamentoDto.QuantidadeDiasLembrete;

            _fechamentoContabilRepository.Save(fechamento);

            return fechamento;
        }
        
        public void ComunicarFechamentoContabil(FechamentoContabil fechamento)
        {
            var corretores = _corretorRepository.ListarTodosDiretoresAtivos();

            foreach (var corr in corretores)
            {
                EnviarEmail(corr.Email, corr.Nome, fechamento);
                EnviarNotificacao(corr, fechamento);
            }
        }

        //Método alterado para adicionar o e-mail na fila de envio
        public void EnviarEmail(string emailDestino, string nome,FechamentoContabil fechamento)
        {
            string siteUrl = ProjectProperties.EvsBaseUrl;
            var imgHeader = siteUrl + "/static/images/template-email/header2.png";
            var logomarca = siteUrl + "/static/images/logo-tenda-tagline-rgb-vermelho.png";
            var imgLeft = siteUrl + "/static/images/template-email/left.png";
            var imgRight = siteUrl + "/static/images/template-email/right.png";
            var imgFooter = siteUrl + "/static/images/mosca-tenda-bco.png";
            
            Dictionary<string, string> toReplace = new Dictionary<string, string>();
            toReplace.Add("imgHeader", imgHeader);
            toReplace.Add("logomarca", logomarca);
            toReplace.Add("imgLeft", imgLeft);
            toReplace.Add("imgRight", imgRight);
            toReplace.Add("imgFooter", imgFooter);

            toReplace.Add("nome", nome);
            toReplace.Add("inicioFechamento", fechamento.InicioFechamento.ToDate());
            toReplace.Add("terminoFechamento", fechamento.TerminoFechamento.ToDate());

            var templateName = "comunicado-fechamento-contabil.html";
            var corpoEmail = TemplateEmailFactory.ResolveTemplateWithReplace(templateName, toReplace);

            //Criação do item fila de email
            var email = new FilaEmail();
            email.Titulo = "[Tenda] Portal de EV - Comunicado Fechamento Contábil";
            email.Destinatario = emailDestino;
            email.Mensagem = corpoEmail;
            email.SituacaoEnvio = SituacaoEnvioFila.Pendente;
            _filaEmailService.PushEmail(email);

            //removido para aplicação da fila de email
            //var email = EmailService.CriarEmail(emailDestino, "[Tenda] Portal de EV - Comunicado Fechamento Contábil", corpoEmail);
            //EmailService.EnviarEmail(email);
        }

        public void EnviarNotificacao(Corretor corretor, FechamentoContabil fechamento)
        {
            var notificacao = new Notificacao
            {
                Titulo = GlobalMessages.NotificacaoFechamentoFinanceiro_Titulo,
                Conteudo = string.Format(GlobalMessages.NotificacaoFechamentoFinanceiro_Conteudo,
                fechamento.InicioFechamento.ToDate(), fechamento.TerminoFechamento.ToDate()),
                Usuario = corretor.Usuario,
                EmpresaVenda = corretor.EmpresaVenda,
                DestinoNotificacao = DestinoNotificacao.Portal,
            };

            _notificacaoRepository.Save(notificacao);
        }
    }
}
