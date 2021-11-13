using System.Collections.Generic;
using System.Net;

namespace Europa.Web
{

    /// <summary>
    /// Proposta de padronização das respostas dadas pelos MVC controllers e API Controllers
    /// </summary>
    public class BaseResponse
    {
        public BaseResponse()
        {
            Success = false;
            Messages = new List<string>();
            Fields = new List<KeyValuePair<string, string>>();
        }

        /// <summary>
        /// Boleano de indicação de sucesso
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Código HTTP de Referência
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Código interno caso seja necessário para controle de fluxo e decisão
        /// </summary>
        public int InternalCode { get; set; }

        /// <summary>
        /// Lista de Mensagens a ser retornada (Não usar para dar mensagens de validação de campos)
        /// </summary>
        public List<string> Messages { get; set; }

        /// <summary>
        /// Estrutura KeyValue, par ser utilizada na valição dos campos da tela
        /// </summary>
        public List<KeyValuePair<string, string>> Fields { get; set; }

        /// <summary>
        /// Vai receber qualquer coisa referente ao retorno de objetos (pode ser um objeto, uma lista, etc)
        /// </summary>
        public object Data { get; set; }

        public void SuccessResponse(string message)
        {
            Messages.Add(message);
            Success = true;
            Code = (int)HttpStatusCode.OK;
        }

        public void SuccessResponse(IEnumerable<string> messages)
        {
            Messages.AddRange(messages);
            Success = true;
            Code = (int)HttpStatusCode.OK;
        }

        public void SuccessResponse(object data)
        {
            Success = true;
            Code = (int)HttpStatusCode.OK;
            Data = data;
        }

        public void SuccessResponse(string message,object data)
        {
            Success = true;
            Code = (int)HttpStatusCode.OK;
            Data = data;
            Messages.Add(message);
        }

        public void ErrorResponse(string message)
        {
            Messages.Add(message);
            Code = (int)HttpStatusCode.BadRequest;
        }

        public void ErrorResponse(IEnumerable<string> messages)
        {
            Messages.AddRange(messages);
            Code = (int)HttpStatusCode.BadRequest;
        }
    }
}
