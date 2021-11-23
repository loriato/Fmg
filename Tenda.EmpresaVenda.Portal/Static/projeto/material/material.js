Europa.Controllers.Material = {};
Europa.Controllers.Material.FormFiltro = "#form-filtro";

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