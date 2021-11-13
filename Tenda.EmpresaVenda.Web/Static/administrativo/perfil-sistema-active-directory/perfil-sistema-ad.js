Europa.Controllers.PerfilSistemaGrupoAD = {};
Europa.Controllers.PerfilSistemaGrupoAD.Tabela = {};
Europa.Controllers.PerfilSistemaGrupoAD.AutoCompletePerfil = undefined;

$(function () {
    Europa.Controllers.PerfilSistemaGrupoAD.InicializarAutoCompletes();
});

Europa.Controllers.PerfilSistemaGrupoAD.InicializarAutoCompletes = function () {
    Europa.Controllers.PerfilSistemaGrupoAD.AutoCompletePerfil = new Europa.Components.AutoCompletePerfil()
        .WithTargetSuffix("perfil")
        .Configure();
};

Europa.Controllers.PerfilSistemaGrupoAD.Novo = function () {
    Europa.Controllers.PerfilSistemaGrupoAD.Tabela.createRowNewData();
    Europa.Controllers.PerfilSistemaGrupoAD.InicializarAutoCompletes()
    Europa.Controllers.PerfilSistemaGrupoAD.modoInclusao = true;

};

Europa.Controllers.PerfilSistemaGrupoAD.PreSalvar = function () {
    var obj = {
        Id: Europa.Controllers.PerfilSistemaGrupoAD.Tabela.getDataRowEdit().Id,
        IdPerfil: $("#autocomplete_perfil").val(),
        GrupoAd: $("#GrupoActiveDirectory").val()
    };

    var url = Europa.Controllers.PerfilSistemaGrupoAD.modoInclusao ?
        Europa.Controllers.PerfilSistemaGrupoAD.UrlIncluir : Europa.Controllers.PerfilSistemaGrupoAD.UrlAlterar;

    Europa.Controllers.PerfilSistemaGrupoAD.Salvar(obj, url);
};

Europa.Controllers.PerfilSistemaGrupoAD.Salvar = function (obj, url) {
    Europa.Controllers.PerfilSistemaGrupoAD.LimparErro();
    $.post(url, obj, function (res) {
        Europa.Controllers.PerfilSistemaGrupoAD.PosSalvar(res);
    });
};

Europa.Controllers.PerfilSistemaGrupoAD.PosSalvar = function (res) {
    if (res.Sucesso) {
        Europa.Controllers.PerfilSistemaGrupoAD.Tabela.closeEdition();
        Europa.Controllers.PerfilSistemaGrupoAD.Tabela.reloadData();
        Europa.Informacao.PosAcao(res);
        Europa.Controllers.PerfilSistemaGrupoAD.modoInclusao = false;
    } else {
        Europa.Controllers.PerfilSistemaGrupoAD.AdicionarErro(res.Campos);
        Europa.Informacao.PosAcao(res);
    }
};

Europa.Controllers.PerfilSistemaGrupoAD.PreDeletar = function (obj) {
    Europa.Confirmacao.PreAcao(Europa.i18n.Messages.Excluir, "", function () {
        Europa.Controllers.PerfilSistemaGrupoAD.Deletar(obj);
    });
};

Europa.Controllers.PerfilSistemaGrupoAD.Deletar = function (obj) {
    $.post(Europa.Controllers.PerfilSistemaGrupoAD.UrlExcluir, { id: obj.Id }, function (res) {
        Europa.Informacao.PosAcao(res);
        Europa.Controllers.PerfilSistemaGrupoAD.Tabela.reloadData();
    });
};

Europa.Controllers.PerfilSistemaGrupoAD.ExportarPagina = function (obj, url) {
    var params = Europa.Controllers.PerfilSistemaGrupoAD.Tabela.lastRequestParams;
    Europa.Components.DownloadFile(Europa.Controllers.PerfilSistemaGrupoAD.UrlExportarPagina, params)
};

Europa.Controllers.PerfilSistemaGrupoAD.ExportarTodos = function (obj, url) {
    var params = Europa.Controllers.PerfilSistemaGrupoAD.Tabela.lastRequestParams;
    Europa.Components.DownloadFile(Europa.Controllers.PerfilSistemaGrupoAD.UrlExportarTodos, params)
};

Europa.Controllers.PerfilSistemaGrupoAD.AdicionarErro = function (campos) {
    campos.forEach(function (chave) {
        $("[name='" + chave + "']").parent().addClass("has-error");
    });
}

Europa.Controllers.PerfilSistemaGrupoAD.LimparErro = function () {
    $("[name='NomePerfil']").parent().removeClass("has-error");
    $("[name='GrupoActiveDirectory']").parent().removeClass("has-error");
}