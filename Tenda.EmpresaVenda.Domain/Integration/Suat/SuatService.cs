using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Integration.Suat.Models;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Web.Models;

namespace Tenda.EmpresaVenda.Domain.Integration.Suat
{
    public class SuatService : BaseSuatService
    {
        private static string _endpointEmpreendimentos = "/informacoesVenda/empreendimentosDisponiveis";
        private static string _endpointEmpresaVendaClienteValidar = "/evs/cliente/validar";
        private static string _endpointEmpresaVendaClienteIncluir = "/evs/cliente/incluir";
        private static string _endpointEmpresaVendaIntegrarPreProposta = "/evs/proposta/integrar";
        private static string _endpointEmpresaVendaLogPreProposta = "/evs/proposta/log";
        private static string _endpointEmpresaVendaBoletoPreProposta = "/evs/proposta/boleto";
        private static string _endpointEmpresaVendaContratoPreProposta = "/evs/proposta/contrato";
        private static string _endpointEmpresaVendaBoletoContratoPreProposta = "/evs/proposta/boletoContrato";
        private static string _endpointEmpresaVendaSituacaoProposta = "/evs/proposta/situacao";
        private static string _endpointEmpresaVendaBuscarProposta = "/evs/proposta/buscar";
        private static string _endpointEmpresaVendaBuscarPropostas = "/evs/proposta/buscarAtualizacao";
        private static string _endpointEmpresaVendaEstoqueEmpreendimento = "/evs/estoque/empreendimento";
        private static string _endpointEmpresaVendaEstoqueTorre = "/evs/estoque/torre";
        private static string _endpointEmpresaVendaEstoqueUnidade = "/evs/estoque/unidade";
        private static string _endpointEmpresaVendaLoja = "/evs/loja";
        private static string _endpointEmpreendimento = "/evs/empreendimento";
        private static string _endpointEmpresaVendaBuscarIdSapCliente = "/evs/cliente/buscarReferenciaSap";
        private static string _endpointEmpresaVendaAtualizarFilaUnificada = "/evs/proposta/atualizarFilaUnificada";

        public FilaUnificadaDTO AtualizarFilaUnificada(List<long> filas,long idUsuario)
        {
            int timeOut = 300 * 1000;
            var request = PrepareRequest(_endpointEmpresaVendaAtualizarFilaUnificada, Method.POST, new { Filas = filas, IdUsuario = idUsuario }, Token);
            request.Timeout = timeOut;
            request.ReadWriteTimeout = timeOut;
            var apiResponse = Client.Execute<FilaUnificadaDTO>(request);
            var response = JsonConvert.DeserializeObject<FilaUnificadaDTO>(apiResponse.Content);
            return response;
        }
        public List<long> EmpreendimentosHabilitadosParaVenda()
        {
            var request = new RestRequest(_endpointEmpreendimentos, Method.GET);
            request.RequestFormat = DataFormat.Json;
            request.JsonSerializer = new JsonNetSerializer();
            request.AddHeader("Authorization", Token);

            var apiResponse = Client.Execute<List<long>>(request);
            var response = JsonConvert.DeserializeObject<List<long>>(apiResponse.Content);

            return response;
        }

        public MensagemRetornoPropostaDTO ValidarCliente(ClienteSuatDTO cliente)
        {
            return Run<MensagemRetornoPropostaDTO>(_endpointEmpresaVendaClienteValidar, Method.POST, cliente);
        }

        public MensagemRetornoPropostaDTO IncluirCliente(ClienteSuatDTO cliente)
        {
            return Run<MensagemRetornoPropostaDTO>(_endpointEmpresaVendaClienteIncluir, Method.POST, cliente);
        }

        public MensagemRetornoPropostaDTO IntegrarPreProposta(PropostaDTO proposta)
        {
            int timeOut = 300 * 1000;
            var request = PrepareRequest(_endpointEmpresaVendaIntegrarPreProposta, Method.POST, proposta, Token);
            request.Timeout = timeOut;
            request.ReadWriteTimeout = timeOut;
            var apiResponse = Client.Execute<MensagemRetornoPropostaDTO>(request);
            var response = JsonConvert.DeserializeObject<MensagemRetornoPropostaDTO>(apiResponse.Content);
            return response;
        }

        public List<EstoqueEmpreendimentoDTO> EstoqueEmpreendimento(string divisao)
        {
            var url = _endpointEmpresaVendaEstoqueEmpreendimento + "?divisao=" + divisao;
            return Run<List<EstoqueEmpreendimentoDTO>>(url, Method.POST, null);
        }

        public List<TorreDTO> EstoqueTorre(string divisao)
        {
            var url = _endpointEmpresaVendaEstoqueTorre + "?divisao=" + divisao;
            return Run<List<TorreDTO>>(url, Method.POST, null);
        }

        public List<EstoqueUnidadeDTO> EstoqueUnidade(string divisao, string caracteristicas, DateTime previsaoEntrega,
            long idTorre)
        {
            var url = _endpointEmpresaVendaEstoqueUnidade + "?divisao=" + divisao + "&caracteristicas=" +
                      caracteristicas +
                      "&previsaoEntrega=" + previsaoEntrega.ToString("O") + "&idTorre=" + idTorre;
            return Run<List<EstoqueUnidadeDTO>>(url, Method.POST, null);
        }

        public List<LogIntegracaoPrePropostaDTO> LogIntegracao(string codigoPreProposta, long idProposta)
        {
            var url = _endpointEmpresaVendaLogPreProposta + "?idProposta=" + idProposta + "&codigoPreProposta=" +
                      codigoPreProposta;
            return Run<List<LogIntegracaoPrePropostaDTO>>(url, Method.POST, null);
        }

        public MensagemRetornoDownloadDTO DownloadBoleto(long idProposta, long? idBoleto)
        {
            var url = _endpointEmpresaVendaBoletoPreProposta + "?idProposta=" + idProposta + "&idBoleto=" + idBoleto;
            return Run<MensagemRetornoDownloadDTO>(url, Method.POST, null);
        }

        public MensagemRetornoDownloadDTO DownloadContrato(long idProposta, long? idContrato)
        {
            var url = _endpointEmpresaVendaContratoPreProposta + "?idProposta=" + idProposta + "&idContrato=" +
                      idContrato;
            return Run<MensagemRetornoDownloadDTO>(url, Method.POST, null);
        }

        public List<PropostaBoletoContratoDTO> BuscarBoletoContratoPropostas(List<long> idsProposta, DateTime? data)
        {
            int timeOut = 3000 * 1000;
            var request = PrepareRequest(_endpointEmpresaVendaBoletoContratoPreProposta, Method.POST, new { Data = data, IdsProposta = idsProposta }, Token);
            request.Timeout = timeOut;
            request.ReadWriteTimeout = timeOut;
            var apiResponse = Client.Execute<List<PropostaBoletoContratoDTO>>(request);
            apiResponse.Validate();
            var response = JsonConvert.DeserializeObject<List<PropostaBoletoContratoDTO>>(apiResponse.Content);
            return response;

        }

        public MensagemRetornoPropostaDTO BuscarSituacaoProposta(long idProposta)
        {
            var url = _endpointEmpresaVendaSituacaoProposta + "?idProposta=" + idProposta;
            return Run<MensagemRetornoPropostaDTO>(url, Method.POST, null);
        }

        public MensagemRetornoDTO BuscarProposta(long idProposta)
        {
            var url = _endpointEmpresaVendaBuscarProposta + "?idProposta=" + idProposta;
            return Run<MensagemRetornoDTO>(url, Method.POST, null);
        }

        public List<AtualizacaoPropostaDTO> BuscarPropostas(List<long> idsProposta, DateTime? data)
        {
            var request = PrepareRequest(_endpointEmpresaVendaBuscarPropostas, Method.POST,
                new { Data = data, IdsProposta = idsProposta }, Token);
            request.Timeout = 6000000;
            request.ReadWriteTimeout = 6000000;
            var apiResponse = Client.Execute<List<AtualizacaoPropostaDTO>>(request);
            apiResponse.Validate();
            return JsonConvert.DeserializeObject<List<AtualizacaoPropostaDTO>>(apiResponse.Content);
        }

        public List<AtualizacaoIdSapClienteDTO> BuscarIdsSapClientes(List<long> idsClientesSuat, DateTime? data)
        {
            var request = PrepareRequest(_endpointEmpresaVendaBuscarIdSapCliente, Method.POST,
                new {DataMinima = data, IdsClientes = idsClientesSuat}, Token);
            request.Timeout = 3000000;
            request.ReadWriteTimeout = 3000000;
            var apiResponse = Client.Execute<List<AtualizacaoIdSapClienteDTO>>(request);
            apiResponse.Validate();
            return JsonConvert.DeserializeObject<List<AtualizacaoIdSapClienteDTO>>(apiResponse.Content);
        }

        public List<LojaDTO> IntegrarLoja(DateTime? data)
        {
            var url = _endpointEmpresaVendaLoja;
            return Run<List<LojaDTO>>(url, Method.POST, new { Data = data });
        }

        public List<EmpreendimentoDTO> IntegrarEmpreendimento(DateTime? data)
        {
            var request = PrepareRequest(_endpointEmpreendimento, Method.POST,
                new { Data = data }, Token);
            request.Timeout = 3000000;
            request.ReadWriteTimeout = 3000000;
            var apiResponse = Client.Execute<List<EmpreendimentoDTO>>(request);
            apiResponse.Validate();
            return JsonConvert.DeserializeObject<List<EmpreendimentoDTO>>(apiResponse.Content);

        }
    }
}