using System;
using System.Collections.Concurrent;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.EmpresaVenda.Domain.Integration.Conecta.Models
{
    public class LeadConectaDto
    {
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        public string Vendedor { get; set; }
        public string OrigemContato { get; set; }
        
        //lead Conecta
        public string Uuid { get; set; }
        public string TelefoneLead { get; set; }
        public string NomeLead { get; set; }

        public ConcurrentDictionary<string,string> Atributos { get; set; }

        //integração
        public string Cep { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string Bairro { get; set; }
        public string TelefoneAdicional { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public TipoSexo Sexo { get; set; }

    }
}
