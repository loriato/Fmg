using System;
using System.Collections.Generic;
using System.Linq;
using Europa.Commons;
using Europa.Extensions;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class ProponenteService : BaseService
    {
        private ProponenteRepository _proponenteRepository { get; set; }
        private ClienteRepository _clienteRepository { get; set; }
        private PrePropostaRepository _prePropostaRepository { get; set; }
        private DocumentoProponenteRepository _documentoProponenteRepository { get; set; }
        private ParecerDocumentoProponenteRepository _parecerDocumentoProponenteRepository { get; set; }

        public void Salvar(Proponente proponente, long idPreProposta, long idCliente)
        {
            var bre = new BusinessRuleException();

            var cliente = _clienteRepository.FindById(idCliente);
            _session.Evict(cliente);
            var preProposta = _prePropostaRepository.FindById(idPreProposta);
            _session.Evict(preProposta);

            proponente.Cliente = cliente;
            proponente.PreProposta = preProposta;
            proponente.RendaFormal = cliente.RendaFormal;
            proponente.RendaInformal = cliente.RendaInformal;

            ValidarProponente(proponente, bre);
            bre.ThrowIfHasError();

            var incluindo = proponente.Id == 0;

            _proponenteRepository.Save(proponente);

             var segundoProponente = _proponenteRepository.BuscarSegundoProponenteDaPreProposta(preProposta.Id);
            preProposta.FatorSocial = preProposta.Cliente.QuantidadeFilhos > 0 || segundoProponente != null;

            _prePropostaRepository.Save(preProposta);

            if (incluindo)
            {
                _proponenteRepository.Flush();
                AtualizarParticipacaoPreProposta(proponente.PreProposta);
            } 
        }

        public Proponente Excluir(long idProponente)
        {
            var proponente = _proponenteRepository.FindById(idProponente);
            var preProposta = _prePropostaRepository.FindById(proponente.PreProposta.Id);
            List<DocumentoProponente> documentoProponentes = _documentoProponenteRepository.BuscarDocumentosPorIdProponente(idProponente);
            
            var bre = new BusinessRuleException();

            if (proponente.Titular)
            {
                bre.AddError("").Complete();
                bre.ThrowIfHasError();
            }
            if (!documentoProponentes.IsEmpty())
            {
                foreach (DocumentoProponente obj in documentoProponentes)
                {
                    List<ParecerDocumentoProponente> parecerDocumentoProponentes = _parecerDocumentoProponenteRepository.BuscarParecerDocumentoPorDocumentoProponente(obj.Id);
                    if (!parecerDocumentoProponentes.IsEmpty())
                    {
                        foreach(ParecerDocumentoProponente par in parecerDocumentoProponentes)
                        {
                            _parecerDocumentoProponenteRepository.Delete(par);
                        }
                    }
                    _documentoProponenteRepository.Delete(obj);
                }
            }
            

            _proponenteRepository.Delete(proponente);
            _proponenteRepository.Flush();

            var segundoProponente = _proponenteRepository.BuscarSegundoProponenteDaPreProposta(preProposta.Id);
            preProposta.FatorSocial = preProposta.Cliente.QuantidadeFilhos > 0 || segundoProponente != null;

            _prePropostaRepository.Save(preProposta);

            AtualizarParticipacaoPreProposta(proponente.PreProposta);
            return proponente;
        }

        private void AtualizarParticipacaoPreProposta(PreProposta preProposta)
        {
            var list = _proponenteRepository.ProponentesDaPreProposta(preProposta.Id).ToList();
            var numProponentes = list.Count;

            if (numProponentes == 1)
            {
                var proponente = list[0];
                proponente.Participacao = 100;
                _proponenteRepository.Save(proponente);
            }
            else if (numProponentes > 1)
            {
                var porcentagemRestante = 100;
                var semRenda = list.Where(x => !PossuiRenda(x)).ToList();
                var comRenda = list.Where(x => PossuiRenda(x)).ToList();
                
                var principalPossuiRenda = comRenda.Any(x => x.Titular);
                var numParticipantes = comRenda.Count;

                List<Proponente> proponentesComApenasUmPorcento = semRenda;
                List<Proponente> proponentesComPorcentagemRestante = comRenda;
                
                if (numParticipantes == 0)
                {
                    numParticipantes = semRenda.Count;
                    proponentesComPorcentagemRestante = semRenda;
                    proponentesComApenasUmPorcento = new List<Proponente>();
                    principalPossuiRenda = true;
                }
                
                foreach (var proponente in proponentesComApenasUmPorcento)
                {
                    proponente.Participacao = 1;
                    porcentagemRestante--;
                    _proponenteRepository.Save(proponente);
                }

                var participacaoComRenda = (int) Math.Floor((decimal) porcentagemRestante / numParticipantes);
                var participacaoTitular = porcentagemRestante - participacaoComRenda * (numParticipantes - 1);

                for (var pos = 0; pos < numParticipantes; pos++)
                {
                    var proponente = proponentesComPorcentagemRestante[pos];
                    if ((principalPossuiRenda && proponente.Titular) ||
                        (!principalPossuiRenda && pos == 0))
                    {
                        proponente.Participacao = participacaoTitular;
                    }
                    else
                    {
                        proponente.Participacao = participacaoComRenda;
                    }
                    _proponenteRepository.Save(proponente);
                }
            }
        }

        private void ValidarProponente(Proponente proponente, BusinessRuleException bre)
        {
            // Realiza as validações de Proponente
            var enclResult = new ProponenteValidator(_proponenteRepository).Validate(proponente);

            // Verifica se retornou algum erro
            bre.WithFluentValidation(enclResult);
        }

        public bool PossuiRenda(Proponente proponente)
        {
            return proponente.RendaInformal > 0 || proponente.RendaFormal > 0;
        }
    }
}