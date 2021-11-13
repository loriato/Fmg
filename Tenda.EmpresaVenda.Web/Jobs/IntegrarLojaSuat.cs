using Europa.Extensions;
using NHibernate;
using System;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Repository;
using Tenda.Domain.Shared;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.Domain.Integration.Suat;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Web.Jobs
{
    public class IntegrarLojaSuat : BaseJob
    {
        public SuatService suatService { get; set; }
        public LojaRepository _lojaRepository { get; set; }
        public LogExecucaoRepository _logExecucaoRepository { get; set; }
        public RegionaisRepository _regionaisRepository { get; set; }
        protected override void Init()
        {
            suatService = new SuatService();
            _lojaRepository = new LojaRepository(_session);
            _lojaRepository._session = _session;
            _logExecucaoRepository = new LogExecucaoRepository(_session);
            _logExecucaoRepository._session = _session;
            _regionaisRepository = new RegionaisRepository();
            _regionaisRepository._session = _session;
        }
        public override void Process()
        {
            try
            {
                WriteLog(TipoLog.Informacao, "Iniciando busca de lojas no Suat.");
                
                var modificador = ProjectProperties.DiasBuscaIntegracaoLojaSuat;
                if (modificador == 0)
                {
                    modificador = 7;
                }
                else if (modificador < 0)
                {
                    modificador *= -1;
                }

                var dataMinima = DateTime.Today.AddDays(-modificador);
                
                var result = suatService.IntegrarLoja(dataMinima);
                if (result.IsEmpty())
                {
                    WriteLog(TipoLog.Informacao, "Lojas Atualizadas.");
                }

                ITransaction transaction = null;
                foreach(var item in result)
                {
                    if (!item.IdSap.IsNull())
                    {
                        bool teveAtualizacao = false;
                        Loja novaLoja = new Loja();
                        var loja = _lojaRepository.FindByIdSap(item.IdSap);

                        //IdSap ToUpper
                        item.IdSap = item.IdSap.ToUpper().Trim();
                        if (!loja.IsNull())
                        {
                            loja.SapId = loja.SapId.ToUpper().Trim();
                        }

                        transaction = _session.BeginTransaction();
                        WriteLog(TipoLog.Informacao, string.Format("Verificando Loja {0}.", item.Nome));
                        var regional = _regionaisRepository.findByName(item.NomeRegional);
                        if (regional.IsEmpty())
                        {
                            regional = new Regionais();
                            regional.Nome = item.NomeRegional;
                            _regionaisRepository.Save(regional);
                        }

                        if (!item.IdSap.IsEmpty() && (loja.IsEmpty() || (!loja.IsEmpty() && item.IdSap != loja.SapId)))
                        {
                            WriteLog(TipoLog.Informacao, string.Format("Inserindo Loja {0}.", item.Nome));



                            novaLoja.Nome = item.Nome;
                            novaLoja.NomeFantasia = item.IdSap;
                            novaLoja.SapId = item.IdSap;
                            novaLoja.Regional = regional;
                            novaLoja.NomeRegional = regional.Nome;
                            novaLoja.DataIntegracao = DateTime.Now;
                            novaLoja.Situacao = Tenda.Domain.Core.Enums.Situacao.Ativo;
                            _lojaRepository.Save(novaLoja);

                        }
                        else
                        {
                            novaLoja = loja;
                            novaLoja.Nome = item.Nome;
                            novaLoja.NomeFantasia = item.IdSap;
                            novaLoja.Regional = regional;
                            novaLoja.NomeRegional = regional.Nome;
                            novaLoja.DataIntegracao = DateTime.Now;
                            novaLoja.Situacao = Tenda.Domain.Core.Enums.Situacao.Ativo;
                            _lojaRepository.Save(novaLoja);

                            teveAtualizacao = true;
                        }
                        transaction.Commit();

                        if (teveAtualizacao)
                        {
                            WriteLog(TipoLog.Informacao, string.Format("Dados da Loja {0} atualizados.", item.Nome));
                        }
                        else
                        {
                            WriteLog(TipoLog.Informacao, string.Format("Dados da Loja {0} inseridos.", item.Nome));
                        }
                    } 
                }

                WriteLog(TipoLog.Informacao, "Lojas Atualizadas.");
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
                WriteLog(TipoLog.Erro, String.Format("Erro ao integrar Lojas do Suat: {0}.", e.Message));
            }
        }
    }
}