using System;
using System.Globalization;

namespace Europa.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Formato 'dd/MM/yyyy HH:mm:ss'
        /// </summary>
        public static readonly string PatternDateTimeSeconds = "dd/MM/yyyy HH:mm:ss";
        public static readonly string PatternDateTime = "dd/MM/yyyy HH:mm";

        /// <summary>
        /// Formato 'dd/MM/yyyy'
        /// </summary>
        public static readonly string PatternDate = "dd/MM/yyyy";
        public static readonly string PatternTimeSeconds = "HH:mm:ss";
        public static readonly string PatternTime = "HH:mm";

        public static bool IsValid(this DateTime? date)
        {
            if (date == null)
            {
                return false;
            }
            if (date.Value == DateTime.MinValue)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Retorna a data no primeiro segundo do dia
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime AbsoluteStart(this DateTime date)
        {
            return date.Date;
        }

        /// <summary>
        /// Retorna a data no último segundo do dia
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime AbsoluteEnd(this DateTime date)
        {
            return AbsoluteStart(date).AddDays(1).AddTicks(-1);
        }

        /// <summary>
        /// Verifica se a data informada é um dia de 'trabalho'. Seg, Ter, Qua, Qui, Sex
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsUsefulDay(this DateTime date)
        {
            return !IsWeekEnd(date);
        }

        /// <summary>
        /// Verifica se a data informada é final de semana. Sab, Dom
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsWeekEnd(this DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday
                || date.DayOfWeek == DayOfWeek.Sunday;
        }

        /// <summary>
        /// Retorna da data no formato dd/MM/yyyy HH:mm:ss
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToDateTimeSeconds(this DateTime date)
        {
            return date.ToString(PatternDateTimeSeconds);
        }

        /// <summary>
        /// Retorna da data no formato dd/MM/yyyy HH:mm
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToDateTime(this DateTime date)
        {
            return date.ToString(PatternDateTime);
        }

        /// <summary>
        /// Converte strings no formato dd/MM/yyyy HH:mm
        /// </summary>
        /// <param name="dateString"></param>
        /// <returns></returns>
        public static DateTime FromDateTime(this string dateString)
        {
            return DateTime.ParseExact(dateString, PatternDateTime, CultureInfo.InvariantCulture, DateTimeStyles.None);
        }

        /// <summary>
        /// Converte strings no formato dd/MM/yyyy
        /// </summary>
        /// <param name="dateString"></param>
        /// <returns></returns>
        public static DateTime FromDate(this string dateString)
        {
            return DateTime.ParseExact(dateString, PatternDate, CultureInfo.InvariantCulture, DateTimeStyles.None);
        }

        /// <summary>
        /// Retorna da data no formato dd/MM/yyyy
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToDate(this DateTime date)
        {
            return date.ToString(PatternDate);
        }

        /// <summary>
        /// Retorna da data no formato HH:mm:ss
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToTimeSeconds(this DateTime date)
        {
            return date.ToString(PatternTimeSeconds);
        }

        /// <summary>
        /// Retorna da data no formato HH:mm
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToTime(this DateTime date)
        {
            return date.ToString(PatternTime);
        }
    }
}
