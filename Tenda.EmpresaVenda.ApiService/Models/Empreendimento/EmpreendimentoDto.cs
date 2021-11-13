using System;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.ApiService.Models.Util;

namespace Tenda.EmpresaVenda.ApiService.Models.Empreendimento
{
    public class EmpreendimentoDto : EntityDto
    {
        public long IdSuat { get; set; }
        public string Divisao { get; set; }
        public bool DisponivelParaVenda { get; set; }
        public bool DisponivelCatalogo { get; set; }
        public string Informacoes { get; set; }
        public string Regional { get; set; }
        public Regionais RegionalObjeto { get; set; }
        public string CodigoEmpresa { get; set; }
        public string NomeEmpresa { get; set; }
        public string CNPJ { get; set; }
        public string Mancha { get; set; }
        public string RegistroIncorporacao { get; set; }
        public DateTime? DataLancamento { get; set; }
        public DateTime? PrevisaoEntrega { get; set; }
        public DateTime? DataEntrega { get; set; }
        public bool PriorizarRegraComissao { get; set; }
        public string FichaTecnica { get; set; }
        public TipoModalidadeComissao ModalidadeComissao { get; set; }
        public TipoModalidadeProgramaFidelidade ModalidadeProgramaFidelidade { get; set; }
        public string NomeEmpreendimento { get; set; }

        public EmpreendimentoDto FromDomain(Tenda.Domain.EmpresaVenda.Models.Empreendimento model)
        {
            Id = model.Id;
            Nome = model.Nome;
            IdSuat = model.IdSuat;
            Divisao = model.Divisao;
            DisponivelParaVenda = model.DisponivelParaVenda;
            DisponivelCatalogo = model.DisponivelCatalogo;
            Informacoes = model.Informacoes;
            RegionalObjeto = model.RegionalObjeto;
            Regional = model.Regional;
            CodigoEmpresa = model.CodigoEmpresa;
            NomeEmpresa = model.NomeEmpresa;
            CNPJ = model.CNPJ;
            Mancha = model.Mancha;
            RegistroIncorporacao = model.RegistroIncorporacao;
            DataLancamento = model.DataLancamento;
            PrevisaoEntrega = model.PrevisaoEntrega;
            DataEntrega = model.DataEntrega;
            PriorizarRegraComissao = model.PriorizarRegraComissao;
            FichaTecnica = model.FichaTecnica;
            ModalidadeComissao = model.ModalidadeComissao;
            ModalidadeProgramaFidelidade = model.ModalidadeProgramaFidelidade;

            return this;
        }
    }
}
