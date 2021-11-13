using Europa.Extensions;
using Europa.Resources;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.Core.Services;
using Tenda.Domain.Security.Models;
using Tenda.Domain.Security.Repository;
using Tenda.Domain.Security.Services.Models;
using Tenda.Domain.Security.Views;

namespace Tenda.Domain.Security.Services
{
    public class LogAcaoService : BaseService
    {
        public PermissaoService _permissaoService { get; set; }
        public FuncionalidadeRepository _repositorioFuncionalidade { get; set; }
        public AcessoRepository _repositorioAcesso { get; set; }
        public LogAcaoRepository _repositorioLogAcao { get; set; }
        public ViewLogAcaoRepository _repositorioViewLogAcao { get; set; }
        public UnidadeFuncionalRepository _repositorioUnidadeFuncional { get; set; }

        public void Logar(String codigoSistema, String codigoUnidadeFuncional, String comando, long idUsuarioLogado, String log)
        {
            if (codigoSistema.IsEmpty())
            {
                throw new ArgumentNullException("sistema");
            }
            if (codigoUnidadeFuncional.IsEmpty())
            {
                throw new ArgumentNullException("unidadeFuncional");
            }
            if (comando.IsEmpty())
            {
                throw new ArgumentNullException("comando");
            }
            if (idUsuarioLogado > 0)
            {
                throw new ArgumentNullException("usuarioLogado");
            }
            if (log.IsEmpty())
            {
                throw new ArgumentNullException("log");
            }

            Funcionalidade funcionalidade = _repositorioFuncionalidade.Queryable()
                .Where(f => f.UnidadeFuncional.Modulo.Sistema.Codigo == codigoSistema)
                .Where(f => f.UnidadeFuncional.Codigo == codigoUnidadeFuncional)
                .Where(f => f.Comando.ToLower() == comando.ToLower())
                .SingleOrDefault();

            if (funcionalidade.IsNull())
            {
                throw new ObjectNotFoundException("[" + codigoSistema + ", " + codigoUnidadeFuncional + ", " + comando + "]", "Funcionalidade");
            }

            Acesso acessoAtual = new AcessoRepository(Session).Queryable()
                .Where(a => a.Sistema.Codigo == codigoSistema)
                .Where(a => a.Usuario.Id == idUsuarioLogado)
                .Where(a => a.FormaEncerramento != 0)
                .OrderByDescending(a => a.InicioSessao)
                .Take(1)
                .SingleOrDefault();

            LogAcao logacao = new LogAcao();
            logacao.Acesso = acessoAtual;
            logacao.Funcionalidade = funcionalidade;
            logacao.Conteudo = log;
            logacao.ComPermissao = _permissaoService.VerificarPermissao(comando, codigoUnidadeFuncional, idUsuarioLogado, codigoSistema);

            _repositorioLogAcao.Save(logacao);

            Session.Flush();
        }

        public DataSourceResponse<UnidadeFuncional> ListarUnidadeFuncionalAutocomplete(DataSourceRequest request)
        {
            var results = _repositorioUnidadeFuncional.Queryable();
            if (request.filter.FirstOrDefault() != null)
            {
                var filterNome = request.filter.Where(reg => reg.column.ToLower() == "nome").FirstOrDefault();
                if (!filterNome.IsNull())
                {
                    String queryTerm = filterNome.value.ToString().ToLower();
                    results = results.Where(x => x.Nome.ToLower().Contains(queryTerm));

                }
                var filterSistema = request.filter.Where(reg => reg.column.ToLower() == "sistema").FirstOrDefault();
                if (!filterSistema.value.IsEmpty())
                {
                    String queryTerm = filterSistema.value.ToString().ToLower();
                    results = results.Where(x => x.Modulo.Sistema.Nome.ToLower().Contains(queryTerm));
                }

            }
            return results.ToDataRequest(request);
        }

        public DataSourceResponse<Funcionalidade> ListarFuncionalidadeAutocomplete(DataSourceRequest request)
        {
            var results = _repositorioFuncionalidade.Queryable();
            if (request.filter.FirstOrDefault() != null)
            {
                var filterNome = request.filter.Where(reg => reg.column.ToLower() == "nome").FirstOrDefault();
                if (!filterNome.IsNull())
                {
                    String queryTerm = filterNome.value.ToString().ToLower();
                    results = results.Where(x => x.Nome.ToLower().Contains(queryTerm));

                }
                var filterUF = request.filter.Where(reg => reg.column.ToLower() == "idunidadefuncional").FirstOrDefault();
                if (!filterUF.IsNull())
                {
                    long queryTerm = 0;
                    if (long.TryParse(filterUF.value, out queryTerm))
                    {
                        results = results.Where(x => x.UnidadeFuncional.Id == queryTerm);
                    }
                }

            }
            return results.ToDataRequest(request);
        }


        public DataSourceResponse<ViewLogAcao> Listar(DataSourceRequest request, LogAcaoDTO filtro)
        {
            var data = _repositorioViewLogAcao.Queryable();

            if (!filtro.IdUsuario.IsEmpty())
            {
                data = data.Where(reg => reg.IdUsuario == filtro.IdUsuario);
            }
            if (!filtro.Sistema.IsEmpty())
            {
                data = data.Where(reg => reg.NomeSistema.ToLower().Equals(filtro.Sistema.ToLower()));
            }
            if (!filtro.IdUnidadeFuncional.IsEmpty())
            {
                data = data.Where(reg => reg.IdUnidadeFuncional == filtro.IdUnidadeFuncional);
            }
            if (!filtro.IdFuncionalidade.IsEmpty())
            {
                data = data.Where(reg => reg.IdFuncionalidade == filtro.IdFuncionalidade);
            }
            if (!filtro.DataInicio.IsEmpty())
            {
                data = data.Where(reg => reg.CriadoEm.Date >= filtro.DataInicio.Date);
            }
            if (!filtro.DataFim.IsEmpty())
            {
                data = data.Where(reg => reg.CriadoEm.Date <= filtro.DataFim.Date);
            }

            return data.ToDataRequest<ViewLogAcao>(request);
        }

        public byte[] ExportarDatatable(DataSourceRequest request, LogAcaoDTO filtro)
        {
            request.start = 0;
            request.pageSize = 0;
            return Exportar(request, filtro);
        }

        public byte[] ExportarPagina(DataSourceRequest request, LogAcaoDTO filtro)
        {
            return Exportar(request, filtro);
        }

        public byte[] Exportar(DataSourceRequest request, LogAcaoDTO filtro)
        {
            ExcelUtil excel = ExcelUtil.NewInstance(20)
                .NewSheet(DateTime.Now.ToString("yyyyMMddHHmmss"))
                .WithHeader(GetHeader());

            var list = Listar(request, filtro);

            string dateCol = "dd/MM/yyyy HH:mm:ss";
            foreach (var model in list.records.ToList())
            {
                excel.CreateCellValue(model.CriadoEm).Format(dateCol)
                    .CreateCellValue(model.NomeSistema)
                    .CreateCellValue(model.NomeUnidadeFuncional)
                    .CreateCellValue(model.NomeFuncionalidade)
                    .CreateCellValue(model.NomeUsuario)
                    .CreateCellValue(model.IdAcesso)
                    .CreateCellValue(model.NomePerfil)
                    .CreateCellValue(model.Conteudo).Width(30);
            }
            excel.Close();
            return excel.DownloadFile();
        }

        public string[] GetHeader()
        {
            IList<string> header = new List<string>
            {
                GlobalMessages.Momento,
                GlobalMessages.Sistema,
                GlobalMessages.UnidadeFuncional,
                GlobalMessages.Funcionalidade,
                GlobalMessages.Usuario,
                GlobalMessages.Acesso,
                GlobalMessages.Perfil,
                GlobalMessages.Conteudo
            };
            return header.ToArray();
        }



    }
}
