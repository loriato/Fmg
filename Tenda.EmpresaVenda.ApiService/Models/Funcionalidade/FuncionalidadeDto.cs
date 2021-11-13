using System.Collections.Generic;

namespace Tenda.EmpresaVenda.ApiService.Models.Funcionalidade
{
    public class FuncionalidadeDto
    {
        public string Nome { get; set; }
        public string Codigo { get; set; }

        public void FromKeyValuePair(KeyValuePair<string, string> keyValuePair)
        {
            Codigo = keyValuePair.Key;
            Nome = keyValuePair.Value;
        }
    }
}
