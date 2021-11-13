using Europa.Commons;
using Europa.Data;
using Europa.Extensions;
using Europa.Resources;
using NHibernate.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.ApiService.Models.Lead;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Repository.Models;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class LeadService : BaseService
    {
        private ViewLeadEmpresaVendaRepository _viewLeadEmpresaVendaRepository { get; set; }
        private LeadRepository _leadRepository { get; set; }
        private LeadEmpresaVendaRepository _leadEmpresaVendaRepository { get; set; }
        private CorretorRepository _corretorRepository { get; set; }
        private NotificacaoRepository _notificacaoRepository { get; set; }
        private EmpresaVendaRepository _empresaVendaRepository { get; set; }
        private OrigemLeadRepository _origemLeadRepository { get; set; }
        private LojaRepository _lojaRepository { get; set; }
        private LeadBackupRepository _leadBackupRepository { get; set; }
        private LeadApiValidator _leadApiValidator { get; set; }

        public byte[] Exportar(DataSourceRequest request, FiltroLeadDTO filtro)
        {
            var results = _viewLeadEmpresaVendaRepository.ListarDatatable(request, filtro);

            ExcelUtil excel = ExcelUtil.NewInstance(20)
                .NewSheet(DateTime.Now.ToString("yyyyMMddHHmmss"))
                .WithHeader(GetHeader());

            foreach (var model in results.records.ToList())
            {
                excel
                    .CreateCellValue(model.Pacote).Width(30)
                    .CreateCellValue(model.Liberar ? "Liberado" : "Não Liberado").Width(20)
                    .CreateCellValue(model.NomeEmpresaVenda).Width(20)
                    .CreateCellValue(model.CodigoPreProposta).Width(20)
                    .CreateCellValue(model.NomeLead).Width(30)
                    .CreateCellValue(model.Telefone1.ToPhoneFormat()).Width(30)
                    .CreateCellValue(model.Telefone2.ToPhoneFormat()).Width(30)
                    .CreateCellValue(model.Email).Width(30)
                    .CreateCellValue(model.Logradouro).Width(30)
                    .CreateCellValue(model.Numero).Width(30)
                    .CreateCellValue(model.Complemento).Width(30)
                    .CreateCellValue(model.Bairro).Width(30)
                    .CreateCellValue(model.Cidade).Width(30)
                    .CreateCellValue(model.Uf).Width(20)
                    .CreateCellValue(model.CEP).Width(30)
                    .CreateCellValue(model.NomeCorretor).Width(30)
                    .CreateCellValue(model.SituacaoLead.AsString()).Width(30)
                    .CreateCellValue(model.Desistencia.AsString()).Width(30)
                    .CreateCellValue(model.DescricaoDesistencia).Width(100)
                    .CreateCellValue(model.NomeCliente).Width(30)
                    .CreateCellValue(model.CpfCliente).Width(30)
                    .CreateCellValue(model.Anotacoes).Width(100)
                    .CreateCellValue(model.StatusIndicacao).Width(30)
                    .CreateDateTimeCell(model.DataIndicacao).Width(30);

            }
            excel.Close();
            return excel.DownloadFile();
        }

        private string[] GetHeader()
        {
            IList<string> header = new List<string>
            {
                GlobalMessages.Pacote,
                GlobalMessages.SituacaoPacote,
                GlobalMessages.EmpresaVenda,
                GlobalMessages.PreProposta,
                GlobalMessages.Nome,
                GlobalMessages.Telefone1,
                GlobalMessages.Telefone2,
                GlobalMessages.Email,
                GlobalMessages.Logradouro,
                GlobalMessages.Numero,
                GlobalMessages.Complemento,
                GlobalMessages.Bairro,
                GlobalMessages.Cidade,
                GlobalMessages.Estado,
                GlobalMessages.CEP,
                GlobalMessages.Corretor,
                GlobalMessages.SituacaoLead,
                GlobalMessages.MotivoDesistencia,
                GlobalMessages.Descricao + " " + GlobalMessages.Outros,
                GlobalMessages.NomeIndicador,
                GlobalMessages.CpfIndicador,
                GlobalMessages.Anotacao,
                GlobalMessages.StatusIndicacao,
                GlobalMessages.DataIndicacao

            };
            return header.ToArray();
        }

        public void LiberarPacote(List<string> pacote, BusinessRuleException bre)
        {
            var leads = _leadRepository.BuscarLeadPorPacote(pacote);

            foreach (var model in leads)
            {
                model.Liberar = true;
                _leadRepository.Save(model);
            }
            var idsEmpresaVenda = _leadEmpresaVendaRepository.BuscarIdsEvPorPacote(pacote);

            EnviarNotificacao(idsEmpresaVenda);
        }

        public void ExcluirPacote(string pacote, BusinessRuleException bre)
        {
            try
            {
                var leadsEmpresaVenda = _leadEmpresaVendaRepository.BuscarPorPacote(pacote);
                var leads = _leadRepository.BuscarLeadPorPacote(pacote);

                foreach (var model in leadsEmpresaVenda)
                {
                    _leadEmpresaVendaRepository.Delete(model);
                }
                foreach (var model in leads)
                {
                    _leadRepository.Delete(model);
                }
            }
            catch (GenericADOException exp)
            {
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(exp))
                {
                    bre.AddError(GlobalMessages.RemovidoSemSucesso).WithParam(pacote).Complete();
                }
            }
            bre.ThrowIfHasError();
        }

        private void EnviarNotificacao(List<long> idsEmpresaVenda)
        {

            var diretores = _corretorRepository.ListarDiretoresAtivosDaEmpresaDeVendas(idsEmpresaVenda);

            foreach (var corretor in diretores)
            {
                var notificacao = new Notificacao();
                if (_leadEmpresaVendaRepository.EmpresaVendaPossuiLead(corretor.EmpresaVenda.Id))
                {

                    notificacao = new Notificacao
                    {

                        Titulo = GlobalMessages.NotificacaoNovoLead_Titulo,
                        Conteudo = GlobalMessages.NotificacaoNovoLead_Conteudo,
                        Usuario = corretor.Usuario,
                        EmpresaVenda = corretor.EmpresaVenda,
                        Link = ProjectProperties.EvsBaseUrl + "/lead",
                        TipoNotificacao = TipoNotificacao.Lead,
                        NomeBotao = GlobalMessages.IrParaLeads,
                        DestinoNotificacao = DestinoNotificacao.Portal,

                    };
                }
                else
                {
                    notificacao = new Notificacao
                    {

                        Titulo = GlobalMessages.NotificacaoPrimeiroLead_Titulo,
                        Conteudo = GlobalMessages.NotificacaoPrimeiroLead_Conteudo,
                        Usuario = corretor.Usuario,
                        EmpresaVenda = corretor.EmpresaVenda,
                        Link = ProjectProperties.EvsBaseUrl + "/lead",
                        TipoNotificacao = TipoNotificacao.Lead,
                        NomeBotao = GlobalMessages.IrParaLeads,
                        DestinoNotificacao = DestinoNotificacao.Portal,
                    };

                };

                _notificacaoRepository.Save(notificacao);
            }
        }

        public List<string> IncluirLeads(long idUsuario, LeadRequestDto leadsDto)
        {
            var bre = new BusinessRuleException();

            var origem = _origemLeadRepository.OrigemLeadValido(idUsuario); ;

            if (origem.IsEmpty())
            {
                bre.AddError(string.Format(GlobalMessages.DadoInvalido, GlobalMessages.Usuario)).Complete();
                bre.ThrowIfHasError();
            }

            if (leadsDto.Pacote.IsEmpty())
            {
                bre.AddError(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Pacote)).Complete();
                bre.ThrowIfHasError();
            }

            var pacoteInvalido = _leadRepository.CheckIfOtherPacoteInLead(leadsDto.Pacote);

            if (pacoteInvalido)
            {
                bre.AddError(string.Format(GlobalMessages.MsgErroPacoteExistente)).Complete();
                bre.ThrowIfHasError();
            }

            var sucesso = 0;
            foreach (var item in leadsDto.Leads)
            {

                var valido = _leadApiValidator.Validate(item);
                bre.WithFluentValidation(valido);
                if (bre.HasError())
                {
                    continue;
                }

                BackupLead(leadsDto.Pacote, origem, item);
                var lead = _leadRepository.BuscarLeadPorCodigoLead(item.CodigoLead);
                if (lead.IsEmpty())
                {
                    lead = new Lead
                    {
                        DataPacote = DateTime.Now,
                        DescricaoPacote = leadsDto.Pacote.ToUpper()
                    };
                }
                lead.NomeCompleto = item.NomeIndicado;
                lead.Email = item.Email;
                lead.Telefone1 = item.Telefone1.OnlyNumber();
                lead.Telefone2 = item.Telefone2.OnlyNumber();
                lead.Liberar = item.Liberar;
                lead.CPF = item.CpfIndicado.OnlyNumber();

                lead.IdSapLoja = item.IdSapCentralImobiliaria;
                lead.CodigoOrigemLead = item.CodigoOrigemLead;

                lead.NomeIndicador = item.NomeIndicador;
                lead.CpfIndicador = item.CpfIndicador.OnlyNumber();
                lead.CodigoLead = item.CodigoLead;
                lead.StatusIndicacao = item.StatusIndicacao.IsEmpty() ? "" : item.StatusIndicacao.ToUpper();

                lead.DataIndicacao = item.DataIndicacao;

                lead.Cep = item.Cep;
                lead.Logradouro = item.Logradouro;
                lead.Numero = item.Numero;
                lead.Complemento = item.Complemento;
                lead.Bairro = item.Bairro;
                lead.Cidade = item.Cidade;
                lead.Estado = item.Estado;

                lead.OrigemLead = origem;

                if (lead.DataIndicacao.IsEmpty())
                {
                    lead.DataIndicacao = DateTime.Today;
                }

                _leadRepository.Save(lead);

                var empresaVenda = _empresaVendaRepository.FindByIdSapLoja(item.IdSapCentralImobiliaria);

                var leadEmpresaVenda = _leadEmpresaVendaRepository.Queryable().Where(x => x.Lead.Id == lead.Id)
                    .Where(x => x.EmpresaVenda.Id == empresaVenda.Id).SingleOrDefault();

                if (leadEmpresaVenda.IsEmpty())
                {
                    leadEmpresaVenda = new LeadEmpresaVenda
                    {
                        Lead = lead,
                        EmpresaVenda = new Tenda.Domain.EmpresaVenda.Models.EmpresaVenda { Id = empresaVenda.Id },
                        Situacao = SituacaoLead.Contato
                    };

                }
                if (item.SituacaoEnviadoOlos?.ToUpper() == "RETORNO NEGATIVO")
                {
                    leadEmpresaVenda.Situacao = SituacaoLead.Desistencia;
                    leadEmpresaVenda.Desistencia = TipoDesistencia.SemInteresse;
                }
                _leadEmpresaVendaRepository.Save(leadEmpresaVenda);

                sucesso++;
            }

            if (sucesso > 0)
            {
                bre.AddError(string.Format(GlobalMessages.RegistrosSalvos, sucesso, leadsDto.Leads.Count)).Complete();
            }

            return bre.Errors;
        }

        public void BackupLead(string pacote, OrigemLead origem, LeadDto lead)
        {
            var backup = new LeadBackupApi
            {
                Telefone1 = lead.Telefone1.OnlyNumber(),
                Telefone2 = lead.Telefone2.OnlyNumber(),
                Email = lead.Email,
                Liberar = lead.Liberar,

                NomeIndicado = lead.NomeIndicado,
                CpfIndicado = lead.CpfIndicado.OnlyNumber(),

                IdSapCentralImobiliaria = lead.IdSapCentralImobiliaria,
                CodigoOrigemLead = lead.CodigoOrigemLead,

                NomeIndicador = lead.NomeIndicado,
                CpfIndicador = lead.CpfIndicado.OnlyNumber(),
                CodigoLead = lead.CodigoLead,
                StatusIndicacao = lead.StatusIndicacao,

                DataIndicacao = lead.DataIndicacao,

                Cep = lead.Cep,
                Logradouro = lead.Logradouro,
                Numero = lead.Numero,
                Complemento = lead.Complemento,
                Bairro = lead.Bairro,
                Cidade = lead.Cidade,
                Estado = lead.Estado,

                DataPacote = DateTime.Now,
                DescricaoPacote = pacote,
                OrigemLead = origem
            };

            _leadBackupRepository.Save(backup);
        }
    }
}
