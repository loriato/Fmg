using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.Core.Models;
using Tenda.Domain.Core.Services;
using Tenda.Domain.Security.Repository;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.Commons;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class RegraAtivaSemAceiteService : BaseService
    {
        public RegraComissaoEvsRepository _regraComissaoEvsRepository { get; set; }
        public AceiteRegraComissaoEvsRepository _aceiteRegraComissaoEvsRepository { get; set; }

        public List<long> EmpresasSemAceite()
        {
            var queryRegraComissao = _regraComissaoEvsRepository.Queryable();
            var queryAceite = _aceiteRegraComissaoEvsRepository.Queryable();
            var result = queryRegraComissao.Where(x => !queryAceite.Select(y => y.RegraComissaoEvs.Id).Contains(x.Id))
                                            .Where(x => x.Situacao == Tenda.Domain.EmpresaVenda.Enums.SituacaoRegraComissao.Ativo)
                                            .Select(x => x.EmpresaVenda.Id).ToList();
            return result;
        }

        public void EnviarEmail(string nome, string emailDestino)
        {
            string siteUrl = ProjectProperties.EvsBaseUrl;
            var imgFooter = siteUrl + "/static/images/template-email/footer.png";
            var imgHeader = siteUrl + "/static/images/template-email/header.png";
            var imgLeft = siteUrl + "/static/images/template-email/left.png";
            var imgRight = siteUrl + "/static/images/template-email/right.png";
            Dictionary<string, string> toReplace = new Dictionary<string, string>();
            toReplace.Add("imgFooter", imgFooter);
            toReplace.Add("imgHeader", imgHeader);
            toReplace.Add("imgLeft", imgLeft);
            toReplace.Add("imgRight", imgRight);
            toReplace.Add("nome", nome);
            toReplace.Add("link", siteUrl);
            var templateName = "regra-comissao-ativa.html";
            var corpoEmail = TemplateEmailFactory.ResolveTemplateWithReplace(templateName, toReplace);

            var email = EmailService.CriarEmail(emailDestino, "[Tenda] Portal de EV - Regra de Comissão Ativa", corpoEmail);
            EmailService.EnviarEmail(email);
        }
    }
}
