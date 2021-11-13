using NHibernate;
using System;
using System.Linq;
using Tenda.Domain.Security.Repository;
using Tenda.EmpresaVenda.Domain.Services.Models;

namespace Europa.Development
{
    public class RelatorioAcessoService
    {
        private ISession _session;

        private Tenda.EmpresaVenda.Domain.Services.RelatorioAcessoService _relatorioAcessoService;
        private AcessoRepository _acessoRepository;

        public void Init(ISession session)
        {
            _session = session;
            _relatorioAcessoService = new Tenda.EmpresaVenda.Domain.Services.RelatorioAcessoService(session);
            _acessoRepository = new AcessoRepository(session);
        }

        public void Run()
        {
            var filtro = new FiltroPeriodoDTO()
            {
                DataInicio = new DateTime(2020, 03, 01),
                DataFim = new DateTime(2020, 03, 02)
            };

            var resultados = _relatorioAcessoService.AcessosCorretorEv(filtro);
            //var resultados = _relatorioAcessoService.AcessosDiariosPorRegional(filtro);
            resultados = resultados.OrderBy(x => x.Corretor);

            foreach (var result in resultados)
            {
                Console.WriteLine(result.Corretor + "\t" + result.EmpresaVenda + "\t" + result.InicioSessao + "\t" + result.FimSessao);
                //Console.WriteLine(result.Corretor + "\t" + result.EmpresaVenda + "\t" + result.TempoMedio);
                //Console.WriteLine(result.Regional + "\t" + result.InicioSessao + "\t" + result.Quantidade);

            }

            Console.WriteLine(resultados.Count());

            var auyx = 1;
        }
    }
}
