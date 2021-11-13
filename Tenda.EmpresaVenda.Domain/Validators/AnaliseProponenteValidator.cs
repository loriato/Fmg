using Europa.Extensions;
using Europa.Resources;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Validators
{
    public class AnaliseProponenteValidator : AbstractValidator<Proponente>
    {
        public DocumentoProponenteRepository _documentoProponenteRepository { get; set; }
        public ClienteRepository _clienteRepository { get; set; }
        public TipoDocumentoRepository _tipoDocumentoRepository { get; set; }
        public DocumentoRuleMachinePrePropostaRepository _documentoRuleMachinePrePropostaRepository { get; set; }

        public AnaliseProponenteValidator(DocumentoProponenteRepository documentoProponenteRepository, ClienteRepository clienteRepository, TipoDocumentoRepository tipoDocumentoRepository, DocumentoRuleMachinePrePropostaRepository documentoRuleMachinePrePropostaRepository, SituacaoProposta destino, bool EvLoja)
        {
            _documentoProponenteRepository = documentoProponenteRepository;
            _clienteRepository = clienteRepository;
            _tipoDocumentoRepository = tipoDocumentoRepository;
            _documentoRuleMachinePrePropostaRepository = documentoRuleMachinePrePropostaRepository;

            // Verificação do cliente
            RuleFor(prop => prop.Cliente.CpfCnpj).NotEmpty().OverridePropertyName("Cliente.CpfCnpj")
                .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.CpfCnpj));
            RuleFor(prop => prop).Must(prop => !CheckIfExistsCpfCnpj(prop)).OverridePropertyName("Cliente.CpfCnpj")
                .WithMessage(string.Format(GlobalMessages.MsgErroRegistroExistenteEmpresaVenda, GlobalMessages.Cliente, GlobalMessages.CpfCnpj));
            RuleFor(prop => prop).Must(prop => CheckIfIsValidCpfCnpj(prop)).OverridePropertyName("Cliente.CpfCnpj")
                .WithMessage(string.Format(GlobalMessages.DadoInvalido, GlobalMessages.CpfCnpj));
            RuleFor(prop => prop.Cliente.NomeCompleto).NotEmpty().OverridePropertyName("Cliente.NomeCompleto")
                .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Nome));
            RuleFor(prop => prop.Cliente.TipoSexo).NotEmpty().OverridePropertyName("Cliente.TipoSexo")
                .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Sexo));
            RuleFor(prop => prop).Must(prop => CheckContacts(prop)).OverridePropertyName("Cliente.TelefoneResidencial")
                .WithMessage(GlobalMessages.MsgErroContato);
            RuleFor(prop => prop.Cliente.Email).NotEmpty().OverridePropertyName("Cliente.Email")
                .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Email));
            RuleFor(prop => prop.Cliente.Email).EmailAddress().OverridePropertyName("Cliente.Email")
                .WithMessage(GlobalMessages.EmailInvalido);
            RuleFor(prop => prop).Must(prop => ValidarDataFutura(prop.Cliente.DataNascimento)).When(prop => !prop.Cliente.DataNascimento.IsEmpty()).OverridePropertyName("Cliente.DataNascimento")
                .WithMessage(string.Format(GlobalMessages.MsgCampoDataFuturo, GlobalMessages.DataNascimento));
            RuleFor(prop => prop).Must(prop => ValidarDataFutura(prop.Cliente.DataEmissao)).When(prop => !prop.Cliente.DataEmissao.IsEmpty()).OverridePropertyName("Cliente.DataEmissao")
                .WithMessage(string.Format(GlobalMessages.MsgCampoDataFuturo, GlobalMessages.DataEmissao));
            RuleFor(prop => prop).Must(prop => ValidarDataFutura(prop.Cliente.DataAdmissao)).When(prop => !prop.Cliente.DataAdmissao.IsEmpty()).OverridePropertyName("Cliente.DataAdmissao")
                .WithMessage(string.Format(GlobalMessages.MsgCampoDataFuturo, GlobalMessages.DataAdmissao));
            RuleFor(prop => prop).Must(prop => ValidarDataFutura(prop.Cliente.DataUltimaParcelaFinanciamentoPaga)).When(prop => !prop.Cliente.DataUltimaParcelaFinanciamentoPaga.IsEmpty()).OverridePropertyName("Cliente.DataUltimaParcelaFinanciamentoPaga")
                .WithMessage(string.Format(GlobalMessages.MsgCampoDataFuturo, GlobalMessages.DataUltimaParcelaFinanciamentoPaga));
            RuleFor(prop => prop).Must(prop => ValidarDataFutura(prop.Cliente.DataUltimaPrestacaoPaga)).When(prop => !prop.Cliente.DataUltimaPrestacaoPaga.IsEmpty()).OverridePropertyName("Cliente.DataUltimaPrestacaoPaga")
                .WithMessage(string.Format(GlobalMessages.MsgCampoDataFuturo, GlobalMessages.DataUltimaPrestacaoPaga));

            // Verificação da renda
            RuleFor(prop => prop.Cliente.RendaMensal).NotNull().OverridePropertyName(prop => prop.Cliente.RendaMensal)
                .WithMessage(prop => string.Format(GlobalMessages.MsgCampoObrigatorioCliente, GlobalMessages.RendaMensal, prop.Cliente.NomeCompleto));
            RuleFor(prop => prop.Cliente.MesesFGTS).NotNull().OverridePropertyName(prop => prop.Cliente.MesesFGTS)
                .WithMessage(prop => string.Format(GlobalMessages.MsgCampoObrigatorioCliente, GlobalMessages.MesesFgts, prop.Cliente.NomeCompleto));
            RuleFor(prop => prop.Cliente.FGTS).NotNull().OverridePropertyName(prop => prop.Cliente.FGTS)
                .WithMessage(prop => string.Format(GlobalMessages.MsgCampoObrigatorioCliente, GlobalMessages.FGTS, prop.Cliente.NomeCompleto));
            if (!EvLoja)
            {
                // Verificação do Veículo
                RuleFor(prop => prop.Cliente.PossuiVeiculo).NotEmpty().OverridePropertyName("Cliente.PossuiVeiculo")
                    .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.PossuiVeiculo));
                RuleFor(prop => prop.Cliente.ValorVeiculo).NotEmpty().When(prop => prop.Cliente.PossuiVeiculo.HasValue && prop.Cliente.PossuiVeiculo.Value)
                    .OverridePropertyName("Cliente.ValorVeiculo").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.ValorVeiculo));
                RuleFor(prop => prop.Cliente.VeiculoFinanciado).NotEmpty().When(prop => prop.Cliente.PossuiVeiculo.HasValue && prop.Cliente.PossuiVeiculo.Value)
                    .OverridePropertyName("Cliente.VeiculoFinanciado").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.VeiculoFinanciado));
                RuleFor(prop => prop.Cliente.ValorUltimaParcelaFinanciamentoVeiculo).NotEmpty().When(prop => prop.Cliente.VeiculoFinanciado.HasValue && prop.Cliente.VeiculoFinanciado.Value)
                    .OverridePropertyName("Cliente.ValorUltimaParcelaFinanciamentoVeiculo").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.ValorUltimaParcelaFinanciamentoVeiculo));
                RuleFor(prop => prop.Cliente.DataUltimaParcelaFinanciamentoPaga).NotEmpty().When(prop => prop.Cliente.VeiculoFinanciado.HasValue && prop.Cliente.VeiculoFinanciado.Value)
                    .OverridePropertyName("Cliente.DataUltimaParcelaFinanciamentoPaga").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.DataUltimaParcelaFinanciamentoPaga));
                // Verificação da Conta Bancária
                RuleFor(prop => prop.Cliente.PossuiContaBanco).NotEmpty().OverridePropertyName("Cliente.PossuiContaBanco")
                    .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.PossuiContaBanco));
                RuleFor(prop => prop.Cliente.Banco).NotEmpty().When(prop => prop.Cliente.PossuiContaBanco.HasValue && prop.Cliente.PossuiContaBanco.Value)
                    .OverridePropertyName("Cliente.Banco").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.Banco));
                RuleFor(prop => prop.Cliente.LimiteChequeEspecial).NotNull().When(prop => prop.Cliente.PossuiContaBanco.HasValue && prop.Cliente.PossuiContaBanco.Value)
                    .OverridePropertyName("Cliente.LimiteChequeEspecial").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.LimiteChequeEspecial));
                // Verificação do Comprometimento Financeiro
                RuleFor(prop => prop.Cliente.PossuiComprometimentoFinanceiro).NotEmpty().OverridePropertyName("Cliente.PossuiComprometimentoFinanceiro")
                    .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.PossuiComprometimentoFinanceiro));
                RuleFor(prop => prop.Cliente.ValorComprometimentoFinanceiro).NotEmpty().When(prop => prop.Cliente.PossuiComprometimentoFinanceiro.HasValue && prop.Cliente.PossuiComprometimentoFinanceiro.Value)
                    .OverridePropertyName("Cliente.ValorComprometimentoFinanceiro").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.ValorComprometimentoFinanceiro));
                RuleFor(prop => prop.Cliente.PrestacoesVencer).NotEmpty().When(prop => prop.Cliente.PossuiComprometimentoFinanceiro.HasValue && prop.Cliente.PossuiComprometimentoFinanceiro.Value)
                    .OverridePropertyName("Cliente.PrestacoesVencer").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.PrestacoesVencer));
                RuleFor(prop => prop.Cliente.DataUltimaPrestacaoPaga).NotEmpty().When(prop => prop.Cliente.PossuiComprometimentoFinanceiro.HasValue && prop.Cliente.PossuiComprometimentoFinanceiro.Value)
                    .OverridePropertyName("Cliente.DataUltimaPrestacaoPaga").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.DataUltimaPrestacaoPaga));
                // Verificação do Cartão de Crédito
                RuleFor(prop => prop.Cliente.PossuiCartaoCredito).NotEmpty().OverridePropertyName("Cliente.PossuiCartaoCredito")
                    .WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.PossuiCartaoCredito));
                RuleFor(prop => prop.Cliente.BandeiraCartaoCredito).NotEmpty().When(prop => prop.Cliente.PossuiCartaoCredito.HasValue && prop.Cliente.PossuiCartaoCredito.Value)
                    .OverridePropertyName("Cliente.BandeiraCartaoCredito").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.BandeiraCartaoCredito));
                RuleFor(prop => prop.Cliente.LimiteCartaoCredito).NotEmpty().When(prop => prop.Cliente.PossuiCartaoCredito.HasValue && prop.Cliente.PossuiCartaoCredito.Value)
                    .OverridePropertyName("Cliente.LimiteCartaoCredito").WithMessage(string.Format(GlobalMessages.CampoObrigatorio, GlobalMessages.LimiteCartaoCredito));
            }

            // Verificação dos Documentos
            RuleFor(prop => prop).Must(prop => VerificaDocumentos(prop, destino))
                .WithMessage(prop => string.Format(GlobalMessages.MsgPendenciaDocumentacaoProponente, prop.Cliente.NomeCompleto));

            RuleFor(prop => prop).Must(prop => TodosDocumentosAprovados(prop, destino))
               .WithMessage(prop => string.Format("Todos documentos do proponente {0} devem ser aprovados", prop.Cliente.NomeCompleto));
        }
        public bool ValidarEmpresaVenda(Proponente proponente)
        {
            if (proponente.Cliente.EmpresaVenda.TipoEmpresaVenda == TipoEmpresaVenda.EmpresaVenda)
            {
                return true;
            }
            return false;
        }

        public bool ValidarDataFutura(DateTime? data)
        {
            if (data.IsEmpty() || data.Value.Date > DateTime.Today)
            {
                return false;
            }
            return true;
        }

        public bool CheckIfExistsCpfCnpj(Proponente proponente)
        {
            if (proponente.Cliente.CpfCnpj.IsEmpty())
            {
                return false;
            }
            return _clienteRepository.CheckIfExistsCpfCnpj(proponente.Cliente);
        }

        public bool CheckIfIsValidCpfCnpj(Proponente proponente)
        {
            if (proponente.Cliente.CpfCnpj.IsEmpty())
            {
                return false;
            }
            return proponente.Cliente.CpfCnpj.IsValidCPF() || proponente.Cliente.CpfCnpj.IsValidCNPJ();
        }

        public bool CheckContacts(Proponente proponente)
        {
            if (proponente.Cliente.TelefoneResidencial.IsEmpty() && proponente.Cliente.TelefoneComercial.IsEmpty())
            {
                return false;
            }
            return true;
        }

        public bool VerificaDocumentos(Proponente proponente, SituacaoProposta destino)
        {
            if (proponente.Cliente.IsEmpty())
            {
                return false;
            }

            var permitidos = new List<SituacaoAprovacaoDocumento>() {
                SituacaoAprovacaoDocumento.Aprovado,
                SituacaoAprovacaoDocumento.Anexado,
                SituacaoAprovacaoDocumento.Informado
            };

            // Verificar se todos os documentos estão nas situações corretas para avançõ
            var temNaoPermitido = _documentoProponenteRepository.Queryable()
                .Where(x => x.PreProposta.Id == proponente.PreProposta.Id)
                .Where(x => x.Proponente.Cliente.Id == proponente.Cliente.Id)
                .Where(x => !permitidos.Contains(x.Situacao))
                .Any();

            // Verificar se todos os documentos estão preenchidos
            SituacaoProposta situacaoProposta = proponente.PreProposta.SituacaoProposta.Value;

            var todosQuePrecisamEstarPreenchidos = new List<string>();

            var filtroObrigatoriedade = _documentoRuleMachinePrePropostaRepository.Queryable()
            .Where(x => x.RuleMachinePreProposta.Origem == situacaoProposta)
            .Where(x => x.RuleMachinePreProposta.Destino == destino)
            .Where(x => x.TipoDocumento.Situacao == Situacao.Ativo);

            if (proponente.PreProposta.EmpresaVenda.TipoEmpresaVenda == TipoEmpresaVenda.EmpresaVenda)
            {
                filtroObrigatoriedade = filtroObrigatoriedade.Where(x => x.ObrigatorioPortal);
            }
            if (proponente.PreProposta.EmpresaVenda.TipoEmpresaVenda == TipoEmpresaVenda.Loja)
            {
                filtroObrigatoriedade = filtroObrigatoriedade.Where(x => x.ObrigatorioHouse);
            }
            todosQuePrecisamEstarPreenchidos = filtroObrigatoriedade
                                    .Select(x => x.TipoDocumento.Nome)
                                    .ToList();
            if (todosQuePrecisamEstarPreenchidos.IsEmpty())
            {
                return true;
            }
            var contagemDocumentos = _documentoProponenteRepository.Queryable()
                .Where(x => x.PreProposta.Id == proponente.PreProposta.Id)
                .Where(x => x.Proponente.Cliente.Id == proponente.Cliente.Id)
                .Where(x => todosQuePrecisamEstarPreenchidos.Contains(x.TipoDocumento.Nome)).ToList();

            return !temNaoPermitido && contagemDocumentos.Count() >= todosQuePrecisamEstarPreenchidos.Count();
        }

        public bool TodosDocumentosAprovados(Proponente proponente, SituacaoProposta destino)
        {
            var permitidos = new List<SituacaoAprovacaoDocumento>() {
                SituacaoAprovacaoDocumento.Aprovado,
                SituacaoAprovacaoDocumento.Anexado,
                SituacaoAprovacaoDocumento.Informado
            };

            SituacaoProposta situacaoProposta = proponente.PreProposta.SituacaoProposta.Value;

            var filtroObrigatoriedade = _documentoRuleMachinePrePropostaRepository.Queryable()
                                        .Where(x => x.RuleMachinePreProposta.Origem == situacaoProposta)
                                        .Where(x => x.RuleMachinePreProposta.Destino == destino)
                                        .Where(x => x.TipoDocumento.Situacao == Situacao.Ativo);

            if (proponente.PreProposta.EmpresaVenda.TipoEmpresaVenda == TipoEmpresaVenda.EmpresaVenda)
            {
                filtroObrigatoriedade = filtroObrigatoriedade.Where(x => x.ObrigatorioPortal);
            }
            if (proponente.PreProposta.EmpresaVenda.TipoEmpresaVenda == TipoEmpresaVenda.Loja)
            {
                filtroObrigatoriedade = filtroObrigatoriedade.Where(x => x.ObrigatorioHouse);
            }

            var todosQuePrecisamEstarPreenchidos = filtroObrigatoriedade
                                    .Select(x => x.TipoDocumento.Nome)
                                    .ToList();

            if (todosQuePrecisamEstarPreenchidos.IsEmpty())
            {
                return true;
            }

            if (destino != SituacaoProposta.AguardandoIntegracao)
            {
                return true;
            }

            var contagemDocumentos = _documentoProponenteRepository.Queryable()
                .Where(x => x.PreProposta.Id == proponente.PreProposta.Id)
                .Where(x => x.Proponente.Cliente.Id == proponente.Cliente.Id)
                .Where(x => todosQuePrecisamEstarPreenchidos.Contains(x.TipoDocumento.Nome))
                .Where(x => x.Situacao == SituacaoAprovacaoDocumento.Aprovado).ToList();

            return contagemDocumentos.Count() >= todosQuePrecisamEstarPreenchidos.Count();
        }
    }
}
