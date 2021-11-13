using Europa.Commons;
using Europa.Resources;
using Europa.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Validators;
using Tenda.EmpresaVenda.Portal.Models;
using Tenda.EmpresaVenda.Portal.Security;

namespace Tenda.EmpresaVenda.Portal.Controllers
{
    public class AlteracaoSenhaEmailController : BaseController
    {
        private CorretorRepository _corretorRepository { get; set; }

        [PublicPage]
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
            return View("Sucesso");
        }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        [PublicPage]
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