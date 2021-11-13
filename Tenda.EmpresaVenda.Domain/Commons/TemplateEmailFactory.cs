using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.Hosting;

namespace Tenda.EmpresaVenda.Domain.Commons
{
    public static class TemplateEmailFactory
    {
        public static string TemplateDirectory = HostingEnvironment.ApplicationPhysicalPath + @"\static\template-email\";

        private static IDictionary<string, string> _cache = new Dictionary<string, string>();

        private static string ResolveFileContent(string templateName)
        {
            string templateBody = "";
            _cache.TryGetValue(templateName, out templateBody);

            if (templateBody != null)
            {
                return templateBody;
            }

            string fullFilePath = TemplateDirectory + templateName;

            if (File.Exists(fullFilePath))
            {
                templateBody = File.ReadAllText(fullFilePath);
                _cache.Add(templateName, templateBody);
            }
            return templateBody;
        }

        public static string ResolveTemplateWithReplace(string templateName, IDictionary<string, string> keyPairToReplace)
        {
            StringBuilder templateBody = new StringBuilder(ResolveFileContent(templateName));

            foreach (var toReplace in keyPairToReplace)
            {
                templateBody.Replace("{{" + toReplace.Key + "}}", toReplace.Value);
            }

            return templateBody.ToString();
        }

        public static void InvalidateCache()
        {
            _cache = new Dictionary<string, string>();
        }
    }
}