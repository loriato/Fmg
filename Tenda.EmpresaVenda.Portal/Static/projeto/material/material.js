Europa.Controllers.Material = {};
Europa.Controllers.Material.FormFiltro = "#form-filtro";
Europa.Controllers.Material.IdPedidoModal = "#modal-pedido";

Europa.Controllers.Material.Filtrar = function () {
    Europa.Controllers.Material.Consumo.DataTable.reloadData();
    Europa.Controllers.Material.Cautela.DataTable.reloadData();
};

Europa.Controllers.Material.AbrirCautela = function () {
    $("#tabela-consumo").addClass("hidden");
    $("#tabela-cautela").removeClass("hidden");
};

Europa.Controllers.Material.AbrirConsumo = function () {
    $("#tabela-cautela").addClass("hidden");
    $("#tabela-consumo").removeClass("hidden");

};

Europa.Controllers.Material.AbrirModalPedido = function () {
    $(Europa.Controllers.Material.IdPedidoModal).modal("show");
    setTimeout(() => { Europa.Controllers.Material.InitAutoCompletes(); }, 300);
    if (Europa.Controllers.Material.AutoCompleteUsuario) {
        Europa.Controllers.Material.AutoCompleteUsuario.Clean();
    };
};
Europa.Controllers.Material.Salvar = function () {
    var param = {
        IdMaterial: $("#IdMaterial").val(),
        IdUsuario: Europa.Controllers.Material.AutoCompleteUsuario.Value(),
        Quantidade: $("#Quantidade").val()
    };
    var url = Europa.Controllers.Material.modoCautela ? Europa.Controllers.Material.UrlPedidoCautela : Europa.Controllers.Material.UrlPedidoConsumo
    $.post(url, param, function (res) {
        if (res.Success) {
            $(Europa.Controllers.Material.IdPedidoModal).modal("hide");
            Europa.Controllers.Material.Filtrar();
        }
        Europa.Informacao.PosAcaoBaseResponse(res);
    });
};

Europa.Controllers.Material.InitAutoCompletes = function () {
    Europa.Controllers.Material.AutoCompleteUsuario = new Europa.Components.AutoCompleteUsuario()
        .WithTargetSuffix("usuario")
        .Configure()
};