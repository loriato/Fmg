using Europa.Commons;
using Europa.Data;
using Europa.Extensions;
using Europa.Resources;
using NHibernate.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.ApiService.Models.Arquivo;
using Tenda.EmpresaVenda.ApiService.Models.EnderecoBreveLancamento;
using Tenda.EmpresaVenda.ApiService.Models.Produto;
using Tenda.EmpresaVenda.ApiService.Models.StaticResource;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Validators;
using Tenda.EmpresaVenda.ApiService.Models.EnderecoEmpreendimento;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class BreveLancamentoService : BaseService
    {
        private BreveLancamentoRepository _breveLancamentoRepository { get; set; }
        private EnderecoBreveLancamentoRepository _enderecoBreveLancamentoRepository { get; set; }
        private EmpreendimentoRepository _empreendimentoRepository { get; set; }
        private EnderecoEmpreendimentoRepository _enderecoEmpreendimentoRepository { get; set; }
        private NotificacaoRepository _notificacaoRepository { get; set; }
        private CorretorRepository _corretorRepository { get; set; }
        private ViewArquivoBreveLancamentoRepository _viewArquivoBreveLancamentoRepository { get; set; }
        private StaticResourceService _staticResourceService { get; set; }
        private ViewArquivoEmpreendimentoRepository _viewArquivoEmpreendimentoRepository { get; set; }
        private ArquivoRepository _arquivoRepository { get; set; }
        private RegionalEmpresaRepository _regionalEmpresaRepository { get; set; }

        public BreveLancamento Salvar(BreveLancamento breveLancamento, BusinessRuleException bre)
        {
            if (!breveLancamento.Empreendimento.HasValue())
            {
                breveLancamento.Empreendimento = null;
            }

            // Realiza as validações de BreveLancamento
            var emprResult = new BreveLancamentoValidator(_breveLancamentoRepository).Validate(breveLancamento);

            // Verifica se retornou algum erro
            bre.WithFluentValidation(emprResult);

            if (emprResult.IsValid)
            {
                _breveLancamentoRepository.Save(breveLancamento);
            }

            return breveLancamento;
        }

        public void ExcluirPorId(long idBreveLancamento)
        {
            var exc = new BusinessRuleException();
            var reg = _breveLancamentoRepository.FindById(idBreveLancamento);
            var endereco = _enderecoBreveLancamentoRepository.FindByBreveLancamento(idBreveLancamento);

            try
            {
                _enderecoBreveLancamentoRepository.Delete(endereco);
                _breveLancamentoRepository.Delete(reg);
            }
            catch (GenericADOException exp)
            {
                if (ConstraintViolationExceptionWrapper.IsConstraintViolationException(exp))
                {
                    exc.AddError(GlobalMessages.RemovidoSemSucesso).WithParam(reg.ChaveCandidata()).Complete();
                }
            }

            exc.ThrowIfHasError();
        }

        public BreveLancamento AssociarComEmpreendimento(long idBreveLancamento, long idEmpreendimento,
            BusinessRuleException bre)
        {
            var breveLancamento = _breveLancamentoRepository.FindById(idBreveLancamento);
            if (breveLancamento.IsEmpty())
            {
                bre.Errors.Add(GlobalMessages.BreveLancamentoNaoEncontrado);
                return breveLancamento;
            }

            if (breveLancamento.Empreendimento.HasValue())
            {
                bre.Errors.Add(GlobalMessages.BreveLancamentoJaPossuiEmpreendimento);
                return breveLancamento;
            }

            var empreendimento = _empreendimentoRepository.FindById(idEmpreendimento);
            if (empreendimento.IsEmpty())
            {
                bre.Errors.Add(GlobalMessages.EmpreendimentoNaoEncontrado);
                return breveLancamento;
            }

            if (_breveLancamentoRepository.Queryable().Any(x => x.Empreendimento.Id == empreendimento.Id))
            {
                bre.Errors.Add(GlobalMessages.EmpreendimentoJaAssociadoBreveLancamento);
                return breveLancamento;
            }

            breveLancamento.Empreendimento = empreendimento;
            EnderecoBreveLancamento enderecoBreveLancamento =
                _enderecoBreveLancamentoRepository.FindByBreveLancamento(breveLancamento.Id);
            breveLancamento.Regional = empreendimento.RegionalObjeto;

            //Pegamos um erro em produção que na busca dos endereços, estava buscando
            //a classe pai, e como o ids do endereço do breve lançamento era igual ao endereço do empreendimento
            //estava lançando erro, pois são classes diferentes que herdam da mesma classe.
            _session.Evict(enderecoBreveLancamento);

            EnderecoEmpreendimento enderecoEmpreendimento =
                _enderecoEmpreendimentoRepository.FindByEmpreendimento(empreendimento.Id);
            _session.Evict(enderecoEmpreendimento);

            if (enderecoBreveLancamento == null)
            {
                enderecoBreveLancamento = new EnderecoBreveLancamento();
                enderecoBreveLancamento.BreveLancamento = breveLancamento;
            }

            // Regra: Ao ser associado, o endereço do breve lançamento deve ser atualizado com as informações do Empreendimento
            enderecoBreveLancamento.Logradouro = enderecoEmpreendimento.Logradouro;
            enderecoBreveLancamento.Numero = enderecoEmpreendimento.Numero;
            enderecoBreveLancamento.Pais = enderecoEmpreendimento.Pais;
            enderecoBreveLancamento.Bairro = enderecoEmpreendimento.Bairro;
            enderecoBreveLancamento.Cep = enderecoEmpreendimento.Cep;
            enderecoBreveLancamento.Cidade = enderecoEmpreendimento.Cidade;
            enderecoBreveLancamento.Complemento = enderecoEmpreendimento.Complemento;
            enderecoBreveLancamento.Estado = enderecoEmpreendimento.Estado;

            _breveLancamentoRepository.Save(breveLancamento);
            _enderecoBreveLancamentoRepository.Save(enderecoBreveLancamento);

            NotificarAssociacao(breveLancamento);

            return breveLancamento;
        }

        private void NotificarAssociacao(BreveLancamento breveLancamento)
        {
            var corretoresAtivos = _corretorRepository.ListarAtivos();

            foreach (var corretor in corretoresAtivos)
            {
                var notificacao = new Notificacao
                {
                    Titulo = string.Format(GlobalMessages.TituloNotificacaoBreveLancamento
                        , breveLancamento.Nome),
                    Conteudo = string.Format(GlobalMessages.MensagemNotificacaoBreveLancamento
                        , breveLancamento.Nome, breveLancamento.Empreendimento.Nome),
                    Usuario = corretor.Usuario,
                    EmpresaVenda = corretor.EmpresaVenda,
                    DestinoNotificacao = Tenda.Domain.EmpresaVenda.Enums.DestinoNotificacao.Portal,
                };
                _notificacaoRepository.Save(notificacao);
            }
        }

        public List<BreveLancamento> DisponiveisParaCatalogoNoEstado(Tenda.Domain.EmpresaVenda.Models.EmpresaVenda empresaVenda)
        {
            var query = _breveLancamentoRepository.Queryable()
                .Where(reg => reg.DisponivelCatalogo);

            if (empresaVenda.HasValue())
            {
                var regionais = _regionalEmpresaRepository.ListarRegionaisPorEmpresa(empresaVenda.Id).Select(s => s.Regional).ToList();
                query = query.Where(brla => _enderecoBreveLancamentoRepository.Queryable()
                                .Where(eebl => eebl.BreveLancamento.Id == brla.Id)
                                .Where(eebl => (empresaVenda.ConsiderarUF == Tenda.Domain.EmpresaVenda.Enums.TipoSimNao.Nao && regionais.Contains(eebl.BreveLancamento.Regional)) ||
                                               (empresaVenda.ConsiderarUF == Tenda.Domain.EmpresaVenda.Enums.TipoSimNao.Sim && regionais.Contains(eebl.BreveLancamento.Regional) && eebl.Estado == empresaVenda.Estado))
                                .Any());
            }
            return query.ToList();
        }

        public BreveLancamento SalvarPrioridade(BreveLancamento breveLancamento, BusinessRuleException bre)
        {
            var objeto = _breveLancamentoRepository.FindById(breveLancamento.Id);
            objeto.Sequencia = breveLancamento.Sequencia;
            if (breveLancamento.Sequencia <= 0)
            {
                bre.Errors.Add(GlobalMessages.ErroCampoValorZero);
                bre.ErrorsFields.Add("Sequencia");
            }

            _breveLancamentoRepository.Save(objeto);

            return objeto;
        }

        public List<ProdutoViewModel> ListarProdutosApi(FiltroProdutoDto filtro)
        {
            var brevesLancamentos = DisponiveisParaCatalogoNoEstado(filtro.EmpresaVenda);

            var enderecoBreveLancamentos = _enderecoBreveLancamentoRepository.EnderecosDeBrevesLancamentosQueryable(
                    brevesLancamentos.Select(x => x.Id).ToList(), filtro.EmpresaVenda).ToList();

            var enderecos = enderecoBreveLancamentos.Select(x => new EnderecoBreveLancamentoDTO().FromDomain(x)).AsQueryable();

            var produtos = new List<ProdutoViewModel>(brevesLancamentos.Count);

            foreach (var breveLancamento in brevesLancamentos)
            {
                ProdutoViewModel produto = new ProdutoViewModel();

                produto.GeneratedAt = DateTime.Now;
                produto.IdBreveLancamento = breveLancamento.Id;
                produto.Nome = breveLancamento.Nome;
                produto.DisponivelParaVenda = breveLancamento.DisponivelCatalogo;
                produto.Informacoes = breveLancamento.Informacoes;
                produto.Endereco = enderecos.FirstOrDefault(reg => reg.BreveLancamento.Id == breveLancamento.Id).ToDomain();
                produto.Sequencia = breveLancamento.Sequencia;

                var arquivosDoEmprendimento = _viewArquivoBreveLancamentoRepository.Listar(breveLancamento.Id);

                foreach (var arquivo in arquivosDoEmprendimento)
                {
                    var arquivoDto = new ArquivoDto();
                    if (arquivo.ContentType.Equals("video"))
                    {
                        arquivoDto.Nome = arquivo.Nome;
                        arquivoDto.UrlThumbnail = "https://img.youtube.com/vi/" + arquivo.Nome + "/hqdefault.jpg";
                        arquivoDto.Url = "https://img.youtube.com/vi/" + arquivo.Nome + "/hqdefault.jpg";
                    }
                    else
                    {
                        var dataSource = _staticResourceService.LoadResource(arquivo.Id);
                        var staticResource = new StaticResourceDTO();
                        staticResource.Id = arquivo.Id;
                        staticResource.FileName = dataSource;
                        staticResource.Url = _staticResourceService.CreateUrlApi(staticResource.FileName);
                        staticResource.UrlThumbnail = _staticResourceService.CreateThumbnailUrlApi(staticResource.FileName);

                        arquivoDto.Nome = arquivo.Nome;
                        arquivoDto.Url = staticResource.Url;
                        arquivoDto.UrlThumbnail = staticResource.UrlThumbnail;
                    }

                    arquivoDto.FileExtension = arquivo.FileExtension;
                    arquivoDto.ContentType = arquivo.ContentType;
                    produto.Book.Add(arquivoDto);
                }

                var imagemPrincipal = arquivosDoEmprendimento
                    .Where(reg => reg.ContentType.ToLower().Contains("image"))
                    .OrderByDescending(reg => reg.AtualizadoEm)
                    .FirstOrDefault();

                if (imagemPrincipal.HasValue())
                {
                    var dataSource = _staticResourceService.LoadResource(imagemPrincipal.IdArquivo);
                    var staticResource = new StaticResourceDTO();
                    staticResource.FileName = dataSource;
                    staticResource.Url = _staticResourceService.CreateUrlApi(staticResource.FileName);
                    staticResource.UrlThumbnail = _staticResourceService.CreateThumbnailUrlApi(staticResource.FileName);

                    ArquivoDto arquivoDto = new ArquivoDto();
                    arquivoDto.Url = staticResource.Url;
                    arquivoDto.UrlThumbnail = staticResource.UrlThumbnail;
                    arquivoDto.FileExtension = imagemPrincipal.FileExtension;
                    arquivoDto.ContentType = imagemPrincipal.ContentType;
                    produto.ImagemPrincipal = arquivoDto;
                }

                if (breveLancamento.Empreendimento.HasValue())
                {
                    if (breveLancamento.Empreendimento.RegistroIncorporacao.HasValue())
                    {
                        var endereçoEmpreendimento = _enderecoEmpreendimentoRepository.FindByEmpreendimento(breveLancamento.Empreendimento.Id);
                        var enderecoEmpreendimentoDTO = new EnderecoEmpreendimentoDTO();
                        enderecoEmpreendimentoDTO.FromDomain(endereçoEmpreendimento);

                        produto.Endereco = enderecoEmpreendimentoDTO.ToDomain();

                        produto.VerificarEmpreendimento = true;

                        produto.Nome = breveLancamento.Empreendimento.Nome;
                        produto.Informacoes = breveLancamento.Empreendimento.Informacoes;
                        produto.ImagemPrincipal = null;
                    }

                    var arquivos = _viewArquivoEmpreendimentoRepository.Listar(breveLancamento.Empreendimento.Id);

                    var principalEmpre = arquivos
                        .Where(x => x.IdEmprendimento == breveLancamento.Empreendimento.Id)
                        .Where(reg => reg.ContentType.ToLower().Contains("image"))
                        .OrderByDescending(reg => reg.AtualizadoEm)
                        .FirstOrDefault();
                    produto.IdEmpreendimento = breveLancamento.Empreendimento.Id;

                    if (principalEmpre.HasValue())
                    {
                        var dataSource = _staticResourceService.LoadResource(principalEmpre.IdArquivo);
                        var staticResource = new StaticResourceDTO();
                        staticResource.FileName = dataSource;
                        staticResource.Url = _staticResourceService.CreateUrlApi(staticResource.FileName);
                        staticResource.UrlThumbnail = _staticResourceService.CreateThumbnailUrlApi(staticResource.FileName);

                        ArquivoDto arquivoDto = new ArquivoDto();
                        arquivoDto.Url = staticResource.Url;
                        arquivoDto.UrlThumbnail = staticResource.UrlThumbnail;
                        arquivoDto.FileExtension = principalEmpre.FileExtension;
                        arquivoDto.ContentType = principalEmpre.ContentType;
                        produto.ImagemPrincipal = arquivoDto;
                    }

                    foreach (var arquivo in arquivos)
                    {
                        ArquivoDto arquivoDto = new ArquivoDto();
                        if (arquivo.ContentType.Equals("video"))
                        {
                            arquivoDto.Nome = arquivo.Nome;
                            arquivoDto.UrlThumbnail = "https://img.youtube.com/vi/" + arquivo.Nome + "/hqdefault.jpg";
                            arquivoDto.Url = "https://img.youtube.com/vi/" + arquivo.Nome + "/hqdefault.jpg";
                            arquivoDto.FileExtension = arquivo.FileExtension;
                            arquivoDto.ContentType = arquivo.ContentType;
                        }
                        else
                        {
                            var metadata = new ArquivoDto().FromDomain(_arquivoRepository.WithNoContentAndNoThumbnail(arquivo.IdArquivo));

                            var dataSource = _staticResourceService.LoadResource(arquivo.IdArquivo);
                            var staticResource = new StaticResourceDTO();
                            staticResource.FileName = dataSource;
                            staticResource.Url = _staticResourceService.CreateUrlApi(staticResource.FileName);
                            staticResource.UrlThumbnail = _staticResourceService.CreateThumbnailUrlApi(staticResource.FileName);

                            arquivoDto.Url = staticResource.Url;
                            arquivoDto.UrlThumbnail = staticResource.UrlThumbnail;
                            arquivoDto.FileExtension = metadata.FileExtension;
                            arquivoDto.ContentType = metadata.ContentType;
                        }

                        produto.Book.Add(arquivoDto);
                    }
                }
                else
                {
                    produto.VerificarEmpreendimento = false;
                }

                produtos.Add(produto);
            }

            produtos = produtos.OrderByDescending(x => x.Sequencia != null)
                            .ThenBy(x => x.VerificarEmpreendimento)
                            .ThenBy(x => x.Sequencia)
                            .ThenBy(x => x.Nome)
                            .ToList();

            return produtos;
        }
    }
}