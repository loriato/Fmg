using System.Collections.Generic;
using System.Net;

namespace Tenda.EmpresaVenda.Domain.Integration.Simulador.Models
{
    public class ApiSimuladorResponseDto
    {
        public HttpStatusCode Code { get; set; }
        public bool Sucess { get; set; }
        public HttpStatusCode InternalCode { get; set; }
        public List<string> Messages { get; set; }
        public object Data { get; set; }
    }
}
