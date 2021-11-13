$(function () {
	var inputs = $('.input');
	var paras = $('.description-flex-container').find('p');
	$(inputs).click(function () {
		var t = $(this),
			ind = t.index(),
			matchedPara = $(paras).eq(ind);

		$(t).add(matchedPara).addClass('active');
		$(inputs).not(t).add($(paras).not(matchedPara)).removeClass('active');
	});
});

Europa.Controllers.Financeiro.AbrirModalHistorico = function (row) {
	var selecionado = Europa.Controllers.Financeiro.Tabela.getRowData(row);
	//if (selecionado.Filhos.length != 1) {
	//	Europa.Informacao.ChangeHeaderAndContent(Europa.i18n.Messages.Erro, "Selecione apenas um registro para executar essa ação");
	//	Europa.Informacao.Show();
	//	return null;
	//};
	var id = selecionado.Filhos[0].IdNotaFiscalPagamento;
	var idEmpresaVenda = selecionado.Filhos[0].IdEmpresaVenda;
	var pedido = selecionado.Filhos[0].PedidoSap;

	$.post(Europa.Controllers.Financeiro.UrlHistoricoNotaFiscal, { Id: id, idEmpresaVenda, pedido }, function (res) {
		$("#dados").html(res.Objeto);
		$("#historico_nota_fiscal").modal("show");
		if ($("#DataAprovado").val()) {
			$('.input').removeClass('active');
			$('#aprovado').addClass('active');

		}
		else if ($("#DataRecebido").val()) {
			$('.input').removeClass('active');
			$('#recebido').addClass('active');
		}
		else if ($("#DataEnviado").val()) {
			$('.input').removeClass('active');
			$('#enviada').addClass('active');
		}
	});
   
};