using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Repository.Models;
using Tenda.EmpresaVenda.Domain.Services.Models;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class LeadEmpresaVendaService : BaseService
    {
        public LeadEmpresaVendaRepository _leadEmpresaVendaRepository { get; set; }
        public ViewLeadEmpresaVendaRepository _viewLeadEmpresaVendaRepository { get; set; }
        public NotificacaoRepository _notificacaoRepository { get; set; }
        public CorretorRepository _corretorRepository { get; set; }

        public void AtribuirCorretorLeadsLote(LeadDTO leads)
        {
            var bre = new BusinessRuleException();

            var validator = new LeadLoteValidator().Validate(leads);
            bre.WithFluentValidation(validator);
            bre.ThrowIfHasError();

            var atribuicoes = _leadEmpresaVendaRepository.FindByIds(leads.IdsLeadsEmpresasVendas);

            if (atribuicoes.IsEmpty())
            {
                bre.AddError("Leads Empresas de Vendas inválidos").Complete();
                bre.ThrowIfHasError();
            }

            try
            {
                foreach (var lead in atribuicoes)
                {
                    lead.Corretor = new Corretor { Id = leads.IdCorretor };
                    _leadEmpresaVendaRepository.Save(lead);
                }

                var primeiroLead = _leadEmpresaVendaRepository.CorretorPossuiLead(leads.IdCorretor);
                if (!primeiroLead)
                {
                    PrimeiraNotificacao(leads.IdCorretor);
                }
                else
                {
                    SegundaNotificacao(leads.IdCorretor);
                }

            }
            catch (Exception ex)
            {
                bre.AddError(ex.Message).Complete();
            }
            finally
            {
                bre.ThrowIfHasError();
            }
        }

        public void SalvarLeadEmpresaVenda(LeadEmpresaVenda leadEmpresaVenda)
        {
            var bre = new BusinessRuleException();

            var lead = _leadEmpresaVendaRepository.FindById(leadEmpresaVenda.Id);

            if (lead.IsEmpty())
            {
                lead = leadEmpresaVenda;
            }
            else
            {
                if (!leadEmpresaVenda.PreProposta.IsEmpty())
                {
                    lead.PreProposta = new PreProposta { Id = leadEmpresaVenda.PreProposta.Id };
                }
                else
                {
                    lead.PreProposta = null;
                }

                lead.Situacao = leadEmpresaVenda.Situacao;
                lead.Anotacoes = leadEmpresaVenda.Anotacoes;
                if (lead.Situacao != SituacaoLead.Desistencia)
                {
                    lead.Desistencia = 0;
                    lead.DescricaoDesistencia = null;
                }
                else
                {
                    lead.Desistencia = leadEmpresaVenda.Desistencia;
                    lead.DescricaoDesistencia = leadEmpresaVenda.DescricaoDesistencia;
                }
            }

            var validator = new LeadEmpresaVendaValidator().Validate(lead);
            bre.WithFluentValidation(validator);
            bre.ThrowIfHasError();

            _leadEmpresaVendaRepository.Save(lead);

        }

        public void AtribuirCorretorLead(LeadDTO lead)
        {
            var bre = new BusinessRuleException();

            if (lead.IdCorretor.IsEmpty())
            {
                bre.AddError(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Corretor)).Complete();
                bre.ThrowIfHasError();
            }

            var novo = _leadEmpresaVendaRepository.FindById(lead.Id);

            if (novo.IsEmpty())
            {
                bre.AddError(string.Format(GlobalMessages.DadoInvalido, GlobalMessages.Lead)).Complete();
                bre.ThrowIfHasError();
            }

            novo.Corretor = new Corretor { Id = lead.IdCorretor };

            var validator = new LeadEmpresaVendaValidator().Validate(novo);
            bre.WithFluentValidation(validator);
            bre.ThrowIfHasError();

            var primeiroLead = _leadEmpresaVendaRepository.CorretorPossuiLead(lead.IdCorretor);
            if (!primeiroLead)
            {
                PrimeiraNotificacao(lead.IdCorretor);
            }
            else
            {
                SegundaNotificacao(lead.IdCorretor);
            }

            _leadEmpresaVendaRepository.Save(novo);

        }

        public void RemoverAtribuicao(LeadDTO lead)
        {
            var leadEmpresaVenda = _leadEmpresaVendaRepository.FindById(lead.Id);

            leadEmpresaVenda.Corretor = null;

            _leadEmpresaVendaRepository.Save(leadEmpresaVenda);
        }

        public byte[] ExpotarDiretor(DataSourceRequest request, FiltroLeadDTO filtro)
        {
            var dados = _viewLeadEmpresaVendaRepository.ListarDatatable(request, filtro).records.ToList();

            ExcelUtil excel = ExcelUtil.NewInstance(25)
                .NewSheet(DateTime.Now.ToString());

            excel.WithHeader(GetHeaderDiretor());

            foreach (var model in dados)
            {
                excel
                    .CreateCellValue(model.NomeLead).Width(20)
                    .CreateCellValue(model.Telefone1).Width(20)
                    .CreateCellValue(model.Telefone2).Width(20)
                    .CreateCellValue(model.Email).Width(20)
                    .CreateCellValue(model.SituacaoLead).Width(20)
                    .CreateCellValue(model.Desistencia).Width(20)
                    .CreateCellValue(model.DescricaoDesistencia).Width(20)
                    .CreateCellValue(model.Logradouro).Width(20)
                    .CreateCellValue(model.Numero).Width(20)
                    .CreateCellValue(model.Bairro).Width(20)
                    .CreateCellValue(model.Cidade).Width(20)
                    .CreateCellValue(model.Uf).Width(20)
                    .CreateCellValue(model.CEP).Width(20)
                    .CreateCellValue(model.Complemento).Width(20)
                    .CreateCellValue(model.NomeCorretor).Width(20)
                    .CreateCellValue(model.Anotacoes).Width(20)
                    ;
            }

            excel.Close();
            return excel.DownloadFile();
        }

        private string[] GetHeaderDiretor()
        {
            IList<string> header = new List<string>
            {
                    GlobalMessages.Nome,
                    GlobalMessages.Telefone1,
                    GlobalMessages.Telefone2,
                    GlobalMessages.Email,
                    GlobalMessages.Situacao,
                    GlobalMessages.MotivoDesistencia,
                    GlobalMessages.DescricaoOutros,
                    GlobalMessages.Logradouro,
                    GlobalMessages.Numero,
                    GlobalMessages.Bairro,
                    GlobalMessages.Cidade,
                    GlobalMessages.Estado,
                    GlobalMessages.CEP,
                    GlobalMessages.Complemento,
                    GlobalMessages.Corretor,
                    GlobalMessages.Anotacao
            };
            return header.ToArray();
        }

        public byte[] ExpotarCorretor(DataSourceRequest request, FiltroLeadDTO filtro)
        {
            var dados = _viewLeadEmpresaVendaRepository.ListarDatatable(request, filtro).records.ToList();

            ExcelUtil excel = ExcelUtil.NewInstance(25)
                .NewSheet(DateTime.Now.ToString());

            excel.WithHeader(GetHeaderCorretor());

            foreach (var model in dados)
            {
                excel
                    .CreateCellValue(model.NomeLead).Width(20)
                    .CreateCellValue(model.Telefone1).Width(20)
                    .CreateCellValue(model.Telefone2).Width(20)
                    .CreateCellValue(model.Email).Width(20)
                    .CreateCellValue(model.SituacaoLead).Width(20)
                    .CreateCellValue(model.Desistencia).Width(20)
                    .CreateCellValue(model.DescricaoDesistencia).Width(20)
                    .CreateCellValue(model.Logradouro).Width(20)
                    .CreateCellValue(model.Numero).Width(20)
                    .CreateCellValue(model.Bairro).Width(20)
                    .CreateCellValue(model.Cidade).Width(20)
                    .CreateCellValue(model.Uf).Width(20)
                    .CreateCellValue(model.CEP).Width(20)
                    .CreateCellValue(model.Complemento).Width(20)
                    .CreateCellValue(model.Pacote).Width(20)
                    .CreateCellValue(model.Anotacoes).Width(20)
                    ;
            }

            excel.Close();
            return excel.DownloadFile();
        }

        private string[] GetHeaderCorretor()
        {
            IList<string> header = new List<string>
            {
                    GlobalMessages.Nome,
                    GlobalMessages.Telefone1,
                    GlobalMessages.Telefone2,
                    GlobalMessages.Email,
                    GlobalMessages.Situacao,
                    GlobalMessages.MotivoDesistencia,
                    GlobalMessages.DescricaoOutros,
                    GlobalMessages.Logradouro,
                    GlobalMessages.Numero,
                    GlobalMessages.Bairro,
                    GlobalMessages.Cidade,
                    GlobalMessages.Estado,
                    GlobalMessages.CEP,
                    GlobalMessages.Complemento,
                    GlobalMessages.Pacote,
                    GlobalMessages.Anotacao
            };
            return header.ToArray();
        }

        public void PrimeiraNotificacao(long idCorretor)
        {
            var bre = new BusinessRuleException();

            var corretor = _corretorRepository.FindById(idCorretor);

            if (corretor.IsEmpty())
            {
                bre.AddError(string.Format(GlobalMessages.DadoInexistente, GlobalMessages.Corretor)).Complete();
                bre.ThrowIfHasError();
            }

            var notificacao = new Notificacao
            {
                Titulo = GlobalMessages.NotificacaoPrimeiroLeadCorretor_Titulo,
                Conteudo = GlobalMessages.NotificacaoPrimeiroLeadCorretor_Conteudo,
                Usuario = corretor.Usuario,
                EmpresaVenda = corretor.EmpresaVenda,
                Link = ProjectProperties.EvsBaseUrl + "/lead",
                TipoNotificacao = TipoNotificacao.Lead,
                NomeBotao = GlobalMessages.IrParaLeads,
                DestinoNotificacao = DestinoNotificacao.Portal,
            };

            _notificacaoRepository.Save(notificacao);
        }

        public void SegundaNotificacao(long idCorretor)
        {
            var bre = new BusinessRuleException();

            var corretor = _corretorRepository.FindById(idCorretor);

            if (corretor.IsEmpty())
            {
                bre.AddError(string.Format(GlobalMessages.DadoInexistente, GlobalMessages.Corretor)).Complete();
                bre.ThrowIfHasError();
            }

            var temNotificacaoLead = _notificacaoRepository.NotificacaoLead(corretor.Usuario.Id);

            if (temNotificacaoLead)
            {
                var lido = _notificacaoRepository.NotificacaoLeadNaoLida(corretor.Usuario.Id);

                if (lido)
                {
                    return;
                }
            }

            var notificacao = new Notificacao
            {
                Titulo = GlobalMessages.NotificacaoSegundoLead_Titulo,
                Conteudo = GlobalMessages.NotificacaoSegundoLead_Conteudo,
                Usuario = corretor.Usuario,
                EmpresaVenda = corretor.EmpresaVenda,
                Link = ProjectProperties.EvsBaseUrl + "/lead",
                TipoNotificacao = TipoNotificacao.Lead,
                NomeBotao = GlobalMessages.IrParaLeads,
                DestinoNotificacao = DestinoNotificacao.Portal,
            };

            _notificacaoRepository.Save(notificacao);
        }
    }
}
