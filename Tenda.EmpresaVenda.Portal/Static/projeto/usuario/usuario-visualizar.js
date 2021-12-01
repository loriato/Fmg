Europa.Controllers.UsuarioVisualizar = {};
$(function () {
    Europa.Controllers.UsuarioVisualizar.InitMask();
});

Europa.Controllers.UsuarioVisualizar.Params = function () {
    var param = {
        IdUsuario: $("#Id").val()
    }
    return param;
}
Europa.Components.ViaturaUsuario.Params = function() {
   
    return Europa.Controllers.UsuarioVisualizar.Params();
};

Europa.Components.UsuarioPedidoCautela.Params = function () {
    return Europa.Controllers.UsuarioVisualizar.Params();
};

Europa.Components.UsuarioPedidoConsumo.Params = function () {
    return Europa.Controllers.UsuarioVisualizar.Params();
};


Europa.Controllers.UsuarioVisualizar.InitMask = function () {
    Europa.Mask.Telefone("#Telefone");
    Europa.Mask.Cpf("#Cpf", true);
    Europa.Mask.Apply("#DataNascimento", Europa.Mask.FORMAT_DATE);
};