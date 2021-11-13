using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using Europa.Web;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Repository.Models;
using Tenda.EmpresaVenda.Domain.Services;
using Tenda.EmpresaVenda.Web.Models.Application;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("PAG06")]
    public class PontuacaoFidelidadeController : BaseController
    {
        public ViewPontuacaoFidelidadeRepository _viewPontuacaoFidelidadeRepository { get; set; }
        public PontuacaoFidelidadeRepository _pontuacaoFidelidadeRepository { get; set; }
        public PontuacaoFidelidadeService _pontuacaoFidelidadeService { get; set; }
        public ItemPontuacaoFidelidadeRepository _itemPontuacaoFidelidadeRepository { get; set; }
        public PontuacaoFidelidadeEmpresaVendaService _pontuacaoFidelidadeEmpresaVendaService { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [BaseAuthorize("PAG06", "Visualizar")]
        public JsonResult ListarDatatablePontuacaoFidelidade(DataSourceRequest request, PontuacaoFidelidadeDTO filtro)
        {
            var result = _viewPontuacaoFidelidadeRepository.ListarDatatablePontuacaoFidelidade(request, filtro);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [BaseAuthorize("PAG06", "Liberar")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult Liberar(long idPontuacaoFidelidade)
        {
            var response = new JsonResponse();

            try
            {
                _pontuacaoFidelidadeService.Liberar(idPontuacaoFidelidade);

                response.Sucesso = true;
                response.Mensagens.Add(string.Format("A tabela de {0} foi {1} com {2}", GlobalMessages.PontuacaoFidelidade, GlobalMessages.Liberada, GlobalMessages.Sucesso));
            }catch(BusinessRuleException bre)
            {
                CurrentTransaction().Rollback();
                response.Mensagens.AddRange(bre.Errors);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [BaseAuthorize("PAG06", "ExportarTodos")]
        public FileContentResult ExportarTodos(DataSourceRequest request, PontuacaoFidelidadeDTO filtro)
        {
            var modifiedRequest = request;
            modifiedRequest.start = 0;
            modifiedRequest.pageSize = 0;
            
            byte[] file = _pontuacaoFidelidadeService.Exportar(modifiedRequest, filtro);
            return File(file, MimeMappingWrapper.Xlsx,
                $"{GlobalMessages.PontuacaoFidelidade} - {DateTime.Now}.xlsx");
        }

        [BaseAuthorize("PAG06", "ExportarPagina")]
        public FileContentResult ExportarPagina(DataSourceRequest request,PontuacaoFidelidadeDTO filtro)
        {
            byte[] file = _pontuacaoFidelidadeService.Exportar(request, filtro);
            return File(file, MimeMappingWrapper.Xlsx,
                $"{GlobalMessages.PontuacaoFidelidade} - {DateTime.Now}.xlsx");
        }

        [HttpPost]
        [BaseAuthorize("PAG06", "Excluir")]
        [Transaction(TransactionAttributeType.Required)]
        public JsonResult Excluir(long idPontuacaoFidelidade)
        {
            var response = new JsonResponse();

            try
            {
                _pontuacaoFidelidadeService.Excluir(idPontuacaoFidelidade);

                response.Sucesso = true;
                response.Mensagens.Add(string.Format("A tabela de {0} foi {1} com {2}", GlobalMessages.PontuacaoFidelidade, GlobalMessages.Excluido, GlobalMessages.Sucesso));
            }
            catch (BusinessRuleException bre)
            {
                CurrentTransaction().Rollback();
                response.Mensagens.AddRange(bre.Errors);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #region Matriz
        public ActionResult MatrizPontuacaoFidelidade(bool? create, String regional = null, List<long> idEmpreendimentos = null, 
            long idPontuacaoFidelidade = 0, TipoPontuacaoFidelidade? tipoPontuacaoFidelidade = null,
            TipoCampanhaFidelidade? tipoCampanhaFidelidade = null, bool ultimo = false,List<long> idEvs=null,long progressao = 0,
            List<long>qtdm=null)
        {
            PontuacaoFidelidadeDTO pontuacaoFidelidade = new PontuacaoFidelidadeDTO();

            if (idPontuacaoFidelidade > 0)
            {
                var existente = _pontuacaoFidelidadeRepository.FindById(idPontuacaoFidelidade);

                if (!existente.IsEmpty())
                {
                    pontuacaoFidelidade.Id = existente.Id;
                    pontuacaoFidelidade.Descricao = existente.Descricao;
                    pontuacaoFidelidade.Situacao = existente.Situacao;
                    pontuacaoFidelidade.InicioVigencia = existente.InicioVigencia;
                    pontuacaoFidelidade.TerminoVigencia = existente.TerminoVigencia;
                    pontuacaoFidelidade.HashDoubleCheck = existente.HashDoubleCheck;
                    pontuacaoFidelidade.IdArquivoDoubleCheck = existente.IdArquivoDoubleCheck;
                    pontuacaoFidelidade.NomeArquivoDoubleCheck = existente.NomeArquivoDoubleCheck;
                    pontuacaoFidelidade.ContentTypeDoubleCheck = existente.ContentTypeDoubleCheck;
                    //pontuacaoFidelidade.Arquivo Arquivo = existente.Arquivo
                    pontuacaoFidelidade.Regional = existente.Regional;
                    pontuacaoFidelidade.TipoPontuacaoFidelidade = existente.TipoPontuacaoFidelidade;
                    pontuacaoFidelidade.TipoCampanhaFidelidade = existente.TipoCampanhaFidelidade;
                    pontuacaoFidelidade.QuantidadeMinima = existente.QuantidadeMinima;

                    var itens = _itemPontuacaoFidelidadeRepository.BuscarItens(idPontuacaoFidelidade);

                    var empreendimentos = itens.Select(x => x.Empreendimento.Id).ToList();

                    pontuacaoFidelidade.IdEmpreendimentos = empreendimentos;

                    var empresasVendas = itens.Select(x => x.EmpresaVenda.Id).ToList();

                    pontuacaoFidelidade.IdEmpresasVenda = empresasVendas;

                    pontuacaoFidelidade.Progressao = existente.Progressao;

                    pontuacaoFidelidade.QuantidadesMinimas = existente.QuantidadesMinimas;

                }
            }
            else
            {
                pontuacaoFidelidade.Situacao = SituacaoPontuacaoFidelidade.Rascunho;
                if (regional.HasValue())
                {
                    pontuacaoFidelidade.Regional = regional;
                }

                if (tipoPontuacaoFidelidade.HasValue())
                {
                    pontuacaoFidelidade.TipoPontuacaoFidelidade = tipoPontuacaoFidelidade.Value;
                }

                if (tipoCampanhaFidelidade.HasValue())
                {
                    pontuacaoFidelidade.TipoCampanhaFidelidade = tipoCampanhaFidelidade.Value;
                }

                if (idEmpreendimentos.HasValue())
                {
                    pontuacaoFidelidade.IdEmpreendimentos = idEmpreendimentos;
                }

                if (idEvs.HasValue())
                {
                    pontuacaoFidelidade.IdEmpresasVenda = idEvs;
                }

                if (progressao > 0)
                {
                    pontuacaoFidelidade.Progressao = progressao;
                }

                if (qtdm.HasValue())
                {
                    pontuacaoFidelidade.QuantidadesMinimas = new JavaScriptSerializer().Serialize(qtdm);
                }

            }

            if (pontuacaoFidelidade.Arquivo.HasValue())
            {
                pontuacaoFidelidade.Arquivo = new Arquivo
                {
                    Id = pontuacaoFidelidade.Arquivo.Id
                };
            }

            ViewBag.Novo = create == true;
            ViewBag.PorUltimaAtualizacao = ultimo;

            return View(pontuacaoFidelidade);
        }
        
        [HttpPost]
        [BaseAuthorize("PAG06", "Visualizar")]
        public ActionResult BuscarMatriz(long? idPontuacaoFidelidade, string regional,
            List<long> idEmpreendimentos ,bool editable, bool novo, bool ultimaAtt, 
            long? progressao, TipoPontuacaoFidelidade? tipoPontuacaoFidelidade=null,
            TipoCampanhaFidelidade? tipoCampanhaFidelidade=null,
            List<long> idEmpresasVenda=null,TipoModalidadeProgramaFidelidade? modalidade=null,
            List<long>qtdm=null)
        {
            var option = _pontuacaoFidelidadeService.BuscarMatriz(idPontuacaoFidelidade == null ? 0 : idPontuacaoFidelidade.Value,
                regional, editable, novo, ultimaAtt, tipoPontuacaoFidelidade.Value,
                tipoCampanhaFidelidade, idEmpreendimentos, idEmpresasVenda, modalidade.Value, progressao==null?1:progressao.Value,
               qtdm);
            var str = JsonConvert.SerializeObject(option, Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            var saida = Json(str, JsonRequestBehavior.AllowGet);
            saida.MaxJsonLength = 1000000000;
            return saida;
        }

        [HttpPost]
        [BaseAuthorize("PAG06", "Incluir")]
        [Transaction(TransactionAttributeType.Required)]
        public ActionResult SalvarMatriz(List<ItemPontuacaoFidelidade> itens, PontuacaoFidelidade pontuacaoFidelidade)
        {
            
            var json = new JsonResponse();
            try
            {
                var pontuacao = _pontuacaoFidelidadeService.SalvarMatriz(itens, pontuacaoFidelidade);

                json.Sucesso = true;
                var tituloRegistro = (pontuacao.Situacao == SituacaoPontuacaoFidelidade.Rascunho
                                         ? GlobalMessages.Rascunho + " "
                                         : "") + GlobalMessages.PontuacaoFidelidade + " - " +
                                     pontuacao.Regional;
                json.Mensagens.Add(string.Format(GlobalMessages.MsgSalvoSucesso, tituloRegistro));
                json.Objeto = new RegraComissao
                {
                    Id = pontuacao.Id,
                    Descricao = pontuacao.Descricao
                };
            }
            catch (BusinessRuleException bre)
            {
                CurrentTransaction().Rollback();
                json.Sucesso = false;
                json.Mensagens.AddRange(bre.Errors);
                json.Campos.AddRange(bre.ErrorsFields);
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GerarExcel(long? idPontuacaoFidelidade, string regional)
        {
            var excel = _pontuacaoFidelidadeService.GerarExcel(idPontuacaoFidelidade == null ? 0 : idPontuacaoFidelidade.Value,
                regional);
            string nomeArquivo = GlobalMessages.PontuacaoFidelidade + "-" + regional;
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            return File(excel, MimeMappingWrapper.Xlsx, $"{nomeArquivo.Trim()}_{date}.xlsx");
        }
        #endregion

        #region Download PDF
        [HttpGet]
        public JsonResult ListarEmpresaVendasPontuacaoFidelidade(long idPontuacaoFidelidade)
        {
            var empresas = _itemPontuacaoFidelidadeRepository.ItensDePontuacaoFidelidade(idPontuacaoFidelidade)
                .Select(x => x.EmpresaVenda)
                .Distinct()
                .Select(x => new
                {
                    Id = x.Id,
                    NomeFantasia = x.NomeFantasia
                });

            return Json(empresas, JsonRequestBehavior.AllowGet);            
        }

        public FileContentResult DownloadPdfPontuacaoFidelidadeEvs(long idPontuacaoFidelidade,long idEmpresaVenda)
        {
            var fileName = "LogoTendaPdf.png";
            var TargetPath = @"/Static/images/";
            var path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + TargetPath + fileName;
            byte[] logo = System.IO.File.ReadAllBytes(path);

            var arquivo = _pontuacaoFidelidadeEmpresaVendaService.GerarPdfAdmin(idPontuacaoFidelidade,idEmpresaVenda, logo);

            return File(arquivo.Content, arquivo.ContentType, arquivo.Nome);
        }

        public FileContentResult DownloadPdfPontuacaoFidelidade(long idPontuacaoFidelidade)
        {
            var fileName = "LogoTendaPdf.png";
            var TargetPath = @"/Static/images/";
            var path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + TargetPath + fileName;
            byte[] logo = System.IO.File.ReadAllBytes(path);

            var arquivo = _pontuacaoFidelidadeEmpresaVendaService.GerarPdfAdmin(idPontuacaoFidelidade, logo);

            return File(arquivo.Content, arquivo.ContentType, arquivo.Nome);
        }
        #endregion

    }
}