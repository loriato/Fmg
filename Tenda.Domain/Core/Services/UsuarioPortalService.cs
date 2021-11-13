using Europa.Commons;
using Europa.Commons.LDAP;
using Europa.Extensions;
using Europa.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.Core.Models;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Models;
using Tenda.Domain.Security.Repository;

namespace Tenda.Domain.Core.Services
{
    public class UsuarioPortalService : BaseService
    {
        public ViewUsuarioPerfilRepository _viewUsuarioPerfilRepository { get; set; }
        public UsuarioPortalRepository _usuarioPortalRepository { get; set; }
        public UsuarioPerfilSistemaRepository _usuarioPerfilSistemaRepository { get; set; }
        public PerfilRepository _perfilRepository { get; set; }
        public SistemaRepository _sistemaRepository { get; set; }

        public void AlterarSitu(IList<long> ids, SituacaoUsuario situacao)
        {
            var usersToBeChanged = _usuarioPortalRepository.Queryable().Where(x => ids.Contains(x.Id));
            foreach (var usuarioPortal in usersToBeChanged)
            {
                usuarioPortal.Situacao = situacao;
                _usuarioPortalRepository.Save(usuarioPortal);
            }
            _usuarioPortalRepository.Flush();
        }

        public UsuarioPerfilSistema BuscarUsuarioPerfilSistema(long id, string codSistema)
        {
            return _usuarioPerfilSistemaRepository.Queryable().SingleOrDefault(x => x.Usuario.Id.Equals(id) && x.Sistema.Codigo.Equals(codSistema));
        }

        public DataSourceResponse<Perfil> ListarPerfisUsuario(DataSourceRequest request, long id, string codSistema)
        {
            return _usuarioPerfilSistemaRepository.Queryable().Where(x => x.Usuario.Id.Equals(id) && x.Sistema.Codigo.Equals(codSistema)).Select(x => x.Perfil).ToDataRequest(request);
        }

        public Usuario SelecionarOuCriarUsuarioPortal(LdapModel model)
        {
            var usuario = new UsuarioPortal { Login = model.Login, Nome = model.Nome };
            return CriarUsuarioPortal(model, usuario);
        }

        private Usuario CriarUsuarioPortal(LdapModel model, UsuarioPortal usuario)
        {
            if (!model.Sobrenome.IsNull())
            {
                usuario.Nome = usuario.Nome + " " + model.Sobrenome;
            }
            usuario.Email = model.Email;
            usuario.Situacao = SituacaoUsuario.Ativo;
            _usuarioPortalRepository.Save(usuario);
            return usuario;
        }

        public Usuario SelecionarOuCriarUsuarioPortal(LdapModel model, string senha)
        {
            var usuario = new UsuarioPortal { Login = model.Login, Nome = model.Nome, Senha = senha };
            return CriarUsuarioPortal(model, usuario);
        }

        public UsuarioPerfilSistema IncluirPerfil(long idUsuario, long idPerfil, string codigoSistema)
        {
            var exists = _usuarioPerfilSistemaRepository.Queryable()
                .Where(x => x.Sistema.Codigo.Equals(codigoSistema))
                .Where(x => x.Usuario.Id == idUsuario)
                .Any(x => x.Perfil.Id == idPerfil);

            var exc = new BusinessRuleException();
            var perfil = _perfilRepository.FindById(idPerfil);
            if (exists)
            {
                exc.AddError(GlobalMessages.MsgErroPerfil)
                    .WithParam(perfil.Nome)
                    .Complete();
            }
            exc.ThrowIfHasError();
            var userPerfil = new UsuarioPerfilSistema();
            var usuario = _usuarioPortalRepository.FindById(idUsuario);
            userPerfil.Perfil = perfil;
            userPerfil.Usuario = usuario.IsNull() ? new Usuario { Id = idUsuario } : usuario;
            userPerfil.Sistema = _sistemaRepository.Queryable().FirstOrDefault(x => x.Codigo.Equals(codigoSistema));
            _usuarioPerfilSistemaRepository.Save(userPerfil);
            return userPerfil;
        }

        public string RemoverPerfil(long idUsuario, long idPerfil, string codigoSistema)
        {

            var count = _usuarioPerfilSistemaRepository.Queryable()
                .Where(x => x.Sistema.Codigo.Equals(codigoSistema))
                .Where(x => x.Usuario.Id == idUsuario);

            var exc = new BusinessRuleException();
            if (count.Count() == 1)
            {
                exc.AddError(GlobalMessages.MsgErroPerfilDelete)
                    .Complete();
            }
            exc.ThrowIfHasError();

            var exists = _usuarioPerfilSistemaRepository.Queryable()
                .Where(x => x.Sistema.Codigo.Equals(codigoSistema))
                .Where(x => x.Usuario.Id == idUsuario)
                .FirstOrDefault(x => x.Perfil.Id == idPerfil);

            if (!exists.IsEmpty())
            {
                _usuarioPerfilSistemaRepository.Delete(exists);
                return exists.Perfil.Nome;
            }
            return string.Empty;
        }

        public UsuarioPortal Salvar(UsuarioPortal model, string codSistema)
        {
            #region Validações de modelo
            var businessRuleException = new BusinessRuleException();
            foreach (var validationResult in model.Validate())
            {
                businessRuleException.AddError(validationResult.ErrorMessage).Complete();
            }

            businessRuleException.ThrowIfHasError();
            #endregion

            return EditUserOnDatabase(model, codSistema);
        }

        public UsuarioPortal SalvarUsuarioPortalLoja(UsuarioPortal usuario, string codSistema)
        {
            var businessRuleException = new BusinessRuleException();
            foreach (var validationResult in usuario.Validate())
            {
                businessRuleException.AddError(validationResult.ErrorMessage).Complete();
            }

            businessRuleException.ThrowIfHasError();

            _usuarioPortalRepository.Save(usuario);
            _usuarioPortalRepository.Flush();
            return usuario;
        }

        public byte[] Exportar(DataSourceRequest request, string nomeLoginEmail, long? idLoja, SituacaoUsuario[] situacoes)
        {
            ExcelUtil excel = ExcelUtil.NewInstance(20)
                .NewSheet(DateTime.Now.ToString("yyyyMMddHHmmss"))
                .WithHeader(GetHeader());

            var list = _usuarioPortalRepository.ListarUsuarioPortalLoja(nomeLoginEmail, idLoja, situacoes).ToDataRequest(request).records.ToList();

            foreach (var model in list)
            {
                excel.CreateCellValue(model.Nome.IsEmpty() ? "" : model.Nome).Width(40)
                    .CreateCellValue(model.Login.IsEmpty() ? "" : model.Login).Width(40)
                    .CreateCellValue(model.Email.IsEmpty() ? "" : model.Email).Width(40)
                    .CreateCellValue(model.Situacao.IsEmpty() ? "" : model.Situacao.AsString()).Width(20);
            }
            excel.Close();
            return excel.DownloadFile();
        }

        public byte[] ExportarUsuariosAtivos(DataSourceRequest request,string nameOrEmail, string[] tipos, string name, string login, string email, long? perfil, string codigoSistema)
        {
            ExcelUtil excel = ExcelUtil.NewInstance(20)
                .NewSheet(DateTime.Now.ToString("yyyyMMddHHmmss"))
                .WithHeader(GetHeaderViewUsuarios());

            var list = _viewUsuarioPerfilRepository.ListarUsuariosAtivos(request,nameOrEmail,tipos,name,login,email, perfil,codigoSistema);

            foreach (var model in list)
            {
                excel.CreateCellValue(model.NomeUsuario.IsEmpty() ? "" : model.NomeUsuario).Width(40)
                    .CreateCellValue(model.Email.IsEmpty() ? "" : model.Email).Width(40)
                    .CreateCellValue(model.Perfis.IsEmpty() ? "" : model.Perfis).Width(40)
                    .CreateCellValue(model.Login.IsEmpty() ? "" : model.Login).Width(40)
                    .CreateCellValue(model.Situacao.IsEmpty() ? "" : model.Situacao.AsString()).Width(20);
            }
            excel.Close();
            return excel.DownloadFile();
        }

        public string[] GetHeader()
        {
            IList<string> header = new List<string>
            {
                GlobalMessages.Nome,
                GlobalMessages.Login,
                GlobalMessages.Email,
                GlobalMessages.Situacao,
                GlobalMessages.Loja,
            };
            return header.ToArray();
        }
        public string[] GetHeaderViewUsuarios()
        {
            IList<string> header = new List<string>
            {
                GlobalMessages.Nome,
                GlobalMessages.Email,
                GlobalMessages.Perfis,
                GlobalMessages.Login,
                GlobalMessages.Situacao,
            };
            return header.ToArray();
        }

        private UsuarioPortal EditUserOnDatabase(UsuarioPortal model, string codSistema)
        {
            var validacao = new BusinessRuleException();
            var auxUser = _usuarioPortalRepository.FindById(model.Id);

            if (!model.Login.Equals(auxUser.Login))
            {
                validacao.AddError(GlobalMessages.CampoNaoPermiteAlteracao)
                    .WithParam(GlobalMessages.Login)
                    .Complete();
            }
            if (!model.Situacao.Equals(auxUser.Situacao))
            {
                validacao.AddError(GlobalMessages.CampoNaoPermiteAlteracao)
                    .WithParam(GlobalMessages.Situacao)
                    .Complete();
            }
            validacao.ThrowIfHasError();

            auxUser.Nome = model.Nome;
            auxUser.Email = model.Email;

            _usuarioPortalRepository.Save(auxUser);
            _usuarioPortalRepository.Flush();
            return auxUser;
        }
    }
}
