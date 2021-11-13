using Europa.Commons;
using Europa.Extensions;
using Europa.Resources;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.Core.Services;
using Tenda.Domain.Security.Models;
using Tenda.Domain.Security.Repository;
using Tenda.EmpresaVenda.Domain.Repository;
using Tenda.EmpresaVenda.Domain.Validators;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class PerfilSistemaGrupoActiveDiretoryService : BaseService
    {
        private PerfilSistemaGrupoActiveDiretoryRepository _perfilSistemaGrupoActiveDiretoryRepository { get; set; }
        private ViewPerfilSistemaGrupoActiveDirectoryRepository _viewPerfilSistemaGrupoActiveDirectoryRepository { get; set; }

        public void Salvar(PerfilSistemaGrupoActiveDirectory perfilSistemaGrupoAD, BusinessRuleException bre)
        {
            var psgad = new PerfilSistemaGrupoAdValidator(_perfilSistemaGrupoActiveDiretoryRepository).Validate(perfilSistemaGrupoAD);
            bre.WithFluentValidation(psgad);

            if (psgad.IsValid)
            {
                _perfilSistemaGrupoActiveDiretoryRepository.Save(perfilSistemaGrupoAD);
            }
        }

        public byte[] Exportar(DataSourceRequest request, long idSistema)
        {
            ExcelUtil excel = ExcelUtil.NewInstance(20)
                .NewSheet(DateTime.Now.ToString("yyyyMMddHHmmss"))
                .WithHeader(GetHeader());

            var list = _viewPerfilSistemaGrupoActiveDirectoryRepository.Listar(request, idSistema)
                        .records.ToList();

            foreach (var model in list)
            {
                excel.CreateCellValue(model.NomePerfil).Width(30)
                    .CreateCellValue(model.GrupoActiveDirectory).Width(30);
            }
            excel.Close();
            return excel.DownloadFile();
        }

        public string[] GetHeader()
        {
            IList<string> header = new List<string>
            {
                GlobalMessages.Perfil,
                GlobalMessages.GrupoAd
            };
            return header.ToArray();
        }
    }
}
