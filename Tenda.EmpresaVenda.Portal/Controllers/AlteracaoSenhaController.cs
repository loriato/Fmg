using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.Core.Models;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Repository;
using Tenda.Domain.Security.Services;
using Tenda.Domain.Security.Services.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Domain.Validators;
using Tenda.EmpresaVenda.Portal.Models;
using Tenda.EmpresaVenda.Portal.Models.Application;
using Tenda.EmpresaVenda.Portal.Security;


namespace Tenda.EmpresaVenda.Portal.Controllers
{
    public class AlteracaoSenhaController : BaseController
    {
        private CorretorRepository _corretorRepository { get; set; }
        private LoginService _loginService { get; set; }


        public ActionResult Index(string tokenAtivacao)
        {
            try
            {
                var corretor = _corretorRepository.FindbyTokenAtivacao(tokenAtivacao);

                var corretorDTO = new AlteracaoSenhaCorretorDTO
                {
                    Id = corretor.Id,
                    Nome = corretor.Nome,
                    Email = corretor.Email,
                    TokenAtivacao = corretor.Usuario.TokenAtivacao
                };

                return View(corretorDTO);
            }
            //Trata excecoes de corretores nulos / corretores múltiplos pela busca de Token Ativacao. 
            //(corretores multiplos só ocorrem devido ao campo 'TokenAtivacao' estar vazio em varios usuarios).
            catch (InvalidOperationException ex)
            {
                return View("Error", new ErroDTO { Mensagem = "O token informado expirou ou não existe." });
            }
        }
        [PublicPage]
        public ActionResult Sucesso()
        {
            SessionAttributes session = SessionAttributes.Current();
            _loginService.Logout(session.Acesso);
            session.Invalidate();
            FormsAuthentication.SignOut();
            return View("Sucesso");
        }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult AlterarSenha(AlteracaoSenhaCorretorDTO corretorDTO)
        {
            var jsonResponse = new JsonResponse();
            try
            {
                var keyPair = new KeyValuePair<string, string>(key: corretorDTO.Senha, value: corretorDTO.ConfirmacaoSenha);
                var corrResult = new PasswordValidator().Validate(keyPair);

                // Verifica se retornou algum erro
                new BusinessRuleException().WithFluentValidation(corrResult).ThrowIfHasError();

                var corretor = _corretorRepository.FindById(corretorDTO.Id);
                var usuario = corretor.Usuario;
                usuario.Senha = HashUtil.SHA512(corretorDTO.Senha);
                usuario.TokenAtivacao = null;
                usuario.DataAtivacaoToken = null;
                _session.SaveOrUpdate(usuario);

                jsonResponse.Sucesso = true;
                jsonResponse.Mensagens.Add(string.Format(GlobalMessages.SenhaAlteradaComSucesso, corretor.Nome));
            }
            catch (BusinessRuleException ex)
            {
                CurrentSession().Transaction.Rollback();
                jsonResponse.Mensagens.AddRange(ex.Errors);
                jsonResponse.Campos.AddRange(ex.ErrorsFields);
            }
            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }
    }
}