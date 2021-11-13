using Europa.Data.Model;

namespace Tenda.Domain.Security.Models
{
    public class QuartzConfiguration : BaseEntity
    {
        public virtual string Nome { get; set; }
        public virtual bool IniciarAutomaticamente { get; set; }
        public virtual bool ForcarDesligamento { get; set; }
        public virtual string CaminhoCompleto { get; set; }
        public virtual string Cron { get; set; }
        public virtual string Observacoes { get; set; }
        public virtual string ServidorExecucao { get; set; }
        public virtual string SiteExecucao { get; set; }
        public virtual string AplicacaoExecucao { get; set; }

        public override string ChaveCandidata()
        {
            return Nome;
        }
    }
}
