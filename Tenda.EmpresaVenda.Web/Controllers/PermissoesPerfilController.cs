using NHibernate;
using System.Linq;
using System.Web.Mvc;
using Tenda.Domain.Security;
using Tenda.Domain.Security.Models;
using Tenda.Domain.Security.Repository;
using Tenda.EmpresaVenda.Web.Models;
using Tenda.EmpresaVenda.Web.Security;

namespace Tenda.EmpresaVenda.Web.Controllers
{
    [BaseAuthorize("SEG04")]
    public class PermissoesPerfilController : BaseController
    {
        private SistemaRepository _sistemaRepository { get; set; }

        public PermissoesPerfilController(ISession session) : base(session)
        {
        }

        [BaseAuthorize("SEG04", "Visualizar")]
        public ActionResult Index()
        {
            SistemaDTO dto = new SistemaDTO();
            dto.Sistema = new Sistema();
            dto.Sistemas = _sistemaRepository.Queryable().OrderBy(x => x.Codigo).ToList();
            return View(dto);
        }
    }
}