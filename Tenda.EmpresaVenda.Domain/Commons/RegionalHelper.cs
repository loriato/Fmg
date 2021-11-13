using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Europa.Extensions;
using NHibernate;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.Domain.Data;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Commons
{
    public static class RegionalHelper
    {
        private static DateTime _lastUpdate = DateTime.MinValue;

        // in Minutes
        private static int TimeToLive = 30;

        private static IList<string> _regionaisMatriz = new List<string>();

        public static IList<string> RegionaisMatriz
        {
            get
            {
                bool shouldRefresh = (DateTime.Now - _lastUpdate).TotalMinutes > TimeToLive;

                if (shouldRefresh || _regionaisMatriz.IsEmpty())
                {
                    lock (_regionaisMatriz)
                    {
                        _regionaisMatriz = GetDataMatriz();
                    }
                }

                return _regionaisMatriz;
            }
        }

        public static List<SelectListItem> RegionaisMatrizSelectListItem =>
            RegionaisMatriz.Select(reg => new SelectListItem() {Value = reg, Text = reg}).ToList();

        private static IList<string> GetDataMatriz()
        {
            List<string> regionais = new List<string>();

            ISession session = null;
            try
            {
                session = NHibernateSession.Session();
                session.FlushMode = FlushMode.Manual;
                var empresaVendaRepository = new EmpresaVendaRepository();
                empresaVendaRepository._session = session;
                var enderecoEmpreendimentoRepository = new EnderecoEmpreendimentoRepository();
                enderecoEmpreendimentoRepository._session = session;
                regionais = empresaVendaRepository.RegionaisDasEmpresasVendas();
                var regionaisEmpresas = enderecoEmpreendimentoRepository.RegionaisDosEmpreendimentos();
                regionais = regionais.Where(x => regionaisEmpresas.Contains(x)).ToList();
            }
            catch (Exception err)
            {
                ExceptionLogger.LogException(err);
            }
            finally
            {
                if (session != null && session.IsOpen)
                {
                    session.Close();
                }
            }

            return regionais;
        }
    }
}