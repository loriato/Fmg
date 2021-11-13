Europa.Controllers.PagamentoUnificado.ModalVisualizarRC = "#visualizar_requisicao_compra";
Europa.Controllers.PagamentoUnificado.DivItemRequisicao = "#div_form_item_requisicao";
Europa.Controllers.PagamentoUnificado.DivDesignacaoContaRequisicao = "#div_form_designacao_conta_requisicao";
Europa.Controllers.PagamentoUnificado.FormItemRequisicao = "#form_item_requisicao";
Europa.Controllers.PagamentoUnificado.FormDesignacaoContaRequisicao = "#form_designacao_conta_requisicao";

$(function () {

});

Europa.Controllers.PagamentoUnificado.AbrirModalGerarRequisicao = function (obj) {
	obj.ValorAPagar = obj.ValorAPagar.toString().replace(".", ",");
	$.post(Europa.Controllers.PagamentoUnificado.UrlVisualizarRequisicaoCompraModal, { model: obj }, function (res) {

		$(Europa.Controllers.PagamentoUnificado.DivItemRequisicao).html(res.Objeto.htmlItemRequisicao);
		$(Europa.Controllers.PagamentoUnificado.DivDesignacaoContaRequisicao).html(res.Objeto.htmlDesignacaoContaRequisicao);

		Europa.Controllers.PagamentoUnificado.InitDatePickerModal();
		Europa.Controllers.PagamentoUnificado.AplicarMascaraModal();
		$(Europa.Controllers.PagamentoUnificado.ModalVisualizarRC).modal("show");
	});
};

Europa.Controllers.PagamentoUnificado.InitDatePickerModal = function () {

	Europa.Controllers.PagamentoUnificado.DataRemessaItem = new Europa.Components.DatePicker()
		.WithTarget("#DataRemessaItem")
		.WithParentEl(Europa.Controllers.PagamentoUnificado.ModalVisualizarRC)
		.WithValue(Europa.Date.Now("DD/MM/YYYY"))
		.Configure();
	Europa.Mask.Apply("#DataRemessaItem", Europa.Mask.FORMAT_DATE);

	Europa.Controllers.PagamentoUnificado.DataSolicitacao = new Europa.Components.DatePicker()
		.WithTarget("#DataSolicitacao")
		.WithValue(Europa.Date.Now("DD/MM/YYYY"))
		.WithParentEl(Europa.Controllers.PagamentoUnificado.ModalVisualizarRC)
		.Configure();
	Europa.Mask.Apply("#DataSolicitacao", Europa.Mask.FORMAT_DATE);

	Europa.Controllers.PagamentoUnificado.DataLiberacao = new Europa.Components.DatePicker()
		.WithTarget("#DataLiberacao")
		.WithValue(Europa.Date.Now("DD/MM/YYYY"))
		.WithParentEl(Europa.Controllers.PagamentoUnificado.ModalVisualizarRC)
		.Configure();
	Europa.Mask.Apply("#DataLiberacao", Europa.Mask.FORMAT_DATE);
};

Europa.Controllers.PagamentoUnificado.AplicarMascaraModal = function () {
	Europa.Mask.Dinheiro($("#Preco", Europa.Controllers.PagamentoUnificado.FormItemRequisicao));
	Europa.Mask.Dinheiro($("#PrecoUnidade", Europa.Controllers.PagamentoUnificado.FormItemRequisicao));
};

Europa.Controllers.PagamentoUnificado.FecharModalGerarRequisicao = function () {
	$(Europa.Controllers.PagamentoUnificado.ModalVisualizarRC).modal("hide");
};