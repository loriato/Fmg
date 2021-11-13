using Europa.Extensions;
using System;
using System.Text;

namespace Europa.Commons
{
    public static class PasswordUtil
    {
        private const string ValidChars = "abcdefghijklmnopqrstuvwxyz1234567890@#!?";
        private const int MinPasswordLength = 8;
        private const int MaxPasswordLength = 16;

        public static string CreatePassword(int passwordLength)
        {
            //Aqui pego o valor máximo de caracteres para gerar a senha
            int maxlength = ValidChars.Length;

            //Criamos um objeto do tipo randon
            Random random = new Random(DateTime.Now.Millisecond);

            //Criamos a string que montaremos a senha
            StringBuilder senha = new StringBuilder(passwordLength);

            //Fazemos um for adicionando os caracteres a senha
            for (int i = 0; i < passwordLength; i++)
            {
                senha.Append(ValidChars[random.Next(0, maxlength)]);
            }

            //retorna a senha
            return senha.ToString();
        }

        public static string CreatePassword()
        {
            return CreatePassword(MinPasswordLength);
        }

        public static bool ValidatePassword(string password)
        {
            if (password.IsEmpty())
            {
                return false;
            }

            bool hasLetter = false;
            bool hasNumber = false;

            if (password.Length < MinPasswordLength || password.Length > MaxPasswordLength)
            {
                return false;
            }

            foreach (char letra in password)
            {
                if (char.IsLetter(letra))
                {
                    hasLetter = true;
                }
                else if (char.IsDigit(letra))
                {
                    hasNumber = true;
                }
                else
                {
                    //Do Nothing. Just to compliance SonarQube
                }
            }
            return hasLetter && hasNumber;
        }
    }
}
