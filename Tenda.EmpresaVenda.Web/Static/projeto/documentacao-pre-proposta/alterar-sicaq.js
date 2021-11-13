Europa.Controllers.DocumentacaoPreProposta.AlterarSicaq.SicaqPrevio = false;

$(function () {
});

Europa.Controllers.DocumentacaoPreProposta.AlterarSicaq.AbrirModal = function (idPreProposta) {

    Europa.Controllers.DocumentacaoPreProposta.AlterarSicaq.SicaqPrevio = false;
    $("#myModalLabel").html(Europa.i18n.Messages.AlterarSicaq);
    $("#StatusSicaq").prop("disabled", false);

    $.post(Europa.Controllers.DocumentacaoPreProposta.UrlBuscarPreProposta, { idPreProposta: idPreProposta }, function (res) {
        if (res.Sucesso && res.Objeto) {
            var preProposta = res.Objeto;
            $("#Id").val(preProposta.Id);
            $("#CodigoPreProposta").val(preProposta.Codigo);
            $("#Cliente_NomeCompleto").val(preProposta.Cliente.NomeCompleto);
            $("#StatusSicaq").val(preProposta.StatusSicaq);
            //$("#FaixaUmMeio option[value='" + preProposta.FaixaUmMeio + "']").prop('selected', true);
            $("#DataSicaq").val(preProposta.DataSicaq);
            $("#ParcelaAprovada").val(preProposta.ParcelaAprovada);
            //console.log(preProposta.StatusSicaq)
            if (preProposta.FaixaUmMeio != null && preProposta.StatusSicaq!=0) {
                $("#FaixaUmMeio option[value='" + preProposta.FaixaUmMeio + "']").prop('selected', true);
            }

            $("#FaixaEv option[value='" + preProposta.FaixaEv + "']").prop('selected', true)
            //console.log(preProposta.FaixaEv)
            var date = "";
            if (preProposta.DataSicaq) {
                var str = preProposta.DataSicaq.replace(/\D/g, "");
                date = new Date(parseInt(str));
            }

            Europa.Controllers.DocumentacaoPreProposta.AlterarSicaq.DataSicaq = new Europa.Components.DatePicker()
                .WithTarget("#DataSicaq")
                .WithParentEl("#alterar_sicaq_modal")
                .WithTimePicker()
                .WithTimePicker24h()
                .WithValue(date)
                .WithFormat(Europa.Date.FORMAT_DATE_HOUR)
                .WithMaxDate(Europa.Date.Format(new Date(), Europa.Date.FORMAT_DATE_HOUR))
                .Configure();

            $("#alterar_sicaq_modal").modal("show");
        }
    });
};

Europa.Controllers.DocumentacaoPreProposta.AlterarSicaq.AbrirModalPrevio = function (idPreProposta) {

    Europa.Controllers.DocumentacaoPreProposta.AlterarSicaq.SicaqPrevio = true;
    $("#myModalLabel").html(Europa.i18n.Messages.AlterarSicaqPrevio);

    $.post(Europa.Controllers.DocumentacaoPreProposta.UrlBuscarPreProposta, { idPreProposta: idPreProposta }, function (res) {
        if (res.Sucesso && res.Objeto) {
            var preProposta = res.Objeto;
            console.log(preProposta)
            $("#Id").val(preProposta.Id);
            $("#CodigoPreProposta").val(preProposta.Codigo);
            $("#Cliente_NomeCompleto").val(preProposta.Cliente.NomeCompleto);
            //$("#StatusSicaq").prop("disabled", true)
            //$("#StatusSicaq").val(1);

            if (preProposta.FaixaUmMeioPrevio != null) {
                $("#FaixaUmMeio option[value='" + preProposta.FaixaUmMeioPrevio + "']").prop('selected', true);
            }

            $("#DataSicaq").val(preProposta.DataSicaqPrevio);
            $("#ParcelaAprovada").val(preProposta.ParcelaAprovadaPrevio);

            $("#FaixaEv option[value='" + preProposta.FaixaEv + "']").prop('selected', true)
            //console.log(preProposta.FaixaEv)
            var date = "";
            if (preProposta.DataSicaqPrevio) {
                var str = preProposta.DataSicaqPrevio.replace(/\D/g, "");
                date = new Date(parseInt(str));
            }

            Europa.Controllers.DocumentacaoPreProposta.AlterarSicaq.DataSicaq = new Europa.Components.DatePicker()
                .WithTarget("#DataSicaq")
                .WithParentEl("#alterar_sicaq_modal")
                .WithTimePicker()
                .WithTimePicker24h()
                .WithValue(date)
                .WithFormat(Europa.Date.FORMAT_DATE_HOUR)
                .WithMaxDate(Europa.Date.Format(new Date(), Europa.Date.FORMAT_DATE_HOUR))
                .Configure();

            $("#alterar_sicaq_modal").modal("show");
        }
    });
};

Europa.Controllers.DocumentacaoPreProposta.AlterarSicaq.FecharModal = function () {
    $("#Id").val("");
    $("#Codigo").val("");
    $("#Cliente_NomeCompleto").val("");
    $("#StatusSicaq").val("");
    $("#DataSicaq").val("");
    $("#ParcelaAprovada").val("");
    $("#alterar_sicaq_modal").modal("hide");
    Europa.Validator.ClearForm("#form_alterar_sicaq");    
};

Europa.Controllers.DocumentacaoPreProposta.AlterarSicaq.Alterar = function () {
    var form = Europa.Form.SerializeJson("#form_alterar_sicaq");
    if (form["ParcelaAprovada"] !== undefined) {
        form["ParcelaAprovada"] = form["ParcelaAprovada"].replace(/\./g, "");
    }
    form.Id = $("#Id").val();
    var statusSicaq = $("#StatusSicaq").val();
    var msg = "";

    if (statusSicaq == 5) {
        msg = Europa.i18n.Messages.MsgReprovarPreProposta;
    }

    var url = Europa.Controllers.DocumentacaoPreProposta.AlterarSicaq.SicaqPrevio ? Europa.Controllers.DocumentacaoPreProposta.UrlAlterarSicaqPrevio : Europa.Controllers.DocumentacaoPreProposta.UrlAlterarSicaq;

    Europa.Confirmacao.PreAcaoV2(Europa.i18n.Messages.AlterarSicaq, msg, Europa.i18n.Messages.Confirmar, function () {

        $.post(url, { model: form }, function (res) {
            if (res.Sucesso) {
                Europa.Controllers.DocumentacaoPreProposta.AlterarSicaq.FecharModal();    
                sleep(2000).then(() => { location.reload(); });
            } else {
                Europa.Validator.InvalidateList(res.Campos, "#form_alterar_sicaq");
                //Por algum motivo, está invalidando data sicaq se status sicaq estiver vazio.
                if (($.inArray("DataSicaq", res.Campos)) == -1) {
                    $("#DataSicaq").closest('.form-group').removeClass('has-error');
                };
            }
            Europa.Informacao.PosAcao(res);             
        });        
    });
};

function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}