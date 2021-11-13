Europa.Controllers.MeusResgates = {};
Europa.Controllers.MeusResgates.Tabela = undefined;

$(function () {
    $("#corpo-conteudo-layout").css('padding-left', '64px');
    $("#corpo-conteudo-layout").css('padding-right', '64px');
});

DataTableApp.controller('ResgateDatatable', resgateDatatable);

function resgateDatatable($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.MeusResgates.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    var self = Europa.Controllers.MeusResgates.Tabela;
    self.setColumns([
        DTColumnBuilder.newColumn('Pontuacao').withTitle(Europa.i18n.Messages.PontosSolicitados).withOption('width', '150px').renderWith(renderPontucao),
        DTColumnBuilder.newColumn('DataResgate').withTitle(Europa.i18n.Messages.DataSolicitacao).withOption('width', '150px').renderWith(Europa.Date.toGeenDateFormat),
        DTColumnBuilder.newColumn('Pontuacao').withTitle(Europa.i18n.Messages.PontosLiberados).withOption('width', '150px').renderWith(renderPontuacaoLiberada),
        DTColumnBuilder.newColumn('DataLiberacao').withTitle(Europa.i18n.Messages.DataLiberacao).withOption('width', '150px').renderWith(renderDataLiberacao),
        DTColumnBuilder.newColumn('Voucher').withTitle(Europa.i18n.Messages.VouchersDisponiveis).withOption('width', '150px').renderWith(renderVoucher),
        DTColumnBuilder.newColumn('Voucher').withTitle("").withOption('width', '10px').renderWith(renderCopyVoucher),
        
    ])
    .setIdAreaHeader("resgate_datatable_header")
    .setDefaultOrder([[4, 'asc']])
    .setDefaultOptions('POST', Europa.Controllers.MeusResgates.Url.ListarResgate, Europa.Controllers.MeusResgates.Filtro);

    function renderPontucao(data) {
        if (data == 1) {
            return data + " ponto";
        }

        return data + " pontos";
    }

    function renderPontuacaoLiberada(data, type, full, meta) {
        if (full.SituacaoResgate != 1) {
            return "-";
        }

        return renderPontucao(data);
    }

    function renderDataLiberacao(data, type, full, meta) {
        if (full.SituacaoResgate != 1) {
            return "-";
        }

        return Europa.Date.toGeenDateFormat(data);
    }

    function renderVoucher(data, type, full, meta) {
        if (full.SituacaoResgate != 1) {
            return "-";
        }

        return '<p class="voucher-datatable">'+data+'</p>';
    }

    function renderCopyVoucher(data, type, full, meta) {
        if (full.SituacaoResgate != 1) {
            return "-";
        }

        if (data) {
            var url = location.href.split("ProgramaFidelidade")[0] + "/Static/europa/css/icons/copiar.svg";
            var icon = $('<img/>').addClass('copiar');
            icon.attr('src', url);

            var button = $('<a/>')
                .attr('type', 'button')
                .attr('title', 'Copiar')
                .attr('ng-click', 'copy(' + meta.row + ')')
                .attr('data-clipboard-target', 'Voucher')
                .attr('href','#')
                .append(icon);

            return button.prop('outerHTML');;
        }
        return "";
    }    

    $scope.copy = function (row) {
        var voucher = Europa.Controllers.MeusResgates.Tabela.getRowData(row).Voucher;

        var $temp = $("<input>");
        $("body").append($temp);
        $temp.val(voucher).select();
        document.execCommand("copy");
        $temp.remove();

        var res = {
            Sucesso: true,
            Mensagens: ["Voucher copiado para área de transferência"]
        };

        Europa.Informacao.PosAcao(res);
    }
};

Europa.Controllers.MeusResgates.Filtro = function () {
    var pontuacao = {
        PeriodoDe: $("#PeriodoDe").val(),
        PeriodoAte: $("#PeriodoAte").val(),
        DataSolicitacao: $("#DataSolicitacao")[0].checked,
        DataLiberacao: $("#DataLiberacao")[0].checked,
    };

    return pontuacao;
};

Europa.Controllers.MeusResgates.Filtrar = function () {
    Europa.Controllers.MeusResgates.Tabela.reloadData();
};

Europa.Controllers.MeusResgates.LimparData = function () {
    $("#PeriodoDe").val("");
    $("#PeriodoAte").val("");
};

Europa.Controllers.MeusResgates.OnChangePeriodoAte = function () {
    Europa.Controllers.MeusResgates.PeriodoDe = new Europa.Components.DatePicker()
        .WithTarget("#PeriodoAte")
        .WithFormat("DD/MM/YYYY")
        .WithMinDate($("#PeriodoDe").val())
        .Configure();
};