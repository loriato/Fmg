Europa.Controllers.PrePropostaAvalistaAguardandoAnalise = {};
Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.Permissoes = {};
Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.Modal = {};
Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.Modal.AlterarSicaq = {};

$(function () {
    $(".europa-datatable-table,.europa-datatable-table table,.europa-datatable-footer,.europa-datatable-top").css("table-layout", "fixed").css("overflow-x", "auto");
    $(".ng-scope").css("word-wrap", "break-word");

    $("#SituacoesPreProposta").select2({
        trags: true
    });

    Europa.Mask.CpfCnpj("#CpfProponente");
    Europa.Mask.ApplyByClass("dinheiro", Europa.Mask.FORMAT_MONEY, undefined, true);
});

Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.InitDatePicker = function () {
    Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.DataElaboracaoDe = new Europa.Components.DatePicker()
        .WithTarget("#DataElaboracaoDe")
        .WithFormat("DD/MM/YYYY")
        .WithMaxDate(Europa.Date.Now("DD/MM/YYYY"))
        .Configure();

    Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.DataElaboracaoAte = new Europa.Components.DatePicker()
        .WithTarget("#DataElaboracaoAte")
        .WithFormat("DD/MM/YYYY")
        .WithMaxDate(Europa.Date.Now("DD/MM/YYYY"))
        .Configure();
};

Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.Modal.AlterarSicaq.AbrirModal = function (idPreProposta) {
    $.post(Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.UrlBuscarPreProposta, { idPreProposta: idPreProposta }, function (res) {
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

            Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.Modal.AlterarSicaq.DataSicaq = new Europa.Components.DatePicker()
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

Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.Modal.AlterarSicaq.FecharModal = function () {
    $("#Id").val("");
    $("#Codigo").val("");
    $("#Cliente_NomeCompleto").val("");
    $("#StatusSicaq").val("");
    $("#DataSicaq").val("");
    $("#ParcelaAprovada").val("");
    $("#alterar_sicaq_modal").modal("hide");
    Europa.Validator.ClearForm("#form_alterar_sicaq");
};

Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.Modal.AlterarSicaq.Alterar = function () {
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
    
    Europa.Confirmacao.PreAcaoV2(Europa.i18n.Messages.AlterarSicaq, msg, Europa.i18n.Messages.Confirmar, function () { 

        $.post(Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.UrlAlterarSicaq, { model: form }, function (res) {
            if (res.Sucesso) {
                Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.Modal.AlterarSicaq.FecharModal();
                Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.Tabela.reloadData();
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

Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.OnChangeDatePicker = function () {
    Europa.Controllers.PrePropostaAvalistaAguardandoAnalise.DataElaboracaoAte = new Europa.Components.DatePicker()
        .WithTarget("#DataElaboracaoAte")
        .WithFormat("DD/MM/YYYY")
        .WithMinDate($("#DataElaboracaoDe").val())
        .WithMaxDate(Europa.Date.Now("DD/MM/YYYY"))
        .Configure();
}