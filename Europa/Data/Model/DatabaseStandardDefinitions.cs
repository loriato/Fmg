using Europa.Extensions;
using System.Collections.Generic;

namespace Europa.Data.Model
{

    /// <summary>
    /// Definições de banco de dados utilizadas para facilitar o mapeamento objeto relacional via FluentNHibernate
    /// 
    /// Contém também definições de erros de SchemaUpdate que não são impeditivos na criação da fábrica de conexões
    /// </summary>
    public static class DatabaseStandardDefinitions
    {
        public const int OneHundredTwentyEigthLength = 128;
        public const int TwoHundredFiftySixLength = 256;
        public const int FiveHundredTwelveLength = 512;
        public const int TwoThousandLength = 2000;
        public const int FourThousandLength = 4000;
        public const int CnpjLength = 18;
        public const int CpfLength = 14;
        public const int CepLength = 10;
        public const int CellphoneLength = 15;
        public const int UuidLength = 40;
        public const int InitialsLength = 5;
        public const int TenLength = 10;
        public const int TwentyLength = 20;
        public const int ThirtyLength = 30;
        public const int FortyLength = 40;
        public const int FiftyLength = 50;
        public const int SixtyLength = 60;
        public const int ThreeLength = 3;
        public const int FourLength = 4;
        public const int FiveLength = 5;
        public const string LargeStringCustomType = "TEXT";
        public const string LargeObjectCustomType = "BYTEA";
        public const string AnsiString = "AnsiString";


        private static Dictionary<string, string> _postgreCommonErrors = new Dictionary<string, string>();

        public static Dictionary<string, string> PostgreCommonErrors
        {
            get
            {
                if (_postgreCommonErrors.IsEmpty())
                {
                    _postgreCommonErrors.Add("42P07: relation ", "Não entende que as sequencias já estão criadas");
                }
                return _postgreCommonErrors;
            }
        }
    }
}
