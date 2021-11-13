Europa.Controllers.PontuacaoFidelidade.TreeViewEmpreendimento = undefined;
Europa.Controllers.PontuacaoFidelidade.TreeViewEmpresaVenda = undefined;
Europa.Controllers.PontuacaoFidelidade.SelecionarTodosEmpreendimentos = false;
Europa.Controllers.PontuacaoFidelidade.SelecionarTodasEvs = false;

$(function () {
    //Empreendimentos
    Europa.Controllers.PontuacaoFidelidade.TreeViewEmpreendimento = new Europa.Components.TreeView("empreendimento_treeview");
    //Empresa de Vendas
    Europa.Controllers.PontuacaoFidelidade.TreeViewEmpresaVenda = new Europa.Components.TreeView("empresa_venda_treeview");
});

Europa.Controllers.PontuacaoFidelidade.AbrirNovaPontuacaoFidelidade = function (pontuacaoFidelidade) {    
    if (pontuacaoFidelidade !== undefined) {
        $("#IdPontuacaoFidelidadeReferencia", "#modalNovaPontuacaoFidelidade").val(pontuacaoFidelidade.Id);
        $("#Regional", "#modalNovaPontuacaoFidelidade").val(regraComissao.Regional);
        $("#Regional", "#modalNovaPontuacaoFidelidade").attr("disabled", true);
        $("#opcao_buscar_ultima_wrapper").css("display", "none");
    } else {
        $("#IdPontuacaoFidelidadeReferencia", "#modalNovaPontuacaoFidelidade").val("");
        $("#Regional", "#modalNovaPontuacaoFidelidade").attr("disabled", false);
        $("#Regional", "#modalNovaPontuacaoFidelidade").val($("#Regional option:first", "#modalNovaPontuacaoFidelidade").val());
        $("#opcao_buscar_ultima_wrapper").css("display", "block");
    }
    //Empresa de Vendas
    Europa.Controllers.PontuacaoFidelidade.AtualizarTreeViewEmpresaVenda();

    //Empreendimento
    Europa.Controllers.PontuacaoFidelidade.AtualizarTreeViewEmpreendimento();

    $("#modalNovaPontuacaoFidelidade").show();
};

Europa.Controllers.PontuacaoFidelidade.FecharNovaPontuacaoFidelidade = function () {
    $("#IdPontuacaoFidelidadeReferencia", "#modalNovaPontuacaoFidelidade").val("");
    $("#Regional", "#modalNovaPontuacaoFidelidade").val($("#Regional option:first", "#modalNovaPontuacaoFidelidade").val());

    //Empresa de Vendas
    Europa.Controllers.PontuacaoFidelidade.AtualizarTreeViewEmpresaVenda();

    //Empreendimento
    Europa.Controllers.PontuacaoFidelidade.AtualizarTreeViewEmpreendimento();

    $("#modalNovaPontuacaoFidelidade").hide();
};

Europa.Controllers.PontuacaoFidelidade.Confirmar = function () {
    //Empresa de vendas
    var evsSelecionadas = [];
    if (!Europa.Controllers.PontuacaoFidelidade.SelecionarTodasEvs) {
        evsSelecionadas = Europa.Controllers.PontuacaoFidelidade.TreeViewEmpresaVenda.GetCheckedNodes();

        if (evsSelecionadas.length == 0) {
            Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Informacao, "É necessário selecionar no mínimo uma Empresa de Vendas para executar esta ação.");
            Europa.Informacao.Show();
            return;
        }
    } 

    console.log(evsSelecionadas)

    //Empreendiemntos
    var empreendimentosSelecionados = [];
    if (!Europa.Controllers.PontuacaoFidelidade.SelecionarTodosEmpreendimentos) {
        empreendimentosSelecionados = Europa.Controllers.PontuacaoFidelidade.TreeViewEmpreendimento.GetCheckedNodes();

        if (empreendimentosSelecionados.length == 0) {
            Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Informacao, "É necessário selecionar no mínimo um Empreendimento para executar esta ação.");
            Europa.Informacao.Show();
            return;
        }
    }    

    var progressao = $("#progressao").val();
    var tipoCampanhaFidelidade = $("#TipoCampanhaFidelidade").val();

    if (tipoCampanhaFidelidade > 1 && progressao < 1) {
        Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Informacao, 'Progressão mínima "1" ');
        Europa.Informacao.Show();
        return;
    }

    for (var i = 0; i < progressao; i++) {
        var qtdm = $("#minimo-" + i).val();        
        if (qtdm == 0) {
            Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Informacao, 'Quantidade mínima ' + (i + 1) + ' inválida');
            Europa.Informacao.Show();
            return;
        }
    }

    var idPontuacaoFidelidadeReferencia = $("#IdPontuacaoFidelidadeReferencia", "#modalNovaPontuacaoFidelidade").val();

    var regional = $("#Regional", "#modalNovaPontuacaoFidelidade").val();

    var atualizado;
    if ($("#IdPontuacaoFidelidadeReferencia", "#modalNovaPontuacaoFidelidade").val() === "") {
        atualizado = $("[name='BuscarUltimaAtt']:checked", "#modalNovaPontuacaoFidelidade").val().toLowerCase() === "true";
    } else {
        atualizado = false;
    }

    Europa.Confirmacao.ChangeHeaderAndContent(Europa.i18n.Messages.Confirmacao, Europa.String.Format(Europa.i18n.Messages.ConfirmacaoInicioCriacaoPontuacaoFidelidade, regional));
    Europa.Confirmacao.ConfirmCallback = function () {
        var url = Europa.Controllers.PontuacaoFidelidade.Url.MatrizPontuacaoFidelidade + "?regional=" + regional;
        
        url = url + "&create=true";

        empreendimentosSelecionados.forEach(function (empreendimento) {
            url = url + "&idEmpreendimentos=" + empreendimento.id;
        });

        if (idPontuacaoFidelidadeReferencia !== "") {
            url = url + "&idPontuacaoFidelidade=" + idPontuacaoFidelidadeReferencia;
        }

        if (atualizado) {
            url = url + "&ultimo=" + atualizado;
        }

        var tipoPontuacaoFidelidade = $("#TipoPontuacaoFidelidade").val();
        url = url + '&tipoPontuacaoFidelidade=' + tipoPontuacaoFidelidade;
                
        if (tipoPontuacaoFidelidade == 2) {            
            url = url + '&tipoCampanhaFidelidade=' + tipoCampanhaFidelidade;
        }

        evsSelecionadas.forEach(function (empresaVenda) {
            url = url + "&idEvs=" + empresaVenda.id;
        });

        if (tipoCampanhaFidelidade > 1) {
            
            url = url + "&progressao=" + progressao;

            for (var i = 0; i < progressao; i++) {
                url = url + "&qtdm=" + $("#minimo-" + i).val();
            }
            
        }
        console.log(url)
        location.href = url;
    };
    Europa.Confirmacao.Show();
};

Europa.Controllers.PontuacaoFidelidade.OnChangeTipoPontuacaoFidelidade = function () {
    if ($("#TipoPontuacaoFidelidade").val() == 1) {
        $(".campanha").addClass("hidden");
        $(".progressao").addClass("hidden");
        $("#progressao").val(0).trigger("change");
        $("#TipoCampanhaFidelidade").val(1).trigger("change");
        return;
    } 

    $(".campanha").removeClass("hidden");
};

Europa.Controllers.PontuacaoFidelidade.OnRegionalChanged = function () {
    //Empresa de vendas
    Europa.Controllers.PontuacaoFidelidade.AtualizarTreeViewEmpresaVenda();
    //Empreendimentos
    Europa.Controllers.PontuacaoFidelidade.AtualizarTreeViewEmpreendimento();
};

Europa.Controllers.PontuacaoFidelidade.FiltroTreeView = function () {
    var idPontuacaoFidelidadeReferencia = $("#IdPontuacaoFidelidadeReferencia", "#modalNovaPontuacaoFidelidade").val();
    var regional = $("#Regional", "#modalNovaPontuacaoFidelidade").val();

    return { idPontuacaoFidelidadeReferencia: idPontuacaoFidelidadeReferencia, regional: regional };
};

//Empresa de vendas
Europa.Controllers.PontuacaoFidelidade.AtualizarTreeViewEmpresaVenda = function () {

    Europa.Controllers.PontuacaoFidelidade.TreeViewEmpresaVenda
        .WithAjax("POST",
            Europa.Controllers.PontuacaoFidelidade.Url.ListarTreeEvs,
            Europa.Controllers.PontuacaoFidelidade.FiltroTreeView)
        .WithShowCheckbox(true)
        .WithRowCheck(true)
        .WithExpandIcon(false)
        .WithCollapseIcon(false)
        .WithCheckRootSiblings(true)
        .Configure();
};

//Empreendimentos
Europa.Controllers.PontuacaoFidelidade.AtualizarTreeViewEmpreendimento = function () {
    
    Europa.Controllers.PontuacaoFidelidade.TreeViewEmpreendimento
        .WithAjax("POST",
            Europa.Controllers.PontuacaoFidelidade.Url.ListarTreeEmpreendimentos,
            Europa.Controllers.PontuacaoFidelidade.FiltroTreeView)
        .WithShowCheckbox(true)
        .WithRowCheck(true)
        .WithExpandIcon(false)
        .WithCollapseIcon(false)
        .WithCheckRootSiblings(true)
        .Configure();
};

Europa.Controllers.PontuacaoFidelidade.MarcarTodasEvs = function () {
    Europa.Controllers.PontuacaoFidelidade.TreeViewEmpresaVenda.CheckAllNodes();
    Europa.Controllers.PontuacaoFidelidade.SelecionarTodasEvs = true;
};

Europa.Controllers.PontuacaoFidelidade.DesmarcarTodasEvs = function () {
    Europa.Controllers.PontuacaoFidelidade.TreeViewEmpresaVenda.UncheckAllNodes();
    Europa.Controllers.PontuacaoFidelidade.SelecionarTodasEvs = false;
};

Europa.Controllers.PontuacaoFidelidade.MarcarTodosEmpreendimentos = function () {
    Europa.Controllers.PontuacaoFidelidade.TreeViewEmpreendimento.CheckAllNodes();
    Europa.Controllers.PontuacaoFidelidade.SelecionarTodosEmpreendimentos = true;
};

Europa.Controllers.PontuacaoFidelidade.DesmarcarTodosEmpreendimentos = function () {
    Europa.Controllers.PontuacaoFidelidade.TreeViewEmpreendimento.UncheckAllNodes();
    Europa.Controllers.PontuacaoFidelidade.SelecionarTodosEmpreendimentos = false;
};

Europa.Controllers.PontuacaoFidelidade.OnChangeTipoCampanha = function () {
    var campanha = $("#TipoCampanhaFidelidade").val();

    $("#qtde_minima").html("");
    $("#progressao").val(0).trigger("change");
    $(".progressao").addClass("hidden");

    if (campanha < 2) {
        return;
    }

    $(".progressao").removeClass("hidden");

};

Europa.Controllers.PontuacaoFidelidade.OnChangeProgressao = function () {
    var progressao = $("#progressao").val();
    $("#qtde_minima").html("");
    $(".quantidade").addClass("hidden");
    Europa.Controllers.PontuacaoFidelidade.Progressao = [];

    if (progressao == 0) {        
        return;
    }

    if ($("#TipoCampanhaFidelidade").val() == 3) {
        return;
    }

    $(".quantidade").removeClass("hidden");
    for (var i = 0; i < progressao; i++) {
        $("#qtde_minima").append("<input min='0' id='minimo-"+i+"' type='number' class='col-md-1 col-md-offset-1 minimo'>");
        $(".minimo").css('width', '50px');
        $(".minimo").css('font-size', '12px');
        $(".minimo").css('height', '28px');
        $(".minimo").css('padding', '3px 6px');
    }
};

