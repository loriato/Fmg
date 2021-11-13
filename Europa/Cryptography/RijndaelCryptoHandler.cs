using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Europa.Cryptography
{
    public class RijndaelCryptoHandler
    {
        private readonly string _key;

        public string Key
        {
            get
            {
                return this._key;
            }
        }

        public RijndaelCryptoHandler(string key)
        {
            this._key = key;
        }

        public string Decrypt(string stringToDecrypt)
        {
            string result;
            try
            {
                result = Decrypt(stringToDecrypt, _key);
            }
            catch (Exception inner)
            {
                throw new CryptographicUnexpectedOperationException(string.Format("Could not decrypt '{0}'.", stringToDecrypt), inner);
            }
            return result;
        }

        public string Encrypt(string stringToEncrypt)
        {
            string result;
            try
            {
                result = Encrypt(stringToEncrypt, _key);
            }
            catch (Exception inner)
            {
                throw new CryptographicUnexpectedOperationException(string.Format("Could not encrypt '{0}'.", stringToEncrypt), inner);
            }
            return result;
        }

        public byte[] Encrypt(byte[] clearData, byte[] Key, byte[] IV)
        {
            MemoryStream memoryStream = new MemoryStream();
            Rijndael rijndael = Rijndael.Create();
            rijndael.Key = Key;
            rijndael.IV = IV;
            CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndael.CreateEncryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(clearData, 0, clearData.Length);
            cryptoStream.Close();
            return memoryStream.ToArray();
        }

        public string Encrypt(string clearText, string Password)
        {
            UTF8Encoding uTF8Encoding = new UTF8Encoding();
            byte[] inArray = this.Encrypt(uTF8Encoding.GetBytes(clearText), uTF8Encoding.GetBytes(Password), uTF8Encoding.GetBytes(Password));
            return Convert.ToBase64String(inArray);
        }

        public byte[] Encrypt(byte[] clearData, string Password)
        {
            UTF8Encoding uTF8Encoding = new UTF8Encoding();
            return this.Encrypt(clearData, uTF8Encoding.GetBytes(Password), uTF8Encoding.GetBytes(Password));
        }

        public void Encrypt(string fileIn, string fileOut, string Password)
        {
            FileStream fileStream = new FileStream(fileIn, FileMode.Open, FileAccess.Read);
            FileStream stream = new FileStream(fileOut, FileMode.OpenOrCreate, FileAccess.Write);
            PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(Password, new byte[]
            {
                73,
                118,
                97,
                110,
                32,
                77,
                101,
                100,
                118,
                101,
                100,
                101,
                118
            });
            UTF8Encoding uTF8Encoding = new UTF8Encoding();
            Rijndael rijndael = Rijndael.Create();
            rijndael.KeySize = 128;
            rijndael.Key = uTF8Encoding.GetBytes(Password);
            rijndael.IV = uTF8Encoding.GetBytes(Password);
            CryptoStream cryptoStream = new CryptoStream(stream, rijndael.CreateEncryptor(), CryptoStreamMode.Write);
            int num = 4096;
            byte[] buffer = new byte[num];
            int num2;
            do
            {
                num2 = fileStream.Read(buffer, 0, num);
                cryptoStream.Write(buffer, 0, num2);
            }
            while (num2 != 0);
            cryptoStream.Close();
            fileStream.Close();
        }

        public byte[] Decrypt(byte[] cipherData, byte[] Key, byte[] IV)
        {
            MemoryStream memoryStream = new MemoryStream();
            Rijndael rijndael = Rijndael.Create();
            rijndael.Key = Key;
            rijndael.IV = IV;
            CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndael.CreateDecryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(cipherData, 0, cipherData.Length);
            cryptoStream.Close();
            return memoryStream.ToArray();
        }

        public string Decrypt(string cipherText, string Password)
        {
            byte[] cipherData = Convert.FromBase64String(cipherText);
            UTF8Encoding uTF8Encoding = new UTF8Encoding();
            byte[] bytes = this.Decrypt(cipherData, uTF8Encoding.GetBytes(Password), uTF8Encoding.GetBytes(Password));
            return uTF8Encoding.GetString(bytes);
        }

        public byte[] Decrypt(byte[] cipherData, string Password)
        {
            UTF8Encoding uTF8Encoding = new UTF8Encoding();
            return this.Decrypt(cipherData, uTF8Encoding.GetBytes(Password), uTF8Encoding.GetBytes(Password));
        }

        public void Decrypt(string fileIn, string fileOut, string Password)
        {
            FileStream fileStream = new FileStream(fileIn, FileMode.Open, FileAccess.Read);
            FileStream stream = new FileStream(fileOut, FileMode.OpenOrCreate, FileAccess.Write);
            UTF8Encoding uTF8Encoding = new UTF8Encoding();
            Rijndael rijndael = Rijndael.Create();
            rijndael.KeySize = 128;
            rijndael.Key = uTF8Encoding.GetBytes(Password);
            rijndael.IV = uTF8Encoding.GetBytes(Password);
            CryptoStream cryptoStream = new CryptoStream(stream, rijndael.CreateDecryptor(), CryptoStreamMode.Write);
            int num = 4096;
            byte[] buffer = new byte[num];
            int num2;
            do
            {
                num2 = fileStream.Read(buffer, 0, num);
                cryptoStream.Write(buffer, 0, num2);
            }
            while (num2 != 0);
            cryptoStream.Close();
            fileStream.Close();
        }
    }
}
