using System.Collections.Generic;
using System.Text;

namespace Europa.Web.Menu
{
    public class MenuItemToBootStrap
    {
        private string _basePath;

        public string ProcessMenu(string baseAppPath, MenuItem menu)
        {
            _basePath = baseAppPath;

            if (menu?.Filhos == null)
            {
                return string.Empty;
            }

            var html = new StringBuilder();

            foreach (var m in menu.Filhos)
            {
                if (m.Filhos.Count > 0)
                {
                    html.AppendLine(BuildDropdown(m));
                }
                else
                {
                    html.AppendLine(BuildLi(m));
                }
            }
            return html.ToString();
        }

        private string BuildDropdown(MenuItem item)
        {
            return DropdownHtml(item.Nome, BuildSubDropdown(item.Filhos));
        }

        private string BuildLi(MenuItem item)
        {
            return $"<li><a href='{_basePath}/{item.EnderecoAcesso.ToLower()}'>{item.Nome}</a></li>";
        }

        private string BuildSubDropdown(IList<MenuItem> itens)
        {
            var html = new StringBuilder();
            foreach (var item in itens)
            {
                if (item.Filhos == null || item.Filhos.Count == 0)
                {
                    html.AppendLine(BuildLi(item));
                }
                else
                {
                    html.AppendLine(SubDropdownHtml(item.Nome, BuildSubDropdown(item.Filhos)));
                }
            }
            return html.ToString();
        }

        private string SubDropdownHtml(string label, string links)
        {
            var linkDropdown =
                $"<a href='#' class='dropdown-toggle' data-toggle='dropdown' role='button' aria-haspopup='true' aria-expanded='false'>{label}</a>";
            var linksDropdownMenu = $"<ul class='dropdown-menu'>{links}</ul>";
            var dropdown = $"<li class='dropdown dropdown-submenu'>{linkDropdown}{linksDropdownMenu}</li>";
            return dropdown;
        }

        private string DropdownHtml(string label, string links)
        {
            var linkDropdown =
                $"<a href='#' class='dropdown-toggle' data-toggle='dropdown' role='button' aria-haspopup='true' aria-expanded='false'>{label}<span class='caret'></span></a>";
            var linksDropdownMenu = $"<ul class='dropdown-menu'>{links}</ul>";
            var dropdown = $"<li class='dropdown'>{linkDropdown}{linksDropdownMenu}</li>";
            return dropdown;
        }
    }
}