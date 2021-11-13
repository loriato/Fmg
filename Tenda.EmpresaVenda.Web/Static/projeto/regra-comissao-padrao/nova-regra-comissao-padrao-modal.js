Europa.Controllers.ModalNovaRegraComissaoPadrao = {
    Url: {
        Matriz: ""
    }
};

Europa.Controllers.ModalNovaRegraComissaoPadrao.Abrir = function (regraComissao) {
    if (regraComissao !== undefined) {
        $("#IdRegraComissaoReferencia", "#modalNovaRegraComissaoPadrao").val(regraComissao.Id);
        $("#Regional", "#modalNovaRegraComissaoPadrao").val(regraComissao.Regional);
        $("#Regional", "#modalNovaRegraComissaoPadrao").attr("disabled", true);
        $("#opcao_buscar_ultima_wrapper").css("display", "none");
    } else {
        $("#IdRegraComissaoReferencia", "#modalNovaRegraComissaoPadrao").val("");
        $("#Regional", "#modalNovaRegraComissaoPadrao").attr("disabled", false);
        $("#Regional", "#modalNovaRegraComissaoPadrao").val($("#Regional option:first", "#modalNovaRegraComissaoPadrao").val());
        $("#opcao_buscar_ultima_wrapper").css("display", "block");
    }
    $("#modalNovaRegraComissaoPadrao").show();
};

Europa.Controllers.ModalNovaRegraComissaoPadrao.Fechar = function () {
    $("#IdRegraComissaoReferencia", "#modalNovaRegraComissaoPadrao").val("");
    $("#Regional", "#modalNovaRegraComissaoPadrao").val($("#Regional option:first", "#modalNovaRegraComissaoPadrao").val());

    $("#modalNovaRegraComissaoPadrao").hide();
};

Europa.Controllers.ModalNovaRegraComissaoPadrao.Confirmar = function () {
    var idRegraReferencia = $("#IdRegraComissaoReferencia", "#modalNovaRegraComissaoPadrao").val();
    var regional = $("#Regional", "#modalNovaRegraComissaoPadrao").val();
    var atualizado;
    if ($("#IdRegraComissaoReferencia", "#modalNovaRegraComissaoPadrao").val() === "") {
        atualizado = $("[name='BuscarUltimaAtt']:checked", "#modalNovaRegraComissaoPadrao").val().toLowerCase() === "true";
    } else {
        atualizado = false;
    }

    Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.Confirmacao, Europa.String.Format(Europa.i18n.Messages.ConfirmacaoInicioCriacaoRegraComissaoPadrao, $("#Regional", "#modalNovaRegraComissaoPadrao").val()));
    Europa.Confirmacao.ConfirmCallback = function () {
        var url = Europa.Controllers.ModalNovaRegraComissaoPadrao.Url.Matriz + "?regional=" + regional;

        url = url + "&create=true";

        if (idRegraReferencia !== "") {
            url = url + "&regra=" + idRegraReferencia;
        }

        if (atualizado) {
            url = url + "&ultimo=" + atualizado;
        }

        location.href = url;
    };
    Europa.Confirmacao.Show();
};