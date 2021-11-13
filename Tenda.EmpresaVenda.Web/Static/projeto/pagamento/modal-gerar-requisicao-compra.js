Europa.Controllers.Pagamento.ModalGerarRC = "#gerar_requisicao_compra";
Europa.Controllers.Pagamento.DivItemRequisicao = "#div_form_item_requisicao";
Europa.Controllers.Pagamento.DivDesignacaoContaRequisicao = "#div_form_designacao_conta_requisicao";
Europa.Controllers.Pagamento.FormItemRequisicao = "#form_item_requisicao";
Europa.Controllers.Pagamento.FormDesignacaoContaRequisicao = "#form_designacao_conta_requisicao";

Europa.Controllers.Pagamento.GetRow = undefined;
Europa.Controllers.Pagamento.GetDataRow = undefined;
Europa.Controllers.Pagamento.ListaPagamentoRC = [];


$(function () {
	
});

Europa.Controllers.Pagamento.AbrirModalGerarRequisicao = function (obj) {
	obj.ValorAPagar = obj.ValorAPagar.toString().replace(".", ",");
	$.post(Europa.Controllers.Pagamento.UrlGerarRequisicaoCompraModal, { model: obj }, function (res) {
		

		$(Europa.Controllers.Pagamento.DivItemRequisicao).html(res.Objeto.htmlItemRequisicao);	
		$(Europa.Controllers.Pagamento.DivDesignacaoContaRequisicao).html(res.Objeto.htmlDesignacaoContaRequisicao);

		Europa.Controllers.Pagamento.InitDatePickerModal();
		Europa.Controllers.Pagamento.AplicarMascaraModal();
		$(Europa.Controllers.Pagamento.ModalGerarRC).modal("show");
	});
};

Europa.Controllers.Pagamento.InitDatePickerModal = function () {

	Europa.Controllers.Pagamento.DataRemessaItem = new Europa.Components.DatePicker()
		.WithTarget("#DataRemessaItem")
		.WithParentEl(Europa.Controllers.Pagamento.ModalGerarRC)
		.WithValue(Europa.Date.Now("DD/MM/YYYY"))
		.Configure();
	Europa.Mask.Apply("#DataRemessaItem", Europa.Mask.FORMAT_DATE);

	Europa.Controllers.Pagamento.DataSolicitacao = new Europa.Components.DatePicker()
		.WithTarget("#DataSolicitacao")
		.WithValue(Europa.Date.Now("DD/MM/YYYY"))
		.WithParentEl(Europa.Controllers.Pagamento.ModalGerarRC)
		.Configure();
	Europa.Mask.Apply("#DataSolicitacao", Europa.Mask.FORMAT_DATE);

	Europa.Controllers.Pagamento.DataLiberacao = new Europa.Components.DatePicker()
		.WithTarget("#DataLiberacao")
		.WithValue(Europa.Date.Now("DD/MM/YYYY"))
		.WithParentEl(Europa.Controllers.Pagamento.ModalGerarRC)
		.Configure();
	Europa.Mask.Apply("#DataLiberacao", Europa.Mask.FORMAT_DATE);
};

Europa.Controllers.Pagamento.GerarRequisicaoCompra = function () {

	if (Europa.Controllers.Pagamento.ListaPagamentos.length < 1) {
		Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Erro, Europa.i18n.Messages.NenhumRegistroValidoSelecionado);
		Europa.Informacao.Show();
		return null;
	};

	$("#gerar-requisicao-compra").prop("disabled", true);

	var pagamentos = {
		pagamentos: Europa.Controllers.Pagamento.ListaPagamentos
	};

	$.post(Europa.Controllers.Pagamento.UrlGerarRequisicaoCompra, pagamentos, function (res) {
		if (res.Sucesso) {
			Europa.Controllers.StatusImportacao.Show(res.Task);
			Europa.Controllers.Pagamento.DesenharTabelas();
			Europa.Controllers.Pagamento.ListaPagamentos = [];
			$("#gerar-requisicao-compra").prop("disabled", false);
			$("#StatusImportacao_Download").addClass("hidden");
		} else {
			Europa.Informacao.PosAcao(res);
		}

	});	

};

Europa.Controllers.Pagamento.FilaRequisicaoCompra = function () {
	var pag = Europa.Controllers.Pagamento.ListaPagamentoRC;
	$(Europa.Controllers.Pagamento.GetRow).addClass('selected');
	$(Europa.Controllers.Pagamento.GetRow).find(".gerar-rc").addClass("disabled");

	objItemRequisicao = Europa.Form.SerializeJson(Europa.Controllers.Pagamento.FormItemRequisicao)
	objItemRequisicao.Preco = objItemRequisicao.Preco.replace('.', '');
	objItemRequisicao.PrecoUnidade = objItemRequisicao.PrecoUnidade.replace('.', '');

	var param = {
		itemRequisicao: objItemRequisicao,
		contabilizacaoRequisicao: Europa.Form.SerializeJson(Europa.Controllers.Pagamento.FormDesignacaoContaRequisicao),
		pagamento: Europa.Controllers.Pagamento.GetDataRow
	};
	pag.push(param);
	Europa.Controllers.Pagamento.FecharModalGerarRequisicao();
};



Europa.Controllers.Pagamento.AplicarMascaraModal = function () {
	Europa.Mask.Dinheiro($("#Preco", Europa.Controllers.Pagamento.FormItemRequisicao));
	Europa.Mask.Dinheiro($("#PrecoUnidade", Europa.Controllers.Pagamento.FormItemRequisicao));
};

Europa.Controllers.Pagamento.FecharModalGerarRequisicao = function () {
	$(Europa.Controllers.Pagamento.ModalGerarRC).modal("hide");
};