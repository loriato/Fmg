using System;
using System.Linq;
using Europa.Extensions;
using Europa.Resources;
using NHibernate;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Repository;
using Tenda.Domain.Shared;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.Domain.Integration.Suat;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Web.Jobs
{
    public class AtualizacaoBoletoContratoPrePropostaJob : BaseJob
    {
        public SuatService suatService { get; set; }
        public PrePropostaRepository _prePropostaRepository { get; set; }
        public BoletoPrePropostaRepository _boletoPrePropostaRepository { get; set; }
        public ContratoPrePropostaRepository _contratoPrePropostaRepository { get; set; }
        public NotificacaoRepository _notificacaoRepository { get; set; }
        public LogExecucaoRepository _logExecucaoRepository { get; set; }
        public PropostaSuatRepository _propostaSuatRepository { get; set; }
        protected override void Init()
        {
            suatService = new SuatService();
            _prePropostaRepository = new PrePropostaRepository();
            _prePropostaRepository._session = _session;
            _boletoPrePropostaRepository = new BoletoPrePropostaRepository();
            _boletoPrePropostaRepository._session = _session;
            _contratoPrePropostaRepository = new ContratoPrePropostaRepository();
            _contratoPrePropostaRepository._session = _session;
            _notificacaoRepository = new NotificacaoRepository();
            _notificacaoRepository._session = _session;
            _logExecucaoRepository = new LogExecucaoRepository(_session);
            _logExecucaoRepository._session = _session;
            _propostaSuatRepository = new PropostaSuatRepository();
            _propostaSuatRepository._session = _session;
        }

        public override void Process()
        {
            WriteLog(TipoLog.Informacao, "Buscando pré-propostas já integradas.");
            var idsPropostas = _prePropostaRepository.BuscarPrePropostasIntegradas().Select(reg => reg.IdSuat)
                .ToList();

            var dataMinima = ProjectProperties.DataBuscaBoletosPropostas;

            if (DateTime.MinValue.Equals(dataMinima))
            {
                var modificador = ProjectProperties.DiasBuscaBoletosPropostas;
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
                $"Buscando referência dos boletos e contratos a partir de [{dataString}].");

            var result = suatService.BuscarBoletoContratoPropostas(idsPropostas, dataMinima);
            if (result.IsEmpty())
            {
                WriteLog(TipoLog.Informacao, "Boletos e Contratos das Pré-Propostas atualizados.");
            }


            ITransaction transaction = null;
            foreach (var item in result)
            {
                bool teveAtualizacao = false;
                var preProposta = _prePropostaRepository.BuscarPorIdSuat(item.IdProposta);

                if (preProposta.IsEmpty())
                {
                    WriteLog(TipoLog.Erro, string.Format("Pré-Proposta não encontrada | ID SUAT: {0}", item.IdProposta));
                    continue;
                }

                var boleto = _boletoPrePropostaRepository.BuscarBoletoMaisRecente(preProposta.Id);
                var contrato = _contratoPrePropostaRepository.BuscarContratoMaisRecente(preProposta.Id);

                transaction = _session.BeginTransaction();
                WriteLog(TipoLog.Informacao,
                    string.Format("Verificando boleto e contrato da pré-proposta {0}.", preProposta.Codigo));
                if (!item.IdBoleto.IsEmpty() &&
                    (boleto.IsEmpty() || (!boleto.IsEmpty() && item.IdBoleto != boleto.IdBoletoSuat)))
                {
                    WriteLog(TipoLog.Informacao,
                        string.Format("Atualizando boleto da pré-proposta {0}.", preProposta.Codigo));
                    // Buscar boleto pra atualizar na EVS
                    var resultBoleto = suatService.DownloadBoleto(item.IdProposta, null);

                    BoletoPreProposta novoContrato = null;

                    if (resultBoleto.Sucesso)
                    {
                        novoContrato = new BoletoPreProposta();
                        novoContrato.PreProposta = preProposta;
                        novoContrato.Boleto = resultBoleto.Objeto.Bytes;
                        novoContrato.IdBoletoSuat = resultBoleto.Objeto.Id;

                        if (novoContrato.Boleto.IsEmpty())
                        {
                            var proposta = _propostaSuatRepository.Queryable()
                                .Where(x => x.PreProposta != null)
                                .Where(x => x.PreProposta.Id == preProposta.Id)
                                .SingleOrDefault();

                            if (proposta.HasValue())
                            {
                                WriteLog(TipoLog.Aviso,
                                string.Format("Falha na aquisição do boleto da proposta {0}.",
                                proposta.CodigoProposta));
                            }
                            else
                            {
                                WriteLog(TipoLog.Informacao,
                                string.Format("Pré-proposta {0} não possui referência de proposta.",
                                 preProposta.Codigo));
                            }
                            
                        }
                        else
                        {
                            _boletoPrePropostaRepository.Save(novoContrato);
                        }
                    }

                    teveAtualizacao = true;

                    var titulonotificacao = (novoContrato == null)
                        ? GlobalMessages.NotificacaoBoletoNovo_Titulo
                        : GlobalMessages.NotificacaoBoletoAtualizado_Titulo;

                    Notificacao notificacao = new Notificacao()
                    {
                        Titulo = string.Format(titulonotificacao, preProposta.Codigo, preProposta.Cliente.NomeCompleto),
                        Conteudo = GlobalMessages.NotificacaoBoleto_Conteudo,
                        Usuario = preProposta.Corretor.Usuario,
                        EmpresaVenda = preProposta.EmpresaVenda,
                        DestinoNotificacao = DestinoNotificacao.Portal,
                    };

                    _notificacaoRepository.Save(notificacao);
                }

                if (!item.IdContrato.IsEmpty() &&
                    (contrato.IsEmpty() || (!contrato.IsEmpty() && item.IdContrato != contrato.IdContratoSuat)))
                {
                    WriteLog(TipoLog.Informacao,
                        string.Format("Atualizando contrato da pré-proposta {0}.", preProposta.Codigo));
                    // Buscar contrato pra atualizar na EVS
                    var resultContrato = suatService.DownloadContrato(item.IdProposta, null);

                    ContratoPreProposta novoContrato = null;

                    if (resultContrato.Sucesso)
                    {
                        novoContrato = new ContratoPreProposta();
                        novoContrato.PreProposta = preProposta;
                        novoContrato.Contrato = resultContrato.Objeto.Bytes;
                        novoContrato.IdContratoSuat = resultContrato.Objeto.Id;
                        _contratoPrePropostaRepository.Save(novoContrato);
                    }

                    teveAtualizacao = true;

                    var titulonotificacao = (novoContrato == null)
                        ? GlobalMessages.NotificacaoContratoNovo_Titulo
                        : GlobalMessages.NotificacaoContratoAtualizado_Titulo;

                    Notificacao notificacao = new Notificacao()
                    {
                        Titulo = string.Format(titulonotificacao, preProposta.Codigo, preProposta.Cliente.NomeCompleto),
                        Conteudo = GlobalMessages.NotificacaoContrato_Conteudo,
                        Usuario = preProposta.Corretor.Usuario,
                        EmpresaVenda = preProposta.EmpresaVenda,
                        DestinoNotificacao = DestinoNotificacao.Portal,
                    };

                    _notificacaoRepository.Save(notificacao);
                }

                transaction.Commit();
                if (teveAtualizacao)
                {
                    WriteLog(TipoLog.Informacao,
                        string.Format("Dados da pré-proposta {0} atualizados.", preProposta.Codigo));
                }
            }

            WriteLog(TipoLog.Informacao, "Boletos e Contratos das Pré-Propostas atualizados.");
            
        }
    }
}