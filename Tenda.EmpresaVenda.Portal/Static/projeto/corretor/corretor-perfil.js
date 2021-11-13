Europa.Controllers.Corretor.AutoCompletePerfilPortal = undefined;
Europa.Controllers.Corretor.EmpresaVendaPortal = undefined;
Europa.Controllers.Corretor.PerfisAtuais = undefined;

$(function () {
    Europa.Controllers.Corretor.initAutocomplete();
    Europa.Controllers.Corretor.BuscaEmpresaVendaPortal();
});

Europa.Controllers.Corretor.initAutocomplete = function () {
    Europa.Controllers.Corretor.AutoCompletePerfilPortal = new Europa.Components.AutoCompletePerfilPortal()
        .WithTargetSuffix("perfil")
        .Configure();
}

Europa.Controllers.Corretor.PreencherPerfil = function (idCorretor) {
    $.get(Europa.Controllers.Corretor.UrlBuscarCorretorView, { idCorretor: idCorretor }, function (res) {
        if (res.Sucesso) {
            var param = {
                NomePerfis: res.Objeto.Perfis
            };

            $.post(Europa.Controllers.Corretor.UrlPerfisSelecionados, param, function (res) {
                Europa.Controllers.Corretor.AutoCompletePerfilPortal.SetMultipleValues(res.records, "Id", "Nome")
                Europa.Controllers.Corretor.PerfisAtuais = Europa.Controllers.Corretor.AutoCompletePerfilPortal.Value();
            });
        } else {
            Europa.Informacao.PosAcao(res);
        }
    });
};

Europa.Controllers.Corretor.ActionAutocompletePerfil = function () {

    var idUsuario = $("#Usuario_Id").val();
    if (idUsuario > 0) {
        $.each(Europa.Controllers.Corretor.AutoCompletePerfilPortal.Value(), function (idx, obj) {
            var existe = Europa.Controllers.Corretor.PerfisAtuais.includes(obj);
            if (!existe) {
                Europa.Controllers.Corretor.IncluirPerfil(obj);
            }
        });

        $.each(Europa.Controllers.Corretor.PerfisAtuais, function (idx, obj) {
            var existe = Europa.Controllers.Corretor.AutoCompletePerfilPortal.Value().includes(obj);
            if (!existe) {
                Europa.Controllers.Corretor.RemoverPerfil(obj);
            }
        });
    }
    Europa.Controllers.Corretor.PerfisAtuais = Europa.Controllers.Corretor.AutoCompletePerfilPortal.Value();
};

Europa.Controllers.Corretor.BuscaEmpresaVendaPortal = function () {
    $.get(Europa.Controllers.Corretor.UrlSistemaEmpresaVendaPortal, function (res) {
        if (res.Sucesso) {
            Europa.Controllers.Corretor.EmpresaVendaPortal = res.Objeto;
        }
    });
}

Europa.Controllers.Corretor.IncluirPerfil = function (idPerfil) {

    var data = {
        idUsuario: $("#Usuario_Id").val(),
        idPerfil: idPerfil,
        idSistema: Europa.Controllers.Corretor.EmpresaVendaPortal.Id
    };

    $.post(Europa.Controllers.Corretor.UrlIncluirPerfil,
        data,
        function (res) {
            if (res.Sucesso) {
                Europa.Controllers.Corretor.FiltrarTabela();
            }
            Europa.Messages.ShowMessages(res);
        });
};

Europa.Controllers.Corretor.RemoverPerfil = function (idPerfil) {
    var sistema = Europa.Controllers.Corretor.EmpresaVendaPortal;

    var data = {
        idUsuario: $("#Usuario_Id").val(),
        idPerfil: idPerfil,
        idSistema: sistema.Id
    };

    $.post(Europa.Controllers.Corretor.UrlExcluirPerfil,
        data,
        function (res) {
            if (res.Sucesso) {
                Europa.Controllers.Corretor.FiltrarTabela();
            } else {
                $.post(Europa.Controllers.Corretor.UrlBuscarPerfil, { id: idPerfil }, function (perfil) {
                    Europa.Controllers.Corretor.AutoCompletePerfilPortal.SetMultipleValues([perfil], "Id", "Nome")
                })
            }
            Europa.Messages.ShowMessages(res);
        });
};






//Europa.Controllers.Corretor.GetPerfis = undefined;
//Europa.Controllers.Corretor.Datatable = undefined;
//Europa.Controllers.Corretor.Perfis = [];

//$(function () {

//});

//Europa.Controllers.Corretor.createDT = function ($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
//    var dataTableWrapper = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
//    Europa.Controllers.Corretor.Datatable = dataTableWrapper;

//    dataTableWrapper.setIdAreaHeader("usuario_datable_header_perfil")
//        .setColumns([
//            DTColumnBuilder.newColumn('Nome').withTitle(Europa.i18n.Messages.Nome).withOption('width', '50%'),
//            DTColumnBuilder.newColumn('CriadoEm').withTitle(Europa.i18n.Messages.DataCriacao).renderWith(dataTableWrapper.renderDateSmall).withOption('width', '30%'),
//            DTColumnBuilder.newColumn('Situacao').withTitle(Europa.i18n.Messages.Situacao).renderWith(situacaoRender)
//        ])
//        .setColActions(actionsHtml, '50px')
//        .setAutoInit(false)
//        .setOptionsSelect('POST', Europa.Controllers.Corretor.GetPerfis, Europa.Controllers.Corretor.Filter);

//    $scope.excluir = function (row) {
//        var idUsuario = $(Europa.Controllers.Corretor.IdFormularioCorretor).find(":input#Usuario_Id").val();

//        if (idUsuario > 0) {
//            var data = {
//                idUsuario: $(Europa.Controllers.Corretor.IdFormularioCorretor).find(":input#Usuario_Id").val(),
//                idPerfil: row,
//                idSistema: 3
//            };

//            $.post(Europa.Controllers.Corretor.UrlExcluirPerfil,
//                data,
//                function (res) {
//                    if (res.Sucesso) {
//                        Europa.Controllers.Corretor.Datatable.reloadData();
//                        Europa.Controllers.Corretor.FiltrarTabela();
//                    }
//                    Europa.Messages.ShowMessages(res);
//                });
//        } else {
//            var index = Europa.Controllers.Corretor.Perfis.indexOf(row);
//            if (index > -1) {
//                Europa.Controllers.Corretor.Perfis.splice(index, 1);
//            };
//            Europa.Controllers.Corretor.Datatable.reloadData();
//        }
//    };

//    function actionsHtml(data, type, full, meta) {
//        var button = '<div>';
//        if (Europa.Controllers.Corretor.ModoEdicao) {
//            button = button + dataTableWrapper.renderButton(Europa.Controllers.Corretor.PermExcluir, Europa.i18n.Messages.Excluir, "fa fa-trash", "excluir(" + full.Id + ")");
//        }
//        button = button + '</div>';

//        return button;
//    }
//    function situacaoRender(data, type, full, meta) {
//        return Europa.i18n.Enum.Resolve("Situacao", data);
//    }
//};

//DataTableApp.controller('PerfisDataTable', Europa.Controllers.Corretor.createDT);

//Europa.Controllers.Corretor.Filter = function () {

//    var data = {
//        idUsuario: $(Europa.Controllers.Corretor.IdFormularioCorretor).find(":input#Usuario_Id").val(),
//        idSistema: 3,
//        Perfis: Europa.Controllers.Corretor.Perfis
//    };

//    return data;
//};

//Europa.Controllers.Corretor.AbrirModalPerfil = function () {
//    Europa.Components.ModalPerfil
//        .WithSelectFuncion(Europa.Controllers.Corretor.ActionSelecionarPerfil)
//        .Show();
//};

//Europa.Controllers.Corretor.ActionSelecionarPerfil = function (perfil) {

//    var idUsuario = $(Europa.Controllers.Corretor.IdFormularioCorretor).find(":input#Usuario_Id").val();

//    if (idUsuario > 0) {
        
//        var data = {
//            idUsuario: $(Europa.Controllers.Corretor.IdFormularioCorretor).find(":input#Usuario_Id").val(),
//            idPerfil: perfil.Id,
//            idSistema: 3
//        };
//        $.post(Europa.Controllers.Corretor.UrlIncluirPerfil,
//            data,
//            function (res) {
//                if (res.Sucesso) {
//                    Europa.Controllers.Corretor.Datatable.reloadData();
//                    Europa.Controllers.Corretor.FiltrarTabela();
//                }
//                Europa.Messages.ShowMessages(res);
//            });
//    } else {
//        var tam = Europa.Controllers.Corretor.Perfis.length;
//        Europa.Controllers.Corretor.Perfis[tam] = perfil.Id;
//        Europa.Controllers.Corretor.Datatable.reloadData();
//    }

//};