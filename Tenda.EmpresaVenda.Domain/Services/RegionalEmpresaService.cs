using Europa.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.Core.Services;
using Tenda.Domain.EmpresaVenda.Models;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class RegionalEmpresaService : BaseService
    {
        public RegionalEmpresaRepository _regionalEmpresaRepository { get; set; }
        public RegionaisRepository _regionaisRepository { get; set; }
        public void Salvar(Tenda.Domain.EmpresaVenda.Models.EmpresaVenda ev, List<long> idsRegionais)
        {
            if (idsRegionais == null)
            {
                var regionaisParaExcluir = _regionalEmpresaRepository.Queryable()
                    .Where(w => w.EmpresaVenda == ev);
                //Remover a regional da empresa
                foreach (var regionalEmpresa in regionaisParaExcluir)
                    _regionalEmpresaRepository.Delete(regionalEmpresa);
                return;
            }
            else
            {
                var regionaisParaExcluir = _regionalEmpresaRepository.Queryable()
                    .Where(w => !idsRegionais.Contains(w.Regional.Id) && w.EmpresaVenda == ev);

                //Remover a regional da empresa
                foreach (var regionalEmpresa in regionaisParaExcluir)
                    _regionalEmpresaRepository.Delete(regionalEmpresa);

                //incluir novos
                foreach (var regional in idsRegionais)
                {
                    var regionalEmpresa = _regionalEmpresaRepository.ListarRegionalComEmpresa(ev.Id, regional);
                    if (regionalEmpresa.HasValue())
                        continue;
                    regionalEmpresa = new RegionalEmpresa();
                    regionalEmpresa.EmpresaVenda = ev;
                    regionalEmpresa.Regional = _regionaisRepository.FindById(regional);
                    _regionalEmpresaRepository.Save(regionalEmpresa);
                }
            }
        }
    }
}
