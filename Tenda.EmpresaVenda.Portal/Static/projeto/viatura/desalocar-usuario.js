Europa.Controllers.Desalocar = {};
Europa.Controllers.Desalocar.IdModal = "#modal-desalocar";
Europa.Controllers.Desalocar.FormAlocar = "#form-desalocar";
$(function () {
});

Europa.Controllers.Desalocar.AbrirModal = function () {
    $(Europa.Controllers.Desalocar.IdModal).modal("show");
    Europa.Controllers.Desalocar.InitMask();
    $("#QuilometragemNovo").val("");
    
};


Europa.Controllers.Desalocar.Salvar = function () {
    var param = {
        IdViatura: $("#IdViaturaDesalocar").val(),
        Quilometragem: $("#QuilometragemNovo").val()
    };

    $.post(Europa.Controllers.Desalocar.UrlDesalocar, param, function (res) {
        if (res.Success) {
            $(Europa.Controllers.Desalocar.IdModal).modal("hide");
            Europa.Controllers.Viatura.Filtrar();
        }
        Europa.Informacao.PosAcaoBaseResponse(res);
    });
}

Europa.Controllers.Desalocar.InitMask = function () {
    Europa.Mask.Int("#QuilometragemNovo");
};