using Europa.Extensions;
using System.Linq;

namespace Europa.Commons
{
    public static class ValidatorUtil
    {
        public static bool ValidateCpf(string cpf)
        {
            if (cpf.IsEmpty())
            {
                return false;
            }
            string valor = new string(cpf.Where(char.IsDigit).ToArray());
            if (valor.Length != 11)
            {
                return false;
            }

            bool igual = valor.Distinct().Count() == 1;
            if (igual)
            {
                return false;
            }

            int[] numeros = valor.Select(s => int.Parse(s.ToString())).ToArray();
            int soma = 0;

            for (int i = 0; i < 9; i++)
            {
                soma += (10 - i) * numeros[i];
            }

            int resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[9] != 0)
                {
                    return false;
                }
            }
            else if (numeros[9] != 11 - resultado)
            {
                return false;
            }

            soma = 0;
            for (int i = 0; i < 10; i++)
            {
                soma += (11 - i) * numeros[i];
            }

            resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {

                if (numeros[10] != 0)
                {
                    return false;
                }
            }
            else if (numeros[10] != 11 - resultado)
            {
                return false;
            }
            return true;
        }
        public static bool ValidateCnpj(string cnpj)
        {
            if (cnpj.IsEmpty())
            {
                return false;
            }
            string CNPJ = new string(cnpj.Where(char.IsDigit).ToArray());
            if (CNPJ.Length != 14)
            {
                return false;
            }

            int[] digitos, soma, resultado;
            int nrDig;
            string ftmt;
            bool[] cnpjOk;
            ftmt = "6543298765432";
            digitos = new int[14];
            soma = new[] { 0, 0 };
            resultado = new[] { 0, 0 };
            cnpjOk = new[] { false, false };
            try
            {
                for (nrDig = 0; nrDig < 14; nrDig++)
                {
                    digitos[nrDig] = int.Parse(
                        CNPJ.Substring(nrDig, 1));
                    if (nrDig <= 11)
                    {
                        soma[0] += digitos[nrDig] *
                                   int.Parse(ftmt.Substring(
                                       nrDig + 1, 1));
                    }

                    if (nrDig <= 12)
                    {
                        soma[1] += digitos[nrDig] *
                                   int.Parse(ftmt.Substring(
                                       nrDig, 1));
                    }
                }

                for (nrDig = 0; nrDig < 2; nrDig++)
                {
                    resultado[nrDig] = soma[nrDig] % 11;

                    if ((resultado[nrDig] == 0) || (
                            resultado[nrDig] == 1))
                    {
                        cnpjOk[nrDig] = digitos[12 + nrDig] == 0;
                    }
                    else
                    {
                        cnpjOk[nrDig] = digitos[12 + nrDig] == 11 - resultado[nrDig];
                    }
                }
                return cnpjOk[0] && cnpjOk[1];
            }
            catch
            {
                return false;
            }

        }
    }
}
