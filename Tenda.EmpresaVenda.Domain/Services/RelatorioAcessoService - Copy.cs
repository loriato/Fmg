using Europa.Extensions;
using Europa.Resources;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using NHibernate;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Tenda.Domain.Core.Services;
using Tenda.EmpresaVenda.Domain.Services.Models;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class RelatorioAcessoService : BaseService
    {
        public RelatorioAcessoService(ISession session) : base(session)
        {
        }

        public IQueryable<AcessoDiarioDTO> AcessosDiarios(FiltroPeriodoDTO filtro)
        {

            StringBuilder hql = new StringBuilder();
            hql.Append("SELECT date(a.InicioSessao) AS \"InicioSessao\", count(*) AS \"Quantidade\" FROM Acesso a ");
            hql.Append("WHERE a.Sistema.Codigo = :codigoSistema ");
            hql.Append("AND date(a.InicioSessao) BETWEEN :inicio AND :fim ");
            hql.Append("GROUP BY date(a.InicioSessao) ");


            IQuery query = Session.CreateQuery(hql.ToString())
                .SetResultTransformer(Transformers.AliasToBean<AcessoDiarioDTO>())
                .SetParameter("codigoSistema", "POR1818")
                .SetParameter("inicio", filtro.DataInicio)
                .SetParameter("fim", filtro.DataFim);


            return query.List<AcessoDiarioDTO>().AsQueryable();
        }

        public IQueryable<AcessoDiarioEvDTO> AcessosDiariosPorEv(FiltroPeriodoDTO filtro)
        {

            StringBuilder hql = new StringBuilder();
            hql.Append("SELECT ev.NomeFantasia AS \"EmpresaVenda\", ");
            hql.Append(" DATE(a.InicioSessao) AS \"InicioSessao\", ");
            hql.Append(" COUNT(*) AS \"Quantidade\" FROM Acesso a ");
            hql.Append(" INNER JOIN Corretor c ON c.Usuario.Id = a.Usuario.Id ");
            hql.Append(" INNER JOIN EmpresaVenda ev ON ev.Id = c.EmpresaVenda.Id ");
            hql.Append(" WHERE a.Sistema.Codigo = :codigoSistema ");
            hql.Append(" AND DATE(a.InicioSessao) BETWEEN :inicio AND :fim ");
            hql.Append(" GROUP BY ev.NomeFantasia, DATE(a.InicioSessao) ");


            IQuery query = Session.CreateQuery(hql.ToString())
                .SetResultTransformer(Transformers.AliasToBean<AcessoDiarioEvDTO>())
                .SetParameter("codigoSistema", "POR1818")
                .SetParameter("inicio", filtro.DataInicio)
                .SetParameter("fim", filtro.DataFim);


            return query.List<AcessoDiarioEvDTO>().AsQueryable();
        }
        //Erro TimeOut
        public IQueryable<AcessoCorretorEvDTO> AcessosCorretorEv(FiltroPeriodoDTO filtro)
        {
            StringBuilder hql = new StringBuilder();

            hql.Append("SELECT c.Nome AS \"Corretor\", ");
            hql.Append(" ev.NomeFantasia AS \"EmpresaVenda\", ");
            hql.Append(" a.InicioSessao AS \"InicioSessao\", ");
            hql.Append(" COALESCE( a.FimSessao, (SELECT max(log.AtualizadoEm)  FROM LogAcao log ");
            hql.Append(" WHERE log.Acesso.Id = a.Id), a.InicioSessao) AS \"FimSessao\" ");
            hql.Append(" FROM Acesso a ");
            hql.Append(" INNER JOIN Corretor c ON c.Usuario.Id = a.Usuario.Id ");
            hql.Append(" INNER JOIN EmpresaVenda ev ON ev.Id = c.EmpresaVenda.Id ");
            hql.Append(" WHERE a.Sistema.Codigo = :codigoSistema ");
            hql.Append(" AND DATE(a.InicioSessao) BETWEEN :inicio AND :fim ");


            IQuery query = Session.CreateQuery(hql.ToString())
                .SetResultTransformer(Transformers.AliasToBean<AcessoCorretorEvDTO>())
                .SetParameter("codigoSistema", "POR1818")
                .SetParameter("inicio", filtro.DataInicio)
                .SetParameter("fim", filtro.DataFim)
                .SetTimeout(30000);

            return query.List<AcessoCorretorEvDTO>().AsQueryable();
        }
        public IQueryable<TempoMedioAcessoDiarioCorretorEvDTO> TempoMedioAcessosDiariosPorCorretorEv(FiltroPeriodoDTO filtro)
        {


            StringBuilder hql = new StringBuilder();

            hql.Append("SELECT c.NM_CORRETOR AS \"Corretor\", ");
            hql.Append(" ev.NM_FANTASIA AS \"EmpresaVenda\", ");
            hql.Append(" AVG(AGE( a.DT_FIM_SESSAO, a.DT_INICIO_SESSAO)) AS \"TempoMedio\" ");
            hql.Append(" FROM TBL_ACESSOS a ");
            hql.Append(" INNER JOIN TBL_CORRETORES c ON c.ID_USUARIO = a.ID_USUARIO ");
            hql.Append(" INNER JOIN TBL_EMPRESAS_VENDAS ev ON ev.ID_EMPRESA_VENDA = c.ID_EMPRESA_VENDA");
            hql.Append(" INNER JOIN TBL_SISTEMAS S ON a.ID_SISTEMA = s.ID_SISTEMA");
            hql.Append(" WHERE S.CD_SISTEMA = :codigoSistema ");
            hql.Append(" AND a.DT_FIM_SESSAO IS NOT NULL ");
            hql.Append(" AND DATE(a.DT_INICIO_SESSAO) BETWEEN :inicio AND :fim ");
            hql.Append(" GROUP BY c.NM_CORRETOR, ev.NM_FANTASIA ");

            IQuery query = Session.CreateSQLQuery(hql.ToString())
                .AddScalar("Corretor", NHibernateUtil.AnsiString)
                .AddScalar("EmpresaVenda", NHibernateUtil.AnsiString)
                .AddScalar("TempoMedio", NHibernateUtil.AnsiString)
                .SetResultTransformer(Transformers.AliasToBean<TempoMedioAcessoDiarioCorretorEvDTO>())
                .SetParameter("codigoSistema", "POR1818")
                .SetParameter("inicio", filtro.DataInicio)
                .SetParameter("fim", filtro.DataFim);


            return query.List<TempoMedioAcessoDiarioCorretorEvDTO>().AsQueryable();
        }
        public IQueryable<TempoMedioAcessoDiarioEvDTO> TempoMedioAcessosDiariosEv(FiltroPeriodoDTO filtro)
        {


            StringBuilder hql = new StringBuilder();

            hql.Append("SELECT ev.NM_FANTASIA AS \"EmpresaVenda\", ");
            hql.Append(" AVG(AGE( a.DT_FIM_SESSAO, a.DT_INICIO_SESSAO)) AS \"TempoMedio\" ");
            hql.Append(" FROM TBL_ACESSOS a ");
            hql.Append(" INNER JOIN TBL_CORRETORES c ON c.ID_USUARIO = a.ID_USUARIO ");
            hql.Append(" INNER JOIN TBL_EMPRESAS_VENDAS ev ON ev.ID_EMPRESA_VENDA = c.ID_EMPRESA_VENDA");
            hql.Append(" INNER JOIN TBL_SISTEMAS S ON a.ID_SISTEMA = s.ID_SISTEMA");
            hql.Append(" WHERE S.CD_SISTEMA = :codigoSistema ");
            hql.Append(" AND a.DT_FIM_SESSAO IS NOT NULL ");
            hql.Append(" AND DATE(a.DT_INICIO_SESSAO) BETWEEN :inicio AND :fim ");
            hql.Append(" GROUP BY ev.NM_FANTASIA ");

            IQuery query = Session.CreateSQLQuery(hql.ToString())
                .AddScalar("EmpresaVenda", NHibernateUtil.AnsiString)
                .AddScalar("TempoMedio", NHibernateUtil.AnsiString)
                .SetResultTransformer(Transformers.AliasToBean<TempoMedioAcessoDiarioEvDTO>())
                .SetParameter("codigoSistema", "POR1818")
                .SetParameter("inicio", filtro.DataInicio)
                .SetParameter("fim", filtro.DataFim);

            return query.List<TempoMedioAcessoDiarioEvDTO>().AsQueryable();
        }

        public IQueryable<AcessoDiarioRegionalEvDTO> AcessosDiariosPorRegionalEv(FiltroPeriodoDTO filtro)
        {

            StringBuilder hql = new StringBuilder();
            hql.Append("SELECT ev.Estado AS \"Regional\", ");
            hql.Append(" ev.NomeFantasia AS \"EmpresaVenda\", ");
            hql.Append(" DATE(a.InicioSessao) AS \"InicioSessao\", ");
            hql.Append(" COUNT(*) AS \"Quantidade\" FROM Acesso a ");
            hql.Append(" INNER JOIN Corretor c ON c.Usuario.Id = a.Usuario.Id ");
            hql.Append(" INNER JOIN EmpresaVenda ev ON ev.Id = c.EmpresaVenda.Id ");
            hql.Append(" WHERE a.Sistema.Codigo = :codigoSistema ");
            hql.Append(" AND DATE(a.InicioSessao) BETWEEN :inicio AND :fim ");
            hql.Append(" GROUP BY ev.Estado, ev.NomeFantasia, DATE(a.InicioSessao) ");


            IQuery query = Session.CreateQuery(hql.ToString())
                .SetResultTransformer(Transformers.AliasToBean<AcessoDiarioRegionalEvDTO>())
                .SetParameter("codigoSistema", "POR1818")
                .SetParameter("inicio", filtro.DataInicio)
                .SetParameter("fim", filtro.DataFim);


            return query.List<AcessoDiarioRegionalEvDTO>().AsQueryable();
        }

        public IQueryable<AcessoDiarioRegionalDTO> AcessosDiariosPorRegional(FiltroPeriodoDTO filtro)
        {

            StringBuilder hql = new StringBuilder();
            hql.Append("SELECT ev.Estado AS \"Regional\", ");
            hql.Append(" DATE(a.InicioSessao) AS \"InicioSessao\", ");
            hql.Append(" COUNT(*) AS \"Quantidade\" FROM Acesso a ");
            hql.Append(" INNER JOIN Corretor c ON c.Usuario.Id = a.Usuario.Id ");
            hql.Append(" INNER JOIN EmpresaVenda ev ON ev.Id = c.EmpresaVenda.Id ");
            hql.Append(" WHERE a.Sistema.Codigo = :codigoSistema ");
            hql.Append(" AND DATE(a.InicioSessao) BETWEEN :inicio AND :fim ");
            hql.Append(" GROUP BY ev.Estado, DATE(a.InicioSessao) ");


            IQuery query = Session.CreateQuery(hql.ToString())
                .SetResultTransformer(Transformers.AliasToBean<AcessoDiarioRegionalDTO>())
                .SetParameter("codigoSistema", "POR1818")
                .SetParameter("inicio", filtro.DataInicio)
                .SetParameter("fim", filtro.DataFim)
                .SetTimeout(30000);


            return query.List<AcessoDiarioRegionalDTO>().AsQueryable();
        }

        private byte[] ExportarAcessosDiarios(DataSourceRequest request, FiltroPeriodoDTO filtro)
        {
            var results = AcessosDiarios(filtro);

            IList<string> header = new List<string>
            {
                GlobalMessages.InicioSessao,
                GlobalMessages.Quantidade
            };

            ExcelUtil excel = ExcelUtil.NewInstance(23)
                .NewSheet(DateTime.Now.ToString("yyyyMMddHHmmss"))
                .WithHeader(header.ToArray());

            foreach (var model in results.ToDataRequest(request).records.ToList())
            {
                excel
                    .CreateCellValue(model.InicioSessao.ToDate()).Width(20)
                    .CreateCellValue(model.Quantidade).Width(20)
                    ;
            }
            excel.Close();
            return excel.DownloadFile();
        }

        private byte[] ExportarAcessosDiariosPorEv(DataSourceRequest request, FiltroPeriodoDTO filtro)
        {
            var results = AcessosDiariosPorEv(filtro);

            IList<string> header = new List<string>
            {
                GlobalMessages.EmpresaVenda,
                GlobalMessages.InicioSessao,
                GlobalMessages.Quantidade
            };

            ExcelUtil excel = ExcelUtil.NewInstance(23)
                .NewSheet(DateTime.Now.ToString("yyyyMMddHHmmss"))
                .WithHeader(header.ToArray());

            foreach (var model in results.ToDataRequest(request).records.ToList())
            {
                excel
                    .CreateCellValue(model.EmpresaVenda).Width(40)
                    .CreateCellValue(model.InicioSessao.ToDate()).Width(20)
                    .CreateCellValue(model.Quantidade).Width(20)
                    ;
            }
            excel.Close();
            return excel.DownloadFile();
        }
        private byte[] ExportarAcessosCorretorEv(DataSourceRequest request, FiltroPeriodoDTO filtro)
        {
            var results = AcessosCorretorEv(filtro);

            IList<string> header = new List<string>
            {

                GlobalMessages.Corretor,
                GlobalMessages.EmpresaVenda,
                GlobalMessages.InicioSessao,
                GlobalMessages.FimSessao
            };

            ExcelUtil excel = ExcelUtil.NewInstance(23)
                .NewSheet(DateTime.Now.ToString("yyyyMMddHHmmss"))
                .WithHeader(header.ToArray());

            foreach (var model in results.ToDataRequest(request).records.ToList())
            {
                excel
                    .CreateCellValue(model.Corretor).Width(40)
                    .CreateCellValue(model.EmpresaVenda).Width(40)
                    .CreateCellValue(model.InicioSessao.ToDateTime()).Width(20)
                    .CreateCellValue(model.FimSessao.ToDateTime()).Width(20)
                    ;
            }
            excel.Close();
            return excel.DownloadFile();
        }

        private byte[] ExportarTempoMedioAcessosDiariosPorCorretorEv(DataSourceRequest request, FiltroPeriodoDTO filtro)
        {
            var results = TempoMedioAcessosDiariosPorCorretorEv(filtro);

            IList<string> header = new List<string>
            {
                GlobalMessages.Corretor,
                GlobalMessages.EmpresaVenda,
                GlobalMessages.TempoMedio

            };

            ExcelUtil excel = ExcelUtil.NewInstance(23)
                .NewSheet(DateTime.Now.ToString("yyyyMMddHHmmss"))
                .WithHeader(header.ToArray());

            foreach (var model in results.ToDataRequest(request).records.ToList())
            {
                excel
                    .CreateCellValue(model.Corretor).Width(40)
                    .CreateCellValue(model.EmpresaVenda).Width(40)
                    .CreateCellValue(model.TempoMedio).Width(20);
            }
            excel.Close();
            return excel.DownloadFile();
        }
        private byte[] ExportarTempoMedioAcessosDiariosEv(DataSourceRequest request, FiltroPeriodoDTO filtro)
        {
            var results = TempoMedioAcessosDiariosEv(filtro);

            IList<string> header = new List<string>
            {
                GlobalMessages.EmpresaVenda,
                GlobalMessages.TempoMedio

            };

            ExcelUtil excel = ExcelUtil.NewInstance(23)
                .NewSheet(DateTime.Now.ToString("yyyyMMddHHmmss"))
                .WithHeader(header.ToArray());

            foreach (var model in results.ToDataRequest(request).records.ToList())
            {
                excel
                    .CreateCellValue(model.EmpresaVenda).Width(40)
                    .CreateCellValue(model.TempoMedio).Width(20);
            }
            excel.Close();
            return excel.DownloadFile();
        }
        private byte[] ExportarAcessosDiariosPorRegionalEv(DataSourceRequest request, FiltroPeriodoDTO filtro)
        {
            var results = AcessosDiariosPorRegionalEv(filtro);

            IList<string> header = new List<string>
            {
                GlobalMessages.Regional,
                GlobalMessages.EmpresaVenda,
                GlobalMessages.InicioSessao,
                GlobalMessages.Quantidade

            };

            ExcelUtil excel = ExcelUtil.NewInstance(23)
                .NewSheet(DateTime.Now.ToString("yyyyMMddHHmmss"))
                .WithHeader(header.ToArray());

            foreach (var model in results.ToDataRequest(request).records.ToList())
            {
                excel
                    .CreateCellValue(model.Regional).Width(20)
                    .CreateCellValue(model.EmpresaVenda).Width(40)
                    .CreateCellValue(model.InicioSessao.ToDate()).Width(20)
                    .CreateCellValue(model.Quantidade).Width(20)
                    ;
            }
            excel.Close();
            return excel.DownloadFile();
        }

        private byte[] ExportarAcessosDiariosPorRegional(DataSourceRequest request, FiltroPeriodoDTO filtro)
        {
            var results = AcessosDiariosPorRegional(filtro);

            IList<string> header = new List<string>
            {
                GlobalMessages.Regional,
                GlobalMessages.InicioSessao,
                GlobalMessages.Quantidade

            };

            ExcelUtil excel = ExcelUtil.NewInstance(23)
                .NewSheet(DateTime.Now.ToString("yyyyMMddHHmmss"))
                .WithHeader(header.ToArray());

            foreach (var model in results.ToDataRequest(request).records.ToList())
            {
                excel
                    .CreateCellValue(model.Regional).Width(40)
                    .CreateCellValue(model.InicioSessao.ToDate()).Width(20)
                    .CreateCellValue(model.Quantidade).Width(20)
                    ;
            }
            excel.Close();
            return excel.DownloadFile();
        }

        public byte[] ExportarRelatorios(DataSourceRequest request, FiltroPeriodoDTO filtro)
        {


            var outputMemStream = new MemoryStream();
            var zipOutputStream = new ZipOutputStream(outputMemStream);
            zipOutputStream.SetLevel(3);

            var relatorio1 = ExportarAcessosDiarios(request, filtro);

            var memoryStream = new MemoryStream(relatorio1);
            var entry = new ZipEntry("Acessos_Diarios.Xlsx");
            entry.IsUnicodeText = true;
            entry.DateTime = DateTime.Now;
            zipOutputStream.PutNextEntry(entry);
            StreamUtils.Copy(memoryStream, zipOutputStream, new byte[4096]);
            zipOutputStream.CloseEntry();

            var relatorio2 = ExportarAcessosDiariosPorEv(request, filtro);

            memoryStream = new MemoryStream(relatorio2);
            entry = new ZipEntry("Acessos_Diarios_Por_Ev.Xlsx");
            entry.IsUnicodeText = true;
            entry.DateTime = DateTime.Now;
            zipOutputStream.PutNextEntry(entry);
            StreamUtils.Copy(memoryStream, zipOutputStream, new byte[4096]);
            zipOutputStream.CloseEntry();

            var relatorio3 = ExportarTempoMedioAcessosDiariosEv(request, filtro);
            memoryStream = new MemoryStream(relatorio3);
            entry = new ZipEntry("Tempo_Medio_Acessos_Diarios_Ev.Xlsx");
            entry.IsUnicodeText = true;
            entry.DateTime = DateTime.Now;
            zipOutputStream.PutNextEntry(entry);
            StreamUtils.Copy(memoryStream, zipOutputStream, new byte[4096]);
            zipOutputStream.CloseEntry();

            var relatorio4 = ExportarTempoMedioAcessosDiariosPorCorretorEv(request, filtro);
            memoryStream = new MemoryStream(relatorio4);
            entry = new ZipEntry("Tempo_Medio_Acessos_Diarios_Por_Corretor_Ev.Xlsx");
            entry.IsUnicodeText = true;
            entry.DateTime = DateTime.Now;
            zipOutputStream.PutNextEntry(entry);
            StreamUtils.Copy(memoryStream, zipOutputStream, new byte[4096]);
            zipOutputStream.CloseEntry();

            var relatorio5 = ExportarAcessosCorretorEv(request, filtro);
            memoryStream = new MemoryStream(relatorio5);
            entry = new ZipEntry("Acessos_Corretor_Ev.Xlsx");
            entry.IsUnicodeText = true;
            entry.DateTime = DateTime.Now;
            zipOutputStream.PutNextEntry(entry);
            StreamUtils.Copy(memoryStream, zipOutputStream, new byte[4096]);
            zipOutputStream.CloseEntry();



            var relatorio6 = ExportarAcessosDiariosPorRegional(request, filtro);
            memoryStream = new MemoryStream(relatorio6);
            entry = new ZipEntry("Acessos_Diarios_Por_Regional.Xlsx");
            entry.IsUnicodeText = true;
            entry.DateTime = DateTime.Now;
            zipOutputStream.PutNextEntry(entry);
            StreamUtils.Copy(memoryStream, zipOutputStream, new byte[4096]);
            zipOutputStream.CloseEntry();

            var relatorio7 = ExportarAcessosDiariosPorRegionalEv(request, filtro);

            memoryStream = new MemoryStream(relatorio7);
            entry = new ZipEntry("Acessos_Diarios_Por_Regional_Ev.Xlsx");
            entry.IsUnicodeText = true;
            entry.DateTime = DateTime.Now;
            zipOutputStream.PutNextEntry(entry);
            StreamUtils.Copy(memoryStream, zipOutputStream, new byte[4096]);
            zipOutputStream.CloseEntry();


            zipOutputStream.IsStreamOwner = false;
            zipOutputStream.Close();

            outputMemStream.Position = 0;

            return outputMemStream.ToArray();
        }
    }
}
