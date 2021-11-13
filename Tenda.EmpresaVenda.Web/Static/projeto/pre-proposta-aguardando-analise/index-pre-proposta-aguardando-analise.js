Europa.Controllers.PrePropostaAguardandoAnalise = {};
Europa.Controllers.PrePropostaAguardandoAnalise.Permissoes = {};
Europa.Controllers.PrePropostaAguardandoAnalise.Modal = {};
Europa.Controllers.PrePropostaAguardandoAnalise.Modal.AlterarSicaq = {};
Europa.Controllers.PrePropostaAguardandoAnalise.Modal.AlterarSicaq.SicaqPrevio = false;
Europa.Controllers.GrupoCCA = {};

$(function () {
    $(".europa-datatable-table,.europa-datatable-table table,.europa-datatable-footer,.europa-datatable-top").css("table-layout", "fixed").css("overflow-x", "auto");
    $(".ng-scope").css("word-wrap", "break-word");


    Europa.Controllers.PrePropostaAguardandoAnalise.AutoCompleteRegionais = new Europa.Components.AutoCompleteRegionais()
        .WithTargetSuffix("regional")
        .Configure();

    $("#SituacoesPreProposta").select2({
        trags: true
    });
    
    Europa.Mask.CpfCnpj("#CpfProponente");
    Europa.Mask.ApplyByClass("dinheiro", Europa.Mask.FORMAT_MONEY, undefined, true);
});

Europa.Controllers.GrupoCCA.FiltroGrupoCCA = function () {
    var param = {
        Descricao: $("#select-novo-cca").val()
    };
    return param;
};

Europa.Controllers.PrePropostaAguardandoAnalise.InitDatePicker = function () {
    Europa.Controllers.PrePropostaAguardandoAnalise.DataElaboracaoDe = new Europa.Components.DatePicker()
        .WithTarget("#DataElaboracaoDe")
        .WithFormat("DD/MM/YYYY")
        .WithMaxDate(Europa.Date.Now("DD/MM/YYYY"))
        .Configure();

    Europa.Controllers.PrePropostaAguardandoAnalise.DataElaboracaoAte = new Europa.Components.DatePicker()
        .WithTarget("#DataElaboracaoAte")
        .WithFormat("DD/MM/YYYY")
        .WithMaxDate(Europa.Date.Now("DD/MM/YYYY"))
        .Configure();
};

Europa.Controllers.PrePropostaAguardandoAnalise.Modal.AlterarSicaq.AbrirModal = function (idPreProposta) {

    Europa.Controllers.PrePropostaAguardandoAnalise.Modal.AlterarSicaq.SicaqPrevio = false;
    $("#myModalLabel").html(Europa.i18n.Messages.AlterarSicaq);
    $("#StatusSicaq").prop("disabled", false);
    $("#label-sicaq-previo").addClass("hidden");
    $("#label-sicaq-definitivo").removeClass("hidden");

    $.post(Europa.Controllers.PrePropostaAguardandoAnalise.UrlBuscarPreProposta, { idPreProposta: idPreProposta }, function (res) {
        if (res.Sucesso && res.Objeto) {
            var preProposta = res.Objeto;
            $("#Id").val(preProposta.Id);
            $("#Codigo").val(preProposta.Codigo);
            $("#Cliente_NomeCompleto").val(preProposta.Cliente.NomeCompleto);
            $("#StatusSicaq").val(preProposta.StatusSicaq);
            $("#FaixaUmMeio option[value='" + preProposta.FaixaUmMeio + "']").prop('selected', true);
            $("#DataSicaq").val(preProposta.DataSicaq);
            $("#ParcelaAprovada").val(preProposta.ParcelaAprovada);

            $("#FaixaEv option[value='" + preProposta.FaixaEv + "']").prop('selected', true)

            var date = "";
            if (preProposta.DataSicaq) {
                var str = preProposta.DataSicaq.replace(/\D/g, "");
                date = new Date(parseInt(str));
            }

            Europa.Controllers.PrePropostaAguardandoAnalise.Modal.AlterarSicaq.DataSicaq = new Europa.Components.DatePicker()
                .WithTarget("#DataSicaq")
                .WithParentEl("#alterar_sicaq_modal")
                .WithTimePicker()
                .WithTimePicker24h()
                .WithValue(date)
                .WithFormat(Europa.Date.FORMAT_DATE_HOUR)
                .WithMaxDate(Europa.Date.Format(new Date(), Europa.Date.FORMAT_DATE_HOUR))
                .Configure();

            $("#alterar_sicaq_modal").modal("show");

            //SICAQ PRÉVIO
            $("#StatusSicaqPrevio").val(preProposta.StatusSicaqPrevio);
            $("#ParcelaAprovadaPrevio").val(preProposta.ParcelaAprovadaPrevio);

            if (preProposta.DataSicaqPrevio) {
                var datePrevio = Europa.Date.Format(preProposta.DataSicaqPrevio, Europa.Date.FORMAT_DATE_HOUR);                
                $("#DataSicaqPrevio").val(datePrevio);
            }

            if (preProposta.FaixaUmMeioPrevio != null) {
                $("#FaixaUmMeioPrevio option[value='" + preProposta.FaixaUmMeioPrevio + "']").prop('selected', true);
            }
                                    
        }
    });
};

Europa.Controllers.PrePropostaAguardandoAnalise.Modal.AlterarSicaq.AbrirModalPrevio = function (idPreProposta) {

    Europa.Controllers.PrePropostaAguardandoAnalise.Modal.AlterarSicaq.SicaqPrevio = true;
    $("#myModalLabel").html(Europa.i18n.Messages.AlterarSicaqPrevio);
    $("#label-sicaq-definitivo").addClass("hidden");
    $("#label-sicaq-previo").removeClass("hidden");

    $.post(Europa.Controllers.PrePropostaAguardandoAnalise.UrlBuscarPreProposta, { idPreProposta: idPreProposta }, function (res) {
        if (res.Sucesso && res.Objeto) {
            var preProposta = res.Objeto;
            //console.log(preProposta)
            $("#Id").val(preProposta.Id);
            $("#Codigo").val(preProposta.Codigo);
            $("#Cliente_NomeCompleto").val(preProposta.Cliente.NomeCompleto);
            //$("#StatusSicaq").prop("disabled", true)
            //$("#StatusSicaq").val(1);

            if (preProposta.FaixaUmMeioPrevio != null) {
                $("#FaixaUmMeio option[value='" + preProposta.FaixaUmMeioPrevio + "']").prop('selected', true);
            }

            $("#DataSicaq").val(preProposta.DataSicaqPrevio);
            $("#ParcelaAprovada").val(preProposta.ParcelaAprovadaPrevio);

            $("#FaixaEv option[value='" + preProposta.FaixaEv + "']").prop('selected', true);

            var date = "";
            if (preProposta.DataSicaqPrevio) {
                var str = preProposta.DataSicaqPrevio.replace(/\D/g, "");
                date = new Date(parseInt(str));
            }

            Europa.Controllers.PrePropostaAguardandoAnalise.Modal.AlterarSicaq.DataSicaq = new Europa.Components.DatePicker()
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

Europa.Controllers.PrePropostaAguardandoAnalise.Modal.AlterarSicaq.FecharModal = function () {
    $("#Id").val("");
    $("#Codigo").val("");
    $("#Cliente_NomeCompleto").val("");
    $("#StatusSicaq").val("");
    $("#DataSicaq").val("");
    $("#ParcelaAprovada").val("");
    $("#alterar_sicaq_modal").modal("hide");
    Europa.Validator.ClearForm("#form_alterar_sicaq");
};

Europa.Controllers.PrePropostaAguardandoAnalise.Modal.AlterarSicaq.Alterar = function () {
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

    var url = Europa.Controllers.PrePropostaAguardandoAnalise.Modal.AlterarSicaq.SicaqPrevio ?
        Europa.Controllers.PrePropostaAguardandoAnalise.UrlAlterarSicaqPrevio :
        Europa.Controllers.PrePropostaAguardandoAnalise.UrlAlterarSicaq;


    Europa.Confirmacao.PreAcaoV2(Europa.i18n.Messages.AlterarSicaq, msg, Europa.i18n.Messages.Confirmar, function () { 

        $.post(url, { model: form }, function (res) {
            if (res.Sucesso) {
                Europa.Controllers.PrePropostaAguardandoAnalise.Modal.AlterarSicaq.FecharModal();
                Europa.Controllers.PrePropostaAguardandoAnalise.Tabela.reloadData();
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

Europa.Controllers.PrePropostaAguardandoAnalise.OnChangeDatePicker = function () {
    Europa.Controllers.PrePropostaAguardandoAnalise.DataElaboracaoAte = new Europa.Components.DatePicker()
        .WithTarget("#DataElaboracaoAte")
        .WithFormat("DD/MM/YYYY")
        .WithMinDate($("#DataElaboracaoDe").val())
        .WithMaxDate(Europa.Date.Now("DD/MM/YYYY"))
        .Configure();
}
