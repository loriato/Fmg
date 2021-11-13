namespace Tenda.EmpresaVenda.Domain.Integration
{
    public class SuatError
    {
        public SuatError(string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }
}
