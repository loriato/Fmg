using Europa.Extensions;
using Europa.Web.Menu;
using System.Collections.Generic;
using System.Text;
using Tenda.Domain.Shared;
using Tenda.EmpresaVenda.Portal.Models.Application;

namespace Tenda.EmpresaVenda.Portal.Commons
{
    public class MenuItemToBootStrap
    {
        private string _basePath;
        private List<string> itensEnabled = new List<string>();

        public string ProcessMenu(string baseAppPath, MenuItem menu)
        {
            _basePath = baseAppPath;

            if (SessionAttributes.Current().AcessoEVSuspensa)
            {
                DesabilitarItensMenu(menu);
            }

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

        private void DesabilitarItensMenu(MenuItem menu)
        {
            //Financeiro
            itensEnabled.Add("EVS19");

            var dropdownMenu = new MenuItem();
            foreach(var m in menu.Filhos)
            {
                if(m.Nome.ToLower().Equals("políticas de vendas"))
                {
                    dropdownMenu = m;
                }
            }

            foreach(var item in dropdownMenu.Filhos)
            {
                itensEnabled.Add(item.Codigo);
            }
            
        }

        private string BuildDropdown(MenuItem item)
        {
            return DropdownHtml(item.Nome, BuildSubDropdown(item.Filhos));
        }

        private string BuildLi(MenuItem item)
        {
            if (item.EnderecoAcesso.Equals("simulador"))
            {
                //return $"<li class='{(!itensEnabled.Contains(item.Codigo) && SessionAttributes.Current().AcessoEVSuspensa ? "disabled" : "")}'>" +
                  //  $"<a href='{_basePath}/{item.EnderecoAcesso.ToLower()}' target='_blank'><span class={item.Codigo.ToLower()}></span> <p>{item.Nome}</p></a></li>";
                return $"<li class='{(!itensEnabled.Contains(item.Codigo) && SessionAttributes.Current().AcessoEVSuspensa ? "disabled" : "")}'>" +
                    $"<a href='#'  onclick='Europa.Controllers.Simulador.MontarUrlSimuladorMenu()'><span class={item.Codigo.ToLower()}></span> <p>{item.Nome}</p></a></li>";
            }
            return $"<li class='{(!itensEnabled.Contains(item.Codigo) && SessionAttributes.Current().AcessoEVSuspensa ? "disabled" : "")}'>" +
                $"<a href='{_basePath}/{item.EnderecoAcesso.ToLower()}'><span class={item.Codigo.ToLower()}></span> <p>{item.Nome}</p></a></li>";
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
            var linkDropdown = $"<a href='#'><span class={label.ToLowerWithoutDiacritics().Replace(' ', '-')}></span> <p>{label}</p><i class='arrow-menu'></i></a>";
            var linksDropdownMenu = $"<ul class='sub-menu'><li class='min'><a href='#'></a></li>{links}<li class='min'><a href='#'></a></li></ul>";
            var dropdown = $"<li>{linkDropdown}{linksDropdownMenu}</li>";
            return dropdown;
        }
    }
}