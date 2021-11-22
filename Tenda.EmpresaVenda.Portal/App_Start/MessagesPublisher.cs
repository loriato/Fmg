using Europa.Fmg.Domain.Data;
using Europa.Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Text;
using System.Web;
using System.Web.Hosting;
using Tenda.Domain.Core.Enums;
using Tenda.Domain.Security.Enums;
using Tenda.Domain.Shared.Commons;
using Situacao = Tenda.Domain.Security.Enums.Situacao;

namespace Europa.Fmg.Portal.App_Start
{
    public class MessagesPublisher
    {
        private const string TargetPath = @"/Static/europa/dynamic/";

        public static void Publish()
        {
            PublishMessages();
            PublishEnum();
        }

        private static void PublishMessages()
        {
            ResourceSet resourceSet = GlobalMessages.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            var resEnum = resourceSet.GetEnumerator();
            StringBuilder resourcesToWrite = new StringBuilder()
                .AppendLine("Europa.i18n.Messages={");

            bool firstProcessed = false;
            string comma = ",";
            while (resEnum.MoveNext())
            {
                if (firstProcessed)
                {
                    resourcesToWrite.AppendLine(comma);
                }
                else
                {
                    firstProcessed = true;
                }
                resourcesToWrite.Append(resEnum.Key).Append(":\"").Append(HttpUtility.JavaScriptStringEncode(resEnum.Value.ToString())).Append("\"");
            }
            resourcesToWrite.Append("};");

            WriteToFile("europa-messages.js", resourcesToWrite);
        }

        private static void PublishEnum()
        {
            // Namespaces Definition
            List<string> enumNamespaces = new List<string>() {
                "Tenda.Domain.Core.Enums",
                "Tenda.Domain.EmpresaVenda.Enums",
                "Tenda.Domain.Security.Enums",
                "Tenda.Domain.Fmg.Enums",
            };

            List<Type> enumsToPublish = new List<Type>();

            var assemblies = NHibernateSession.CurrentAssemblies();
            foreach (string enumNamespace in enumNamespaces)
            {
                List<Type> publishing = ReflectionHelper.EnumInNamespace(enumNamespace, assemblies);
                enumsToPublish.AddRange(publishing);
            }

            // Write Content File
            StringBuilder resourcesToWrite = new StringBuilder()
                .Append("Europa.i18n.Enum={};Europa.i18n.Enum.Data={};")
                .Append("Europa.i18n.Enum.Resolve=function(type,value){return Europa.i18n.Enum.Data[type][value]};");

            string enumDeclarationFormat = "Europa.i18n.Enum.Data['{0}']={{}};";
            string enumValueFormat = "Europa.i18n.Enum.Data['{0}'][{1}]='{2}';";
            foreach (var enumeration in enumsToPublish)
            {
                var values = LocalizedEnumConverter.GetNumberValues(enumeration);
                resourcesToWrite.Append(String.Format(enumDeclarationFormat, enumeration.Name));
                foreach (KeyValuePair<int, string> pair in values)
                {
                    resourcesToWrite.Append(String.Format(enumValueFormat, enumeration.Name, pair.Key, pair.Value));
                }
            }

            // Write file On Path
            WriteToFile("europa-messages-enuns.js", resourcesToWrite);
        }

        private static void WriteToFile(string fileName, StringBuilder resourcesToWrite)
        {
            string writePath = HostingEnvironment.ApplicationPhysicalPath + TargetPath + fileName;

            StreamWriter sw = new StreamWriter(writePath, false);
            sw.Write(resourcesToWrite);
            sw.Close();
        }
    }
}