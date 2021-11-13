namespace Tenda.Domain.Shared.Models
{
    public class FilaPersonalizadaAuxiliarDTO
    {
        public virtual string CodigoIdentificador { get; set; }
        public virtual FilaPersonalizadaDTO FilaPersonalizada { get; set; }
    }
}
