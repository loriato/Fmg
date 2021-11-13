using NHibernate.Exceptions;

namespace Europa.Data
{
    public static class ConstraintViolationExceptionWrapper
    {
        public static bool IsConstraintViolationException(GenericADOException exp)
        {
            return exp.InnerException != null && exp.InnerException.Message.Contains("23503");
        }
    }
}
