Europa.Controllers.Alocar = {};
Europa.Controllers.Alocar.IdModal = "#modal-alocar";
Europa.Controllers.Alocar.FormAlocar = "#form-alocar";
$(function () {
});

Europa.Controllers.Alocar.AbrirModal = function () {
    $(Europa.Controllers.Alocar.IdModal).modal("show");
    setTimeout(() => { Europa.Controllers.Alocar.InitAutoCompletes(); }, 300);
    if (Europa.Controllers.Alocar.AutoCompleteUsuario) {
        Europa.Controllers.Alocar.AutoCompleteUsuario.Clean();
    };   
};
Europa.Controllers.Alocar.InitAutoCompletes = function () {
    Europa.Controllers.Alocar.AutoCompleteUsuario = new Europa.Components.AutoCompleteUsuario()
        .WithTargetSuffix("usuario")
        .Configure()
};

Europa.Controllers.Alocar.Salvar = function () {
    var param = {
        IdViatura: $("#IdViatura").val(),
        IdUsuario: Europa.Controllers.Alocar.AutoCompleteUsuario.Value()
    };

    $.post(Europa.Controllers.Alocar.UrlAlocar, param, function (res) {
        if (res.Success) {
            $(Europa.Controllers.Alocar.IdModal).modal("hide");
            Europa.Controllers.Viatura.Filtrar();
        }
        Europa.Informacao.PosAcaoBaseResponse(res);
    });
}