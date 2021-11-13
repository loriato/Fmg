using System.Collections.Generic;
using Tenda.Domain.EmpresaVenda.Enums;
using Tenda.Domain.Security.Enums;

namespace Tenda.EmpresaVenda.Domain.Repository.Models
{
    public class FiltroCorretorDTO
    {
        public long IdEmpresaVenda { get; set; }
        public string EmpresaVenda { get; set; }
        public List<SituacaoUsuario> Situacao { get; set; }
        public string Nome { get; set; }
        public string CpfCnpjCreci { get; set; }
        public List<TipoFuncao> Funcao { get; set; }
        public string Perfil { get; set; }
        public List<string> UF { get; set; }
        public List<long> IdRegional { get; set; }

    }
}