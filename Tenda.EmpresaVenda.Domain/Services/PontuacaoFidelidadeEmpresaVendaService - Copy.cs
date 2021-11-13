using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.Core.Models;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.Commons;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Repository.Models;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class PontuacaoFidelidadeEmpresaVendaService : BaseService
    {
        public PontuacaoFidelidadeEmpresaVendaRepository _pontuacaoFidelidadeEmpresaVendaRepository { get; set; }
        public ResgatePontuacaoFidelidadeRepository _resgatePontuacaoFidelidadeRepository { get; set; }
        public ViewConsolidadoPontuacaoFidelidadeRepository _viewConsolidadoPontuacaoFidelidadeRepository { get; set; }
        public ViewResgatePontuacaoFidelidadeRepository _viewResgatePontuacaoFidelidadeRepository { get; set; }
        public NotificacaoRepository _notificacaoRepository { get; set; }
        public PontuacaoFidelidadePdfService _pontuacaoFidelidadePdfService { get; set; }
        public RegraComissaoPadraoRepository _regraComissaoPadraoRepository { get; set; }
        private CorretorRepository _corretorRepository { get; set; }
        private EmpresaVendaRepository _empresaVendaRepository { get; set; }

        public void SolicitarResgate(PontuacaoFidelidadeDTO pontuacao)
        {
            var bre = new BusinessRuleException();

            if (pontuacao.IdEmpresaVenda.IsEmpty())
            {
                bre.AddError(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.EmpresaVenda)).Complete();
                bre.ThrowIfHasError();
            }

            if (pontuacao.PontuacaoSolicitada.IsEmpty())
            {
                bre.AddError(string.Format(GlobalMessages.MsgValorMaiorParam, GlobalMessages.PontuacaoSolicitada, "0")).Complete();
                bre.ThrowIfHasError();
            }

            var pontos = _pontuacaoFidelidadeEmpresaVendaRepository.FindByIdEmpresaVenda(pontuacao.IdEmpresaVenda);

            if (pontos.IsEmpty())
            {
                bre.AddError(GlobalMessages.ErroPontosContabilizados).Complete();
                bre.ThrowIfHasError();
            }

            if (pontuacao.PontuacaoSolicitada > pontos.PontuacaoDisponivel)
            {
                bre.AddError(string.Format(GlobalMessages.PontuacaoInsuficiente)).Complete();
                bre.ThrowIfHasError();
            }

            var resgate = new ResgatePontuacaoFidelidade
            {
                EmpresaVenda = new Tenda.Domain.EmpresaVenda.Models.EmpresaVenda { Id = pontuacao.IdEmpresaVenda },
                Pontuacao = pontuacao.PontuacaoSolicitada,
                DataResgate = DateTime.Now,
                SituacaoResgate = SituacaoResgate.Solicitado,
                SolicitadoPor = new UsuarioPortal { Id = pontuacao.IdSolicitadoPor }
            };

            pontos.PontuacaoDisponivel -= pontuacao.PontuacaoSolicitada;
            pontos.PontuacaoSolicitada += pontuacao.PontuacaoSolicitada;

            _pontuacaoFidelidadeEmpresaVendaRepository.Save(pontos);

            _resgatePontuacaoFidelidadeRepository.Save(resgate);

            var sequence = resgate.Id.ToString();
            resgate.Codigo = "RES" + sequence.PadLeft(6, '0');

            _resgatePontuacaoFidelidadeRepository.Save(resgate);

        }

        public void AprovarResgate(PontuacaoFidelidadeDTO pontuacao)
        {
            var bre = new BusinessRuleException();

            if (pontuacao.Voucher.IsEmpty())
            {
                bre.AddError(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Voucher)).Complete();
                bre.ThrowIfHasError();
            }

            var resgate = _resgatePontuacaoFidelidadeRepository.FindById(pontuacao.IdResgate);

            if (resgate.IsEmpty())
            {
                bre.AddError(string.Format(GlobalMessages.DadoInvalido, GlobalMessages.Resgate)).Complete();
                bre.ThrowIfHasError();
            }

            if (resgate.SituacaoResgate != SituacaoResgate.Solicitado)
            {
                bre.AddError(GlobalMessages.MsgErroResgateAnalisado).Complete();
                bre.ThrowIfHasError();
            }

            var pontos = _pontuacaoFidelidadeEmpresaVendaRepository.FindByIdEmpresaVenda(pontuacao.IdEmpresaVenda);

            if (pontos.IsEmpty())
            {
                bre.AddError(GlobalMessages.ErroPontosContabilizados).Complete();
                bre.ThrowIfHasError();
            }

            pontos.PontuacaoSolicitada -= resgate.Pontuacao;
            pontos.PontuacaoResgatada += resgate.Pontuacao;
            _pontuacaoFidelidadeEmpresaVendaRepository.Save(pontos);

            resgate.SituacaoResgate = SituacaoResgate.Liberado;
            resgate.Voucher = pontuacao.Voucher;
            resgate.DataLiberacao = DateTime.Now;

            _resgatePontuacaoFidelidadeRepository.Save(resgate);

            var notificacao = new Notificacao
            {
                Usuario = resgate.SolicitadoPor,
                EmpresaVenda = resgate.EmpresaVenda,
                Titulo = GlobalMessages.TituloSolicitacaoResgateDisponivel,
                Conteudo = string.Format(GlobalMessages.MsgSolicitacaoResgateDisponivel, resgate.DataResgate.ToDate()),
                NomeBotao = GlobalMessages.IrParaProgramaFidelidade,
                Link = ProjectProperties.EvsBaseUrl + "/ProgramaFidelidade",
                DestinoNotificacao = DestinoNotificacao.Portal,
            };

            _notificacaoRepository.Save(notificacao);

            var corretores = _corretorRepository.ListarDiretoresAtivosDaEmpresaDeVendas(resgate.EmpresaVenda.Id);
            foreach (var corr in corretores)
            {
                EnviarEmail(corr.Email, corr.Nome, DateTime.Now.ToDate(), resgate.Pontuacao.ToString("0.##"));
            }

        }

        public void ReprovarResgate(PontuacaoFidelidadeDTO pontuacao)
        {
            var bre = new BusinessRuleException();

            if (pontuacao.Motivo.IsEmpty())
            {
                bre.AddError(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Motivo)).Complete();
                bre.ThrowIfHasError();
            }

            var resgate = _resgatePontuacaoFidelidadeRepository.FindById(pontuacao.IdResgate);

            if (resgate.IsEmpty())
            {
                bre.AddError(string.Format(GlobalMessages.DadoInvalido, GlobalMessages.Resgate)).Complete();
                bre.ThrowIfHasError();
            }

            if (resgate.SituacaoResgate != SituacaoResgate.Solicitado)
            {
                bre.AddError(GlobalMessages.MsgErroResgateAnalisado).Complete();
                bre.ThrowIfHasError();
            }

            var pontos = _pontuacaoFidelidadeEmpresaVendaRepository.FindByIdEmpresaVenda(pontuacao.IdEmpresaVenda);

            if (pontos.IsEmpty())
            {
                bre.AddError(GlobalMessages.ErroPontosContabilizados).Complete();
                bre.ThrowIfHasError();
            }

            pontos.PontuacaoSolicitada -= resgate.Pontuacao;
            pontos.PontuacaoDisponivel += resgate.Pontuacao;

            //resgate.SituacaoResgate = SituacaoResgate.Reprovado;
            resgate.Motivo = pontuacao.Motivo;

            _resgatePontuacaoFidelidadeRepository.Save(resgate);

        }

        public byte[] ExportarPontuacao(DataSourceRequest request, PontuacaoFidelidadeDTO filtro)
        {
            var resultados = _viewConsolidadoPontuacaoFidelidadeRepository.ListarPontuacao(request, filtro)
                .records.ToList();

            ExcelUtil excel = ExcelUtil.NewInstance(25)
                .NewSheet(DateTime.Now.ToString(GlobalMessages.PontuacaoFidelidade))
                .WithHeader(GetHeaderPontuacao());

            foreach (var model in resultados)
            {
                excel
                    .CreateCellValue(model.CodigoProposta).Width(10)
                    .CreateCellValue(model.NomeEmpreendimento).Width(50)
                    .CreateCellValue(model.Pontuacao).Width(20)
                    .CreateCellValue(model.Codigo).Width(20)
                    .CreateDateTimeCell(model.DataPontuacao).Width(20)
                    .CreateCellValue(model.SituacaoPontuacao.AsString()).Width(50);
            }

            excel.Close();
            return excel.DownloadFile();
        }

        private string[] GetHeaderPontuacao()
        {
            IList<string> header = new List<string>
            {
                GlobalMessages.CodigoProposta,
                GlobalMessages.Empreendimento,
                GlobalMessages.Pontuacao,
                GlobalMessages.Codigo,
                GlobalMessages.DataPontuacao,
                GlobalMessages.Situacao
            };
            return header.ToArray();
        }

        public byte[] ExportarExtrato(DataSourceRequest request, PontuacaoFidelidadeDTO filtro)
        {
            var resultados = _viewResgatePontuacaoFidelidadeRepository.ListarResgate(request, filtro)
                .records.ToList();

            ExcelUtil excel = ExcelUtil.NewInstance(25)
                .NewSheet(DateTime.Now.ToString(GlobalMessages.Extrato))
                .WithHeader(GetHeaderExtrato());

            foreach (var model in resultados)
            {
                excel
                    .CreateCellValue(model.Codigo).Width(10)
                    .CreateCellValue(model.Pontuacao).Width(50)
                    .CreateCellValue(model.SituacaoResgate.AsString()).Width(20)
                    .CreateCellValue(model.Motivo).Width(20)
                    .CreateDateTimeCell(model.DataResgate).Width(20)
                    .CreateCellValue(model.Voucher).Width(50)
                    .CreateCellValue(model.DataLiberacao.HasValue() ? model.DataLiberacao.Value.ToDate() : "").Width(50);
            }

            excel.Close();
            return excel.DownloadFile();
        }

        private string[] GetHeaderExtrato()
        {
            IList<string> header = new List<string>
            {
                GlobalMessages.Codigo,
                GlobalMessages.Pontos,
                GlobalMessages.Situacao,
                GlobalMessages.Motivo,
                GlobalMessages.DataPontuacao,
                GlobalMessages.Voucher,
                GlobalMessages.DataLiberacao
            };
            return header.ToArray();
        }

        public byte[] ExportarEfetivarResgate(DataSourceRequest request, PontuacaoFidelidadeDTO filtro)
        {
            var resultados = _viewResgatePontuacaoFidelidadeRepository.ListarResgate(request, filtro)
                .records.ToList();

            ExcelUtil excel = ExcelUtil.NewInstance(25)
                .NewSheet(DateTime.Now.ToString(GlobalMessages.Extrato))
                .WithHeader(GetHeaderEfetivarResgate());

            foreach (var model in resultados)
            {
                excel
                    .CreateCellValue(model.Codigo).Width(10)
                    .CreateCellValue(model.NomeEmpresaVenda).Width(10)
                    .CreateCellValue(model.NomeSolicitadoPor).Width(10)
                    .CreateCellValue(model.Pontuacao).Width(50)
                    .CreateCellValue(model.SituacaoResgate.AsString()).Width(20)
                    .CreateCellValue(model.Motivo).Width(20)
                    .CreateDateTimeCell(model.DataResgate).Width(20)
                    .CreateCellValue(model.Voucher).Width(50);
            }

            excel.Close();
            return excel.DownloadFile();
        }

        private string[] GetHeaderEfetivarResgate()
        {
            IList<string> header = new List<string>
            {
                GlobalMessages.Codigo,
                GlobalMessages.EmpresaVenda,
                GlobalMessages.Solicitante,
                GlobalMessages.Pontos,
                GlobalMessages.Situacao,
                GlobalMessages.Motivo,
                GlobalMessages.DataPontuacao,
                GlobalMessages.Voucher
            };
            return header.ToArray();
        }

        public Arquivo GerarPdf(long idEmpresaVenda, byte[] logo)
        {
            var EmpresaVenda = _empresaVendaRepository.FindById(idEmpresaVenda);
            var files = _pontuacaoFidelidadePdfService.CriarPdf(EmpresaVenda, logo);
            var file = files[0];

            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            var fileName = $"PontuacaoFidelidade_{date}.pdf";

            Arquivo arquivo = new Arquivo();
            arquivo.Nome = fileName;
            arquivo.ContentType = "application/pdf";
            arquivo.Content = file;

            return arquivo;
        }
        //download do ADMIN
        public Arquivo GerarPdfAdmin(long idPontuacaoFidelidade,long idEmpresaVenda, byte[] logo)
        {
            var EmpresaVenda = _empresaVendaRepository.FindById(idEmpresaVenda);
            var files = _pontuacaoFidelidadePdfService.CriarPdfAdmin(idPontuacaoFidelidade,EmpresaVenda, logo);
            var file = files[0];

            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            var fileName = $"PontuacaoFidelidade_{date}.pdf";

            Arquivo arquivo = new Arquivo();
            arquivo.Nome = fileName;
            arquivo.ContentType = "application/pdf";
            arquivo.Content = file;

            return arquivo;
        }

        public Arquivo GerarPdfAdmin(long idPontuacaoFidelidade,byte[] logo)
        {
            var files = _pontuacaoFidelidadePdfService.CriarPdfAdmin(idPontuacaoFidelidade, logo);
            var file = files[0];

            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            var fileName = $"PontuacaoFidelidade_{date}.pdf";

            Arquivo arquivo = new Arquivo();
            arquivo.Nome = fileName;
            arquivo.ContentType = "application/pdf";
            arquivo.Content = file;

            return arquivo;
        }

        public void EnviarEmail(string emailDestino, string nome, string data, string pontos)
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
            toReplace.Add("data", data);
            toReplace.Add("pontos", pontos);
            toReplace.Add("link", siteUrl);
            var templateName = "voucher-disponivel.html";
            var corpoEmail = TemplateEmailFactory.ResolveTemplateWithReplace(templateName, toReplace);

            var email = EmailService.CriarEmail(emailDestino, "[Tenda] Portal de EV - Voucher Disponível", corpoEmail);
            EmailService.EnviarEmail(email);
        }
    }
}
