using Europa.Data.Model;
using System;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class ItemPontuacaoFidelidade : BaseEntity
    {
        public virtual decimal PontuacaoFaixaUmMeio { get; set; }
        public virtual decimal PontuacaoFaixaDois { get; set; }
        public virtual decimal PontuacaoPNE { get; set; }

        //F1.5
        public virtual decimal PontuacaoFaixaUmMeioSeca { get; set; }
        public virtual decimal PontuacaoFaixaUmMeioNormal { get; set; }
        public virtual decimal PontuacaoFaixaUmMeioTurbinada { get; set; }

        //F2.0
        public virtual decimal PontuacaoFaixaDoisSeca { get; set; }
        public virtual decimal PontuacaoFaixaDoisNormal { get; set; }
        public virtual decimal PontuacaoFaixaDoisTurbinada { get; set; }

        //PNE
        public virtual decimal PontuacaoPNESeca { get; set; }
        public virtual decimal PontuacaoPNENormal { get; set; }
        public virtual decimal PontuacaoPNETurbinada { get; set; }

        //Pontuação Padrão
        public virtual decimal FatorUmMeio { get; set; }
        public virtual decimal PontuacaoPadraoUmMeio { get; set; }
        public virtual decimal FatorDois { get; set; }
        public virtual decimal PontuacaoPadraoDois { get; set; }

        //Pontuação Nominal
        //F1.5
        public virtual decimal FatorUmMeioSeca { get; set; }
        public virtual decimal FatorUmMeioNormal { get; set; }
        public virtual decimal FatorUmMeioTurbinada { get; set; }

        //F2.0
        public virtual decimal FatorDoisSeca { get; set; }
        public virtual decimal FatorDoisNormal { get; set; }
        public virtual decimal FatorDoisTurbinada { get; set; }

        //PNE
        public virtual decimal FatorPNESeca { get; set; }
        public virtual decimal FatorPNENormal { get; set; }
        public virtual decimal FatorPNETurbinada { get; set; }

        public virtual long QuantidadeMinima { get; set; }
        public virtual DateTime? InicioVigencia { get; set; }
        public virtual DateTime? TerminoVigencia { get; set; }
        public virtual PontuacaoFidelidade PontuacaoFidelidade { get; set; }
        public virtual Empreendimento Empreendimento { get; set; }
        public virtual SituacaoPontuacaoFidelidade Situacao { get; set; }
        public virtual string Codigo { get; set; }
        public virtual EmpresaVenda EmpresaVenda { get; set; }
        public virtual TipoModalidadeProgramaFidelidade Modalidade { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
