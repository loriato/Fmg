using System;

namespace Europa.Rest
{
    public class InternalServerException : Exception
    {
        private ExceptionDto ExceptionDto = new ExceptionDto();

        public InternalServerException()
        {
        }

        public InternalServerException(ExceptionDto exception)
        {
            ExceptionDto = exception;
        }

        public ExceptionDto GetExceptionDto()
        {
            return ExceptionDto;
        }
    }
}