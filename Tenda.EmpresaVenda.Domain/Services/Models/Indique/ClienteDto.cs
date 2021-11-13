using System;
using System.Collections.Generic;
using Tenda.Domain.EmpresaVenda.Enums;

namespace Tenda.EmpresaVenda.Domain.Services.Models.Indique
{
    public class ClienteDto
    {
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        public TipoSexo? Genero { get; set; }
        public TipoEstadoCivil? EstadoCivil { get; set; }
        public DateTime? DataNascimento { get; set; }
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Observacao { get; set; }
        public string IdSap { get; set; }
        public List<ContatoClienteDto> ContatoCliente { get; set; }
        public string Nacionalidade { get; set; }
        public string Filiacao { get; set; }
        public TipoDocumentoEnum? TipoDocumento { get; set; }
        public string Documento { get; set; }
        public string EstadoEmissor { get; set; }
        public DateTime? DataEmissao { get; set; }
        public ProfissaoDto Profissao { get; set; }
        public string Cargo { get; set; }
        public string Referencia { get; set; }
        public string Referencia2 { get; set; }
        public string TelefoneReferencia { get; set; }
        public string TelefoneReferencia2 { get; set; }
    }
}
