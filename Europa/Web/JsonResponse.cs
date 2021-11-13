using Europa.Commons;
using Europa.Rest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Europa.Web
{
    [Obsolete]
    // FIXME: Expose HTTP Code
    // Expose Internal Code
    // Campos e MEnsagens ser um dicionario
    // Objeto virar "Data"
    // Todos nomes em Ingles (Framework))
    public class JsonResponse
    {
        public object Objeto { get; set; }
        public bool Sucesso { get; set; }
        public int Code { get; set; }
        public List<String> Mensagens { get; set; }
        public List<String> Campos { get; set; }

        public JsonResponse()
        {
            Mensagens = new List<string>();
            Campos = new List<string>();
            Sucesso = false;
        }

        public void FromException(BusinessRuleException bre)
        {
            Sucesso = false;
            Mensagens.AddRange(bre.Errors);
            Campos.AddRange(bre.ErrorsFields);
        }

        public void FromBaseResponse(BaseResponse baseResponse)
        {
            Sucesso = baseResponse.Code == (int)HttpStatusCode.OK;
            Mensagens.AddRange(baseResponse.Messages);
            Objeto = baseResponse.Data;
        }

        public void FromApiException(ApiException e)
        {
            Sucesso = false;
            Mensagens.AddRange(e.GetResponse().Messages);
            Campos.AddRange(e.GetResponse().Fields.Select(x => x.Value));
        }
    }
}
