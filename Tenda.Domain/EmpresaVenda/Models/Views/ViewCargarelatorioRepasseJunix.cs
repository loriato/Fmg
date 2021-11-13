using Europa.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.Domain.EmpresaVenda.Models.Views
{
    public class ViewCargarelatorioRepasseJunix : BaseEntity
    {
        public virtual SituacaoArquivo Situacao { get; set; }//erro ou sucesso
        public virtual TipoOrigem Origem { get; set; }//maunal ou robo
        public virtual DateTime? DataInicio { get; set; }
        public virtual DateTime? DataFim { get; set; }
        public virtual long IdArquivo { get; set; }
        public virtual string NomeArquivo { get; set; }
        public virtual long IdExecucao { get; set; }
        public virtual string  NomeUsuario { get; set; }
        public virtual long? TotalRegistros { get; set; }
        public virtual long? RegistroAtual { get; set; }

        public override string ChaveCandidata()
        {
            throw new NotImplementedException();
        }
    }
}
