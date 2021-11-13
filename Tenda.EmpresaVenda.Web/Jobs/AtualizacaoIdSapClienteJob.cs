using Europa.Commons;
using Europa.Extensions;
using Newtonsoft.Json;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Repository;
using Tenda.Domain.Shared;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.Domain.Integration.Suat;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Web.Jobs
{
    public class AtualizacaoIdSapClienteJob : BaseJob
    {
        public SuatService suatService { get; set; }
        public ClienteRepository _clienteRepository { get; set; }
        public LogExecucaoRepository _logExecucaoRepository { get; set; }

        protected override void Init()
        {
            suatService = new SuatService();
            _clienteRepository = new ClienteRepository();
            _clienteRepository._session = _session;
            _logExecucaoRepository = new LogExecucaoRepository(_session);
            _logExecucaoRepository._session = _session;
        }

        public override void Process()
        {
            ITransaction transaction = null;
            try
            {
                var dataMinima = ProjectProperties.DataBuscaAtualizacoesIdSapCliente;

                if (DateTime.MinValue.Equals(dataMinima))
                {
                    var modificador = ProjectProperties.DiasBuscaAtualizacoesIdSapCliente;
                    if (modificador == 0)
                    {
                        modificador = 7;
                    }
                    else if (modificador < 0)
                    {
                        modificador *= -1;
                    }

                    dataMinima = DateTime.Today.AddDays(-modificador);
                }

                var dataString = dataMinima.ToDateTimeSeconds();

                WriteLog(TipoLog.Informacao,
                    $"Buscando novas referências sap dos clientes a partir de [{dataString}].");

                var results = suatService.BuscarIdsSapClientes(null, dataMinima);

                transaction = _session.BeginTransaction();
                foreach (var item in results)
                {
                    var cliente = _clienteRepository.BuscarPorIdSuat(item.IdSuat);

                    if (cliente.HasValue())
                    {
                        cliente.IdSap = item.IdSap;
                        _clienteRepository.Save(cliente);
                    }
                }

                transaction.Commit();

                WriteLog(TipoLog.Informacao,
                    "Execução do robô de atualização das referências sap dos clientes finalizada com sucesso.");
            }
            catch (Exception e)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                }

                ExceptionLogger.LogException(e);
                WriteLog(TipoLog.Erro,
                    String.Format("Erro ao atualizar as referências sap dos clientes: {0}.", e.Message));
            }
        }
    }
}