using Europa.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Portal.Controllers
{
    public class MotivoController : BaseController
    {
        private MotivoRepository _motivoRepository { get; set; }

        public ActionResult RenderMotivosNegativosAnexo()
        {
            var results = _motivoRepository.Listar()
                .Where(x => x.TipoMotivo == Tenda.Domain.EmpresaVenda.Enums.TipoMotivo.NegativaAnexarDocumentoProponente)
                .Where(x => x.Situacao == Tenda.Domain.Core.Enums.Situacao.Ativo)
                .ToList();
            results.Add(new Motivo
            {
                Id = 0,
                Descricao = GlobalMessages.NaoQueroAnexar,
                TipoMotivo = Tenda.Domain.EmpresaVenda.Enums.TipoMotivo.NegativaAnexarDocumentoProponente,
                Situacao = Tenda.Domain.Core.Enums.Situacao.Ativo
            });
            var list = results.OrderBy(x => x.Descricao).Select(x => new SelectListItem
            {
                Text = x.Descricao,
                Value = x.Id.ToString(),
            });

            return PartialView("~/Views/PreProposta/_MotivoDropDownList.cshtml", list);
        }
    }
}