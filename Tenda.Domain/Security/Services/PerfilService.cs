using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.Core.Services;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Models;
using Tenda.Domain.Security.Repository;
using Tenda.Domain.Security.Repository.Models;

namespace Tenda.Domain.Security.Services
{
    public class PerfilService : BaseService
    {
        public PerfilRepository _perfilRepository { get; set; }
        
        public DataSourceResponse<Perfil> ListaPerfisSelecionados(DataSourceRequest request, FiltroPerfilDTO filtro)
        {
            var result = ListarPerfis();

            result = result.Where(x => x.ExibePortal == true);
            result = result.Where(x => x.Situacao == Situacao.Ativo);

            if (!filtro.NomePerfis.IsEmpty())
            {
                result = result.Where(x => filtro.NomePerfis.ToLower().Contains(x.Nome.ToLower()));
            }
            else
                return new DataSourceResponse<Perfil>();

            return result.ToDataRequest(request);
        }
        public DataSourceResponse<Perfil> ListarPerfisDataTable(DataSourceRequest request, string nome, bool? buscaSuspensos)
        {
            var result = ListarPerfis();
            if (!nome.IsEmpty())
            {
                result = result.Where(x => x.Nome.ToLower().Contains(nome.ToLower()));
            }
            if (buscaSuspensos.HasValue && buscaSuspensos == true)
            {
                result = result.Where(x => x.Situacao != Situacao.Cancelado);
            }
            else
            {
                result = result.Where(x => x.Situacao == Situacao.Ativo);
            }
            return result.ToDataRequest(request);
        }

        public string ListarNomesPerfis(List<long> idsPerfis)
        {
            string result = null;
            var builder = new System.Text.StringBuilder();
            builder.Append(result);
            foreach (long idPerfil in idsPerfis)
            {
                builder.Append(_perfilRepository.FindById(idPerfil).Nome);
                builder.Append(", ");
            }

            result = builder.ToString();

            result = result.IsEmpty() ? "" : result.Remove(result.Length - 2, 2);
            return result;
        }

        public object ListarPerfisPortalAutocomplete(DataSourceRequest request)
        {
            var results = ListarPerfis();
            if (request.filter.FirstOrDefault() != null)
            {
                String filtro = request.filter.FirstOrDefault().column.ToString().ToLower();
                String queryTerm = request.filter.FirstOrDefault().value.ToString().ToLower();
                if (filtro.Equals("nome"))
                {
                    results = results.Where(x => x.Nome.ToLower().Contains(queryTerm));
                }
                results = results.Where(x => x.Situacao == Situacao.Ativo);
            }

            results = results.Where(x => x.ExibePortal);

            return results.ToDataRequest(request);
        }

        public object ListarPerfisAutocomplete(DataSourceRequest request)
        {
            var results = ListarPerfis();
            if (request.filter.FirstOrDefault() != null)
            {
                String filtro = request.filter.FirstOrDefault().column.ToString().ToLower();
                String queryTerm = request.filter.FirstOrDefault().value.ToString().ToLower();
                if (filtro.Equals("nome"))
                {
                    results = results.Where(x => x.Nome.ToLower().Contains(queryTerm));
                }
                results = results.Where(x => x.Situacao == Situacao.Ativo);
            }
            return results.ToDataRequest(request);
        }

        public object ListarPerfisOrigemContatoAutocomplete(DataSourceRequest request, long[] objs)
        {
            var results = ListarPerfis();
            if (request.filter.FirstOrDefault() != null)
            {
                String filtro = request.filter.FirstOrDefault().column.ToString().ToLower();
                String queryTerm = request.filter.FirstOrDefault().value.ToString().ToLower();
                if (filtro.Equals("nome"))
                {
                    results = results.Where(x => x.Nome.ToLower().Contains(queryTerm));
                }
                results = results.Where(x => x.Situacao == Situacao.Ativo);
                if (objs != null)
                {
                    results = results.Where(x => !objs.Contains(x.Id));
                }
            }
            return results.ToDataRequest(request);
        }

        private IQueryable<Perfil> ListarPerfis()
        {
            return _perfilRepository.Queryable().Select(x => new Perfil
            {
                Nome = x.Nome,
                Id = x.Id,
                ResponsavelCriacao = x.ResponsavelCriacao,
                Situacao = x.Situacao,
                CriadoEm = x.CriadoEm,
                ExibePortal = x.ExibePortal
                
            });
        }

        public Perfil Salvar(Perfil entityPerfil)
        {
            _perfilRepository.Save(entityPerfil);
            return entityPerfil;
        }

        public void AlterarSituacaoById(long id, Situacao situacao)
        {
            var entidade = _perfilRepository.FindById(id);

            if (entidade != null)
            {
                switch (situacao)
                {
                    case Situacao.Ativo:
                        entidade.Situacao = Situacao.Ativo;
                        _perfilRepository.Save(entidade);
                        _perfilRepository.Flush();
                        break;
                    case Situacao.Cancelado:
                        entidade.Situacao = Situacao.Cancelado;
                        _perfilRepository.Save(entidade);
                        _perfilRepository.Flush();
                        break;
                    case Situacao.Suspenso:
                        entidade.Situacao = Situacao.Suspenso;
                        _perfilRepository.Save(entidade);
                        _perfilRepository.Flush();
                        break;
                }
            }
            else
            {
                var bre = new BusinessRuleException();
                bre.Errors.Add("Perfil não encontrado");

                throw bre;
            }
        }

        public Perfil CriarPerfil(Perfil entityPerfil)
        {
            var validation = new BusinessRuleException();
            if (entityPerfil.Nome.IsEmpty())
            {
                validation.AddError(GlobalMessages.CampoObrigatorio).WithParam(GlobalMessages.Nome).Complete();
            }

            bool hasPerfil = _perfilRepository.Queryable()
                .Where(reg => reg.Id != entityPerfil.Id)
                .Any(x => x.Nome.ToLower().Equals(entityPerfil.Nome.ToLower()));
            if (hasPerfil)
            {
                validation.AddError(GlobalMessages.RegistroNaoUnico).WithParam(GlobalMessages.Perfil).WithParam(entityPerfil.Nome).Complete();
            }
            validation.ThrowIfHasError();
            _perfilRepository.Save(entityPerfil);
            return entityPerfil;
        }


    }
}
