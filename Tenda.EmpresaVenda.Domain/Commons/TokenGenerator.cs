using System;
using System.Text;

namespace Europa.Fmg.Domain.Commons
{
    public static class TokenGenerator
    {
        private static string _allValues = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static int _allValuesLength = _allValues.Length;
        private static int _lengthToken = 6;

        public static string NewToken()
        {
            return NewToken(_lengthToken);
        }

        public static string NewToken(int length)
        {
            StringBuilder token = new StringBuilder();
            Random random = new Random();
            while (length > 0)
            {
                int charPosition = random.Next(0, _allValuesLength);
                token.Append(_allValues[charPosition]);
                length--;
            }
            return token.ToString();
        }

    }
}
