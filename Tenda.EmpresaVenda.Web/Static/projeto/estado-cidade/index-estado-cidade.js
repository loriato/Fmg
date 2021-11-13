Europa.Controllers.EstadoCidade = {};
Europa.Controllers.EstadoCidade.OnEstado = "xxxx";

$(function () {

});

Europa.Controllers.EstadoCidade.LimparErro = function () {
    $("[name='Cidade']").parent().removeClass("has-error");
};

Europa.Controllers.EstadoCidade.AdicionarErro = function (campos) {
    campos.forEach(function (chave) {
        $("[name='" + chave + "']").parent().addClass("has-error");
    });
};