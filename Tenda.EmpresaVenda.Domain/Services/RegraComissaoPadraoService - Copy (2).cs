using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Dto;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class RegraComissaoPadraoService : BaseService
    {
        public RegraComissaoPadraoRepository _regraComissaoPadraoRepository { get; set; }
        private ArquivoService _arquivoService { get; set; }
        private EnderecoEmpreendimentoRepository _enderecoEmpreendimentoRepository { get; set; }
        private ItemRegraComissaoPadraoRepository _itemRegraComissaoPadraoRepository { get; set; }
        private EmpreendimentoRepository _empreendimentoRepository { get; set; }
        private ArquivoRepository _arquivoRepository { get; set; }
        private RegraComissaoPadraoPdfService _regraComissaoPadraoPdfService { get; set; }

        public RegraComissaoPadrao Salvar(RegraComissaoPadrao regra, HttpPostedFileBase arquivo,
            BusinessRuleException bre)
        {
            if (regra.Regional.IsEmpty())
            {
                bre.AddError(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Regional))
                    .AddField("Regional").Complete();
            }

            if (regra.Descricao.IsEmpty())
            {
                bre.AddError(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Descricao))
                    .AddField("Descricao").Complete();
            }

            if (arquivo.IsEmpty())
            {
                bre.AddError(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Arquivo)).AddField("Arquivo")
                    .Complete();
            }
            else if (arquivo.ContentType != MimeMappingWrapper.Pdf)
            {
                bre.AddError(string.Format(GlobalMessages.MsgUploadFormatoInvalido, GlobalMessages.PDF))
                    .AddField("Arquivo").Complete();
            }

            bre.ThrowIfHasError();

            regra.Arquivo = _arquivoService.CreateFile(arquivo);
            regra.ContentTypeDoubleCheck = regra.Arquivo.ContentType;
            regra.HashDoubleCheck = regra.Arquivo.Hash;
            regra.IdArquivoDoubleCheck = regra.Arquivo.Id;
            regra.NomeDoubleCheck = regra.Arquivo.Nome;

            _regraComissaoPadraoRepository.Save(regra);

            return regra;
        }

        public RegraComissaoPadrao Liberar(RegraComissaoPadrao regra)
        {
            ValidarRegraComissao(regra);

            FinalizarRegraComissao(regra.Id);
            AtivarRegraComissaoNova(ref regra);

            return regra;
        }

        private void ValidarRegraComissao(RegraComissaoPadrao regra)
        {
            BusinessRuleException bre = new BusinessRuleException();

            if (regra.IsEmpty())
            {
                bre.AddError(GlobalMessages.RegraComissaoNaoEncontrada).Complete();
                bre.ThrowIfHasError();
            }

            if (regra.Situacao != SituacaoRegraComissao.Rascunho)
            {
                bre.AddError(GlobalMessages.MsgLiberarRegraComissaoRascunho).Complete();
                bre.ThrowIfHasError();
            }

            if (regra.Arquivo.IsEmpty())
            {
                bre.AddError(GlobalMessages.ErroLiberarRegraComissao).Complete();
                bre.ThrowIfHasError();
            }

            bre.ThrowIfHasError();
        }

        private void FinalizarRegraComissao(long idRegraComissao)
        {
            var transaction = _session.BeginTransaction();

            var regra = _regraComissaoPadraoRepository.FindById(idRegraComissao);

            var regraComissao = _regraComissaoPadraoRepository.BuscarRegraVigente(regra.Regional);

            if (regraComissao.HasValue())
            {
                regraComissao.Situacao = SituacaoRegraComissao.Vencido;
                regraComissao.TerminoVigencia = DateTime.Now;
                _regraComissaoPadraoRepository.Save(regraComissao);
            }

            transaction.Commit();
        }

        private void AtivarRegraComissaoNova(ref RegraComissaoPadrao regraComissao)
        {
            regraComissao.Situacao = SituacaoRegraComissao.Ativo;
            regraComissao.InicioVigencia = DateTime.Now;
            _regraComissaoPadraoRepository.Save(regraComissao);
        }

        public RegraComissaoPadrao SalvarMatriz(List<ItemRegraComissaoPadrao> itens, RegraComissaoPadrao regraComissao)
        {
            var bre = new BusinessRuleException();

            if (regraComissao.Regional.IsEmpty())
            {
                bre.AddError(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Regional))
                    .AddField("Regional").Complete();
            }

            if (regraComissao.Descricao.IsEmpty())
            {
                bre.AddError(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Descricao))
                    .AddField("Descricao").Complete();
            }

            bre.ThrowIfHasError();

            if (regraComissao.Id > 0)
            {
                var found = _regraComissaoPadraoRepository.FindById(regraComissao.Id);
                _regraComissaoPadraoRepository._session.Evict(found);
                if (found == null)
                {
                    bre.AddError(string.Format(GlobalMessages.RegistroNaoEncontrado, GlobalMessages.RegraComissao,
                            regraComissao.Id))
                        .AddField("Descricao").Complete();
                }
                else
                {
                    found.Descricao = regraComissao.Descricao;
                    regraComissao = found;
                }
            }
            else
            {
                regraComissao.Situacao = SituacaoRegraComissao.Rascunho;
                regraComissao.HashDoubleCheck = "pendente";
                regraComissao.ContentTypeDoubleCheck = "pendente";
                regraComissao.IdArquivoDoubleCheck = 0;
                regraComissao.NomeDoubleCheck = "pendente";
            }

            bre.ThrowIfHasError();

            _regraComissaoPadraoRepository.Save(regraComissao);

            foreach (var item in itens)
            {
                var porcentagemTotal = item.ValorConformidade + item.ValorKitCompleto + item.ValorRepasse;

                var empreendimento = _empreendimentoRepository.FindById(item.Empreendimento.Id);
                if (porcentagemTotal > 0 && Math.Abs(porcentagemTotal - 100) > Double.Epsilon)
                {
                    bre.AddError(GlobalMessages.ItemRegraComissaoPadraoSomaValorInvalido)
                        .WithParams(empreendimento.Nome)
                        .Complete();
                }
                else
                {
                    _itemRegraComissaoPadraoRepository.Save(item);
                }
            }

            bre.ThrowIfHasError();

            return regraComissao;
        }

        public JExcelOptions BuscarMatriz(long idRegraComissao, string regional, bool editable,
            bool novo, bool ultimaAtt, TipoModalidadeComissao modalidade)
        {
            var options = new JExcelOptions();

            var empreendimentos = _enderecoEmpreendimentoRepository
                .EnderecosDaRegional(regional)
                .Select(reg => reg.Empreendimento)
                .Where(x => x.ModalidadeComissao == modalidade)
                .OrderByDescending(x => x.PriorizarRegraComissao)
                .ThenBy(x => x.Nome)
                .ToList();

            var regraComissao = _regraComissaoPadraoRepository.FindById(idRegraComissao);

            if (regraComissao.IsEmpty())
            {
                regraComissao = new RegraComissaoPadrao();
                regraComissao.Regional = regional;
            }
            else
            {
                var itensRegraComissao = _itemRegraComissaoPadraoRepository.BuscarItens(regraComissao.Id, modalidade);

                var empreendimentosItens = itensRegraComissao.Select(x => x.Empreendimento).ToList();

                if (novo)
                {
                    empreendimentosItens = empreendimentosItens.Where(x => empreendimentos.Contains(x)).ToList();
                    empreendimentosItens = empreendimentosItens.Union(empreendimentos).ToList();
                }

                empreendimentos = empreendimentosItens.Distinct()
                    .ToList();
            }

            switch (modalidade)
            {
                case TipoModalidadeComissao.Fixa:
                    options = InsertColumnsMatriz(options, editable);
                    options = InsertDataMatriz(options, empreendimentos, regraComissao, novo, ultimaAtt, modalidade);
                    break;

                case TipoModalidadeComissao.Nominal:
                    options = InsertNominalColumnsMatriz(options, editable);
                    options = InsertNominalDataMatriz(options, empreendimentos, regraComissao, novo, ultimaAtt, modalidade);
                    break;
            }

            return options;
        }

        private JExcelOptions InsertNominalColumnsMatriz(JExcelOptions options, bool editable)
        {
            options.Columns = new List<Column>
            {
                new Column
                {
                    ReadOnly = true, Title = GlobalMessages.Empreendimentos, Type = ColumnType.Text, Width = "165px",
                    WordWrap = true
                },
                new Column {ReadOnly = true, Title = "IdRegraComissao", Type = ColumnType.Hidden, Width = "0px"},
                new Column {ReadOnly = true, Title = "IdEmpreendimento", Type = ColumnType.Hidden, Width = "0px"},
            };

            options.Columns.AddRange(new List<Column>
            {
                new Column
                {
                    ReadOnly = true, Title = "IdItemRegraComissao", Type = ColumnType.Hidden, Width = "0px",
                    AllowEmpty = false
                },
                //faixa 1.5
                new Column
                {
                    Title = GlobalMessages.ColunaMenorValorNominalUmMeio, Type = ColumnType.Numeric,
                    Width = "150px", DecimalCount = 2,
                    ReadOnly = !editable, AllowEmpty = false
                },
                new Column
                {
                    Title = GlobalMessages.ColunaIgualValorNominalUmMeio, Type = ColumnType.Numeric,
                    Width = "150px", DecimalCount = 2,
                    ReadOnly = !editable, AllowEmpty = false
                },
                new Column
                {
                    Title = GlobalMessages.ColunaMaiorValorNominalUmMeio, Type = ColumnType.Numeric,
                    Width = "150px", ReadOnly = !editable, DecimalCount = 2,
                    AllowEmpty = false
                },
                //faixa 2.0
                new Column
                {
                    Title = GlobalMessages.ColunaMenorValorNominalDois, Type = ColumnType.Numeric,
                    Width = "150px", DecimalCount = 2,
                    ReadOnly = !editable, AllowEmpty = false
                },
                new Column
                {
                    Title = GlobalMessages.ColunaIgualValorNominalDois, Type = ColumnType.Numeric,
                    Width = "150px", DecimalCount = 2,
                    ReadOnly = !editable, AllowEmpty = false
                },
                new Column
                {
                    Title = GlobalMessages.ColunaMaiorValorNominalDois, Type = ColumnType.Numeric,
                    Width = "150px", ReadOnly = !editable, DecimalCount = 2,
                    AllowEmpty = false
                },
                //faixa PNE
                new Column
                {
                    Title = GlobalMessages.ColunaMenorValorNominalPNE, Type = ColumnType.Numeric,
                    Width = "150px", DecimalCount = 2,
                    ReadOnly = !editable, AllowEmpty = false
                },
                new Column
                {
                    Title = GlobalMessages.ColunaIgualValorNominalPNE, Type = ColumnType.Numeric,
                    Width = "150px", DecimalCount = 2,
                    ReadOnly = !editable, AllowEmpty = false
                },
                new Column
                {
                    Title = GlobalMessages.ColunaMaiorValorNominalPNE, Type = ColumnType.Numeric,
                    Width = "150px", ReadOnly = !editable, DecimalCount = 2,
                    AllowEmpty = false
                },
                new Column
                {
                    Title = GlobalMessages.ColunaValorKitCompleto, Type = ColumnType.Numeric,
                    Width = "70px", DecimalCount = 2,
                    ReadOnly = !editable, AllowEmpty = false
                },
                new Column
                {
                    Title = GlobalMessages.ColunaValorConformidade, Type = ColumnType.Numeric,
                    Width = "70px", DecimalCount = 2,
                    ReadOnly = !editable, AllowEmpty = false
                },
                new Column
                {
                    Title = GlobalMessages.ColunaValorRepasse, Type = ColumnType.Numeric,
                    Width = "70px", ReadOnly = !editable, DecimalCount = 2,
                    AllowEmpty = false
                }
            });

            return options;
        }
        private JExcelOptions InsertNominalDataMatriz(JExcelOptions options, List<Empreendimento> empreendimentos,
            RegraComissaoPadrao regraComissao, bool novo, bool ultimaAtt, TipoModalidadeComissao modalidade)
        {
            options.Data = new List<List<object>>();

            var regrasComissaoExistentes = _itemRegraComissaoPadraoRepository.ItensDeRegra(regraComissao.Id);

            foreach (var empreendimento in empreendimentos)
            {
                var row = new List<object>
                {
                    empreendimento.Nome,
                    regraComissao.Id,
                    empreendimento.Id
                };

                var itemRegraComissao = BuscarItemRegraComissaoMatriz(regrasComissaoExistentes,
                    empreendimento, ultimaAtt, regraComissao, modalidade);
                row.AddRange(new List<Object>
                {
                    novo ? 0 : itemRegraComissao.Id,
                    itemRegraComissao.MenorValorNominalUmMeio.ToString().Replace(".", ","),
                    itemRegraComissao.IgualValorNominalUmMeio.ToString().Replace(".", ","),
                    itemRegraComissao.MaiorValorNominalUmMeio.ToString().Replace(".", ","),

                    itemRegraComissao.MenorValorNominalDois.ToString().Replace(".", ","),
                    itemRegraComissao.IgualValorNominalDois.ToString().Replace(".", ","),
                    itemRegraComissao.MaiorValorNominalDois.ToString().Replace(".", ","),

                    itemRegraComissao.MenorValorNominalPNE.ToString().Replace(".", ","),
                    itemRegraComissao.IgualValorNominalPNE.ToString().Replace(".", ","),
                    itemRegraComissao.MaiorValorNominalPNE.ToString().Replace(".", ","),

                    itemRegraComissao.ValorKitCompleto.ToString().Replace(".", ","),
                    itemRegraComissao.ValorConformidade.ToString().Replace(".", ","),
                    itemRegraComissao.ValorRepasse.ToString().Replace(".", ",")
                });

                options.Data.Add(row);
            }

            return options;
        }

        private JExcelOptions InsertColumnsMatriz(JExcelOptions options, bool editable)
        {
            options.Columns = new List<Column>
            {
                new Column
                {
                    ReadOnly = true, Title = GlobalMessages.Empreendimentos, Type = ColumnType.Text, Width = "165px",
                    WordWrap = true
                },
                new Column {ReadOnly = true, Title = "IdRegraComissao", Type = ColumnType.Hidden, Width = "0px"},
                new Column {ReadOnly = true, Title = "IdEmpreendimento", Type = ColumnType.Hidden, Width = "0px"},
            };

            options.Columns.AddRange(new List<Column>
            {
                new Column
                {
                    ReadOnly = true, Title = "IdItemRegraComissao", Type = ColumnType.Hidden, Width = "0px",
                    AllowEmpty = false
                },
                new Column
                {
                    Title = GlobalMessages.ColunaFaixaUmMeio, Type = ColumnType.Numeric,
                    Width = "70px", ReadOnly = !editable, DecimalCount = 2,
                    AllowEmpty = false
                },
                new Column
                {
                    Title = GlobalMessages.ColunaFaixaDois, Type = ColumnType.Numeric,
                    Width = "70px", ReadOnly = !editable, DecimalCount = 2,
                    AllowEmpty = false
                },
                new Column
                {
                    Title = GlobalMessages.ColunaValorKitCompleto, Type = ColumnType.Numeric,
                    Width = "70px", DecimalCount = 2,
                    ReadOnly = !editable, AllowEmpty = false
                },
                new Column
                {
                    Title = GlobalMessages.ColunaValorConformidade, Type = ColumnType.Numeric,
                    Width = "70px", DecimalCount = 2,
                    ReadOnly = !editable, AllowEmpty = false
                },
                new Column
                {
                    Title = GlobalMessages.ColunaValorRepasse, Type = ColumnType.Numeric,
                    Width = "70px", ReadOnly = !editable, DecimalCount = 2,
                    AllowEmpty = false
                }
            });

            return options;
        }

        private JExcelOptions InsertDataMatriz(JExcelOptions options, List<Empreendimento> empreendimentos,
            RegraComissaoPadrao regraComissao, bool novo,
            bool ultimaAtt, TipoModalidadeComissao modalidade)
        {
            options.Data = new List<List<object>>();

            var regrasComissaoExistentes = _itemRegraComissaoPadraoRepository.ItensDeRegra(regraComissao.Id);

            foreach (var empreendimento in empreendimentos)
            {
                var row = new List<object>
                {
                    empreendimento.Nome,
                    regraComissao.Id,
                    empreendimento.Id
                };

                var itemRegraComissao = BuscarItemRegraComissaoMatriz(regrasComissaoExistentes,
                    empreendimento, ultimaAtt, regraComissao, modalidade);
                row.AddRange(new List<Object>
                {
                    novo ? 0 : itemRegraComissao.Id,
                    itemRegraComissao.FaixaUmMeio.ToString().Replace(".", ","),
                    itemRegraComissao.FaixaDois.ToString().Replace(".", ","),
                    itemRegraComissao.ValorKitCompleto.ToString().Replace(".", ","),
                    itemRegraComissao.ValorConformidade.ToString().Replace(".", ","),
                    itemRegraComissao.ValorRepasse.ToString().Replace(".", ",")
                });

                options.Data.Add(row);
            }

            return options;
        }

        private ItemRegraComissaoPadrao BuscarItemRegraComissaoMatriz(
            List<ItemRegraComissaoPadrao> regrasComissaoExistentes,
            Empreendimento empreendimento, bool ultimaAtt, RegraComissaoPadrao regraComissaoPadrao,
            TipoModalidadeComissao modalidade)
        {
            ItemRegraComissaoPadrao itemRegraComissao = null;
            if (ultimaAtt)
            {
                var regraVigente = _regraComissaoPadraoRepository.BuscarRegraVigente(regraComissaoPadrao.Regional);
                if (regraVigente.HasValue())
                {
                    itemRegraComissao = _itemRegraComissaoPadraoRepository.Buscar(regraVigente.Id, empreendimento.Id, modalidade);
                    regraComissaoPadrao = regraVigente;
                }
            }

            if (itemRegraComissao.IsEmpty())
            {
                itemRegraComissao = regrasComissaoExistentes
                    .Where(reg => reg.Empreendimento.Id == empreendimento.Id)
                    .SingleOrDefault();
            }

            if (itemRegraComissao == null)
            {
                itemRegraComissao = new ItemRegraComissaoPadrao();
                itemRegraComissao.Empreendimento = empreendimento;
                itemRegraComissao.RegraComissaoPadrao = regraComissaoPadrao;
            }

            return itemRegraComissao;
        }

        public byte[] GerarExcel(long idRegraComissao, string regional, bool ultimaAtt)
        {
            var regraComissao = _regraComissaoPadraoRepository.FindById(idRegraComissao);

            if (regraComissao.IsEmpty())
            {
                regraComissao = new RegraComissaoPadrao();
            }

            var empreendimentos = _enderecoEmpreendimentoRepository
                .EnderecosDaRegional(regional)
                .Select(reg => reg.Empreendimento)
                .OrderByDescending(x => x.PriorizarRegraComissao)
                .ThenBy(x => x.Nome)
                .ToList();

            var regrasComissaoExistentes = _itemRegraComissaoPadraoRepository.ItensDeRegra(idRegraComissao);

            ExcelUtil excel = ExcelUtil.NewInstance(35);

            excel = ConstruirExcel(excel, regrasComissaoExistentes,
                ultimaAtt, TipoModalidadeComissao.Fixa, regraComissao);
            excel = ConstruirExcel(excel, regrasComissaoExistentes,
                ultimaAtt, TipoModalidadeComissao.Nominal, regraComissao);

            excel.Close();
            return excel.DownloadFile();
        }
        public ExcelUtil ConstruirExcel(ExcelUtil excel,
            List<ItemRegraComissaoPadrao> regrasComissaoExistentes, bool ultimaAtt, TipoModalidadeComissao modalidade,
            RegraComissaoPadrao regraComissaoPadrao)
        {
            regrasComissaoExistentes = regrasComissaoExistentes.Where(x => x.Modalidade == modalidade).ToList();

            var empreendimentos = regrasComissaoExistentes.Select(x => x.Empreendimento).Distinct().ToList();

            excel.NewSheet(modalidade.AsString());
            var idx = 0;

            var currentWorksheet = excel.CurrentExcelWorksheet();
            currentWorksheet.Cells[1, 1].Value = GlobalMessages.Empreendimento;
            currentWorksheet.Cells[1, 1].Style.Font.Size = 12;
            currentWorksheet.Cells[1, 1].Style.Font.Bold = true;
            currentWorksheet.Cells[1, 1].AutoFitColumns(26);

            switch (modalidade)
            {
                case TipoModalidadeComissao.Fixa:

                    currentWorksheet.Cells[1, 2].Value = GlobalMessages.ColunaFaixaUmMeio;
                    currentWorksheet.Cells[1, 2].Style.Font.Bold = true;
                    currentWorksheet.Cells[1, 2].Style.Font.Size = 12;

                    currentWorksheet.Cells[1, 3].Value = GlobalMessages.ColunaFaixaDois;
                    currentWorksheet.Cells[1, 3].Style.Font.Bold = true;
                    currentWorksheet.Cells[1, 3].Style.Font.Size = 12;
                    idx = 3;

                    break;
                case TipoModalidadeComissao.Nominal:

                    currentWorksheet.Cells[1, 2].Value = GlobalMessages.ColunaMenorValorNominalUmMeio;
                    currentWorksheet.Cells[1, 2].Style.Font.Bold = true;
                    currentWorksheet.Cells[1, 2].Style.Font.Size = 12;
                    currentWorksheet.Cells[1, 2].AutoFitColumns(30);

                    currentWorksheet.Cells[1, 3].Value = GlobalMessages.ColunaIgualValorNominalUmMeio;
                    currentWorksheet.Cells[1, 3].Style.Font.Bold = true;
                    currentWorksheet.Cells[1, 3].Style.Font.Size = 12;
                    currentWorksheet.Cells[1, 3].AutoFitColumns(30);

                    currentWorksheet.Cells[1, 4].Value = GlobalMessages.ColunaMaiorValorNominalUmMeio;
                    currentWorksheet.Cells[1, 4].Style.Font.Bold = true;
                    currentWorksheet.Cells[1, 4].Style.Font.Size = 12;
                    currentWorksheet.Cells[1, 4].AutoFitColumns(30);

                    currentWorksheet.Cells[1, 5].Value = GlobalMessages.ColunaMenorValorNominalDois;
                    currentWorksheet.Cells[1, 5].Style.Font.Bold = true;
                    currentWorksheet.Cells[1, 5].Style.Font.Size = 12;
                    currentWorksheet.Cells[1, 5].AutoFitColumns(30);

                    currentWorksheet.Cells[1, 6].Value = GlobalMessages.ColunaIgualValorNominalDois;
                    currentWorksheet.Cells[1, 6].Style.Font.Bold = true;
                    currentWorksheet.Cells[1, 6].Style.Font.Size = 12;
                    currentWorksheet.Cells[1, 6].AutoFitColumns(30);

                    currentWorksheet.Cells[1, 7].Value = GlobalMessages.ColunaMaiorValorNominalDois;
                    currentWorksheet.Cells[1, 7].Style.Font.Bold = true;
                    currentWorksheet.Cells[1, 7].Style.Font.Size = 12;
                    currentWorksheet.Cells[1, 7].AutoFitColumns(30);

                    currentWorksheet.Cells[1, 8].Value = GlobalMessages.ColunaMenorValorNominalPNE;
                    currentWorksheet.Cells[1, 8].Style.Font.Bold = true;
                    currentWorksheet.Cells[1, 8].Style.Font.Size = 12;
                    currentWorksheet.Cells[1, 8].AutoFitColumns(30);

                    currentWorksheet.Cells[1, 9].Value = GlobalMessages.ColunaIgualValorNominalPNE;
                    currentWorksheet.Cells[1, 9].Style.Font.Bold = true;
                    currentWorksheet.Cells[1, 9].Style.Font.Size = 12;
                    currentWorksheet.Cells[1, 9].AutoFitColumns(30);

                    currentWorksheet.Cells[1, 10].Value = GlobalMessages.ColunaMaiorValorNominalPNE;
                    currentWorksheet.Cells[1, 10].Style.Font.Bold = true;
                    currentWorksheet.Cells[1, 10].Style.Font.Size = 12;
                    currentWorksheet.Cells[1, 10].AutoFitColumns(30);

                    idx = 10;
                    break;
            }

            currentWorksheet.Cells[1, idx + 1].Value = GlobalMessages.ColunaValorKitCompleto;
            currentWorksheet.Cells[1, idx + 1].Style.Font.Bold = true;
            currentWorksheet.Cells[1, idx + 1].Style.Font.Size = 12;
            currentWorksheet.Cells[1, idx + 2].Value = GlobalMessages.ColunaValorConformidade;
            currentWorksheet.Cells[1, idx + 2].Style.Font.Bold = true;
            currentWorksheet.Cells[1, idx + 2].Style.Font.Size = 12;
            currentWorksheet.Cells[1, idx + 3].Value = GlobalMessages.ColunaValorRepasse;
            currentWorksheet.Cells[1, idx + 3].Style.Font.Bold = true;
            currentWorksheet.Cells[1, idx + 3].Style.Font.Size = 12;

            for (var j = 0; j < empreendimentos.Count; j++)
            {
                var empreendimento = empreendimentos[j];

                currentWorksheet.Cells[2 + j, 1].Value = empreendimento.Nome;
                currentWorksheet.Cells[2 + j, 1].AutoFitColumns(26);
                currentWorksheet.Cells[2 + j, 1].Style.Font.Bold = true;
                currentWorksheet.Cells[2 + j, 1].Style.Font.Size = 12;

                var itemRegraComissao = BuscarItemRegraComissaoMatriz(regrasComissaoExistentes, empreendimento, ultimaAtt, regraComissaoPadrao, modalidade);

                switch (modalidade)
                {
                    case TipoModalidadeComissao.Fixa:
                        currentWorksheet.Cells[2 + j, 2].Value = itemRegraComissao.FaixaUmMeio;
                        currentWorksheet.Cells[2 + j, 3].Value = itemRegraComissao.FaixaDois;
                        break;
                    case TipoModalidadeComissao.Nominal:
                        currentWorksheet.Cells[2 + j, 2].Value = itemRegraComissao.MenorValorNominalUmMeio;
                        currentWorksheet.Cells[2 + j, 3].Value = itemRegraComissao.IgualValorNominalUmMeio;
                        currentWorksheet.Cells[2 + j, 4].Value = itemRegraComissao.MaiorValorNominalUmMeio;

                        currentWorksheet.Cells[2 + j, 5].Value = itemRegraComissao.MenorValorNominalDois;
                        currentWorksheet.Cells[2 + j, 6].Value = itemRegraComissao.IgualValorNominalDois;
                        currentWorksheet.Cells[2 + j, 7].Value = itemRegraComissao.MaiorValorNominalDois;

                        currentWorksheet.Cells[2 + j, 8].Value = itemRegraComissao.MenorValorNominalPNE;
                        currentWorksheet.Cells[2 + j, 9].Value = itemRegraComissao.IgualValorNominalPNE;
                        currentWorksheet.Cells[2 + j, 10].Value = itemRegraComissao.MaiorValorNominalPNE;
                        break;
                }

                currentWorksheet.Cells[2 + j, idx + 1].Value = itemRegraComissao.ValorKitCompleto;
                currentWorksheet.Cells[2 + j, idx + 2].Value = itemRegraComissao.ValorConformidade;
                currentWorksheet.Cells[2 + j, idx + 3].Value = itemRegraComissao.ValorRepasse;

            }

            return excel;
        }

        public RegraComissaoPadrao GerarPdf(long idRegraComissao, byte[] logo)
        {
            var transaction = _session.BeginTransaction();

            var regraComissao = _regraComissaoPadraoRepository.FindById(idRegraComissao);

            Arquivo prevArquivo = null;
            if (regraComissao.Arquivo.HasValue())
            {
                prevArquivo = regraComissao.Arquivo;
            }

            var files = _regraComissaoPadraoPdfService.CriarPdf(regraComissao, logo);
            var file = files[0];

            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            var fileName = $"RegraComissao_{regraComissao.Regional}_{regraComissao.Id}_{date}.pdf";

            var arquivo = _arquivoService.CreateFile(file, fileName, fileName, "application/pdf", file.Length);

            if (regraComissao.IdArquivoDoubleCheck == 0 && regraComissao.Situacao == SituacaoRegraComissao.Ativo)
            {
                regraComissao.ContentTypeDoubleCheck = arquivo.ContentType;
                regraComissao.HashDoubleCheck = arquivo.Hash;
                regraComissao.IdArquivoDoubleCheck = arquivo.Id;
                regraComissao.NomeDoubleCheck = arquivo.Nome;
            }

            regraComissao.Arquivo = arquivo;

            _regraComissaoPadraoRepository.Save(regraComissao);

            if (prevArquivo.HasValue() && regraComissao.IdArquivoDoubleCheck != prevArquivo.Id)
            {
                _arquivoRepository.Delete(prevArquivo);
            }

            transaction.Commit();

            return regraComissao;
        }
    }
}