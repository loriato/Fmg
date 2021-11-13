using Europa.Extensions;
using Europa.Resources;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.Domain.Repository;
using Color = iTextSharp.text.Color;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class RegraComissaoPdfService
    {
        public EnderecoEmpreendimentoRepository _enderecoEmpreendimentoRepository { get; set; }
        public ItemRegraComissaoRepository _itemRegraComissaoRepository { get; set; }
        public RegraComissaoEvsRepository _regraComissaoEvsRepository { get; set; }

        private bool _fontInitialized;

        private Font TitleFont;
        private Font RegionalFont;
        private Font EmpresaVendaFont;
        private Font ColumnFont;
        private Font TextFont;
        private Font RedTextFont;

        public RegraComissaoPdfService()
        {
            
        }

        #region Regra de Comissão Padrão Job
        //JOB - 2
        public byte[] CriarPdfJob(RegraComissao regraComissao,RegraComissaoEvs regraComissaoEvs,List<ItemRegraComissao> itens, byte[] logo)
        {
            EnsureFontInitialized();

            var stream = new MemoryStream();
            var documentoGeral = CreateDocument();

            PdfWriter.GetInstance(documentoGeral, stream);

            documentoGeral.Open();

            var empreendimentos = _enderecoEmpreendimentoRepository.EnderecosDaRegional(regraComissao.Regional)
                .OrderByDescending(reg => reg.Empreendimento.PriorizarRegraComissao)
                .ThenBy(reg => reg.Empreendimento.Nome)
                .ToList();

            var hasAnyPage = false;

            var bytes = CriarPdfJob(regraComissao, regraComissaoEvs, empreendimentos,itens, logo);
            
            if (hasAnyPage)
            {
                documentoGeral.NewPage();
            }

            IncluirConteudoJob(documentoGeral, regraComissao, regraComissaoEvs, empreendimentos,itens, logo);
            hasAnyPage = true;

            documentoGeral.Close();
            
            return bytes;
        }

        //JOB - 3
        private byte[] CriarPdfJob(RegraComissao regraComissao, RegraComissaoEvs regraComissaoEvs,
            List<EnderecoEmpreendimento> empreendimentos, List<ItemRegraComissao> itens, byte[] logo)
        {
            EnsureFontInitialized();

            var stream = new MemoryStream();
            var documentoGeral = CreateDocument();

            PdfWriter.GetInstance(documentoGeral, stream);

            documentoGeral.Open();

            IncluirConteudoJob(documentoGeral, regraComissao, regraComissaoEvs, empreendimentos, itens, logo);

            documentoGeral.Close();

            return stream.ToArray();
        }

        //JOB - 4
        private void IncluirConteudoJob(Document document, RegraComissao regraComissao, RegraComissaoEvs regraComissaoEvs,
            List<EnderecoEmpreendimento> enderecoEmpreendimentos, List<ItemRegraComissao> itens, byte[] logo)
        {
            IncluirConteudoJob(document, regraComissao, regraComissaoEvs, enderecoEmpreendimentos, itens, logo, TipoModalidadeComissao.Fixa);
            document.NewPage();
            IncluirConteudoJob(document, regraComissao, regraComissaoEvs, enderecoEmpreendimentos, itens, logo, TipoModalidadeComissao.Nominal);
        }

        //JOB - 5
        private void IncluirConteudoJob(Document document, RegraComissao regraComissao, RegraComissaoEvs regraComissaoEvs,
            List<EnderecoEmpreendimento> enderecoEmpreendimentos, List<ItemRegraComissao> itens, byte[] logo, TipoModalidadeComissao modalidade)
        {
            var itensAtuais = itens;

            var page = CriarConteudo(regraComissaoEvs, enderecoEmpreendimentos.ToList(), itens, itensAtuais, logo, modalidade);
            document.Add(page);
        }

        #endregion

        private void EnsureFontInitialized()
        {
            try
            {
                if (!_fontInitialized)
                {
                    FontFactory.RegisterDirectory("C:\\WINDOWS\\Fonts");
                    TitleFont = FontFactory.GetFont("Arial", 13, Font.BOLD);
                    RegionalFont = FontFactory.GetFont("Arial", 7, Font.BOLD);
                    EmpresaVendaFont = FontFactory.GetFont("Arial", 7, Font.BOLD);
                    ColumnFont = FontFactory.GetFont("Arial", 7, Font.BOLD, Color.WHITE);
                    TextFont = FontFactory.GetFont("Arial", 7);
                    RedTextFont = FontFactory.GetFont("Arial", 7, Font.BOLD, new Color(175, 10, 10));
                }
            }catch(Exception ex)
            {
                ExceptionLogger.LogException(ex);
                throw;
            }
        }

        //2
        public Dictionary<long, byte[]> CriarPdf(RegraComissao regraComissao, byte[] logo)
        {

            EnsureFontInitialized();

            var documentos = new Dictionary<long, byte[]>();

            var stream = new MemoryStream();
            var documentoGeral = CreateDocument();

            PdfWriter.GetInstance(documentoGeral, stream);

            documentoGeral.Open();

            var regrasEvs = _regraComissaoEvsRepository.BuscarPorRegraComissao(regraComissao.Id);
            var empreendimentos = _enderecoEmpreendimentoRepository.EnderecosDaRegional(regraComissao.Regional)
                .OrderByDescending(reg => reg.Empreendimento.PriorizarRegraComissao)
                .ThenBy(reg => reg.Empreendimento.Nome)
                .ToList();

            var hasAnyPage = false;
            foreach (var rev in regrasEvs)
            {
                var bytes = CriarPdf(regraComissao, rev, empreendimentos, logo);
                documentos.Add(rev.EmpresaVenda.Id, bytes);

                if (hasAnyPage)
                {
                    documentoGeral.NewPage();
                }

                IncluirConteudo(documentoGeral, regraComissao, rev, empreendimentos, logo);
                hasAnyPage = true;
            }

            documentoGeral.Close();

            documentos.Add(0, stream.ToArray());

            return documentos;
        }

        public Dictionary<long, byte[]> CriarPdf(RegraComissao regraComissao,RegraComissaoEvs regraComissaoEvs,
            List<ItemRegraComissao> itens,byte[] logo)
        {
            EnsureFontInitialized();

            var documentos = new Dictionary<long, byte[]>();

            var stream = new MemoryStream();
            var documentoGeral = CreateDocument();

            PdfWriter.GetInstance(documentoGeral, stream);

            documentoGeral.Open();

            //var regrasEvs = _regraComissaoEvsRepository.BuscarPorRegraComissao(regraComissao.Id);
            var empreendimentos = _enderecoEmpreendimentoRepository.EnderecosDaRegional(regraComissao.Regional)
                .OrderByDescending(reg => reg.Empreendimento.PriorizarRegraComissao)
                .ThenBy(reg => reg.Empreendimento.Nome)
                .ToList();

            var hasAnyPage = false;

            var bytes = CriarPdf(regraComissao, regraComissaoEvs, empreendimentos,itens, logo);
            documentos.Add(regraComissaoEvs.EmpresaVenda.Id, bytes);

            if (hasAnyPage)
            {
                documentoGeral.NewPage();
            }

            IncluirConteudo(documentoGeral, regraComissao, regraComissaoEvs, empreendimentos, logo);
            hasAnyPage = true;
            
            documentoGeral.Close();

            documentos.Add(0, stream.ToArray());

            return documentos;
        }

        private byte[] CriarPdf(RegraComissao regraComissao, RegraComissaoEvs regraComissaoEvs,
            List<EnderecoEmpreendimento> empreendimentos,List<ItemRegraComissao>itens, byte[] logo)
        {
            EnsureFontInitialized();

            var stream = new MemoryStream();
            var documentoGeral = CreateDocument();

            PdfWriter.GetInstance(documentoGeral, stream);

            documentoGeral.Open();

            IncluirConteudo(documentoGeral, regraComissao, regraComissaoEvs, empreendimentos,itens, logo);

            documentoGeral.Close();

            return stream.ToArray();
        }
        
        //3
        private byte[] CriarPdf(RegraComissao regraComissao, RegraComissaoEvs regraComissaoEvs,
            List<EnderecoEmpreendimento> empreendimentos, byte[] logo)
        {
            EnsureFontInitialized();

            var stream = new MemoryStream();
            var documentoGeral = CreateDocument();

            PdfWriter.GetInstance(documentoGeral, stream);

            documentoGeral.Open();

            IncluirConteudo(documentoGeral, regraComissao, regraComissaoEvs, empreendimentos, logo);

            documentoGeral.Close();

            return stream.ToArray();
        }

        private Document CreateDocument()
        {
            var document = new Document(PageSize.A4.Rotate());
            document.SetMargins(10, 10, 10, 10);
            document.AddCreationDate();

            return document;
        }

        private void IncluirConteudo(Document document, RegraComissao regraComissao, RegraComissaoEvs regraComissaoEvs,
            List<EnderecoEmpreendimento> enderecoEmpreendimentos,List<ItemRegraComissao>itens, byte[] logo)
        {
            IncluirConteudo(document, regraComissaoEvs, enderecoEmpreendimentos, logo,itens, TipoModalidadeComissao.Fixa);
            document.NewPage();
            IncluirConteudo(document, regraComissaoEvs, enderecoEmpreendimentos, logo,itens, TipoModalidadeComissao.Nominal);
        }
        
        //4
        private void IncluirConteudo(Document document, RegraComissao regraComissao, RegraComissaoEvs regraComissaoEvs,
            List<EnderecoEmpreendimento> enderecoEmpreendimentos, byte[] logo)
        {
            IncluirConteudo(document, regraComissao, regraComissaoEvs, enderecoEmpreendimentos, logo, TipoModalidadeComissao.Fixa);
            document.NewPage();
            IncluirConteudo(document, regraComissao, regraComissaoEvs, enderecoEmpreendimentos, logo, TipoModalidadeComissao.Nominal);
        }

        private void IncluirConteudo(Document document , RegraComissaoEvs regraComissaoEvs,
            List<EnderecoEmpreendimento> enderecoEmpreendimentos, byte[] logo,
            List<ItemRegraComissao> itensAtuais, TipoModalidadeComissao modalidade)
        {
            var itens = new List<ItemRegraComissao>();

            var page = CriarConteudo(regraComissaoEvs, enderecoEmpreendimentos.ToList(), itens, itensAtuais, logo, modalidade);
            document.Add(page);            
        }
        
        //5
        private void IncluirConteudo(Document document, RegraComissao regraComissao, RegraComissaoEvs regraComissaoEvs,
            List<EnderecoEmpreendimento> enderecoEmpreendimentos, byte[] logo, TipoModalidadeComissao modalidade)
        {
            var itens = _itemRegraComissaoRepository.ItensDeRegra(regraComissao.Id)
                .Where(x => x.EmpresaVenda.Id == regraComissaoEvs.EmpresaVenda.Id)
                .ToList();
            var itensAtuais = itens;
            if (regraComissaoEvs.Situacao == SituacaoRegraComissao.Rascunho)
            {
                var regraEvAtiva = _regraComissaoEvsRepository
                    .BuscarRegraEvsVigente(regraComissaoEvs.EmpresaVenda.Id);
                if (regraEvAtiva.HasValue())
                {
                    itensAtuais = _itemRegraComissaoRepository
                        .BuscarItens(regraEvAtiva.RegraComissao.Id, regraEvAtiva.EmpresaVenda.Id);
                }
                else
                {
                    itensAtuais = new List<ItemRegraComissao>();
                }
            }

            var page = CriarConteudo(regraComissaoEvs, enderecoEmpreendimentos.ToList(), itens, itensAtuais, logo, modalidade);
            document.Add(page);
        }
        //6
        private IElement CriarConteudo(RegraComissaoEvs regraComissaoEvs,
            List<EnderecoEmpreendimento> enderecoEmpreendimentos, List<ItemRegraComissao> itens,
            List<ItemRegraComissao> itensAtivos, byte[] logo, TipoModalidadeComissao modalidade)
        {
            var numColunas = 28;
            var offset = 0;
            if (modalidade == TipoModalidadeComissao.Nominal)
            {
                numColunas = 32;
                offset = 4;
            }

            var paddingLeft = 5;
            var headerPaddingBottom = 20;

            var pagina = new PdfPTable(numColunas);
            pagina.WidthPercentage = 100;

            var logoCell = CreateImageCell(logo);
            logoCell.PaddingBottom = headerPaddingBottom;
            logoCell.PaddingLeft = paddingLeft;
            pagina.AddCell(logoCell);

            var headerOffsetCell = CreateCell("", TitleFont, 8);
            headerOffsetCell.PaddingBottom = headerPaddingBottom;
            pagina.AddCell(headerOffsetCell);

            var tituloCell = CreateCell(GlobalMessages.RegraComissao.ToUpper(), TitleFont,
                14 + offset);
            tituloCell.HorizontalAlignment = Element.ALIGN_CENTER;
            tituloCell.PaddingTop = 15;
            pagina.AddCell(tituloCell);

            var regionalTituloCell = CreateCell(GlobalMessages.Regional.ToUpper(), RegionalFont, 6);
            regionalTituloCell.PaddingLeft = paddingLeft;
            pagina.AddCell(regionalTituloCell);

            var regionalCell = CreateCell(regraComissaoEvs.EmpresaVenda.Estado.ToUpper(), TextFont, 22 + offset * 2);
            pagina.AddCell(regionalCell);

            var empresaVendaTituloCell = CreateCell(GlobalMessages.EmpresaVendas.ToUpper(), EmpresaVendaFont, 6);
            empresaVendaTituloCell.PaddingLeft = paddingLeft;
            empresaVendaTituloCell.PaddingTop = 0;
            empresaVendaTituloCell.PaddingBottom = 15;
            pagina.AddCell(empresaVendaTituloCell);

            var empresaVendaCell = CreateCell(regraComissaoEvs.EmpresaVenda.NomeFantasia.ToUpper(), TextFont, 22 + offset * 2);
            empresaVendaCell.PaddingTop = 0;
            empresaVendaCell.PaddingBottom = 15;
            pagina.AddCell(empresaVendaCell);

            int colspanEmpreendimento = 6,
                colspanCidade = 4,
                colspanFaixaUmMeio = 2,
                colspanFaixaDois = 2,
                colspanValorKitCompleto = 5,
                colspanValorConformidade = 5,
                colspanValorRepasse = 5,
                colspanMenorValorNominal = 2,
                colspanValorNominal = 2,
                colspanMaiorValorNominal = 2;

            if (enderecoEmpreendimentos.Any())
            {
                var headerBackgroundColor = new Color(90, 90, 90);
                var borderColor = new Color(240, 240, 240);
                var border = Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                var borderWidth = 1;

                var comissaoCell = CreateCell(GlobalMessages.Comissao.ToUpper(), ColumnFont,
                        colspanFaixaUmMeio + colspanFaixaDois);

                if (modalidade == TipoModalidadeComissao.Nominal)
                {
                    comissaoCell = CreateCell(GlobalMessages.Comissao.ToUpper(), ColumnFont,
                        colspanMenorValorNominal * 3 + colspanValorNominal * 3 + colspanMaiorValorNominal * 3);

                    colspanValorKitCompleto = 2;
                    colspanValorConformidade = 2;
                    colspanValorRepasse = 2;

                    colspanEmpreendimento -= 2;
                }

                var offsetCell = CreateCell("", TitleFont, colspanCidade + colspanEmpreendimento);
                offsetCell.BorderColor = borderColor;
                offsetCell.Border = Rectangle.RIGHT_BORDER | Rectangle.BOTTOM_BORDER;
                offsetCell.BorderWidth = borderWidth;
                offsetCell.PaddingLeft = paddingLeft;
                pagina.AddCell(offsetCell);

                comissaoCell.HorizontalAlignment = Element.ALIGN_CENTER;
                comissaoCell.BackgroundColor = headerBackgroundColor;
                comissaoCell.BorderColor = borderColor;
                comissaoCell.Border = border | Rectangle.BOTTOM_BORDER;
                comissaoCell.BorderWidth = borderWidth;
                comissaoCell.PaddingLeft = paddingLeft;
                pagina.AddCell(comissaoCell);

                var regraPagamentoCell = CreateCell(GlobalMessages.RegraPagamento.ToUpper(), ColumnFont,
                    colspanValorKitCompleto + colspanValorConformidade + colspanValorRepasse);
                regraPagamentoCell.BackgroundColor = headerBackgroundColor;
                regraPagamentoCell.BorderColor = borderColor;
                regraPagamentoCell.Border = Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER;
                regraPagamentoCell.BorderWidth = borderWidth;
                regraPagamentoCell.PaddingLeft = paddingLeft;
                regraPagamentoCell.HorizontalAlignment = Element.ALIGN_CENTER;
                pagina.AddCell(regraPagamentoCell);

                var empreendimentoCell = CreateCell(GlobalMessages.Empreendimento.ToUpper(), ColumnFont,
                    colspanEmpreendimento);
                empreendimentoCell.BackgroundColor = headerBackgroundColor;
                empreendimentoCell.BorderColor = borderColor;
                empreendimentoCell.Border = Rectangle.RIGHT_BORDER;
                empreendimentoCell.BorderWidth = borderWidth;
                empreendimentoCell.PaddingLeft = paddingLeft;
                pagina.AddCell(empreendimentoCell);

                var cidadeCell = CreateCell(GlobalMessages.Cidade.ToUpper(), ColumnFont, colspanCidade);
                cidadeCell.BackgroundColor = headerBackgroundColor;
                cidadeCell.BorderColor = borderColor;
                cidadeCell.Border = border;
                cidadeCell.BorderWidth = borderWidth;
                cidadeCell.PaddingLeft = paddingLeft;
                pagina.AddCell(cidadeCell);

                switch (modalidade)
                {
                    case TipoModalidadeComissao.Fixa:
                        var faixaUmMeioCell = CreateCell(GlobalMessages.FaixaUmMeio.ToUpper(), ColumnFont,
                    colspanFaixaUmMeio);
                        faixaUmMeioCell.BackgroundColor = headerBackgroundColor;
                        faixaUmMeioCell.BorderColor = borderColor;
                        faixaUmMeioCell.Border = border;
                        faixaUmMeioCell.BorderWidth = borderWidth;
                        faixaUmMeioCell.PaddingLeft = paddingLeft;
                        pagina.AddCell(faixaUmMeioCell);

                        var faixaDoisCell = CreateCell(GlobalMessages.FaixaDois.ToUpper(), ColumnFont,
                            colspanFaixaDois);
                        faixaDoisCell.BackgroundColor = headerBackgroundColor;
                        faixaDoisCell.BorderColor = borderColor;
                        faixaDoisCell.Border = border;
                        faixaDoisCell.BorderWidth = borderWidth;
                        faixaDoisCell.PaddingLeft = paddingLeft;
                        pagina.AddCell(faixaDoisCell);
                        break;

                    case TipoModalidadeComissao.Nominal:
                        var menorValorNominal = CreateCell(GlobalMessages.ColunaMenorValorNominalUmMeio.ToUpper(), ColumnFont,
                            colspanMenorValorNominal);
                        menorValorNominal.BackgroundColor = headerBackgroundColor;
                        menorValorNominal.BorderColor = borderColor;
                        menorValorNominal.Border = Rectangle.LEFT_BORDER;
                        menorValorNominal.BorderWidth = borderWidth;
                        menorValorNominal.PaddingLeft = paddingLeft;
                        pagina.AddCell(menorValorNominal);

                        var valorNominal = CreateCell(GlobalMessages.ColunaIgualValorNominalUmMeio.ToUpper(), ColumnFont,
                            colspanValorNominal);
                        valorNominal.BackgroundColor = headerBackgroundColor;
                        valorNominal.BorderColor = borderColor;
                        valorNominal.Border = Rectangle.LEFT_BORDER;
                        valorNominal.BorderWidth = borderWidth;
                        valorNominal.PaddingLeft = paddingLeft;
                        pagina.AddCell(valorNominal);

                        var maiorValorNominal = CreateCell(GlobalMessages.ColunaMaiorValorNominalUmMeio.ToUpper(), ColumnFont,
                            colspanValorNominal);
                        maiorValorNominal.BackgroundColor = headerBackgroundColor;
                        maiorValorNominal.BorderColor = borderColor;
                        maiorValorNominal.Border = Rectangle.LEFT_BORDER;
                        maiorValorNominal.BorderWidth = borderWidth;
                        maiorValorNominal.PaddingLeft = paddingLeft;
                        pagina.AddCell(maiorValorNominal);

                        var faixaDoisSeca = CreateCell(GlobalMessages.ColunaMenorValorNominalDois.ToUpper(), ColumnFont,
                            colspanMenorValorNominal);
                        faixaDoisSeca.BackgroundColor = headerBackgroundColor;
                        faixaDoisSeca.BorderColor = borderColor;
                        faixaDoisSeca.Border = Rectangle.LEFT_BORDER;
                        faixaDoisSeca.BorderWidth = borderWidth;
                        faixaDoisSeca.PaddingLeft = paddingLeft;
                        pagina.AddCell(faixaDoisSeca);

                        var faixaDoisNormal = CreateCell(GlobalMessages.ColunaIgualValorNominalDois.ToUpper(), ColumnFont,
                            colspanValorNominal);
                        faixaDoisNormal.BackgroundColor = headerBackgroundColor;
                        faixaDoisNormal.BorderColor = borderColor;
                        faixaDoisNormal.Border = Rectangle.LEFT_BORDER;
                        faixaDoisNormal.BorderWidth = borderWidth;
                        faixaDoisNormal.PaddingLeft = paddingLeft;
                        pagina.AddCell(faixaDoisNormal);

                        var faixaDoisTurbinada = CreateCell(GlobalMessages.ColunaMaiorValorNominalDois.ToUpper(), ColumnFont,
                            colspanValorNominal);
                        faixaDoisTurbinada.BackgroundColor = headerBackgroundColor;
                        faixaDoisTurbinada.BorderColor = borderColor;
                        faixaDoisTurbinada.Border = Rectangle.LEFT_BORDER;
                        faixaDoisTurbinada.BorderWidth = borderWidth;
                        faixaDoisTurbinada.PaddingLeft = paddingLeft;
                        pagina.AddCell(faixaDoisTurbinada);

                        var faixaPNESeca = CreateCell(GlobalMessages.ColunaMenorValorNominalPNE.ToUpper(), ColumnFont,
                            colspanMenorValorNominal);
                        faixaPNESeca.BackgroundColor = headerBackgroundColor;
                        faixaPNESeca.BorderColor = borderColor;
                        faixaPNESeca.Border = Rectangle.LEFT_BORDER;
                        faixaPNESeca.BorderWidth = borderWidth;
                        faixaPNESeca.PaddingLeft = paddingLeft;
                        pagina.AddCell(faixaPNESeca);

                        var faixaPNENormal = CreateCell(GlobalMessages.ColunaIgualValorNominalPNE.ToUpper(), ColumnFont,
                            colspanValorNominal);
                        faixaPNENormal.BackgroundColor = headerBackgroundColor;
                        faixaPNENormal.BorderColor = borderColor;
                        faixaPNENormal.Border = Rectangle.LEFT_BORDER;
                        faixaPNENormal.BorderWidth = borderWidth;
                        faixaPNENormal.PaddingLeft = paddingLeft;
                        pagina.AddCell(faixaPNENormal);

                        var faixaPNETurbinada = CreateCell(GlobalMessages.ColunaMaiorValorNominalPNE.ToUpper(), ColumnFont,
                            colspanValorNominal);
                        faixaPNETurbinada.BackgroundColor = headerBackgroundColor;
                        faixaPNETurbinada.BorderColor = borderColor;
                        faixaPNETurbinada.Border = Rectangle.LEFT_BORDER;
                        faixaPNETurbinada.BorderWidth = borderWidth;
                        faixaPNETurbinada.PaddingLeft = paddingLeft;
                        pagina.AddCell(faixaPNETurbinada);
                        break;
                }

                var valorKitCompletoCell = CreateCell(GlobalMessages.KitCompleto.ToUpper(), ColumnFont,
                    colspanValorKitCompleto);
                valorKitCompletoCell.BackgroundColor = headerBackgroundColor;
                valorKitCompletoCell.BorderColor = borderColor;
                valorKitCompletoCell.Border = border;
                valorKitCompletoCell.BorderWidth = borderWidth;
                valorKitCompletoCell.PaddingLeft = paddingLeft;
                pagina.AddCell(valorKitCompletoCell);

                var valorConformidadeCell = CreateCell(GlobalMessages.Conformidade.ToUpper(), ColumnFont,
                    colspanValorConformidade);
                valorConformidadeCell.BackgroundColor = headerBackgroundColor;
                valorConformidadeCell.BorderColor = borderColor;
                valorConformidadeCell.Border = border;
                valorConformidadeCell.BorderWidth = borderWidth;
                valorConformidadeCell.PaddingLeft = paddingLeft;
                pagina.AddCell(valorConformidadeCell);

                var valorRepasseCell = CreateCell(GlobalMessages.Repasse.ToUpper(), ColumnFont,
                    colspanValorRepasse);
                valorRepasseCell.BackgroundColor = headerBackgroundColor;
                valorRepasseCell.BorderColor = borderColor;
                valorRepasseCell.Border = Rectangle.LEFT_BORDER;
                valorRepasseCell.BorderWidth = borderWidth;
                valorRepasseCell.PaddingLeft = paddingLeft;
                pagina.AddCell(valorRepasseCell);
            }

            foreach (var enderecoEmpreendimento in enderecoEmpreendimentos)
            {
                var itensPertinentes =
                    itens.Where(x => x.TipoModalidadeComissao == modalidade)
                    .Where(x => x.Empreendimento.Id == enderecoEmpreendimento.Empreendimento.Id);
                if (itensPertinentes.Any())
                {
                    foreach (var itemRegraComissao in itensPertinentes)
                    {
                        var itemAtivo = itensAtivos.Where(x =>
                            x.Empreendimento.Id == enderecoEmpreendimento.Empreendimento.Id).FirstOrDefault();
                        if (itemAtivo.IsEmpty())
                        {
                            itemAtivo = new ItemRegraComissao();
                        }

                        var empreendimentoCell = CreateCell(enderecoEmpreendimento.Empreendimento.Nome, TextFont,
                            colspanEmpreendimento);
                        empreendimentoCell.PaddingLeft = paddingLeft;
                        pagina.AddCell(empreendimentoCell);

                        var cidadeCell = CreateCell(enderecoEmpreendimento.Cidade, TextFont, colspanCidade);
                        pagina.AddCell(cidadeCell);
                        switch (modalidade)
                        {
                            case TipoModalidadeComissao.Fixa:
                                var faixaUmMeioCell = CreateCell(itemRegraComissao.FaixaUmMeio + "%",
                            GetItemFont(itemRegraComissao.FaixaUmMeio, itemAtivo.FaixaUmMeio),
                            colspanFaixaUmMeio);
                                faixaUmMeioCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                pagina.AddCell(faixaUmMeioCell);

                                var faixaDoisCell = CreateCell(itemRegraComissao.FaixaDois + "%",
                                    GetItemFont(itemRegraComissao.FaixaDois, itemAtivo.FaixaDois),
                                    colspanFaixaDois);
                                faixaDoisCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                pagina.AddCell(faixaDoisCell);
                                break;
                            case TipoModalidadeComissao.Nominal:
                                var faixaUmMeioSecaCell = CreateCell(itemRegraComissao.MenorValorNominalUmMeio + "%",
                                    GetItemFont(itemRegraComissao.MenorValorNominalUmMeio, itemAtivo.MenorValorNominalUmMeio),
                                    colspanMenorValorNominal);
                                faixaUmMeioSecaCell.BackgroundColor = new Color(200, 200, 200);
                                faixaUmMeioSecaCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                pagina.AddCell(faixaUmMeioSecaCell);

                                var faixaUmMeioNormalCell = CreateCell(itemRegraComissao.IgualValorNominalUmMeio + "%",
                                    GetItemFont(itemRegraComissao.IgualValorNominalUmMeio, itemAtivo.IgualValorNominalUmMeio),
                                    colspanValorNominal);
                                faixaUmMeioNormalCell.BackgroundColor = new Color(200, 200, 200);
                                faixaUmMeioNormalCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                pagina.AddCell(faixaUmMeioNormalCell);

                                var faixaUmMeioTurbinadaCell = CreateCell(itemRegraComissao.MaiorValorNominalUmMeio + "%",
                                    GetItemFont(itemRegraComissao.MaiorValorNominalUmMeio, itemAtivo.MaiorValorNominalUmMeio),
                                    colspanMaiorValorNominal);
                                faixaUmMeioTurbinadaCell.BackgroundColor = new Color(200, 200, 200);
                                faixaUmMeioTurbinadaCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                pagina.AddCell(faixaUmMeioTurbinadaCell);

                                var faixaDoisSecaCell = CreateCell(itemRegraComissao.MenorValorNominalDois + "%",
                                    GetItemFont(itemRegraComissao.MenorValorNominalDois, itemAtivo.MenorValorNominalDois),
                                    colspanMenorValorNominal);
                                faixaDoisSecaCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                pagina.AddCell(faixaDoisSecaCell);

                                var faixaDoisNormalCell = CreateCell(itemRegraComissao.IgualValorNominalDois + "%",
                                    GetItemFont(itemRegraComissao.IgualValorNominalDois, itemAtivo.IgualValorNominalDois),
                                    colspanValorNominal);
                                faixaDoisNormalCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                pagina.AddCell(faixaDoisNormalCell);

                                var faixaDoisTurbinadaCell = CreateCell(itemRegraComissao.MaiorValorNominalDois + "%",
                                    GetItemFont(itemRegraComissao.MaiorValorNominalDois, itemAtivo.MaiorValorNominalDois),
                                    colspanMaiorValorNominal);
                                faixaDoisTurbinadaCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                pagina.AddCell(faixaDoisTurbinadaCell);

                                var faixaPNESecaCell = CreateCell(itemRegraComissao.MenorValorNominalPNE + "%",
                                    GetItemFont(itemRegraComissao.MenorValorNominalPNE, itemAtivo.MenorValorNominalPNE),
                                    colspanMenorValorNominal);
                                faixaPNESecaCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                faixaPNESecaCell.BackgroundColor = new Color(200, 200, 200);
                                pagina.AddCell(faixaPNESecaCell);

                                var faixaPNENormalCell = CreateCell(itemRegraComissao.IgualValorNominalPNE + "%",
                                    GetItemFont(itemRegraComissao.IgualValorNominalPNE, itemAtivo.IgualValorNominalPNE),
                                    colspanValorNominal);
                                faixaPNENormalCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                faixaPNENormalCell.BackgroundColor = new Color(200, 200, 200);
                                pagina.AddCell(faixaPNENormalCell);

                                var faixaPNETurbinadaCell = CreateCell(itemRegraComissao.MaiorValorNominalPNE + "%",
                                    GetItemFont(itemRegraComissao.MaiorValorNominalPNE, itemAtivo.MaiorValorNominalPNE),
                                    colspanMaiorValorNominal);
                                faixaPNETurbinadaCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                faixaPNETurbinadaCell.BackgroundColor = new Color(200, 200, 200);
                                pagina.AddCell(faixaPNETurbinadaCell);
                                break;
                        }

                        var valorKitCompletoCell = CreateCell(itemRegraComissao.ValorKitCompleto + "%",
                            GetItemFont(itemRegraComissao.ValorKitCompleto, itemAtivo.ValorKitCompleto),
                            colspanValorKitCompleto);
                        valorKitCompletoCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        pagina.AddCell(valorKitCompletoCell);

                        var valorConformidadeCell = CreateCell(itemRegraComissao.ValorConformidade + "%",
                            GetItemFont(itemRegraComissao.ValorConformidade, itemAtivo.ValorConformidade),
                            colspanValorConformidade);
                        valorConformidadeCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        pagina.AddCell(valorConformidadeCell);

                        var valorRepasseCell = CreateCell(itemRegraComissao.ValorRepasse + "%",
                            GetItemFont(itemRegraComissao.ValorRepasse, itemAtivo.ValorRepasse),
                            colspanValorRepasse);
                        valorRepasseCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                        pagina.AddCell(valorRepasseCell);

                    }
                }
            }

            var description = CreateCell("* " + "Itens marcados em vermelho indicam modificações no valor em relação a regra de comissão atual.", TextFont, numColunas);
            description.BackgroundColor = Color.WHITE;
            description.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            pagina.AddCell(description);

            return pagina;
        }

        private PdfPCell CreateImageCell(byte[] logo)
        {
            Image logoImg = Image.GetInstance(logo);
            PdfPCell cell = new PdfPCell(logoImg, true);
            cell.Colspan = 6;
            cell.PaddingTop = 7;
            cell.PaddingBottom = 7;
            cell.Border = Rectangle.NO_BORDER;
            cell.VerticalAlignment = Element.ALIGN_CENTER;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new Color(240, 240, 240);

            return cell;
        }

        private PdfPCell CreateCell(string content, Font font, int colspan)
        {
            var chunk = new Chunk(content, font);
            var paragraph = new Paragraph(chunk);
            var cell = new PdfPCell(paragraph);
            cell.PaddingTop = 7;
            cell.PaddingBottom = 7;
            cell.Colspan = colspan;
            cell.Border = Rectangle.NO_BORDER;
            cell.VerticalAlignment = PdfPCell.ALIGN_CENTER;
            cell.BackgroundColor = new Color(240, 240, 240);
            return cell;
        }

        private Font GetItemFont(double value, double activeValue)
        {
            return Math.Abs(value - activeValue) > double.Epsilon ? RedTextFont : TextFont;
        }
    }
}