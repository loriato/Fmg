using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Europa.Extensions
{
    public static class StringExtensionMethods
    {

        private static readonly List<string> InvalidDomainNames = new List<string> {
            "@gmail.com.br",
        };


        public const string Line = "==================================================================================================";
        public const string Break = "\n";
        /// <summary>
        ///     Formata um Telefone com a máscara (00) 00000-0000 ou (00) 0000-0000
        /// </summary>
        /// <param name="phone">string phone</param>
        /// <returns></returns>
        public static string ToPhoneFormat(this string phone)
        {
            if (!phone.IsEmpty())
            {
                phone = phone.Trim();

                MaskedTextProvider mtpPhone;
                if (phone.Length == 11)
                {
                    mtpPhone = new MaskedTextProvider(@"\(00\) 00000\-0000");
                    mtpPhone.Set(phone.PadLeft(11, '0'));
                }
                else
                {
                    mtpPhone = new MaskedTextProvider(@"\(00\) 0000\-0000");
                    mtpPhone.Set(phone.PadLeft(10, '0'));
                }
                return mtpPhone.ToString();
            }
            return null;
        }

        public static void BreakLine(this StringBuilder builder)
        {

            SimpleBreakLine(builder);
            builder.Append(Line);
            SimpleBreakLine(builder);
        }

        public static void SimpleBreakLine(this StringBuilder builder)
        {
            builder.Append(Break);
        }

        /// <summary>
        /// Retorna string sem acentuação e em lowercase
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToLowerWithoutDiacritics(this String s)
        {
            var normalizedString = s.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    stringBuilder.Append(c);
            }
            return stringBuilder.ToString().ToLower();
        }


        public static string RemoveLeadingZeros(this string value)
        {
            if (value == null)
            {
                return value;
            }
            return value.TrimStart('0');
        }


        /// <summary>
        ///     Formata um CPF com a máscara 000.000.000-00
        /// </summary>
        /// <param name="cpf">string Cpf</param>
        /// <returns></returns>
        public static string ToCPFFormat(this string cpf)
        {
            if (!cpf.IsEmpty())
            {
                cpf = cpf.Trim();

                var mtpCpf = new MaskedTextProvider(@"000\.000\.000-00");
                mtpCpf.Set(cpf.PadLeft(11, '0'));
                return mtpCpf.ToString();
            }
            return null;
        }

        /// <summary>
        ///     Formata um CNPJ com a máscara 00.000.000/0000-00
        /// </summary>
        /// <param name="cnpj">string Cnpj</param>
        /// <returns></returns>
        public static string ToCNPJFormat(this string cnpj)
        {
            if (!cnpj.IsEmpty())
            {
                cnpj = cnpj.Trim();

                var mtpCnpj = new MaskedTextProvider(@"00\.000\.000/0000-00");
                mtpCnpj.Set(cnpj.PadLeft(14, '0'));
                return mtpCnpj.ToString();
            }

            return null;
        }

        /// <summary>
        ///     Remove a formatação de CNPJ
        /// </summary>
        /// <param name="cnpj">string Cnpj</param>
        /// <returns></returns>
        [Obsolete("Use Regex to Keep Only Numbers")]
        public static string ClearCNPJFormat(this string cnpj)
        {
            if (!cnpj.IsEmpty())
            {
                cnpj = cnpj.Trim();

                return cnpj.Replace(".", "").Replace("-", "").Replace("/", "").Trim();
            }

            return null;
        }

        /// <summary>
        ///     Formata um CEP com a máscara 00.000-00
        /// </summary>
        /// <param name="cnpj">string CEP</param>
        /// <returns></returns>
        public static string ToCEPFormat(this string cep)
        {
            if (!cep.IsEmpty())
            {
                var mtpCEP = new MaskedTextProvider(@"00000-000");
                mtpCEP.Set(cep.PadLeft(8, '0'));
                return mtpCEP.ToString();
            }

            return null;
        }

        /// <summary>
        ///     Formata uma inscricao estadual com a máscara 000.000.000.000
        /// </summary>
        /// <param name="insc">string inscricaoEstadual</param>
        /// <returns></returns>
        public static string ToInscricaoEstadualFormat(this string insc)
        {
            if (!insc.IsEmpty())
            {
                insc = insc.Trim();

                var mtpInsc = new MaskedTextProvider(@"000\.000\.000\.000");
                mtpInsc.Set(insc.PadLeft(12, '0'));
                return mtpInsc.ToString();
            }

            return null;
        }

        public static string OnlyNumber(this string texto)
        {
            if (texto.IsEmpty())
            {
                return string.Empty;
            }
            return Regex.Replace(texto, @"[^\d]", String.Empty).Trim();
        }

        /// <summary>
        ///     Valida um CNPJ passado como parâmetro
        /// </summary>
        /// <param name="cnpj">CNPJ a ser validado</param>
        /// <returns>True/False</returns>
        public static bool IsValidCNPJ(this string cnpj)
        {
            if (string.IsNullOrEmpty(cnpj))
                return false;

            var multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            int soma;
            int resto;
            string digito;
            string tempCnpj;
            cnpj = cnpj.Trim().OnlyNumber();

            if (cnpj.Length != 14)
                return false;

            tempCnpj = cnpj.Substring(0, 12);
            soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();

            tempCnpj = tempCnpj + digito;
            soma = 0;

            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto;
            return cnpj.EndsWith(digito);
        }

        /// <summary>
        ///     Valida um CPF passado como parâmetro
        /// </summary>
        /// <param name="cpf">CPF a ser validado</param>
        /// <returns>Tue/False</returns>
        public static bool IsValidCPF(this string cpf)
        {
            if (string.IsNullOrEmpty(cpf))
                return false;

            var multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            int cont = 0;
            for (int c = 0; c < cpf.Length; c++)
            {
                if (cpf.Substring(c, 1) == cpf.Substring(cpf.Length - 1))
                    cont++;
            }

            if (cont == 11)
                return false;

            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim().OnlyNumber();

            if (cpf.Length != 11)
                return false;

            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto;

            return cpf.EndsWith(digito);
        }


        /// <summary>
        ///     Verifica a validade da senha
        /// </summary>
        /// <param name="password">string password</param>
        /// <returns>bool that represents the validation</returns>
        public static bool IsValidPassword(this string password)
        {
            if (password.IsEmpty())
                return false;

            bool hasLetter = false;
            bool hasNumber = false;

            if (password.Length < 6 || password.Length > 16) return false;
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
            }
            return hasLetter && hasNumber;
        }

        /// <summary>
        ///     Verifica a validade do e-mail
        /// </summary>
        /// <param name="email">string email</param>
        /// <returns></returns>
        public static bool IsValidEmail(this string email)
        {
            bool isValid = new EmailAddressAttribute().IsValid(email);
            if (!isValid) { return false; }

            email = email.ToLower();

            // Se o domínio não for um dos inexistentes
            return !InvalidDomainNames.Any(reg => email.EndsWith(reg));
        }

        /// <summary>
        ///     Verifica a validade de cartões de crédito
        /// </summary>
        /// <param name="NumeroCartao">string creditCardNumber</param>
        /// <returns>bool</returns>
        public static bool IsValidCard(this string creditCardNumber)
        {
            if (string.IsNullOrEmpty(creditCardNumber))
            {
                return false;
            }

            int sumOfDigits = creditCardNumber.Where((e) => e >= '0' && e <= '9')
                            .Reverse()
                            .Select((e, i) => ((int)e - 48) * (i % 2 == 0 ? 1 : 2))
                            .Sum((e) => e / 10 + e % 10);
            return sumOfDigits % 10 == 0;
        }

        /// <summary>
        ///     Replica a string
        /// </summary>
        /// <param name="count">Quantidade de repetições</param>
        /// <returns>String replicada</returns>
        public static string DupeString(this string input, int? count)
        {
            StringBuilder result = new StringBuilder();
            while (count-- > 0)
            {
                result.Append(input);
            }

            return result.ToString();
        }

        public static string Truncate(this string value, int maxChars)
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars);
        }
    }
}