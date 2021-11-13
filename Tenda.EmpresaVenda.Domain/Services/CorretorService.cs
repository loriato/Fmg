using Europa.Commons;
using Europa.Data;
using Europa.Extensions;
using Europa.Resources;
using NHibernate;
using NHibernate.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tenda.Domain.Core.Models;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Models;
using Tenda.Domain.Security.Repository;
using Tenda.Domain.Security.Services;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.Commons;
using Tenda.EmpresaVenda.Domain.Integration.Suat.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Repository.Models;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class CorretorService : BaseService
    {
        private UsuarioPortalRepository _usuarioPortalRepository { get; set; }
        private CorretorRepository _corretorRepository { get; set; }
        private EnderecoCorretorRepository _enderecoCorretorRepository { get; set; }
        private UsuarioPortalService _usuarioPortalService { get; set; }
        private SistemaRepository _sistemaRepository { get; set; }
        public SistemaService _sistemaService { get; set; }
        private UsuarioPerfilSistemaRepository _usuarioPerfilSistemaRepository { get; set; }
        private ViewCorretorRepository _viewCorretorRepository { get; set; }

        public Corretor SalvarViaEmpresaVenda(Corretor corretor, BusinessRuleException bre)
        {
            return Salvar(corretor, true, bre);
        }

        public Corretor Salvar(Corretor corretor, BusinessRuleException bre)
        {
            return Salvar(corretor, false, bre);
        }


        private Corretor Salvar(Corretor corretor, bool criadoViaEmpresaVenda, BusinessRuleException bre)
        {
            var novoCorretor = corretor.Id == 0;

            // Remove máscaras
            corretor.CNPJ = corretor.CNPJ.OnlyNumber();
            corretor.CPF = corretor.CPF.OnlyNumber();
            corretor.Telefone = corretor.Telefone.OnlyNumber();

            // Realiza as validações de Corretor
            var corrResult = new CorretorValidator(_corretorRepository).Validate(corretor);

            // Verifica se retornou algum erro
            bre.WithFluentValidation(corrResult);

            if (corrResult.IsValid)
            {
                corretor = ConsolidarUsuarioPortalCorretor(corretor, criadoViaEmpresaVenda);
                var usuaResult = new UsuarioPortalValidator(_usuarioPortalRepository).Validate(corretor.Usuario);
                bre.WithFluentValidation(usuaResult);

                if (usuaResult.IsValid)
                {
                    _usuarioPortalRepository.Save(corretor.Usuario);

                    if (novoCorretor)
                    {
                        var parametroSistema = _sistemaService.FindByCodigoSistema(ProjectProperties.CodigoEmpresaVendaPortal);

                        var ups = new UsuarioPerfilSistema
                        {
                            Sistema = parametroSistema.Sistema,
                            Usuario = corretor.Usuario,
                            Perfil = new Perfil
                            {
                                Id = ProjectProperties.IdPerfilDiretorPortalEvs
                            }
                        };
                        _usuarioPerfilSistemaRepository.Save(ups);

                        CriarTokenAtivacaoEEnviarEmail(corretor);
                    }

                    _corretorRepository.Save(corretor);
                    if (corretor.Codigo.IsEmpty() || corretor.Codigo == "000000")
                    {
                        corretor.Codigo = corretor.Id.ToString().PadLeft(6, '0');
                        _corretorRepository.Save(corretor);
                    }
                }
            }
            return corretor;
        }

        public string CriarTokenAtivacao(Corretor corretor)
        {
            corretor.Usuario.TokenAtivacao = HashUtil.SHA1(corretor.Usuario.Id + DateTime.Now.ToString() + Guid.NewGuid());
            corretor.Usuario.DataAtivacaoToken = DateTime.Now;
            _usuarioPortalRepository.Save(corretor.Usuario);

            return ProjectProperties.EvsBaseUrl + ProjectProperties.LinkAtivacaoUsuarioLogado + "?tokenAtivacao=" + corretor.Usuario.TokenAtivacao;
        }

        public string CriarTokenAtivacaoEmail(Corretor corretor)
        {
            corretor.Usuario.TokenAtivacao = HashUtil.SHA1(corretor.Usuario.Id + DateTime.Now.ToString() + Guid.NewGuid());
            corretor.Usuario.DataAtivacaoToken = DateTime.Now;
            _usuarioPortalRepository.Save(corretor.Usuario);

            return ProjectProperties.EvsBaseUrl + ProjectProperties.LinkAtivacaoUsuario + "?tokenAtivacao=" + corretor.Usuario.TokenAtivacao;
        }

        public void CriarTokenAtivacaoEEnviarEmail(Corretor corretor)
        {
            var linkAtivacao = CriarTokenAtivacaoEmail(corretor);

            GenericFileLogUtil.DevLogWithDateOnBegin(string.Format("Preparando E-mail de ativação enviado para {0}, no e-mail {1}. O link de ativação é {2}", corretor.Nome, corretor.Email, linkAtivacao));

            EnviarEmailAtivacaoCadastro(corretor.Email, corretor.Nome, linkAtivacao, corretor.Usuario.DataAtivacaoToken.Value.AddDays(1));

            GenericFileLogUtil.DevLogWithDateOnBegin(string.Format("E-mail de ativação enviado para {0}, no e-mail {1}. O link de ativação é {2}", corretor.Nome, corretor.Email, linkAtivacao));
        }

        public void EnviarEmailAtivacaoCadastro(string emailDestino, string nome, string token, DateTime dataExpi)
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
            toReplace.Add("linkAtivacao", token);
            toReplace.Add("dataExpiracao", dataExpi.ToDateTime());
            var templateName = "esquecimento-senha.html";
            var corpoEmail = TemplateEmailFactory.ResolveTemplateWithReplace(templateName, toReplace);

            var email = EmailService.CriarEmail(emailDestino, "[Tenda] Portal de Corretores - Acesso", corpoEmail);
            EmailService.EnviarEmail(email);
        }
        //Email de Envio para esquecimento de senha
        public void CriarTokenAtivacaoEEnviarEmailSenha(Corretor corretor)
        {
            var linkAtivacao = CriarTokenAtivacaoEmail(corretor);

            GenericFileLogUtil.DevLogWithDateOnBegin(string.Format("Preparando E-mail de ativação enviado para {0}, no e-mail {1}. O link de ativação é {2}", corretor.Nome, corretor.Email, linkAtivacao));

            EnviarEmailAtivacaoCadastroSenha(corretor.Email, corretor.Nome, linkAtivacao, corretor.Usuario.DataAtivacaoToken.Value.AddDays(1));

            GenericFileLogUtil.DevLogWithDateOnBegin(string.Format("E-mail de ativação enviado para {0}, no e-mail {1}. O link de ativação é {2}", corretor.Nome, corretor.Email, linkAtivacao));
        }

        public void EnviarEmailAtivacaoCadastroSenha(string emailDestino, string nome, string token, DateTime dataExpi)
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
            toReplace.Add("linkAtivacao", token);
            toReplace.Add("dataExpiracao", dataExpi.ToDateTime());
            var templateName = "nova-senha.html";
            var corpoEmail = TemplateEmailFactory.ResolveTemplateWithReplace(templateName, toReplace);

            var email = EmailService.CriarEmail(emailDestino, "[Tenda] Portal de Corretores - Acesso", corpoEmail);
            EmailService.EnviarEmail(email);
        }


        public IQueryable<Corretor> Listar(DataSourceRequest request)
        {
            var results = _corretorRepository.Listar();

            if (request.HasValue() && request.filter.FirstOrDefault() != null)
            {
                string filtroNome = request.filter.FirstOrDefault(reg => reg.column.ToLower() == "nome").column.ToLower();
                var filtroEmpresaVenda = request.filter.FirstOrDefault(reg => reg.column.ToLower() == "idempresavenda");
                if (filtroNome.Equals("nome"))
                {
                    string queryTerm = request.filter.FirstOrDefault().value.ToLower();
                    results = results.Where(x => x.Nome.ToLower().Contains(queryTerm));
                }
                if (filtroEmpresaVenda.HasValue() && filtroEmpresaVenda.column.ToLower().Equals("idempresavenda"))
                {
                    long queryTerm = 0;
                    if (long.TryParse(filtroEmpresaVenda.value, out queryTerm))
                    {
                        results = results.Where(x => x.EmpresaVenda.Id == queryTerm);
                    }
                }
            }

            return results;
        }

        public void ExcluirPorId(long idCorretor)
        {
            var exc = new BusinessRuleException();
            var reg = _corretorRepository.FindById(idCorretor);
            var endereco = _enderecoCorretorRepository.FindByCorretor(idCorretor);

            try
            {
                // Endereço corretor deixou de estar no cadastro
                if (endereco != null)
                {
                    _enderecoCorretorRepository.Delete(endereco);
                }
                _corretorRepository.Delete(reg);
            }
            catch (GenericADOException exp)
            {
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(exp))
                {
                    exc.AddError(GlobalMessages.RemovidoSemSucesso).WithParam(reg.ChaveCandidata()).Complete();
                }
            }
            exc.ThrowIfHasError();
        }

        private Corretor ConsolidarUsuarioPortalCorretor(Corretor corretor, bool criadoViaEmpresaVenda)
        {
            if (corretor.Usuario == null || corretor.Usuario.Id.IsEmpty())
            {
                corretor.Usuario = new UsuarioPortal();
                corretor.Usuario.Situacao = SituacaoUsuario.Ativo;
                if (criadoViaEmpresaVenda)
                {
                    corretor.Funcao = TipoFuncao.Diretor;
                }
            }
            else
            {
                corretor.Usuario = _usuarioPortalRepository.FindById(corretor.Usuario.Id);
            }
            corretor.Usuario.Login = corretor.Email;
            corretor.Usuario.Nome = corretor.Nome;
            if (corretor.Apelido.IsEmpty())
            {
                corretor.Apelido = corretor.Nome.IsEmpty() ? "" : corretor.Nome.Split(' ').First();
            }
            corretor.Usuario.Email = corretor.Email;
            if (corretor.DataCredenciamento.IsEmpty())
            {
                corretor.DataCredenciamento = DateTime.Now;
            }
            return corretor;
        }

        public int AtivarEmLote(long usuarioAlteracao, List<long> registros)
        {
            return AlterarSituacaoEmLote(usuarioAlteracao, Situacao.Ativo, registros);
        }

        public int SuspenderEmLote(long usuarioAlteracao, List<long> registros)
        {
            return AlterarSituacaoEmLote(usuarioAlteracao, Situacao.Suspenso, registros);
        }

        public int CancelarEmLote(long usuarioAlteracao, List<long> registros)
        {
            return AlterarSituacaoEmLote(usuarioAlteracao, Situacao.Cancelado, registros);
        }

        public int AlterarSituacaoEmLote(long usuarioAlteracao, Situacao situacao, List<long> registros)
        {
            var idUsuariosDosCorretores = _corretorRepository.Queryable()
                .Where(reg => registros.Contains(reg.Id))
                .Select(reg => reg.Usuario.Id)
                .ToList();

            var updateQuery = new StringBuilder();
            updateQuery.Append(" UPDATE UsuarioPortal cor ");
            updateQuery.Append(" SET cor.AtualizadoPor = :atualizadoPor ");
            updateQuery.Append(" , cor.AtualizadoEm = :atualizadoEm ");
            updateQuery.Append(" , cor.Situacao = :situacao ");
            updateQuery.Append(" WHERE cor.Situacao != :situacaoCancelado ");
            updateQuery.Append(" AND cor.Situacao != :situacao ");
            updateQuery.Append(" AND cor.Id in (:registros) ");

            IQuery query = Session.CreateQuery(updateQuery.ToString());
            query.SetParameter("atualizadoPor", usuarioAlteracao);
            query.SetParameter("atualizadoEm", DateTime.Now);
            query.SetParameter("situacao", situacao);
            query.SetParameter("situacaoCancelado", Situacao.Cancelado);
            query.SetParameterList("registros", idUsuariosDosCorretores);

            var updates = query.ExecuteUpdate();

            return updates;
        }

        private static string ResolveServerUrl(string serverUrl, bool forceHttps)
        {
            if (serverUrl.IndexOf("://") > -1)
                return serverUrl;

            string newUrl = serverUrl;
            Uri originalUri = System.Web.HttpContext.Current.Request.Url;
            newUrl = (forceHttps ? "https" : originalUri.Scheme) +
                "://" + originalUri.Authority + newUrl;
            return newUrl;
        }

        public Corretor SalvarComPerfil(CorretorDTO corretorDTO, BusinessRuleException bre)
        {
            if (corretorDTO.Perfis.IsEmpty())
            {
                bre.AddError(GlobalMessages.MsgErroPerfilDelete).AddField(GlobalMessages.Perfil).Complete();
            }

            var corretor = Salvar(corretorDTO.NovoCorretor, false, bre);

            bre.ThrowIfHasError();

            var idUsuario = corretor.Usuario.Id;

            var idSistema = corretorDTO.IdSistema;
            Sistema sistema = _sistemaRepository.FindById(idSistema);

            foreach (long idPerfil in corretorDTO.Perfis)
            {
                var obj = _usuarioPortalService.IncluirPerfil(idUsuario, idPerfil, sistema.Codigo);
            }

            return corretor;
        }

        public bool VerificarCorretoresEmLote(List<Corretor> corretores)
        {
            var passouPorTodos = true;
            if (corretores.HasValue())
            {
                foreach (var corretor in corretores)
                {
                    var antigosCorretores = _corretorRepository.ListarCorretoresCpf(corretor.CPF);
                    foreach (var antigoCorretor in antigosCorretores)
                    {
                        if (antigoCorretor.Usuario.Situacao.Equals(SituacaoUsuario.Ativo))
                        {
                            passouPorTodos = false;
                            break;
                        }
                    }
                }
                return passouPorTodos;
            }
            return false;
        }

        public Corretor SalvarPreCadastro(Corretor corretor, BusinessRuleException bre)
        {
            corretor.CNPJ = corretor.CNPJ.OnlyNumber();
            corretor.CPF = corretor.CPF.OnlyNumber();
            corretor.Telefone = corretor.Telefone.OnlyNumber();

            var corrResult = new PreCadastroCorretorValidator(_corretorRepository).Validate(corretor);
            bre.WithFluentValidation(corrResult);
            bre.ThrowIfHasError();

            if (corretor.Codigo.IsEmpty() || corretor.Codigo == "000000")
            {
                corretor.Codigo = corretor.Id.ToString().PadLeft(6, '0');
            }
            _corretorRepository.Save(corretor);

            return corretor;
        }

        public Corretor SalvarViaNovaEv(Corretor corretor, BusinessRuleException bre)
        {
            bool criadoViaEmpresaVenda = true;

            // Remove máscaras
            corretor.CNPJ = corretor.CNPJ.OnlyNumber();
            corretor.CPF = corretor.CPF.OnlyNumber();
            corretor.Telefone = corretor.Telefone.OnlyNumber();

            // Realiza as validações de Corretor
            var corrResult = new CorretorValidator(_corretorRepository).Validate(corretor);

            // Verifica se retornou algum erro
            bre.WithFluentValidation(corrResult);
            bre.ThrowIfHasError();

            corretor = ConsolidarUsuarioPortalCorretor(corretor, criadoViaEmpresaVenda);
            var usuaResult = new UsuarioPortalValidator(_usuarioPortalRepository).Validate(corretor.Usuario);
            bre.WithFluentValidation(usuaResult);

            if (usuaResult.IsValid)
            {
                _usuarioPortalRepository.Save(corretor.Usuario);

                var parametroSistema = _sistemaService.FindByCodigoSistema(ProjectProperties.CodigoEmpresaVendaPortal);

                var ups = new UsuarioPerfilSistema
                {
                    Sistema = parametroSistema.Sistema,
                    Usuario = corretor.Usuario,
                    Perfil = new Perfil
                    {
                        Id = ProjectProperties.IdPerfilDiretorPortalEvs
                    }
                };
                _usuarioPerfilSistemaRepository.Save(ups);

                CriarTokenAtivacaoEEnviarEmail(corretor);

                if (corretor.Codigo.IsEmpty() || corretor.Codigo == "000000")
                {
                    corretor.Codigo = corretor.Id.ToString().PadLeft(6, '0');
                }
                _corretorRepository.Save(corretor);
            }
            return corretor;
        }

        public void EnviarEmailRegraAtivaSemAceite(string emailDestino, string nome, string token, DateTime dataExpi)
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
            toReplace.Add("linkAtivacao", token);
            toReplace.Add("dataExpiracao", dataExpi.ToDateTime());
            var templateName = "nova-senha.html";
            var corpoEmail = TemplateEmailFactory.ResolveTemplateWithReplace(templateName, toReplace);

            var email = EmailService.CriarEmail(emailDestino, "[Tenda] Portal de Corretores - Acesso", corpoEmail);
            EmailService.EnviarEmail(email);
        }

        public byte[] Exportar(DataSourceRequest request, FiltroCorretorDTO filtro)
        {
            var results = _viewCorretorRepository.Listar(filtro).ToDataRequest(request);

            ExcelUtil excel = ExcelUtil.NewInstance(23)
                .NewSheet(DateTime.Now.ToString("yyyyMMddHHmmss"))
                .WithHeader(GetHeader());

            foreach (var model in results.records.ToList())
            {
                excel
                    .CreateCellValue(model.NomeEmpresaVenda).Width(20)
                    .CreateCellValue(model.Nome).Width(20)
                    .CreateCellValue(model.Apelido).Width(20)
                    .CreateCellValue(model.Cpf.ToCPFFormat()).Width(20)
                    .CreateCellValue(model.Creci).Width(20)
                    .CreateCellValue(model.Telefone.ToPhoneFormat()).Width(20)
                    .CreateCellValue(model.Email).Width(20)
                    .CreateCellValue(model.Funcao.AsString()).Width(20)
                    .CreateDateTimeCell(model.DataCredenciamento).Width(20)
                    .CreateCellValue(model.Situacao.AsString()).Width(20)
                    .CreateCellValue(model.Perfis).Width(20)
                    .CreateDateTimeCell(model.CriadoEm).Width(20);
            }
            excel.Close();
            return excel.DownloadFile();
        }

        private string[] GetHeader()
        {
            IList<string> header = new List<string>
            {
                GlobalMessages.EmpresaVenda,
                GlobalMessages.Nome,
                GlobalMessages.NomeGuerra,
                GlobalMessages.Cpf,
                GlobalMessages.Creci,
                GlobalMessages.Telefone,
                GlobalMessages.Email,
                GlobalMessages.Funcao,
                GlobalMessages.DataParceria,
                GlobalMessages.Situacao,
                GlobalMessages.Perfil,
                GlobalMessages.DataCriacao
            };
            return header.ToArray();
        }
    }
}
