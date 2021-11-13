using Europa.Data.Model;
using System;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.Security.Models;

namespace Tenda.Domain.EmpresaVenda.Models
{
    public class ImportacaoJunix : BaseEntity
    {
        public virtual SituacaoArquivo Situacao { get; set; }//erro ou sucesso
        public virtual TipoOrigem Origem { get; set; }//maunal ou robo
        public virtual DateTime? DataInicio { get; set; }
        public virtual DateTime? DataFim { get; set; }
        public virtual Arquivo Arquivo { get; set; }
        public virtual Execucao Execucao { get; set; }
        public virtual long TotalRegistros { get; set; }
        public virtual long RegistroAtual { get; set; }
        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
