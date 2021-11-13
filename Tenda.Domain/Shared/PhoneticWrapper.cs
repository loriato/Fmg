using Europa.Commons.Phonetic;

namespace Tenda.Domain.Shared
{
    public static class PhoneticWrapper
    {
        public static bool IsActive { get { return ProjectProperties.HabilitarFonetica; } }
        public static bool IsDisabled { get { return !IsActive; } }

        public static string BuildKey(string target)
        {
            if (IsDisabled)
            {
                return target;
            }
            return Fonetico.Fonetiza(target);
        }
    }
}
