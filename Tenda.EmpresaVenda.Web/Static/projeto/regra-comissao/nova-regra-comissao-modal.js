Europa.Controllers.ModalNovaRegraComissao = {
    Url: {
        Matriz: "",
        ListarEvs: ""
    },
    TreeViewEv: undefined
};

$(function () {
    Europa.Controllers.ModalNovaRegraComissao.TreeViewEv = new Europa.Components.TreeView("empresa_venda_treeview");
});

Europa.Controllers.ModalNovaRegraComissao.Abrir = function (regraComissao) {
    if (regraComissao !== undefined) {
        $("#IdRegraComissaoReferencia", "#modalNovaRegraComissao").val(regraComissao.Id);
        $("#Regional", "#modalNovaRegraComissao").val(regraComissao.Regional);
        $("#Regional", "#modalNovaRegraComissao").attr("disabled", true);
        $("#opcao_buscar_ultima_wrapper").css("display", "none");
    } else {
        $("#IdRegraComissaoReferencia", "#modalNovaRegraComissao").val("");
        $("#Regional", "#modalNovaRegraComissao").attr("disabled", false);
        $("#Regional", "#modalNovaRegraComissao").val($("#Regional option:first", "#modalNovaRegraComissao").val());
        $("#opcao_buscar_ultima_wrapper").css("display", "block");
    }
    Europa.Controllers.ModalNovaRegraComissao.AtualizarTreeViewEv();

    $("#modalNovaRegraComissao").show();
};

Europa.Controllers.ModalNovaRegraComissao.Fechar = function () {
    $("#IdRegraComissaoReferencia", "#modalNovaRegraComissao").val("");
    $("#Regional", "#modalNovaRegraComissao").val($("#Regional option:first", "#modalNovaRegraComissao").val());
    Europa.Controllers.ModalNovaRegraComissao.AtualizarTreeViewEv();

    $("#modalNovaRegraComissao").hide();
};

Europa.Controllers.ModalNovaRegraComissao.Confirmar = function () {
    var selecionados = Europa.Controllers.ModalNovaRegraComissao.TreeViewEv.GetCheckedNodes();

    if (selecionados.length == 0) {
        Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Informacao, Europa.i18n.Messages.MsgSelecioneEmpresaVenda);
        Europa.Informacao.Show();
        return;
    }

    var idRegraReferencia = $("#IdRegraComissaoReferencia", "#modalNovaRegraComissao").val();
    var regional = $("#Regional", "#modalNovaRegraComissao").val();
    var atualizado;
    if($("#IdRegraComissaoReferencia", "#modalNovaRegraComissao").val() === ""){
        atualizado = $("[name='BuscarUltimaAtt']:checked", "#modalNovaRegraComissao").val().toLowerCase() === "true";
    }else{
        atualizado = false;
    }

    Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.Confirmacao, Europa.i18n.Messages.ConfirmacaoInicioCriacaoRegraComissao);
    Europa.Confirmacao.ConfirmCallback = function () {
        var url = Europa.Controllers.ModalNovaRegraComissao.Url.Matriz + "?regional=" + regional;

        if (idRegraReferencia !== "") {
            url = url + "&regra=" + idRegraReferencia;
        }

        selecionados.forEach(function (empresaVenda) {
            url = url + "&evendas=" + empresaVenda.id;
        });

        if (atualizado) {
            url = url + "&ultimo=" + atualizado;
        }

        location.href = url;
    };
    Europa.Confirmacao.Show();
};

Europa.Controllers.ModalNovaRegraComissao.OnRegionalChanged = function () {
    Europa.Controllers.ModalNovaRegraComissao.AtualizarTreeViewEv();
};

Europa.Controllers.ModalNovaRegraComissao.FiltroTreeViewEv = function () {
    var idRegraReferencia = $("#IdRegraComissaoReferencia", "#modalNovaRegraComissao").val();
    var regional = $("#Regional", "#modalNovaRegraComissao").val();

    return {idRegraReferencia: idRegraReferencia, regional: regional};
};

Europa.Controllers.ModalNovaRegraComissao.AtualizarTreeViewEv = function () {
    Europa.Controllers.ModalNovaRegraComissao.TreeViewEv
        .WithAjax("GET",
            Europa.Controllers.ModalNovaRegraComissao.Url.ListarEvs,
            Europa.Controllers.ModalNovaRegraComissao.FiltroTreeViewEv)
        .WithShowCheckbox(true)
        .WithRowCheck(true)
        .WithExpandIcon(false)
        .WithCollapseIcon(false)
        .WithCheckRootSiblings(true)
        .Configure();
};

Europa.Controllers.ModalNovaRegraComissao.MarcarTodos = function () {
    Europa.Controllers.ModalNovaRegraComissao.TreeViewEv.CheckAllNodes();
};

Europa.Controllers.ModalNovaRegraComissao.DesmarcarTodos = function () {
    Europa.Controllers.ModalNovaRegraComissao.TreeViewEv.UncheckAllNodes();
};