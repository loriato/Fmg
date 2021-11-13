using Europa.Extensions;
using Europa.Resources;
using Europa.Rest;
using Europa.Web;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Services.Models.Indique;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class IndiqueService : BaseService
    {
        private EnderecoClienteRepository _enderecoClienteRepository { get; set; }
        private ProponenteRepository _proponenteRepository { get; set; }
        private HierarquiaHouseRepository _hierarquiaHouseRepository { get; set; }
        private CoordenadorSupervisorRepository _coordenadorSupervisorRepository { get; set; }
        private CoordenadorViabilizadorRepository _coordenadorViabilizadorRepository { get; set; }
        private ClienteRepository _clienteRepository { get; set; }

        private SupervisorViabilizadorRepository _supervisorViabilizadorRepository { get; set; }
        private PontoVendaRepository _pontoVendaRepository { get; set; }

        public PropostaDto BuscarPrePropostaPorCpfCliente(string cpf)
        {
            var proponente = _proponenteRepository.BuscarPorCpfClienteIndique(cpf);

            if (proponente == null) return null;

            var proponentes = _proponenteRepository.ProponentesDaPreProposta(proponente.PreProposta.Id);

            var propostaDto = ModelToPropostaDto(proponente.PreProposta);
            propostaDto.Proponentes = ModelToProponenteDto(proponentes);
            propostaDto.Cliente = propostaDto.Proponentes.SingleOrDefault(x => x.Cliente.Cpf.OnlyNumber() == cpf)?.Cliente;

            return propostaDto;
        }

        public IEnumerable<PropostaDto> BuscarPrePropostasClientesPorCpf(string[] cpfs)
        {
            if (cpfs == null || !cpfs.Any()) return null;

            var result = new List<PropostaDto>();

            var proponentes = _proponenteRepository.BuscarPrePropostasClientesPorCpfIndique(cpfs);

            if (!proponentes.Any()) return null;

            foreach (var group in proponentes.GroupBy(x => x.Cliente.CpfCnpj).Distinct())
            {
                var proponente = group.FirstOrDefault();

                if (group.Any(x => x.PreProposta.SituacaoProposta != SituacaoProposta.Cancelada && x.Id != proponente.Id))
                {
                    proponente = group.FirstOrDefault(x => x.PreProposta.SituacaoProposta != SituacaoProposta.Cancelada);
                }

                var proponentesProposta = _proponenteRepository.ProponentesDaPreProposta(proponente.PreProposta.Id);

                var propostaDto = ModelToPropostaDto(proponente.PreProposta);
                propostaDto.Proponentes = ModelToProponenteDto(proponentesProposta);
                propostaDto.Cliente = propostaDto.Proponentes.SingleOrDefault(x => x.Cliente.Cpf.OnlyNumber() == proponente.Cliente.CpfCnpj)?.Cliente;

                result.Add(propostaDto);
            }

            return result;
        }

        private PropostaDto ModelToPropostaDto(PreProposta preProposta)
        {
            var propostaDto = new PropostaDto
            {
                Codigo = preProposta.Codigo,
                DataElaboracao = preProposta.DataElaboracao,
                DataAtualizacao = preProposta.CriadoEm,
                StatusSicaq = preProposta.StatusSicaq,
                PassoAtual = preProposta.PassoAtualSuat,

                Corretor = new UsuarioDto()
                {
                    Nome = preProposta.Corretor?.Usuario?.Nome,
                    Email = preProposta.Corretor?.Usuario?.Email,
                    Login = preProposta.Corretor?.Usuario?.Login
                },

                Vendedor = new UsuarioDto()
                {
                    Nome = preProposta.Viabilizador?.Nome,
                    Email = preProposta.Viabilizador?.Email,
                    Login = preProposta.Viabilizador?.Login
                },

                Loja = new LojaDto()
                {
                    Nome = preProposta.EmpresaVenda?.Loja?.Nome,
                    IdSap = preProposta.EmpresaVenda?.Loja?.SapId
                }
            };

            propostaDto.Empreendimento = new EmpreendimentoDto()
            {
                IdSap = preProposta.BreveLancamento?.Empreendimento?.Divisao,
                Nome = preProposta.BreveLancamento?.Empreendimento?.Nome
            };

            //Define o Status da Proposta usado no Indique baseado na SituacaoProposta
            propostaDto.StatusProposta = preProposta.SituacaoProposta == SituacaoProposta.Cancelada
                ? StatusProposta.Cancelada
                : StatusProposta.EmElaboracao;

            return propostaDto;
        }

        private ClienteDto ModelToClienteDto(Cliente model)
        {
            var enderecoCliente = _enderecoClienteRepository.FindByCliente(model.Id);

            var nomeSplit = model.NomeCompleto.Split(null, 2);

            var dto = new ClienteDto()
            {
                Nome = nomeSplit.Length > 0 ? nomeSplit[0] : "",
                Sobrenome = nomeSplit.Length > 1 ? nomeSplit[1] : "",
                Cpf = model.CpfCnpj,
                Genero = model.TipoSexo,
                EstadoCivil = model.EstadoCivil,
                DataNascimento = model.DataNascimento,
                IdSap = model.IdSap,
                Nacionalidade = model.Nacionalidade,
                Filiacao = model.Filiacao,
                TipoDocumento = model.TipoDocumento,
                Documento = model.NumeroDocumento,
                EstadoEmissor = model.EstadoEmissor,
                DataEmissao = model.DataEmissao,
                Cargo = model.Cargo,
                Referencia = model.PrimeiraReferencia,
                Referencia2 = model.SegundaReferencia,
                TelefoneReferencia = model.TelefonePrimeiraReferencia,
                TelefoneReferencia2 = model.TelefoneSegundaReferencia,
                Cep = enderecoCliente?.Cep,
                Logradouro = enderecoCliente?.Logradouro,
                Numero = enderecoCliente?.Numero,
                Complemento = enderecoCliente?.Complemento,
                Estado = enderecoCliente?.Estado,
                Bairro = enderecoCliente?.Bairro,
                Cidade = enderecoCliente?.Cidade
            };

            dto.Profissao = new ProfissaoDto
            {
                IdSap = model.Profissao?.IdSap,
                Nome = model.Profissao?.Nome
            };

            var listaContatos = new List<ContatoClienteDto>();

            if (model.TelefoneResidencial.HasValue())
            {
                var contato = new ContatoClienteDto()
                {
                    Tipo = TipoContato.Residencial,
                    Comentario = model.TelefoneResidencial,
                    Principal = true
                };

                listaContatos.Add(contato);
            }

            if (model.TelefoneComercial.HasValue())
            {
                var contato = new ContatoClienteDto()
                {
                    Tipo = TipoContato.Comercial,
                    Comentario = model.TelefoneComercial,
                    Principal = true
                };

                listaContatos.Add(contato);
            }

            if (model.Email.HasValue())
            {
                var contato = new ContatoClienteDto()
                {
                    Tipo = TipoContato.Email,
                    Comentario = model.Email,
                    Principal = true
                };

                listaContatos.Add(contato);
            }

            dto.ContatoCliente = listaContatos;

            return dto;
        }

        private List<ProponenteDto> ModelToProponenteDto(List<Proponente> proponentes)
        {
            var proponentesDto = new List<ProponenteDto>();

            foreach (var proponente in proponentes)
            {
                var proponenteDto = new ProponenteDto
                {
                    Cliente = ModelToClienteDto(proponente.Cliente),
                    Titular = proponente.Titular,
                    Participacao = proponente.Participacao
                };

                proponentesDto.Add(proponenteDto);
            }

            return proponentesDto;
        }

        public BaseResponse EnviarWhatsBotmakerIndicado(long id, string nome, string telefone)
        {

            try
            {
                var cliente = _clienteRepository.FindById(id);

                var exc = new ApiException();

                if (nome.IsEmpty())
                {
                    exc.AddError("NomeIndicado", string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Nome));
                }
                if (telefone.IsEmpty())
                {
                    exc.AddError("TelefoneIndicado", string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Telefone));

                }
                exc.ThrowIfHasError();

                var baseResponseLink = BuscarLinkIndicador(cliente.CpfCnpj);

                var destinatario = new DestinatarioIndicadoDto()
                {
                    LinkIndicacao = baseResponseLink.Data.ToString(),
                    Nome = nome,
                    Telefone = telefone,
                    Amigo = cliente.NomeCompleto
                };
                return EnviarWhatsIndicadoBotmaker(destinatario, "mgm_templateindicado_220921");
            }
            catch (ApiException e)
            {
                return e.GetResponse();
            }

        }
        public BaseResponse EnviarWhatsBotmakerIndicador(long id)
        {

            try
            {
                var cliente = _clienteRepository.FindById(id);
                var baseResponseLink = BuscarLinkIndicador(cliente.CpfCnpj);

                var destinatario = new DestinatarioIndicadorDto()
                {
                    LinkIndicacao = baseResponseLink.Data.ToString(),
                    Nome = cliente.NomeCompleto,
                    Telefone = cliente.TelefoneResidencial
                };
                return EnviarWhatsIndicadorBotmaker(destinatario, "mgm_templateindicador_220921");
            }
            catch (ApiException e)
            {
                return e.GetResponse();
            }

        }

        private BaseResponse BuscarLinkIndicador(string CpfCnpj)
        {
            var indiqueService = new Tenda.EmpresaVenda.Domain.Integration.Indique.IndiqueService();
            var response = indiqueService.BuscarLinkPorCpf(CpfCnpj);
            if (response.Data.IsEmpty())
            {
                var exc = new ApiException();
                exc.AddError("O Cliente não possui link de indicação");
                exc.ThrowIfHasError();

            }
            return response;
        }


        private BaseResponse EnviarWhatsIndicadorBotmaker(DestinatarioIndicadorDto destinatarioDto, string acao)
        {

            var indiqueService = new Tenda.EmpresaVenda.Domain.Integration.Indique.IndiqueService();
            var response = indiqueService.EnviarWhatsIndicadorBotmaker(destinatarioDto, acao);

            return response;

        }
        private BaseResponse EnviarWhatsIndicadoBotmaker(DestinatarioIndicadoDto destinatarioDto, string acao)
        {

            var indiqueService = new Tenda.EmpresaVenda.Domain.Integration.Indique.IndiqueService();
            var response = indiqueService.EnviarWhatsIndicadoBotmaker(destinatarioDto, acao);

            return response;

        }

        public List<CarteiraDto> BuscarCarteiraHouse()
        {
            var hierarquiaHouse = _hierarquiaHouseRepository.Queryable().Where(x => x.Fim == null)
                                                                        .Where(x => x.Situacao == SituacaoHierarquiaHouse.Ativo)
                                                                        .OrderByDescending(x => x.AtualizadoEm)
                                                                        .ToList();
            var carteiraHouseDto = new List<CarteiraDto>();
            foreach (var item in hierarquiaHouse)
            {
                var dto = new CarteiraDto();
                dto.Loja = new LojaDto
                {
                    Nome = item.House.NomeFantasia,
                    IdSap = item.House.Loja?.SapId
                };
                dto.Coordenador = new UsuarioDto
                {
                    Login = item.Coordenador.Login,
                    Nome = item.Coordenador.Nome,
                    Email = item.Coordenador.Email
                };
                dto.Supervisor = new UsuarioDto
                {
                    Login = item.Supervisor.Login,
                    Nome = item.Supervisor.Nome,
                    Email = item.Supervisor.Email
                };
                dto.AgenteVenda = new UsuarioDto
                {
                    Login = item.AgenteVenda.Login,
                    Nome = item.AgenteVenda.Nome,
                    Email = item.AgenteVenda.Email
                };
                carteiraHouseDto.Add(dto);
            }

            return carteiraHouseDto;

        }

        public List<CarteiraDto> BuscarCarteiraEv()
        {

            var pontodeVenda = _pontoVendaRepository.Queryable().Where(x => x.EmpresaVenda.TipoEmpresaVenda == TipoEmpresaVenda.EmpresaVenda)
                .Where(x => x.Situacao == Tenda.Domain.Core.Enums.Situacao.Ativo)
                .Where(x => x.Viabilizador != null)
                .OrderByDescending(x => x.AtualizadoEm)
                .ToList();


            var carteiraEvDto = new List<CarteiraDto>();
            foreach (var item in pontodeVenda)
            {
                var dto = new CarteiraDto();
                dto.Loja = new LojaDto
                {
                    Nome = item.EmpresaVenda.NomeFantasia,
                    IdSap = item.EmpresaVenda.Loja?.SapId
                };
                dto.AgenteVenda = new UsuarioDto
                {
                    Login = item.Viabilizador.Login,
                    Nome = item.Viabilizador.Nome,
                    Email = item.Viabilizador.Email
                };

                var supervisor = _supervisorViabilizadorRepository.Queryable().Where(x => x.Viabilizador.Id == item.Viabilizador.Id).Select(x => x.Supervisor).FirstOrDefault();
                if (!supervisor.IsEmpty())
                {
                    dto.Supervisor = new UsuarioDto
                    {
                        Login = supervisor?.Login,
                        Nome = supervisor?.Nome,
                        Email = supervisor?.Email
                    };

                    var coordenador = _coordenadorSupervisorRepository.Queryable().Where(x => x.Supervisor.Id == supervisor.Id).Select(x => x.Coordenador).FirstOrDefault();
                    dto.Coordenador = coordenador.IsEmpty() ? null : new UsuarioDto
                    {
                        Login = coordenador?.Login,
                        Nome = coordenador?.Nome,
                        Email = coordenador?.Email
                    };
                }
                else
                {
                    var coordenador = _coordenadorViabilizadorRepository.Queryable().Where(x => x.Viabilizador.Id == item.Viabilizador.Id).Select(x => x.Coordenador).FirstOrDefault();
                    dto.Supervisor = supervisor.IsEmpty() ? null : new UsuarioDto
                    {
                        Login = coordenador?.Login,
                        Nome = coordenador?.Nome,
                        Email = coordenador?.Email
                    };

                    dto.Coordenador = coordenador.IsEmpty() ? null : new UsuarioDto
                    {
                        Login = coordenador?.Login,
                        Nome = coordenador?.Nome,
                        Email = coordenador?.Email
                    };
                }

                carteiraEvDto.Add(dto);
            }

            return carteiraEvDto;
        }
    }
}
