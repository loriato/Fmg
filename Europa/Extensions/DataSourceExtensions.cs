using System;
using System.Collections.Generic;
using System.Linq;

namespace Europa.Extensions
{
    public class DataSourceResponse<T>
    {
        public List<T> records { get; set; }
        public long total { get; set; }
        public long filtered { get; set; }
    }

    public class DataSourceRequest
    {
        public IList<ColParam> order { get; set; }
        public IList<Filter> filter { get; set; }
        public string search { get; set; }
        public string draw { get; set; }
        public int start { get; set; }
        public int pageSize { get; set; }
    }

    public class ColParam : Dictionary<string, string>
    {
        public string column
        {
            get { return this["column"]; }
        }

        public string value
        {
            get { return this["value"]; }
        }

        public bool IsAsc()
        {
            return value.ToLower() == "asc";
        }
    }

    public class Filter : Dictionary<string, string>
    {
        public string column
        {
            get { return this["column"]; }
        }

        public string value
        {
            get { return this["value"]; }
        }

        public bool regex
        {
            get { return bool.Parse(this["regex"]); }
        }
    }


    public static class DataTableExtensions
    {
        public static DataSourceRequest DefaultRequest()
        {
            DataSourceRequest request = new DataSourceRequest();
            request.filter = new List<Filter>();
            request.start = 0;
            request.pageSize = 10;
            request.draw = "2";
            return request;
        }

        //Nathalia - Clone para fins de dtos
        public static DataSourceResponse<T> CloneDataSourceResponse<A, T>(this DataSourceResponse<A> dataSourceResponse,
            List<T> records)
        {
            var response = new DataSourceResponse<T>();
            response.filtered = dataSourceResponse.filtered;
            response.total = dataSourceResponse.total;
            response.records = records;
            return response;
        }

        public static DataSourceResponse<T> ToDataRequest<T>(this IQueryable<T> source, DataSourceRequest request,
            Func<IQueryable<T>, string, string, IQueryable<T>> funcaoDeFiltro = null)
        {
            //Caso se passe uma funcao de filtro como parâmetro, filtrar o source (Funcao de Switch/Case - Service deve implementar para realizar os filtros)
            if (funcaoDeFiltro != null)
            {
                source = source.ApplyDataSourceFilters(request, funcaoDeFiltro);
            }

            var total = source.Count();
            if (request.order != null)
            {
                foreach (ColParam order in request.order)
                {
                    source = order.IsAsc() ? source.OrderBy(order.column) : source.OrderByDescending(order.column);
                }
            }

            //Hermann Miertschink Neto
            //Na interface, não está sendo exibido o dado que usa o filtered. No caso em questão, o filtered sempre é igual ao total.
            var filtered = total;
            source = source.Skip(request.start);
            if (request.pageSize > 0)
            {
                source = source.Take(request.pageSize);
            }

            return new DataSourceResponse<T> { records = source.ToList(), total = total, filtered = filtered };
        }

        /// <summary>
        /// Método de abstração de aplicação de filtros - O service deve implementar o parâmetro "filtroDoIquerybleEspecifico" para sua classe específica.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="request"></param>
        /// <param name="filtroDoIquerybleEspecifico"></param>
        /// <returns></returns>
        public static IQueryable<T> ApplyDataSourceFilters<T>(
            this IQueryable<T> source,
            DataSourceRequest request,
            Func<IQueryable<T>, string, string, IQueryable<T>> filtroDoIquerybleEspecifico)
        {
            foreach (var currentFilter in request.filter)
            {
                var chave = currentFilter.column;
                var valor = currentFilter.value;
                if (chave == null || valor == null || valor.IsEmpty()) continue;

                chave = chave.ToLower();
                valor = valor.ToLower();
                source = filtroDoIquerybleEspecifico(source, chave, valor);
            }

            return source;
        }
    }
}