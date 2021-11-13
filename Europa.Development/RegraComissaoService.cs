using System;
using Newtonsoft.Json;
using NHibernate;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Europa.Extensions;
using Europa.Resources;
using FluentNHibernate.Testing.Values;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services.Models;

namespace Europa.Development
{
    public class RegraComissaoService
    {
        public ISession _session { get; set; }

        public EmpreendimentoRepository _empreendimentoRepository { get; set; }
        public EnderecoEmpreendimentoRepository _enderecoEmpreendimentoRepository { get; set; }
        public EmpresaVendaRepository _empresaVendaRepository { get; set; }
        public RegraComissaoRepository _regraComissaoRepository { get; set; }
        public ItemRegraComissaoRepository _itemRegraComissaoRepository { get; set; }

        public void Init(ISession session)
        {
            _session = session;

            _empreendimentoRepository = new EmpreendimentoRepository();
            _empreendimentoRepository._session = _session;
            _empresaVendaRepository = new EmpresaVendaRepository();
            _empresaVendaRepository._session = _session;
            _regraComissaoRepository = new RegraComissaoRepository();
            _regraComissaoRepository._session = _session;
            _itemRegraComissaoRepository = new ItemRegraComissaoRepository();
            _itemRegraComissaoRepository._session = _session;
            _enderecoEmpreendimentoRepository = new EnderecoEmpreendimentoRepository();
            _enderecoEmpreendimentoRepository._session = _session;
        }

        public void RunForJExcel()
        {
            string regional = "SP";
            long idRegraComissao = 1;

            // Dimensão X (horizontal)
            var empreendimentos = _enderecoEmpreendimentoRepository
                .EnderecosDaRegional(regional)
                .Select(reg => reg.Empreendimento)
                .ToList();

            // Dimensão Y (vertical)
            var empresasDeVenda = _empresaVendaRepository.EmpresasDaRegional(regional);

            // Buscando itens e jogando para memória para evitar acesso granular no BD (uma query só e o resto em memória)
            var regrasComissaoExistentes = _itemRegraComissaoRepository.ItensDeRegra(idRegraComissao);

            empreendimentos.ForEach(reg => _session.Evict(reg));


            var columns = new List<Column>();
            columns.Add(new Column() {Type = ColumnType.Text, Width = 150, ReadOnly = true});


            var headersEmpreendimentos = new List<Header>();
            headersEmpreendimentos.Add(new Header() {Title = " ---------- ", Colspan = 1});
            empreendimentos.Take(3).ToList().ForEach(reg =>
                headersEmpreendimentos.Add(new Header() {Title = reg.Nome, Colspan = 5}));

            var headersConfiguracoes = new List<Header>();
            headersConfiguracoes.Add(new Header() {Title = " Empresa de Venda ", Colspan = 1});

            var configuracaoFaixaUmMeio = new Header {Title = "Faixa 1,5", Colspan = 1};
            var configuracaoFaixaDois = new Header {Title = "Faixa 2", Colspan = 1};
            var configuracaoPagtoKitCompleto = new Header {Title = "Pagto Kit Completo", Colspan = 1};
            var configuracaoPagtoConformidade = new Header {Title = "Pagto Conformidade", Colspan = 1};
            var configuracaoPagtoRepasse = new Header {Title = "Pagto Repasse", Colspan = 1};
            empreendimentos.Take(3).ToList().ForEach(reg =>
            {
                headersConfiguracoes.Add(configuracaoFaixaUmMeio);
                headersConfiguracoes.Add(configuracaoFaixaDois);
                headersConfiguracoes.Add(configuracaoPagtoKitCompleto);
                headersConfiguracoes.Add(configuracaoPagtoConformidade);
                headersConfiguracoes.Add(configuracaoPagtoRepasse);

                for (int index = 0; index < 5; index++)
                {
                    columns.Add(new Column() {Type = ColumnType.Numeric, Width = 30});
                }
            });


            var nestedHeaders = new object[2];
            nestedHeaders[0] = headersEmpreendimentos;
            nestedHeaders[1] = headersConfiguracoes;

            var config = new
            {
                columns = columns,
                nestedHeaders = nestedHeaders
            };


            const string columnDefinition = @"{type:'{{type}}',readOnly:{{readonly}},width:{{width}}}";

            var serializedData = JsonConvert.SerializeObject(config);

            File.WriteAllText(@"C:\tmp\headers.json", serializedData);


            //var chaveValorEmpreendimento = empreendimentos.Select(reg => new KeyValueDTO() { Id = reg.Id, Nome = reg.Nome }).ToList();

            //empresasDeVenda.ForEach(reg => _session.Evict(reg));
            //var chaveValorEmpresaVenda = empresasDeVenda.Select(reg => new KeyValueDTO() { Id = reg.Id, Nome = reg.NomeFantasia }).ToList();

            //regrasComissaoExistentes.ForEach(reg => _session.Evict(reg));

            //var regraComisssao = new RegraComissao();
            //regraComisssao.Id = idRegraComissao;

            //RegraComissaoDTO regraComissaoDto = new RegraComissaoDTO();
            //regraComissaoDto.Descricao = "MOCK";
            //regraComissaoDto.Regional = regional;
            //regraComissaoDto.Empreendimentos = chaveValorEmpreendimento;
            //regraComissaoDto.EmpresasVenda = chaveValorEmpreendimento;

            //foreach (var empresaVenda in empresasDeVenda)
            //{
            //    foreach (var empreendimento in empreendimentos)
            //    {
            //        // Single or default para evitar possíveis duplicados na mesma regra
            //        var itemRegraComissao = regrasComissaoExistentes
            //            .Where(reg => reg.Empreendimento.Id == empreendimento.Id)
            //            .Where(reg => reg.EmpresaVenda.Id == empresaVenda.Id)
            //            .SingleOrDefault();

            //        if (itemRegraComissao == null)
            //        {
            //            itemRegraComissao = new ItemRegraComissao();
            //            itemRegraComissao.Empreendimento = empreendimento;
            //            itemRegraComissao.EmpresaVenda = empresaVenda;
            //        }

            //        var itemRegraComissaoDto = new ItemRegraComissaoDTO(regraComisssao, itemRegraComissao);

            //        regraComissaoDto.Itens.Add(itemRegraComissaoDto);
            //    }
            //}


            //var serializedData = JsonConvert.SerializeObject(regraComissaoDto);

            //File.WriteAllText(@"C:\tmp\regra-comissao.json", serializedData);
        }

        public JExcelOptions BuildJExcelOptions(string regional)
        {
            var options = new JExcelOptions();

            var regraComissao = _regraComissaoRepository.BuscarRegraVigente(regional);

            if (regraComissao.IsEmpty())
            {
                regraComissao = new RegraComissao();
            }

            var empreendimentos = _enderecoEmpreendimentoRepository
                .EnderecosDaRegional(regional)
                .Select(reg => reg.Empreendimento)
                .ToList();
            var empresasVendas = _empresaVendaRepository.EmpresasDaRegional(regional);

            options = InsertHeaders(options, empreendimentos);
            options = InsertColumns(options, empreendimentos.Count);
            options = InsertData(options, empreendimentos, empresasVendas, regraComissao.Id);

            return options;
        }

        public JExcelOptions InsertHeaders(JExcelOptions options, List<Empreendimento> empreendimentos)
        {
            options.NestedHeaders = new List<List<Header>>()
            {
                new List<Header>
                {
                    new Header {Colspan = 1, Title = ""}
                }
            };

            foreach (var empreendimento in empreendimentos)
            {
                options.NestedHeaders[0].Add(new Header {Colspan = 7, Title = empreendimento.Nome});
            }

            return options;
        }

        public JExcelOptions InsertColumns(JExcelOptions options, int numEmpreendimentos)
        {
            options.Columns = new List<Column>
            {
                new Column {ReadOnly = true, Title = "IdEmpresaVenda", Type = ColumnType.Hidden, Width = 0},
                new Column {ReadOnly = true, Title = "Empresa Venda", Type = ColumnType.Text, Width = 200},
            };

            for (var i = 0; i < numEmpreendimentos; i++)
            {
                options.Columns.AddRange(new List<Column>
                {
                    new Column {ReadOnly = true, Title = "IdEmpreendimento", Type = ColumnType.Hidden, Width = 0},
                    new Column {ReadOnly = true, Title = "IdRegraComissao", Type = ColumnType.Hidden, Width = 0},
                    new Column {ReadOnly = false, Title = "Faixa 1.5", Type = ColumnType.Numeric, Width = 100},
                    new Column {ReadOnly = false, Title = "Faixa 2", Type = ColumnType.Numeric, Width = 100},
                    new Column {ReadOnly = false, Title = "Valor Kit Completo", Type = ColumnType.Numeric, Width = 200},
                    new Column {ReadOnly = false, Title = "Valor Conformidade", Type = ColumnType.Numeric, Width = 200},
                    new Column {ReadOnly = false, Title = "Valor Repasse", Type = ColumnType.Numeric, Width = 200}
                });
            }

            return options;
        }

        public JExcelOptions InsertData(JExcelOptions options, List<Empreendimento> empreendimentos,
            List<EmpresaVenda> empresasVendas, long idRegraComissao)
        {
            options.Data = new List<List<object>>();
            
            var regrasComissaoExistentes = _itemRegraComissaoRepository.ItensDeRegra(idRegraComissao);

            foreach (var empresaVenda in empresasVendas)
            {
                var row = new List<object>
                {
                    empresaVenda.Id,
                    empresaVenda.NomeFantasia
                };
                foreach (var empreendimento in empreendimentos)
                {
                    var itemRegraComissao = regrasComissaoExistentes
                        .Where(reg => reg.Empreendimento.Id == empreendimento.Id)
                        .Where(reg => reg.EmpresaVenda.Id == empresaVenda.Id)
                        .SingleOrDefault();

                    if (itemRegraComissao == null)
                    {
                        itemRegraComissao = new ItemRegraComissao();
                        itemRegraComissao.Empreendimento = empreendimento;
                        itemRegraComissao.EmpresaVenda = empresaVenda;
                    }
                    
                    row.AddRange(new List<Object>
                    {
                        itemRegraComissao.Empreendimento.Id,
                        idRegraComissao,
                        itemRegraComissao.FaixaUmMeio,
                        itemRegraComissao.FaixaDois,
                        itemRegraComissao.ValorKitCompleto,
                        itemRegraComissao.ValorConformidade,
                        itemRegraComissao.ValorRepasse
                    });
                }
                options.Data.Add(row);
            }

            return options;
        }

        public void Run()
        {
            string regional = "SP";
            long idRegraComissao = 1;

            // Dimensão X (horizontal)
            var empreendimentos = _enderecoEmpreendimentoRepository
                .EnderecosDaRegional(regional)
                .Select(reg => reg.Empreendimento)
                .ToList();

            // Dimensão Y (vertical)
            var empresasDeVenda = _empresaVendaRepository.EmpresasDaRegional(regional);

            // Buscando itens e jogando para memória para evitar acesso granular no BD (uma query só e o resto em memória)
            var regrasComissaoExistentes = _itemRegraComissaoRepository.ItensDeRegra(idRegraComissao);

            empreendimentos.ForEach(reg => _session.Evict(reg));
            var chaveValorEmpreendimento =
                empreendimentos.Select(reg => new KeyValueDTO() {Id = reg.Id, Nome = reg.Nome}).ToList();

            empresasDeVenda.ForEach(reg => _session.Evict(reg));
            var chaveValorEmpresaVenda = empresasDeVenda
                .Select(reg => new KeyValueDTO() {Id = reg.Id, Nome = reg.NomeFantasia}).ToList();

            regrasComissaoExistentes.ForEach(reg => _session.Evict(reg));

            var regraComisssao = new RegraComissao();
            regraComisssao.Id = idRegraComissao;

            RegraComissaoExcelDTO regraComissaoDto = new RegraComissaoExcelDTO();
            regraComissaoDto.Descricao = "MOCK";
            regraComissaoDto.Regional = regional;
            regraComissaoDto.Empreendimentos = chaveValorEmpreendimento;
            regraComissaoDto.EmpresasVenda = chaveValorEmpreendimento;

            foreach (var empresaVenda in empresasDeVenda)
            {
                foreach (var empreendimento in empreendimentos)
                {
                    // Single or default para evitar possíveis duplicados na mesma regra
                    var itemRegraComissao = regrasComissaoExistentes
                        .Where(reg => reg.Empreendimento.Id == empreendimento.Id)
                        .Where(reg => reg.EmpresaVenda.Id == empresaVenda.Id)
                        .SingleOrDefault();

                    if (itemRegraComissao == null)
                    {
                        itemRegraComissao = new ItemRegraComissao();
                        itemRegraComissao.Empreendimento = empreendimento;
                        itemRegraComissao.EmpresaVenda = empresaVenda;
                    }

                    var itemRegraComissaoDto = new ItemRegraComissaoDTO(regraComisssao, itemRegraComissao);

                    regraComissaoDto.Itens.Add(itemRegraComissaoDto);
                }
            }


            var serializedData = JsonConvert.SerializeObject(regraComissaoDto);

            File.WriteAllText(@"C:\tmp\regra-comissao.json", serializedData);
        }
    }
}