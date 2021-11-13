using System.Collections.Generic;

namespace Europa.Commons.LDAP
{
    public class LdapModel
    {
        public string Login { get; set; }
        public string Email { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public List<string> GruposActiveDiretory { get; set; }
    }
}
