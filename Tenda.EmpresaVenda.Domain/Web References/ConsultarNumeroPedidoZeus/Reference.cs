﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
// 
#pragma warning disable 1591

namespace Tenda.EmpresaVenda.Domain.ConsultarNumeroPedidoZeus
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "ConsultarNumeroDePedidoSoapBinding", Namespace = "http://consultarnumerodepedido.cadastro.webservice.tenda.com.br/")]
    public partial class ConsultarNumeroDePedido : System.Web.Services.Protocols.SoapHttpClientProtocol
    {

        private dadosAuditoria auditoriaHeaderField;

        private System.Threading.SendOrPostCallback ConsultarNumeroPedidoOperationCompleted;

        private bool useDefaultCredentialsSetExplicitly;

        /// <remarks/>
        public ConsultarNumeroDePedido(string url)
        {
            this.Url = url;
            if ((this.IsLocalFileSystemWebService(this.Url) == true))
            {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else
            {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }

        public dadosAuditoria AuditoriaHeader
        {
            get
            {
                return this.auditoriaHeaderField;
            }
            set
            {
                this.auditoriaHeaderField = value;
            }
        }

        public new string Url
        {
            get
            {
                return base.Url;
            }
            set
            {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true)
                            && (this.useDefaultCredentialsSetExplicitly == false))
                            && (this.IsLocalFileSystemWebService(value) == false)))
                {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }

        public new bool UseDefaultCredentials
        {
            get
            {
                return base.UseDefaultCredentials;
            }
            set
            {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }

        /// <remarks/>
        public event ConsultarNumeroPedidoCompletedEventHandler ConsultarNumeroPedidoCompleted;

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("AuditoriaHeader")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("ConsultarNumeroPedido", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlArrayAttribute("ConsultarNumeroPedidoResponse", Namespace = "http://br.com.tenda.zeus/cadastro/response", IsNullable = true)]
        [return: System.Xml.Serialization.XmlArrayItemAttribute("Pedido", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public pedido[] ConsultarNumeroPedido([System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://br.com.tenda.zeus/cadastro/request", IsNullable = true)][System.Xml.Serialization.XmlArrayItemAttribute("NumeroRequisicaoCompra", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)] string[] ConsultarNumeroPedidoRequest)
        {
            object[] results = this.Invoke("ConsultarNumeroPedido", new object[] {
                        ConsultarNumeroPedidoRequest});
            return ((pedido[])(results[0]));
        }

        /// <remarks/>
        public void ConsultarNumeroPedidoAsync(string[] ConsultarNumeroPedidoRequest)
        {
            this.ConsultarNumeroPedidoAsync(ConsultarNumeroPedidoRequest, null);
        }

        /// <remarks/>
        public void ConsultarNumeroPedidoAsync(string[] ConsultarNumeroPedidoRequest, object userState)
        {
            if ((this.ConsultarNumeroPedidoOperationCompleted == null))
            {
                this.ConsultarNumeroPedidoOperationCompleted = new System.Threading.SendOrPostCallback(this.OnConsultarNumeroPedidoOperationCompleted);
            }
            this.InvokeAsync("ConsultarNumeroPedido", new object[] {
                        ConsultarNumeroPedidoRequest}, this.ConsultarNumeroPedidoOperationCompleted, userState);
        }

        private void OnConsultarNumeroPedidoOperationCompleted(object arg)
        {
            if ((this.ConsultarNumeroPedidoCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.ConsultarNumeroPedidoCompleted(this, new ConsultarNumeroPedidoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        public new void CancelAsync(object userState)
        {
            base.CancelAsync(userState);
        }

        private bool IsLocalFileSystemWebService(string url)
        {
            if (((url == null)
                        || (url == string.Empty)))
            {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024)
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0)))
            {
                return true;
            }
            return false;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.4084.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://br.com.tenda.zeus/cadastro/request")]
    [System.Xml.Serialization.XmlRootAttribute("AuditoriaHeader", Namespace = "http://br.com.tenda.zeus/cadastro/request", IsNullable = true)]
    public partial class dadosAuditoria : System.Web.Services.Protocols.SoapHeader
    {

        private string sistemaField;

        private string usuarioField;

        private System.DateTime dataTransacaoField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string sistema
        {
            get
            {
                return this.sistemaField;
            }
            set
            {
                this.sistemaField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string usuario
        {
            get
            {
                return this.usuarioField;
            }
            set
            {
                this.usuarioField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime dataTransacao
        {
            get
            {
                return this.dataTransacaoField;
            }
            set
            {
                this.dataTransacaoField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.4084.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://br.com.tenda.zeus/cadastro/request")]
    public partial class pedido
    {

        private string mandanteField;

        private string numeroRequisicaoCompraField;

        private string numeroItemRequisicaoCompraField;

        private string numeroDocumentoCompraField;

        private string numeroItemDocumentoCompraField;

        private System.DateTime dataField;

        private bool dataFieldSpecified;

        private string codigoLiberacaoDocumentoCompraField;

        private string linhaTextoField;

        private string statusField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Mandante
        {
            get
            {
                return this.mandanteField;
            }
            set
            {
                this.mandanteField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string NumeroRequisicaoCompra
        {
            get
            {
                return this.numeroRequisicaoCompraField;
            }
            set
            {
                this.numeroRequisicaoCompraField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string NumeroItemRequisicaoCompra
        {
            get
            {
                return this.numeroItemRequisicaoCompraField;
            }
            set
            {
                this.numeroItemRequisicaoCompraField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string NumeroDocumentoCompra
        {
            get
            {
                return this.numeroDocumentoCompraField;
            }
            set
            {
                this.numeroDocumentoCompraField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string NumeroItemDocumentoCompra
        {
            get
            {
                return this.numeroItemDocumentoCompraField;
            }
            set
            {
                this.numeroItemDocumentoCompraField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime Data
        {
            get
            {
                return this.dataField;
            }
            set
            {
                this.dataField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DataSpecified
        {
            get
            {
                return this.dataFieldSpecified;
            }
            set
            {
                this.dataFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string CodigoLiberacaoDocumentoCompra
        {
            get
            {
                return this.codigoLiberacaoDocumentoCompraField;
            }
            set
            {
                this.codigoLiberacaoDocumentoCompraField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string LinhaTexto
        {
            get
            {
                return this.linhaTextoField;
            }
            set
            {
                this.linhaTextoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    public delegate void ConsultarNumeroPedidoCompletedEventHandler(object sender, ConsultarNumeroPedidoCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class ConsultarNumeroPedidoCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal ConsultarNumeroPedidoCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public pedido[] Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((pedido[])(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591