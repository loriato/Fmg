using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Europa.Commons;
using Europa.Data;
using Europa.Extensions;
using Europa.Resources;
using FluentValidation.Results;
using NHibernate.Exceptions;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.Core.Models;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.Commons;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class EmpresaVendaService : BaseService
    {
        private EmpresaVendaRepository _empresaVendaRepository { get; set; }
        private ArquivoService _arquivoService { get; set; }
        private CorretorRepository _corretorRepository { get; set; }
        private DocumentoEmpresaVendaRepository _documentoEmpresaVendaRepository { get; set; }
        private ViewStandVendaEmpresaVendaRepository _viewStandVendaEmpresaVenda { get; set; }
        private RegraComissaoEvsRepository _regraComissaoEvsRepository { get; set; }
        private RegraComissaoService _regraComissaoService { get; set; }
        private HistoricoRegraComissaoService _historicoRegraComissaoService { get; set; }
        public Tenda.Domain.EmpresaVenda.Models.EmpresaVenda Salvar(
            Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda, BusinessRuleException bre)
        {
            // Remove máscaras
            empresaVenda.CNPJ = empresaVenda.CNPJ.OnlyNumber();
            empresaVenda.Cep = empresaVenda.Cep.OnlyNumber();

            empresaVenda.TipoEmpresaVenda = TipoEmpresaVenda.EmpresaVenda;

            // Realiza as validações de EmpresaVenda
            ValidationResult emveResult = new EmpresaVendaValidator(_empresaVendaRepository).Validate(empresaVenda);

            // Verifica se retornou algum erro
            bre.WithFluentValidation(emveResult);

            if (emveResult.IsValid)
            {
                _empresaVendaRepository.Save(empresaVenda);
            }

            return empresaVenda;
        }
        public Tenda.Domain.EmpresaVenda.Models.EmpresaVenda SalvarPreEv(
            Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda, BusinessRuleException bre)
        {
            // Realiza as validações de EmpresaVenda
            ValidationResult emveResult = new PreCadastroValidator(_empresaVendaRepository).Validate(empresaVenda);

            // Verifica se retornou algum erro
            bre.WithFluentValidation(emveResult);

            // Remove máscaras
            empresaVenda.CNPJ = empresaVenda.CNPJ.OnlyNumber();
            empresaVenda.Cep = empresaVenda.Cep.OnlyNumber();

            empresaVenda.TipoEmpresaVenda = TipoEmpresaVenda.EmpresaVenda;

            if (emveResult.IsValid)
            {
                _empresaVendaRepository.Save(empresaVenda);
            }

            return empresaVenda;
        }
        /// <summary>
        /// retorna o ID do arquivo carregado
        /// </summary>
        /// <param name="foto"></param>
        /// <param name="idEmpresaVenda"></param>
        /// <returns></returns>
        public long UploadFoto(HttpPostedFileBase foto, long idEmpresaVenda)
        {
            var empresaVenda = _empresaVendaRepository.FindById(idEmpresaVenda);
            var excecaoRegras = new BusinessRuleException();
            if (foto == null)
            {
                excecaoRegras.AddError(GlobalMessages.MsgNenhumArquivoSelecionado).Complete();
            }

            excecaoRegras.ThrowIfHasError();
            var arquivo = _arquivoService.CreateFile(foto);
            empresaVenda.FotoFachada = arquivo;
            _empresaVendaRepository.Save(empresaVenda);
            return arquivo.Id;
        }

        public IQueryable<Tenda.Domain.EmpresaVenda.Models.EmpresaVenda> Listar(DataSourceRequest request)
        {
            var results = _empresaVendaRepository.Listar();

            if (request.HasValue() && request.filter.FirstOrDefault() != null)
            {
                var filtroStand = request.filter.Where(x => x.column.ToLower().Equals("standvenda")).SingleOrDefault();
                if (filtroStand.HasValue() && filtroStand.value.HasValue())
                {
                    results = _viewStandVendaEmpresaVenda.Queryable()
                        .Where(x => x.IdStandVendaEmpresaVenda != null)
                        .Where(x => x.IdStandVenda == long.Parse(filtroStand.value))
                        .Select(x => new Tenda.Domain.EmpresaVenda.Models.EmpresaVenda
                        {
                            Id = x.IdEmpresaVenda,
                            NomeFantasia = x.NomeEmpresaVenda,
                            Estado = x.Regional
                        }).AsQueryable();
                }

                foreach (var filtro in request.filter)
                {
                    if (filtro.column.ToLower().Equals("nomefantasia"))
                    {
                        results = results.Where(x => x.NomeFantasia.ToLower().Contains(filtro.value.ToLower()));
                    }

                    if (filtro.column.ToLower().Equals("regional"))
                    {
                        if (filtro.value.HasValue() && !filtro.value.ToLower().Split(',').Contains(""))
                        {
                            results = results.Where(x => filtro.value.ToLower().Contains(x.Estado.ToLower()));
                        }
                    }

                    if (filtro.column.ToLower().Equals("idspermitidos") && filtro.value.HasValue())
                    {
                        var valor = filtro.value.Trim()
                            .Split(',')
                            .Where(x => x.Length > 0)
                            .Select(x => long.Parse(x))
                            .ToList();
                        results = results.Where(x => valor.Contains(x.Id));
                    }
                }
            }

            return results;
        }

        public Tenda.Domain.EmpresaVenda.Models.EmpresaVenda CancelarEV(
            Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda, string motivo)
        {
            var exc = new BusinessRuleException();
            //empresaVenda.Situacao = Tenda.Domain.Security.Enums.Situacao.Cancelado;
            //_empresaVendaRepository.Save(empresaVenda);
            var nome = empresaVenda.Corretor.Nome;
            var email = empresaVenda.Corretor.Email;
            var nomeEv = empresaVenda.NomeFantasia;

            EnviarEmailEvCancelada(email, nome, motivo, nomeEv);
            var corretor = _corretorRepository.FindById(empresaVenda.Corretor.Id);
            var documentos = _documentoEmpresaVendaRepository.FindByIdEmpresaVenda(empresaVenda.Id);
            try
            {
                foreach(var doc in documentos)
                {
                    _documentoEmpresaVendaRepository.Delete(doc);
                }
                _corretorRepository.Delete(corretor);
                _empresaVendaRepository.Delete(empresaVenda);
            }
            catch (GenericADOException exp)
            {
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(exp))
                {
                    exc.AddError(GlobalMessages.RemovidoSemSucesso).WithParam(empresaVenda.NomeFantasia).Complete();
                }
            }
            exc.ThrowIfHasError();
            return empresaVenda;
        }

        public void EnviarEmailEvCancelada(string emailDestino, string nome, string motivo, string empresaVenda)
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
            toReplace.Add("empresavenda", empresaVenda);
            toReplace.Add("motivo", motivo);
            var templateName = "reprovacao-ev.html";
            var corpoEmail = TemplateEmailFactory.ResolveTemplateWithReplace(templateName, toReplace);

            var email = EmailService.CriarEmail(emailDestino, "[Tenda] Portal de EV - Acesso Negado", corpoEmail);
            EmailService.EnviarEmail(email);
        }

        public List<EmpresaVendaTendaDTO> ListarEVSAtivas()
        {

            var list = _empresaVendaRepository.ListarEVSAtivas();
            var listDto = list.Select(MontarDtoTenda).ToList();

            return listDto;
        }

        private EmpresaVendaTendaDTO MontarDtoTenda(Tenda.Domain.EmpresaVenda.Models.EmpresaVenda model)
        {
            var dto = new EmpresaVendaTendaDTO();
            dto.FromDomain(model);

            return dto;
        }

        public int AtivarEmLote(UsuarioPortal usuarioAlteracao, List<long> idEmpresasEvs)
        {
            var updates = _empresaVendaRepository.AlterarSituacaoEmLote(usuarioAlteracao.Id, Situacao.Ativo, idEmpresasEvs);

            _regraComissaoService.AtribuirRegraPadrao(idEmpresasEvs, usuarioAlteracao);

            return updates;
        }
        public int SuspenderEmLote(UsuarioPortal usuarioAlteracao, List<long> idEmpresasEvs)
        {
            var updates = _empresaVendaRepository.AlterarSituacaoEmLote(usuarioAlteracao.Id, Situacao.Suspenso, idEmpresasEvs);

            _regraComissaoService.FinalizarRegraComissao(idEmpresasEvs, usuarioAlteracao);

            return updates;
        }

        public int CancelarEmLote(UsuarioPortal usuarioAlteracao, List<long> idEmpresasEvs)
        {
            var updates = _empresaVendaRepository.CancelarEmLote(usuarioAlteracao.Id, idEmpresasEvs);

            _regraComissaoService.FinalizarRegraComissao(idEmpresasEvs, usuarioAlteracao);
            
            return updates;
        }

        public int CancelarEmLote(long usuarioAlteracao, List<long> registros)
        {
            return _empresaVendaRepository.AlterarSituacaoEmLote(usuarioAlteracao, Situacao.Cancelado, registros);
        }

    }
}