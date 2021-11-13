using Europa.Data;
using Europa.Extensions;
using Europa.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.Domain.Security.Repository;
using Tenda.EmpresaVenda.Domain.Services.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewPrePropostaAguardandoAnaliseRepository : NHibernateStatelessRepository<ViewPrePropostaAguardandoAnalise>
    {
        ParametroRepository _parametroRepository { get; set; }
        PerfilRepository _perfilRepository { get; set; }
        public DataSourceResponse<ViewPrePropostaAguardandoAnalise> Listar(DataSourceRequest request, FiltroPrePropostaAguardandoAnaliseDTO filtro)
        {
            var query = Queryable();

            if (filtro.SituacoesParaVisualizacao.HasValue())
            {
                query = query.Where(x => filtro.SituacoesParaVisualizacao.Contains(x.StatusPreProposta));
            }
            query = query.Where(reg => filtro.IdsEvs.Contains(reg.IdEmpresaVenda) || filtro.IdPrePropostasTransferidas.Contains(reg.IdPreProposta));

            if (!filtro.Regional.IsEmpty() && !filtro.Regional.Contains(0))
            {
                query = query.Where(x => filtro.Regional.Contains(x.IdRegional));
            }
            if (!filtro.CodigoPreProposta.IsEmpty())
            {
                filtro.CodigoPreProposta = filtro.CodigoPreProposta.ToUpper();
                query = query.Where(x => x.CodigoPreProposta.StartsWith(filtro.CodigoPreProposta));
            }
            if (!filtro.SituacoesPreProposta.IsEmpty())
            {
                query = query.Where(x => filtro.SituacoesPreProposta.Contains(x.StatusPreProposta));
            }
            if (!filtro.BreveLancamento.IsEmpty())
            {
                query = query.Where(x => x.BreveLancamento.ToLower().Contains(filtro.BreveLancamento.ToLower()));
            }
            if (!filtro.EmpresaVendas.IsEmpty())
            {
                query = query.Where(x => x.EmpresaVenda.ToLower().Contains(filtro.EmpresaVendas.ToLower()));
            }
            if (filtro.DataElaboracaoDe.HasValue)
            {
                query = query.Where(x => x.DataElaboracao.Value.Date >= filtro.DataElaboracaoDe.Value.Date);
            }
            if (filtro.DataElaboracaoAte.HasValue)
            {
                query = query.Where(x => x.DataElaboracao.Value.Date <= filtro.DataElaboracaoAte.Value.Date);
            }
            if (!filtro.Cpf.IsEmpty())
            {
                filtro.Cpf = filtro.Cpf.OnlyNumber();
                query = query.Where(x => x.CpfProponenteUm.Equals(filtro.Cpf) || x.CpfProponenteDois.Equals(filtro.Cpf));
            }
            if (!filtro.Cliente.IsEmpty())
            {
                query = query.Where(x => x.ProponenteUm.ToLower().Contains(filtro.Cliente.ToLower()) || x.ProponenteDois.ToLower().Contains(filtro.Cliente.ToLower()));
                
            }
            if (filtro.DataUltimoEnvioDe.HasValue)
            {
                query = query.Where(x => x.DataHoraUltimoEnvio.Value.Date >= filtro.DataUltimoEnvioDe.Value.Date);
            }
            if (filtro.DataUltimoEnvioAte.HasValue)
            {
                query = query.Where(x => x.DataHoraUltimoEnvio.Value.Date <= filtro.DataUltimoEnvioAte.Value.Date);
            }
            if (!filtro.Viabilizador.IsEmpty())
            {
                query = query.Where(x => x.AgenteViabilizador.ToLower().Contains(filtro.Viabilizador.ToLower()));
            }
            if (filtro.IdBreveLancamento.HasValue())
            {
                query = query.Where(reg => reg.IdBreveLancamento == filtro.IdBreveLancamento);
            }
            if (filtro.IdEmpresaVenda.HasValue())
            {
                query = query.Where(reg => reg.IdEmpresaVenda == filtro.IdEmpresaVenda);
            }

            if (filtro.IdViabilizador.HasValue())
            {
                query = query.Where(reg => reg.IdViabilizador == filtro.IdViabilizador);
            }

            if (filtro.AvalistaPendente)
            {
                query = query
                    .Where(reg=>reg.IdAvalista > 0)
                    .Where(reg => reg.SituacaoDocumento == SituacaoAprovacaoDocumento.Enviado);
            }

            if (filtro.IdPrePropostasRemovidas.HasValue())
            {
                query = query.Where(x => !filtro.IdPrePropostasRemovidas.Contains(x.IdPreProposta));
            }

            if (filtro.UF.HasValue() && !filtro.UF.Contains(""))
            {
                query = query.Where(reg => filtro.UF.Contains(reg.UF));
            }

            return query.ToDataRequest(request);
        }

        public DataSourceResponse<ViewPrePropostaAguardandoAnalise> ListarPrepropostaAvalista(DataSourceRequest request, FiltroPrePropostaAguardandoAnaliseDTO filtro, long? idSistema)
        {
            var idPerfil = _parametroRepository.BuscarParametros("emvs_id_perfil_avalista", idSistema).FirstOrDefault();
            var perfil = _perfilRepository.FindById(Convert.ToInt64(idPerfil.Valor));
            var query = Queryable().Where(reg => reg.IdAvalista > 0).Where(reg => reg.SituacaoDocumento != 0);

            //query = query.Where(x => filtro.Regionais.Contains(x.Regional));

            if (!filtro.Perfis.Contains(perfil))
            {
                query = query.Where(x => x.Id < 0);
            }
            if (filtro.AvalistaPendente)
            {
                query = query.Where(reg => reg.SituacaoDocumento == SituacaoAprovacaoDocumento.Enviado);
            }
            if (!filtro.CodigoPreProposta.IsEmpty())
            {
                filtro.CodigoPreProposta = filtro.CodigoPreProposta.ToUpper();
                query = query.Where(x => x.CodigoPreProposta.StartsWith(filtro.CodigoPreProposta));
            }
            if (!filtro.SituacoesPreProposta.IsEmpty())
            {
                query = query.Where(x => filtro.SituacoesPreProposta.Contains(x.StatusPreProposta));
            }
            if (!filtro.BreveLancamento.IsEmpty())
            {
                query = query.Where(x => x.BreveLancamento.ToLower().Contains(filtro.BreveLancamento.ToLower()));
            }
            if (!filtro.EmpresaVendas.IsEmpty())
            {
                query = query.Where(x => x.EmpresaVenda.ToLower().Contains(filtro.EmpresaVendas.ToLower()));
            }
            if (filtro.DataElaboracaoDe.HasValue)
            {
                query = query.Where(x => x.DataElaboracao.Value.Date >= filtro.DataElaboracaoDe.Value.Date);
            }
            if (filtro.DataElaboracaoAte.HasValue)
            {
                query = query.Where(x => x.DataElaboracao.Value.Date <= filtro.DataElaboracaoAte.Value.Date);
            }
            if (!filtro.Cpf.IsEmpty())
            {
                filtro.Cpf = filtro.Cpf.OnlyNumber();
                query = query.Where(x => x.CpfProponenteUm.Equals(filtro.Cpf) || x.CpfProponenteDois.Equals(filtro.Cpf));
            }
            if (!filtro.Cliente.IsEmpty())
            {
                query = query.Where(x => x.ProponenteUm.ToLower().Contains(filtro.Cliente.ToLower()) || x.ProponenteDois.ToLower().Contains(filtro.Cliente.ToLower()));

            }
            if (filtro.DataUltimoEnvioDe.HasValue)
            {
                query = query.Where(x => x.DataHoraUltimoEnvio.Value.Date >= filtro.DataUltimoEnvioDe.Value.Date);
            }
            if (filtro.DataUltimoEnvioAte.HasValue)
            {
                query = query.Where(x => x.DataHoraUltimoEnvio.Value.Date <= filtro.DataUltimoEnvioAte.Value.Date);
            }
            if (!filtro.Viabilizador.IsEmpty())
            {
                query = query.Where(x => x.AgenteViabilizador.ToLower().Contains(filtro.Viabilizador.ToLower()));
            }
            if (filtro.IdBreveLancamento.HasValue())
            {
                query = query.Where(reg => reg.IdBreveLancamento == filtro.IdBreveLancamento);
            }
            if (filtro.IdEmpresaVenda.HasValue())
            {
                query = query.Where(reg => reg.IdEmpresaVenda == filtro.IdEmpresaVenda);
            }

            if (filtro.IdViabilizador.HasValue())
            {
                query = query.Where(reg => reg.IdViabilizador == filtro.IdViabilizador);
            }

            //if (filtro.Regionais.HasValue())
            //{
            //    query = query.Where(reg => filtro.Regionais.Contains(reg.Regional));
            //}

            if (filtro.SituacoesAvalista.HasValue())
            {
                query = query.Where(reg => filtro.SituacoesAvalista.Contains(reg.SituacaoAvalista.Value));
            }
            if (!filtro.Regional.IsEmpty() && !filtro.Regional.Contains(0))
            {
                query = query.Where(reg => filtro.Regional.Contains(reg.IdRegional));
            }
            if (filtro.UF.HasValue() && !filtro.UF.Contains(""))
            {
                query = query.Where(reg => filtro.UF.Contains(reg.UF));
            }


            return query.ToDataRequest(request);
        }

        public byte[] Exportar(DataSourceRequest request, FiltroPrePropostaAguardandoAnaliseDTO filtro)
        {
            var results = Listar(request, filtro);

            ExcelUtil excel = ExcelUtil.NewInstance(25)
                .NewSheet(DateTime.Now.ToString("yyyyMMddHHmmss"))
                .WithHeader(GetHeader());

            foreach (var model in results.records.ToList())
            {
                excel
                    .CreateCellValue(model.Regional).Width(15)
                    .CreateCellValue(model.UF).Width(15)
                    .CreateCellValue(model.CodigoPreProposta).Width(40)
                    .CreateCellValue(model.SituacaoPrePropostaSuatEvs).Width(23)
                    .CreateDateTimeCell(model.DataHoraUltimoEnvio.HasValue ? model.DataHoraUltimoEnvio.Value.ToDateTime().FromDateTime() : Convert.ToDateTime(null), DateTimeExtensions.PatternDateTimeSeconds).Width(30)
                    .CreateCellValue(model.ProponenteUm).Width(40)
                    .CreateCellValue(model.CpfProponenteUm.ToCPFFormat()).Width(30)
                    .CreateCellValue(model.ProponenteDois).Width(40)
                    .CreateCellValue(model.CpfProponenteDois.ToCPFFormat()).Width(30)
                    .CreateCellValue(model.BreveLancamento).Width(40)
                    .CreateCellValue(model.EmpresaVenda).Width(40)
                    .CreateCellValue(model.NomeElaborador).Width(40)
                    .CreateCellValue(model.Corretor).Width(40)
                    .CreateCellValue(model.AgenteViabilizador).Width(40)
                    .CreateDateTimeCell(model.DataElaboracao.HasValue ? model.DataElaboracao.Value.ToDateTime().FromDateTime() : Convert.ToDateTime(null), DateTimeExtensions.PatternDateTimeSeconds).Width(30)
                    .CreateDateTimeCell(model.DataInicioPrimeiraAnalise.HasValue ? model.DataInicioPrimeiraAnalise.Value.ToDate().FromDate() : Convert.ToDateTime(null)).Width(18)
                    .CreateCellValue(model.HoraInicioPrimeiraAnalise.HasValue ? model.HoraInicioPrimeiraAnalise.Value.ToTime() : "").Width(18)
                    .CreateCellValue(model.HoraFimPrimeiraAnalise.HasValue ? model.HoraFimPrimeiraAnalise.Value.ToTime() : "").Width(18)
                    .CreateCellValue(model.NomeUsuarioPrimeiraAnalise).Width(18)
                    .CreateCellValue(model.ContadorPrimeiraAnalise.MinutesToDuration()).Width(18)
                    .CreateDateTimeCell(model.DataInicioUltimaAnalise.HasValue ? model.DataInicioUltimaAnalise.Value.ToDate().FromDate() : Convert.ToDateTime(null)).Width(18)
                    .CreateCellValue(model.HoraInicioUltimaAnalise.HasValue ? model.HoraInicioUltimaAnalise.Value.ToTime() : "").Width(18)
                    .CreateCellValue(model.HoraFimUltimaAnalise.HasValue ? model.HoraFimUltimaAnalise.Value.ToTime() : "").Width(18)
                    .CreateCellValue(model.NomeUsuarioUltimaAnalise).Width(18)
                    .CreateCellValue(model.Contador.MinutesToDuration()).Width(10)
                    .CreateCellValue(model.ContadorTotal.MinutesToDuration()).Width(15)
                    .CreateCellValue(model.QuantidadeAnalise).Width(30)
                    .CreateCellValue(model.PropostasAnteriores).Width(30)
                    .CreateCellValue(model.NumeroPropostasAnteriores).Width(30)
                    .CreateCellValue(model.MotivoPendencia).Width(45)
                    .CreateCellValue(model.StatusSicaq.AsString()).Width(40)
                    .CreateDateTimeCell(model.DataSicaq.HasValue ? model.DataSicaq.Value.ToDateTime().FromDateTime() : Convert.ToDateTime(null), DateTimeExtensions.PatternDateTimeSeconds).Width(30)
                    .CreateCellValue(model.NomeAnalistaSicaq).Width(40)
                    .CreateMoneyCell(model.ParcelaAprovada).Width(45)
                    .CreateCellValue(model.Observacao).Width(40);
            }
            excel.Close();
            return excel.DownloadFile();
        }

        private string[] GetHeader()
        {
            IList<string> header = new List<string>
            {
                GlobalMessages.Regional,
                GlobalMessages.UF,
                GlobalMessages.Codigo,
                GlobalMessages.SituacaoPreProposta,
                GlobalMessages.DataHoraUltimoEnvio,
                GlobalMessages.ProponenteUm,
                GlobalMessages.CpfProponenteUm,
                GlobalMessages.ProponenteDois,
                GlobalMessages.CpfProponenteDois,
                GlobalMessages.Produto,
                GlobalMessages.EmpresaVenda,
                GlobalMessages.Elaborador,
                GlobalMessages.Corretor,
                GlobalMessages.Viabilizador,
                GlobalMessages.DataDaElaboracao,
                GlobalMessages.DataDaPrimeiraAnalise,
                GlobalMessages.HoraDaPrimeiraAnalise,
                GlobalMessages.FimDaPrimeiraAnalise,
                GlobalMessages.AnalistaPrimeiraAnalise,
                GlobalMessages.ContadorPrimeiraAnalise,
                GlobalMessages.DataDaUltimaAnalise,
                GlobalMessages.HoraDaUltimaAnalise,
                GlobalMessages.FimDaUltimaAnalise,
                GlobalMessages.AnalistaUltimaAnalise,
                GlobalMessages.ContadorUltimaAnalise,
                GlobalMessages.ContadorTotal,
                GlobalMessages.QuantidadeAnalise,
                GlobalMessages.HouvePropostasAnteriores,
                GlobalMessages.NumeroPropostasAnteriores,
                GlobalMessages.MotivoPendencia,
                GlobalMessages.StatusSicaq,
                GlobalMessages.DataSicaq,
                GlobalMessages.AnalistaSicaq,
                GlobalMessages.ParcelaAprovadaDoSICAQ,
                GlobalMessages.Observacao
            };
            return header.ToArray();
        }
        public byte[] ExportarAvalista(DataSourceRequest request, FiltroPrePropostaAguardandoAnaliseDTO filtro)
        {
            var results = ListarPrepropostaAvalista(request, filtro, null);

            ExcelUtil excel = ExcelUtil.NewInstance(25)
                .NewSheet(DateTime.Now.ToString("yyyyMMddHHmmss"))
                .WithHeader(GetHeaderAvalista());

            foreach (var model in results.records.ToList())
            {
                excel
                    .CreateCellValue(model.Regional).Width(15)
                    .CreateCellValue(model.UF).Width(15)
                    .CreateCellValue(model.CodigoPreProposta).Width(40)
                    .CreateCellValue(model.SituacaoPrePropostaSuatEvs).Width(23)
                    .CreateCellValue(model.SituacaoAvalista).Width(23)
                    .CreateDateTimeCell(model.DataHoraUltimoEnvio.HasValue ? model.DataHoraUltimoEnvio.Value.ToDateTime().FromDateTime() : Convert.ToDateTime(null), DateTimeExtensions.PatternDateTimeSeconds).Width(30)
                    .CreateCellValue(model.ProponenteUm).Width(40)
                    .CreateCellValue(model.CpfProponenteUm.ToCPFFormat()).Width(30)
                    .CreateCellValue(model.ProponenteDois).Width(40)
                    .CreateCellValue(model.CpfProponenteDois.ToCPFFormat()).Width(30)
                    .CreateCellValue(model.BreveLancamento).Width(40)
                    .CreateCellValue(model.EmpresaVenda).Width(40)
                    .CreateCellValue(model.NomeElaborador).Width(40)
                    .CreateCellValue(model.Corretor).Width(40)
                    .CreateCellValue(model.AgenteViabilizador).Width(40)
                    .CreateDateTimeCell(model.DataElaboracao.HasValue ? model.DataElaboracao.Value.ToDateTime().FromDateTime() : Convert.ToDateTime(null), DateTimeExtensions.PatternDateTimeSeconds).Width(30)
                    .CreateDateTimeCell(model.DataInicioPrimeiraAnalise.HasValue ? model.DataInicioPrimeiraAnalise.Value.ToDate().FromDate() : Convert.ToDateTime(null)).Width(18)
                    .CreateCellValue(model.HoraInicioPrimeiraAnalise.HasValue ? model.HoraInicioPrimeiraAnalise.Value.ToTime() : "").Width(18)
                    .CreateCellValue(model.HoraFimPrimeiraAnalise.HasValue ? model.HoraFimPrimeiraAnalise.Value.ToTime() : "").Width(18)
                    .CreateCellValue(model.NomeUsuarioPrimeiraAnalise).Width(18)
                    .CreateCellValue(model.ContadorPrimeiraAnalise.MinutesToDuration()).Width(18)
                    .CreateDateTimeCell(model.DataInicioUltimaAnalise.HasValue ? model.DataInicioUltimaAnalise.Value.ToDate().FromDate() : Convert.ToDateTime(null)).Width(18)
                    .CreateCellValue(model.HoraInicioUltimaAnalise.HasValue ? model.HoraInicioUltimaAnalise.Value.ToTime() : "").Width(18)
                    .CreateCellValue(model.HoraFimUltimaAnalise.HasValue ? model.HoraFimUltimaAnalise.Value.ToTime() : "").Width(18)
                    .CreateCellValue(model.NomeUsuarioUltimaAnalise).Width(18)
                    .CreateCellValue(model.Contador.MinutesToDuration()).Width(10)
                    .CreateCellValue(model.ContadorTotal.MinutesToDuration()).Width(15)
                    .CreateCellValue(model.QuantidadeAnalise).Width(30)
                    .CreateCellValue(model.PropostasAnteriores).Width(30)
                    .CreateCellValue(model.NumeroPropostasAnteriores).Width(30)
                    .CreateCellValue(model.MotivoPendencia).Width(45)
                    .CreateCellValue(model.StatusSicaq.AsString()).Width(40)
                    .CreateDateTimeCell(model.DataSicaq.HasValue ? model.DataSicaq.Value.ToDateTime().FromDateTime() : Convert.ToDateTime(null), DateTimeExtensions.PatternDateTimeSeconds).Width(30)
                    .CreateCellValue(model.NomeAnalistaSicaq).Width(40)
                    .CreateMoneyCell(model.ParcelaAprovada).Width(45)
                    .CreateCellValue(model.Observacao).Width(40);
            }
            excel.Close();
            return excel.DownloadFile();
        }

        private string[] GetHeaderAvalista()
        {
            IList<string> header = new List<string>
            {
                GlobalMessages.Regional,
                GlobalMessages.UF,
                GlobalMessages.Codigo,
                GlobalMessages.SituacaoPreProposta,
                GlobalMessages.SituacaoAvalista,
                GlobalMessages.DataHoraUltimoEnvio,
                GlobalMessages.ProponenteUm,
                GlobalMessages.CpfProponenteUm,
                GlobalMessages.ProponenteDois,
                GlobalMessages.CpfProponenteDois,
                GlobalMessages.Produto,
                GlobalMessages.EmpresaVenda,
                GlobalMessages.Elaborador,
                GlobalMessages.Corretor,
                GlobalMessages.Viabilizador,
                GlobalMessages.DataDaElaboracao,
                GlobalMessages.DataDaPrimeiraAnalise,
                GlobalMessages.HoraDaPrimeiraAnalise,
                GlobalMessages.FimDaPrimeiraAnalise,
                GlobalMessages.AnalistaPrimeiraAnalise,
                GlobalMessages.ContadorPrimeiraAnalise,
                GlobalMessages.DataDaUltimaAnalise,
                GlobalMessages.HoraDaUltimaAnalise,
                GlobalMessages.FimDaUltimaAnalise,
                GlobalMessages.AnalistaUltimaAnalise,
                GlobalMessages.ContadorUltimaAnalise,
                GlobalMessages.ContadorTotal,
                GlobalMessages.QuantidadeAnalise,
                GlobalMessages.HouvePropostasAnteriores,
                GlobalMessages.NumeroPropostasAnteriores,
                GlobalMessages.MotivoPendencia,
                GlobalMessages.StatusSicaq,
                GlobalMessages.DataSicaq,
                GlobalMessages.AnalistaSicaq,
                GlobalMessages.ParcelaAprovadaDoSICAQ,
                GlobalMessages.Observacao
            };
            return header.ToArray();
        }
    }
}
