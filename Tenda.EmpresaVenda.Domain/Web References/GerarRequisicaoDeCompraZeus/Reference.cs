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

namespace Tenda.EmpresaVenda.Domain.GerarRequisicaoDeCompraZeus
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "GerarRequisicaoCompraSoapBinding", Namespace = "http://gerarrequisicaodecompra.cadastro.webservice.tenda.com.br/")]
    public partial class GerarRequisicaoCompra : System.Web.Services.Protocols.SoapHttpClientProtocol
    {

        private dadosAuditoria auditoriaHeaderField;

        private System.Threading.SendOrPostCallback CallGerarRequisicaoCompraOperationCompleted;

        private bool useDefaultCredentialsSetExplicitly;

        /// <remarks/>
        public GerarRequisicaoCompra(string url)
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
        public event CallGerarRequisicaoCompraCompletedEventHandler CallGerarRequisicaoCompraCompleted;

        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeaderAttribute("AuditoriaHeader")]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("GerarRequisicaoCompra", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlArrayAttribute("GerarRequisicaoCompraResponse", Namespace = "http://br.com.tenda.zeus/cadastro/response", IsNullable = true)]
        [return: System.Xml.Serialization.XmlArrayItemAttribute("requisicaoCompra", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
        public requisicaoCompraResponseItem[] CallGerarRequisicaoCompra([System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://br.com.tenda.zeus/cadastro/request", IsNullable = true)][System.Xml.Serialization.XmlArrayItemAttribute("requisicoesCompra", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)] requisicaoCompraRequestItem[] GerarRequisicaoCompraRequest)
        {
            object[] results = this.Invoke("CallGerarRequisicaoCompra", new object[] {
                        GerarRequisicaoCompraRequest});
            return ((requisicaoCompraResponseItem[])(results[0]));
        }

        /// <remarks/>
        public void CallGerarRequisicaoCompraAsync(requisicaoCompraRequestItem[] GerarRequisicaoCompraRequest)
        {
            this.CallGerarRequisicaoCompraAsync(GerarRequisicaoCompraRequest, null);
        }

        /// <remarks/>
        public void CallGerarRequisicaoCompraAsync(requisicaoCompraRequestItem[] GerarRequisicaoCompraRequest, object userState)
        {
            if ((this.CallGerarRequisicaoCompraOperationCompleted == null))
            {
                this.CallGerarRequisicaoCompraOperationCompleted = new System.Threading.SendOrPostCallback(this.OnCallGerarRequisicaoCompraOperationCompleted);
            }
            this.InvokeAsync("CallGerarRequisicaoCompra", new object[] {
                        GerarRequisicaoCompraRequest}, this.CallGerarRequisicaoCompraOperationCompleted, userState);
        }

        private void OnCallGerarRequisicaoCompraOperationCompleted(object arg)
        {
            if ((this.CallGerarRequisicaoCompraCompleted != null))
            {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.CallGerarRequisicaoCompraCompleted(this, new CallGerarRequisicaoCompraCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    public partial class requisicaoCompraResponseItem
    {

        private string numeroField;

        private string textoField;

        private string statusField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string numero
        {
            get
            {
                return this.numeroField;
            }
            set
            {
                this.numeroField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string texto
        {
            get
            {
                return this.textoField;
            }
            set
            {
                this.textoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string status
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.4084.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://br.com.tenda.zeus/cadastro/request")]
    public partial class contabilizacaoRequisicao
    {

        private string numeroField;

        private int numeroItemField;

        private int numeroSeqSegmentoClasseContabilField;

        private double quantidadeField;

        private string divisaoField;

        private string centroCustoField;

        private string numeroOrdemField;

        private string areaContabilidadeCustosField;

        private string centroDeLucroField;

        private string numeroContaRazaoField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string numero
        {
            get
            {
                return this.numeroField;
            }
            set
            {
                this.numeroField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int numeroItem
        {
            get
            {
                return this.numeroItemField;
            }
            set
            {
                this.numeroItemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int numeroSeqSegmentoClasseContabil
        {
            get
            {
                return this.numeroSeqSegmentoClasseContabilField;
            }
            set
            {
                this.numeroSeqSegmentoClasseContabilField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double quantidade
        {
            get
            {
                return this.quantidadeField;
            }
            set
            {
                this.quantidadeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string divisao
        {
            get
            {
                return this.divisaoField;
            }
            set
            {
                this.divisaoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string centroCusto
        {
            get
            {
                return this.centroCustoField;
            }
            set
            {
                this.centroCustoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string numeroOrdem
        {
            get
            {
                return this.numeroOrdemField;
            }
            set
            {
                this.numeroOrdemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string areaContabilidadeCustos
        {
            get
            {
                return this.areaContabilidadeCustosField;
            }
            set
            {
                this.areaContabilidadeCustosField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string centroDeLucro
        {
            get
            {
                return this.centroDeLucroField;
            }
            set
            {
                this.centroDeLucroField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string numeroContaRazao
        {
            get
            {
                return this.numeroContaRazaoField;
            }
            set
            {
                this.numeroContaRazaoField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.4084.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://br.com.tenda.zeus/cadastro/request")]
    public partial class itemRequisicao
    {

        private string numeroField;

        private string tipoDocumentoField;

        private int numeroItemField;

        private bool numeroItemFieldSpecified;

        private string grupoCompradoresField;

        private string nomeRequisitanteField;

        private System.DateTime dataSolicitacaoField;

        private string textoBreveField;

        private string numeroMaterialField;

        private string centroDeCustoField;

        private string grupoMercadoriasField;

        private int quantidadeField;

        private string unidadeMedidaField;

        private string tipoDataField;

        private System.DateTime dataRemessaItemField;

        private System.DateTime dataLiberacaoField;

        private double precoField;

        private double precoUnidadeField;

        private string categoriaItemDocumentoCompraField;

        private string categoriaClassificacaoContabilField;

        private string codigoEntradaMercadoriasField;

        private string codigoEntradaFaturasField;

        private string fornecedorPretendidoField;

        private string organizacaoDeComprasField;

        private string codigoMoedaField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string numero
        {
            get
            {
                return this.numeroField;
            }
            set
            {
                this.numeroField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string tipoDocumento
        {
            get
            {
                return this.tipoDocumentoField;
            }
            set
            {
                this.tipoDocumentoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int numeroItem
        {
            get
            {
                return this.numeroItemField;
            }
            set
            {
                this.numeroItemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool numeroItemSpecified
        {
            get
            {
                return this.numeroItemFieldSpecified;
            }
            set
            {
                this.numeroItemFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string grupoCompradores
        {
            get
            {
                return this.grupoCompradoresField;
            }
            set
            {
                this.grupoCompradoresField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string nomeRequisitante
        {
            get
            {
                return this.nomeRequisitanteField;
            }
            set
            {
                this.nomeRequisitanteField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime dataSolicitacao
        {
            get
            {
                return this.dataSolicitacaoField;
            }
            set
            {
                this.dataSolicitacaoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string textoBreve
        {
            get
            {
                return this.textoBreveField;
            }
            set
            {
                this.textoBreveField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string numeroMaterial
        {
            get
            {
                return this.numeroMaterialField;
            }
            set
            {
                this.numeroMaterialField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string centroDeCusto
        {
            get
            {
                return this.centroDeCustoField;
            }
            set
            {
                this.centroDeCustoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string grupoMercadorias
        {
            get
            {
                return this.grupoMercadoriasField;
            }
            set
            {
                this.grupoMercadoriasField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int quantidade
        {
            get
            {
                return this.quantidadeField;
            }
            set
            {
                this.quantidadeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string unidadeMedida
        {
            get
            {
                return this.unidadeMedidaField;
            }
            set
            {
                this.unidadeMedidaField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string tipoData
        {
            get
            {
                return this.tipoDataField;
            }
            set
            {
                this.tipoDataField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime dataRemessaItem
        {
            get
            {
                return this.dataRemessaItemField;
            }
            set
            {
                this.dataRemessaItemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime dataLiberacao
        {
            get
            {
                return this.dataLiberacaoField;
            }
            set
            {
                this.dataLiberacaoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double preco
        {
            get
            {
                return this.precoField;
            }
            set
            {
                this.precoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public double precoUnidade
        {
            get
            {
                return this.precoUnidadeField;
            }
            set
            {
                this.precoUnidadeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string categoriaItemDocumentoCompra
        {
            get
            {
                return this.categoriaItemDocumentoCompraField;
            }
            set
            {
                this.categoriaItemDocumentoCompraField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string categoriaClassificacaoContabil
        {
            get
            {
                return this.categoriaClassificacaoContabilField;
            }
            set
            {
                this.categoriaClassificacaoContabilField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string codigoEntradaMercadorias
        {
            get
            {
                return this.codigoEntradaMercadoriasField;
            }
            set
            {
                this.codigoEntradaMercadoriasField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string codigoEntradaFaturas
        {
            get
            {
                return this.codigoEntradaFaturasField;
            }
            set
            {
                this.codigoEntradaFaturasField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string fornecedorPretendido
        {
            get
            {
                return this.fornecedorPretendidoField;
            }
            set
            {
                this.fornecedorPretendidoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string organizacaoDeCompras
        {
            get
            {
                return this.organizacaoDeComprasField;
            }
            set
            {
                this.organizacaoDeComprasField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string codigoMoeda
        {
            get
            {
                return this.codigoMoedaField;
            }
            set
            {
                this.codigoMoedaField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.4084.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://br.com.tenda.zeus/cadastro/request")]
    public partial class requisicaoCompraRequestItem
    {

        private itemRequisicao itemRequisicaoField;

        private contabilizacaoRequisicao designacaoContaRequisicaoField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public itemRequisicao itemRequisicao
        {
            get
            {
                return this.itemRequisicaoField;
            }
            set
            {
                this.itemRequisicaoField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public contabilizacaoRequisicao designacaoContaRequisicao
        {
            get
            {
                return this.designacaoContaRequisicaoField;
            }
            set
            {
                this.designacaoContaRequisicaoField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    public delegate void CallGerarRequisicaoCompraCompletedEventHandler(object sender, CallGerarRequisicaoCompraCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.4084.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class CallGerarRequisicaoCompraCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal CallGerarRequisicaoCompraCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public requisicaoCompraResponseItem[] Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((requisicaoCompraResponseItem[])(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591