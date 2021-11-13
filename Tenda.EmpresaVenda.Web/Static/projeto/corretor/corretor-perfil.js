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

Europa.Controllers.Corretor.PreencherPerfil = function (objetoLinhaTabela) {

    var param = {
        NomePerfis: objetoLinhaTabela.Perfis
    };

    $.post(Europa.Controllers.Corretor.UrlPerfisSelecionados, param, function (res) {
        Europa.Controllers.Corretor.AutoCompletePerfilPortal.SetMultipleValues(res.records, "Id", "Nome")
        Europa.Controllers.Corretor.PerfisAtuais = Europa.Controllers.Corretor.AutoCompletePerfilPortal.Value();
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
    var sistema = Europa.Controllers.Corretor.EmpresaVendaPortal;

    var data = {
        idUsuario: $("#Usuario_Id").val(),
        idPerfil: idPerfil,
        idSistema: sistema.Id
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