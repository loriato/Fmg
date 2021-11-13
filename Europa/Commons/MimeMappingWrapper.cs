using Microsoft.Win32;
using System.Linq;

namespace Europa.Commons
{
    public static class MimeMappingWrapper
    {
        public const string Xlsx = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        public const string Pdf = "application/pdf";

        public static string GetDefaultExtension(string mimeType)
        {
            RegistryKey key = Registry.ClassesRoot.OpenSubKey(@"MIME\Database\Content Type\" + mimeType, false);
            object value = key?.GetValue("Extension", null);
            return value?.ToString() ?? string.Empty;
        }

        public static string GetExtensionFromName(string filename)
        {
            return "." + filename.Split('.').Last().ToLower();
        }

        public static class MimeType
        {
            public const string Xlsx = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            public const string Pdf = "application/pdf";
            public const string Zip = "application/zip";
            public const string Jpeg = "image/jpeg";
            public const string Jpg = "image/jpg";
            public const string Png = "image/png";
            public const string Txt = "text/plain";
            public const string Docx = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
        }

    }
}
