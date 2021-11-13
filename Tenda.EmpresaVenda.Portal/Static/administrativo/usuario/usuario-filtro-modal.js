Europa.Components.Modal.FiltroUsuario = {};
Europa.Components.Modal.FiltroUsuario.SelectorModal = undefined;
Europa.Components.Modal.FiltroUsuario.ActionSelecionar = undefined;

$(document).ready(function () {
    Europa.Components.Modal.FiltroUsuario.AutoCompletePerfil = new Europa.Components.AutoCompletePerfil()
        .WithTargetSuffix("perfil_modal_filtro_usuario")
        .Configure();
    Europa.Components.Modal.FiltroUsuario.AutoCompleteArea = new Europa.Components.AutoCompleteAreaNegocio()
        .WithTargetSuffix("area_modal_filtro_usuario")
        .Configure();
});

Europa.Components.Modal.FiltroUsuario.AbrirModal = function (selectCallback) {
    $("#FiltroUsuario").modal("show");
    Europa.Components.Modal.FiltroUsuario.ActionSelecionar = selectCallback;
}

Europa.Components.Modal.FiltroUsuario.Selecionar = function () {
    Europa.Components.Modal.FiltroUsuario.ActionSelecionar();
    Europa.Components.Modal.FiltroUsuario.CloseModal();
}

Europa.Components.Modal.FiltroUsuario.CloseModal = function () {
    $("#FiltroUsuario").modal("hide");
}

Europa.Components.Modal.FiltroUsuario.LimparFiltros = function () {
    Europa.Components.Modal.FiltroUsuario.AutoCompletePerfil.Clean();
    Europa.Components.Modal.FiltroUsuario.AutoCompleteArea.Clean();
    $("#formFiltroUsuario").trigger("reset");
    $("#autocomplete_perfil_modal_filtro_usuario").val("");
}

Europa.Components.Modal.FiltroUsuario.AbrirModalPerfil = function () {
    Europa.Components.ModalPerfil
    .WithSelectFuncion(Europa.Components.Modal.FiltroUsuario.ActionSelecionarPerfil)
        .Show();
}

Europa.Components.Modal.FiltroUsuario.ActionSelecionarPerfil = function (perfil) {
    Europa.Components.Modal.FiltroUsuario.AutoCompletePerfil.SetValue(perfil.Id, perfil.Nome);
}

Europa.Components.Modal.FiltroUsuario.AbrirModalAreas = function () {
    Europa.Components.AreaNegocioModal.AbrirModal(Europa.Components.Modal.FiltroUsuario.ActionSelecionarAreas);
}

Europa.Components.Modal.FiltroUsuario.ActionSelecionarAreas = function (area) {
    Europa.Components.Modal.FiltroUsuario.AutoCompleteArea.SetValue(area.Id, area.Nome);
}

