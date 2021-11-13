using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using Europa.Data.Model;

namespace Europa.Extensions
{
    public static class ObjectExtensionMethods
    {
        /// <summary>
        ///     Verifica se o objeto é nulo.
        /// </summary>
        /// <param name="source">Objeto a ser verificado.</param>
        /// <returns>Retorna true se o valor for nulo, caso contrário retorna false.</returns>
        public static bool IsNull(this object source)
        {
            return source == null;
        }

        /// <summary>
        ///     Verifica se o objeto é nulo ou possui valores iniciais como Zero ou "".
        /// </summary>
        /// <param name="source">Objeto a ser verificado.</param>
        /// <returns>Retorna true se o valor for nulo ou possui valores iniciais, caso contrário retorna false.</returns>
        public static bool IsEmpty(this object source)
        {
            bool result = (source == null ||
                           source.Equals(0) ||
                           source.ToString().Equals("0") ||
                           (source is string && source.ToString().Trim().Equals(string.Empty)) ||
                           source.Equals(string.Empty) ||
                           (source is decimal && ((decimal)source).Equals(0)) ||
                           SqlDateTime.MinValue.Equals(source) ||
                           DateTime.MinValue.Equals(source)) ||
                            (source is BaseEntity && ((BaseEntity)source).Id <= 0) ||
                          (source is ICollection && ((ICollection)source).Count == 0) ||
                          (source is IQueryable && !((IQueryable<object>)source).Any()) ||
                          TimeSpan.Zero.Equals(source);

            if (!result)
            {
                Type collectionType =
                    source.GetType()
                        .GetInterfaces()
                        .SingleOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICollection<>));

                result = collectionType != null &&
                         ((int)collectionType.GetProperty("Count").GetValue(source, null)) == 0;
            }

            return result;
        }

        /// <summary>
        /// Retorna o oposto do método IsEmpty() - Facilita leitura de código
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool HasValue(this object source)
        {
            return !IsEmpty(source);
        }


        /// <summary>
        ///     Substitui o valor nulo com o valor especificado.
        /// </summary>
        /// <param name="source">Objeto a ser verificado.</param>
        /// <param name="result">Objeto a ser retornado caso o parâmetro source seja nulo.</param>
        /// <returns>Retorna o próprio objeto se o mesmo não for nulo, caso o contrário retorna o valor do parâmetro result.</returns>
        public static T IsNull<T>(this T source, T result)
        {
            return source != null ? source : result;
        }
    }
}