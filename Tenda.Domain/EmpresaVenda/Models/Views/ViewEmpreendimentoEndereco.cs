using Europa.Data.Model;
using System;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewEmpreendimentoEndereco : BaseEntity
    {
        public virtual string Nome { get; set; }
        public virtual string Cidade { get; set; }
        public virtual string Divisao { get; set; }
        public virtual string Estado { get; set; }
        public virtual bool DisponibilizarCatalogo { get; set; }
        public virtual bool DisponivelVenda { get; set; }
        public virtual string Regional { get; set; }
        public virtual long IdRegional { get; set; }
        public virtual TipoModalidadeComissao ModalidadeComissao { get; set; }
        public virtual TipoModalidadeProgramaFidelidade ModalidadeProgramaFidelidade { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
