using Europa.Extensions;
using Europa.Resources;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.Domain.Repository;
using Color = iTextSharp.text.Color;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class PontuacaoFidelidadePdfService
    {
        private ItemPontuacaoFidelidadeRepository _itemPontuacaoFidelidadeRepository { get; set; }

        private Font TitleFont;
        private Font RegionalFont;
        private Font EmpresaVendaFont;
        private Font ColumnFont;
        private Font TextFont;
        private Font RedTextFont;

        private bool _fontInitialized;
        public PontuacaoFidelidadePdfService()
        {
            
        }
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
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
                throw;
            }
        }
        public Dictionary<long, byte[]> CriarPdf(Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda, byte[] logo)
        {

            EnsureFontInitialized();

            var documentos = new Dictionary<long, byte[]>();

            var stream = new MemoryStream();
            var documentoGeral = CreateDocument();

            PdfWriter.GetInstance(documentoGeral, stream);

            documentoGeral.Open();
            var i = 1;
            var hasAnyPage = false;

            var itens = _itemPontuacaoFidelidadeRepository.BuscarItemAtivoPorEmpresaVenda(empresaVenda.Id);

            var itensPadrao = itens.Where(x => x.PontuacaoFidelidade.TipoPontuacaoFidelidade == TipoPontuacaoFidelidade.Normal)
                .ToList();

            if (itensPadrao.HasValue())
            {
                var bytes = CriarPdf(empresaVenda, null, TipoPontuacaoFidelidade.Normal, logo, false, itensPadrao);
                documentos.Add(i++, bytes);

                if (hasAnyPage)
                {
                    documentoGeral.NewPage();
                }

                IncluirConteudo(documentoGeral, empresaVenda, null, TipoPontuacaoFidelidade.Normal, logo, false, itensPadrao);
                hasAnyPage = true;
            }

            foreach (TipoCampanhaFidelidade tpCampanha in Enum.GetValues(typeof(TipoCampanhaFidelidade)))
            {
                var itensCampanha = itens.Where(x => x.PontuacaoFidelidade.TipoCampanhaFidelidade == tpCampanha)
                .ToList();

                if (itensCampanha.HasValue())
                {
                    var bytes = CriarPdf(empresaVenda, null, TipoPontuacaoFidelidade.Normal, logo, false, itensCampanha);
                    documentos.Add(i++, bytes);

                    if (hasAnyPage)
                    {
                        documentoGeral.NewPage();
                    }

                    IncluirConteudo(documentoGeral, empresaVenda, null, TipoPontuacaoFidelidade.Normal, logo, false, itensCampanha);
                    hasAnyPage = true;
                }

            }

            if (documentos.Count == 0)
            {
                var bytes = CriarPdf(empresaVenda, null, TipoPontuacaoFidelidade.Normal, logo, true, new List<ItemPontuacaoFidelidade>());
                documentos.Add(i++, bytes);
                IncluirConteudo(documentoGeral, empresaVenda, null, TipoPontuacaoFidelidade.Normal, logo, true, new List<ItemPontuacaoFidelidade>());
            }

            documentoGeral.Close();

            documentos.Add(0, stream.ToArray());

            return documentos;
        }

        private Document CreateDocument()
        {
            var document = new Document(PageSize.A4);
            document.SetMargins(10, 10, 10, 10);
            document.AddCreationDate();

            return document;
        }

        private byte[] CriarPdf(Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda,
            TipoCampanhaFidelidade? tpCampanha, TipoPontuacaoFidelidade? tpPontuacao, byte[] logo, bool existe, List<ItemPontuacaoFidelidade> itens)
        {
            EnsureFontInitialized();

            var stream = new MemoryStream();
            var documentoGeral = CreateDocument();

            PdfWriter.GetInstance(documentoGeral, stream);

            documentoGeral.Open();

            IncluirConteudo(documentoGeral, empresaVenda, tpCampanha, tpPontuacao, logo, existe, itens);

            documentoGeral.Close();

            return stream.ToArray();
        }

        private void IncluirConteudo(Document document, Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda,
            TipoCampanhaFidelidade? tpCampanha, TipoPontuacaoFidelidade? tpPontuacao, byte[] logo, bool existe, List<ItemPontuacaoFidelidade> itens)
        {
            var pontuacaoFidelidade = itens.Select(x => x.PontuacaoFidelidade)
                .Distinct().FirstOrDefault();

            var empreendimentosFixa = itens.Where(x => x.Modalidade == TipoModalidadeProgramaFidelidade.Fixa)
                .Select(x => x.Empreendimento)
                .OrderBy(x => x.Nome)
                .ToList();

            var empreendimentosNominal = itens.Where(x => x.Modalidade == TipoModalidadeProgramaFidelidade.Nominal)
                .Select(x => x.Empreendimento)
                .OrderBy(x => x.Nome)
                .ToList();

            var hasAnyPage = false;
            if (empreendimentosFixa.Count() > 0)
            {
                var lista = itens.Where(x => empreendimentosFixa.Contains(x.Empreendimento)).ToList();
                var page = CriarConteudo(empreendimentosFixa.ToList(), pontuacaoFidelidade,
                logo, empresaVenda, TipoModalidadeProgramaFidelidade.Fixa, lista);

                document.Add(page);
                hasAnyPage = true;
            }
            if (empreendimentosNominal.Count() > 0)
            {
                if (hasAnyPage)
                {
                    document.NewPage();
                }
                var lista = itens.Where(x => empreendimentosNominal.Contains(x.Empreendimento)).ToList();
                var page = CriarConteudo(empreendimentosNominal.ToList(), pontuacaoFidelidade,
                logo, empresaVenda, TipoModalidadeProgramaFidelidade.Nominal, lista);

                document.Add(page);
            }
        }

        private IElement CriarConteudo(List<Empreendimento> empreendimentos,
            PontuacaoFidelidade pontuacaoFidelidade, byte[] logo,
            Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda, TipoModalidadeProgramaFidelidade modalidade, List<ItemPontuacaoFidelidade> itens)
        {
            var Data = DateTime.Now;
            var paddingLeft = 5;
            var headerPaddingBottom = 20;
            var numColunas = 0;

            int colspanHeaderOffset = 1,
                colspanTitle = 3,
                colspanRegional = 1,
                colspanDescricao = 1,
                colspanEmpresavenda = 1,
                colspanNomeEmpresavenda = 1,
                colspanTipoPontuacaoFidelidade = 1,
                colspanTipo = 1;

            var logoCell = CreateImageCell(logo);
            logoCell.PaddingBottom = headerPaddingBottom;
            logoCell.PaddingLeft = paddingLeft;

            switch (modalidade)
            {
                case TipoModalidadeProgramaFidelidade.Fixa:
                    switch (pontuacaoFidelidade.TipoPontuacaoFidelidade)
                    {
                        case TipoPontuacaoFidelidade.Normal:
                            numColunas = 5;
                            logoCell.Colspan = 1;
                            colspanHeaderOffset = 1;
                            colspanTitle = 3;

                            colspanRegional = 1;
                            colspanDescricao = 3;

                            colspanEmpresavenda = 1;
                            colspanNomeEmpresavenda = 4;

                            colspanTipoPontuacaoFidelidade = 1;
                            colspanTipo = 4;
                            break;
                        case TipoPontuacaoFidelidade.Campanha:
                            switch (pontuacaoFidelidade.TipoCampanhaFidelidade)
                            {
                                case TipoCampanhaFidelidade.PorVenda:
                                    numColunas = 7;
                                    logoCell.Colspan = 2;
                                    colspanHeaderOffset = 1;
                                    colspanTitle = 4;

                                    colspanDescricao = 5;

                                    colspanNomeEmpresavenda = 6;

                                    colspanTipoPontuacaoFidelidade = 2;
                                    colspanTipo = 5;
                                    break;
                                case TipoCampanhaFidelidade.PorVendaMinima:
                                    numColunas = 8;
                                    logoCell.Colspan = 2;
                                    colspanHeaderOffset = 2;
                                    colspanTitle = 4;

                                    colspanDescricao = 6;

                                    colspanEmpresavenda = 2;
                                    colspanNomeEmpresavenda = 6;

                                    colspanTipoPontuacaoFidelidade = 2;
                                    colspanTipo = 6;
                                    break;
                                case TipoCampanhaFidelidade.PorVendaMinimaEmpreendimento:
                                    numColunas = 8;
                                    logoCell.Colspan = 2;
                                    colspanHeaderOffset = 2;
                                    colspanTitle = 4;

                                    colspanDescricao = 6;

                                    colspanEmpresavenda = 2;
                                    colspanNomeEmpresavenda = 6;

                                    colspanTipoPontuacaoFidelidade = 2;
                                    colspanTipo = 6;
                                    break;
                            }
                            break;
                    }

                    break;
                case TipoModalidadeProgramaFidelidade.Nominal:
                    switch (pontuacaoFidelidade.TipoPontuacaoFidelidade)
                    {
                        case TipoPontuacaoFidelidade.Normal:
                            numColunas = 12;
                            logoCell.Colspan = 3;
                            colspanHeaderOffset = 3;
                            colspanTitle = 6;

                            colspanDescricao = 10;

                            colspanEmpresavenda = 2;
                            colspanNomeEmpresavenda = 10;

                            colspanTipoPontuacaoFidelidade = 3;
                            colspanTipo = 9;
                            break;
                        case TipoPontuacaoFidelidade.Campanha:
                            switch (pontuacaoFidelidade.TipoCampanhaFidelidade)
                            {
                                case TipoCampanhaFidelidade.PorVenda:
                                    numColunas = 21;
                                    logoCell.Colspan = 4;
                                    colspanHeaderOffset = 5;
                                    colspanTitle = 12;

                                    colspanRegional = 2;
                                    colspanDescricao = 20;

                                    colspanEmpresavenda = 4;
                                    colspanNomeEmpresavenda = 17;

                                    colspanTipoPontuacaoFidelidade = 5;
                                    colspanTipo = 16;
                                    break;
                                case TipoCampanhaFidelidade.PorVendaMinima:
                                    numColunas = 22;
                                    logoCell.Colspan = 5;
                                    colspanHeaderOffset = 5;
                                    colspanTitle = 12;

                                    colspanRegional = 2;
                                    colspanDescricao = 22;

                                    colspanEmpresavenda = 4;
                                    colspanNomeEmpresavenda = 18;

                                    colspanTipoPontuacaoFidelidade = 5;
                                    colspanTipo = 17;
                                    break;
                                case TipoCampanhaFidelidade.PorVendaMinimaEmpreendimento:
                                    numColunas = 22;
                                    logoCell.Colspan = 5;
                                    colspanHeaderOffset = 4;
                                    colspanTitle = 13;

                                    colspanRegional = 2;
                                    colspanDescricao = 22;

                                    colspanEmpresavenda = 4;
                                    colspanNomeEmpresavenda = 18;

                                    colspanTipoPontuacaoFidelidade = 5;
                                    colspanTipo = 17;
                                    break;
                            }
                            break;
                    }
                    break;
            }

            var pagina = new PdfPTable(numColunas);
            pagina.WidthPercentage = 100;

            pagina.AddCell(logoCell);

            var headerOffsetCell = CreateCell("", TitleFont, colspanHeaderOffset);
            headerOffsetCell.PaddingBottom = headerPaddingBottom;
            pagina.AddCell(headerOffsetCell);

            var tituloCell = CreateCell(GlobalMessages.ProgramaFidelidade.ToUpper(), TitleFont,
                colspanTitle);
            tituloCell.HorizontalAlignment = Element.ALIGN_CENTER;
            tituloCell.PaddingTop = 15;
            pagina.AddCell(tituloCell);

            var regionalTituloCell = CreateCell(GlobalMessages.Regional.ToUpper(), RegionalFont, colspanRegional);
            regionalTituloCell.PaddingLeft = paddingLeft;
            pagina.AddCell(regionalTituloCell);

            var regionalCell = CreateCell(empresaVenda.Estado.ToUpper(), TextFont, 1);
            pagina.AddCell(regionalCell);

            var description =
               CreateCell("* Pontuação vigente em " + Data.ToDate() + ".",
                   EmpresaVendaFont, colspanDescricao);
            description.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            pagina.AddCell(description);

            var empresaVendaTituloCell = CreateCell(GlobalMessages.EmpresaVenda.ToUpper(),
                RegionalFont, colspanEmpresavenda);
            empresaVendaTituloCell.PaddingLeft = paddingLeft;
            pagina.AddCell(empresaVendaTituloCell);

            var empresaVendaCell = CreateCell(empresaVenda.NomeFantasia.ToUpper(), TextFont, colspanNomeEmpresavenda);
            empresaVendaCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            pagina.AddCell(empresaVendaCell);

            if (pontuacaoFidelidade.HasValue())
            {
                var tipoPontuacaoTituloCell = CreateCell(GlobalMessages.TipoPontuacaoFidelidade.ToUpper()
                    , RegionalFont, colspanTipoPontuacaoFidelidade);
                tipoPontuacaoTituloCell.PaddingLeft = paddingLeft;
                pagina.AddCell(tipoPontuacaoTituloCell);

                var tipoPontuacaoCell = CreateCell(pontuacaoFidelidade.TipoPontuacaoFidelidade.AsString()
                    , TextFont, colspanTipo);
                tipoPontuacaoCell.HorizontalAlignment = PdfCell.ALIGN_LEFT;
                pagina.AddCell(tipoPontuacaoCell);

                if (pontuacaoFidelidade.TipoCampanhaFidelidade.HasValue())
                {
                    var tipoCampanhaTituloCell = CreateCell(GlobalMessages.TipoCampanhaFidelidade.ToUpper(),
                            RegionalFont, colspanTipoPontuacaoFidelidade);
                    tipoCampanhaTituloCell.PaddingLeft = paddingLeft;
                    pagina.AddCell(tipoCampanhaTituloCell);

                    var tipoCampanhaCell = CreateCell(pontuacaoFidelidade.TipoCampanhaFidelidade.AsString(),
                            TextFont, 22);
                    tipoCampanhaCell.HorizontalAlignment = PdfCell.ALIGN_LEFT;
                    pagina.AddCell(tipoCampanhaCell);
                }
            }

            int colspanEmpreendimento = 1,
                colspanFaixaUmMeio = 1,
                colspanFaixaDois = 1,
                colspanSituacao = 1,
                colspanCodigo = 1,
                colspanSubHeader = 1,
                colspanFatorFaixa = 1,
                colspanPontPadrao = 1,
                colspanQuantMinima = 1,
                colspanFatorNominal = 1,
                colspanPadraoNominal = 1;


            int colspanSubHeaders = 0;
            int colspanAtual = 0;

            if (empreendimentos.Any())
            {
                var headerBackgroundColor = new Color(90, 90, 90);
                var borderColor = new Color(240, 240, 240);
                var borderWidth = 1;

                //Criação do Subheards
                #region subheaders
                if (modalidade == TipoModalidadeProgramaFidelidade.Nominal)
                {
                    var subHeaderOffSet = CreateCellHeader("", ColumnFont, 1);

                    var subHeaderUmMeio = CreateCellHeader(GlobalMessages.ColunaPontuacaoFaixaoUmMeio.ToUpper(), ColumnFont,
                        colspanSubHeader);
                    subHeaderUmMeio.Border = Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER;

                    var subHeaderDois = CreateCellHeader(GlobalMessages.ColunaPontuacaoFaixaDois.ToUpper(), ColumnFont,
                        colspanSubHeader);
                    subHeaderDois.Border = Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER;



                    var subHeaderPNE = CreateCellHeader(GlobalMessages.ColunaPontuacaoPNE.ToUpper(), ColumnFont,
                        colspanSubHeader);
                    subHeaderPNE.Border = Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER;

                    var emptyCell = CreateCellHeader("", ColumnFont, colspanEmpreendimento);
                    emptyCell.Border = Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER;

                    switch (pontuacaoFidelidade.TipoPontuacaoFidelidade)
                    {
                        case TipoPontuacaoFidelidade.Normal:
                            subHeaderUmMeio.Colspan = 3;
                            subHeaderDois.Colspan = 3;
                            subHeaderPNE.Colspan = 3;
                            emptyCell.Colspan = colspanCodigo + colspanSituacao;
                            colspanSubHeaders = 12;

                            break;
                        case TipoPontuacaoFidelidade.Campanha:
                            switch (pontuacaoFidelidade.TipoCampanhaFidelidade)
                            {
                                case TipoCampanhaFidelidade.PorVenda:
                                    subHeaderUmMeio.Colspan = 6;
                                    subHeaderDois.Colspan = 6;
                                    subHeaderPNE.Colspan = 6;
                                    emptyCell.Colspan = colspanCodigo + colspanSituacao;
                                    colspanSubHeaders = 21;
                                    break;
                                case TipoCampanhaFidelidade.PorVendaMinima:
                                    pagina.AddCell(subHeaderOffSet);
                                    subHeaderUmMeio.Colspan = 6;
                                    subHeaderDois.Colspan = 6;
                                    subHeaderPNE.Colspan = 6;
                                    emptyCell.Colspan = colspanCodigo + colspanSituacao;
                                    colspanSubHeaders = 22;
                                    break;
                                case TipoCampanhaFidelidade.PorVendaMinimaEmpreendimento:
                                    pagina.AddCell(subHeaderOffSet);
                                    subHeaderUmMeio.Colspan = 6;
                                    subHeaderDois.Colspan = 6;
                                    subHeaderPNE.Colspan = 6;
                                    emptyCell.Colspan = colspanCodigo + colspanSituacao;
                                    colspanSubHeaders = 22;
                                    break;
                            }
                            break;
                    }
                    pagina.AddCell(subHeaderOffSet);
                    pagina.AddCell(subHeaderUmMeio);
                    pagina.AddCell(subHeaderDois);
                    pagina.AddCell(subHeaderPNE);
                    pagina.AddCell(emptyCell);
                    if (numColunas - colspanSubHeaders != 0)
                    {
                        var offsetCell2 = CreateCell("", TitleFont, numColunas - colspanSubHeaders);
                        offsetCell2.BorderColor = borderColor;
                        offsetCell2.Border = Rectangle.LEFT_BORDER;
                        offsetCell2.BorderWidth = borderWidth;
                        offsetCell2.PaddingLeft = paddingLeft;
                        pagina.AddCell(offsetCell2);
                    }
                }

                #endregion

                #region column
                var empreendimentoCellHeader = CreateCellHeader(GlobalMessages.Empreendimento.ToUpper(), ColumnFont,
                   colspanEmpreendimento);
                pagina.AddCell(empreendimentoCellHeader);

                switch (modalidade)
                {
                    case TipoModalidadeProgramaFidelidade.Fixa:
                        switch (pontuacaoFidelidade.TipoPontuacaoFidelidade)
                        {
                            case TipoPontuacaoFidelidade.Normal:
                                var faixaUmMeioCell = CreateCellHeader(GlobalMessages.ColunaPontuacaoFaixaoUmMeio.ToUpper(), ColumnFont,
                                    colspanFaixaUmMeio);
                                pagina.AddCell(faixaUmMeioCell);

                                var faixaDoisCell = CreateCellHeader(GlobalMessages.ColunaPontuacaoFaixaDois.ToUpper(), ColumnFont,
                                    colspanFaixaDois);
                                pagina.AddCell(faixaDoisCell);
                                colspanAtual = 5;
                                break;
                            case TipoPontuacaoFidelidade.Campanha:
                                var fatorFaixaUmMeio = CreateCellHeader("Fator F 1.5", ColumnFont, colspanFatorFaixa);
                                pagina.AddCell(fatorFaixaUmMeio);

                                var fatorPontuacaoPadraoFaixaUmMeio = CreateCellHeader("Pontuação Padrão F 1.5", ColumnFont, colspanPontPadrao);
                                pagina.AddCell(fatorPontuacaoPadraoFaixaUmMeio);

                                var fatorFaixaDois = CreateCellHeader("Fator F 2.0", ColumnFont, colspanFatorFaixa);
                                pagina.AddCell(fatorFaixaDois);

                                var fatorPontuacaoPadraoFaixaDois = CreateCellHeader("Pontuação Padrão F 2.0", ColumnFont, colspanPontPadrao);
                                pagina.AddCell(fatorPontuacaoPadraoFaixaDois);
                                colspanAtual = 7;
                                if (pontuacaoFidelidade.TipoCampanhaFidelidade == TipoCampanhaFidelidade.PorVendaMinima)
                                {
                                    var quantidadeMinima = CreateCellHeader("Qtd. Mínima", ColumnFont, colspanQuantMinima);
                                    pagina.AddCell(quantidadeMinima);
                                    colspanAtual = 8;
                                }

                                if (pontuacaoFidelidade.TipoCampanhaFidelidade == TipoCampanhaFidelidade.PorVendaMinimaEmpreendimento)
                                {
                                    var quantidadeMinimaEmpreendimento = CreateCellHeader("Qtd. Mínima Empreendimento", ColumnFont, colspanQuantMinima);
                                    pagina.AddCell(quantidadeMinimaEmpreendimento);
                                    colspanAtual = 8;
                                }

                                break;
                        }
                        break;
                    case TipoModalidadeProgramaFidelidade.Nominal:

                        if (pontuacaoFidelidade.TipoPontuacaoFidelidade == TipoPontuacaoFidelidade.Campanha &&
                            pontuacaoFidelidade.TipoCampanhaFidelidade != TipoCampanhaFidelidade.PorVenda)
                        {
                            var quantidadeMinima = CreateCellHeader("Qtd. Mínima", ColumnFont, colspanQuantMinima);
                            pagina.AddCell(quantidadeMinima);
                            colspanAtual = 22;
                        }

                        for (var i = 0; i < 3; i++)
                        {
                            switch (pontuacaoFidelidade.TipoPontuacaoFidelidade)
                            {
                                case TipoPontuacaoFidelidade.Normal:
                                    var pontuacaoSeca = CreateCellHeader("Pontuação Seca", ColumnFont, colspanFatorNominal);
                                    pagina.AddCell(pontuacaoSeca);

                                    var pontuacaoNormal = CreateCellHeader("Pontuação Normal", ColumnFont, colspanFatorNominal);
                                    pagina.AddCell(pontuacaoNormal);

                                    var pontuacaoTurbinada = CreateCellHeader("Pontuação Turbinada", ColumnFont, colspanFatorNominal);
                                    pagina.AddCell(pontuacaoTurbinada);
                                    colspanAtual = 12;

                                    break;
                                case TipoPontuacaoFidelidade.Campanha:
                                    var fatorSeca = CreateCellHeader("Fator Seca", ColumnFont, colspanFatorNominal);
                                    pagina.AddCell(fatorSeca);

                                    var pontuacaoPadraoSeca = CreateCellHeader("Pontuação Padrão Seca", ColumnFont, colspanPadraoNominal);
                                    pagina.AddCell(pontuacaoPadraoSeca);

                                    var fatorNormal = CreateCellHeader("Fator Normal", ColumnFont, colspanFatorNominal);
                                    pagina.AddCell(fatorNormal);

                                    var pontuacaoPadraoNormal = CreateCellHeader("Pontuação Padrão Normal", ColumnFont, colspanPadraoNominal);
                                    pagina.AddCell(pontuacaoPadraoNormal);

                                    var fatorTurbinada = CreateCellHeader("Fator Turbinada", ColumnFont, colspanFatorNominal);
                                    pagina.AddCell(fatorTurbinada);

                                    var pontuacaoPadraoTurbinada = CreateCellHeader("Pontuação Padrão Turbinada", ColumnFont, colspanPadraoNominal);
                                    pagina.AddCell(pontuacaoPadraoTurbinada);
                                    colspanAtual = colspanAtual == 0 ? 21 : colspanAtual;

                                    break;
                            }
                        }
                        break;
                }
                var situacaoCell = CreateCellHeader(GlobalMessages.Situacao.ToUpper(), ColumnFont, colspanSituacao);
                pagina.AddCell(situacaoCell);

                var codigoCell = CreateCellHeader(GlobalMessages.Codigo.ToUpper(), ColumnFont, colspanCodigo);
                pagina.AddCell(codigoCell);

                if (numColunas - colspanAtual != 0)
                {
                    var offsetCell2 = CreateCell("", TitleFont, numColunas - colspanAtual);
                    offsetCell2.BorderColor = borderColor;
                    offsetCell2.Border = Rectangle.LEFT_BORDER;
                    offsetCell2.BorderWidth = borderWidth;
                    offsetCell2.PaddingLeft = paddingLeft;
                    pagina.AddCell(offsetCell2);
                }

                #endregion

                #region Data
                switch (pontuacaoFidelidade.TipoPontuacaoFidelidade)
                {
                    case TipoPontuacaoFidelidade.Normal:
                        foreach (var empreendimento in empreendimentos)
                        {
                            var itensPontuacaoFidelidade = _itemPontuacaoFidelidadeRepository
                                                            .BuscarItemAtivoPorEmpreendimentoEmpresaVenda(empreendimento.Id, empresaVenda.Id, modalidade);
                            var empreendimentoCell = CreateCell(empreendimento.Nome, TextFont,
                            colspanEmpreendimento);
                            empreendimentoCell.PaddingLeft = paddingLeft;
                            pagina.AddCell(empreendimentoCell);
                            switch (modalidade)
                            {
                                case TipoModalidadeProgramaFidelidade.Fixa:
                                    var faixaUmMeio = CreateCell(itensPontuacaoFidelidade.PontuacaoFaixaUmMeio.ToString("F"),
                                        TextFont,
                                        colspanFaixaUmMeio);
                                    faixaUmMeio.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                    pagina.AddCell(faixaUmMeio);

                                    var faixaDois = CreateCell(itensPontuacaoFidelidade.PontuacaoFaixaDois.ToString("F"),
                                        TextFont,
                                        colspanFaixaDois);
                                    faixaDois.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                    pagina.AddCell(faixaDois);

                                    break;
                                case TipoModalidadeProgramaFidelidade.Nominal:
                                    var pontuacaofaixaUmMeioSeca = CreateCell(itensPontuacaoFidelidade.PontuacaoFaixaUmMeioSeca.ToString("F"),
                                       TextFont,
                                       colspanFaixaDois);
                                    pontuacaofaixaUmMeioSeca.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                    pontuacaofaixaUmMeioSeca.BackgroundColor = new Color(200, 200, 200);
                                    pagina.AddCell(pontuacaofaixaUmMeioSeca);
                                    var pontuacaoFaixaUmMeioNormal = CreateCell(itensPontuacaoFidelidade.PontuacaoFaixaUmMeioNormal.ToString("F"),
                                       TextFont,
                                       colspanFaixaDois);
                                    pontuacaoFaixaUmMeioNormal.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                    pontuacaoFaixaUmMeioNormal.BackgroundColor = new Color(200, 200, 200);
                                    pagina.AddCell(pontuacaoFaixaUmMeioNormal);
                                    var pontuacaoFaixaUmMeioTurbinada = CreateCell(itensPontuacaoFidelidade.PontuacaoFaixaUmMeioTurbinada.ToString("F"),
                                       TextFont,
                                       colspanFaixaDois);
                                    pontuacaoFaixaUmMeioTurbinada.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                    pontuacaoFaixaUmMeioTurbinada.BackgroundColor = new Color(200, 200, 200);
                                    pagina.AddCell(pontuacaoFaixaUmMeioTurbinada);

                                    var pontuacaoFaixaDoisSeca = CreateCell(itensPontuacaoFidelidade.PontuacaoFaixaDoisSeca.ToString("F"),
                                       TextFont,
                                       colspanFaixaDois);
                                    pontuacaoFaixaDoisSeca.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                    pagina.AddCell(pontuacaoFaixaDoisSeca);
                                    var pontuacaoFaixaDoisNormal = CreateCell(itensPontuacaoFidelidade.PontuacaoFaixaDoisNormal.ToString("F"),
                                       TextFont,
                                       colspanFaixaDois);
                                    pontuacaoFaixaDoisNormal.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                    pagina.AddCell(pontuacaoFaixaDoisNormal);
                                    var pontuacaoFaixaDoisTurbinada = CreateCell(itensPontuacaoFidelidade.PontuacaoFaixaDoisTurbinada.ToString("F"),
                                       TextFont,
                                       colspanFaixaDois);
                                    pontuacaoFaixaDoisTurbinada.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                    pagina.AddCell(pontuacaoFaixaDoisTurbinada);

                                    var pontuacaoPNESeca = CreateCell(itensPontuacaoFidelidade.PontuacaoPNESeca.ToString("F"),
                                       TextFont,
                                       colspanFaixaDois);
                                    pontuacaoPNESeca.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                    pontuacaoPNESeca.BackgroundColor = new Color(200, 200, 200);
                                    pagina.AddCell(pontuacaoPNESeca);
                                    var pontuacaoPNENormal = CreateCell(itensPontuacaoFidelidade.PontuacaoPNENormal.ToString("F"),
                                       TextFont,
                                       colspanFaixaDois);
                                    pontuacaoPNENormal.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                    pontuacaoPNENormal.BackgroundColor = new Color(200, 200, 200);
                                    pagina.AddCell(pontuacaoPNENormal);
                                    var pontuacaoPNETurbinada = CreateCell(itensPontuacaoFidelidade.PontuacaoPNETurbinada.ToString("F"),
                                       TextFont,
                                       colspanFaixaDois);
                                    pontuacaoPNETurbinada.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                    pontuacaoPNETurbinada.BackgroundColor = new Color(200, 200, 200);
                                    pagina.AddCell(pontuacaoPNETurbinada);

                                    break;
                            }
                            var situacao = CreateCell(itensPontuacaoFidelidade.Situacao.ToString(),
                                       TextFont,
                                       colspanFaixaDois);
                            situacao.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            pagina.AddCell(situacao);

                            var codigo = CreateCell(itensPontuacaoFidelidade.Codigo,
                                       TextFont,
                                       colspanFaixaDois);
                            codigo.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            pagina.AddCell(codigo);
                            if (numColunas - colspanAtual != 0)
                            {
                                var offsetCell2 = CreateCell("", TextFont, numColunas - colspanAtual);
                                offsetCell2.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                pagina.AddCell(offsetCell2);
                            }

                        }
                        break;
                    case TipoPontuacaoFidelidade.Campanha:
                        List<long> lista = new List<long>();
                        if (TipoCampanhaFidelidade.PorVenda != pontuacaoFidelidade.TipoCampanhaFidelidade)
                        {
                            lista = new JavaScriptSerializer().Deserialize<List<long>>(pontuacaoFidelidade.QuantidadesMinimas);
                        }

                        var progressao = pontuacaoFidelidade.Progressao == 0 ? 1 : pontuacaoFidelidade.Progressao;
                        foreach (var empreendimento in empreendimentos)
                        {
                            var pontos = itens.Where(x => x.Empreendimento.Id == empreendimento.Id)
                                .OrderBy(x => x.QuantidadeMinima).ToList();
                            for (var i = 0; i < progressao; i++)
                            {

                                var itensPontuacaoFidelidade = new ItemPontuacaoFidelidade();
                                if (TipoCampanhaFidelidade.PorVenda == pontuacaoFidelidade.TipoCampanhaFidelidade)
                                {
                                    itensPontuacaoFidelidade = pontos[i];
                                    //itensPontuacaoFidelidade = _itemPontuacaoFidelidadeRepository
                                    //                       .BuscarItemAtivoPorEmpreendimentoEmpresaVenda(empreendimento.Id, empresaVenda.Id, modalidade);
                                }
                                else
                                {
                                    itensPontuacaoFidelidade = pontos[i];

                                    //itensPontuacaoFidelidade = _itemPontuacaoFidelidadeRepository
                                    //                       .BuscarItemAtivoPorEmpreendimentoEmpresaVendaQuant(empreendimento.Id, empresaVenda.Id, modalidade, lista[i]);
                                }

                                if (i == 0)
                                {
                                    var empreendimentoCell = CreateCell(empreendimento.Nome, TextFont,
                                   colspanEmpreendimento);
                                    empreendimentoCell.PaddingLeft = paddingLeft;
                                    pagina.AddCell(empreendimentoCell);
                                }
                                else
                                {
                                    var empreendimentoCell = CreateCell(" ", TextFont,
                                  colspanEmpreendimento);
                                    empreendimentoCell.PaddingLeft = paddingLeft;
                                    pagina.AddCell(empreendimentoCell);
                                }
                                switch (modalidade)
                                {
                                    case TipoModalidadeProgramaFidelidade.Fixa:
                                        //F1.5
                                        var fatorUmMeio = CreateCell(itensPontuacaoFidelidade.FatorUmMeio.ToString("F"),
                                        TextFont,
                                        colspanFatorFaixa);
                                        fatorUmMeio.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        pagina.AddCell(fatorUmMeio);

                                        var pontuacaoPadraoUmMeio = CreateCell(itensPontuacaoFidelidade.PontuacaoPadraoUmMeio.ToString("F"),
                                        TextFont,
                                        colspanPontPadrao);
                                        pontuacaoPadraoUmMeio.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        pagina.AddCell(pontuacaoPadraoUmMeio);

                                        //F2.0
                                        var fatorDois = CreateCell(itensPontuacaoFidelidade.FatorDois.ToString("F"),
                                        TextFont,
                                        colspanFatorFaixa);
                                        fatorDois.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        pagina.AddCell(fatorDois);

                                        var pontuacaoPadraoDois = CreateCell(itensPontuacaoFidelidade.PontuacaoPadraoDois.ToString("F"),
                                        TextFont,
                                        colspanPontPadrao);
                                        pontuacaoPadraoDois.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        pagina.AddCell(pontuacaoPadraoDois);

                                        if (pontuacaoFidelidade.TipoCampanhaFidelidade != TipoCampanhaFidelidade.PorVenda)
                                        {
                                            var quantidadeMinima = CreateCell(itensPontuacaoFidelidade.QuantidadeMinima.ToString("F"),
                                            TextFont,
                                            colspanQuantMinima);
                                            quantidadeMinima.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                            pagina.AddCell(quantidadeMinima);
                                        }

                                        break;
                                    case TipoModalidadeProgramaFidelidade.Nominal:
                                        if (pontuacaoFidelidade.TipoCampanhaFidelidade != TipoCampanhaFidelidade.PorVenda)
                                        {
                                            var quantidadeMinima = CreateCell(itensPontuacaoFidelidade.QuantidadeMinima.ToString("F"),
                                            TextFont,
                                            colspanQuantMinima);
                                            quantidadeMinima.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                            pagina.AddCell(quantidadeMinima);
                                        }
                                        //F1.5

                                        var fatorUmMeioSeca = CreateCell(itensPontuacaoFidelidade.FatorUmMeioSeca.ToString("F"),
                                       TextFont,
                                       colspanFatorNominal);
                                        fatorUmMeioSeca.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        fatorUmMeioSeca.BackgroundColor = new Color(200, 200, 200);
                                        pagina.AddCell(fatorUmMeioSeca);
                                        var pontuacaoFaixaUmMeioSeca = CreateCell(itensPontuacaoFidelidade.PontuacaoFaixaUmMeioSeca.ToString("F"),
                                       TextFont,
                                       colspanFatorNominal);
                                        pontuacaoFaixaUmMeioSeca.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        pontuacaoFaixaUmMeioSeca.BackgroundColor = new Color(200, 200, 200);
                                        pagina.AddCell(pontuacaoFaixaUmMeioSeca);

                                        var fatorUmMeioNormal = CreateCell(itensPontuacaoFidelidade.FatorUmMeioNormal.ToString("F"),
                                       TextFont,
                                       colspanFatorNominal);
                                        fatorUmMeioNormal.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        fatorUmMeioNormal.BackgroundColor = new Color(200, 200, 200);
                                        pagina.AddCell(fatorUmMeioNormal);
                                        var pontuacaoFaixaUmMeioNormal = CreateCell(itensPontuacaoFidelidade.PontuacaoFaixaUmMeioNormal.ToString("F"),
                                       TextFont,
                                       colspanFatorNominal);
                                        pontuacaoFaixaUmMeioNormal.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        pontuacaoFaixaUmMeioNormal.BackgroundColor = new Color(200, 200, 200);
                                        pagina.AddCell(pontuacaoFaixaUmMeioNormal);

                                        var fatorUmMeioTurbinada = CreateCell(itensPontuacaoFidelidade.FatorUmMeioTurbinada.ToString("F"),
                                      TextFont,
                                      colspanFatorNominal);
                                        fatorUmMeioTurbinada.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        fatorUmMeioTurbinada.BackgroundColor = new Color(200, 200, 200);
                                        pagina.AddCell(fatorUmMeioTurbinada);
                                        var pontuacaoFaixaUmMeioTurbinada = CreateCell(itensPontuacaoFidelidade.PontuacaoFaixaUmMeioTurbinada.ToString("F"),
                                       TextFont,
                                       colspanFatorNominal);
                                        pontuacaoFaixaUmMeioTurbinada.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        pontuacaoFaixaUmMeioTurbinada.BackgroundColor = new Color(200, 200, 200);
                                        pagina.AddCell(pontuacaoFaixaUmMeioTurbinada);


                                        //F2.0
                                        var fatorDoisSeca = CreateCell(itensPontuacaoFidelidade.FatorDoisSeca.ToString("F"),
                                       TextFont,
                                       colspanFatorNominal);
                                        fatorDoisSeca.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        pagina.AddCell(fatorDoisSeca);
                                        var pontuacaoFaixaDoisSeca = CreateCell(itensPontuacaoFidelidade.PontuacaoFaixaDoisSeca.ToString("F"),
                                       TextFont,
                                       colspanFatorNominal);
                                        pontuacaoFaixaDoisSeca.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        pagina.AddCell(pontuacaoFaixaDoisSeca);

                                        var fatorDoisNormal = CreateCell(itensPontuacaoFidelidade.FatorDoisNormal.ToString("F"),
                                       TextFont,
                                       colspanFatorNominal);
                                        fatorDoisNormal.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        pagina.AddCell(fatorDoisNormal);
                                        var pontuacaoFaixaDoisNormal = CreateCell(itensPontuacaoFidelidade.PontuacaoFaixaDoisNormal.ToString("F"),
                                       TextFont,
                                       colspanFatorNominal);
                                        pontuacaoFaixaDoisNormal.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        pagina.AddCell(pontuacaoFaixaDoisNormal);

                                        var fatorDoisTurbinada = CreateCell(itensPontuacaoFidelidade.FatorDoisTurbinada.ToString("F"),
                                      TextFont,
                                      colspanFatorNominal);
                                        fatorDoisTurbinada.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        pagina.AddCell(fatorDoisTurbinada);
                                        var pontuacaoFaixaDoisTurbinada = CreateCell(itensPontuacaoFidelidade.PontuacaoFaixaDoisTurbinada.ToString("F"),
                                       TextFont,
                                       colspanFatorNominal);
                                        pontuacaoFaixaDoisTurbinada.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        pagina.AddCell(pontuacaoFaixaDoisTurbinada);


                                        //PNE
                                        var fatorPNESeca = CreateCell(itensPontuacaoFidelidade.FatorPNESeca.ToString("F"),
                                       TextFont,
                                       colspanFatorNominal);
                                        fatorPNESeca.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        fatorPNESeca.BackgroundColor = new Color(200, 200, 200);
                                        pagina.AddCell(fatorPNESeca);
                                        var pontuacaoPNESeca = CreateCell(itensPontuacaoFidelidade.PontuacaoPNESeca.ToString("F"),
                                       TextFont,
                                       colspanFatorNominal);
                                        pontuacaoPNESeca.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        pontuacaoPNESeca.BackgroundColor = new Color(200, 200, 200);
                                        pagina.AddCell(pontuacaoPNESeca);

                                        var fatorPNENormal = CreateCell(itensPontuacaoFidelidade.FatorPNENormal.ToString("F"),
                                       TextFont,
                                       colspanFatorNominal);
                                        fatorPNENormal.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        fatorPNENormal.BackgroundColor = new Color(200, 200, 200);
                                        pagina.AddCell(fatorPNENormal);
                                        var pontuacaoFaixaPNENormal = CreateCell(itensPontuacaoFidelidade.PontuacaoPNENormal.ToString("F"),
                                       TextFont,
                                       colspanFatorNominal);
                                        pontuacaoFaixaPNENormal.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        pontuacaoFaixaPNENormal.BackgroundColor = new Color(200, 200, 200);
                                        pagina.AddCell(pontuacaoFaixaPNENormal);

                                        var fatorPNETurbinada = CreateCell(itensPontuacaoFidelidade.FatorPNETurbinada.ToString("F"),
                                      TextFont,
                                      colspanFatorNominal);
                                        fatorPNETurbinada.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        fatorPNETurbinada.BackgroundColor = new Color(200, 200, 200);
                                        pagina.AddCell(fatorPNETurbinada);
                                        var pontuacaoPNETurbinada = CreateCell(itensPontuacaoFidelidade.PontuacaoPNETurbinada.ToString("F"),
                                       TextFont,
                                       colspanFatorNominal);
                                        pontuacaoPNETurbinada.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        pontuacaoPNETurbinada.BackgroundColor = new Color(200, 200, 200);
                                        pagina.AddCell(pontuacaoPNETurbinada);

                                        break;
                                }

                                var situacao = CreateCell(itensPontuacaoFidelidade.Situacao.ToString(),
                                       TextFont,
                                       colspanFaixaDois);
                                situacao.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                pagina.AddCell(situacao);

                                var codigo = CreateCell(itensPontuacaoFidelidade.Codigo,
                                           TextFont,
                                           colspanFaixaDois);
                                codigo.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                pagina.AddCell(codigo);

                                if (numColunas - colspanAtual != 0)
                                {
                                    var offsetCell2 = CreateCell("", TextFont, numColunas - colspanAtual);
                                    offsetCell2.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                    pagina.AddCell(offsetCell2);
                                }

                            }
                        }
                        break;
                }
                #endregion
            }
            return pagina;
        }

        private PdfPCell CreateImageCell(byte[] logo)
        {
            Image logoImg = Image.GetInstance(logo);
            PdfPCell cell = new PdfPCell(logoImg, true);
            //cell.Colspan = 4;
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
        private PdfPCell CreateCellHeader(string content, Font font, int colspan)
        {
            var chunk = new Chunk(content, font);
            var paragraph = new Paragraph(chunk);
            var cell = new PdfPCell(paragraph);
            cell.PaddingTop = 7;
            cell.PaddingBottom = 7;
            cell.Colspan = colspan;
            cell.Border = Rectangle.RIGHT_BORDER;
            cell.VerticalAlignment = PdfPCell.ALIGN_CENTER;
            cell.BackgroundColor = new Color(90, 90, 90);
            cell.BorderColor = new Color(240, 240, 240);
            cell.BorderWidth = 1;
            cell.PaddingLeft = 5;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            return cell;
        }
        private Font GetItemFont(double value, double activeValue)
        {
            return Math.Abs(value - activeValue) > double.Epsilon ? RedTextFont : TextFont;
        }

        #region PDF Admin
        public Dictionary<long, byte[]> CriarPdfAdmin(long idPontuacaoFidelidade, byte[] logo)
        {
            EnsureFontInitialized();

            var documentos = new Dictionary<long, byte[]>();

            var stream = new MemoryStream();
            var documentoGeral = CreateDocument();

            PdfWriter.GetInstance(documentoGeral, stream);

            documentoGeral.Open();
            var i = 1;
            var hasAnyPage = false;

            var itens = _itemPontuacaoFidelidadeRepository.BuscarItens(idPontuacaoFidelidade);

            var evs = itens.Select(x => x.EmpresaVenda)
                .Distinct();

            foreach (var empresaVenda in evs)
            {
                var itensPadrao = itens.Where(x => x.PontuacaoFidelidade.TipoPontuacaoFidelidade == TipoPontuacaoFidelidade.Normal)
                    .Where(x => x.EmpresaVenda.Id == empresaVenda.Id)
                    .ToList();

                if (itensPadrao.HasValue())
                {
                    var bytes = CriarPdfAdmin(empresaVenda, logo, itensPadrao);
                    documentos.Add(i++, bytes);

                    if (hasAnyPage)
                    {
                        documentoGeral.NewPage();
                    }

                    IncluirConteudoAdmin(documentoGeral, empresaVenda, logo, itensPadrao);
                    hasAnyPage = true;
                }

                foreach (TipoCampanhaFidelidade tpCampanha in Enum.GetValues(typeof(TipoCampanhaFidelidade)))
                {
                    var itensCampanha = itens.Where(x => x.PontuacaoFidelidade.TipoCampanhaFidelidade == tpCampanha)
                        .Where(x => x.EmpresaVenda.Id == empresaVenda.Id)
                        .ToList();

                    if (itensCampanha.HasValue())
                    {
                        var bytes = CriarPdfAdmin(empresaVenda, logo, itensCampanha);
                        documentos.Add(i++, bytes);

                        if (hasAnyPage)
                        {
                            documentoGeral.NewPage();
                        }

                        IncluirConteudoAdmin(documentoGeral, empresaVenda, logo, itensCampanha);
                        hasAnyPage = true;
                    }

                }

                if (documentos.Count == 0)
                {
                    var bytes = CriarPdfAdmin(empresaVenda, logo, new List<ItemPontuacaoFidelidade>());
                    documentos.Add(i++, bytes);
                    IncluirConteudoAdmin(documentoGeral, empresaVenda, logo, new List<ItemPontuacaoFidelidade>());
                }
            }

            documentoGeral.Close();

            documentos.Add(0, stream.ToArray());

            return documentos;
        }

        public Dictionary<long, byte[]> CriarPdfAdmin(long idPontuacaoFidelidade,Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda, byte[] logo)
        {
            EnsureFontInitialized();

            var documentos = new Dictionary<long, byte[]>();

            var stream = new MemoryStream();
            var documentoGeral = CreateDocument();

            PdfWriter.GetInstance(documentoGeral, stream);

            documentoGeral.Open();
            var i = 1;
            var hasAnyPage = false;

            var itens = _itemPontuacaoFidelidadeRepository.BuscarItens(idPontuacaoFidelidade,empresaVenda.Id);

            var itensPadrao = itens.Where(x => x.PontuacaoFidelidade.TipoPontuacaoFidelidade == TipoPontuacaoFidelidade.Normal)
                .ToList();

            if (itensPadrao.HasValue())
            {
                var bytes = CriarPdfAdmin(empresaVenda, logo, itensPadrao);
                documentos.Add(i++, bytes);

                if (hasAnyPage)
                {
                    documentoGeral.NewPage();
                }

                IncluirConteudoAdmin(documentoGeral, empresaVenda, logo, itensPadrao);
                hasAnyPage = true;
            }

            foreach (TipoCampanhaFidelidade tpCampanha in Enum.GetValues(typeof(TipoCampanhaFidelidade)))
            {
                var itensCampanha = itens.Where(x => x.PontuacaoFidelidade.TipoCampanhaFidelidade == tpCampanha)
                .ToList();

                if (itensCampanha.HasValue())
                {
                    var bytes = CriarPdfAdmin(empresaVenda, logo, itensCampanha);
                    documentos.Add(i++, bytes);

                    if (hasAnyPage)
                    {
                        documentoGeral.NewPage();
                    }

                    IncluirConteudoAdmin(documentoGeral, empresaVenda, logo, itensCampanha);
                    hasAnyPage = true;
                }

            }

            if (documentos.Count == 0)
            {
                var bytes = CriarPdfAdmin(empresaVenda, logo, new List<ItemPontuacaoFidelidade>());
                documentos.Add(i++, bytes);
                IncluirConteudoAdmin(documentoGeral, empresaVenda, logo, new List<ItemPontuacaoFidelidade>());
            }

            documentoGeral.Close();

            documentos.Add(0, stream.ToArray());

            return documentos;
        }

        private byte[] CriarPdfAdmin(Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda,
            byte[] logo, List<ItemPontuacaoFidelidade> itens)
        {
            var stream = new MemoryStream();
            var documentoGeral = CreateDocument();

            PdfWriter.GetInstance(documentoGeral, stream);

            documentoGeral.Open();

            IncluirConteudoAdmin(documentoGeral, empresaVenda, logo, itens);

            documentoGeral.Close();

            return stream.ToArray();
        }

        private void IncluirConteudoAdmin(Document document, Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda,
            byte[] logo,List<ItemPontuacaoFidelidade> itens)
        {
            var pontuacaoFidelidade = itens.Select(x => x.PontuacaoFidelidade)
                .Distinct().FirstOrDefault();

            var empreendimentosFixa = itens.Where(x => x.Modalidade == TipoModalidadeProgramaFidelidade.Fixa)
                .Select(x => x.Empreendimento)
                .OrderBy(x => x.Nome)
                .ToList();

            var empreendimentosNominal = itens.Where(x => x.Modalidade == TipoModalidadeProgramaFidelidade.Nominal)
                .Select(x => x.Empreendimento)
                .OrderBy(x => x.Nome)
                .ToList();

            var hasAnyPage = false;
            if (empreendimentosFixa.Count() > 0)
            {
                var lista = itens.Where(x => empreendimentosFixa.Contains(x.Empreendimento)).ToList();
                var page = CriarConteudoAdmin(empreendimentosFixa.ToList(), pontuacaoFidelidade,
                logo, empresaVenda, TipoModalidadeProgramaFidelidade.Fixa, lista);

                document.Add(page);
                hasAnyPage = true;
            }
            if (empreendimentosNominal.Count() > 0)
            {
                if (hasAnyPage)
                {
                    document.NewPage();
                }
                var lista = itens.Where(x => empreendimentosNominal.Contains(x.Empreendimento)).ToList();
                var page = CriarConteudoAdmin(empreendimentosNominal.ToList(), pontuacaoFidelidade,
                logo, empresaVenda, TipoModalidadeProgramaFidelidade.Nominal, lista);

                document.Add(page);
            }
        }

        private IElement CriarConteudoAdmin(List<Empreendimento> empreendimentos,
            PontuacaoFidelidade pontuacaoFidelidade, byte[] logo,
            Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda, TipoModalidadeProgramaFidelidade modalidade, List<ItemPontuacaoFidelidade> itens)
        {
            var Data = DateTime.Now;
            var paddingLeft = 5;
            var headerPaddingBottom = 20;
            var numColunas = 0;

            int colspanHeaderOffset = 1,
                colspanTitle = 3,
                colspanRegional = 1,
                colspanDescricao = 1,
                colspanEmpresavenda = 1,
                colspanNomeEmpresavenda = 1,
                colspanTipoPontuacaoFidelidade = 1,
                colspanTipo = 1;

            var logoCell = CreateImageCell(logo);
            logoCell.PaddingBottom = headerPaddingBottom;
            logoCell.PaddingLeft = paddingLeft;

            switch (modalidade)
            {
                case TipoModalidadeProgramaFidelidade.Fixa:
                    switch (pontuacaoFidelidade.TipoPontuacaoFidelidade)
                    {
                        case TipoPontuacaoFidelidade.Normal:
                            numColunas = 5;
                            logoCell.Colspan = 1;
                            colspanHeaderOffset = 1;
                            colspanTitle = 3;

                            colspanRegional = 1;
                            colspanDescricao = 3;

                            colspanEmpresavenda = 1;
                            colspanNomeEmpresavenda = 4;

                            colspanTipoPontuacaoFidelidade = 1;
                            colspanTipo = 4;
                            break;
                        case TipoPontuacaoFidelidade.Campanha:
                            switch (pontuacaoFidelidade.TipoCampanhaFidelidade)
                            {
                                case TipoCampanhaFidelidade.PorVenda:
                                    numColunas = 7;
                                    logoCell.Colspan = 2;
                                    colspanHeaderOffset = 1;
                                    colspanTitle = 4;

                                    colspanDescricao = 5;

                                    colspanNomeEmpresavenda = 6;

                                    colspanTipoPontuacaoFidelidade = 2;
                                    colspanTipo = 5;
                                    break;
                                case TipoCampanhaFidelidade.PorVendaMinima:
                                    numColunas = 8;
                                    logoCell.Colspan = 2;
                                    colspanHeaderOffset = 2;
                                    colspanTitle = 4;

                                    colspanDescricao = 6;

                                    colspanEmpresavenda = 2;
                                    colspanNomeEmpresavenda = 6;

                                    colspanTipoPontuacaoFidelidade = 2;
                                    colspanTipo = 6;
                                    break;
                                case TipoCampanhaFidelidade.PorVendaMinimaEmpreendimento:
                                    numColunas = 8;
                                    logoCell.Colspan = 2;
                                    colspanHeaderOffset = 2;
                                    colspanTitle = 4;

                                    colspanDescricao = 6;

                                    colspanEmpresavenda = 2;
                                    colspanNomeEmpresavenda = 6;

                                    colspanTipoPontuacaoFidelidade = 2;
                                    colspanTipo = 6;
                                    break;
                            }
                            break;
                    }

                    break;
                case TipoModalidadeProgramaFidelidade.Nominal:
                    switch (pontuacaoFidelidade.TipoPontuacaoFidelidade)
                    {
                        case TipoPontuacaoFidelidade.Normal:
                            numColunas = 12;
                            logoCell.Colspan = 3;
                            colspanHeaderOffset = 3;
                            colspanTitle = 6;

                            colspanDescricao = 10;

                            colspanEmpresavenda = 2;
                            colspanNomeEmpresavenda = 10;

                            colspanTipoPontuacaoFidelidade = 3;
                            colspanTipo = 9;
                            break;
                        case TipoPontuacaoFidelidade.Campanha:
                            switch (pontuacaoFidelidade.TipoCampanhaFidelidade)
                            {
                                case TipoCampanhaFidelidade.PorVenda:
                                    numColunas = 21;
                                    logoCell.Colspan = 4;
                                    colspanHeaderOffset = 5;
                                    colspanTitle = 12;

                                    colspanRegional = 2;
                                    colspanDescricao = 20;

                                    colspanEmpresavenda = 4;
                                    colspanNomeEmpresavenda = 17;

                                    colspanTipoPontuacaoFidelidade = 5;
                                    colspanTipo = 16;
                                    break;
                                case TipoCampanhaFidelidade.PorVendaMinima:
                                    numColunas = 22;
                                    logoCell.Colspan = 5;
                                    colspanHeaderOffset = 5;
                                    colspanTitle = 12;

                                    colspanRegional = 2;
                                    colspanDescricao = 22;

                                    colspanEmpresavenda = 4;
                                    colspanNomeEmpresavenda = 18;

                                    colspanTipoPontuacaoFidelidade = 5;
                                    colspanTipo = 17;
                                    break;
                                case TipoCampanhaFidelidade.PorVendaMinimaEmpreendimento:
                                    numColunas = 22;
                                    logoCell.Colspan = 5;
                                    colspanHeaderOffset = 4;
                                    colspanTitle = 13;

                                    colspanRegional = 2;
                                    colspanDescricao = 22;

                                    colspanEmpresavenda = 4;
                                    colspanNomeEmpresavenda = 18;

                                    colspanTipoPontuacaoFidelidade = 5;
                                    colspanTipo = 17;
                                    break;
                            }
                            break;
                    }
                    break;
            }

            var pagina = new PdfPTable(numColunas);
            pagina.WidthPercentage = 100;

            pagina.AddCell(logoCell);

            var headerOffsetCell = CreateCell("", TitleFont, colspanHeaderOffset);
            headerOffsetCell.PaddingBottom = headerPaddingBottom;
            pagina.AddCell(headerOffsetCell);

            var tituloCell = CreateCell(GlobalMessages.ProgramaFidelidade.ToUpper(), TitleFont,
                colspanTitle);
            tituloCell.HorizontalAlignment = Element.ALIGN_CENTER;
            tituloCell.PaddingTop = 15;
            pagina.AddCell(tituloCell);

            var regionalTituloCell = CreateCell(GlobalMessages.Regional.ToUpper(), RegionalFont, colspanRegional);
            regionalTituloCell.PaddingLeft = paddingLeft;
            pagina.AddCell(regionalTituloCell);

            var regionalCell = CreateCell(empresaVenda.Estado.ToUpper(), TextFont, 1);
            pagina.AddCell(regionalCell);

            var description =
               CreateCell("",
                   EmpresaVendaFont, colspanDescricao);

            description.HorizontalAlignment = PdfPCell.ALIGN_RIGHT;
            pagina.AddCell(description);

            var empresaVendaTituloCell = CreateCell(GlobalMessages.EmpresaVenda.ToUpper(),
                RegionalFont, colspanEmpresavenda);
            empresaVendaTituloCell.PaddingLeft = paddingLeft;
            pagina.AddCell(empresaVendaTituloCell);

            var empresaVendaCell = CreateCell(empresaVenda.NomeFantasia.ToUpper(), TextFont, colspanNomeEmpresavenda);
            empresaVendaCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            pagina.AddCell(empresaVendaCell);

            if (pontuacaoFidelidade.HasValue())
            {
                var tipoPontuacaoTituloCell = CreateCell(GlobalMessages.TipoPontuacaoFidelidade.ToUpper()
                    , RegionalFont, colspanTipoPontuacaoFidelidade);
                tipoPontuacaoTituloCell.PaddingLeft = paddingLeft;
                pagina.AddCell(tipoPontuacaoTituloCell);

                var tipoPontuacaoCell = CreateCell(pontuacaoFidelidade.TipoPontuacaoFidelidade.AsString()
                    , TextFont, colspanTipo);
                tipoPontuacaoCell.HorizontalAlignment = PdfCell.ALIGN_LEFT;
                pagina.AddCell(tipoPontuacaoCell);

                if (pontuacaoFidelidade.TipoCampanhaFidelidade.HasValue())
                {
                    var tipoCampanhaTituloCell = CreateCell(GlobalMessages.TipoCampanhaFidelidade.ToUpper(),
                            RegionalFont, colspanTipoPontuacaoFidelidade);
                    tipoCampanhaTituloCell.PaddingLeft = paddingLeft;
                    pagina.AddCell(tipoCampanhaTituloCell);

                    var tipoCampanhaCell = CreateCell(pontuacaoFidelidade.TipoCampanhaFidelidade.AsString(),
                            TextFont, 22);
                    tipoCampanhaCell.HorizontalAlignment = PdfCell.ALIGN_LEFT;
                    pagina.AddCell(tipoCampanhaCell);
                }
            }

            int colspanEmpreendimento = 1,
                colspanFaixaUmMeio = 1,
                colspanFaixaDois = 1,
                colspanSituacao = 1,
                colspanCodigo = 1,
                colspanSubHeader = 1,
                colspanFatorFaixa = 1,
                colspanPontPadrao = 1,
                colspanQuantMinima = 1,
                colspanFatorNominal = 1,
                colspanPadraoNominal = 1;

            int colspanSubHeaders = 0;
            int colspanAtual = 0;
            
            if (empreendimentos.Any())
            {
                var headerBackgroundColor = new Color(90, 90, 90);
                var borderColor = new Color(240, 240, 240);
                var borderWidth = 1;

                //Criação do Subheards
                #region subheaders
                if (modalidade == TipoModalidadeProgramaFidelidade.Nominal)
                {
                    var subHeaderOffSet = CreateCellHeader("", ColumnFont, 1);

                    var subHeaderUmMeio = CreateCellHeader(GlobalMessages.ColunaPontuacaoFaixaoUmMeio.ToUpper(), ColumnFont,
                        colspanSubHeader);
                    subHeaderUmMeio.Border = Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER;

                    var subHeaderDois = CreateCellHeader(GlobalMessages.ColunaPontuacaoFaixaDois.ToUpper(), ColumnFont,
                        colspanSubHeader);
                    subHeaderDois.Border = Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER;

                    var subHeaderPNE = CreateCellHeader(GlobalMessages.ColunaPontuacaoPNE.ToUpper(), ColumnFont,
                        colspanSubHeader);
                    subHeaderPNE.Border = Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER;

                    var emptyCell = CreateCellHeader("", ColumnFont, colspanEmpreendimento);
                    emptyCell.Border = Rectangle.LEFT_BORDER | Rectangle.BOTTOM_BORDER | Rectangle.RIGHT_BORDER;

                    switch (pontuacaoFidelidade.TipoPontuacaoFidelidade)
                    {
                        case TipoPontuacaoFidelidade.Normal:
                            subHeaderUmMeio.Colspan = 3;
                            subHeaderDois.Colspan = 3;
                            subHeaderPNE.Colspan = 3;
                            emptyCell.Colspan = colspanCodigo + colspanSituacao;
                            colspanSubHeaders = 12;

                            break;
                        case TipoPontuacaoFidelidade.Campanha:
                            switch (pontuacaoFidelidade.TipoCampanhaFidelidade)
                            {
                                case TipoCampanhaFidelidade.PorVenda:
                                    subHeaderUmMeio.Colspan = 6;
                                    subHeaderDois.Colspan = 6;
                                    subHeaderPNE.Colspan = 6;
                                    emptyCell.Colspan = colspanCodigo + colspanSituacao;
                                    colspanSubHeaders = 21;
                                    break;
                                case TipoCampanhaFidelidade.PorVendaMinima:
                                    pagina.AddCell(subHeaderOffSet);
                                    subHeaderUmMeio.Colspan = 6;
                                    subHeaderDois.Colspan = 6;
                                    subHeaderPNE.Colspan = 6;
                                    emptyCell.Colspan = colspanCodigo + colspanSituacao;
                                    colspanSubHeaders = 22;
                                    break;
                                case TipoCampanhaFidelidade.PorVendaMinimaEmpreendimento:
                                    pagina.AddCell(subHeaderOffSet);
                                    subHeaderUmMeio.Colspan = 6;
                                    subHeaderDois.Colspan = 6;
                                    subHeaderPNE.Colspan = 6;
                                    emptyCell.Colspan = colspanCodigo + colspanSituacao;
                                    colspanSubHeaders = 22;
                                    break;
                            }
                            break;
                    }
                    pagina.AddCell(subHeaderOffSet);
                    pagina.AddCell(subHeaderUmMeio);
                    pagina.AddCell(subHeaderDois);
                    pagina.AddCell(subHeaderPNE);
                    pagina.AddCell(emptyCell);
                    if (numColunas - colspanSubHeaders != 0)
                    {
                        var offsetCell2 = CreateCell("", TitleFont, numColunas - colspanSubHeaders);
                        offsetCell2.BorderColor = borderColor;
                        offsetCell2.Border = Rectangle.LEFT_BORDER;
                        offsetCell2.BorderWidth = borderWidth;
                        offsetCell2.PaddingLeft = paddingLeft;
                        pagina.AddCell(offsetCell2);
                    }
                }

                #endregion

                #region column
                var empreendimentoCellHeader = CreateCellHeader(GlobalMessages.Empreendimento.ToUpper(), ColumnFont,
                   colspanEmpreendimento);
                pagina.AddCell(empreendimentoCellHeader);

                switch (modalidade)
                {
                    case TipoModalidadeProgramaFidelidade.Fixa:
                        switch (pontuacaoFidelidade.TipoPontuacaoFidelidade)
                        {
                            case TipoPontuacaoFidelidade.Normal:
                                var faixaUmMeioCell = CreateCellHeader(GlobalMessages.ColunaPontuacaoFaixaoUmMeio.ToUpper(), ColumnFont,
                                    colspanFaixaUmMeio);
                                pagina.AddCell(faixaUmMeioCell);

                                var faixaDoisCell = CreateCellHeader(GlobalMessages.ColunaPontuacaoFaixaDois.ToUpper(), ColumnFont,
                                    colspanFaixaDois);
                                pagina.AddCell(faixaDoisCell);
                                colspanAtual = 5;
                                break;
                            case TipoPontuacaoFidelidade.Campanha:
                                var fatorFaixaUmMeio = CreateCellHeader("Fator F 1.5", ColumnFont, colspanFatorFaixa);
                                pagina.AddCell(fatorFaixaUmMeio);

                                var fatorPontuacaoPadraoFaixaUmMeio = CreateCellHeader("Pontuação Padrão F 1.5", ColumnFont, colspanPontPadrao);
                                pagina.AddCell(fatorPontuacaoPadraoFaixaUmMeio);

                                var fatorFaixaDois = CreateCellHeader("Fator F 2.0", ColumnFont, colspanFatorFaixa);
                                pagina.AddCell(fatorFaixaDois);

                                var fatorPontuacaoPadraoFaixaDois = CreateCellHeader("Pontuação Padrão F 2.0", ColumnFont, colspanPontPadrao);
                                pagina.AddCell(fatorPontuacaoPadraoFaixaDois);
                                colspanAtual = 7;
                                if (pontuacaoFidelidade.TipoCampanhaFidelidade == TipoCampanhaFidelidade.PorVendaMinima)
                                {
                                    var quantidadeMinima = CreateCellHeader("Qtd. Mínima", ColumnFont, colspanQuantMinima);
                                    pagina.AddCell(quantidadeMinima);
                                    colspanAtual = 8;
                                }

                                if (pontuacaoFidelidade.TipoCampanhaFidelidade == TipoCampanhaFidelidade.PorVendaMinimaEmpreendimento)
                                {
                                    var quantidadeMinimaEmpreendimento = CreateCellHeader("Qtd. Mínima Empreendimento", ColumnFont, colspanQuantMinima);
                                    pagina.AddCell(quantidadeMinimaEmpreendimento);
                                    colspanAtual = 8;
                                }

                                break;
                        }
                        break;
                    case TipoModalidadeProgramaFidelidade.Nominal:

                        if (pontuacaoFidelidade.TipoPontuacaoFidelidade == TipoPontuacaoFidelidade.Campanha &&
                            pontuacaoFidelidade.TipoCampanhaFidelidade != TipoCampanhaFidelidade.PorVenda)
                        {
                            var quantidadeMinima = CreateCellHeader("Qtd. Mínima", ColumnFont, colspanQuantMinima);
                            pagina.AddCell(quantidadeMinima);
                            colspanAtual = 22;
                        }

                        for (var i = 0; i < 3; i++)
                        {
                            switch (pontuacaoFidelidade.TipoPontuacaoFidelidade)
                            {
                                case TipoPontuacaoFidelidade.Normal:
                                    var pontuacaoSeca = CreateCellHeader("Pontuação Seca", ColumnFont, colspanFatorNominal);
                                    pagina.AddCell(pontuacaoSeca);

                                    var pontuacaoNormal = CreateCellHeader("Pontuação Normal", ColumnFont, colspanFatorNominal);
                                    pagina.AddCell(pontuacaoNormal);

                                    var pontuacaoTurbinada = CreateCellHeader("Pontuação Turbinada", ColumnFont, colspanFatorNominal);
                                    pagina.AddCell(pontuacaoTurbinada);
                                    colspanAtual = 12;

                                    break;
                                case TipoPontuacaoFidelidade.Campanha:
                                    var fatorSeca = CreateCellHeader("Fator Seca", ColumnFont, colspanFatorNominal);
                                    pagina.AddCell(fatorSeca);

                                    var pontuacaoPadraoSeca = CreateCellHeader("Pontuação Padrão Seca", ColumnFont, colspanPadraoNominal);
                                    pagina.AddCell(pontuacaoPadraoSeca);

                                    var fatorNormal = CreateCellHeader("Fator Normal", ColumnFont, colspanFatorNominal);
                                    pagina.AddCell(fatorNormal);

                                    var pontuacaoPadraoNormal = CreateCellHeader("Pontuação Padrão Normal", ColumnFont, colspanPadraoNominal);
                                    pagina.AddCell(pontuacaoPadraoNormal);

                                    var fatorTurbinada = CreateCellHeader("Fator Turbinada", ColumnFont, colspanFatorNominal);
                                    pagina.AddCell(fatorTurbinada);

                                    var pontuacaoPadraoTurbinada = CreateCellHeader("Pontuação Padrão Turbinada", ColumnFont, colspanPadraoNominal);
                                    pagina.AddCell(pontuacaoPadraoTurbinada);
                                    colspanAtual = colspanAtual == 0 ? 21 : colspanAtual;

                                    break;
                            }
                        }
                        break;
                }
                var situacaoCell = CreateCellHeader(GlobalMessages.Situacao.ToUpper(), ColumnFont, colspanSituacao);
                pagina.AddCell(situacaoCell);

                var codigoCell = CreateCellHeader(GlobalMessages.Codigo.ToUpper(), ColumnFont, colspanCodigo);
                pagina.AddCell(codigoCell);

                if (numColunas - colspanAtual != 0)
                {
                    var offsetCell2 = CreateCell("", TitleFont, numColunas - colspanAtual);
                    offsetCell2.BorderColor = borderColor;
                    offsetCell2.Border = Rectangle.LEFT_BORDER;
                    offsetCell2.BorderWidth = borderWidth;
                    offsetCell2.PaddingLeft = paddingLeft;
                    pagina.AddCell(offsetCell2);
                }

                #endregion

                #region Data
                switch (pontuacaoFidelidade.TipoPontuacaoFidelidade)
                {
                    case TipoPontuacaoFidelidade.Normal:
                        foreach (var empreendimento in empreendimentos)
                        {
                            var itensPontuacaoFidelidade = itens.Where(x => x.Empreendimento.Id == empreendimento.Id)
                                .Where(x => x.EmpresaVenda.Id == empresaVenda.Id)
                                .Where(x => x.Modalidade == modalidade)
                                .SingleOrDefault();
                            
                            var empreendimentoCell = CreateCell(empreendimento.Nome, TextFont,
                            colspanEmpreendimento);
                            empreendimentoCell.PaddingLeft = paddingLeft;
                            pagina.AddCell(empreendimentoCell);
                            switch (modalidade)
                            {
                                case TipoModalidadeProgramaFidelidade.Fixa:
                                    var faixaUmMeio = CreateCell(itensPontuacaoFidelidade.PontuacaoFaixaUmMeio.ToString("F"),
                                        TextFont,
                                        colspanFaixaUmMeio);
                                    faixaUmMeio.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                    pagina.AddCell(faixaUmMeio);

                                    var faixaDois = CreateCell(itensPontuacaoFidelidade.PontuacaoFaixaDois.ToString("F"),
                                        TextFont,
                                        colspanFaixaDois);
                                    faixaDois.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                    pagina.AddCell(faixaDois);

                                    break;
                                case TipoModalidadeProgramaFidelidade.Nominal:
                                    var pontuacaofaixaUmMeioSeca = CreateCell(itensPontuacaoFidelidade.PontuacaoFaixaUmMeioSeca.ToString("F"),
                                       TextFont,
                                       colspanFaixaDois);
                                    pontuacaofaixaUmMeioSeca.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                    pontuacaofaixaUmMeioSeca.BackgroundColor = new Color(200, 200, 200);
                                    pagina.AddCell(pontuacaofaixaUmMeioSeca);
                                    var pontuacaoFaixaUmMeioNormal = CreateCell(itensPontuacaoFidelidade.PontuacaoFaixaUmMeioNormal.ToString("F"),
                                       TextFont,
                                       colspanFaixaDois);
                                    pontuacaoFaixaUmMeioNormal.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                    pontuacaoFaixaUmMeioNormal.BackgroundColor = new Color(200, 200, 200);
                                    pagina.AddCell(pontuacaoFaixaUmMeioNormal);
                                    var pontuacaoFaixaUmMeioTurbinada = CreateCell(itensPontuacaoFidelidade.PontuacaoFaixaUmMeioTurbinada.ToString("F"),
                                       TextFont,
                                       colspanFaixaDois);
                                    pontuacaoFaixaUmMeioTurbinada.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                    pontuacaoFaixaUmMeioTurbinada.BackgroundColor = new Color(200, 200, 200);
                                    pagina.AddCell(pontuacaoFaixaUmMeioTurbinada);

                                    var pontuacaoFaixaDoisSeca = CreateCell(itensPontuacaoFidelidade.PontuacaoFaixaDoisSeca.ToString("F"),
                                       TextFont,
                                       colspanFaixaDois);
                                    pontuacaoFaixaDoisSeca.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                    pagina.AddCell(pontuacaoFaixaDoisSeca);
                                    var pontuacaoFaixaDoisNormal = CreateCell(itensPontuacaoFidelidade.PontuacaoFaixaDoisNormal.ToString("F"),
                                       TextFont,
                                       colspanFaixaDois);
                                    pontuacaoFaixaDoisNormal.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                    pagina.AddCell(pontuacaoFaixaDoisNormal);
                                    var pontuacaoFaixaDoisTurbinada = CreateCell(itensPontuacaoFidelidade.PontuacaoFaixaDoisTurbinada.ToString("F"),
                                       TextFont,
                                       colspanFaixaDois);
                                    pontuacaoFaixaDoisTurbinada.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                    pagina.AddCell(pontuacaoFaixaDoisTurbinada);

                                    var pontuacaoPNESeca = CreateCell(itensPontuacaoFidelidade.PontuacaoPNESeca.ToString("F"),
                                       TextFont,
                                       colspanFaixaDois);
                                    pontuacaoPNESeca.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                    pontuacaoPNESeca.BackgroundColor = new Color(200, 200, 200);
                                    pagina.AddCell(pontuacaoPNESeca);
                                    var pontuacaoPNENormal = CreateCell(itensPontuacaoFidelidade.PontuacaoPNENormal.ToString("F"),
                                       TextFont,
                                       colspanFaixaDois);
                                    pontuacaoPNENormal.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                    pontuacaoPNENormal.BackgroundColor = new Color(200, 200, 200);
                                    pagina.AddCell(pontuacaoPNENormal);
                                    var pontuacaoPNETurbinada = CreateCell(itensPontuacaoFidelidade.PontuacaoPNETurbinada.ToString("F"),
                                       TextFont,
                                       colspanFaixaDois);
                                    pontuacaoPNETurbinada.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                    pontuacaoPNETurbinada.BackgroundColor = new Color(200, 200, 200);
                                    pagina.AddCell(pontuacaoPNETurbinada);

                                    break;
                            }
                            var situacao = CreateCell(itensPontuacaoFidelidade.Situacao.ToString(),
                                       TextFont,
                                       colspanFaixaDois);
                            situacao.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            pagina.AddCell(situacao);

                            var codigo = CreateCell(itensPontuacaoFidelidade.Codigo,
                                       TextFont,
                                       colspanFaixaDois);
                            codigo.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                            pagina.AddCell(codigo);
                            if (numColunas - colspanAtual != 0)
                            {
                                var offsetCell2 = CreateCell("", TextFont, numColunas - colspanAtual);
                                offsetCell2.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                pagina.AddCell(offsetCell2);
                            }

                        }
                        break;
                    case TipoPontuacaoFidelidade.Campanha:
                        List<long> lista = new List<long>();
                        if (TipoCampanhaFidelidade.PorVenda != pontuacaoFidelidade.TipoCampanhaFidelidade)
                        {
                            lista = new JavaScriptSerializer().Deserialize<List<long>>(pontuacaoFidelidade.QuantidadesMinimas);
                        }

                        var progressao = pontuacaoFidelidade.Progressao == 0 ? 1 : pontuacaoFidelidade.Progressao;
                        foreach (var empreendimento in empreendimentos)
                        {
                            var pontos = itens.Where(x => x.Empreendimento.Id == empreendimento.Id)
                                .OrderBy(x => x.QuantidadeMinima).ToList();
                            for (var i = 0; i < progressao; i++)
                            {

                                var itensPontuacaoFidelidade = new ItemPontuacaoFidelidade();
                                if (TipoCampanhaFidelidade.PorVenda == pontuacaoFidelidade.TipoCampanhaFidelidade)
                                {
                                    itensPontuacaoFidelidade = pontos[i];
                                    //itensPontuacaoFidelidade = _itemPontuacaoFidelidadeRepository
                                    //                       .BuscarItemAtivoPorEmpreendimentoEmpresaVenda(empreendimento.Id, empresaVenda.Id, modalidade);
                                }
                                else
                                {
                                    itensPontuacaoFidelidade = pontos[i];

                                    //itensPontuacaoFidelidade = _itemPontuacaoFidelidadeRepository
                                    //                       .BuscarItemAtivoPorEmpreendimentoEmpresaVendaQuant(empreendimento.Id, empresaVenda.Id, modalidade, lista[i]);
                                }

                                if (i == 0)
                                {
                                    var empreendimentoCell = CreateCell(empreendimento.Nome, TextFont,
                                   colspanEmpreendimento);
                                    empreendimentoCell.PaddingLeft = paddingLeft;
                                    pagina.AddCell(empreendimentoCell);
                                }
                                else
                                {
                                    var empreendimentoCell = CreateCell(" ", TextFont,
                                  colspanEmpreendimento);
                                    empreendimentoCell.PaddingLeft = paddingLeft;
                                    pagina.AddCell(empreendimentoCell);
                                }
                                switch (modalidade)
                                {
                                    case TipoModalidadeProgramaFidelidade.Fixa:
                                        //F1.5
                                        var fatorUmMeio = CreateCell(itensPontuacaoFidelidade.FatorUmMeio.ToString("F"),
                                        TextFont,
                                        colspanFatorFaixa);
                                        fatorUmMeio.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        pagina.AddCell(fatorUmMeio);

                                        var pontuacaoPadraoUmMeio = CreateCell(itensPontuacaoFidelidade.PontuacaoPadraoUmMeio.ToString("F"),
                                        TextFont,
                                        colspanPontPadrao);
                                        pontuacaoPadraoUmMeio.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        pagina.AddCell(pontuacaoPadraoUmMeio);

                                        //F2.0
                                        var fatorDois = CreateCell(itensPontuacaoFidelidade.FatorDois.ToString("F"),
                                        TextFont,
                                        colspanFatorFaixa);
                                        fatorDois.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        pagina.AddCell(fatorDois);

                                        var pontuacaoPadraoDois = CreateCell(itensPontuacaoFidelidade.PontuacaoPadraoDois.ToString("F"),
                                        TextFont,
                                        colspanPontPadrao);
                                        pontuacaoPadraoDois.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        pagina.AddCell(pontuacaoPadraoDois);

                                        if (pontuacaoFidelidade.TipoCampanhaFidelidade != TipoCampanhaFidelidade.PorVenda)
                                        {
                                            var quantidadeMinima = CreateCell(itensPontuacaoFidelidade.QuantidadeMinima.ToString("F"),
                                            TextFont,
                                            colspanQuantMinima);
                                            quantidadeMinima.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                            pagina.AddCell(quantidadeMinima);
                                        }

                                        break;
                                    case TipoModalidadeProgramaFidelidade.Nominal:
                                        if (pontuacaoFidelidade.TipoCampanhaFidelidade != TipoCampanhaFidelidade.PorVenda)
                                        {
                                            var quantidadeMinima = CreateCell(itensPontuacaoFidelidade.QuantidadeMinima.ToString("F"),
                                            TextFont,
                                            colspanQuantMinima);
                                            quantidadeMinima.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                            pagina.AddCell(quantidadeMinima);
                                        }
                                        //F1.5

                                        var fatorUmMeioSeca = CreateCell(itensPontuacaoFidelidade.FatorUmMeioSeca.ToString("F"),
                                       TextFont,
                                       colspanFatorNominal);
                                        fatorUmMeioSeca.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        fatorUmMeioSeca.BackgroundColor = new Color(200, 200, 200);
                                        pagina.AddCell(fatorUmMeioSeca);
                                        var pontuacaoFaixaUmMeioSeca = CreateCell(itensPontuacaoFidelidade.PontuacaoFaixaUmMeioSeca.ToString("F"),
                                       TextFont,
                                       colspanFatorNominal);
                                        pontuacaoFaixaUmMeioSeca.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        pontuacaoFaixaUmMeioSeca.BackgroundColor = new Color(200, 200, 200);
                                        pagina.AddCell(pontuacaoFaixaUmMeioSeca);

                                        var fatorUmMeioNormal = CreateCell(itensPontuacaoFidelidade.FatorUmMeioNormal.ToString("F"),
                                       TextFont,
                                       colspanFatorNominal);
                                        fatorUmMeioNormal.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        fatorUmMeioNormal.BackgroundColor = new Color(200, 200, 200);
                                        pagina.AddCell(fatorUmMeioNormal);
                                        var pontuacaoFaixaUmMeioNormal = CreateCell(itensPontuacaoFidelidade.PontuacaoFaixaUmMeioNormal.ToString("F"),
                                       TextFont,
                                       colspanFatorNominal);
                                        pontuacaoFaixaUmMeioNormal.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        pontuacaoFaixaUmMeioNormal.BackgroundColor = new Color(200, 200, 200);
                                        pagina.AddCell(pontuacaoFaixaUmMeioNormal);

                                        var fatorUmMeioTurbinada = CreateCell(itensPontuacaoFidelidade.FatorUmMeioTurbinada.ToString("F"),
                                      TextFont,
                                      colspanFatorNominal);
                                        fatorUmMeioTurbinada.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        fatorUmMeioTurbinada.BackgroundColor = new Color(200, 200, 200);
                                        pagina.AddCell(fatorUmMeioTurbinada);
                                        var pontuacaoFaixaUmMeioTurbinada = CreateCell(itensPontuacaoFidelidade.PontuacaoFaixaUmMeioTurbinada.ToString("F"),
                                       TextFont,
                                       colspanFatorNominal);
                                        pontuacaoFaixaUmMeioTurbinada.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        pontuacaoFaixaUmMeioTurbinada.BackgroundColor = new Color(200, 200, 200);
                                        pagina.AddCell(pontuacaoFaixaUmMeioTurbinada);


                                        //F2.0
                                        var fatorDoisSeca = CreateCell(itensPontuacaoFidelidade.FatorDoisSeca.ToString("F"),
                                       TextFont,
                                       colspanFatorNominal);
                                        fatorDoisSeca.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        pagina.AddCell(fatorDoisSeca);
                                        var pontuacaoFaixaDoisSeca = CreateCell(itensPontuacaoFidelidade.PontuacaoFaixaDoisSeca.ToString("F"),
                                       TextFont,
                                       colspanFatorNominal);
                                        pontuacaoFaixaDoisSeca.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        pagina.AddCell(pontuacaoFaixaDoisSeca);

                                        var fatorDoisNormal = CreateCell(itensPontuacaoFidelidade.FatorDoisNormal.ToString("F"),
                                       TextFont,
                                       colspanFatorNominal);
                                        fatorDoisNormal.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        pagina.AddCell(fatorDoisNormal);
                                        var pontuacaoFaixaDoisNormal = CreateCell(itensPontuacaoFidelidade.PontuacaoFaixaDoisNormal.ToString("F"),
                                       TextFont,
                                       colspanFatorNominal);
                                        pontuacaoFaixaDoisNormal.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        pagina.AddCell(pontuacaoFaixaDoisNormal);

                                        var fatorDoisTurbinada = CreateCell(itensPontuacaoFidelidade.FatorDoisTurbinada.ToString("F"),
                                      TextFont,
                                      colspanFatorNominal);
                                        fatorDoisTurbinada.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        pagina.AddCell(fatorDoisTurbinada);
                                        var pontuacaoFaixaDoisTurbinada = CreateCell(itensPontuacaoFidelidade.PontuacaoFaixaDoisTurbinada.ToString("F"),
                                       TextFont,
                                       colspanFatorNominal);
                                        pontuacaoFaixaDoisTurbinada.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        pagina.AddCell(pontuacaoFaixaDoisTurbinada);


                                        //PNE
                                        var fatorPNESeca = CreateCell(itensPontuacaoFidelidade.FatorPNESeca.ToString("F"),
                                       TextFont,
                                       colspanFatorNominal);
                                        fatorPNESeca.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        fatorPNESeca.BackgroundColor = new Color(200, 200, 200);
                                        pagina.AddCell(fatorPNESeca);
                                        var pontuacaoPNESeca = CreateCell(itensPontuacaoFidelidade.PontuacaoPNESeca.ToString("F"),
                                       TextFont,
                                       colspanFatorNominal);
                                        pontuacaoPNESeca.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        pontuacaoPNESeca.BackgroundColor = new Color(200, 200, 200);
                                        pagina.AddCell(pontuacaoPNESeca);

                                        var fatorPNENormal = CreateCell(itensPontuacaoFidelidade.FatorPNENormal.ToString("F"),
                                       TextFont,
                                       colspanFatorNominal);
                                        fatorPNENormal.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        fatorPNENormal.BackgroundColor = new Color(200, 200, 200);
                                        pagina.AddCell(fatorPNENormal);
                                        var pontuacaoFaixaPNENormal = CreateCell(itensPontuacaoFidelidade.PontuacaoPNENormal.ToString("F"),
                                       TextFont,
                                       colspanFatorNominal);
                                        pontuacaoFaixaPNENormal.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        pontuacaoFaixaPNENormal.BackgroundColor = new Color(200, 200, 200);
                                        pagina.AddCell(pontuacaoFaixaPNENormal);

                                        var fatorPNETurbinada = CreateCell(itensPontuacaoFidelidade.FatorPNETurbinada.ToString("F"),
                                      TextFont,
                                      colspanFatorNominal);
                                        fatorPNETurbinada.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        fatorPNETurbinada.BackgroundColor = new Color(200, 200, 200);
                                        pagina.AddCell(fatorPNETurbinada);
                                        var pontuacaoPNETurbinada = CreateCell(itensPontuacaoFidelidade.PontuacaoPNETurbinada.ToString("F"),
                                       TextFont,
                                       colspanFatorNominal);
                                        pontuacaoPNETurbinada.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                        pontuacaoPNETurbinada.BackgroundColor = new Color(200, 200, 200);
                                        pagina.AddCell(pontuacaoPNETurbinada);

                                        break;
                                }

                                var situacao = CreateCell(itensPontuacaoFidelidade.Situacao.ToString(),
                                       TextFont,
                                       colspanFaixaDois);
                                situacao.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                pagina.AddCell(situacao);

                                var codigo = CreateCell(itensPontuacaoFidelidade.Codigo,
                                           TextFont,
                                           colspanFaixaDois);
                                codigo.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                pagina.AddCell(codigo);

                                if (numColunas - colspanAtual != 0)
                                {
                                    var offsetCell2 = CreateCell("", TextFont, numColunas - colspanAtual);
                                    offsetCell2.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                    pagina.AddCell(offsetCell2);
                                }

                            }
                        }
                        break;
                }
                #endregion
            }
            return pagina;
        }
        #endregion
    }
}

