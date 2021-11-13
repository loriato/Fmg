using Europa.Extensions;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Tenda.Domain.Core.Enums
{
    public static class EstadosSelectList
    {
        // InnerData
        private static List<SelectListItem> _domain = new List<SelectListItem>();
        private static List<SelectListItem> _domainWithEmpty = new List<SelectListItem>();

        // Concurrency control
        private static object _lock = new object();

        public static List<SelectListItem> ValuesWithEmpty {
            get
            {
                if (_domainWithEmpty.IsEmpty())
                {
                    _domainWithEmpty.Add(new SelectListItem { Text = "Todos", Value = "" });
                    _domainWithEmpty.AddRange(Values);
                }
                return _domainWithEmpty;
            }
        }
        public static List<SelectListItem> Values
        {
            get
            {

                if (_domain.IsEmpty())
                {
                    lock (_lock)
                    {
                        if (_domain.IsEmpty())
                        {
                            _domain.Add(new SelectListItem { Text = "AC", Value = "AC" });
                            _domain.Add(new SelectListItem { Text = "AL", Value = "AL" });
                            _domain.Add(new SelectListItem { Text = "AM", Value = "AM" });
                            _domain.Add(new SelectListItem { Text = "AP", Value = "AP" });
                            _domain.Add(new SelectListItem { Text = "BA", Value = "BA" });
                            _domain.Add(new SelectListItem { Text = "CE", Value = "CE" });
                            _domain.Add(new SelectListItem { Text = "DF", Value = "DF" });
                            _domain.Add(new SelectListItem { Text = "ES", Value = "ES" });
                            _domain.Add(new SelectListItem { Text = "GO", Value = "GO" });
                            _domain.Add(new SelectListItem { Text = "MA", Value = "MA" });
                            _domain.Add(new SelectListItem { Text = "MG", Value = "MG" });
                            _domain.Add(new SelectListItem { Text = "MS", Value = "MS" });
                            _domain.Add(new SelectListItem { Text = "MT", Value = "MT" });
                            _domain.Add(new SelectListItem { Text = "PA", Value = "PA" });
                            _domain.Add(new SelectListItem { Text = "PB", Value = "PB" });
                            _domain.Add(new SelectListItem { Text = "PE", Value = "PE" });
                            _domain.Add(new SelectListItem { Text = "PI", Value = "PI" });
                            _domain.Add(new SelectListItem { Text = "PR", Value = "PR" });
                            _domain.Add(new SelectListItem { Text = "RJ", Value = "RJ" });
                            _domain.Add(new SelectListItem { Text = "RN", Value = "RN" });
                            _domain.Add(new SelectListItem { Text = "RO", Value = "RO" });
                            _domain.Add(new SelectListItem { Text = "RR", Value = "RR" });
                            _domain.Add(new SelectListItem { Text = "RS", Value = "RS" });
                            _domain.Add(new SelectListItem { Text = "SC", Value = "SC" });
                            _domain.Add(new SelectListItem { Text = "SE", Value = "SE" });
                            _domain.Add(new SelectListItem { Text = "SP", Value = "SP" });
                            _domain.Add(new SelectListItem { Text = "TO", Value = "TO" });
                        }
                    }
                }
                return _domain;
            }
        }
    }
}
