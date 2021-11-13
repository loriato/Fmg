using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.Domain.Shared.Commons;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Web.Models;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{

    public class BookEmpreendimentoController : BaseController
    {
        private EmpreendimentoRepository _empreendimentoRepository { get; set; }
        private ArquivoRepository _arquivoRepository { get; set; }
        private ViewArquivoEmpreendimentoRepository _viewArquivoEmpreendimentoRepository { get; set; }
        private ArquivoEmpreendimentoRepository _arquivoEmpreendimentoRepository { get; set; }
        private ArquivoService _arquivoService { get; set; }
        private StaticResourceService _staticResourceService { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Listar(DataSourceRequest request, long idEmpreendimento)
        {
            var arquivosDoEmprendimento = _viewArquivoEmpreendimentoRepository.Listar(request, idEmpreendimento);

            var results = arquivosDoEmprendimento.records.ToList();

            foreach (var arquivo in results)
            {
                if (arquivo.ContentType.Equals("video"))
                {
                    arquivo.UrlThumbnail = "https://img.youtube.com/vi/" + arquivo.Nome + "/hqdefault.jpg";
                    arquivo.Url = "https://img.youtube.com/vi/" + arquivo.Nome + "/hqdefault.jpg";
                }
                else
                {
                    string webRoot = GetWebAppRoot();
                    string filename = _staticResourceService.LoadResource(arquivo.IdArquivo);
                    arquivo.UrlThumbnail = _staticResourceService.CreateThumbnailUrl(webRoot, filename);
                    arquivo.Url = _staticResourceService.CreateUrl(webRoot, filename);
                }
            }

            var result = new DataSourceResponse<ViewArquivoEmpreendimento>
            {
                filtered = arquivosDoEmprendimento.filtered,
                total = arquivosDoEmprendimento.total,
                records = results
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Upload(ArquivoDTO dto)
        {
            var jsonResponse = new JsonResponse();
            try
            {
                var excecaoRegras = new BusinessRuleException();
                if (dto == null || dto.TargetId == null || dto.File == null)
                {
                    excecaoRegras.AddError(GlobalMessages.MsgNenhumArquivoSelecionado).Complete();
                }
                excecaoRegras.ThrowIfHasError();

                var target = _empreendimentoRepository.FindById(dto.TargetId);
                if (target == null)
                {
                    excecaoRegras.AddError(GlobalMessages.RegistroNaoEncontrado).WithParams(GlobalMessages.Empreendimento, dto.TargetId.ToString()).Complete();
                }
                excecaoRegras.ThrowIfHasError();

                var arquivo = _arquivoService.CreateFile(dto.File);
                ArquivoEmpreendimento associacaoArquivo = new ArquivoEmpreendimento();
                associacaoArquivo.Empreendimento = target;
                associacaoArquivo.Arquivo = arquivo;
                _arquivoEmpreendimentoRepository.Save(associacaoArquivo);

                jsonResponse.Sucesso = true;
                jsonResponse.Mensagens.Add(GlobalMessages.MsgUploadFotoSucesso);
            }
            catch (BusinessRuleException bre)
            {
                jsonResponse.Mensagens.AddRange(bre.Errors);
                jsonResponse.Sucesso = false;

            }
            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult IncluirVideo(ArquivoDTO dto)
        {
            var jsonResponse = new JsonResponse();
            try
            {
                var excecaoRegras = new BusinessRuleException();
                if (dto == null || dto.TargetId == null || dto.YoutubeVideoCode == null)
                {
                    excecaoRegras.AddError(GlobalMessages.CampoObrigatorio).WithParam(GlobalMessages.UrlVideo).Complete();
                }
                excecaoRegras.ThrowIfHasError();

                var target = _empreendimentoRepository.FindById(dto.TargetId);
                if (target == null)
                {
                    excecaoRegras.AddError(GlobalMessages.RegistroNaoEncontrado).WithParams(GlobalMessages.Empreendimento, dto.TargetId.ToString()).Complete();
                }
                excecaoRegras.ThrowIfHasError();

                var videoUrl = dto.YoutubeVideoCode;
                var lwVideoUrl = videoUrl.ToLower();
                if (lwVideoUrl.Contains("watch"))
                {
                    var split = videoUrl.Split('/');
                    videoUrl = split[split.Length - 1].Replace("watch?v=", "");
                }else if (lwVideoUrl.Contains("embed"))
                {
                    var split = videoUrl.Split('/');
                    videoUrl = split[split.Length - 1];
                }

                ArquivoEmpreendimento associacaoArquivo = new ArquivoEmpreendimento();
                associacaoArquivo.Empreendimento = target;
                associacaoArquivo.YoutubeVideoCode = videoUrl;
                _arquivoEmpreendimentoRepository.Save(associacaoArquivo);

                jsonResponse.Sucesso = true;
                jsonResponse.Mensagens.Add(GlobalMessages.MsgUploadFotoSucesso);
            }
            catch (BusinessRuleException bre)
            {
                jsonResponse.Mensagens.AddRange(bre.Errors);
                jsonResponse.Sucesso = false;

            }
            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult Excluir(long idAssociacao)
        {
            var jsonResponse = new JsonResponse();
            try
            {
                var target = _arquivoEmpreendimentoRepository.FindById(idAssociacao);
                var excecaoRegras = new BusinessRuleException();
                if (target == null)
                {
                    excecaoRegras.AddError(GlobalMessages.RegistroNaoEncontrado).WithParams(GlobalMessages.Arquivo, idAssociacao.ToString()).Complete();
                }
                excecaoRegras.ThrowIfHasError();
                _arquivoEmpreendimentoRepository.Delete(target);
                if (target.Arquivo.HasValue())
                {
                    _arquivoRepository.Delete(target.Arquivo);
                }
                jsonResponse.Sucesso = true;
                jsonResponse.Mensagens.Add(GlobalMessages.RemovidoSucesso);
            }
            catch (BusinessRuleException bre)
            {
                jsonResponse.Mensagens.AddRange(bre.Errors);
                jsonResponse.Sucesso = false;
            }
            return Json(jsonResponse, JsonRequestBehavior.AllowGet);
        }

    }
}