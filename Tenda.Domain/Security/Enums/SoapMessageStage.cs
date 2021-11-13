namespace Tenda.Domain.Security.Enums
{
    public enum SoapMessageStage
    {
        BeforeSerialize = 1,
        //
        // Resumo:
        //     O estágio imediatamente após um System.Web.Services.Protocols.SoapMessage ser
        //     serializada, mas antes da mensagem SOAP ser enviada pela rede.
        AfterSerialize = 2,
        //
        // Resumo:
        //     O estágio imediatamente antes de um System.Web.Services.Protocols.SoapMessage
        //     ser desserializado da mensagem SOAP enviada pela rede para um objeto.
        BeforeDeserialize = 4,
        //
        // Resumo:
        //     O estágio imediatamente após um System.Web.Services.Protocols.SoapMessage ser
        //     desserializado de uma mensagem SOAP para um objeto.
        AfterDeserialize = 8
    }
}
