using Europa.Data;
using Europa.Extensions;
using Europa.Resources;
using System.Collections.Generic;
using System.Linq;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.EmpresaVenda.Models.Views;
using Tenda.EmpresaVenda.Domain.Services.Models;

namespace Tenda.EmpresaVenda.Domain.Repository
{
    public class ViewConsultaPosVendaRepository : NHibernateRepository<ViewConsultaPosVenda>
    {
        public DataSourceResponse<ViewConsultaPosVenda> Listar(DataSourceRequest request, PosVendaDTO filtro)
        {
            //Retirando Prop. Cancelada da view e filtrando por Ev 
            var query = Queryable().Where(x => x.IdEmpresaVenda == filtro.IdEmpresaVenda)
                .Where(x => x.IdSituacaoPreProposta != 35 && x.IdSituacaoPreProposta != 21);

            if (!filtro.CodigoPreProposta.IsEmpty())
            {
                filtro.CodigoPreProposta = filtro.CodigoPreProposta.ToLower();
                query = query.Where(x=>x.CodigoPreProposta.ToLower().Contains(filtro.CodigoPreProposta));
            }

            if (!filtro.CodigoProposta.IsEmpty())
            {
                filtro.CodigoProposta = filtro.CodigoProposta.ToLower();
                query = query.Where(x => x.CodigoProposta.ToLower().Contains(filtro.CodigoProposta));
            }

            if (!filtro.NomeCliente.IsEmpty())
            {
                filtro.NomeCliente = filtro.NomeCliente.ToLower();
                query = query.Where(x => x.NomeClientePreProposta.ToLower().Contains(filtro.NomeCliente));
            }

            if (!filtro.IdCorretor.IsEmpty())
            {
                query = query.Where(x => x.IdCorretor == filtro.IdCorretor);
            }

            if (!filtro.IdProduto.IsEmpty())
            {
                query = query.Where(x => x.IdProduto == filtro.IdProduto);
            }
            
            if (!filtro.IdPontoVenda.IsEmpty())
            {
                query = query.Where(x => x.IdPontoVenda == filtro.IdPontoVenda);
            }

            if (filtro.IdSituacaoPreProposta.HasValue())
            {
                query = query.Where(x => x.IdSituacaoPreProposta == filtro.IdSituacaoPreProposta);
            }

            if (!filtro.StatusConformidade.IsEmpty())
            {
                query = query.Where(x=>x.StatusConformidade.ToLower().Contains(filtro.StatusConformidade));
            }

            switch (filtro.SituacaoContrato)
            {
                case SituacaoContrato.Repassado:
                    query = query.Where(x => x.DataRepasse != null)
                        .Where(x => x.DataRepasse.Date >= filtro.Inicio.Date)
                        .Where(x => x.DataRepasse.Date <= filtro.Termino.Date);
                    break;
                case SituacaoContrato.Conforme:
                    query = query.Where(x => x.DataConformidade != null)
                        .Where(x => x.DataConformidade.Value.Date >= filtro.Inicio.Date)
                        .Where(x => x.DataConformidade.Value.Date <= filtro.Termino.Date);
                    break;

                default:
                    query = query.Where(x => x.DataVenda.Date >= filtro.Inicio.Date || x.DataVenda.Date == null)
                        .Where(x => x.DataVenda.Date <= filtro.Termino.Date || x.DataVenda.Date == null);
                    break;
            }

            switch (filtro.TipoFiltroPosVenda)
            {
                case TipoFiltroPosVenda.SemDocsAvalista:
                    query = query.Where(x => x.SituacaoDocAvalista != SituacaoAprovacaoDocumentoAvalista.Enviado && x.SituacaoDocAvalista != SituacaoAprovacaoDocumentoAvalista.PreAprovado || x.SituacaoDocAvalista == null);
                    query = query.Where(x => x.PosChaves > 0 || (x.PreChaves > 0 && x.PreChaves <= 1000) || x.SituacaoDocAvalista == SituacaoAprovacaoDocumentoAvalista.NaoAnexado || x.SituacaoDocAvalista == SituacaoAprovacaoDocumentoAvalista.Pendente);
                    break;
                case TipoFiltroPosVenda.DocsAvalistaEnviados:
                    query = query.Where(x => x.SituacaoDocAvalista == SituacaoAprovacaoDocumentoAvalista.Enviado);
                    break;
                case TipoFiltroPosVenda.AvalistaPreAprovado:
                    query = query.Where(x => x.SituacaoDocAvalista == SituacaoAprovacaoDocumentoAvalista.PreAprovado);
                    break;
            }

            return query.ToDataRequest(request);
        }

        public IQueryable<ViewConsultaPosVenda> Listar(PosVendaDTO filtro)
        {
            var query = Queryable()
            .Where(x => x.IdEmpresaVenda == filtro.IdEmpresaVenda)
            .Where(x => x.DataConformidade!= null)
            .Where(x => x.DataConformidade.Value.Date >= filtro.Inicio)
            .Where(x => x.DataConformidade.Value.Date <= filtro.Termino);

            switch (filtro.SituacaoContrato)
            {
                case SituacaoContrato.Repassado:
                    query = query.Where(x => x.DataRepasse != null);
                    break;
                case SituacaoContrato.Conforme:
                    query = query.Where(x => x.DataConformidade != null);
                    break;
            }

            return query;
        }
        
        //Verificar situação como agson
        public int TotalKitCompleto(PosVendaDTO filtro)
        {
            var query = Queryable()
                .Where(x => x.IdEmpresaVenda == filtro.IdEmpresaVenda)
                .Where(x=>x.StatusProposta.ToLower().Equals("kit completo"));

            if (filtro.Inicio.Date.HasValue())
            {
                query = query.Where(x => x.DataVenda.Date >= filtro.Inicio.Date);
            }

            if (filtro.Termino.Date.HasValue())
            {
                query = query.Where(x => x.DataVenda.Date <= filtro.Termino.Date);
            }

            return query.Count();
        }

        public int TotalRepasse(PosVendaDTO filtro)
        {
            var query = Queryable()
                .Where(x => x.IdEmpresaVenda == filtro.IdEmpresaVenda)
                .Where(x => x.DataRepasse != null);

            if (filtro.Inicio.Date.HasValue())
            {
                query = query.Where(x => x.DataRepasse.Date >= filtro.Inicio.Date);
            }

            if (filtro.Termino.Date.HasValue())
            {
                query = query.Where(x => x.DataRepasse.Date <= filtro.Termino.Date);
            }

            return query.Count();
        }

        public int TotalConformidade(PosVendaDTO filtro)
        {
            var query = Queryable()
                .Where(x => x.IdEmpresaVenda == filtro.IdEmpresaVenda)
                .Where(x => x.DataConformidade != null);

            if (filtro.Inicio.Date.HasValue())
            {
                query = query.Where(x => x.DataConformidade.Value.Date >= filtro.Inicio.Date);
            }

            if (filtro.Termino.Date.HasValue())
            {
                query = query.Where(x => x.DataConformidade.Value.Date <= filtro.Termino.Date);
            }

            return query.Count();
        }

        public int ListarSemDocsAvalista(PosVendaDTO filtro)
        {
            var query = Queryable().Where(x => x.IdEmpresaVenda == filtro.IdEmpresaVenda)
                .Where(x => x.IdSituacaoPreProposta != 35 && x.IdSituacaoPreProposta != 21);

            query = query.Where(x => x.DataVenda.Date >= filtro.Inicio.Date || x.DataVenda.Date == null)
                .Where(x => x.DataVenda.Date <= filtro.Termino.Date || x.DataVenda.Date == null);

            query = query.Where(x => x.SituacaoDocAvalista != SituacaoAprovacaoDocumentoAvalista.Enviado && x.SituacaoDocAvalista != SituacaoAprovacaoDocumentoAvalista.PreAprovado || x.SituacaoDocAvalista == null);

            var result = query.Count(x => x.PosChaves > 0 || (x.PreChaves > 0 && x.PreChaves <= 1000) || x.SituacaoDocAvalista == SituacaoAprovacaoDocumentoAvalista.NaoAnexado || x.SituacaoDocAvalista == SituacaoAprovacaoDocumentoAvalista.Pendente);
            return result;
        }

        public int ListarDocsAvalistaEnviados(PosVendaDTO filtro)
        {
            var query = Queryable().Where(x => x.IdEmpresaVenda == filtro.IdEmpresaVenda)
                .Where(x => x.IdSituacaoPreProposta != 35 && x.IdSituacaoPreProposta != 21);

            return query.Where(x => x.DataVenda.Date >= filtro.Inicio.Date || x.DataVenda.Date == null)
                        .Where(x => x.DataVenda.Date <= filtro.Termino.Date || x.DataVenda.Date == null)
                        .Count(x => x.SituacaoDocAvalista == SituacaoAprovacaoDocumentoAvalista.Enviado);
        }
        public int ListarAvalistaPreAprovado(PosVendaDTO filtro)
        {
            var query = Queryable().Where(x => x.IdEmpresaVenda == filtro.IdEmpresaVenda)
                .Where(x => x.IdSituacaoPreProposta != 35 && x.IdSituacaoPreProposta != 21);

            return query.Where(x => x.DataVenda.Date >= filtro.Inicio.Date || x.DataVenda.Date == null)
                        .Where(x => x.DataVenda.Date <= filtro.Termino.Date || x.DataVenda.Date == null)
                        .Count(x => x.SituacaoDocAvalista == SituacaoAprovacaoDocumentoAvalista.PreAprovado);
        }

    }
}
