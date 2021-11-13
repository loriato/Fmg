Europa.Controllers.LogAcao = {};
Europa.Controllers.LogAcao.Modal = {};
Europa.Controllers.LogAcao.Sistema = undefined;

$(document).ready(function () {
    Europa.Controllers.LogAcao.InitAutoCompleteUsuario();
    Europa.Controllers.LogAcao.InitAutoCompleteUnidadeFuncional();
    Europa.Controllers.LogAcao.InitAutoCompleteFuncionalidade();
    Europa.Components.DatePicker.AutoApply();

    $('[name="Sistema"]').on('change', function () {
        Europa.Controllers.LogAcao.AutoCompleteFuncionalidade.Clean();
        Europa.Controllers.LogAcao.AutoCompleteUnidadeFuncional.Clean();
    });

});

Europa.Controllers.LogAcao.LimparFiltros = function() {
    $("#form_filtro_log_acao").find("#DataInicio").val("");
    $("#form_filtro_log_acao").find("#DataFim").val("");
    $("#Sistema").val("");
    Europa.Controllers.LogAcao.AutoCompleteUsuario.Clean();
    Europa.Controllers.LogAcao.AutoCompleteUnidadeFuncional.Clean();
    Europa.Controllers.LogAcao.AutoCompleteFuncionalidade.Clean();
};


Europa.Controllers.LogAcao.InitAutoCompleteUsuario = function() {
    Europa.Controllers.LogAcao.AutoCompleteUsuario = new Europa.Components.AutoCompleteUsuario()
        .WithTargetSuffix("usuario_log_acao")
        .Configure();
};

Europa.Controllers.LogAcao.InitAutoCompleteUnidadeFuncional = function() {
    Europa.Controllers.LogAcao.AutoCompleteUnidadeFuncional = new Europa.Components.AutoCompleteUnidadeFuncional()
        .WithTargetSuffix("unidade_funcional_log_acao");

    $('[name="IdUnidadeFuncional"]').on('change',
        function() {
            Europa.Controllers.LogAcao.AutoCompleteFuncionalidade.Clean();
        });

    Europa.Controllers.LogAcao.AutoCompleteUnidadeFuncional.Data = function(params) {
        return {
            start: 0,
            pageSize: 10,
            filter: [
                {
                    value: params.term,
                    column: this.param,
                    regex: true
                },
                {
                    value: function() { return $("#Sistema").val(); },
                    column: 'Sistema'
                }
            ],
            order: [
                {
                    value: "asc",
                    column: this.param
                }
            ]
        };
    };
    Europa.Controllers.LogAcao.AutoCompleteUnidadeFuncional.ProcessResult = function(data) {
        var formattedResult = [];
        data.records.forEach(function(element) {
            formattedResult.push({ id: element.Id, text: element.Modulo.Sistema.Nome + " - " + element.Nome });
        });
        return {
            results: formattedResult
        };
    };
    Europa.Controllers.LogAcao.AutoCompleteUnidadeFuncional.Configure();
};


Europa.Controllers.LogAcao.InitAutoCompleteFuncionalidade = function() {
    Europa.Controllers.LogAcao.AutoCompleteFuncionalidade = new Europa.Components.AutoCompleteFuncionalidade()
        .WithTargetSuffix("funcionalidade_log_acao");

    Europa.Controllers.LogAcao.AutoCompleteFuncionalidade.Data = function(params) {
        return {
            start: 0,
            pageSize: 10,
            filter: [
                {
                    value: params.term,
                    column: this.param,
                    regex: true
                },
                {
                    value: function() { return Europa.Controllers.LogAcao.AutoCompleteUnidadeFuncional.Value(); },
                    column: 'IdUnidadeFuncional'
                }
            ],
            order: [
                {
                    value: "asc",
                    column: this.param
                }
            ]
        };
    };
    Europa.Controllers.LogAcao.AutoCompleteFuncionalidade.ProcessResult = function(data) {
        var formattedResult = [];
        data.records.forEach(function(element) {
            formattedResult.push({ id: element.Id, text: element.Nome });
        });
        return {
            results: formattedResult
        };
    };
    Europa.Controllers.LogAcao.AutoCompleteFuncionalidade.Configure();
};



Europa.Controllers.LogAcao.Modal.AbrirModal = function (logAcao) {
    Europa.Controllers.LogAcao.Modal.PreencherCampos(logAcao);
    $("#detalhamentoLogAcao").modal("show");
};

Europa.Controllers.LogAcao.Modal.CloseModal = function () {
    $("#detalhamentoLogAcao").modal("hide");
};


Europa.Controllers.LogAcao.Modal.PreencherCampos = function (logAcao) {
    $("#form_detalhar_log_acao").find("#Entidade").val(logAcao.Entidade);
    $("#form_detalhar_log_acao").find("#ChavePrimaria").val(logAcao.ChavePrimaria);
    $("#form_detalhar_log_acao").find("#NomeUsuarioCriacao").val(logAcao.NomeUsuarioCriacao);
    $("#form_detalhar_log_acao").find("#NomeUsuarioAtualizacao").val(logAcao.NomeUsuarioAtualizacao);
    var temp = logAcao.Conteudo;
    temp = JSON.stringify(JSON.parse(temp), null, 4);
    temp = Europa.Controllers.LogAcao.Modal.syntaxHighlight(temp);
    $("#form_detalhar_log_acao").find("#Conteudo").html(temp);
};

Europa.Controllers.LogAcao.Modal.CopyToClipboard = function () {
    var t = document.createElement('textarea');
    t.id = 't';
    t.style.height = 0;
    document.body.appendChild(t);
    t.value = document.getElementById('Conteudo').innerText;
    document.querySelector('#t').select();
    document.execCommand('copy');
    document.body.removeChild(t);
};


Europa.Controllers.LogAcao.Modal.syntaxHighlight = function (json) {
    json = json.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
    return json
        .replace(/("(\\u[a-zA-Z0-9]{4}|\\[^u]|[^\\"])*"(\s*:)?|\b(true|false|null)\b|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?)/g,
            function (match) {
                var cls = 'number';
                if (/^"/.test(match)) {
                    if (/:$/.test(match)) {
                        cls = 'key';
                    } else {
                        cls = 'string';
                    }
                } else if (/true|false/.test(match)) {
                    cls = 'boolean';
                } else if (/null/.test(match)) {
                    cls = 'null';
                }
                return '<span class="' + cls + '">' + match + '</span>';
            });
};