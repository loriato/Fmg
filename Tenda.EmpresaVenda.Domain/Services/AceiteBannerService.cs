using Europa.Extensions;
using Europa.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.Core.Services;
using Tenda.EmpresaVenda.Domain.Repository;

namespace Tenda.EmpresaVenda.Domain.Services
{
    public class AceiteBannerService : BaseService
    {
        private AceitesBannersRepository _aceitesBannersRepository { get; set; }
        public byte[] Exportar(DataSourceRequest request, long? IdBanner)
        {
            var results = _aceitesBannersRepository.Listar(request,IdBanner);

            ExcelUtil excel = ExcelUtil.NewInstance(20)
                .NewSheet(DateTime.Now.ToString("yyyyMMddHHmmss"))
                .WithHeader(GetHeader());

            foreach (var model in results.records.ToList())
            {
                excel
                    .CreateCellValue(model.TituloBanner).Width(30)
                    .CreateCellValue(model.DataAceite.ToString()).Width(20)
                    .CreateCellValue(model.NomeCorretor).Width(30)
                    .CreateCellValue(model.EmailCorretor).Width(30);

            }
            excel.Close();
            return excel.DownloadFile();
        }

        private string[] GetHeader()
        {
            IList<string> header = new List<string>
            {
                GlobalMessages.TituloBanner,
                GlobalMessages.DataAceite,
                GlobalMessages.NomeCorretor,
                GlobalMessages.EmailCorretor,

            };
            return header.ToArray();
        }

    }
}
