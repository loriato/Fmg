using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Security.Enums;
using Tenda.EmpresaVenda.Domain.Dto;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Repository.Models;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class PontuacaoFidelidadeService : BaseService
    {
        public PontuacaoFidelidadeRepository _pontuacaoFidelidadeRepository { get; set; }
        public EnderecoEmpreendimentoRepository _enderecoEmpreendimentoRepository{get;set; }
        public ItemPontuacaoFidelidadeRepository _itemPontuacaoFidelidadeRepository { get; set; }
        public ArquivoService _arquivoService { get; set; }
        public ArquivoRepository _arquivoRepository { get; set; }
        public ViewPontuacaoFidelidadeRepository _viewPontuacaoFidelidadeRepository { get; set; }
        public EmpresaVendaRepository _empresaVendaRepository { get; set; }
        public CorretorRepository _corretorRepository { get; set; }
        public NotificacaoRepository _notificacaoRepository { get; set; }
        public JExcelOptions BuscarMatriz(long idPontuacaoFidelidade, string regional, bool editable,
            bool novo, bool ultimaAtt,TipoPontuacaoFidelidade tipoPontuacaoFidelidade,
            TipoCampanhaFidelidade? tipoCampanhaFidelidade,List<long> idEmpreendimentos,
            List<long> idEmpresasVenda,TipoModalidadeProgramaFidelidade modalidade,long progressao,
            List<long>qtdm)
        {
            if (idEmpreendimentos.Contains(0))
            {
                idEmpreendimentos.Remove(0);
            }

            if (idEmpresasVenda.Contains(0))
            {
                idEmpresasVenda.Remove(0);
            }

            var empreendimentos = _enderecoEmpreendimentoRepository
                .EnderecosDaRegional(regional)
                .Select(reg => reg.Empreendimento)
                .Where(reg => reg.ModalidadeProgramaFidelidade == modalidade)
                .OrderByDescending(x => x.Nome)
                .ThenBy(x => x.Nome)
                .ToList();

            if (idEmpreendimentos.HasValue())
            {
                empreendimentos = empreendimentos.Where(x => idEmpreendimentos.Contains(x.Id)).ToList();
            }

            var empresasVenda = _empresaVendaRepository
                .EmpresasDaRegional(regional)
                .Where(x => x.Situacao == Situacao.Ativo)
                .ToList();

            if (idEmpresasVenda.HasValue())
            {
                empresasVenda = empresasVenda.Where(x => idEmpresasVenda.Contains(x.Id)).ToList();
            }

            var options = new JExcelOptions();
            var pontuacoesFidelidadeExistentes = new List<ItemPontuacaoFidelidade>();

            var pontuacaoFidelidade = _pontuacaoFidelidadeRepository.FindById(idPontuacaoFidelidade);

            if (pontuacaoFidelidade.IsEmpty())
            {
                pontuacaoFidelidade = new PontuacaoFidelidade();
                pontuacaoFidelidade.Regional = regional;
                pontuacaoFidelidade.TipoPontuacaoFidelidade = tipoPontuacaoFidelidade;
                pontuacaoFidelidade.TipoCampanhaFidelidade = tipoCampanhaFidelidade;             
            }
            else
            {
                empreendimentos = new List<Empreendimento>();
                empresasVenda = new List<Tenda.Domain.EmpresaVenda.Models.EmpresaVenda>();

                pontuacoesFidelidadeExistentes = _itemPontuacaoFidelidadeRepository.ItensDePontuacaoFidelidade(pontuacaoFidelidade.Id);

                if (pontuacoesFidelidadeExistentes.HasValue())
                {
                    var empreendimentosItens = pontuacoesFidelidadeExistentes
                        .Where(x=>x.Modalidade==modalidade)
                        .Select(x => x.Empreendimento)
                        .ToList();

                    if (novo)
                    {
                        empreendimentosItens = empreendimentosItens.Where(x => empreendimentos.Contains(x)).ToList();
                        empreendimentosItens = empreendimentosItens.Union(empreendimentos).ToList();
                    }

                    empreendimentos = empreendimentosItens.Distinct()
                        .ToList();

                    empresasVenda = pontuacoesFidelidadeExistentes.Select(x => x.EmpresaVenda)
                        .Distinct().ToList();
                }
            }

            empreendimentos = empreendimentos.OrderBy(x => x.Nome).ToList();
             
            options = InsertHeadersMatriz(options, empresasVenda, pontuacaoFidelidade,modalidade);
            options = InsertSubHeadersMatriz(options, empresasVenda, pontuacaoFidelidade,modalidade);
            options = InsertColumnsMatriz(options, editable,pontuacaoFidelidade,empresasVenda.Count,modalidade);
                        
            switch (pontuacaoFidelidade.TipoPontuacaoFidelidade)
            {
                case TipoPontuacaoFidelidade.Normal:
                    options = InsertDataPadrao(options,empreendimentos,empresasVenda,
                        pontuacoesFidelidadeExistentes,pontuacaoFidelidade, ultimaAtt,
                        modalidade,novo);
                    break;
                case TipoPontuacaoFidelidade.Campanha:
                    options = InsertDataCampanha(options, empreendimentos, empresasVenda,
                        pontuacoesFidelidadeExistentes, pontuacaoFidelidade, ultimaAtt,
                        modalidade, novo,progressao,qtdm);
                    break;
            }
            //options = InsertDataMatriz(options, empreendimentos, pontuacaoFidelidade, novo, ultimaAtt,empresasVenda);

            return options;
        }

        private JExcelOptions InsertSubHeadersMatriz(JExcelOptions options,
            List<Tenda.Domain.EmpresaVenda.Models.EmpresaVenda> evs, 
            PontuacaoFidelidade pontuacaoFidelidade, TipoModalidadeProgramaFidelidade modalidade)
        {
            if (modalidade == TipoModalidadeProgramaFidelidade.Fixa)
            {
                return options;
            }

            var subHeader = new List<Header>{
                new Header {Colspan = 1, Title = "", Id = 0}
            };

            foreach (var ev in evs)
            {
                var subHeaderUmMeio = new Header
                {
                    Colspan = 3,
                    Title = GlobalMessages.ColunaPontuacaoFaixaoUmMeio,
                    Id = 0
                };

                var subHeaderDois = new Header
                {
                    Colspan = 3,
                    Title = GlobalMessages.ColunaPontuacaoFaixaDois,
                    Id = 0
                };

                var subHeaderPNE = new Header
                {
                    Colspan = 3,
                    Title = GlobalMessages.ColunaPontuacaoPNE,
                    Id = 0
                };

                var emptyCell = new Header { Colspan = 1, Title = "", Id = 0 };

                switch (pontuacaoFidelidade.TipoPontuacaoFidelidade)
                {
                    case TipoPontuacaoFidelidade.Normal:
                        subHeaderUmMeio.Colspan = 3;
                        subHeaderDois.Colspan = 3;
                        subHeaderPNE.Colspan = 3;
                        emptyCell.Colspan = 2;
                        break;
                    case TipoPontuacaoFidelidade.Campanha:
                        switch (pontuacaoFidelidade.TipoCampanhaFidelidade)
                        {
                            case TipoCampanhaFidelidade.PorVenda:
                                subHeaderUmMeio.Colspan = 6;
                                subHeaderDois.Colspan = 6;
                                subHeaderPNE.Colspan = 6;
                                emptyCell.Colspan = 2;
                                break;
                            case TipoCampanhaFidelidade.PorVendaMinima:
                                subHeader.Add(new Header { Colspan = 1, Title = "", Id = 0 });
                                subHeaderUmMeio.Colspan = 6;
                                subHeaderDois.Colspan = 6;
                                subHeaderPNE.Colspan = 6;
                                emptyCell.Colspan = 2;
                                break;
                            case TipoCampanhaFidelidade.PorVendaMinimaEmpreendimento:
                                subHeader.Add(new Header { Colspan = 1, Title = "", Id = 0 });
                                subHeaderUmMeio.Colspan = 6;
                                subHeaderDois.Colspan = 6;
                                subHeaderPNE.Colspan = 6;
                                emptyCell.Colspan = 2;
                                break;
                        }
                        break;
                }
                subHeader.Add(subHeaderUmMeio);
                subHeader.Add(subHeaderDois);
                subHeader.Add(subHeaderPNE);
                subHeader.Add(emptyCell);
            }

            options.NestedHeaders.Add(subHeader);

            return options;
        }

        private JExcelOptions InsertHeadersMatriz(JExcelOptions options,
            List<Tenda.Domain.EmpresaVenda.Models.EmpresaVenda> evs,
            PontuacaoFidelidade pontuacaoFidelidade, TipoModalidadeProgramaFidelidade modalidade)
        {
            options.NestedHeaders = new List<List<Header>>()
            {
                new List<Header>
                {
                    new Header {Colspan = 1,Rowspan = 2, Title = "Matriz", Id = 0}
                }                
            };

            foreach (var ev in evs)
            {
                var header = new Header { Colspan = 4, Title = ev.NomeFantasia, Id = ev.Id };

                switch (pontuacaoFidelidade.TipoPontuacaoFidelidade)
                {
                    case TipoPontuacaoFidelidade.Normal:
                        if (modalidade == TipoModalidadeProgramaFidelidade.Nominal)
                        {
                            header.Colspan = 11;
                        }
                        break;
                    case TipoPontuacaoFidelidade.Campanha:
                        switch (pontuacaoFidelidade.TipoCampanhaFidelidade)
                        {
                            case TipoCampanhaFidelidade.PorVenda:
                                header.Colspan = 6;
                                if (modalidade == TipoModalidadeProgramaFidelidade.Nominal)
                                {
                                    header.Colspan = 20;
                                }
                                break;
                            case TipoCampanhaFidelidade.PorVendaMinima:
                                header.Colspan = 7;
                                if (modalidade == TipoModalidadeProgramaFidelidade.Nominal)
                                {
                                    header.Colspan = 21;
                                }
                                break;
                            case TipoCampanhaFidelidade.PorVendaMinimaEmpreendimento:
                                header.Colspan = 7;
                                if (modalidade == TipoModalidadeProgramaFidelidade.Nominal)
                                {
                                    header.Colspan = 21;
                                }
                                break;
                        }
                        break;
                }

                options.NestedHeaders[0].Add(header);

            }

            if (options.NestedHeaders[0].Count == 1)
            {
                options.NestedHeaders = null;
            }

            return options;
        }

        private JExcelOptions InsertColumnsMatriz(JExcelOptions options, bool editable,
            PontuacaoFidelidade pontuacaoFidelidade,long numEvs, TipoModalidadeProgramaFidelidade modalidade)
        {
            editable = true;

            if (pontuacaoFidelidade.Situacao == SituacaoPontuacaoFidelidade.Rascunho)
            {
                editable = false;
            }

            options.Columns = new List<Column>
            {
                new Column
                {
                    ReadOnly = true, Title = GlobalMessages.Empreendimentos, Type = ColumnType.Text, Width = "165px",
                    WordWrap = true
                },
                new Column {ReadOnly = true, Title = "IdPontuacaoFidelidade", Type = ColumnType.Hidden, Width = "0px"},
                new Column {ReadOnly = true, Title = "IdEmpreendimento", Type = ColumnType.Hidden, Width = "0px"},
            };
            for (var i = 0; i < numEvs; i++)
            {
                options.Columns.Add(new Column
                {
                    ReadOnly = true,
                    Title = "IdEmpresaVenda",
                    Type = ColumnType.Hidden,
                    Width = "0px",
                    AllowEmpty = false
                });

                options.Columns.Add(new Column
                {
                    ReadOnly = true,
                    Title = "IdItemPontuacaoFidelidade",
                    Type = ColumnType.Hidden,
                    Width = "0px",
                    AllowEmpty = false
                });

                switch (modalidade)
                {
                    case TipoModalidadeProgramaFidelidade.Fixa:
                        options = InserColumnFixa(options,editable, pontuacaoFidelidade);
                        break;
                    case TipoModalidadeProgramaFidelidade.Nominal:
                        options = InserColumnNominal(options, editable, pontuacaoFidelidade);
                        break;
                }

                options.Columns.Add(new Column
                {
                    Title = GlobalMessages.Situacao,
                    Type = ColumnType.String,
                    Width = "90px",
                    DecimalCount = 2,
                    ReadOnly = true,
                    AllowEmpty = false
                });

                options.Columns.Add(new Column
                {
                    Title = GlobalMessages.Codigo,
                    Type = ColumnType.String,
                    Width = "90px",
                    DecimalCount = 2,
                    ReadOnly = true,
                    AllowEmpty = false
                });
            }
            return options;
        }
        /*
         * Colunas da modalidade Nominal
         */
        private JExcelOptions InserColumnNominal(JExcelOptions options, bool editable,
            PontuacaoFidelidade pontuacaoFidelidade)
        {
            var columns = new List<Column>();

            if (pontuacaoFidelidade.TipoPontuacaoFidelidade == TipoPontuacaoFidelidade.Campanha &&
                pontuacaoFidelidade.TipoCampanhaFidelidade != TipoCampanhaFidelidade.PorVenda)
            {
                columns.Add(new Column
                {
                    Title = "Qtd. Mínima",
                    Type = ColumnType.Numeric,
                    Width = "120px",
                    ReadOnly = pontuacaoFidelidade.TipoCampanhaFidelidade != TipoCampanhaFidelidade.PorVendaMinimaEmpreendimento,
                    DecimalCount = 2,
                    AllowEmpty = false
                });
            }

            for (var i = 0; i < 3; i++)
            {
                switch (pontuacaoFidelidade.TipoPontuacaoFidelidade)
                {
                    case TipoPontuacaoFidelidade.Normal:
                        columns.Add(new Column
                        {
                            Title = "Pontuação Seca",
                            Type = ColumnType.Numeric,
                            Width = "100px",
                            ReadOnly = editable,
                            DecimalCount = 2,
                            AllowEmpty = false
                        });

                        columns.Add(new Column
                        {
                            Title = "Pontuação Normal",
                            Type = ColumnType.Numeric,
                            Width = "120px",
                            ReadOnly = editable,
                            DecimalCount = 2,
                            AllowEmpty = false
                        });

                        columns.Add(new Column
                        {
                            Title = "Pontuação Turbinada",
                            Type = ColumnType.Numeric,
                            Width = "140px",
                            ReadOnly = editable,
                            DecimalCount = 2,
                            AllowEmpty = false
                        });
                        break;
                    case TipoPontuacaoFidelidade.Campanha:
                        
                        columns.Add(new Column
                        {
                            Title = "Fator Seca",
                            Type = ColumnType.Numeric,
                            Width = "100px",
                            ReadOnly = editable,
                            DecimalCount = 2,
                            AllowEmpty = false
                        });

                        columns.Add(new Column
                        {
                            Title = "Pontuação Padrão Seca",
                            Type = ColumnType.Numeric,
                            Width = "180px",
                            ReadOnly = true,
                            DecimalCount = 2,
                            AllowEmpty = false
                        });

                        columns.Add(new Column
                        {
                            Title = "Fator Normal",
                            Type = ColumnType.Numeric,
                            Width = "100px",
                            ReadOnly = editable,
                            DecimalCount = 2,
                            AllowEmpty = false
                        });

                        columns.Add(new Column
                        {
                            Title = "Pontuação Padrão Normal",
                            Type = ColumnType.Numeric,
                            Width = "180px",
                            ReadOnly = true,
                            DecimalCount = 2,
                            AllowEmpty = false
                        });

                        columns.Add(new Column
                        {
                            Title = "Fator Turbinada",
                            Type = ColumnType.Numeric,
                            Width = "100px",
                            ReadOnly = editable,
                            DecimalCount = 2,
                            AllowEmpty = false
                        });

                        columns.Add(new Column
                        {
                            Title = "Pontuação Padrão Turbinada",
                            Type = ColumnType.Numeric,
                            Width = "180px",
                            ReadOnly = true,
                            DecimalCount = 2,
                            AllowEmpty = false
                        });

                        break;
                }
            }

            options.Columns.AddRange(columns);

            return options;
        }
        /*
         * Colunas da modalidade Fixa
         */
        private JExcelOptions InserColumnFixa(JExcelOptions options,bool editable,
        PontuacaoFidelidade pontuacaoFidelidade)
        {
            var columns = new List<Column>();
            switch (pontuacaoFidelidade.TipoPontuacaoFidelidade)
            {
                case TipoPontuacaoFidelidade.Normal:
                    columns.Add(new Column
                    {
                        Title = GlobalMessages.ColunaPontuacaoFaixaoUmMeio,
                        Type = ColumnType.Numeric,
                        Width = "100px",
                        ReadOnly = editable,
                        DecimalCount = 2,
                        AllowEmpty = false
                    });

                    columns.Add(new Column
                    {
                        Title = GlobalMessages.ColunaPontuacaoFaixaDois,
                        Type = ColumnType.Numeric,
                        Width = "100px",
                        ReadOnly = editable,
                        DecimalCount = 2,
                        AllowEmpty = false
                    });

                    break;
                case TipoPontuacaoFidelidade.Campanha:

                    columns.Add(new Column
                    {
                        Title = "Fator F 1.5",
                        Type = ColumnType.Numeric,
                        Width = "100px",
                        ReadOnly = editable,
                        DecimalCount = 2,
                        AllowEmpty = false
                    });

                    columns.Add(new Column
                    {
                        Title = "Pontuação Padrão F 1.5",
                        Type = ColumnType.Numeric,
                        Width = "150px",
                        ReadOnly = true,
                        DecimalCount = 2,
                        AllowEmpty = false
                    });

                    columns.Add(new Column
                    {
                        Title = "Fator F 2.0",
                        Type = ColumnType.Numeric,
                        Width = "100px",
                        ReadOnly = editable,
                        DecimalCount = 2,
                        AllowEmpty = false
                    });

                    columns.Add(new Column
                    {
                        Title = "Pontuação Padrão F 2.0",
                        Type = ColumnType.Numeric,
                        Width = "150px",
                        ReadOnly = true,
                        DecimalCount = 2,
                        AllowEmpty = false
                    });

                    if (pontuacaoFidelidade.TipoCampanhaFidelidade == TipoCampanhaFidelidade.PorVendaMinima)
                    {
                        columns.Add(new Column
                        {
                            Title = "Qtd. Mínima",
                            Type = ColumnType.Numeric,
                            Width = "120px",
                            ReadOnly = true,
                            DecimalCount = 2,
                            AllowEmpty = false
                        });
                    }

                    if (pontuacaoFidelidade.TipoCampanhaFidelidade == TipoCampanhaFidelidade.PorVendaMinimaEmpreendimento)
                    {
                        columns.Add(new Column
                        {
                            Title = "Qtd. Mínima Empreendimento",
                            Type = ColumnType.Numeric,
                            Width = "200px",
                            ReadOnly = editable,
                            DecimalCount = 2,
                            AllowEmpty = false
                        });
                    }

                    break;
            }

            options.Columns.AddRange(columns);
            
            return options;
        }

        private JExcelOptions InsertDataCampanha(JExcelOptions options, List<Empreendimento> empreendimentos,
            List<Tenda.Domain.EmpresaVenda.Models.EmpresaVenda> empresasVendas,
            List<ItemPontuacaoFidelidade> pontuacoesFidelidadeExistentes, PontuacaoFidelidade pontuacaoFidelidade,
            bool ultimaAtt, TipoModalidadeProgramaFidelidade modalidade, bool novo,long progressao, List<long>qtdm)
        {
            var idEvs = empresasVendas.Select(x => x.Id).ToList();
            options.Data = new List<List<object>>();

            //itens normais ativos dos empreendimentos
            var situacoes = new List<SituacaoPontuacaoFidelidade> { SituacaoPontuacaoFidelidade.Ativo, SituacaoPontuacaoFidelidade.Suspenso };
            var itensNormaisAtivos = _itemPontuacaoFidelidadeRepository.ItensNormaisAtivosESuspensos(empreendimentos, idEvs, pontuacaoFidelidade.TipoPontuacaoFidelidade);

            foreach (var empreendimento in empreendimentos)
            {
                for (var i = 0; i < progressao; i++)
                {
                    var row = new List<object>();

                    if (i == 0)
                    {
                        row.Add(empreendimento.Nome);
                    }
                    else
                    {
                        row.Add("");
                    }

                    row.Add(pontuacaoFidelidade.Id);
                    row.Add(empreendimento.Id);

                    foreach (var empresaVenda in empresasVendas)
                    {
                        row.Add(empresaVenda.Id);

                        var itemPontuacaoFidelidade = BuscarItemPontuacaoFidelidadeMatriz(pontuacoesFidelidadeExistentes,
                        empreendimento, ultimaAtt, pontuacaoFidelidade, empresaVenda, modalidade,qtdm==null?0:qtdm[i]);

                        var celulas = new List<Object>();
                        celulas.Add(novo ? 0 : itemPontuacaoFidelidade.Id);

                        switch (modalidade)
                        {
                            case TipoModalidadeProgramaFidelidade.Fixa:
                                //F1.5
                                celulas.Add(Math.Round(itemPontuacaoFidelidade.FatorUmMeio, 2).ToString().Replace(".", ","));
                                celulas.Add(Math.Round(itemPontuacaoFidelidade.PontuacaoPadraoUmMeio, 2).ToString().Replace(".", ","));

                                //F2.0
                                celulas.Add(Math.Round(itemPontuacaoFidelidade.FatorDois, 2).ToString().Replace(".", ","));
                                celulas.Add(Math.Round(itemPontuacaoFidelidade.PontuacaoPadraoDois, 2).ToString().Replace(".", ","));

                                if (pontuacaoFidelidade.TipoCampanhaFidelidade != TipoCampanhaFidelidade.PorVenda)
                                {
                                    celulas.Add(itemPontuacaoFidelidade.QuantidadeMinima.ToString().Replace(".", ","));
                                }

                                break;
                            case TipoModalidadeProgramaFidelidade.Nominal:
                                if (pontuacaoFidelidade.TipoCampanhaFidelidade != TipoCampanhaFidelidade.PorVenda)
                                {
                                    celulas.Add(itemPontuacaoFidelidade.QuantidadeMinima.ToString().Replace(".", ","));
                                }
                                //F1.5
                                celulas.Add(Math.Round(itemPontuacaoFidelidade.FatorUmMeioSeca, 2).ToString().Replace(".", ","));
                                celulas.Add(Math.Round(itemPontuacaoFidelidade.PontuacaoFaixaUmMeioSeca, 2).ToString().Replace(".", ","));

                                celulas.Add(Math.Round(itemPontuacaoFidelidade.FatorUmMeioNormal, 2).ToString().Replace(".", ","));
                                celulas.Add(Math.Round(itemPontuacaoFidelidade.PontuacaoFaixaUmMeioNormal, 2).ToString().Replace(".", ","));

                                celulas.Add(Math.Round(itemPontuacaoFidelidade.FatorUmMeioTurbinada, 2).ToString().Replace(".", ","));
                                celulas.Add(Math.Round(itemPontuacaoFidelidade.PontuacaoFaixaUmMeioTurbinada, 2).ToString().Replace(".", ","));

                                //F2.0
                                celulas.Add(Math.Round(itemPontuacaoFidelidade.FatorDoisSeca, 2).ToString().Replace(".", ","));
                                celulas.Add(Math.Round(itemPontuacaoFidelidade.PontuacaoFaixaDoisSeca, 2).ToString().Replace(".", ","));

                                celulas.Add(Math.Round(itemPontuacaoFidelidade.FatorDoisNormal, 2).ToString().Replace(".", ","));
                                celulas.Add(Math.Round(itemPontuacaoFidelidade.PontuacaoFaixaDoisNormal, 2).ToString().Replace(".", ","));

                                celulas.Add(Math.Round(itemPontuacaoFidelidade.FatorDoisTurbinada, 2).ToString().Replace(".", ","));
                                celulas.Add(Math.Round(itemPontuacaoFidelidade.PontuacaoFaixaDoisTurbinada, 2).ToString().Replace(".", ","));

                                //PNE
                                celulas.Add(Math.Round(itemPontuacaoFidelidade.FatorPNESeca, 2).ToString().Replace(".", ","));
                                celulas.Add(Math.Round(itemPontuacaoFidelidade.PontuacaoPNESeca, 2).ToString().Replace(".", ","));

                                celulas.Add(Math.Round(itemPontuacaoFidelidade.FatorPNENormal, 2).ToString().Replace(".", ","));
                                celulas.Add(Math.Round(itemPontuacaoFidelidade.PontuacaoPNENormal, 2).ToString().Replace(".", ","));

                                celulas.Add(Math.Round(itemPontuacaoFidelidade.FatorPNETurbinada, 2).ToString().Replace(".", ","));
                                celulas.Add(Math.Round(itemPontuacaoFidelidade.PontuacaoPNETurbinada, 2).ToString().Replace(".", ","));

                                break;
                        }
                        
                        celulas.Add(itemPontuacaoFidelidade.Situacao.AsString());
                        celulas.Add(itemPontuacaoFidelidade.Codigo);

                        row.AddRange(celulas);
                    }

                    options.Data.Add(row);
                }
            }
            return options;
        }

        private JExcelOptions InsertDataPadrao(JExcelOptions options,List<Empreendimento> empreendimentos,
            List<Tenda.Domain.EmpresaVenda.Models.EmpresaVenda> empresasVendas,
            List<ItemPontuacaoFidelidade> pontuacoesFidelidadeExistentes,PontuacaoFidelidade pontuacaoFidelidade,
            bool ultimaAtt,TipoModalidadeProgramaFidelidade modalidade, bool novo)
        {
            var idEvs = empresasVendas.Select(x => x.Id).ToList();
            options.Data = new List<List<object>>();

            //itens normais ativos dos empreendimentos
            var situacoes = new List<SituacaoPontuacaoFidelidade> { SituacaoPontuacaoFidelidade.Ativo, SituacaoPontuacaoFidelidade.Suspenso };
            var itensNormaisAtivos = _itemPontuacaoFidelidadeRepository.ItensNormaisAtivosESuspensos(empreendimentos, idEvs,pontuacaoFidelidade.TipoPontuacaoFidelidade);

            foreach (var empreendimento in empreendimentos)
            {
                var row = new List<object>
                {
                    empreendimento.Nome,
                    pontuacaoFidelidade.Id,
                    empreendimento.Id
                };

                foreach (var empresaVenda in empresasVendas)
                {
                    row.Add(empresaVenda.Id);

                    var itemPontuacaoFidelidade = BuscarItemPontuacaoFidelidadeMatriz(pontuacoesFidelidadeExistentes,
                    empreendimento, ultimaAtt, pontuacaoFidelidade, empresaVenda,modalidade,0);

                    var celulas = new List<Object>();
                    celulas.Add(novo ? 0 : itemPontuacaoFidelidade.Id);

                    switch (modalidade)
                    {
                        case TipoModalidadeProgramaFidelidade.Fixa:
                            celulas.Add(Math.Round(itemPontuacaoFidelidade.PontuacaoFaixaUmMeio, 2).ToString().Replace(".", ","));
                            celulas.Add(Math.Round(itemPontuacaoFidelidade.PontuacaoFaixaDois,2).ToString().Replace(".", ","));
                            break;
                        case TipoModalidadeProgramaFidelidade.Nominal:
                            celulas.Add(Math.Round(itemPontuacaoFidelidade.PontuacaoFaixaUmMeioSeca, 2).ToString().Replace(".", ","));
                            celulas.Add(Math.Round(itemPontuacaoFidelidade.PontuacaoFaixaUmMeioNormal, 2).ToString().Replace(".", ","));
                            celulas.Add(Math.Round(itemPontuacaoFidelidade.PontuacaoFaixaUmMeioTurbinada, 2).ToString().Replace(".", ","));

                            celulas.Add(Math.Round(itemPontuacaoFidelidade.PontuacaoFaixaDoisSeca, 2).ToString().Replace(".", ","));
                            celulas.Add(Math.Round(itemPontuacaoFidelidade.PontuacaoFaixaDoisNormal, 2).ToString().Replace(".", ","));
                            celulas.Add(Math.Round(itemPontuacaoFidelidade.PontuacaoFaixaDoisTurbinada, 2).ToString().Replace(".", ","));

                            celulas.Add(Math.Round(itemPontuacaoFidelidade.PontuacaoPNESeca, 2).ToString().Replace(".", ","));
                            celulas.Add(Math.Round(itemPontuacaoFidelidade.PontuacaoPNENormal, 2).ToString().Replace(".", ","));
                            celulas.Add(Math.Round(itemPontuacaoFidelidade.PontuacaoPNETurbinada, 2).ToString().Replace(".", ","));

                            break;
                    }

                    celulas.Add(itemPontuacaoFidelidade.Situacao.AsString());
                    celulas.Add(itemPontuacaoFidelidade.Codigo);

                    row.AddRange(celulas);
                }

                options.Data.Add(row);
            }
            return options;
        }

        private ItemPontuacaoFidelidade BuscarItemPontuacaoFidelidadeMatriz(
            List<ItemPontuacaoFidelidade> pontuacaosComissaoExistentes,
            Empreendimento empreendimento, bool ultimaAtt, PontuacaoFidelidade pontuacaoFidelidade, 
            Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda, TipoModalidadeProgramaFidelidade modalidade,
            long qtdm)
        {
            var itemPontuacaoFidelidade = pontuacaosComissaoExistentes
                    .Where(reg => reg.Empreendimento.Id == empreendimento.Id)
                    .Where(reg=>reg.EmpresaVenda.Id==empresaVenda.Id)
                    .Where(reg=>reg.Modalidade == modalidade)
                    .FirstOrDefault();           
            
            if (itemPontuacaoFidelidade == null)
            {
                itemPontuacaoFidelidade = new ItemPontuacaoFidelidade();
                itemPontuacaoFidelidade.Empreendimento = empreendimento;
                itemPontuacaoFidelidade.PontuacaoFidelidade = pontuacaoFidelidade;
                itemPontuacaoFidelidade.Situacao = SituacaoPontuacaoFidelidade.Rascunho;
                itemPontuacaoFidelidade.Modalidade = modalidade;

                var situacao = new List<SituacaoPontuacaoFidelidade> { SituacaoPontuacaoFidelidade.Ativo, SituacaoPontuacaoFidelidade.Suspenso };

                var itemPadrao = _itemPontuacaoFidelidadeRepository.Queryable()
                    .Where(x => x.PontuacaoFidelidade.TipoPontuacaoFidelidade == TipoPontuacaoFidelidade.Normal)
                .Where(x =>situacao.Contains(x.Situacao))
                .Where(x => x.Empreendimento.Id == empreendimento.Id)
                .Where(x => x.EmpresaVenda.Id == empresaVenda.Id)
                .OrderByDescending(x=>x.Situacao)
                .FirstOrDefault();

                if (itemPadrao.HasValue() && pontuacaoFidelidade.TipoPontuacaoFidelidade == TipoPontuacaoFidelidade.Campanha)
                {
                    itemPontuacaoFidelidade.PontuacaoPadraoUmMeio = itemPadrao.PontuacaoFaixaUmMeio;
                    itemPontuacaoFidelidade.PontuacaoPadraoDois = itemPadrao.PontuacaoFaixaDois;

                    itemPontuacaoFidelidade.PontuacaoFaixaUmMeioSeca = itemPadrao.PontuacaoFaixaUmMeioSeca;
                    itemPontuacaoFidelidade.PontuacaoFaixaUmMeioNormal = itemPadrao.PontuacaoFaixaUmMeioNormal;
                    itemPontuacaoFidelidade.PontuacaoFaixaUmMeioTurbinada = itemPadrao.PontuacaoFaixaUmMeioTurbinada;

                    itemPontuacaoFidelidade.PontuacaoFaixaDoisSeca = itemPadrao.PontuacaoFaixaDoisSeca;
                    itemPontuacaoFidelidade.PontuacaoFaixaDoisNormal = itemPadrao.PontuacaoFaixaDoisNormal;
                    itemPontuacaoFidelidade.PontuacaoFaixaDoisTurbinada = itemPadrao.PontuacaoFaixaDoisTurbinada;

                    itemPontuacaoFidelidade.PontuacaoPNESeca = itemPadrao.PontuacaoPNESeca;
                    itemPontuacaoFidelidade.PontuacaoPNENormal = itemPadrao.PontuacaoPNENormal;
                    itemPontuacaoFidelidade.PontuacaoPNETurbinada = itemPadrao.PontuacaoPNETurbinada;
                }
            }
            else
            {
                pontuacaosComissaoExistentes.Remove(itemPontuacaoFidelidade);
            }
                        
            if (ultimaAtt)
            {
                var item = _itemPontuacaoFidelidadeRepository.BuscarItemAtivoPorEmpreendimentoEmpresaVenda(empreendimento.Id, empresaVenda.Id, modalidade);

                if (item.HasValue())
                {
                    itemPontuacaoFidelidade.PontuacaoFaixaUmMeio = item.PontuacaoFaixaUmMeio;
                    itemPontuacaoFidelidade.PontuacaoFaixaDois = item.PontuacaoFaixaDois;

                    itemPontuacaoFidelidade.PontuacaoFaixaUmMeioSeca = item.PontuacaoFaixaUmMeioSeca;
                    itemPontuacaoFidelidade.PontuacaoFaixaUmMeioNormal = item.PontuacaoFaixaUmMeioNormal;
                    itemPontuacaoFidelidade.PontuacaoFaixaUmMeioTurbinada = item.PontuacaoFaixaUmMeioTurbinada;

                    itemPontuacaoFidelidade.PontuacaoPNESeca = item.PontuacaoPNESeca;
                    itemPontuacaoFidelidade.PontuacaoPNENormal = item.PontuacaoPNENormal;
                    itemPontuacaoFidelidade.PontuacaoPNETurbinada = item.PontuacaoPNETurbinada;

                    itemPontuacaoFidelidade.PontuacaoFaixaDoisSeca = item.PontuacaoFaixaDoisSeca;
                    itemPontuacaoFidelidade.PontuacaoFaixaDoisNormal = item.PontuacaoFaixaDoisNormal;
                    itemPontuacaoFidelidade.PontuacaoFaixaDoisTurbinada = item.PontuacaoFaixaDoisTurbinada;

                    itemPontuacaoFidelidade.FatorUmMeio = item.FatorUmMeio;
                    itemPontuacaoFidelidade.FatorDois = item.FatorDois;

                    itemPontuacaoFidelidade.FatorUmMeioSeca = item.FatorUmMeioSeca;
                    itemPontuacaoFidelidade.FatorUmMeioNormal = item.FatorUmMeioNormal;
                    itemPontuacaoFidelidade.FatorUmMeioTurbinada = item.FatorUmMeioTurbinada;

                    itemPontuacaoFidelidade.FatorDoisSeca = item.FatorDoisSeca;
                    itemPontuacaoFidelidade.FatorDoisNormal = item.FatorDoisNormal;
                    itemPontuacaoFidelidade.FatorDoisTurbinada = item.FatorDoisTurbinada;

                    itemPontuacaoFidelidade.FatorPNESeca = item.FatorPNESeca;
                    itemPontuacaoFidelidade.FatorPNENormal = item.FatorPNENormal;
                    itemPontuacaoFidelidade.FatorPNETurbinada = item.FatorPNETurbinada;

                    itemPontuacaoFidelidade.QuantidadeMinima = item.QuantidadeMinima;
                    itemPontuacaoFidelidade.Modalidade = item.Modalidade;
                }
            }

            if (pontuacaoFidelidade.TipoCampanhaFidelidade == TipoCampanhaFidelidade.PorVendaMinima)
            {
                itemPontuacaoFidelidade.QuantidadeMinima = qtdm;
            }

            return itemPontuacaoFidelidade;
        }

        public PontuacaoFidelidade SalvarMatriz(List<ItemPontuacaoFidelidade> itens, PontuacaoFidelidade pontuacaoFidelidade)
        {
            var bre = new BusinessRuleException();
                        
            var validation = new PontuacaoFidelidadeValidator(_pontuacaoFidelidadeRepository).Validate(pontuacaoFidelidade);
            bre.WithFluentValidation(validation);
            bre.ThrowIfHasError();

            if (pontuacaoFidelidade.Id > 0)
            {
                var found = _pontuacaoFidelidadeRepository.FindById(pontuacaoFidelidade.Id);
                _pontuacaoFidelidadeRepository._session.Evict(found);
                if (found == null)
                {
                    bre.AddError(string.Format(GlobalMessages.RegistroNaoEncontrado, GlobalMessages.PontuacaoFidelidade,
                            pontuacaoFidelidade.Id))
                        .AddField("Descricao").Complete();
                }
                else
                {
                    found.Descricao = pontuacaoFidelidade.Descricao;
                    found.InicioVigencia = pontuacaoFidelidade.InicioVigencia;
                    found.TerminoVigencia = pontuacaoFidelidade.TerminoVigencia;
                    found.QuantidadeMinima = pontuacaoFidelidade.QuantidadeMinima;
                    pontuacaoFidelidade = found;
                }
            }
            else
            {
                pontuacaoFidelidade.Situacao = SituacaoPontuacaoFidelidade.Rascunho;
                pontuacaoFidelidade.HashDoubleCheck = "pendente";
                pontuacaoFidelidade.ContentTypeDoubleCheck = "pendente";
                pontuacaoFidelidade.IdArquivoDoubleCheck = 0;
                pontuacaoFidelidade.NomeArquivoDoubleCheck = "pendente";
            }

            bre.ThrowIfHasError();

            _pontuacaoFidelidadeRepository.Save(pontuacaoFidelidade);

            foreach (var item in itens)
            {
                item.PontuacaoFidelidade = pontuacaoFidelidade;
                item.EmpresaVenda = new Tenda.Domain.EmpresaVenda.Models.EmpresaVenda { Id = item.EmpresaVenda.Id ,NomeFantasia=item.EmpresaVenda.NomeFantasia};

                var itemValidation = new ItemPontuacaoFidelidadeValidator().Validate(item);
                bre.WithFluentValidation(itemValidation);
                bre.ThrowIfHasError();
                
                if (pontuacaoFidelidade.TipoPontuacaoFidelidade == TipoPontuacaoFidelidade.Campanha)
                {
                    item.InicioVigencia = pontuacaoFidelidade.InicioVigencia;
                    item.TerminoVigencia = pontuacaoFidelidade.TerminoVigencia;
                }
                _itemPontuacaoFidelidadeRepository.Save(item);
            }

            bre.ThrowIfHasError();

            return pontuacaoFidelidade;
        }
        
        public byte[] GerarExcel(long idPontuacaoFidelidade, string regional)
        {
            var pontuacaoFidelidade = _pontuacaoFidelidadeRepository.FindById(idPontuacaoFidelidade);

            if (pontuacaoFidelidade.IsEmpty())
            {
                pontuacaoFidelidade = new PontuacaoFidelidade();
            }

            var empreendimentos = _enderecoEmpreendimentoRepository
                .EnderecosDaRegional(regional)
                .Select(reg => reg.Empreendimento)
                .OrderByDescending(x => x.Nome)
                .ThenBy(x => x.Nome)
                .ToList();

            var itensPontuacaoFidelidadeExistentes = _itemPontuacaoFidelidadeRepository.ItensDePontuacaoFidelidade(idPontuacaoFidelidade);

            ExcelUtil excel = ExcelUtil.NewInstance(35)
                .NewSheet(GlobalMessages.PontuacaoFidelidade + " - " + pontuacaoFidelidade.Regional);

            //var currentWorksheet = excel.CurrentExcelWorksheet();
            //currentWorksheet.Cells[1, 1].Value = GlobalMessages.Empreendimento;
            //currentWorksheet.Cells[1, 1].Style.Font.Size = 12;
            //currentWorksheet.Cells[1, 1].Style.Font.Bold = true;
            //currentWorksheet.Cells[1, 1].AutoFitColumns(40);

            //currentWorksheet.Cells[1, 2].Value = GlobalMessages.ColunaPontuacaoFaixaoUmMeio;
            //currentWorksheet.Cells[1, 2].Style.Font.Bold = true;
            //currentWorksheet.Cells[1, 2].Style.Font.Size = 12;
            //currentWorksheet.Cells[1, 2].AutoFitColumns(20);

            //if (pontuacaoFidelidade.TipoPontuacaoFidelidade == TipoPontuacaoFidelidade.Normal)
            //{
            //    currentWorksheet.Cells[1, 3].Value = GlobalMessages.ColunaPontuacaoFaixaDois;
            //    currentWorksheet.Cells[1, 3].Style.Font.Bold = true;
            //    currentWorksheet.Cells[1, 3].Style.Font.Size = 12;
            //    currentWorksheet.Cells[1, 3].AutoFitColumns(20);
            //}
            //else
            //{
            //    switch (pontuacaoFidelidade.TipoCampanhaFidelidade)
            //    {
            //        case TipoCampanhaFidelidade.PorVenda:
            //            currentWorksheet.Cells[1, 3].Value = GlobalMessages.ColunaPontuacaoFaixaDois;
            //            currentWorksheet.Cells[1, 3].Style.Font.Bold = true;
            //            currentWorksheet.Cells[1, 3].Style.Font.Size = 12;
            //            currentWorksheet.Cells[1, 3].AutoFitColumns(20);
            //            break;

            //        case TipoCampanhaFidelidade.PorVendaMinima:
            //            currentWorksheet.Cells[1, 3].Value = GlobalMessages.ColunaPontuacaoFaixaUmMeioNormal;
            //            currentWorksheet.Cells[1, 3].Style.Font.Bold = true;
            //            currentWorksheet.Cells[1, 3].Style.Font.Size = 12;
            //            currentWorksheet.Cells[1, 3].AutoFitColumns(20);

            //            currentWorksheet.Cells[1, 4].Value = GlobalMessages.ColunaPontuacaoFaixaDois;
            //            currentWorksheet.Cells[1, 4].Style.Font.Bold = true;
            //            currentWorksheet.Cells[1, 4].Style.Font.Size = 12;
            //            currentWorksheet.Cells[1, 4].AutoFitColumns(20);

            //            currentWorksheet.Cells[1, 5].Value = GlobalMessages.ColunaPontuacaoFaixaDoisNormal;
            //            currentWorksheet.Cells[1, 5].Style.Font.Bold = true;
            //            currentWorksheet.Cells[1, 5].Style.Font.Size = 12;
            //            currentWorksheet.Cells[1, 5].AutoFitColumns(20);
            //            break;

            //        case TipoCampanhaFidelidade.PorVendaMinimaEmpreendimento:
            //            currentWorksheet.Cells[1, 3].Value = GlobalMessages.ColunaPontuacaoFaixaUmMeioNormal;
            //            currentWorksheet.Cells[1, 3].Style.Font.Bold = true;
            //            currentWorksheet.Cells[1, 3].Style.Font.Size = 12;
            //            currentWorksheet.Cells[1, 3].AutoFitColumns(20);

            //            currentWorksheet.Cells[1, 4].Value = GlobalMessages.ColunaPontuacaoFaixaDois;
            //            currentWorksheet.Cells[1, 4].Style.Font.Bold = true;
            //            currentWorksheet.Cells[1, 4].Style.Font.Size = 12;
            //            currentWorksheet.Cells[1, 4].AutoFitColumns(20);

            //            currentWorksheet.Cells[1, 5].Value = GlobalMessages.ColunaPontuacaoFaixaDoisNormal;
            //            currentWorksheet.Cells[1, 5].Style.Font.Bold = true;
            //            currentWorksheet.Cells[1, 5].Style.Font.Size = 12;
            //            currentWorksheet.Cells[1, 5].AutoFitColumns(20);

            //            currentWorksheet.Cells[1, 6].Value = GlobalMessages.QuantidadeMinima;
            //            currentWorksheet.Cells[1, 6].Style.Font.Bold = true;
            //            currentWorksheet.Cells[1, 6].Style.Font.Size = 12;
            //            currentWorksheet.Cells[1, 6].AutoFitColumns(20);
            //            break;
            //    }
            //}
            
            //for (var j = 0; j < empreendimentos.Count; j++)
            //{
            //    var empreendimento = empreendimentos[j];

            //    currentWorksheet.Cells[2 + j, 1].Value = empreendimento.Nome;
            //    currentWorksheet.Cells[2 + j, 1].AutoFitColumns(26);
            //    currentWorksheet.Cells[2 + j, 1].Style.Font.Bold = true;
            //    currentWorksheet.Cells[2 + j, 1].Style.Font.Size = 12;

            //    var itemPontuacaoFidelidade = BuscarItemPontuacaoFidelidadeMatriz(itensPontuacaoFidelidadeExistentes,empreendimento, false, pontuacaoFidelidade);

            //    currentWorksheet.Cells[2 + j, 2].Value = itemPontuacaoFidelidade.ValorFaixaUmMeio;

            //    if (pontuacaoFidelidade.TipoPontuacaoFidelidade == TipoPontuacaoFidelidade.Normal)
            //    {
            //        currentWorksheet.Cells[2 + j, 3].Value = itemPontuacaoFidelidade.ValorFaixaDois;
            //    }
            //    else
            //    {
            //        switch (pontuacaoFidelidade.TipoCampanhaFidelidade)
            //        {
            //            case TipoCampanhaFidelidade.PorVenda:
            //                currentWorksheet.Cells[2 + j, 3].Value = itemPontuacaoFidelidade.ValorFaixaDois;
            //                break;

            //            case TipoCampanhaFidelidade.PorVendaMinima:
            //                currentWorksheet.Cells[2 + j, 3].Value = itemPontuacaoFidelidade.ValorFaixaUmMeioAnterior;
            //                currentWorksheet.Cells[2 + j, 4].Value = itemPontuacaoFidelidade.ValorFaixaDois;
            //                currentWorksheet.Cells[2 + j, 5].Value = itemPontuacaoFidelidade.ValorFaixaDoisAnterior;
            //                break;

            //            case TipoCampanhaFidelidade.PorVendaMinimaEmpreendimento:
            //                currentWorksheet.Cells[2 + j, 3].Value = itemPontuacaoFidelidade.ValorFaixaUmMeioAnterior;
            //                currentWorksheet.Cells[2 + j, 4].Value = itemPontuacaoFidelidade.ValorFaixaDois;
            //                currentWorksheet.Cells[2 + j, 5].Value = itemPontuacaoFidelidade.ValorFaixaDoisAnterior;
            //                currentWorksheet.Cells[2 + j, 6].Value = itemPontuacaoFidelidade.QuantidadeMinima;
            //                break;
            //        }
            //    }
            //}
            
            excel.Close();
            return excel.DownloadFile();
        }   
        /**
         * Libera a pontuação
         */
        public void Liberar(long idPontuacaoFidelidade)
        {
            var bre = new BusinessRuleException();

            var pontuacaoFidelidade = _pontuacaoFidelidadeRepository.FindById(idPontuacaoFidelidade);

            if (pontuacaoFidelidade.IsEmpty())
            {
                bre.AddError(string.Format(GlobalMessages.ErroRegistroInexistente, GlobalMessages.PontuacaoFidelidade));            
            }

            bre.ThrowIfHasError();

            ValidarLiberacaoPontuacaoFidelidade(pontuacaoFidelidade);

            switch (pontuacaoFidelidade.TipoPontuacaoFidelidade)
            {
                case TipoPontuacaoFidelidade.Normal:
                    LiberarPontuacaoNormal(pontuacaoFidelidade);
                    break;
                case TipoPontuacaoFidelidade.Campanha:
                    LiberarPontuacaoCampanha(pontuacaoFidelidade);
                    break;
                default:
                    bre.AddError(GlobalMessages.ErroCampoValorZero).Complete();
                    break;
            }
            
            bre.ThrowIfHasError();
            
        }

        /*
         * Altera a situação dos itens normais que estão ativos
         */
        public List<ItemPontuacaoFidelidade> AlterarSituacaoDeItemPontuacao(List<ItemPontuacaoFidelidade> itensComNormalAtiva,SituacaoPontuacaoFidelidade situacao)
        {
            foreach(var item in itensComNormalAtiva)
            {
                item.Situacao = situacao;

                if (situacao == SituacaoPontuacaoFidelidade.Vencido && item.PontuacaoFidelidade.TipoPontuacaoFidelidade==TipoPontuacaoFidelidade.Normal)
                {
                    item.TerminoVigencia = DateTime.Now;
                }

                _itemPontuacaoFidelidadeRepository.Save(item);
            }

            return itensComNormalAtiva;
        }

        /*
         * altera a situação da pontuação que não tenha item ativo
         */
        public List<ItemPontuacaoFidelidade> AlterarSituacaoDePontuacao(List<ItemPontuacaoFidelidade> itensPontuacaoFidelidade,SituacaoPontuacaoFidelidade situacao)
        {
            var ids = itensPontuacaoFidelidade.Select(x => x.Id);
            foreach(var item in itensPontuacaoFidelidade)
            {
                var itemAtivo = _itemPontuacaoFidelidadeRepository.Queryable()
                    .Where(x=>!ids.Contains(x.Id))
                    .Where(x=>x.PontuacaoFidelidade.Id==item.PontuacaoFidelidade.Id)
                    .Where(x => x.PontuacaoFidelidade.TipoPontuacaoFidelidade == TipoPontuacaoFidelidade.Normal)
                    .Where(x => x.Situacao == SituacaoPontuacaoFidelidade.Ativo)
                    .Any();

                if (!itemAtivo)
                {
                    if (situacao == SituacaoPontuacaoFidelidade.Vencido && item.PontuacaoFidelidade.TipoPontuacaoFidelidade == TipoPontuacaoFidelidade.Normal)
                    {
                        item.PontuacaoFidelidade.TerminoVigencia = DateTime.Now;
                    }

                    item.PontuacaoFidelidade.Situacao = situacao;
                    
                    _pontuacaoFidelidadeRepository.Save(item.PontuacaoFidelidade);
                }
            }

            return itensPontuacaoFidelidade;

        }

        /*
         * Ativa nova pontuação de fidelidade
         */
        public PontuacaoFidelidade AtivarPontuacaoFidelidade(PontuacaoFidelidade pontuacaoFidelidade)
        {
            pontuacaoFidelidade.Situacao = SituacaoPontuacaoFidelidade.AguardandoLiberacao;

            if (pontuacaoFidelidade.TipoPontuacaoFidelidade == TipoPontuacaoFidelidade.Normal)
            {
                pontuacaoFidelidade.InicioVigencia = DateTime.Now;
                pontuacaoFidelidade.Situacao = SituacaoPontuacaoFidelidade.Ativo;
            }
            
            _pontuacaoFidelidadeRepository.Save(pontuacaoFidelidade);

            return pontuacaoFidelidade;
        }

        /*
         * Ativa o item da nova pontuação de fidelidade
         */
        public List<ItemPontuacaoFidelidade> AtivarItemPontuacaoFidelidade(List<ItemPontuacaoFidelidade> itensPontuacaoFidelidade)
        {
            foreach (var item in itensPontuacaoFidelidade)
            {
                item.Situacao = SituacaoPontuacaoFidelidade.AguardandoLiberacao;

                if (item.PontuacaoFidelidade.TipoPontuacaoFidelidade == TipoPontuacaoFidelidade.Normal)
                {
                    item.InicioVigencia = DateTime.Now;
                    item.Situacao = SituacaoPontuacaoFidelidade.Ativo;
                }

                _itemPontuacaoFidelidadeRepository.Save(item);
            }

            return itensPontuacaoFidelidade;
        }
        
        /*
         * Libera uma nova pontuação de fidelidade do tipo normal
         */
        public void LiberarPontuacaoNormal(PontuacaoFidelidade pontuacaoFidelidade)
        {
            var bre = new BusinessRuleException();

            var itensPontuacaoFidelidade = _itemPontuacaoFidelidadeRepository.BuscarItens(pontuacaoFidelidade.Id);

            if (itensPontuacaoFidelidade.IsEmpty())
            {
                bre.AddError("A tabela não possui itens registrados");
            }

            bre.ThrowIfHasError();

            var empreendimentos = itensPontuacaoFidelidade.Select(x => x.Empreendimento.Id).ToList();

            var itensComNormalAtiva = _itemPontuacaoFidelidadeRepository.EmpreendimentosComNormalAtiva(empreendimentos);

            //Finaliza item de pontuação
            itensComNormalAtiva = AlterarSituacaoDeItemPontuacao(itensComNormalAtiva,SituacaoPontuacaoFidelidade.Vencido);

            itensComNormalAtiva = AlterarSituacaoDePontuacao(itensComNormalAtiva, SituacaoPontuacaoFidelidade.Vencido);
            
            pontuacaoFidelidade = AtivarPontuacaoFidelidade(pontuacaoFidelidade);

            itensPontuacaoFidelidade = AtivarItemPontuacaoFidelidade(itensPontuacaoFidelidade);

            var empresasVenda = itensPontuacaoFidelidade.Where(x => x.Situacao == SituacaoPontuacaoFidelidade.Ativo)
                .Select(x => x.EmpresaVenda.Id).ToList();
            NotificarEmpresaVenda(empresasVenda,pontuacaoFidelidade.Descricao);
                
            bre.ThrowIfHasError();
            
        }
        /**
         * Notifica a empresa de venda sempre que a tabela é ativada
         */
        public void NotificarEmpresaVenda(List<long> idsEmpresaVenda,string pontuacaoFidelidade)
        {
            var corretores = _corretorRepository.ListarDiretoresAtivosDaEmpresaDeVendas(idsEmpresaVenda);

            foreach(var corretor in corretores)
            {
                var notificacao = new Notificacao
                {
                    Titulo = GlobalMessages.TituloNovaTabelaPontuacaoAtivada,
                    Conteudo = string.Format(GlobalMessages.MsgNovaTabelaPontuacaoAtivada, pontuacaoFidelidade),
                    Usuario = corretor.Usuario,
                    EmpresaVenda = corretor.EmpresaVenda,
                    TipoNotificacao = TipoNotificacao.Comum,
                    DestinoNotificacao = DestinoNotificacao.Portal,
                };

                _notificacaoRepository.Save(notificacao);
            }

        }

        public void NotificarEmpresaVenda(List<PontuacaoFidelidade> pontuacoesAtivas,List<ItemPontuacaoFidelidade> itensAtivos)
        {
            foreach(var pontuacao in pontuacoesAtivas)
            {
                var evs = itensAtivos.Where(x => x.PontuacaoFidelidade.Id == pontuacao.Id)
                    .Select(x => x.EmpresaVenda).Distinct().ToList();

                var diretores = _corretorRepository.Queryable()
                    .Where(x => evs.Contains(x.EmpresaVenda))
                    .Where(x => x.Funcao == TipoFuncao.Diretor)
                    .ToList();

                foreach (var diretor in diretores)
                {
                    var notificacao = new Notificacao
                    {
                        Titulo = GlobalMessages.TituloNovaTabelaPontuacaoAtivada,
                        Conteudo = string.Format(GlobalMessages.MsgNovaTabelaPontuacaoAtivada, pontuacao.Descricao),
                        Usuario = diretor.Usuario,
                        EmpresaVenda = diretor.EmpresaVenda,
                        TipoNotificacao = TipoNotificacao.Comum,
                        DestinoNotificacao = DestinoNotificacao.Portal,
                    };
                    _notificacaoRepository.Save(notificacao);
                }
            }
        }

        /*
         * Libera uma nova pontuação de fidelidade do tipo Campanha
         */
        public void LiberarPontuacaoCampanha(PontuacaoFidelidade pontuacaoFidelidade)
        {
            var bre = new BusinessRuleException();

            var itensPontuacaoFidelidade = _itemPontuacaoFidelidadeRepository.BuscarItens(pontuacaoFidelidade.Id);

            if (itensPontuacaoFidelidade.IsEmpty())
            {
                bre.AddError("A tabela não possui itens registrados");
            }

            bre.ThrowIfHasError();

            var empreendimentos = itensPontuacaoFidelidade.Select(x => x.Empreendimento.Id).ToList();

            var itensComNormalAtiva = _itemPontuacaoFidelidadeRepository.EmpreendimentosComNormalAtiva(empreendimentos);

            itensComNormalAtiva = AlterarSituacaoDeItemPontuacao(itensComNormalAtiva,SituacaoPontuacaoFidelidade.Suspenso);

            itensComNormalAtiva = AlterarSituacaoDePontuacao(itensComNormalAtiva, SituacaoPontuacaoFidelidade.Suspenso);

            pontuacaoFidelidade = AtivarPontuacaoFidelidade(pontuacaoFidelidade);

            itensPontuacaoFidelidade = AtivarItemPontuacaoFidelidade(itensPontuacaoFidelidade);

            bre.ThrowIfHasError();

        }

        /*
         * Método que valida a vigência de campanha e normal para que não haja interseções
         */
        public void ValidarLiberacaoPontuacaoFidelidade(PontuacaoFidelidade pontuacaoFidelidade)
        {
            var bre = new BusinessRuleException();
            
            var itensNovos = _itemPontuacaoFidelidadeRepository.BuscarItens(pontuacaoFidelidade.Id);

            if (!itensNovos.HasValue())
            {
                bre.AddError(string.Format("Não há itens cadastrados para a tabela de pontuação {0}",pontuacaoFidelidade.Descricao)).Complete();
            }          

            bre.ThrowIfHasError();

            var idsNovos = itensNovos.Select(x => x.Id);
            var empreendimentos = itensNovos.Select(x => x.Empreendimento.Id);

            var itensVelhos = _itemPontuacaoFidelidadeRepository.Queryable()
                .Where(x => empreendimentos.Contains(x.Empreendimento.Id))
                .Where(x=> !idsNovos.Contains(x.Id));

            foreach(var emp in empreendimentos)
            {
                if (pontuacaoFidelidade.TipoPontuacaoFidelidade == TipoPontuacaoFidelidade.Normal)
                {
                    var situacoes = new List<SituacaoPontuacaoFidelidade> { SituacaoPontuacaoFidelidade.Ativo, SituacaoPontuacaoFidelidade.AguardandoLiberacao };
                    var valido = itensVelhos.Where(x => x.Empreendimento.Id == emp)
                        .Where(x => x.PontuacaoFidelidade.TipoPontuacaoFidelidade == TipoPontuacaoFidelidade.Campanha)
                        .Where(x => situacoes.Contains(x.Situacao))
                        .Where(x => DateTime.Now >= x.InicioVigencia)
                        .Where(x => DateTime.Now <= x.TerminoVigencia)
                        .Any();

                    if (valido)
                    {
                        bre.AddError(string.Format("Não é possível liberar a tabela de pontuação {0}",pontuacaoFidelidade.Descricao)).Complete();
                    }

                    bre.ThrowIfHasError();
                }

                if (pontuacaoFidelidade.TipoPontuacaoFidelidade == TipoPontuacaoFidelidade.Campanha)
                {
                    var situacoes = new List<SituacaoPontuacaoFidelidade> { SituacaoPontuacaoFidelidade.Ativo, SituacaoPontuacaoFidelidade.AguardandoLiberacao };
                    var valido = itensVelhos.Where(x => x.Empreendimento.Id == emp)
                        .Where(x => x.PontuacaoFidelidade.TipoPontuacaoFidelidade == TipoPontuacaoFidelidade.Campanha)
                        .Where(x => situacoes.Contains(x.Situacao))
                        .Where(x => pontuacaoFidelidade.InicioVigencia <= x.InicioVigencia)
                        .Where(x => pontuacaoFidelidade.TerminoVigencia >= x.InicioVigencia)
                        .Any();

                    if (valido)
                    {
                        bre.AddError(string.Format("Não é possível liberar a tabela de pontuação {0}", pontuacaoFidelidade.Descricao)).Complete();
                        bre.ThrowIfHasError();
                    }

                    valido = itensVelhos.Where(x => x.Empreendimento.Id == emp)
                        .Where(x => x.PontuacaoFidelidade.TipoPontuacaoFidelidade == TipoPontuacaoFidelidade.Campanha)
                        .Where(x => situacoes.Contains(x.Situacao))
                        .Where(x => pontuacaoFidelidade.InicioVigencia <= x.TerminoVigencia)
                        .Where(x => pontuacaoFidelidade.TerminoVigencia >= x.TerminoVigencia)
                        .Any();

                    if (valido)
                    {
                        bre.AddError(string.Format("Não é possível liberar a tabela de pontuação {0}", pontuacaoFidelidade.Descricao)).Complete();
                        bre.ThrowIfHasError();
                    }

                    valido = itensVelhos.Where(x => x.Empreendimento.Id == emp)
                        .Where(x => x.PontuacaoFidelidade.TipoPontuacaoFidelidade == TipoPontuacaoFidelidade.Campanha)
                        .Where(x => situacoes.Contains(x.Situacao))
                        .Where(x => x.InicioVigencia <= pontuacaoFidelidade.InicioVigencia)
                        .Where(x => x.TerminoVigencia >= pontuacaoFidelidade.TerminoVigencia)
                        .Any();

                    if (valido)
                    {
                        bre.AddError(string.Format("Não é possível liberar a tabela de pontuação {0}", pontuacaoFidelidade.Descricao)).Complete();
                        bre.ThrowIfHasError();
                    }

                    valido = itensVelhos.Where(x => x.Empreendimento.Id == emp)
                        .Where(x => x.PontuacaoFidelidade.TipoPontuacaoFidelidade == TipoPontuacaoFidelidade.Campanha)
                        .Where(x => situacoes.Contains(x.Situacao))
                        .Where(x => pontuacaoFidelidade.InicioVigencia <= x.InicioVigencia)
                        .Where(x => pontuacaoFidelidade.TerminoVigencia >= x.TerminoVigencia)
                        .Any();

                    if (valido)
                    {
                        bre.AddError(string.Format("Não é possível liberar a tabela de pontuação {0}", pontuacaoFidelidade.Descricao)).Complete();
                        bre.ThrowIfHasError();
                    }
                }
            }
        }

        public byte[] Exportar(DataSourceRequest request, PontuacaoFidelidadeDTO filtro)
        {
            var resultados = _viewPontuacaoFidelidadeRepository.ListarDatatablePontuacaoFidelidade(request, filtro)
                .records.ToList();
            
            ExcelUtil excel = ExcelUtil.NewInstance(25)
                .NewSheet(DateTime.Now.ToString(GlobalMessages.PontuacaoFidelidade))
                .WithHeader(GetHeader());

            foreach (var model in resultados)
            {
                excel
                    .CreateCellValue(model.Regional).Width(10)
                    .CreateCellValue(model.NomeEmpresaVenda).Width(50)
                    .CreateCellValue(model.NomeEmpreendimento).Width(50)
                    .CreateCellValue(model.Descricao).Width(20)
                    .CreateCellValue(model.Codigo).Width(20)
                    .CreateCellValue(model.InicioVigencia.HasValue()?model.InicioVigencia.Value.ToString():"").Width(20)
                    .CreateCellValue(model.TerminoVigencia.HasValue() ? model.TerminoVigencia.Value.ToString() : "").Width(20)
                    .CreateCellValue(model.TipoPontuacaoFidelidade).Width(20)
                    .CreateCellValue(model.TipoCampanhaFidelidade).Width(50)
                    .CreateCellValue(model.Situacao.AsString()).Width(50);
            }

            excel.Close();
            return excel.DownloadFile();
        }

        private string[] GetHeader()
        {
            IList<string> header = new List<string>
            {
                GlobalMessages.Regional,
                GlobalMessages.EmpresaVenda,
                GlobalMessages.Empreendimento,
                GlobalMessages.Descricao,
                GlobalMessages.Codigo,
                GlobalMessages.InicioVigencia,
                GlobalMessages.TerminoVigencia,
                GlobalMessages.TipoPontuacaoFidelidade,
                GlobalMessages.TipoCampanhaFidelidade,
                GlobalMessages.Situacao
            };
            return header.ToArray();
        }

        public void InativarCampanha(PontuacaoFidelidade pontuacaoFidelidade)
        {
            pontuacaoFidelidade.Situacao = SituacaoPontuacaoFidelidade.Vencido;
            _pontuacaoFidelidadeRepository.Save(pontuacaoFidelidade);
        }

        public void Excluir(long idPontuacaoFidelidade)
        {
            var bre = new BusinessRuleException();

            var pontuacaoFidelidade = _pontuacaoFidelidadeRepository.FindById(idPontuacaoFidelidade);

            if (pontuacaoFidelidade.IsEmpty())
            {
                bre.AddError(string.Format(GlobalMessages.ErroRegistroInexistente, GlobalMessages.PontuacaoFidelidade));
                bre.ThrowIfHasError();
            }

            if (pontuacaoFidelidade.Situacao != SituacaoPontuacaoFidelidade.Rascunho)
            {
                bre.AddError(string.Format("Não é permitodo excluir tabela em situação {0}", pontuacaoFidelidade.Situacao.AsString())).Complete();
                bre.ThrowIfHasError();
            }

            var itens = _itemPontuacaoFidelidadeRepository.BuscarItens(pontuacaoFidelidade.Id);

            foreach(var item in itens)
            {
                _itemPontuacaoFidelidadeRepository.Delete(item);
            }

            _pontuacaoFidelidadeRepository.Delete(pontuacaoFidelidade);

        }
    }
}
