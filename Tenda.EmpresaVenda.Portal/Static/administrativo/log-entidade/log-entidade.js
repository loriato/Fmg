Europa.Controllers.LogEntidade = {};
Europa.Controllers.LogEntidade.Modal = {};

$(document).ready(function () {
    Europa.Controllers.LogEntidade.InitAutoCompleteUsuarios();
});


Europa.Controllers.LogEntidade.LimparFiltros = function() {
    $("#Entidade").val("");
    $("#ChavePrimariaFiltro").val("");
    $("#DataInicio").val("");
    $("#DataFim").val("");
    Europa.Controllers.LogEntidade.AutoCompleteUsuarioCriador.Clean();
    Europa.Controllers.LogEntidade.AutoCompleteUsuarioAtualizacao.Clean();
};


Europa.Controllers.LogEntidade.InitAutoCompleteUsuarios = function() {
    Europa.Controllers.LogEntidade.AutoCompleteUsuarioCriador = new Europa.Components.AutoCompleteUsuario()
        .WithTargetSuffix("usuario_criador_log_entidade")
        .Configure();

    Europa.Controllers.LogEntidade.AutoCompleteUsuarioAtualizacao = new Europa.Components.AutoCompleteUsuario()
        .WithTargetSuffix("usuario_atualizacao_log_entidade")
        .Configure();
};

Europa.Controllers.LogEntidade.Modal.AbrirModal = function(logEntidade) {
    Europa.Controllers.LogEntidade.Modal.PreencherCampos(logEntidade);
    $("#detalhamentoLogEntidade").modal("show");
};

Europa.Controllers.LogEntidade.Modal.CloseModal = function() {
    $("#detalhamentoLogEntidade").modal("hide");
};


Europa.Controllers.LogEntidade.Modal.PreencherCampos = function (logEntidade) {
    $("#form_detalhar_log_entidade").find("#Entidade").val(logEntidade.Entidade);
    $("#form_detalhar_log_entidade").find("#ChavePrimaria").val(logEntidade.ChavePrimaria);
    $("#form_detalhar_log_entidade").find("#NomeUsuarioCriacao").val(logEntidade.NomeUsuarioCriacao);
    $("#form_detalhar_log_entidade").find("#NomeUsuarioAtualizacao").val(logEntidade.NomeUsuarioAtualizacao);
    var temp = logEntidade.Conteudo;
    temp = JSON.stringify(JSON.parse(temp), null, 4);
    temp = Europa.Controllers.LogEntidade.Modal.syntaxHighlight(temp);
    $("#form_detalhar_log_entidade").find("#Conteudo").html(temp);
};

Europa.Controllers.LogEntidade.Modal.CopyToClipboard = function() {
    var t = document.createElement('textarea');
    t.id = 't';
    t.style.height = 0;
    document.body.appendChild(t);
    t.value = document.getElementById('Conteudo').innerText;
    document.querySelector('#t').select();
    document.execCommand('copy');
    document.body.removeChild(t);
};


Europa.Controllers.LogEntidade.Modal.syntaxHighlight = function(json) {
    json = json.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;');
    return json
        .replace(/("(\\u[a-zA-Z0-9]{4}|\\[^u]|[^\\"])*"(\s*:)?|\b(true|false|null)\b|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?)/g,
            function(match) {
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