using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tenda.Domain.Core.Models;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Security.Repository;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Domain.Dto;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class RegraComissaoService : BaseService
    {
        private AceiteRegraComissaoRepository _aceiteRegraComissaoRepository { get; set; }
        public RegraComissaoRepository _regraComissaoRepository { get; set; }
        public ArquivoService _arquivoService { get; set; }
        public NotificacaoRepository _notificacaoRepository { get; set; }
        public CorretorRepository _corretorRepository { get; set; }
        public EmpresaVendaRepository _empresaVendaRepository { get; set; }
        private ViewAceiteRegraComissaoRepository _viewAceiteRegraComissaoRepository { get; set; }
        private EnderecoEmpreendimentoRepository _enderecoEmpreendimentoRepository { get; set; }
        private EmpreendimentoRepository _empreendimentoRepository { get; set; }
        public ItemRegraComissaoRepository _itemRegraComissaoRepository { get; set; }
        public RegraComissaoPdfService _regraComissaoPdfService { get; set; }
        public RegraComissaoEvsService _regraComissaoEvsService { get; set; }
        public RegraComissaoEvsRepository _regraComissaoEvsRepository { get; set; }
        public ArquivoRepository _arquivoRepository { get; set; }
        private RegraComissaoPadraoRepository _regraComissaoPadraoRepository { get; set; }
        private ItemRegraComissaoPadraoRepository _itemRegraComissaoPadraoRepository { get; set; }
        public HistoricoRegraComissaoService _historicoRegraComissaoService { get; set; }
        public ValorNominalRepository _valorNominalRepository { get; set; }
        public ItemRegraComissaoValidator _itemRegraComissaoValidator { get; set; }
        private UsuarioPortalRepository _usuarioPortalRepository { get; set; }

        public RegraComissaoService()
        {
        }

        public void ExcluirPorId(RegraComissao regra)
        {
            BusinessRuleException bre = new BusinessRuleException();

            if (regra.IsEmpty())
            {
                bre.AddError(GlobalMessages.RegistroNaoExiste).Complete(); ;
            }

            bre.ThrowIfHasError();

            var itensRegraComissao = _itemRegraComissaoRepository.ItensDeRegra(regra.Id);
            foreach (var item in itensRegraComissao)
            {
                _itemRegraComissaoRepository.Delete(item);
            }

            var regrasEvs = _regraComissaoEvsRepository.BuscarPorRegraComissao(regra.Id);

            if (!regrasEvs.IsEmpty())
            {
                var arquivos = regrasEvs
                    .Where(x => x.Arquivo != null)
                    .Select(x => x.Arquivo)
                    .ToList();

                foreach (var rce in regrasEvs)
                {
                    rce.Arquivo = null;
                    _historicoRegraComissaoService.DeletarHistorico(rce.Id);
                    _regraComissaoEvsRepository.Delete(rce);
                }

                foreach (var arq in arquivos)
                {
                    _arquivoRepository.Delete(arq);
                }
            }

            _regraComissaoRepository.Delete(regra);

            if (!regra.Arquivo.IsEmpty())
            {
                _arquivoRepository.Delete(regra.Arquivo);
            }

        }
        public RegraComissao Salvar(RegraComissao regra, HttpPostedFileBase arquivo, BusinessRuleException bre)
        {
            if (regra.Regional.IsEmpty())
            {
                bre.AddError(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Regional))
                    .AddField("Regional").Complete();
            }

            if (regra.Descricao.IsEmpty())
            {
                bre.AddError(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Descricao))
                    .AddField("Descricao").Complete();
            }

            if (regra.Tipo.IsEmpty())
            {
                bre.AddError(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Tipo))
                    .AddField("Tipo").Complete();
            }

            if (arquivo.IsEmpty())
            {
                bre.AddError(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Arquivo)).AddField("Arquivo")
                    .Complete();
            }
            else if (arquivo.ContentType != MimeMappingWrapper.Pdf)
            {
                bre.AddError(string.Format(GlobalMessages.MsgUploadFormatoInvalido, GlobalMessages.PDF))
                    .AddField("Arquivo").Complete();
            }

            bre.ThrowIfHasError();

            regra.Arquivo = _arquivoService.CreateFile(arquivo);
            regra.ContentTypeDoubleCheck = regra.Arquivo.ContentType;
            regra.HashDoubleCheck = regra.Arquivo.Hash;
            regra.IdArquivoDoubleCheck = regra.Arquivo.Id;
            regra.NomeDoubleCheck = regra.Arquivo.Nome;

            _regraComissaoRepository.Save(regra);

            return regra;
        }

        public void AtivarCampanhaRobo(RegraComissao regra)
        {
            //Valida os dados da regra de comissão
            ValidarRegraComissaoRobo(regra);

            //Suspende as regras de comissao evs ativas
            SuspenderRegraComissaoEvs(regra.Id);

            //Suspende a regra de comissao
            SuspenderRegraComissao(regra.Id);

            //ativa a regra de comissao
            AtivarRegraComissaoNova(ref regra);

            //ativa a regra de comissao das evs
            AtivarCampanhaEvs(regra.Id);

            //lista o Id das empresas de vendas afetadas
            var idsEmpresaVenda = EvsComNovaRegraComissaoAtiva(regra.Id);

            //Envia notificação das campanhas
            EnviarNotificacaoCampanhaEvsAtiva(idsEmpresaVenda);

        }

        public void InativarCampanha(RegraComissao regra)
        {
            //Reativando regra de comissão evs
            ReativarRegraComissaoEvs(regra.Id);

            //Reativando regra comissao
            ReativarRegraComissao(regra.Id);

            //Finalizar as campanhas das EVS
            FinalizarCampanhaEvs(regra.Id);

            //Finalizar campanha
            FinalizarCampanha(ref regra);

        }

        private void FinalizarCampanhaEvs(long idRegraComissao)
        {
            var transaction = _session.Transaction;

            transaction.Begin();

            //regrasEvs da nova regra que estejam rascunho
            var campanhasAtivas = _regraComissaoEvsRepository.BuscarCampanhasAtivas(idRegraComissao);

            //mata os filhos ativos
            foreach (var regrasEvs in campanhasAtivas)
            {
                regrasEvs.Situacao = SituacaoRegraComissao.Vencido;
                _regraComissaoEvsRepository.Save(regrasEvs);
                var responsavel = new UsuarioPortal()
                {
                    Id = ProjectProperties.IdUsuarioSistema
                };
                _historicoRegraComissaoService.CriarOuAvancar(regrasEvs, responsavel);
            }

            transaction.Commit();
        }

        public void FinalizarCampanha(ref RegraComissao regra)
        {
            var transaction = _session.BeginTransaction();
            regra.Situacao = SituacaoRegraComissao.Vencido;
            _regraComissaoRepository.Save(regra);
            transaction.Commit();
        }

        private void ReativarRegraComissaoEvs(long idRegraComissao)
        {
            var transaction = _session.Transaction;

            transaction.Begin();

            //Campanhas EVs Ativas
            var campanhasAtivas = _regraComissaoEvsRepository.BuscarCampanhasAtivas(idRegraComissao);
            var idsEvs = campanhasAtivas.Select(x => x.EmpresaVenda.Id).ToList();

            //filhos que tenham regra de comissao suspensa
            var regrasEvsSuspensas = _regraComissaoEvsRepository.BuscarRegrasEvsSuspensas(idsEvs);

            //mata os filhos ativos
            foreach (var regrasEvs in regrasEvsSuspensas)
            {
                regrasEvs.Situacao = SituacaoRegraComissao.Ativo;
                _regraComissaoEvsRepository.Save(regrasEvs);

                var responsavel = new UsuarioPortal()
                {
                    Id = ProjectProperties.IdUsuarioSistema
                };
                _historicoRegraComissaoService.CriarOuAvancar(regrasEvs, responsavel);
            }

            transaction.Commit();
        }
        private void ReativarRegraComissao(long idRegraComissao)
        {
            var transaction = _session.Transaction;

            transaction.Begin();

            var campanhasAtivas = _regraComissaoEvsRepository.BuscarCampanhasAtivas(idRegraComissao);

            var idsEvs = campanhasAtivas.Select(x => x.EmpresaVenda.Id).ToList();

            var regrasEvsSuspensas = _regraComissaoEvsRepository.BuscarRegrasEvsAtivas(idsEvs).Where(x => x.RegraComissao.Id != idRegraComissao);

            //suspende os pais ativos
            foreach (var regraEvs in regrasEvsSuspensas)
            {
                var regraComissao = regraEvs.RegraComissao;
                var semRegraEvs = _regraComissaoEvsRepository.ExistemRegrasEvsSuspensas(regraComissao.Id);
                if (!semRegraEvs)
                {
                    regraComissao.Situacao = SituacaoRegraComissao.Ativo;
                    _regraComissaoRepository.Save(regraComissao);
                }
            }

            transaction.Commit();
        }

        public RegraComissao LiberarCampanha(RegraComissao regra, UsuarioPortal responsavel)
        {
            ValidarRegraComissao(regra);
            AlteraSituacaoRegraComissao(ref regra, SituacaoRegraComissao.AguardandoLiberacao);
            AlteraSituacaoRegraComissaoEvs(regra.Id, SituacaoRegraComissao.AguardandoLiberacao, responsavel);
            return regra;
        }
        public void ValidarRegraComissaoRobo(RegraComissao regra)
        {
            BusinessRuleException bre = new BusinessRuleException();

            if (regra.IsEmpty())
            {
                bre.AddError(GlobalMessages.RegraComissaoNaoEncontrada).Complete();
                bre.ThrowIfHasError();
            }

            if (regra.Situacao != SituacaoRegraComissao.AguardandoLiberacao)
            {
                bre.AddError(GlobalMessages.MsgLiberarRegraComissaoAguardandoLiberacaoRobo).Complete();
                bre.ThrowIfHasError();
            }

            if (regra.Arquivo.IsEmpty())
            {
                bre.AddError(GlobalMessages.ErroLiberarRegraComissao).Complete();
                bre.ThrowIfHasError();
            }
        }
        private void ValidarRegraComissao(RegraComissao regra)
        {
            BusinessRuleException bre = new BusinessRuleException();

            if (regra.IsEmpty())
            {
                bre.AddError(GlobalMessages.RegraComissaoNaoEncontrada).Complete();
                bre.ThrowIfHasError();
            }

            if (regra.Situacao != SituacaoRegraComissao.Rascunho)
            {
                bre.AddError(GlobalMessages.MsgLiberarRegraComissaoRascunho).Complete();
                bre.ThrowIfHasError();
            }

            if (regra.Arquivo.IsEmpty())
            {
                bre.AddError(GlobalMessages.ErroLiberarRegraComissao).Complete();
                bre.ThrowIfHasError();
            }

            //validar empreendimentos 
            var itens = _itemRegraComissaoRepository.BuscarItens(regra.Id)
                .Where(x => x.TipoModalidadeComissao == TipoModalidadeComissao.Nominal);
            if (itens.HasValue())
            {
                var empreendimentos = itens.Select(x => x.Empreendimento).Distinct();
                var valores = _valorNominalRepository.Queryable()
                    .Where(x => x.InicioVigencia.Value.Date <= DateTime.Now.Date)
                    .Where(x => x.Situacao == SituacaoValorNominal.Ativo)
                    .Select(x => x.Empreendimento).ToList();

                if (valores.IsEmpty())
                {
                    bre.AddError("Não há tabela de valores nominais ativas").Complete();
                    bre.ThrowIfHasError();
                }

                foreach(var emp in empreendimentos)
                {
                    if (!valores.Contains(emp))
                    {
                        bre.AddError(string.Format("Empreendimento {0} não possui tabela de valor nominal ativa",emp.Nome)).Complete();                        
                    }
                }

                bre.ThrowIfHasError();

            }                       

            //regrasEvs da nova regra que estejam rascunho
            var regrasEvsRascunho = _regraComissaoEvsRepository.BuscarRegrasRascunho(regra.Id);
            //Evs que terão novas regras ativas
            var idsEvs = regrasEvsRascunho.Select(x => x.EmpresaVenda.Id).ToList();

            //filhos da nova regra que tenham campanha ativa
            var campanhasEvsAtivas = _regraComissaoEvsRepository.BuscarCampanhasEvsAtivas(idsEvs);

            foreach (var campanha in campanhasEvsAtivas)
            {
                var rascunho = regrasEvsRascunho.Where(x => x.EmpresaVenda.Id == campanha.EmpresaVenda.Id).FirstOrDefault();

                if (!rascunho.IsEmpty())
                {
                    var dataTesteInicio = DateTime.Now;

                    if (rascunho.Tipo == TipoRegraComissao.Campanha)
                    {
                        dataTesteInicio = rascunho.InicioVigencia.Value;
                    }

                    if (dataTesteInicio >= campanha.InicioVigencia.Value && dataTesteInicio <= campanha.TerminoVigencia.Value)
                    {
                        bre.AddError(string.Format(GlobalMessages.PossuiCampanhaPeriodo, campanha.EmpresaVenda.NomeFantasia, SituacaoRegraComissao.Ativo)).Complete();
                    }
                    else if (rascunho.Tipo == TipoRegraComissao.Campanha && rascunho.TerminoVigencia.Value >= campanha.InicioVigencia.Value && rascunho.TerminoVigencia.Value <= campanha.TerminoVigencia.Value)
                    {
                        bre.AddError(string.Format(GlobalMessages.PossuiCampanhaPeriodo, campanha.EmpresaVenda.NomeFantasia, SituacaoRegraComissao.Ativo)).Complete();
                    }
                    else if (rascunho.Tipo == TipoRegraComissao.Campanha && campanha.InicioVigencia.Value >= rascunho.InicioVigencia.Value && campanha.InicioVigencia.Value <= rascunho.TerminoVigencia.Value)
                    {
                        bre.AddError(string.Format(GlobalMessages.PossuiCampanhaPeriodo, campanha.EmpresaVenda.NomeFantasia, SituacaoRegraComissao.Ativo)).Complete();
                    }
                    else if (rascunho.Tipo == TipoRegraComissao.Campanha && campanha.TerminoVigencia.Value >= rascunho.InicioVigencia.Value && campanha.TerminoVigencia.Value <= rascunho.TerminoVigencia.Value)
                    {
                        bre.AddError(string.Format(GlobalMessages.PossuiCampanhaPeriodo, campanha.EmpresaVenda.NomeFantasia, SituacaoRegraComissao.Ativo)).Complete();
                    }
                }
            }

            bre.ThrowIfHasError();

            var campanhasEvsAguardando = _regraComissaoEvsRepository.BuscarCampanhasEvsAguardando(idsEvs);
            foreach (var aguardando in campanhasEvsAguardando)
            {
                var rascunho = regrasEvsRascunho.Where(x => x.EmpresaVenda.Id == aguardando.EmpresaVenda.Id).FirstOrDefault();

                if (!rascunho.IsEmpty())
                {
                    if (rascunho.Tipo == TipoRegraComissao.Campanha && rascunho.InicioVigencia.Value >= aguardando.InicioVigencia.Value && rascunho.InicioVigencia.Value <= aguardando.TerminoVigencia.Value)
                    {
                        bre.AddError(string.Format(GlobalMessages.PossuiCampanhaPeriodo, aguardando.EmpresaVenda.NomeFantasia, SituacaoRegraComissao.AguardandoLiberacao)).Complete();
                    }
                    else if (rascunho.Tipo == TipoRegraComissao.Campanha && rascunho.TerminoVigencia.Value >= aguardando.InicioVigencia.Value && rascunho.TerminoVigencia.Value <= aguardando.TerminoVigencia.Value)
                    {
                        bre.AddError(string.Format(GlobalMessages.PossuiCampanhaPeriodo, aguardando.EmpresaVenda.NomeFantasia, SituacaoRegraComissao.AguardandoLiberacao)).Complete();
                    }
                    else if (rascunho.Tipo == TipoRegraComissao.Campanha && aguardando.InicioVigencia.Value >= rascunho.InicioVigencia.Value && aguardando.InicioVigencia.Value <= rascunho.TerminoVigencia.Value)
                    {
                        bre.AddError(string.Format(GlobalMessages.PossuiCampanhaPeriodo, aguardando.EmpresaVenda.NomeFantasia, SituacaoRegraComissao.AguardandoLiberacao)).Complete();
                    }
                    else if (rascunho.Tipo == TipoRegraComissao.Campanha && aguardando.TerminoVigencia.Value >= rascunho.InicioVigencia.Value && aguardando.TerminoVigencia.Value <= rascunho.TerminoVigencia.Value)
                    {
                        bre.AddError(string.Format(GlobalMessages.PossuiCampanhaPeriodo, aguardando.EmpresaVenda.NomeFantasia, SituacaoRegraComissao.AguardandoLiberacao)).Complete();
                    }

                }
            }

            bre.ThrowIfHasError();
        }

        public void AlteraSituacaoRegraComissao(ref RegraComissao regraComissao, SituacaoRegraComissao situacao)
        {
            var transaction = _session.BeginTransaction();
            regraComissao.Situacao = situacao;
            _regraComissaoRepository.Save(regraComissao);
            transaction.Commit();
        }

        public void AlteraSituacaoRegraComissaoEvs(long idRegraComissao, SituacaoRegraComissao situacao, UsuarioPortal responsavel)
        {
            //regrasEvs da nova regra que estejam rascunho
            var regrasEvsRascunho = _regraComissaoEvsRepository.BuscarRegrasRascunho(idRegraComissao);

            foreach (var regraEvs in regrasEvsRascunho)
            {
                var transaction = _session.BeginTransaction();
                regraEvs.Situacao = situacao;
                if (situacao == SituacaoRegraComissao.Ativo)
                {
                    regraEvs.InicioVigencia = DateTime.Now;
                }
                _regraComissaoEvsRepository.Save(regraEvs);
                _historicoRegraComissaoService.CriarOuAvancar(regraEvs, responsavel);
                transaction.Commit();
            }
        }

        public void SuspenderRegraComissaoEvs(long idRegraComissao)
        {
            //regrasEvs da nova regra que estejam rascunho
            var regrasEvsAguardando = _regraComissaoEvsRepository.BuscarCampanhasAguardadoLiberacao(idRegraComissao);
            var idsEvs = regrasEvsAguardando.Select(x => x.EmpresaVenda.Id).ToList();

            //filhos da nova regra que tenham regra de comissao ativa
            var regrasEvsAtivas = _regraComissaoEvsRepository.BuscarRegrasEvsAtivas(idsEvs);

            //suspende os filhos ativos
            foreach (var regrasEvs in regrasEvsAtivas)
            {
                var transaction = _session.BeginTransaction();
                regrasEvs.Situacao = SituacaoRegraComissao.SuspensoPorCampanha;
                _regraComissaoEvsRepository.Save(regrasEvs);

                var responsavel = new UsuarioPortal()
                {
                    Id = ProjectProperties.IdUsuarioSistema
                };
                _historicoRegraComissaoService.CriarOuAvancar(regrasEvs, responsavel);

                transaction.Commit();
            }

        }

        public void SuspenderRegraComissao(long idRegraComissao)
        {
            var regrasEvsAguardando = _regraComissaoEvsRepository.BuscarCampanhasAguardadoLiberacao(idRegraComissao);
            var idsEvs = regrasEvsAguardando.Select(x => x.EmpresaVenda.Id).ToList();

            var regrasEvsSuspensas = _regraComissaoEvsRepository.BuscarRegrasEvsSuspensas(idsEvs);

            //suspende os pais ativos
            foreach (var regraEvs in regrasEvsSuspensas)
            {
                var regraComissao = regraEvs.RegraComissao;
                var semRegraEvs = _regraComissaoEvsRepository.ExistemCampanhasAguardandoLiberacao(regraComissao.Id);
                if (!semRegraEvs)
                {
                    var transaction = _session.BeginTransaction();
                    regraComissao.Situacao = SituacaoRegraComissao.SuspensoPorCampanha;
                    _regraComissaoRepository.Save(regraComissao);
                    transaction.Commit();
                }
            }
        }

        public List<long> EvsComNovaRegraComissaoAtiva(long idRegraComissao)
        {
            var transaction = _session.BeginTransaction();
            var regrasEvsAtivas = _regraComissaoEvsRepository.BuscarCampanhasAtivas(idRegraComissao);
            var idsEmpresaVenda = regrasEvsAtivas.Select(x => x.EmpresaVenda.Id).ToList();
            transaction.Commit();

            return idsEmpresaVenda;
        }

        public void EnviarNotificacaoCampanhaEvsAtiva(List<long> idsEmpresaVendas)
        {
            var transaction = _session.BeginTransaction();
            var diretores = _corretorRepository.ListarDiretoresAtivosDaEmpresaDeVendas(idsEmpresaVendas);
            foreach (var corretor in diretores)
            {
                var notificacao = new Notificacao
                {
                    Titulo = "Nova campanha ativa!",
                    Conteudo =
                        "Você precisa aceitar o novo contrato de campanha.",
                    Usuario = corretor.Usuario,
                    EmpresaVenda = corretor.EmpresaVenda,
                    DestinoNotificacao = DestinoNotificacao.Portal,
                };
                _notificacaoRepository.Save(notificacao);
            }
            transaction.Commit();
        }

        private void AtivarCampanhaEvs(long idRegraComissao)
        {
            var transaction = _session.Transaction;

            transaction.Begin();

            //regrasEvs da nova regra que estejam rascunho
            var campanhasAguardando = _regraComissaoEvsRepository.BuscarCampanhasAguardadoLiberacao(idRegraComissao);

            foreach (var regraEvs in campanhasAguardando)
            {
                regraEvs.Situacao = SituacaoRegraComissao.Ativo;
                _regraComissaoEvsRepository.Save(regraEvs);

                var responsavel = new UsuarioPortal()
                {
                    Id = ProjectProperties.IdUsuarioSistema
                };
                _historicoRegraComissaoService.CriarOuAvancar(regraEvs, responsavel);
            }

            transaction.Commit();
        }

        public RegraComissao Liberar(RegraComissao regra, UsuarioPortal responsavel)
        {
            ValidarRegraComissao(regra);

            FinalizarRegraComissaoEvs(regra.Id, responsavel);
            FinalizarRegraComissao(regra.Id);
            AtivarRegraComissaoNova(ref regra);
            AtivarRegraComissaoEvs(regra.Id, responsavel);
            var idsEmpresaVenda = EvsComNovaRegraComissao(regra.Id);

            EnviarNotificacaoRegraEvsAtualizada(idsEmpresaVenda);

            return regra;
        }

        private void FinalizarRegraComissaoEvs(long idRegraComissao, UsuarioPortal responsavel)
        {
            var transaction = _session.Transaction;

            transaction.Begin();

            //regrasEvs da nova regra que estejam rascunho
            var regrasEvsRascunho = _regraComissaoEvsRepository.BuscarRegrasRascunho(idRegraComissao);
            var idsEvs = regrasEvsRascunho.Select(x => x.EmpresaVenda.Id).ToList();

            //filhos da nova regra que tenham regra de comissao ativa
            var regrasEvsAtivas = _regraComissaoEvsRepository.BuscarRegrasEvsAtivas(idsEvs);

            //mata os filhos ativos
            foreach (var regrasEvs in regrasEvsAtivas)
            {
                regrasEvs.Situacao = SituacaoRegraComissao.Vencido;
                if (regrasEvs.Tipo == TipoRegraComissao.Normal)
                {
                    regrasEvs.TerminoVigencia = DateTime.Now;
                }
                _regraComissaoEvsRepository.Save(regrasEvs);
                _historicoRegraComissaoService.CriarOuAvancar(regrasEvs, responsavel);
            }

            transaction.Commit();
        }

        private void FinalizarRegraComissao(long idRegraComissao)
        {
            var transaction = _session.Transaction;

            transaction.Begin();

            var regrasEvsRascunho = _regraComissaoEvsRepository.BuscarRegrasRascunho(idRegraComissao);
            var idsEvs = regrasEvsRascunho.Select(x => x.EmpresaVenda.Id).ToList();

            var regrasEvsAtivas = _regraComissaoEvsRepository.BuscarRegrasEvsInativas(idsEvs);

            //mata os filhos ativos
            foreach (var regraEvs in regrasEvsAtivas)
            {
                var regraComissao = regraEvs.RegraComissao;
                var semRegraEvs = _regraComissaoEvsRepository.VerificarFilhosAtivos(regraComissao.Id);
                if (!semRegraEvs)
                {
                    regraComissao.Situacao = SituacaoRegraComissao.Vencido;
                    if (regraComissao.Tipo == TipoRegraComissao.Normal)
                    {
                        regraComissao.TerminoVigencia = DateTime.Now;
                    }
                    _regraComissaoRepository.Save(regraComissao);
                }
            }

            transaction.Commit();
        }

        private void AtivarRegraComissaoNova(ref RegraComissao regraComissao)
        {
            regraComissao.Situacao = SituacaoRegraComissao.Ativo;
            regraComissao.InicioVigencia = regraComissao.InicioVigencia;
            if (regraComissao.Tipo == TipoRegraComissao.Normal)
            {
                regraComissao.InicioVigencia = DateTime.Now;
            }
            _regraComissaoRepository.Save(regraComissao);
        }

        private void AtivarRegraComissaoEvs(long idRegraComissao, UsuarioPortal responsavel)
        {
            var transaction = _session.Transaction;

            transaction.Begin();

            //regrasEvs da nova regra que estejam rascunho
            var regrasEvsRascunho = _regraComissaoEvsRepository.BuscarRegrasRascunho(idRegraComissao);

            foreach (var regraEvs in regrasEvsRascunho)
            {
                regraEvs.Situacao = SituacaoRegraComissao.Ativo;
                regraEvs.InicioVigencia = DateTime.Now;
                _regraComissaoEvsRepository.Save(regraEvs);
                _historicoRegraComissaoService.CriarOuAvancar(regraEvs, responsavel);
            }

            transaction.Commit();
        }

        private List<long> EvsComNovaRegraComissao(long idRegraComissao)
        {
            var transaction = _session.Transaction;

            transaction.Begin();

            var regrasEvsRascunho = _regraComissaoEvsRepository.BuscarFilhosAtivos(idRegraComissao);
            var idsEmpresaVenda = regrasEvsRascunho.Select(x => x.EmpresaVenda.Id).ToList();

            transaction.Commit();

            return idsEmpresaVenda;
        }

        private void EnviarNotificacaoRegraEvsAtualizada(List<long> idsEmpresaVendas)
        {
            var transaction = _session.BeginTransaction();
            var diretores = _corretorRepository.ListarDiretoresAtivosDaEmpresaDeVendas(idsEmpresaVendas);
            foreach (var corretor in diretores)
            {
                var notificacao = new Notificacao
                {
                    Titulo = "Seu Contrato de Prestação de Serviços de Corretagem foi alterado!",
                    Conteudo =
                        "Você precisa ler e concordar com o teor do novo contrato para ter acesso a todas as funcionalidades da plataforma.",
                    Usuario = corretor.Usuario,
                    EmpresaVenda = corretor.EmpresaVenda,
                    DestinoNotificacao = DestinoNotificacao.Portal,
                };
                _notificacaoRepository.Save(notificacao);
            }
            transaction.Commit();
        }

        private void EnviarNotificacaoRegraAtualizada(string regional)
        {
            var diretores = _corretorRepository.ListarDiretoresAtivosDaRegional(regional);
            foreach (var corretor in diretores)
            {
                var notificacao = new Notificacao
                {
                    Titulo = "Seu Contrato de Prestação de Serviços de Corretagem foi alterado!",
                    Conteudo =
                        "Você precisa ler e concordar com o teor do novo contrato para ter acesso a todas as funcionalidades da plataforma.",
                    Usuario = corretor.Usuario,
                    EmpresaVenda = corretor.EmpresaVenda,
                    DestinoNotificacao = DestinoNotificacao.Portal,
                };
                _notificacaoRepository.Save(notificacao);
            }
        }

        private void EnviarNotificacaoRegraAtualizada(long idEmpresaVenda)
        {
            var transaction = _session.BeginTransaction();

            var diretores = _corretorRepository.ListarDiretoresAtivosDaEmpresaDeVendas(idEmpresaVenda);
            foreach (var corretor in diretores)
            {
                var notificacao = new Notificacao
                {
                    Titulo = "Seu Contrato de Prestação de Serviços de Corretagem foi alterado!",
                    Conteudo =
                        "Você precisa ler e concordar com o teor do novo contrato para ter acesso a todas as funcionalidades da plataforma.",
                    Usuario = corretor.Usuario,
                    EmpresaVenda = corretor.EmpresaVenda,
                    DestinoNotificacao = DestinoNotificacao.Portal,
                };
                _notificacaoRepository.Save(notificacao);
            }
            transaction.Commit();
        }

        private void EnviarNotificacaoRegraAtualizada(Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda,long idResponsavel)
        {
            var diretores = _corretorRepository.ListarDiretoresAtivosDaEmpresaDeVendas(empresaVenda.Id);
            foreach (var corretor in diretores)
            {
                var notificacao = new Notificacao
                {
                    Titulo = "Seu Contrato de Prestação de Serviços de Corretagem foi alterado!",
                    Conteudo =
                        "Você precisa ler e concordar com o teor do novo contrato para ter acesso a todas as funcionalidades da plataforma.",
                    Usuario = corretor.Usuario,
                    EmpresaVenda = corretor.EmpresaVenda,
                    CriadoPor = idResponsavel,
                    DestinoNotificacao = DestinoNotificacao.Portal,
                };
                _notificacaoRepository.Save(notificacao);
            }
        }

        public RegraComissao RegraComissaoVigente(string regional)
        {
            return _regraComissaoRepository.BuscarRegraVigente(regional);
        }

        public bool PossuiAceiteParaRegraComissaoVigente(long idEmpresaVenda)
        {
            var empresaVenda = _empresaVendaRepository.FindById(idEmpresaVenda);
            var regraAtiva = RegraComissaoVigente(empresaVenda.Estado.ToUpper());
            if (regraAtiva == null)
            {
                return true;
            }

            return _aceiteRegraComissaoRepository.BuscarAceiteParaRegraAndEmpresaVenda(regraAtiva.Id, idEmpresaVenda);
        }

        public byte[] Exportar(DataSourceRequest request, long idRegraComissao)
        {
            var regraComissao = _regraComissaoRepository.FindById(idRegraComissao);

            var resultados = _viewAceiteRegraComissaoRepository.ListarPorRegraComissao(idRegraComissao)
                .ToDataRequest(request).records.ToList();

            ExcelUtil excel = ExcelUtil.NewInstance(25)
                .NewSheet(DateTime.Now.ToString(regraComissao.Descricao))
                .WithHeader(GetHeader());

            foreach (var model in resultados)
            {
                excel
                    .CreateCellValue(regraComissao.Regional).Width(10)
                    .CreateCellValue(model.NomeEmpresaVenda).Width(50)
                    .CreateCellValue(model.CnpjEmpresaVenda.ToCNPJFormat()).Width(20)
                    .CreateCellValue(regraComissao.InicioVigencia.Value.ToDate()).Width(20)
                    .CreateCellValue(model.TerminoVigenciaRegraComissao.IsEmpty()
                        ? ""
                        : model.TerminoVigenciaRegraComissao.Value.ToDate()).Width(20)
                    .CreateCellValue(model.DataAceite.IsEmpty() ? "" : model.DataAceite.Value.ToDateTimeSeconds())
                    .Width(20)
                    .CreateCellValue(model.NomeAprovador).Width(50);
            }

            excel.Close();
            return excel.DownloadFile();
        }

        private string[] GetHeader()
        {
            IList<string> header = new List<string>
            {
                GlobalMessages.Regional,
                GlobalMessages.EmpresaVenda,
                GlobalMessages.Cnpj,
                GlobalMessages.InicioVigencia,
                GlobalMessages.TerminoVigencia,
                GlobalMessages.DataAceite,
                GlobalMessages.Aprovador
            };
            return header.ToArray();
        }

        #region

        #endregion

        public JExcelOptions BuscarMatriz(long idRegraComissao, long[] listaEvs, string regional, bool editable,
            bool novo, bool ultimaAtt, TipoModalidadeComissao modalidade)
        {
            //Lista dos Empreendimentos conforme a modalidade de comissão
            var empreendimentos = _enderecoEmpreendimentoRepository
                .EnderecosDaRegional(regional)
                .Select(reg => reg.Empreendimento)
                .Where(x => x.ModalidadeComissao == modalidade)
                .OrderByDescending(x => x.PriorizarRegraComissao)
                .ThenBy(x => x.Nome)
                .ToList();

            //Lista das Empresas de Vendas Ativas por regional
            var empresasVenda = _empresaVendaRepository
                .EmpresasDaRegional(regional)
                .Where(x => x.Situacao == Situacao.Ativo)
                .ToList();


            if (listaEvs.HasValue())
            {
                //Traz apenas as empresas selecionadas
                empresasVenda = empresasVenda.Where(x => listaEvs.Contains(x.Id)).ToList();
            }
            else if (idRegraComissao > 0)
            {
                //Lista das empresas de vendas pertencentes a regra de comissão selecionada
                empresasVenda = _regraComissaoEvsRepository.BuscarPorRegraComissao(idRegraComissao)
                    .Select(x => x.EmpresaVenda)
                    .ToList();
            }

            empresasVenda = empresasVenda.OrderBy(x => x.NomeFantasia)
                .ToList();

            var options = new JExcelOptions();

            var itensRegraComissao = new List<ItemRegraComissao>();
            var regraComissao = _regraComissaoRepository.FindById(idRegraComissao);

            if (regraComissao.IsEmpty())
            {
                regraComissao = new RegraComissao();
                regraComissao.Regional = regional;
            }
            else
            {
                //lista dos itens de regra de comissão existentes
                itensRegraComissao = _itemRegraComissaoRepository.Buscar(regraComissao.Id, modalidade);

                var empreendimentosItens = itensRegraComissao.Select(x => x.Empreendimento).ToList();

                if (novo)
                {
                    empreendimentosItens = empreendimentosItens.Where(x => empreendimentos.Contains(x)).ToList();
                    empreendimentosItens = empreendimentosItens.Union(empreendimentos).ToList();
                }

                empreendimentos = empreendimentosItens
                    .OrderByDescending(x => x.PriorizarRegraComissao)
                    .ThenBy(x => x.Nome)
                    .Distinct()
                    .ToList(); 
                    
            }

            var itensAtivos = new List<ItemRegraComissao>();

            if (novo)
            {
                //lista de regras de comissão evs ativas para o grupo de evs passadas
                var idRegrasEvsAtivas = _regraComissaoEvsRepository.Queryable()
                    .Where(x => x.Situacao == SituacaoRegraComissao.Ativo)
                    .Where(x => empresasVenda.Contains(x.EmpresaVenda))
                    .Select(x => x.RegraComissao.Id)
                    .ToList();

                //lista de itens de regra de comissão ativos conforme a modalidade de comissão
                itensAtivos = _itemRegraComissaoRepository.Queryable()
                    .Where(x => x.TipoModalidadeComissao == modalidade)
                    .Where(x => idRegrasEvsAtivas.Contains(x.RegraComisao.Id))
                    .Where(x => empreendimentos.Contains(x.Empreendimento))
                    .Where(x => empresasVenda.Contains(x.EmpresaVenda))
                    .ToList();
            }

            options = InsertHeadersMatriz(options, empresasVenda, modalidade);

            switch (modalidade)
            {
                case TipoModalidadeComissao.Fixa:
                    options = InsertColumnsMatriz(options, empresasVenda.Count, editable);
                    options = InsertDataMatriz(options, empreendimentos, empresasVenda, regraComissao.Id, itensRegraComissao,itensAtivos, novo, ultimaAtt, modalidade);
                    break;

                case TipoModalidadeComissao.Nominal:
                    options = InsertNominalColumnsMatriz(options, empresasVenda.Count, editable);
                    options = InsertNominalDataMatriz(options, empreendimentos, empresasVenda, regraComissao.Id, itensRegraComissao, itensAtivos, novo, ultimaAtt, modalidade);
                    break;
            }

            return options;
        }

        private JExcelOptions InsertHeadersMatriz(JExcelOptions options,
            List<Tenda.Domain.EmpresaVenda.Models.EmpresaVenda> evs, TipoModalidadeComissao modalidade)
        {
            options.NestedHeaders = new List<List<Header>>()
            {
                new List<Header>
                {
                    new Header {Colspan = 1, Title = "Matriz", Id = 0}
                }
            };

            foreach (var ev in evs)
            {
                switch (modalidade)
                {
                    case TipoModalidadeComissao.Fixa:
                        options.NestedHeaders[0].Add(new Header { Colspan = 5, Title = ev.NomeFantasia, Id = ev.Id });
                        break;

                    case TipoModalidadeComissao.Nominal:
                        options.NestedHeaders[0].Add(new Header { Colspan = 12, Title = ev.NomeFantasia, Id = ev.Id });
                        break;
                }
            }
            if (options.NestedHeaders[0].Count == 1)
            {
                options.NestedHeaders = null;
            }

            return options;
        }

        private JExcelOptions InsertColumnsMatriz(JExcelOptions options, int numEvs, bool editable)
        {
            options.Columns = new List<Column>
            {
                new Column
                {
                    ReadOnly = true, Title = GlobalMessages.Empreendimentos, Type = ColumnType.Text, Width = "165px",
                    WordWrap = true
                },
                new Column {ReadOnly = true, Title = "IdRegraComissao", Type = ColumnType.Hidden, Width = "0px"},
                new Column {ReadOnly = true, Title = "IdEmpreendimento", Type = ColumnType.Hidden, Width = "0px"},
            };

            for (var i = 0; i < numEvs; i++)
            {
                options.Columns.AddRange(new List<Column>
                {
                    new Column
                    {
                        ReadOnly = true, Title = "IdEmpresaVenda", Type = ColumnType.Hidden, Width = "0px",
                        AllowEmpty = false
                    },
                    new Column
                    {
                        ReadOnly = true, Title = "IdItemRegraComissao", Type = ColumnType.Hidden, Width = "0px",
                        AllowEmpty = false
                    },
                    new Column
                    {
                        Title = GlobalMessages.ColunaFaixaUmMeio, Type = ColumnType.Numeric,
                        Width = "70px", ReadOnly = !editable, DecimalCount = 2,
                        AllowEmpty = false
                    },
                    new Column
                    {
                        Title = GlobalMessages.ColunaFaixaDois, Type = ColumnType.Numeric,
                        Width = "70px", ReadOnly = !editable, DecimalCount = 2,
                        AllowEmpty = false
                    },
                    new Column
                    {
                        Title = GlobalMessages.ColunaValorKitCompleto, Type = ColumnType.Numeric,
                        Width = "70px", DecimalCount = 2,
                        ReadOnly = !editable, AllowEmpty = false
                    },
                    new Column
                    {
                        Title = GlobalMessages.ColunaValorConformidade, Type = ColumnType.Numeric,
                        Width = "70px", DecimalCount = 2,
                        ReadOnly = !editable, AllowEmpty = false
                    },
                    new Column
                    {
                        Title = GlobalMessages.ColunaValorRepasse, Type = ColumnType.Numeric,
                        Width = "70px", ReadOnly = !editable, DecimalCount = 2,
                        AllowEmpty = false
                    }
                });
            }

            return options;
        }

        private JExcelOptions InsertNominalColumnsMatriz(JExcelOptions options, int numEvs, bool editable)
        {
            options.Columns = new List<Column>
            {
                new Column
                {
                    ReadOnly = true, Title = GlobalMessages.Empreendimentos, Type = ColumnType.Text, Width = "165px",
                    WordWrap = true
                },
                new Column {ReadOnly = true, Title = "IdRegraComissao", Type = ColumnType.Hidden, Width = "0px"},
                new Column {ReadOnly = true, Title = "IdEmpreendimento", Type = ColumnType.Hidden, Width = "0px"},
            };

            for (var i = 0; i < numEvs; i++)
            {
                options.Columns.AddRange(new List<Column>
                {
                    new Column
                    {
                        ReadOnly = true, Title = "IdEmpresaVenda", Type = ColumnType.Hidden, Width = "0px",
                        AllowEmpty = false
                    },
                    new Column
                    {
                        ReadOnly = true, Title = "IdItemRegraComissao", Type = ColumnType.Hidden, Width = "0px",
                        AllowEmpty = false
                    },
                    //faixa 1.5
                    new Column
                    {
                        Title = GlobalMessages.ColunaMenorValorNominalUmMeio, Type = ColumnType.Numeric,
                        Width = "150px", DecimalCount = 2,
                        ReadOnly = !editable, AllowEmpty = false
                    },
                    new Column
                    {
                        Title = GlobalMessages.ColunaIgualValorNominalUmMeio, Type = ColumnType.Numeric,
                        Width = "150px", DecimalCount = 2,
                        ReadOnly = !editable, AllowEmpty = false
                    },
                    new Column
                    {
                        Title = GlobalMessages.ColunaMaiorValorNominalUmMeio, Type = ColumnType.Numeric,
                        Width = "150px", ReadOnly = !editable, DecimalCount = 2,
                        AllowEmpty = false
                    },
                    //faixa 2.0
                    new Column
                    {
                        Title = GlobalMessages.ColunaMenorValorNominalDois, Type = ColumnType.Numeric,
                        Width = "150px", DecimalCount = 2,
                        ReadOnly = !editable, AllowEmpty = false
                    },
                    new Column
                    {
                        Title = GlobalMessages.ColunaIgualValorNominalDois, Type = ColumnType.Numeric,
                        Width = "150px", DecimalCount = 2,
                        ReadOnly = !editable, AllowEmpty = false
                    },
                    new Column
                    {
                        Title = GlobalMessages.ColunaMaiorValorNominalDois, Type = ColumnType.Numeric,
                        Width = "150px", ReadOnly = !editable, DecimalCount = 2,
                        AllowEmpty = false
                    },
                    //faixa PNE
                    new Column
                    {
                        Title = GlobalMessages.ColunaMenorValorNominalPNE, Type = ColumnType.Numeric,
                        Width = "150px", DecimalCount = 2,
                        ReadOnly = !editable, AllowEmpty = false
                    },
                    new Column
                    {
                        Title = GlobalMessages.ColunaIgualValorNominalPNE, Type = ColumnType.Numeric,
                        Width = "150px", DecimalCount = 2,
                        ReadOnly = !editable, AllowEmpty = false
                    },
                    new Column
                    {
                        Title = GlobalMessages.ColunaMaiorValorNominalPNE, Type = ColumnType.Numeric,
                        Width = "150px", ReadOnly = !editable, DecimalCount = 2,
                        AllowEmpty = false
                    },
                    new Column
                    {
                        Title = GlobalMessages.ColunaValorKitCompleto, Type = ColumnType.Numeric,
                        Width = "70px", DecimalCount = 2,
                        ReadOnly = !editable, AllowEmpty = false
                    },
                    new Column
                    {
                        Title = GlobalMessages.ColunaValorConformidade, Type = ColumnType.Numeric,
                        Width = "70px", DecimalCount = 2,
                        ReadOnly = !editable, AllowEmpty = false
                    },
                    new Column
                    {
                        Title = GlobalMessages.ColunaValorRepasse, Type = ColumnType.Numeric,
                        Width = "70px", ReadOnly = !editable, DecimalCount = 2,
                        AllowEmpty = false
                    }
                });
            }

            return options;
        }

        private JExcelOptions InsertDataMatriz(JExcelOptions options, List<Empreendimento> empreendimentos,
            List<Tenda.Domain.EmpresaVenda.Models.EmpresaVenda> empresasVendas, long idRegraComissao,
            List<ItemRegraComissao> regrasComissaoExistentes, List<ItemRegraComissao> itensAtivos,bool novo,
            bool ultimaAtt, TipoModalidadeComissao modalidade)
        {
            options.Data = new List<List<object>>();

            if (itensAtivos == null)
            {
                itensAtivos = new List<ItemRegraComissao>();
            }

            foreach (var empreendimento in empreendimentos)
            {
                var row = new List<object>
                {
                    empreendimento.Nome,
                    idRegraComissao,
                    empreendimento.Id
                };
                foreach (var empresaVenda in empresasVendas)
                {
                    row.Add(empresaVenda.Id);

                    var itemRegraComissao = BuscarItemRegraComissaoMatriz(idRegraComissao,regrasComissaoExistentes,
                        itensAtivos,empreendimento, empresaVenda, ultimaAtt, modalidade);

                    row.AddRange(new List<Object>
                    {
                        novo ? 0 : itemRegraComissao.Id,
                        itemRegraComissao.FaixaUmMeio.ToString().Replace(".", ","),
                        itemRegraComissao.FaixaDois.ToString().Replace(".", ","),
                        itemRegraComissao.ValorKitCompleto.ToString().Replace(".", ","),
                        itemRegraComissao.ValorConformidade.ToString().Replace(".", ","),
                        itemRegraComissao.ValorRepasse.ToString().Replace(".", ",")
                    });
                }

                options.Data.Add(row);
            }

            return options;
        }

        private JExcelOptions InsertNominalDataMatriz(JExcelOptions options, List<Empreendimento> empreendimentos,
            List<Tenda.Domain.EmpresaVenda.Models.EmpresaVenda> empresasVendas, long idRegraComissao, 
            List<ItemRegraComissao> regrasComissaoExistentes,List<ItemRegraComissao> itensAtivos, bool novo,
            bool ultimaAtt, TipoModalidadeComissao modalidade)
        {
            options.Data = new List<List<object>>();

            if (itensAtivos == null)
            {
                itensAtivos = new List<ItemRegraComissao>();
            }

            foreach (var empreendimento in empreendimentos)
            {
                var row = new List<object>
                {
                    empreendimento.Nome,
                    idRegraComissao,
                    empreendimento.Id
                };
                foreach (var empresaVenda in empresasVendas)
                {
                    row.Add(empresaVenda.Id);

                    var itemRegraComissao = BuscarItemRegraComissaoMatriz(idRegraComissao, regrasComissaoExistentes,
                        itensAtivos, empreendimento, empresaVenda, ultimaAtt, modalidade);

                    row.AddRange(new List<Object>
                    {
                        novo ? 0 : itemRegraComissao.Id,
                        itemRegraComissao.MenorValorNominalUmMeio.ToString().Replace(".", ","),
                        itemRegraComissao.IgualValorNominalUmMeio.ToString().Replace(".", ","),
                        itemRegraComissao.MaiorValorNominalUmMeio.ToString().Replace(".", ","),

                        itemRegraComissao.MenorValorNominalDois.ToString().Replace(".", ","),
                        itemRegraComissao.IgualValorNominalDois.ToString().Replace(".", ","),
                        itemRegraComissao.MaiorValorNominalDois.ToString().Replace(".", ","),

                        itemRegraComissao.MenorValorNominalPNE.ToString().Replace(".", ","),
                        itemRegraComissao.IgualValorNominalPNE.ToString().Replace(".", ","),
                        itemRegraComissao.MaiorValorNominalPNE.ToString().Replace(".", ","),

                        itemRegraComissao.ValorKitCompleto.ToString().Replace(".", ","),
                        itemRegraComissao.ValorConformidade.ToString().Replace(".", ","),
                        itemRegraComissao.ValorRepasse.ToString().Replace(".", ",")
                    });
                }

                options.Data.Add(row);
            }

            return options;
        }

        private ItemRegraComissao BuscarItemRegraComissaoMatriz(long idRegraComissao,List<ItemRegraComissao> regrasComissaoExistentes,
            List<ItemRegraComissao>itensAtivos,Empreendimento empreendimento, 
            Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda, bool ultimaAtt,
            TipoModalidadeComissao modalidade)
        {
            ItemRegraComissao itemRegraComissao = null;
            if (ultimaAtt)
            {
                itemRegraComissao = itensAtivos.Where(x => x.RegraComisao.Id == idRegraComissao)
                    .Where(x => x.EmpresaVenda.Id == empresaVenda.Id)
                    .Where(x => x.Empreendimento.Id == empreendimento.Id)
                    .Where(x => x.TipoModalidadeComissao == modalidade)
                    .SingleOrDefault();                
            }
            else
            {
                itemRegraComissao = regrasComissaoExistentes
                    .Where(reg => reg.Empreendimento.Id == empreendimento.Id)
                    .Where(reg => reg.EmpresaVenda.Id == empresaVenda.Id)
                    .Where(reg => reg.TipoModalidadeComissao == modalidade)
                    .SingleOrDefault();
            }

            if (itemRegraComissao == null)
            {
                itemRegraComissao = new ItemRegraComissao();
                itemRegraComissao.Empreendimento = empreendimento;
                itemRegraComissao.EmpresaVenda = empresaVenda;
            }

            return itemRegraComissao;
        }        
        
        public RegraComissao SalvarRegraComissao(RegraComissao regraComissao)
        {
            var bre = new BusinessRuleException();

            var valido = new RegraComissaoValidator(_regraComissaoRepository).Validate(regraComissao);
            bre.WithFluentValidation(valido);
            bre.ThrowIfHasError();

            if (regraComissao.Id > 0)
            {
                var found = _regraComissaoRepository.FindById(regraComissao.Id);
                _regraComissaoRepository._session.Evict(found);
                if (found == null)
                {
                    bre.AddError(string.Format(GlobalMessages.RegistroNaoEncontrado, GlobalMessages.RegraComissao,
                            regraComissao.Id))
                        .AddField("Descricao").Complete();
                }
                else
                {
                    found.Descricao = regraComissao.Descricao;
                    if (found.Tipo == TipoRegraComissao.Campanha)
                    {
                        found.InicioVigencia = regraComissao.InicioVigencia;
                        found.TerminoVigencia = regraComissao.TerminoVigencia;
                    }
                    regraComissao = found;
                }
            }
            else
            {
                regraComissao.Situacao = SituacaoRegraComissao.Rascunho;
                regraComissao.HashDoubleCheck = "pendente";
                regraComissao.ContentTypeDoubleCheck = "pendente";
                regraComissao.IdArquivoDoubleCheck = 0;
                regraComissao.NomeDoubleCheck = "pendente";
            }

            bre.ThrowIfHasError();

            if (regraComissao.Tipo == TipoRegraComissao.Campanha)
            {
                TimeSpan timeSpan = new TimeSpan(23, 59, 59);
                var addHoras = regraComissao.TerminoVigencia.Value.Add(timeSpan);
                regraComissao.TerminoVigencia = addHoras;
            }

            _regraComissaoRepository.Save(regraComissao);

            return regraComissao;
        }

        public List<ItemRegraComissao> SalvarItensRegraComissao(List<ItemRegraComissao> itens, RegraComissao regraComissao)
        {
            var bre = new BusinessRuleException();

            foreach (var item in itens)
            {
                item.RegraComisao = new RegraComissao();
                item.RegraComisao.Id = regraComissao.Id;

                var validate = _itemRegraComissaoValidator.Validate(item);
                bre.WithFluentValidation(validate);

                _itemRegraComissaoRepository.Save(item);

            }

            bre.ThrowIfHasError();

            return itens;
        }

        public RegraComissao SalvarMatriz(List<ItemRegraComissao> itens, RegraComissao regraComissao, UsuarioPortal responsavel)
        {
            var bre = new BusinessRuleException();

            //Salvando regra de comissão
            regraComissao = SalvarRegraComissao(regraComissao);

            //Salvando itens de regra de comissão
            itens = SalvarItensRegraComissao(itens, regraComissao);

            //Criando regras de comissão evs

            //Lista das evs
            var empresas = itens.Select(x => x.EmpresaVenda.Id).Distinct().ToList();

            var regrasExistentes = _regraComissaoEvsRepository.Queryable()
                .Where(x => x.RegraComissao.Id == regraComissao.Id)
                .Where(x => empresas.Contains(x.EmpresaVenda.Id))
                .ToList();

            RegraComissaoEvs regraEvs = null;
            
            foreach (var idEv in empresas)
            {
                if (regrasExistentes.HasValue())
                {
                    regraEvs = regrasExistentes.Where(x => x.EmpresaVenda.Id == idEv)
                        .SingleOrDefault();
                }
                else
                {
                    regraEvs = null;
                }

                if (regraEvs.IsEmpty())
                {
                    regraEvs = new RegraComissaoEvs();
                    regraEvs.EmpresaVenda = new Tenda.Domain.EmpresaVenda.Models.EmpresaVenda { Id = idEv };
                    regraEvs.RegraComissao = regraComissao;
                    regraEvs.Regional = regraComissao.Regional;
                    regraEvs.Situacao = SituacaoRegraComissao.Rascunho;
                    regraEvs.HashDoubleCheck = "pendente";
                    regraEvs.ContentTypeDoubleCheck = "pendente";
                    regraEvs.IdArquivoDoubleCheck = 0;
                    regraEvs.NomeDoubleCheck = "pendente";
                    regraEvs.Tipo = regraComissao.Tipo;
                }

                if (regraEvs.Tipo == TipoRegraComissao.Campanha)
                {
                    regraEvs.InicioVigencia = regraComissao.InicioVigencia;
                    regraEvs.TerminoVigencia = regraComissao.TerminoVigencia;
                }

                regraEvs.Descricao = regraComissao.Descricao;
                _regraComissaoEvsRepository.Save(regraEvs);

                _historicoRegraComissaoService.CriarOuAvancar(regraEvs, responsavel);

            }

            bre.ThrowIfHasError();

            return regraComissao;
        }
                
        public RegraComissao GerarPdf(long idRegraComissao, byte[] logo,RegraComissaoEvs regraComissaoEvs)
        {
            var transaction = _session.Transaction;

            transaction.Begin();

            var regraComissao = _regraComissaoRepository.FindById(idRegraComissao);

            Arquivo prevArquivo = null;
            if (regraComissao.Arquivo.HasValue())
            {
                prevArquivo = regraComissao.Arquivo;
            }

            var files = _regraComissaoPdfService.CriarPdf(regraComissao, logo);
            var file = files[0];

            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            var fileName = $"RegraComissao_{regraComissao.Regional}_{regraComissao.Id}_{date}.pdf";

            var arquivo = _arquivoService.CreateFile(file, fileName, fileName, "application/pdf", file.Length);

            if (regraComissao.IdArquivoDoubleCheck == 0 && regraComissao.Situacao == SituacaoRegraComissao.Ativo)
            {
                regraComissao.ContentTypeDoubleCheck = arquivo.ContentType;
                regraComissao.HashDoubleCheck = arquivo.Hash;
                regraComissao.IdArquivoDoubleCheck = arquivo.Id;
                regraComissao.NomeDoubleCheck = arquivo.Nome;
            }

            regraComissao.Arquivo = arquivo;

            _regraComissaoRepository.Save(regraComissao);

            var regrasEvs = _regraComissaoEvsRepository.BuscarPorRegraComissao(regraComissao.Id);

            foreach (var key in files.Keys)
            {
                if (key == 0)
                {
                    continue;
                }

                var empresaVenda = _empresaVendaRepository.FindById(key);

                _regraComissaoEvsService.SalvarArquivo(files[key], regraComissao, empresaVenda,regraComissaoEvs);
            }

            if (prevArquivo.HasValue() && regraComissao.IdArquivoDoubleCheck != prevArquivo.Id)
            {
                _arquivoRepository.Delete(prevArquivo);
            }

            transaction.Commit();

            return regraComissao;
        }

        public void FinalizarRegraComissao(List<long>idsEmpresaVenda, UsuarioPortal responsavel)
        {
            var regrasComissaoEvs = _regraComissaoEvsRepository.Queryable()
                .Where(x => idsEmpresaVenda.Contains(x.EmpresaVenda.Id))
                .Where(x => x.Situacao == SituacaoRegraComissao.Ativo ||
                x.Situacao == SituacaoRegraComissao.SuspensoPorCampanha ||
                x.Situacao == SituacaoRegraComissao.AguardandoLiberacao)
                .OrderBy(x => x.Id)
                .ToList();

            //Campanhas ativas
            var campanhasAtivas = regrasComissaoEvs
                .Where(x => x.Tipo == TipoRegraComissao.Campanha)
                .Where(x => x.Situacao == SituacaoRegraComissao.Ativo)
                .ToList();

            //finalizar campanhas
            FinalizarRegraComissao(campanhasAtivas, responsavel);

            //Suspensas por Campanha
            var suspensasPorCampanha = regrasComissaoEvs
                .Where(x => x.Situacao == SituacaoRegraComissao.SuspensoPorCampanha)
                .ToList();

            FinalizarRegraComissao(suspensasPorCampanha, responsavel);

            var regrasAtivas = regrasComissaoEvs
                .Where(x => x.Situacao == SituacaoRegraComissao.Ativo)
                .ToList();

            FinalizarRegraComissao(regrasAtivas, responsavel);

            var aguardandoLiberacao= regrasComissaoEvs
                .Where(x => x.Situacao == SituacaoRegraComissao.AguardandoLiberacao)
                .ToList();

            FinalizarRegraComissao(aguardandoLiberacao, responsavel);

            FinalizarRegraComissaoPai(regrasComissaoEvs);
        }

        public void FinalizarRegraComissao(List<RegraComissaoEvs> regras, UsuarioPortal responsavel)
        {
            foreach(var regra in regras)
            {
                if (regra.Situacao == SituacaoRegraComissao.AguardandoLiberacao&&regra.Tipo==TipoRegraComissao.Campanha)
                {
                    regra.InicioVigencia = DateTime.Now;
                }

                regra.TerminoVigencia = DateTime.Now;
                regra.Situacao = SituacaoRegraComissao.Vencido;
                _regraComissaoEvsRepository.Save(regra);
                _historicoRegraComissaoService.CriarOuAvancar(regra, responsavel);
            }
        }

        /*
         * Finalizando regras de comissão pai sem filhos ativos
         */
        public void FinalizarRegraComissaoPai(List<RegraComissaoEvs> regras)
        {
            //Encerrando Pai sem Filhos Ativos
            var regrasPai = regras.Select(x => x.RegraComissao).Distinct();

            foreach (var pai in regrasPai)
            {
                var filhoAtivo = _regraComissaoEvsRepository.Queryable()
                    .Where(x => x.RegraComissao.Id == pai.Id)
                    .Where(x => x.Situacao == SituacaoRegraComissao.Ativo)
                    .Where(x => !regras.Contains(x))
                    .Any();

                if (!filhoAtivo)
                {
                    pai.TerminoVigencia = DateTime.Now;
                    pai.Situacao = SituacaoRegraComissao.Vencido;
                    _regraComissaoRepository.Save(pai);
                }
            }
        }

        public void AtribuirRegraPadrao(List<long> idsEmpresaVenda,UsuarioPortal responsavel)
        {
            FinalizarRegraComissao(idsEmpresaVenda, responsavel);

            //busca as evs
            var empresasVendas = _empresaVendaRepository.Queryable()
                .Where(x => idsEmpresaVenda.Contains(x.Id))
                .ToList();

            var estados = empresasVendas.Select(x => x.Estado).Distinct();

            //buscar regras de comissão padrão ativas
            var regrasComissaoPadraoAtivas = _regraComissaoPadraoRepository.Queryable()
                .Where(x => estados.Contains(x.Regional.ToUpper()))
                .Where(x => x.Situacao == SituacaoRegraComissao.Ativo)
                .ToList();

            var idsRegraPadraoAtiva = regrasComissaoPadraoAtivas.Select(x => x.Id);

            //buscar itens padrao
            var itensRegraComissaoPadraoAtiva = _itemRegraComissaoPadraoRepository.Queryable()
                .Where(x => idsRegraPadraoAtiva.Contains(x.RegraComissaoPadrao.Id))
                .OrderBy(x=>x.Empreendimento.Nome)
                .ToList();

            foreach (var ev in empresasVendas)
            {
                var regraPadrao = regrasComissaoPadraoAtivas.Where(x => x.Regional.ToUpper().Equals(ev.Estado.ToUpper())).SingleOrDefault();
                var itensPadrao = itensRegraComissaoPadraoAtiva.Where(x => x.RegraComissaoPadrao.Id == regraPadrao.Id).ToList();

                // atribuindo regra de comissão padrão
                AtribuirRegraPadrao(ev, regraPadrao, itensPadrao, responsavel);

            }
        }

        #region Regra de Comissão Padrão Job
        public void AtribuirRegraPadrao(Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda, RegraComissaoPadrao regraComissaoPadrao, List<ItemRegraComissaoPadrao> itensRegraComissaoPadrao, UsuarioPortal responsavel)
        {
            //criando regra de comissão
            var regraComissao = new RegraComissao();
            regraComissao.Regional = regraComissaoPadrao.Regional;
            regraComissao.Descricao = regraComissaoPadrao.Descricao;
            regraComissao.Tipo = TipoRegraComissao.Normal;
            regraComissao.Situacao = regraComissaoPadrao.Situacao;
            regraComissao.InicioVigencia = DateTime.Now;
            regraComissao.HashDoubleCheck = "pendente";
            regraComissao.ContentTypeDoubleCheck = "pendente";
            regraComissao.IdArquivoDoubleCheck = 0;
            regraComissao.NomeDoubleCheck = "pendente";
            regraComissao.CriadoPor = responsavel.Id;

            _regraComissaoRepository.Save(regraComissao);

            var itensRegraComissao = new List<ItemRegraComissao>();

            //criando itens de regra de comissão
            foreach (var itemPadrao in itensRegraComissaoPadrao)
            {
                var item = new ItemRegraComissao();
                item.EmpresaVenda = new Tenda.Domain.EmpresaVenda.Models.EmpresaVenda
                {
                    Id = empresaVenda.Id
                };
                item.Empreendimento = new Empreendimento
                {
                    Id = itemPadrao.Empreendimento.Id
                };
                item.RegraComisao = regraComissao;
                item.FaixaUmMeio = itemPadrao.FaixaUmMeio;
                item.FaixaDois = itemPadrao.FaixaDois;
                item.ValorConformidade = itemPadrao.ValorConformidade;
                item.ValorRepasse = itemPadrao.ValorRepasse;
                item.ValorKitCompleto = itemPadrao.ValorKitCompleto;
                item.MenorValorNominalUmMeio = itemPadrao.MenorValorNominalUmMeio;
                item.IgualValorNominalUmMeio = itemPadrao.IgualValorNominalUmMeio;
                item.MaiorValorNominalUmMeio = itemPadrao.MaiorValorNominalUmMeio;
                item.MenorValorNominalDois = itemPadrao.MenorValorNominalDois;
                item.IgualValorNominalDois = itemPadrao.IgualValorNominalDois;
                item.MaiorValorNominalDois = itemPadrao.MaiorValorNominalDois;
                item.MenorValorNominalPNE = itemPadrao.MenorValorNominalPNE;
                item.IgualValorNominalPNE = itemPadrao.IgualValorNominalPNE;
                item.MaiorValorNominalPNE = itemPadrao.MaiorValorNominalPNE;
                item.TipoModalidadeComissao = itemPadrao.Modalidade;
                item.CriadoPor = responsavel.Id;

                _itemRegraComissaoRepository.Save(item);
                itensRegraComissao.Add(item);
            }

            //Criando Regra de Comissão EVS
            var regraComissaoEvs = new RegraComissaoEvs();
            regraComissaoEvs.Descricao = regraComissao.Descricao;
            regraComissaoEvs.EmpresaVenda = empresaVenda;
            regraComissaoEvs.RegraComissao = regraComissao;
            regraComissaoEvs.Regional = regraComissao.Regional;
            regraComissaoEvs.Situacao = regraComissao.Situacao;
            regraComissaoEvs.HashDoubleCheck = "pendente";
            regraComissaoEvs.ContentTypeDoubleCheck = "pendente";
            regraComissaoEvs.IdArquivoDoubleCheck = 0;
            regraComissaoEvs.NomeDoubleCheck = "pendente";
            regraComissaoEvs.Tipo = regraComissao.Tipo;
            regraComissaoEvs.InicioVigencia = DateTime.Now;
            regraComissaoEvs.CriadoPor = responsavel.Id;

            _regraComissaoEvsRepository.Save(regraComissaoEvs);

            //Criando histórico
            _historicoRegraComissaoService.CriarOuAvancar(regraComissaoEvs, responsavel);

            //Criando arquivo
            var fileName = "LogoTendaPdf.png";
            var TargetPath = @"/Static/images/";
            var path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + TargetPath + fileName;
            byte[] logo = System.IO.File.ReadAllBytes(path);

            GerarPdfJob(empresaVenda, regraComissao, regraComissaoEvs, itensRegraComissao, logo);

            //Notificar empresa de venda
            EnviarNotificacaoRegraAtualizada(empresaVenda, responsavel.Id);
        }

        //JOB - 1
        public RegraComissao GerarPdfJob(Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda,RegraComissao regraComissao,RegraComissaoEvs regraComissaoEvs,List<ItemRegraComissao> itens,byte[] logo)
        {
            var file = _regraComissaoPdfService.CriarPdfJob(regraComissao, regraComissaoEvs,itens, logo);

            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            var fileName = $"RegraComissao_{regraComissao.Regional}_{regraComissao.Id}_{date}.pdf";

            var arquivo = _arquivoService.CreateFile(file, fileName, fileName, "application/pdf", file.Length);

            regraComissao.ContentTypeDoubleCheck = arquivo.ContentType;
            regraComissao.HashDoubleCheck = arquivo.Hash;
            regraComissao.IdArquivoDoubleCheck = arquivo.Id;
            regraComissao.NomeDoubleCheck = arquivo.Nome;

            regraComissao.Arquivo = arquivo;

            _regraComissaoRepository.Save(regraComissao);

            _regraComissaoEvsService.SalvarArquivoJob(file, regraComissao, empresaVenda, regraComissaoEvs);

            return regraComissao;
        }
        #endregion
        //1
        public RegraComissao GerarPdf(long idRegraComissao, byte[] logo)
        {
            var transaction = _session.Transaction;

            transaction.Begin();

            var regraComissao = _regraComissaoRepository.FindById(idRegraComissao);

            Arquivo prevArquivo = null;
            if (regraComissao.Arquivo.HasValue())
            {
                prevArquivo = regraComissao.Arquivo;
            }

            var files = _regraComissaoPdfService.CriarPdf(regraComissao, logo);
            var file = files[0];

            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            var fileName = $"RegraComissao_{regraComissao.Regional}_{regraComissao.Id}_{date}.pdf";

            var arquivo = _arquivoService.CreateFile(file, fileName, fileName, "application/pdf", file.Length);

            if (regraComissao.IdArquivoDoubleCheck == 0 && regraComissao.Situacao == SituacaoRegraComissao.Ativo)
            {
                regraComissao.ContentTypeDoubleCheck = arquivo.ContentType;
                regraComissao.HashDoubleCheck = arquivo.Hash;
                regraComissao.IdArquivoDoubleCheck = arquivo.Id;
                regraComissao.NomeDoubleCheck = arquivo.Nome;
            }

            regraComissao.Arquivo = arquivo;

            _regraComissaoRepository.Save(regraComissao);

            var regrasEvs = _regraComissaoEvsRepository.BuscarPorRegraComissao(regraComissao.Id);

            foreach (var key in files.Keys)
            {
                if (key == 0)
                {
                    continue;
                }

                var empresaVenda = _empresaVendaRepository.FindById(key);

                var regraComissaoEvs = regrasEvs.Where(x => x.EmpresaVenda.Id == key).FirstOrDefault();

                _regraComissaoEvsService.SalvarArquivo(files[key], regraComissao, empresaVenda, regraComissaoEvs);
            }

            if (prevArquivo.HasValue() && regraComissao.IdArquivoDoubleCheck != prevArquivo.Id)
            {
                _arquivoRepository.Delete(prevArquivo);
            }

            transaction.Commit();

            return regraComissao;
        }

        public byte[] GerarExcel(long idRegraComissao, long[] listaEvs, string regional, bool ultimaAtt)
        {
            var regraComissao = _regraComissaoRepository.FindById(idRegraComissao);

            if (regraComissao.IsEmpty())
            {
                regraComissao = new RegraComissao();
            }

            var empreendimentos = _enderecoEmpreendimentoRepository
                .EnderecosDaRegional(regional)
                .Select(reg => reg.Empreendimento)
                .OrderByDescending(x => x.PriorizarRegraComissao)
                .ThenBy(x => x.Nome)
                .ToList();
            List<Tenda.Domain.EmpresaVenda.Models.EmpresaVenda> empresasVendas;
            if (regraComissao.Id > 0 && listaEvs.IsEmpty())
            {
                empresasVendas = _regraComissaoEvsRepository.BuscarPorRegraComissao(regraComissao.Id)
                    .Select(x => x.EmpresaVenda)
                    .ToList();
            }
            else
            {
                empresasVendas = _empresaVendaRepository.EmpresasDaRegional(regional);
            }

            if (listaEvs.HasValue())
            {
                empresasVendas = empresasVendas.Where(x => listaEvs.Contains(x.Id)).ToList();
            }

            empresasVendas = empresasVendas.OrderBy(x => x.NomeFantasia)
                .ToList();

            var regrasComissaoExistentes = _itemRegraComissaoRepository.ItensDeRegra(idRegraComissao);

            ExcelUtil excel = ExcelUtil.NewInstance(35);

            excel = ConstruirExcel(excel, empresasVendas, idRegraComissao, regrasComissaoExistentes,
                ultimaAtt, TipoModalidadeComissao.Fixa);
            excel = ConstruirExcel(excel, empresasVendas, idRegraComissao, regrasComissaoExistentes,
                ultimaAtt, TipoModalidadeComissao.Nominal);

            excel.Close();
            return excel.DownloadFile();
        }

        public ExcelUtil ConstruirExcel(ExcelUtil excel, List<Tenda.Domain.EmpresaVenda.Models.EmpresaVenda> empresasVendas,
            long idRegraComissao,List<ItemRegraComissao> regrasComissaoExistentes, bool ultimaAtt, TipoModalidadeComissao modalidade)
        {
            //necessário pois utiliza o mesmo método de contrução da matriz
            var itensAtivos = new List<ItemRegraComissao>();

            regrasComissaoExistentes = regrasComissaoExistentes.Where(x => x.TipoModalidadeComissao == modalidade).ToList();

            var empreendimentos = regrasComissaoExistentes.Select(x => x.Empreendimento).Distinct().ToList();

            excel.NewSheet(modalidade.AsString());

            var currentWorksheet = excel.CurrentExcelWorksheet();
            currentWorksheet.Cells[1, 1].Value = "";
            currentWorksheet.Cells[2, 1].Value = GlobalMessages.Empreendimento;
            currentWorksheet.Cells[1, 1].Style.Font.Size = 12;
            currentWorksheet.Cells[2, 1].Style.Font.Size = 12;
            currentWorksheet.Cells[1, 1].Style.Font.Bold = true;
            currentWorksheet.Cells[2, 1].Style.Font.Bold = true;
            currentWorksheet.Cells[1, 1].AutoFitColumns(35);
            currentWorksheet.Cells[2, 1].AutoFitColumns(35);

            for (var i = 0; i < empresasVendas.Count; i++)
            {
                var empresaVenda = empresasVendas[i];
                int idx = 1;
                int qtdCol = 0;
                switch (modalidade)
                {
                    case TipoModalidadeComissao.Fixa:
                        qtdCol = 5;
                        var cellFixaTitle = currentWorksheet.Cells[1, 2 + i * qtdCol];
                        cellFixaTitle.Value = empresaVenda.NomeFantasia;
                        cellFixaTitle.Style.Font.Bold = true;
                        cellFixaTitle.Style.Font.Size = 12;
                        var cellFixaMerge = currentWorksheet.Cells[1, 2 + i * qtdCol, 1, 6 + i * qtdCol];
                        cellFixaMerge.Merge = true;

                        currentWorksheet.Cells[2, 2 + i * qtdCol].Value = GlobalMessages.ColunaFaixaUmMeio;
                        currentWorksheet.Cells[2, 2 + i * qtdCol].Style.Font.Bold = true;
                        currentWorksheet.Cells[2, 2 + i * qtdCol].Style.Font.Size = 12;

                        currentWorksheet.Cells[2, 3 + i * qtdCol].Value = GlobalMessages.ColunaFaixaDois;
                        currentWorksheet.Cells[2, 3 + i * qtdCol].Style.Font.Bold = true;
                        currentWorksheet.Cells[2, 3 + i * qtdCol].Style.Font.Size = 12;
                        idx = 0;

                        break;
                    case TipoModalidadeComissao.Nominal:
                        qtdCol = 12;
                        var cellNominalTitle = currentWorksheet.Cells[1, 2 + i * qtdCol];
                        cellNominalTitle.Value = empresaVenda.NomeFantasia;
                        cellNominalTitle.Style.Font.Bold = true;    
                        cellNominalTitle.Style.Font.Size = 12;
                        var cellNominalMerge = currentWorksheet.Cells[1, 2 + i * qtdCol, 1, 13 + i * qtdCol];
                        cellNominalMerge.Merge = true;

                        currentWorksheet.Cells[2, 2 + i * qtdCol].Value = GlobalMessages.ColunaMenorValorNominalUmMeio;
                        currentWorksheet.Cells[2, 2 + i * qtdCol].Style.Font.Bold = true;
                        currentWorksheet.Cells[2, 2 + i * qtdCol].Style.Font.Size = 12;
                        currentWorksheet.Cells[2, 2 + i * qtdCol].AutoFitColumns(30);

                        currentWorksheet.Cells[2, 3 + i * qtdCol].Value = GlobalMessages.ColunaIgualValorNominalUmMeio;
                        currentWorksheet.Cells[2, 3 + i * qtdCol].Style.Font.Bold = true;
                        currentWorksheet.Cells[2, 3 + i * qtdCol].Style.Font.Size = 12;
                        currentWorksheet.Cells[2, 3 + i * qtdCol].AutoFitColumns(30);

                        currentWorksheet.Cells[2, 4 + i * qtdCol].Value = GlobalMessages.ColunaMaiorValorNominalUmMeio;
                        currentWorksheet.Cells[2, 4 + i * qtdCol].Style.Font.Bold = true;
                        currentWorksheet.Cells[2, 4 + i * qtdCol].Style.Font.Size = 12;
                        currentWorksheet.Cells[2, 4 + i * qtdCol].AutoFitColumns(30);

                        currentWorksheet.Cells[2, 5 + i * qtdCol].Value = GlobalMessages.ColunaMenorValorNominalDois;
                        currentWorksheet.Cells[2, 5 + i * qtdCol].Style.Font.Bold = true;
                        currentWorksheet.Cells[2, 5 + i * qtdCol].Style.Font.Size = 12;
                        currentWorksheet.Cells[2, 5 + i * qtdCol].AutoFitColumns(30);

                        currentWorksheet.Cells[2, 6 + i * qtdCol].Value = GlobalMessages.ColunaIgualValorNominalDois;
                        currentWorksheet.Cells[2, 6 + i * qtdCol].Style.Font.Bold = true;
                        currentWorksheet.Cells[2, 6 + i * qtdCol].Style.Font.Size = 12;
                        currentWorksheet.Cells[2, 6 + i * qtdCol].AutoFitColumns(30);

                        currentWorksheet.Cells[2, 7 + i * qtdCol].Value = GlobalMessages.ColunaMaiorValorNominalDois;
                        currentWorksheet.Cells[2, 7 + i * qtdCol].Style.Font.Bold = true;
                        currentWorksheet.Cells[2, 7 + i * qtdCol].Style.Font.Size = 12;
                        currentWorksheet.Cells[2, 7 + i * qtdCol].AutoFitColumns(30);

                        currentWorksheet.Cells[2, 8 + i * qtdCol].Value = GlobalMessages.ColunaMenorValorNominalPNE;
                        currentWorksheet.Cells[2, 8 + i * qtdCol].Style.Font.Bold = true;
                        currentWorksheet.Cells[2, 8 + i * qtdCol].Style.Font.Size = 12;
                        currentWorksheet.Cells[2, 8 + i * qtdCol].AutoFitColumns(30);

                        currentWorksheet.Cells[2, 9 + i * qtdCol].Value = GlobalMessages.ColunaIgualValorNominalPNE;
                        currentWorksheet.Cells[2, 9 + i * qtdCol].Style.Font.Bold = true;
                        currentWorksheet.Cells[2, 9 + i * qtdCol].Style.Font.Size = 12;
                        currentWorksheet.Cells[2, 9 + i * qtdCol].AutoFitColumns(30);

                        currentWorksheet.Cells[2, 10 + i * qtdCol].Value = GlobalMessages.ColunaMaiorValorNominalPNE;
                        currentWorksheet.Cells[2, 10 + i * qtdCol].Style.Font.Bold = true;
                        currentWorksheet.Cells[2, 10 + i * qtdCol].Style.Font.Size = 12;
                        currentWorksheet.Cells[2, 10 + i * qtdCol].AutoFitColumns(30);

                        idx = 7;

                        break;
                }

                currentWorksheet.Cells[2, idx + 4 + i * qtdCol].Value = GlobalMessages.ColunaValorKitCompleto;
                currentWorksheet.Cells[2, idx + 4 + i * qtdCol].Style.Font.Bold = true;
                currentWorksheet.Cells[2, idx + 4 + i * qtdCol].Style.Font.Size = 12;
                currentWorksheet.Cells[2, idx + 5 + i * qtdCol].Value = GlobalMessages.ColunaValorConformidade;
                currentWorksheet.Cells[2, idx + 5 + i * qtdCol].Style.Font.Bold = true;
                currentWorksheet.Cells[2, idx + 5 + i * qtdCol].Style.Font.Size = 12;
                currentWorksheet.Cells[2, idx + 6 + i * qtdCol].Value = GlobalMessages.ColunaValorRepasse;
                currentWorksheet.Cells[2, idx + 6 + i * qtdCol].Style.Font.Bold = true;
                currentWorksheet.Cells[2, idx + 6 + i * qtdCol].Style.Font.Size = 12;

                for (var j = 0; j < empreendimentos.Count; j++)
                {
                    var empreendimento = empreendimentos[j];
                    if (i == 0)
                    {
                        currentWorksheet.Cells[3 + j, 1].Value = empreendimento.Nome;
                        currentWorksheet.Cells[3 + j, 1].AutoFitColumns(35);
                        currentWorksheet.Cells[3 + j, 1].Style.Font.Bold = true;
                        currentWorksheet.Cells[3 + j, 1].Style.Font.Size = 12;
                    }

                    var itemRegraComissao = BuscarItemRegraComissaoMatriz(idRegraComissao,regrasComissaoExistentes,
                        itensAtivos,empreendimento, empresaVenda, ultimaAtt, modalidade);

                    switch (modalidade)
                    {
                        case TipoModalidadeComissao.Fixa:
                            currentWorksheet.Cells[3 + j, 2 + i * qtdCol].Value = itemRegraComissao.FaixaUmMeio;
                            currentWorksheet.Cells[3 + j, 3 + i * qtdCol].Value = itemRegraComissao.FaixaDois;
                            break;
                        case TipoModalidadeComissao.Nominal:
                            currentWorksheet.Cells[3 + j, 2 + i * qtdCol].Value = itemRegraComissao.MenorValorNominalUmMeio;
                            currentWorksheet.Cells[3 + j, 3 + i * qtdCol].Value = itemRegraComissao.IgualValorNominalUmMeio;
                            currentWorksheet.Cells[3 + j, 4 + i * qtdCol].Value = itemRegraComissao.MaiorValorNominalUmMeio;

                            currentWorksheet.Cells[3 + j, 5 + i * qtdCol].Value = itemRegraComissao.MenorValorNominalDois;
                            currentWorksheet.Cells[3 + j, 6 + i * qtdCol].Value = itemRegraComissao.IgualValorNominalDois;
                            currentWorksheet.Cells[3 + j, 7 + i * qtdCol].Value = itemRegraComissao.MaiorValorNominalDois;

                            currentWorksheet.Cells[3 + j, 8 + i * qtdCol].Value = itemRegraComissao.MenorValorNominalPNE;
                            currentWorksheet.Cells[3 + j, 9 + i * qtdCol].Value = itemRegraComissao.IgualValorNominalPNE;
                            currentWorksheet.Cells[3 + j, 10 + i * qtdCol].Value = itemRegraComissao.MaiorValorNominalPNE;
                            break;
                    }

                    currentWorksheet.Cells[3 + j, idx + 4 + i * qtdCol].Value = itemRegraComissao.ValorKitCompleto;
                    currentWorksheet.Cells[3 + j, idx + 5 + i * qtdCol].Value = itemRegraComissao.ValorConformidade;
                    currentWorksheet.Cells[3 + j, idx + 6 + i * qtdCol].Value = itemRegraComissao.ValorRepasse;

                }
            }

            return excel;
        }

        public void AtualizarTerminoCampanha(ref RegraComissao regra)
        {
            var transaction = _session.BeginTransaction();
            regra.TerminoVigencia = DateTime.Now;
            _regraComissaoRepository.Save(regra);
            transaction.Commit();
        }

        private void AtualizarCampanhaEvs(long idRegraComissao)
        {
            var transaction = _session.Transaction;

            transaction.Begin();

            //regrasEvs da nova regra que estejam rascunho
            var campanhasAtivas = _regraComissaoEvsRepository.BuscarCampanhasAtivas(idRegraComissao);

            //mata os filhos ativos
            foreach (var regrasEvs in campanhasAtivas)
            {
                regrasEvs.Situacao = SituacaoRegraComissao.Vencido;
                regrasEvs.TerminoVigencia = DateTime.Now;
                _regraComissaoEvsRepository.Save(regrasEvs);
            }

            transaction.Commit();
        }

        public void InterromperCampanha(long idRegra)
        {
            BusinessRuleException bre = new BusinessRuleException();

            var regra = _regraComissaoRepository.FindById(idRegra);

            if (regra.IsEmpty())
            {
                bre.AddError(GlobalMessages.CampanhaInexistente).Complete();
                bre.ThrowIfHasError();
            }

            if (regra.Tipo != TipoRegraComissao.Campanha)
            {
                bre.AddError(GlobalMessages.ErroTipoRegraComissao).WithParam(regra.Tipo.AsString()).Complete();
                bre.ThrowIfHasError();
            }

            //Reativando regra de comissão evs
            ReativarRegraComissaoEvs(idRegra);

            //Reativando regra comissao
            ReativarRegraComissao(idRegra);

            AtualizarTerminoCampanha(ref regra);

            //Finalizar as campanhas das EVS
            AtualizarCampanhaEvs(idRegra);

            //Finalizar campanha
            FinalizarCampanha(ref regra);
        }

        public void AtivarCampanhaNoLogin(long idEmpresaVenda)
        {
            // Busca as campanhas aguardando liberação para a EV
            var campanhas = _regraComissaoEvsRepository.BuscarCampanhaAguardandoLiberacaoEv(idEmpresaVenda);
            foreach (var campanha in campanhas)
            {
                AtivarCampanhaRobo(campanha);
            }

            //Finalizar todas campanhas vencidas
            var campanhasVencidas = _regraComissaoRepository.CampanhasAguardandoInativacao();
            foreach (var campanha in campanhasVencidas)
            {
                InativarCampanha(campanha);
            }
        }

        public void LimparCamapanhasComErro()
        {
            var idsCampanhasComErro = _regraComissaoEvsRepository.CampanhasComErro().Select(x => x.Id);

            foreach (var idsCampanha in idsCampanhasComErro)
            {
                LimpaCampanhaEvs(idsCampanha);
            }
        }

        public void LimpaCampanhaEvs(long idRegraComissao)
        {
            var transaction = _session.BeginTransaction();

            var campanhas = _regraComissaoEvsRepository.BuscarPorRegraComissao(idRegraComissao);

            foreach (var campanha in campanhas)
            {
                campanha.Situacao = SituacaoRegraComissao.Vencido;
                _regraComissaoEvsRepository.Save(campanha);

                var responsavel = new UsuarioPortal()
                {
                    Id = ProjectProperties.IdUsuarioSistema
                };
                _historicoRegraComissaoService.CriarOuAvancar(campanha, responsavel);


            }

            transaction.Commit();
        }
    }
}