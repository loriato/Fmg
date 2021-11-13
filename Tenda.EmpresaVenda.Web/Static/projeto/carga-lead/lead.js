Europa.Controllers.Lead = {};
Europa.Controllers.Lead.TreeViewEv = undefined;


$(function () {
    $("#Estado").select2({
        trags: true
    });
    Europa.Controllers.Lead.TreeViewEv = new Europa.Components.TreeView("empresa_venda_treeview");
    Europa.Controllers.Lead.AtualizarTreeViewEv();
})

Europa.Controllers.Lead.OnRegionalChanged = function () {
    Europa.Controllers.Lead.AtualizarTreeViewEv();
};

Europa.Controllers.Lead.FiltroTreeViewEv = function () {
    var param = {

        estado: $("#Estado").val(),
        empresaVenda: $("#EmpresaVenda").val(),
        isCargaLead: true
    };


    return param;
};

Europa.Controllers.Lead.Filtrar = function () {
        Europa.Controllers.Lead.AtualizarTreeViewEv();    
};

Europa.Controllers.Lead.LimparFiltro = function () {
    $("#EmpresaVenda").val("");
}

Europa.Controllers.Lead.AtualizarTreeViewEv = function () {
    Europa.Controllers.Lead.TreeViewEv
        .WithAjax("GET",
            Europa.Controllers.Lead.UrlListarEvs,
            Europa.Controllers.Lead.FiltroTreeViewEv)
        .WithShowCheckbox(true)
        .WithRowCheck(true)
        .WithExpandIcon(false)
        .WithCollapseIcon(false)
        .WithCheckRootSiblings(true)
        .Configure();
};

Europa.Controllers.Lead.MarcarTodos = function () {
    Europa.Controllers.Lead.TreeViewEv.CheckAllNodes();
};

Europa.Controllers.Lead.DesmarcarTodos = function () {
    Europa.Controllers.Lead.TreeViewEv.UncheckAllNodes();
};
