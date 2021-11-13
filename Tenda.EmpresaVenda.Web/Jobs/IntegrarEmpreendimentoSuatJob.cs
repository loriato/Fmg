using Europa.Extensions;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Repository;
using Tenda.Domain.Shared;
using Tenda.Domain.Shared.Log;
using Tenda.EmpresaVenda.Domain.Integration.Suat;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Web.Models;

namespace Tenda.EmpresaVenda.Web.Jobs
{
    public class IntegrarEmpreendimentoSuatJob : BaseJob
    {
        public SuatService suatService { get; set; }
        public EmpreendimentoRepository _empreendimentoRepository { get; set; }
        public EnderecoEmpreendimentoRepository _enderecoEmpreendimentoRepository { get; set; }
        public LogExecucaoRepository _logExecucaoRepository { get; set; }
        public EnderecoEstabelecimentoRepository _enderecoEstabelecimentoRepository { get; set; }
        public RegionaisRepository _regionaisRepository { get; set; }


        protected override void Init()
        {
            suatService = new SuatService();
            _empreendimentoRepository = new EmpreendimentoRepository(_session);
            _empreendimentoRepository._session = _session;
            _enderecoEmpreendimentoRepository = new EnderecoEmpreendimentoRepository(_session);
            _enderecoEmpreendimentoRepository._session = _session;
            _logExecucaoRepository = new LogExecucaoRepository(_session);
            _logExecucaoRepository._session = _session;
            _enderecoEstabelecimentoRepository = new EnderecoEstabelecimentoRepository(_session);
            _enderecoEstabelecimentoRepository._session = _session;
            _regionaisRepository = new RegionaisRepository();
            _regionaisRepository._session = _session;
        }

        public override void Process()
        {
            try
            {
                WriteLog(TipoLog.Informacao, "Iniciando busca de empreendimentos no Suat.");

                var modificador = ProjectProperties.DiasBuscaIntegracaoEmpreendimentoSuat;
                if (modificador == 0)
                {
                    modificador = 7;
                }
                else if (modificador < 0)
                {
                    modificador *= -1;
                }

                var dataMinima = DateTime.Today.AddDays(-modificador);

                var result = suatService.IntegrarEmpreendimento(dataMinima);
                if (result.IsEmpty())
                {
                    WriteLog(TipoLog.Informacao, "Empreendimentos Atualizados.");
                }

                ITransaction transaction = null;
                foreach (EmpreendimentoDTO empdto in result)
                {
                    bool teveAtualizacao = false;

                    transaction = _session.BeginTransaction();
                    WriteLog(TipoLog.Informacao, string.Format("Verificando Empreendimento {0}.", empdto.Nome));
                    Empreendimento empreendimento = _empreendimentoRepository.BuscarPorDivisao(empdto.Divisao.ToUpper().Trim());
                    if (empreendimento.HasValue())
                    {
                        var regionalObjeto = _regionaisRepository.findByName(empdto.NomeRegional);
                        if (regionalObjeto.IsEmpty())
                        {
                            regionalObjeto.Nome = empdto.NomeRegional;
                            _regionaisRepository.Save(regionalObjeto);
                        }
                        empreendimento.Nome = empdto.Nome;
                        empreendimento.IdSuat = empdto.Id;
                        empreendimento.Divisao = empdto.Divisao.ToUpper().Trim();
                        empreendimento.DisponivelParaVenda = empdto.DisponivelVenda;
                        empreendimento.Regional = empdto.NomeRegional;
                        empreendimento.RegionalObjeto = regionalObjeto;
                        empreendimento.CodigoEmpresa = empdto.CodigoEmpresa;
                        empreendimento.NomeEmpresa = empdto.NomeEmpresa;
                        empreendimento.CNPJ = empdto.Cnpj;
                        empreendimento.RegistroIncorporacao = empdto.RegistroIncorporacao;
                        empreendimento.DataLancamento = empdto.DataLancamento;
                        empreendimento.PrevisaoEntrega = empdto.PrevisaoEntrega;
                        empreendimento.DataEntrega = empdto.DataEntrega;

                        EnderecoEmpreendimento enderecoempreedimento = _enderecoEmpreendimentoRepository.FindByEmpreendimento(empreendimento.Id);
                        enderecoempreedimento.Cidade = empdto.Cidade;
                        enderecoempreedimento.Logradouro = empdto.Logradouro;
                        enderecoempreedimento.Bairro = empdto.Bairro;
                        enderecoempreedimento.Numero = empdto.Numero;
                        enderecoempreedimento.Cep = empdto.Cep;
                        enderecoempreedimento.Complemento = empdto.Complemento;
                        enderecoempreedimento.Estado = empdto.Estado;
                        _empreendimentoRepository.Save(empreendimento);
                        enderecoempreedimento.Empreendimento = empreendimento;
                        _enderecoEmpreendimentoRepository.Save(enderecoempreedimento);

                        EnderecoEstabelecimento enderecoEstabelecimento = _enderecoEstabelecimentoRepository.FindByEmpreendimento(empreendimento.Id);
                        if (enderecoEstabelecimento.IsEmpty())
                        {
                            enderecoEstabelecimento = new EnderecoEstabelecimento();
                        }
                        enderecoEstabelecimento.Cidade = empdto.CidadeEmpresa;
                        enderecoEstabelecimento.Logradouro = empdto.LogradouroEmpresa;
                        enderecoEstabelecimento.Bairro = empdto.BairroEmpresa;
                        enderecoEstabelecimento.Numero = empdto.NumeroEmpresa;
                        enderecoEstabelecimento.Cep = empdto.CepEmpresa;
                        enderecoEstabelecimento.Complemento = empdto.ComplementoEmpresa;
                        enderecoEstabelecimento.Estado = empdto.EstadoEmpresa;
                        enderecoEstabelecimento.Empreendimento = empreendimento;
                        _enderecoEstabelecimentoRepository.Save(enderecoEstabelecimento);


                        teveAtualizacao = true;
                    }
                    else
                    {
                        WriteLog(TipoLog.Informacao, string.Format("Inserindo Empreendimento {0}.", empdto.Nome));
                        Empreendimento novoEmpreendimento = new Empreendimento();
                        var regionalObjeto = _regionaisRepository.findByName(empdto.NomeRegional);
                        if (regionalObjeto.IsEmpty())
                        {
                            regionalObjeto = new Regionais();
                            regionalObjeto.Nome = empdto.NomeRegional;
                            _regionaisRepository.Save(regionalObjeto);

                        }
                        novoEmpreendimento.Nome = empdto.Nome;
                        novoEmpreendimento.IdSuat = empdto.Id;
                        novoEmpreendimento.Divisao = empdto.Divisao.ToUpper().Trim();
                        novoEmpreendimento.DisponivelParaVenda = empdto.DisponivelVenda;
                        novoEmpreendimento.DisponivelCatalogo = empdto.DisponivelCatalogo;
                        novoEmpreendimento.RegionalObjeto = regionalObjeto;
                        novoEmpreendimento.Regional = empdto.NomeRegional;
                        novoEmpreendimento.CodigoEmpresa = empdto.CodigoEmpresa;
                        novoEmpreendimento.NomeEmpresa = empdto.NomeEmpresa;
                        novoEmpreendimento.CNPJ = empdto.Cnpj;
                        novoEmpreendimento.RegistroIncorporacao = empdto.RegistroIncorporacao;
                        novoEmpreendimento.DataLancamento = empdto.DataLancamento;
                        novoEmpreendimento.PrevisaoEntrega = empdto.PrevisaoEntrega;
                        novoEmpreendimento.DataEntrega = empdto.DataEntrega;
                        novoEmpreendimento.PriorizarRegraComissao = false;
                        novoEmpreendimento.ModalidadeComissao = TipoModalidadeComissao.Fixa;
                        novoEmpreendimento.ModalidadeProgramaFidelidade = TipoModalidadeProgramaFidelidade.Fixa;

                        EnderecoEmpreendimento novoEnderecoEmpreedimento = new EnderecoEmpreendimento();
                        novoEnderecoEmpreedimento.Cidade = empdto.Cidade;
                        novoEnderecoEmpreedimento.Logradouro = empdto.Logradouro;
                        novoEnderecoEmpreedimento.Bairro = empdto.Bairro;
                        novoEnderecoEmpreedimento.Numero = empdto.Numero;
                        novoEnderecoEmpreedimento.Cep = empdto.Cep;
                        novoEnderecoEmpreedimento.Complemento = empdto.Complemento;
                        novoEnderecoEmpreedimento.Estado = empdto.Estado;
                        _empreendimentoRepository.Save(novoEmpreendimento);
                        novoEnderecoEmpreedimento.Empreendimento = novoEmpreendimento;
                        _enderecoEmpreendimentoRepository.Save(novoEnderecoEmpreedimento);

                        EnderecoEstabelecimento enderecoEstabelecimento = new EnderecoEstabelecimento();
                        enderecoEstabelecimento.Cidade = empdto.CidadeEmpresa;
                        enderecoEstabelecimento.Logradouro = empdto.LogradouroEmpresa;
                        enderecoEstabelecimento.Bairro = empdto.BairroEmpresa;
                        enderecoEstabelecimento.Numero = empdto.NumeroEmpresa;
                        enderecoEstabelecimento.Cep = empdto.CepEmpresa;
                        enderecoEstabelecimento.Complemento = empdto.ComplementoEmpresa;
                        enderecoEstabelecimento.Estado = empdto.EstadoEmpresa;
                        enderecoEstabelecimento.Empreendimento = novoEmpreendimento;
                        _enderecoEstabelecimentoRepository.Save(enderecoEstabelecimento);
                    }
                    transaction.Commit();
                    if (teveAtualizacao)
                    {
                        WriteLog(TipoLog.Informacao, string.Format("Dados do Empreendimento {0} atualizados.", empdto.Nome));
                    }
                    else
                    {
                        WriteLog(TipoLog.Informacao, string.Format("Dados do Empreendimento {0} inseridos.", empdto.Nome));
                    }
                }
                WriteLog(TipoLog.Informacao, "Empreendimentos Atualizados.");
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
                WriteLog(TipoLog.Erro, String.Format("Erro ao integrar Empreendimentos do Suat: {0}.", e.Message));
            }
        }
    }
}