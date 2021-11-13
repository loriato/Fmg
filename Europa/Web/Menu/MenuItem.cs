using System.Collections.Generic;

namespace Europa.Web.Menu
{
    public class MenuItem
    {
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string EnderecoAcesso { get; set; }
        public IList<MenuItem> Filhos { get; set; } = new List<MenuItem>();
        public int Ordem { get; set; }

        public MenuItem()
        {

        }

        public MenuItem(string nomeModulo, int ordem)
        {
            Nome = nomeModulo;
            Ordem = ordem;
        }

        public MenuItem(string codigo, string nome, string enderecoAcesso, int ordem)
        {
            this.Codigo = codigo;
            this.Nome = nome;
            this.EnderecoAcesso = enderecoAcesso;
            this.Ordem = ordem;
        }

        public bool IsModulo()
        {
            return Filhos != null && Filhos.Count > 0;
        }

        public bool IsUnidadeFuncional()
        {
            return !IsModulo();
        }

        public void AddFilho(MenuItem menuItem)
        {
            Filhos.Add(menuItem);
        }

        public MenuItem(string nomeModulo)
        {
            Nome = nomeModulo;
        }

        public MenuItem(string codigo, string nome, string enderecoAcesso)
        {
            this.Codigo = codigo;
            this.Nome = nome;
            this.EnderecoAcesso = enderecoAcesso;
        }

    }
}
