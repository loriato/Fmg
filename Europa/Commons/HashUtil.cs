using System;
using System.Security.Cryptography;
using System.Text;

namespace Europa.Commons
{
    public static class HashUtil
    {
        private static string Salt = "Xjab1238*a61}";
        /// </summary>
        /// <param name="text">input string</param>
        /// <param name="enc">Character encoding</param>
        /// <returns>SHA1 hash</returns>
        public static string SHA1(string text, Encoding enc)
        {
            var saltedText = text + Salt;
            byte[] buffer = enc.GetBytes(saltedText);
            SHA1CryptoServiceProvider cryptoTransformSHA1 = new SHA1CryptoServiceProvider();
            return BitConverter.ToString(cryptoTransformSHA1.ComputeHash(buffer)).Replace("-", "");
        }

        /// </summary>
        /// <param name="text">input string</param>
        /// <returns>SHA1 hash with Encoding.ASCII</returns>
        public static string SHA1(string text)
        {
            return SHA1(text, Encoding.ASCII);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="enc"></param>
        /// <returns></returns>
        public static string SHA512(string text, Encoding enc)
        {
            var saltedText = text + Salt;
            byte[] sourceBytes = enc.GetBytes(saltedText);
            byte[] hashBytes = System.Security.Cryptography.SHA512.Create().ComputeHash(sourceBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.AppendFormat("{0:x2}", hashBytes[i]);
            }
            return sb.ToString();
        }

        public static string SHA256(byte[] content)
        {
            var mySHA256 = System.Security.Cryptography.SHA256.Create();
            byte[] hashValue = mySHA256.ComputeHash(content);
            StringBuilder hash = new StringBuilder();
            for (int i = 0; i < hashValue.Length; i++)
            {
                hash.Append($"{hashValue[i]:X2}");
            }
            return hash.ToString();
        }

        public static string SHA512(byte[] content)
        {
            var sha = System.Security.Cryptography.SHA512.Create();
            byte[] hashValue = sha.ComputeHash(content);
            StringBuilder hash = new StringBuilder();
            for (int i = 0; i < hashValue.Length; i++)
            {
                hash.Append($"{hashValue[i]:X2}");
            }
            return hash.ToString();
        }

        public static string SHA512(string text)
        {
            return SHA512(text, Encoding.ASCII);
        }

        /// </summary>
        /// <param name="text">input string</param>
        /// <param name="enc">Character encoding</param>
        /// <returns>MD5 hash</returns>
        public static string MD5(string text, Encoding enc)
        {
            string saltedText = text + Salt;

            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = enc.GetBytes(saltedText);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        /// </summary>
        /// <param name="text">input string</param>
        /// <returns>MD5 hash with Encoding.ASCII</returns>
        public static string MD5(string text)
        {
            return MD5(text, Encoding.ASCII);
        }

        public static string MagentoMD5(MD5 md5Hash, string input, string random)
        {
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(random + input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

    }
}
