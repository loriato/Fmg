using Europa.Extensions;
using Europa.Resources;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Color = iTextSharp.text.Color;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class NotaFiscalPagamentoPdfService
    {
        private EmpresaVendaRepository _empresaVendaRepository { get; set; }
        private EnderecoEstabelecimentoRepository _enderecoEstabelecimentoRepository { get; set; }
        private ViewNotaFiscalPagamentoRepository _viewNotaFiscalPagamentoRepository { get; set; }
        private EmpreendimentoRepository _empreendimentoRepository { get; set; }
        private ValorNominalRepository _valorNominalRepository { get; set; }
        private EnderecoFornecedorRepository _enderecoFornecedorRepository { get; set; }

        private Font TitleFont;
        private Font TextFont;
        private Font RedTextFont;
        private Font SubTitleFont;
        private Font SubHeaderFont;
        private Font HeaderFont;
        private Font ValorNotaTitleFont;
        private Font ValorNotaFont;
        private int num = 10;

        private Color RedTenda = new Color(228, 0, 43);
        private Color MarineBlue = new Color(0, 62, 98);
        private Color LightBlue = new Color(187, 229, 238);
        private Color VeryLightGray = new Color(239, 239, 239);

        private bool _fontInitialized;

        public NotaFiscalPagamentoPdfService()
        {
            
        }

        private void EnsureFontInitialized()
        {
            try
            {
                if (!_fontInitialized)
                {
                    FontFactory.RegisterDirectory("C:\\WINDOWS\\Fonts");
                    TitleFont = FontFactory.GetFont("AktivGrotesk", 13, Font.BOLD, RedTenda);
                    SubTitleFont = FontFactory.GetFont("AktivGrotesk", num, Font.BOLD, MarineBlue);
                    TextFont = FontFactory.GetFont("AktivGrotesk", num, Font.NORMAL, MarineBlue);
                    SubHeaderFont = FontFactory.GetFont("AktivGrotesk", num, Font.NORMAL, Color.BLACK);
                    HeaderFont = FontFactory.GetFont("AktivGrotesk", 14, Font.BOLD, new Color(179, 179, 179));
                    ValorNotaTitleFont = FontFactory.GetFont("AktivGrotesk", 12, Font.BOLD, Color.WHITE);
                    ValorNotaFont = FontFactory.GetFont("AktivGrotesk", 12, Font.BOLD, Color.WHITE);
                }
            }
            catch (Exception ex)
            {
                ExceptionLogger.LogException(ex);
                throw;
            }
        }

        public Dictionary<long, byte[]> CriarPdf(NotaFiscalPagamentoPdfDTO dto, byte[] logo)
        {
            EnsureFontInitialized();

            var documentos = new Dictionary<long, byte[]>();

            var stream = new MemoryStream();
            var documentoGeral = CreateDocument();

            PdfWriter.GetInstance(documentoGeral, stream);

            documentoGeral.Open();

            IncluirConteudo(documentoGeral, dto, logo);

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

        private void IncluirConteudo(Document document, NotaFiscalPagamentoPdfDTO dto, byte[] logo)
        {
            var empresaVenda = _empresaVendaRepository.FindById(dto.IdEmpresaVenda);
            var empreendimento = _empreendimentoRepository.FindById(dto.IdEmpreendimento);
            var viewNotaFiscalPagamento = _viewNotaFiscalPagamentoRepository.BuscarPagamentos(dto.IdEmpresaVenda, dto.PedidoSap);

            var endereco = _enderecoFornecedorRepository.BuscarEnderecoPorCodigoERegional(empreendimento.CodigoEmpresa, empresaVenda.Estado);

            var notaFiscal = new NotaFiscalDTO();

            if (endereco.HasValue())
            {
                notaFiscal.CnpjEstabelecimento = endereco.Cnpj.ToCNPJFormat();
                notaFiscal.RazaoSocialEstabelecimento = endereco.RazaoSocial;
                notaFiscal.LogradouroEstabelecimento = endereco.Logradouro;
                notaFiscal.NumeroEstabelecimento = endereco.Numero;
                notaFiscal.BairroEstabelecimento = endereco.Bairro;
                notaFiscal.CidadeEstabelecimento = endereco.Cidade;
                notaFiscal.CepEstabelecimento = endereco.Cep.ToCEPFormat();
                notaFiscal.RegionalEstabelecimento = endereco.Estado;
            }

            if (endereco.IsEmpty())
            {
                var estabelecimento = _enderecoEstabelecimentoRepository.FindByEmpreendimento(empreendimento.Id);

                if (estabelecimento.HasValue())
                {
                    notaFiscal.CnpjEstabelecimento = empreendimento.CNPJ.ToCNPJFormat();
                    notaFiscal.RazaoSocialEstabelecimento = empreendimento.NomeEmpresa;
                    notaFiscal.LogradouroEstabelecimento = estabelecimento.Logradouro;
                    notaFiscal.NumeroEstabelecimento = estabelecimento.Numero;
                    notaFiscal.BairroEstabelecimento = estabelecimento.Bairro;
                    notaFiscal.CidadeEstabelecimento = estabelecimento.Cidade;
                    notaFiscal.CepEstabelecimento = estabelecimento.Cep.ToCEPFormat();
                    notaFiscal.RegionalEstabelecimento = estabelecimento.Estado;
                }
            }

            var page = CriarConteudoPaginaPrincipal(empresaVenda, empreendimento, viewNotaFiscalPagamento,notaFiscal, dto.PedidoSap, logo);
            document.Add(page);
            page = CriarConteudoResumido(empresaVenda, empreendimento, viewNotaFiscalPagamento, notaFiscal, dto.PedidoSap, logo);
            document.Add(page);
            document.NewPage();
            int index = 1;
            foreach(var item in viewNotaFiscalPagamento)
            {
            page = CriarConteudoDetalhado(empresaVenda, empreendimento, item, notaFiscal, dto.PedidoSap, logo, index);
            document.Add(page);
            document.NewPage();
            index++;
            }

        }

        private IElement CriarConteudoPaginaPrincipal(Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda, Empreendimento empreendimento, List<ViewNotaFiscalPagamento> viewNotaFiscalPagamento, NotaFiscalDTO notaFiscalDTO, string PedidoSap, byte[] logo)
        {
            var Data = DateTime.Now;
            var numColunas = 24;

            int colspanCNPJTitulo = 2,
                colspanCNPJ = 6,
                colspanRazaoSocialTitulo = 4,
                colspanRazaoSocial = 11,
                colspanEnderecoTitulo = 3,
                colspanEndereco = 21,
                colspanMunicipioTitulo = 3,
                colspanMunicipio = 16,
                colspanUFTitulo = 2,
                colspanUF = 2;
                //colspanNumeroPedidoTitulo = 5,
                //colspanNumeroPedido = 5,
                //colspanClienteTitulo = 3,
                //colspanCliente = 24,
                //colspanPropostaTitulo = 2,
                //colspanProposta = 5,
                //colspanComissaoTiTulo = 3,
                //colspanComissao = 4,
                //colspanCodigoTitulo = 5,
                //colspanCodigo = 2,
                //colspanPrePropostaTitulo = 2,
                //colspanPreProposta = 5,
                //colspanEmpreendimento = 5,
                //colspanTorreTitulo = 2,
                //colspanTorreConteudo = 3,
                //colspanUnidade = 3,
                //colspanVgvSemPremiada = 5,
                //colspanParcelaTitulo = 3,
                //colspanParcelaConteudo = 5,
                //colspanTabelaTitulo = 2,
                //colspanTabelaConteudo = 2,
                //colspanSubTabelaTitulo = 3,
                //colspanSubTabelaConteudo = 3,
                //colspanPorcentagemTitulo = 4,
                //colspanPorcentagemConteudo = 2,
                //colspanFaixaTitulo = 2,
                //colspanFaixaConteudo = 2;

            var pagina = new PdfPTable(numColunas);
            pagina.WidthPercentage = 100;
            pagina.KeepTogether = false;
            var offsetCell = CreateCellOffset("", TitleFont, 1);

            var offsetCellLinha = CreateCellOffset("", TitleFont, numColunas);
            var logoCell = CreateImageCell(logo);
            pagina.AddCell(logoCell);

            //Prestador
            var tituloEvCell = CreateCellHeader("PRESTADOR DE SERVIÇOS", TitleFont,
                numColunas);
            pagina.AddCell(tituloEvCell);

            //Linha 1
            var CNPJTituloCell = CreateCell(GlobalMessages.Cnpj.ToUpper() + ":", SubTitleFont, colspanCNPJTitulo);
            pagina.AddCell(CNPJTituloCell);

            var CNPJEvCell = CreateCell(empresaVenda.CNPJ.ToCNPJFormat(), TextFont, colspanCNPJ);
            pagina.AddCell(CNPJEvCell);

            pagina.AddCell(offsetCell);

            var RazaoSocialTituloCell = CreateCell(GlobalMessages.RazaoSocial + ":", SubTitleFont, colspanRazaoSocialTitulo);
            pagina.AddCell(RazaoSocialTituloCell);

            var RazaoSocialEvCell = CreateCell(empresaVenda.RazaoSocial, TextFont, colspanRazaoSocial);
            pagina.AddCell(RazaoSocialEvCell);

            //Linha 2
            var EnderecoTituloCell = CreateCell(GlobalMessages.Endereco + ":", SubTitleFont, colspanEnderecoTitulo);
            pagina.AddCell(EnderecoTituloCell);
            var enderecoEv = empresaVenda.Logradouro + ", " + empresaVenda.Numero + ", " + empresaVenda.Bairro + " - CEP " + empresaVenda.Cep.ToCEPFormat();
            enderecoEv = enderecoEv.Replace(",,", ",");
            var EnderecoEvCell = CreateCell(enderecoEv, TextFont, colspanEndereco);
            pagina.AddCell(EnderecoEvCell);

            //Linha 3 
            var MunicipioTituloCell = CreateCell(GlobalMessages.Municipio + ":", SubTitleFont, colspanMunicipioTitulo);
            pagina.AddCell(MunicipioTituloCell);
            var MunicipioEvCell = CreateCell(empresaVenda.Cidade, TextFont, colspanMunicipio);
            pagina.AddCell(MunicipioEvCell);

            pagina.AddCell(offsetCell);

            var UFTituloCell = CreateCell(GlobalMessages.UF + ":", SubTitleFont, colspanUFTitulo);
            pagina.AddCell(UFTituloCell);
            var UFEvCell = CreateCell(empresaVenda.Estado, TextFont, colspanUF);
            pagina.AddCell(UFEvCell);

            //Tomador
            pagina.AddCell(offsetCellLinha);
            var tituloTomadorCell = CreateCellHeader("TOMADOR DE SERVIÇOS", TitleFont,
                numColunas);
            pagina.AddCell(tituloTomadorCell);

            //Linha 1
            pagina.AddCell(CNPJTituloCell);
            var CNPJTomadorCell = CreateCell(notaFiscalDTO.CnpjEstabelecimento.ToCNPJFormat(), TextFont, colspanCNPJ);
            pagina.AddCell(CNPJTomadorCell);

            pagina.AddCell(offsetCell);

            pagina.AddCell(RazaoSocialTituloCell);
            var RazaoSocialTomadorCell = CreateCell(notaFiscalDTO.RazaoSocialEstabelecimento, TextFont, colspanRazaoSocial);
            pagina.AddCell(RazaoSocialTomadorCell);

            //Linha 2

            pagina.AddCell(EnderecoTituloCell);
            var enderecoTomador = notaFiscalDTO.LogradouroEstabelecimento + ", " + notaFiscalDTO.NumeroEstabelecimento + ", " + notaFiscalDTO.BairroEstabelecimento + " - CEP " + notaFiscalDTO.CepEstabelecimento.ToCEPFormat();
            enderecoTomador = enderecoTomador.Replace(",,", ",");
            var EnderecoTomadorCell = CreateCell(enderecoTomador, TextFont, colspanEndereco);
            pagina.AddCell(EnderecoTomadorCell);

            //Linha 3

            pagina.AddCell(MunicipioTituloCell);
            var MunicipioTomadorCell = CreateCell(notaFiscalDTO.CidadeEstabelecimento, TextFont, colspanMunicipio);
            pagina.AddCell(MunicipioTomadorCell);

            pagina.AddCell(offsetCell);

            pagina.AddCell(UFTituloCell);
            var UFTomadorCell = CreateCell(notaFiscalDTO.RegionalEstabelecimento, TextFont, colspanUF);
            pagina.AddCell(UFTomadorCell);



            //Linhas seq
            return pagina;
        }

        private IElement CriarConteudoResumido(Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda, Empreendimento empreendimento, List<ViewNotaFiscalPagamento> viewNotaFiscalPagamento, NotaFiscalDTO notaFiscalDTO, string PedidoSap, byte[] logo)
        {
            var Data = DateTime.Now;
            var numColunas = 24;

            int colspanClienteTitulo = 3,
                colspanCliente = 24,
                colspanPropostaTitulo = 2,
                colspanProposta = 5,
                colspanComissaoTiTulo = 3,
                colspanComissao = 4,
                colspanNumeroPedidoTitulo = 5,
                colspanNumeroPedido = 5;


            var pagina = new PdfPTable(numColunas);
            pagina.WidthPercentage = 100;
            var offsetCell = CreateCellOffset("", TitleFont, 1);

            var offsetCellLinha = CreateCellOffset("", TitleFont, numColunas);


            //Descrição de Servicos
            pagina.AddCell(offsetCellLinha);

            var rectanCell = CreateCellHeader("", TitleFont, numColunas);
            pagina.AddCell(rectanCell);

            pagina.AddCell(offsetCellLinha);
            var TituloDescriçãoCell = CreateCellHeader("DESCRIÇÃO DOS SERVIÇOS", TitleFont,
                numColunas);
            pagina.AddCell(TituloDescriçãoCell);

            var NumeroPedidoTituloCell = CreateCell(GlobalMessages.NumeroPedido + ":", SubTitleFont, colspanNumeroPedidoTitulo);
            NumeroPedidoTituloCell.BackgroundColor = LightBlue;
            pagina.AddCell(NumeroPedidoTituloCell);

            var NumeroPedidoCell = CreateCell(PedidoSap, TextFont, colspanNumeroPedido);
            NumeroPedidoCell.BackgroundColor = VeryLightGray;
            pagina.AddCell(NumeroPedidoCell);

            offsetCell.Colspan = numColunas - colspanNumeroPedido + colspanNumeroPedidoTitulo;
            pagina.AddCell(offsetCell);


            offsetCell.Colspan = 1;

            pagina.AddCell(offsetCellLinha);
            int i = 1;
            //Linhas seq
            foreach (var item in viewNotaFiscalPagamento)
            {
                #region linha 1
                var ClienteTituloCellPreview = CreateCell(GlobalMessages.Cliente + " " + i + ":", SubTitleFont, colspanClienteTitulo);
                ClienteTituloCellPreview.BackgroundColor = LightBlue;
                pagina.AddCell(ClienteTituloCellPreview);

                var ClienteCellPreview = CreateCell(item.NomeCliente, TextFont, colspanCliente);
                ClienteCellPreview.BackgroundColor = VeryLightGray;
                pagina.AddCell(ClienteCellPreview);
                #endregion

                #region linha 2
                var PropostaTituloCellPreview = CreateCell("PRO" + ":", SubTitleFont, colspanPropostaTitulo);
                PropostaTituloCellPreview.BackgroundColor = LightBlue;
                pagina.AddCell(PropostaTituloCellPreview);

                var PropostaCellPreview = CreateCell(item.CodigoProposta, TextFont, colspanProposta);
                PropostaCellPreview.BackgroundColor = VeryLightGray;
                pagina.AddCell(PropostaCellPreview);

                offsetCell.Colspan = 1;
                pagina.AddCell(offsetCell);

                offsetCell.Colspan = 1;
                var ComissaoTituloCellPreview = CreateCell(GlobalMessages.Comissao + ":", SubTitleFont, colspanComissaoTiTulo);
                ComissaoTituloCellPreview.BackgroundColor = LightBlue;
                pagina.AddCell(ComissaoTituloCellPreview);

                var ValorAPagarPreview = item.ValorAPagar;
                var ComissaoCellPreview = CreateCell(String.Format(new CultureInfo("pt-BR"), "{0:C}", ValorAPagarPreview), TextFont, colspanComissao);
                ComissaoCellPreview.BackgroundColor = VeryLightGray;
                pagina.AddCell(ComissaoCellPreview);
                pagina.AddCell(offsetCellLinha);
                pagina.AddCell(offsetCellLinha);


                #endregion

                i++;
            }

            //SumárioNF
            pagina.AddCell(offsetCellLinha);
            var CodigoServicoTituloCell = CreateCell("Cód. do Serviço:", SubTitleFont, 5);
            CodigoServicoTituloCell.BackgroundColor = LightBlue;
            pagina.AddCell(CodigoServicoTituloCell);
            var CodigoServicoCell = CreateCell("10.05.03 - agenciamento, corretagem ou intermediação de bens imóveis", TextFont, 19);
            CodigoServicoCell.BackgroundColor = VeryLightGray;
            pagina.AddCell(CodigoServicoCell);

            pagina.AddCell(offsetCellLinha);

            var linhaCell = CreateCellOffset("", TitleFont, numColunas);
            linhaCell.Border = Rectangle.TOP_BORDER;
            linhaCell.BorderColor = MarineBlue;
            linhaCell.BorderWidth = 1;
            pagina.AddCell(linhaCell);

            var ValorNotaTituloCell = CreateCell("Valor da Nota:", ValorNotaTitleFont, 12);
            ValorNotaTituloCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            ValorNotaTituloCell.BackgroundColor = RedTenda;
            pagina.AddCell(ValorNotaTituloCell);

            var total = viewNotaFiscalPagamento.Sum(x => x.ValorAPagar);
            var ValorNotaCell = CreateCell(String.Format(new CultureInfo("pt-BR"), "{0:C}", total), ValorNotaFont, 12);
            ValorNotaCell.BackgroundColor = RedTenda;
            ValorNotaCell.HorizontalAlignment = Element.ALIGN_LEFT;
            pagina.AddCell(ValorNotaCell);
            var abc = pagina.CalculateHeights(true);
            var def = pagina.CalculateHeights(false);
            var j = pagina.Chunks;

            return pagina;
        }
                
        private IElement CriarConteudoDetalhado(Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda, Empreendimento empreendimento, ViewNotaFiscalPagamento item, NotaFiscalDTO notaFiscalDTO, string PedidoSap, byte[] logo, int index)
        {
            var Data = DateTime.Now;
            var numColunas = 24;

            int colspanClienteTitulo = 3,
                colspanCliente = 24,
                colspanPropostaTitulo = 2,
                colspanProposta = 5,
                colspanComissaoTiTulo = 3,
                colspanComissao = 4,
                colspanCodigoTitulo = 5,
                colspanCodigo = 2,
                colspanPrePropostaTitulo = 2,
                colspanPreProposta = 5,
                colspanEmpreendimento = 5,
                colspanTorreTitulo = 2,
                colspanTorreConteudo = 3,
                colspanUnidade = 3,
                colspanVgvSemPremiada = 5,
                colspanParcelaTitulo = 3,
                colspanParcelaConteudo = 5,
                colspanTabelaTitulo = 2,
                colspanTabelaConteudo = 2,
                colspanSubTabelaTitulo = 3,
                colspanSubTabelaConteudo = 3,
                colspanPorcentagemTitulo = 4,
                colspanPorcentagemConteudo = 2,
                colspanFaixaTitulo = 2,
                colspanFaixaConteudo = 2;


            var pagina = new PdfPTable(numColunas);
            pagina.WidthPercentage = 100;
            pagina.KeepTogether = true;
            var offsetCell = CreateCellOffset("", TitleFont, 1);

            var offsetCellLinha = CreateCellOffset("", TitleFont, numColunas);
            var logoCell = CreateImageCell(logo);
            pagina.AddCell(logoCell);



                #region linha 1
                var ClienteTituloCell = CreateCell(GlobalMessages.Cliente + " " + index + ":", SubTitleFont, colspanClienteTitulo);
                ClienteTituloCell.BackgroundColor = LightBlue;
                pagina.AddCell(ClienteTituloCell);

                var ClienteCell = CreateCell(item.NomeCliente, TextFont, colspanCliente);
                ClienteCell.BackgroundColor = VeryLightGray;
                pagina.AddCell(ClienteCell);
                pagina.AddCell(offsetCellLinha);
                #endregion

                #region linha 2
                var PropostaTituloCell = CreateCell("PRO" + ":", SubTitleFont, colspanPropostaTitulo);
                PropostaTituloCell.BackgroundColor = LightBlue;
                pagina.AddCell(PropostaTituloCell);

                var PropostaCell = CreateCell(item.CodigoProposta, TextFont, colspanProposta);
                PropostaCell.BackgroundColor = VeryLightGray;
                pagina.AddCell(PropostaCell);

                offsetCell.Colspan = 1;
                pagina.AddCell(offsetCell);

                var prePropostaTituloCell = CreateCell("PPR" + ":", SubTitleFont, colspanPrePropostaTitulo);
                prePropostaTituloCell.BackgroundColor = LightBlue;
                pagina.AddCell(prePropostaTituloCell);

                var prePropostaCell = CreateCell(item.CodigoPreProposta, TextFont, colspanPreProposta);
                prePropostaCell.BackgroundColor = VeryLightGray;
                pagina.AddCell(prePropostaCell);

                offsetCell.Colspan = numColunas - (colspanPropostaTitulo + colspanProposta + colspanPreProposta + colspanPrePropostaTitulo);
                pagina.AddCell(offsetCell);


                #endregion

                #region linha 3
                var empreendimentoTituloCell = CreateCell(GlobalMessages.Empreendimento + ":", SubTitleFont, colspanEmpreendimento);
                empreendimentoTituloCell.BackgroundColor = LightBlue;
                pagina.AddCell(empreendimentoTituloCell);

                var empreendimentoCell = CreateCell(item.NomeEmpreendimento, TextFont, numColunas - colspanEmpreendimento);
                empreendimentoCell.BackgroundColor = VeryLightGray;
                pagina.AddCell(empreendimentoCell);

                pagina.AddCell(offsetCellLinha);
                #endregion

                #region linha 4
                offsetCell.Colspan = 1;

                var torreTituloCell = CreateCell(GlobalMessages.Torre + ":", SubTitleFont, colspanTorreTitulo);
                torreTituloCell.BackgroundColor = LightBlue;
                pagina.AddCell(torreTituloCell);

                var torreCell = CreateCell(item.DescricaoTorre, TextFont, colspanTorreConteudo);
                torreCell.BackgroundColor = VeryLightGray;
                pagina.AddCell(torreCell);

                pagina.AddCell(offsetCell);

                var unidadeTituloCell = CreateCell(GlobalMessages.Unidade + ":", SubTitleFont, colspanUnidade);
                unidadeTituloCell.BackgroundColor = LightBlue;
                pagina.AddCell(unidadeTituloCell);

                var unidadeCell = CreateCell(item.DescricaoUnidade, TextFont, colspanUnidade - 1);
                unidadeCell.BackgroundColor = VeryLightGray;
                pagina.AddCell(unidadeCell);

                pagina.AddCell(offsetCell);

                var valorVgvTituloCell = CreateCell(GlobalMessages.VgvSemPremiada + ":", SubTitleFont, colspanVgvSemPremiada);
                valorVgvTituloCell.BackgroundColor = LightBlue;
                pagina.AddCell(valorVgvTituloCell);

                var valorVgvCell = CreateCell(String.Format(new CultureInfo("pt-BR"), "{0:C}", item.ValorVgv), TextFont, colspanVgvSemPremiada - 1);
                valorVgvCell.BackgroundColor = VeryLightGray;
                pagina.AddCell(valorVgvCell);

                pagina.AddCell(offsetCellLinha);
                pagina.AddCell(offsetCellLinha);
                #endregion

                #region linha 5

                offsetCell.Colspan = 1;
                var ComissaoTituloCell = CreateCell(GlobalMessages.Comissao + ":", SubTitleFont, colspanComissaoTiTulo);
                ComissaoTituloCell.BackgroundColor = LightBlue;
                pagina.AddCell(ComissaoTituloCell);

                var ValorAPagar = item.ValorAPagar;
                var ComissaoCell = CreateCell(String.Format(new CultureInfo("pt-BR"), "{0:C}", ValorAPagar), TextFont, colspanComissao);
                ComissaoCell.BackgroundColor = VeryLightGray;
                pagina.AddCell(ComissaoCell);

                pagina.AddCell(offsetCell);

                var porcentagemTituloCell = CreateCell(GlobalMessages.Porcentagem + ":", SubTitleFont, colspanPorcentagemTitulo);
                porcentagemTituloCell.BackgroundColor = LightBlue;
                pagina.AddCell(porcentagemTituloCell);

                var porcentagemCell = CreateCell(Math.Round(item.Percentual, 2) + "%", TextFont, colspanPorcentagemConteudo);
                porcentagemCell.BackgroundColor = VeryLightGray;
                pagina.AddCell(porcentagemCell);

                pagina.AddCell(offsetCell);

                var parcelaTituloCell = CreateCell(GlobalMessages.Parcela + ":", SubTitleFont, colspanParcelaTitulo);
                parcelaTituloCell.BackgroundColor = LightBlue;
                pagina.AddCell(parcelaTituloCell);

                var parcela = item.RegraPagamento + "% " + item.TipoPagamento.AsString();
                var parcelaCell = CreateCell(parcela, TextFont, colspanParcelaConteudo);
                parcelaCell.BackgroundColor = VeryLightGray;
                pagina.AddCell(parcelaCell);

                offsetCell.Colspan = 1;
                pagina.AddCell(offsetCell);
                pagina.AddCell(offsetCellLinha);
                #endregion

                #region Linha 6
                var tabelaTituloCell = CreateCell(GlobalMessages.Tabela + ":", SubTitleFont, colspanTabelaTitulo);
                tabelaTituloCell.BackgroundColor = LightBlue;
                pagina.AddCell(tabelaTituloCell);

                var tabelaCell = CreateCell(item.ModalidadeComissao.AsString(), TextFont, colspanTabelaConteudo);
                tabelaCell.BackgroundColor = VeryLightGray;
                pagina.AddCell(tabelaCell);

                pagina.AddCell(offsetCell);

                if (item.ModalidadeComissao == TipoModalidadeComissao.Nominal)
                {
                    var subTabelaTituloCell = CreateCell(GlobalMessages.SubTabela + ":", SubTitleFont, colspanSubTabelaTitulo);
                    subTabelaTituloCell.BackgroundColor = LightBlue;
                    pagina.AddCell(subTabelaTituloCell);

                    var subTabelaCell = CreateCell(SubTabela(item).AsString(), TextFont, colspanSubTabelaConteudo);
                    subTabelaCell.BackgroundColor = VeryLightGray;
                    pagina.AddCell(subTabelaCell);

                    pagina.AddCell(offsetCell);
                }

                var faixaTituloCell = CreateCell(GlobalMessages.Faixa + ":", SubTitleFont, colspanFaixaTitulo);
                faixaTituloCell.BackgroundColor = LightBlue;
                pagina.AddCell(faixaTituloCell);

                var faixaCell = CreateCell(item.Tipologia.AsString(), TextFont, colspanFaixaConteudo);
                faixaCell.BackgroundColor = VeryLightGray;
                pagina.AddCell(faixaCell);

                pagina.AddCell(offsetCell);

                var CodigoTituloCell = CreateCell("Cód. fornecedor SAP" + ":", SubTitleFont, colspanCodigoTitulo);
                CodigoTituloCell.BackgroundColor = LightBlue;
                pagina.AddCell(CodigoTituloCell);
                var CodigoCell = CreateCell(empreendimento.CodigoEmpresa, TextFont, colspanCodigo);
                CodigoCell.BackgroundColor = VeryLightGray;
                pagina.AddCell(CodigoCell);

                #endregion

                if (item.ModalidadeComissao == TipoModalidadeComissao.Fixa)
                {
                    offsetCell.Colspan = 7;
                    pagina.AddCell(offsetCell);
                }

                pagina.AddCell(offsetCellLinha);

            
            return pagina;
        }

        public TipoSubtabelaComissao SubTabela(ViewNotaFiscalPagamento item)
        {
            var subTabela = TipoSubtabelaComissao.Seca;

            var valorNominal = _valorNominalRepository.Queryable()
                .Where(x => x.Empreendimento.Id == item.IdEmpreendimento)
                .Where(x => x.InicioVigencia.Value.Date <= item.DataVenda.Value.Date)
                .Where(x => x.TerminoVigencia.Value.Date >= item.DataVenda.Value.Date)
                .FirstOrDefault();

            if (valorNominal.IsEmpty())
            {
                valorNominal = _valorNominalRepository.Queryable()
            .Where(x => x.Empreendimento.Id == item.IdEmpreendimento)
            .Where(x => x.InicioVigencia.Value.Date <= item.DataVenda.Value.Date)
            .Where(x => x.TerminoVigencia.Value.Date == null)
            .FirstOrDefault();
            }

            if (valorNominal.IsEmpty())
            {
                return subTabela;
            }

            switch (item.Tipologia)
            {
                case Tipologia.PNE:
                    if (item.ValorVgv < valorNominal.PNEDe)
                    {
                        return TipoSubtabelaComissao.Seca;
                    }
                    else if (item.ValorVgv > valorNominal.PNEAte)
                    {
                        return TipoSubtabelaComissao.Turbinada;
                    }
                    else
                    {
                        return TipoSubtabelaComissao.Normal;
                    }
                    
                case Tipologia.FaixaUmMeio:
                    if (item.ValorVgv < valorNominal.FaixaUmMeioDe)
                    {
                        return TipoSubtabelaComissao.Seca;
                    }
                    else if (item.ValorVgv > valorNominal.FaixaUmMeioAte)
                    {
                        return TipoSubtabelaComissao.Turbinada;
                    }
                    else
                    {
                        return TipoSubtabelaComissao.Normal;
                    }
                    
                case Tipologia.FaixaDois:
                    if (item.ValorVgv < valorNominal.FaixaDoisDe)
                    {
                        return TipoSubtabelaComissao.Seca;
                    }
                    else if (item.ValorVgv > valorNominal.FaixaDoisDe)
                    {
                        return TipoSubtabelaComissao.Turbinada;
                    }
                    else
                    {
                        return TipoSubtabelaComissao.Normal;
                    }
                    
            }
            
            return subTabela;
        }

        private PdfPCell CreateImageCell(byte[] logo)
        {
            Image logoImg = Image.GetInstance(logo);
            PdfPCell cell = new PdfPCell(logoImg, true);
            cell.Colspan = 24;
            cell.PaddingTop = 1;
            cell.PaddingBottom = 1;
            cell.Border = Rectangle.NO_BORDER;
            cell.VerticalAlignment = Element.ALIGN_CENTER;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = Color.WHITE;

            return cell;
        }

        private PdfPCell CreateCell(string content, Font font, int colspan)
        {
            var chunk = new Chunk(content, font);
            var paragraph = new Paragraph(chunk);
            var cell = new PdfPCell(paragraph);
            cell.PaddingTop = 8;
            cell.PaddingBottom = 10;
            cell.PaddingLeft = 7;
            cell.Colspan = colspan;
            cell.Border = Rectangle.TOP_BORDER | Rectangle.BOTTOM_BORDER;
            cell.BorderWidth = 1;
            cell.BorderColor = Color.WHITE;
            cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            cell.BackgroundColor = new Color(239, 239, 239);
            return cell;
        }
        private PdfPCell CreateCellOffset(string content, Font font, int colspan)
        {
            var chunk = new Chunk(content, font);
            var paragraph = new Paragraph(chunk);
            var cell = new PdfPCell(paragraph);
            cell.PaddingTop = 7;
            cell.PaddingBottom = 7;
            cell.Colspan = colspan;
            cell.Border = Rectangle.NO_BORDER;
            cell.VerticalAlignment = PdfPCell.ALIGN_RIGHT;
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
            cell.Border = Rectangle.TOP_BORDER;
            cell.VerticalAlignment = PdfPCell.ALIGN_CENTER;
            cell.BorderColor = MarineBlue;
            cell.BorderWidth = 1;
            cell.PaddingLeft = 5;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            return cell;
        }
        private Font GetItemFont(double value, double activeValue)
        {
            return Math.Abs(value - activeValue) > double.Epsilon ? RedTextFont : TextFont;
        }

    }
}
