using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Dto;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class SetupRegraComissaoService:BaseService
    {
        private EnderecoEmpreendimentoRepository _enderecoEmpreendimentoRepository { get; set; }
        private EmpresaVendaRepository _empresaVendaRepository { get; set; }
        private ItemRegraComissaoValidator _itemRegraComissaoValidator { get; set; }
        private RegraComissaoService _regraComissaoService { get; set; }
        private RegraComissaoRepository _regraComissaoRepository { get; set; }
        public JExcelOptions MontarMatriz(SetupRegraComissaoDTO setup)
        {
            var options = new JExcelOptions();
            var itensRegraComissao = new List<ItemRegraComissao>();

            //Lista dos Empreendimentos conforme a modalidade de comissão
            var empreendimentos = _enderecoEmpreendimentoRepository
                .EnderecosDaRegional(setup.Regional)
                .Select(reg => reg.Empreendimento)
                .Where(x => x.ModalidadeComissao == setup.Modalidade)
                .OrderByDescending(x => x.PriorizarRegraComissao)
                .ThenBy(x => x.Nome)
                .ToList();

            Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda = null;

            if (setup.IdEmpresaVenda.HasValue())
            {
                empresaVenda = _empresaVendaRepository.FindById(setup.IdEmpresaVenda);
            }

            options = InsertHeadersMatriz(options,empresaVenda,setup.Modalidade);

            switch (setup.Modalidade)
            {
                case TipoModalidadeComissao.Fixa:
                    options = InsertColumnsMatrizFixa(options, 1, true);
                    options = InsertDataMatrizFixa(options, empreendimentos);
                    break;

                case TipoModalidadeComissao.Nominal:
                    options = InsertColumnsMatrizNominal(options, 1, true);
                    options = InsertDataMatrizNominal(options, empreendimentos);
                    break;
            }

            return options;
        }
        private JExcelOptions InsertHeadersMatriz(JExcelOptions options,
            Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda, TipoModalidadeComissao modalidade)
        {
            options.NestedHeaders = new List<List<Header>>()
            {
                new List<Header>
                {
                    new Header {Colspan = 1, Title = "Matriz", Id = 0}
                }
            };

            var header = new Header { Title = GlobalMessages.EmpresaVenda, Id = 0 };

            if (empresaVenda.HasValue())
            {
                header.Title = empresaVenda.NomeFantasia;
                header.Id = empresaVenda.Id;
            }

            switch (modalidade)
            {
                case TipoModalidadeComissao.Fixa:
                    header.Colspan = 5;
                    //options.NestedHeaders[0].Add(new Header { Colspan = 5, Title = GlobalMessages.EmpresaVenda, Id = 0 });
                    break;

                case TipoModalidadeComissao.Nominal:
                    header.Colspan = 12;
                   // options.NestedHeaders[0].Add(new Header { Colspan = 12, Title = GlobalMessages.EmpresaVenda, Id = 0 });
                    break;
            }

            options.NestedHeaders[0].Add(header);

            if (options.NestedHeaders[0].Count == 1)
            {
                options.NestedHeaders = null;
            }

            return options;
        }
        private JExcelOptions InsertColumnsMatrizFixa(JExcelOptions options, int numEvs, bool editable)
        {
            options.Columns = new List<Column>
            {
                new Column
                {
                    ReadOnly = true, Title = GlobalMessages.Empreendimentos, Type = ColumnType.Text, Width = "165px",
                    WordWrap = true
                },
                new Column {
                    ReadOnly = true, Title = "IdEmpreendimento", Type = ColumnType.Hidden, Width = "0px"
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
            };

            return options;
        }
        private JExcelOptions InsertColumnsMatrizNominal(JExcelOptions options, int numEvs, bool editable)
        {
            options.Columns = new List<Column>
            {
                new Column
                {
                    ReadOnly = true, Title = GlobalMessages.Empreendimentos, Type = ColumnType.Text, Width = "165px",
                    WordWrap = true
                },
                new Column {
                    ReadOnly = true, Title = "IdEmpreendimento", Type = ColumnType.Hidden, Width = "0px"
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
            };

            return options;
        }
        private JExcelOptions InsertDataMatrizFixa(JExcelOptions options, List<Empreendimento> empreendimentos)
        {
            options.Data = new List<List<object>>();

            foreach (var empreendimento in empreendimentos)
            {
                var row = new List<object>
                {
                    empreendimento.Nome,
                    //0,
                    empreendimento.Id
                };

                var itemRegraComissao = new ItemRegraComissao();

                row.AddRange(new List<Object>
                {
                    //0,
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
        private JExcelOptions InsertDataMatrizNominal(JExcelOptions options, List<Empreendimento> empreendimentos)
        {
            options.Data = new List<List<object>>();

            foreach (var empreendimento in empreendimentos)
            {
                var row = new List<object>
                {
                    empreendimento.Nome,
                    //0,
                    empreendimento.Id
                };

                var itemRegraComissao = new ItemRegraComissao();

                row.AddRange(new List<Object>
                    {
                        //0,
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
        private ItemRegraComissao BuscarItemRegraComissaoMatriz(long idRegraComissao, List<ItemRegraComissao> regrasComissaoExistentes,
            List<ItemRegraComissao> itensAtivos, Empreendimento empreendimento,
            Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda, bool ultimaAtt,
            TipoModalidadeComissao modalidade)
        {
            ItemRegraComissao itemRegraComissao = null;
            if (ultimaAtt)
            {
                itemRegraComissao = itensAtivos.Where(x => x.RegraComisao.Id == idRegraComissao)
                    .Where(x => x.EmpresaVenda.Id == empresaVenda.Id)
                    .Where(x => x.Empreendimento.Id == empreendimento.Id)
                    .Where(x => x.TipoModalidadeComissao == modalidade)
                    .SingleOrDefault();
            }
            else
            {
                itemRegraComissao = regrasComissaoExistentes
                    .Where(reg => reg.Empreendimento.Id == empreendimento.Id)
                    .Where(reg => reg.EmpresaVenda.Id == empresaVenda.Id)
                    .Where(reg => reg.TipoModalidadeComissao == modalidade)
                    .SingleOrDefault();
            }

            if (itemRegraComissao == null)
            {
                itemRegraComissao = new ItemRegraComissao();
                itemRegraComissao.Empreendimento = empreendimento;
                itemRegraComissao.EmpresaVenda = empresaVenda;
            }

            return itemRegraComissao;
        }
        public void ValidarItemRegraComissao(List<ItemRegraComissao> itens)
        {
            var bre = new BusinessRuleException();

            foreach(var item in itens)
            {
                var validate = _itemRegraComissaoValidator.Validate(item);
                bre.WithFluentValidation(validate);

                if (item.EmpresaVenda.IsEmpty() || item.EmpresaVenda.Id.IsEmpty())
                {
                    bre.AddError(string.Format("O item referente ao empreendimento {0} não contém Empresa de Venda associada", item.Empreendimento.Nome)).Complete();
                    bre.ThrowIfHasError();
                }
            }

            bre.ThrowIfHasError();
        }

        public RegraComissao GerarRegraComissao(SetupRegraComissaoDTO setup)
        {
            //Salvar Regra de Comissão
            var regraComissao = NormalizarRegraComissao(setup);
            regraComissao = _regraComissaoService.SalvarRegraComissao(regraComissao);
            setup.RegraComissao = regraComissao;

            //Normalizar itens de regra de comissão
            var itens = NormalizarItensRegraComissao(setup);

            ValidarItemRegraComissao(itens);

            var empresas = itens.Select(x => x.EmpresaVenda.Id).Distinct().ToList();

            var result = _regraComissaoService.SalvarMatriz(itens, regraComissao, setup.UsuarioPortal);

            return regraComissao;
        }

        public RegraComissao NormalizarRegraComissao(SetupRegraComissaoDTO setup)
        {
            //var bre = new BusinessRuleException();

            var regraComissao = new RegraComissao();
            regraComissao.Descricao = setup.Descricao;
            regraComissao.Situacao = SituacaoRegraComissao.Rascunho;
            regraComissao.InicioVigencia = setup.InicioVigencia;
            regraComissao.TerminoVigencia = setup.TerminoVigencia;
            regraComissao.Regional = setup.Regional;
            regraComissao.Tipo = setup.TipoRegraComissao;

            regraComissao.HashDoubleCheck = "pendente";
            regraComissao.ContentTypeDoubleCheck = "pendente";
            regraComissao.IdArquivoDoubleCheck = 0;
            regraComissao.NomeDoubleCheck = "pendente";

            if (regraComissao.Tipo == TipoRegraComissao.Campanha)
            {
                TimeSpan timeSpan = new TimeSpan(23, 59, 59);
                var addHoras = regraComissao.TerminoVigencia.Value.Add(timeSpan);
                regraComissao.TerminoVigencia = addHoras;
            }

            //var valido = new RegraComissaoValidator(_regraComissaoRepository).Validate(regraComissao);
            //bre.WithFluentValidation(valido);
            //bre.ThrowIfHasError();

            //_regraComissaoRepository.Save(regraComissao);

            return regraComissao;
        }

        public List<ItemRegraComissao> NormalizarItensRegraComissao(SetupRegraComissaoDTO setup)
        {
            var itens = new List<ItemRegraComissao>();

            if (setup.EvsComuns.HasValue())
            {
                //atribuindo itens à evs comuns
                foreach (var idEv in setup.EvsComuns)
                {
                    foreach (var itemComum in setup.ItensComuns)
                    {
                        var item = new ItemRegraComissao();

                        item.RegraComisao = new RegraComissao { Id = setup.RegraComissao.Id };
                        item.Empreendimento = itemComum.Empreendimento;
                        item.EmpresaVenda = new Tenda.Domain.EmpresaVenda.Models.EmpresaVenda { Id = idEv };

                        item.FaixaUmMeio = itemComum.FaixaUmMeio;
                        item.FaixaDois = itemComum.FaixaDois;
                        item.ValorKitCompleto = itemComum.ValorKitCompleto;
                        item.ValorConformidade = itemComum.ValorConformidade;
                        item.ValorRepasse = itemComum.ValorRepasse;
                        item.TipoModalidadeComissao = itemComum.TipoModalidadeComissao;

                        //faixa 1.5
                        item.MenorValorNominalUmMeio = itemComum.MenorValorNominalUmMeio;
                        item.IgualValorNominalUmMeio = itemComum.IgualValorNominalUmMeio;
                        item.MaiorValorNominalUmMeio = itemComum.MaiorValorNominalUmMeio;

                        //Faixa 2
                        item.MenorValorNominalDois = itemComum.MenorValorNominalDois;
                        item.IgualValorNominalDois = itemComum.IgualValorNominalDois;
                        item.MaiorValorNominalDois = itemComum.MaiorValorNominalDois;

                        //Faixa PNE
                        item.MenorValorNominalPNE = itemComum.MenorValorNominalPNE;
                        item.IgualValorNominalPNE = itemComum.IgualValorNominalPNE;
                        item.MaiorValorNominalPNE = itemComum.MaiorValorNominalPNE;

                        itens.Add(item);
                    }
                }
            }

            if (setup.ItensDiferenciados.HasValue())
            {
                itens.AddRange(setup.ItensDiferenciados);
            }

            return itens;
        }

    }
}
