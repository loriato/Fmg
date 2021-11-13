using Europa.Commons;
using NHibernate;
using System;

namespace Europa.Development
{
    public static class Program
    {

        private static void Main(string[] args)
        {
            try
            {
                RelatorioAcessoService service = new RelatorioAcessoService();
                ISession session = MockDevDatabase.OpenSession();
                service.Init(session);

                service.Run();
            }
            catch (Exception e)
            {
                ExceptionUtility.LogException(e);
                Console.WriteLine(e);
            }
            MockDevDatabase.TearDown();
        }
    }
}