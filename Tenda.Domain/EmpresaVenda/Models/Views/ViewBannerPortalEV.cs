﻿using Europa.Data.Model;
using System;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewBannerPortalEV : BaseEntity
    {
        public virtual string Descricao { get; set; }
        public virtual SituacaoBanner Situacao { get; set; }
        public virtual DateTime? InicioVigencia { get; set; }
        public virtual DateTime? FimVigencia { get; set; }
        public virtual long IdArquivo { get; set; }
        public virtual string NomeArquivo { get; set; }
        public virtual string Regional { get; set; }
        public virtual long? IdRegional { get; set; }
        public virtual string Estado { get; set; }
        public virtual TipoBanner Tipo { get; set; }
        public virtual bool Exibicao { get; set; }
        public virtual bool Diretor { get; set; }
        public virtual bool Visualizado { get; set; }
        public virtual string Link { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}