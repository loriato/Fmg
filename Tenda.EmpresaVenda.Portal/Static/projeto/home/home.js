Europa.Controllers.Home = {};

$(function () {
    Europa.Controllers.Home.Esconder();
});
Europa.Components.ViaturaUsuario.Params = function () {

    return {};
};

Europa.Components.UsuarioPedidoCautela.Params = function () {
    return {};
};

Europa.Components.UsuarioPedidoConsumo.Params = function () {
    return {};
};

Europa.Controllers.Home.Esconder = function () {
    $("#cautelas").hide();
    $("#viaturas").hide();
    $("#consumos").hide();
};

Europa.Controllers.Home.Cautela = function () {
    $("#cautelas").show();
    $("#viaturas").hide();
    $("#consumos").hide();
};

Europa.Controllers.Home.Consumo = function () {
    $("#cautelas").hide();
    $("#viaturas").hide();
    $("#consumos").show();
};


Europa.Controllers.Home.Viatura = function () {
    $("#cautelas").hide();
    $("#viaturas").show();
    $("#consumos").hide();
};

