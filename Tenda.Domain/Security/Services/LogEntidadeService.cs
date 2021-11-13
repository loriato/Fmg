using Europa.Data.Model;
using Europa.Extensions;
using Europa.Resources;
using NHibernate;
using NHibernate.Metadata;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.Core.Services;
using Tenda.Domain.Security.Repository;
using Tenda.Domain.Security.Services.Models;
using Tenda.Domain.Security.Views;

namespace Tenda.Domain.Security.Services
{
    public class LogEntidadeService : BaseService
    {
        private static string QueryControlAttributes = "SELECT {1} AS Id " +
            ", DT_CRIADO_EM AS " + AuditUtil.PROPERTY_CREATED_AT +
            ", ID_CRIADO_POR AS " + AuditUtil.PROPERTY_CREATED_BY +
            ", DT_ATUALIZADO_EM AS " + AuditUtil.PROPERTY_UPDATED_AT +
            ", ID_ATUALIZADO_POR AS " + AuditUtil.PROPERTY_UPDATED_BY +
            " FROM {0} WHERE {1} = :primaryKey";

        public ViewLogEntidadeRepository _logEntidadeRepository { get; set; }
        public UsuarioRepository _usuarioRepository { get; set; }

        public ControlBaseEntity SelectControlAttributes(string targetClass, long primaryKey)
        {
            IClassMetadata classMetadata = Session.SessionFactory.GetClassMetadata(targetClass);
            var tableName = classMetadata.GetType().GetProperty("TableName").GetValue(classMetadata, null);
            var keyColumnNames = classMetadata.GetType().GetProperty("KeyColumnNames").GetValue(classMetadata, null);
            var keyColumn = ((string[])keyColumnNames)[0];

            string currentSqlQuery = String.Format(QueryControlAttributes, tableName.ToString(), keyColumn.ToString());

            IQuery query = Session.CreateSQLQuery(currentSqlQuery)
                .AddScalar("Id", NHibernateUtil.Int64)
                .AddScalar("CriadoEm", NHibernateUtil.DateTime)
                .AddScalar("CriadoPor", NHibernateUtil.Int64)
                .AddScalar("AtualizadoEm", NHibernateUtil.DateTime)
                .AddScalar("AtualizadoPor", NHibernateUtil.Int64)
                .SetResultTransformer(Transformers.AliasToBean<ControlBaseEntity>())
                .SetInt64("primaryKey", primaryKey);

            var metadata = (ControlBaseEntity)query.UniqueResult();

            metadata.NomeCriadoPor = "<<sistema>>";
            metadata.NomeAtualizadoPor = "<<sistema>>";

            if (metadata.CriadoPor.IsEmpty() == false)
            {
                string name = _usuarioRepository.Queryable().Where(reg => reg.Id == metadata.CriadoPor)
                    .Select(reg => reg.Nome).SingleOrDefault();
                if (name.IsEmpty() == false)
                {
                    metadata.NomeCriadoPor = name;
                }
            }

            if (metadata.AtualizadoPor.IsEmpty() == false)
            {
                string name = _usuarioRepository.Queryable().Where(reg => reg.Id == metadata.AtualizadoPor)
                    .Select(reg => reg.Nome).SingleOrDefault();
                if (name.IsEmpty() == false)
                {
                    metadata.NomeAtualizadoPor = name;
                }
            }


            return metadata;
        }

        public DataSourceResponse<ViewLogEntidade> Listar(DataSourceRequest request, LogEntidadeDTO filtro)
        {
            var data = _logEntidadeRepository.Queryable();

            if (!filtro.Entidade.IsEmpty())
            {
                data = data.Where(reg => reg.Entidade.ToLower().Equals(filtro.Entidade.ToLower()));
            }
            if (!filtro.ChavePrimaria.IsEmpty())
            {
                data = data.Where(reg => reg.ChavePrimaria == filtro.ChavePrimaria);
            }
            if (!filtro.IdUsuarioCriador.IsEmpty())
            {
                data = data.Where(reg => reg.CriadoPor == filtro.IdUsuarioCriador);
            }
            if (!filtro.IdUsuarioAtualizacao.IsEmpty())
            {
                data = data.Where(reg => reg.AtualizadoPor == filtro.IdUsuarioAtualizacao);
            }
            if (!filtro.DataInicio.IsEmpty())
            {
                data = data.Where(reg => reg.CriadoEm.Date >= filtro.DataInicio.Date);
            }
            if (!filtro.DataFim.IsEmpty())
            {
                data = data.Where(reg => reg.CriadoEm.Date <= filtro.DataFim.Date);
            }

            return data.ToDataRequest<ViewLogEntidade>(request);
        }

        public byte[] ExportarDatatable(DataSourceRequest request, LogEntidadeDTO filtro)
        {
            request.start = 0;
            request.pageSize = 0;
            return Exportar(request, filtro);
        }

        public byte[] ExportarPagina(DataSourceRequest request, LogEntidadeDTO filtro)
        {
            return Exportar(request, filtro);
        }

        private byte[] Exportar(DataSourceRequest request, LogEntidadeDTO filtro)
        {
            ExcelUtil excel = ExcelUtil.NewInstance(20)
                .NewSheet(DateTime.Now.ToString("yyyyMMddHHmmss"))
                .WithHeader(GetHeader());

            var list = Listar(request, filtro);

            string dateCol = "dd/MM/yyyy HH:mm:ss";
            foreach (var model in list.records.ToList())
            {
                excel.CreateCellValue(model.Entidade)
                    .CreateCellValue(model.ChavePrimaria)
                    .CreateCellValue(model.Conteudo).Width(30)
                    .CreateCellValue(model.CriadoPor)
                    .CreateCellValue(model.CriadoEm).Format(dateCol).Width(30)
                    .CreateCellValue(model.AtualizadoPor)
                    .CreateCellValue(model.AtualizadoEm).Format(dateCol).Width(30);
            }
            excel.Close();
            return excel.DownloadFile();
        }

        private string[] GetHeader()
        {
            IList<string> header = new List<string>
            {
                GlobalMessages.Entidade,
                GlobalMessages.ChavePrimaria,
                GlobalMessages.Conteudo,
                GlobalMessages.CriadoPor,
                GlobalMessages.CriadoEm,
                GlobalMessages.AtualizadoPor,
                GlobalMessages.AtualizadoEm,
            };
            return header.ToArray();
        }

    }
}
