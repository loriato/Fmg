//AutoComplete Area de Negócio
Europa.Components.AutoCompleteAreaNegocio = function() {};
Europa.Components.AutoCompleteAreaNegocio.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteAreaNegocio);

//AutoComplete Cidade
Europa.Components.AutoCompleteCidade = function () { };
Europa.Components.AutoCompleteCidade.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteCidade);
Europa.Components.AutoCompleteCidade.prototype.ProcessResult = function (data) {
	var formattedResult = [];
	data.records.forEach(function (element) { formattedResult.push({ id: element.Id, text: element.Nome + " - " + element.Estado.Codigo }); });
	return {
		results: formattedResult
	};
};

//AutoComplete Cliente
Europa.Components.AutoCompleteCliente = function () { };
Europa.Components.AutoCompleteCliente.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteCliente);

//AutoComplete Empreendimento
Europa.Components.AutoCompleteEmpreendimento = function () { };
Europa.Components.AutoCompleteEmpreendimento.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteEmpreendimento);

//AutoComplete EmpreendimentoParaExtensao
Europa.Components.AutoCompleteEmpreendimentoParaExtensao = function () { };
Europa.Components.AutoCompleteEmpreendimentoParaExtensao.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteEmpreendimentoParaExtensao);

//AutoComplete ExtensaoEmpreendimento
Europa.Components.AutoCompleteExtensaoEmpreendimento = function () { };
Europa.Components.AutoCompleteExtensaoEmpreendimento.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteExtensaoEmpreendimento).WithParamName("Empreendimento.Nome");
Europa.Components.AutoCompleteExtensaoEmpreendimento.prototype.ProcessResult = function (data) {
		var formattedResult = [];
		data.records.forEach(function (element) {
			formattedResult.push({ id: element.Id, text: element.Empreendimento.Nome });
		});
		return {
			results: formattedResult
	};
};


//AutoComplete Perfil
Europa.Components.AutoCompletePerfil = function () { };
Europa.Components.AutoCompletePerfil.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompletePerfil);

//AutoComplete Unidade Funcional
Europa.Components.AutoCompleteUnidadeFuncional = function () { };
Europa.Components.AutoCompleteUnidadeFuncional.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteUnidadeFuncional); 
Europa.Components.AutoCompleteUnidadeFuncional.prototype.ProcessResult = function (data) {
		var formattedResult = [];
		data.records.forEach(function (element) {
			formattedResult.push({ id: element.Id, text: element.Nome });
		});
		return {
			results: formattedResult
		};
	};

//AutoComplete Funcionalidade
Europa.Components.AutoCompleteFuncionalidade = function () { };
Europa.Components.AutoCompleteFuncionalidade.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteFuncionalidade);
Europa.Components.AutoCompleteFuncionalidade.prototype.ProcessResult = function (data) {
	var formattedResult = [];
	data.records.forEach(function (element) {
		formattedResult.push({ id: element.Id, text: element.Nome });
	});
	return {
		results: formattedResult
	};
};

//AutoComplete MachineClass
Europa.Components.AutoCompleteMachineClass = function () { };
Europa.Components.AutoCompleteMachineClass.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteMachineClass).WithParamName("Name");
Europa.Components.AutoCompleteMachineClass.prototype.ProcessResult = function (data) {
	var formattedResult = [];
	data.records.forEach(function (element) {
		formattedResult.push({ id: element.Id, text: element.Name });
	});
	return {
		results: formattedResult
	};
};

//AutoComplete Regional
Europa.Components.AutoCompleteRegional = function () { };
Europa.Components.AutoCompleteRegional.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteRegional);
Europa.Components.AutoCompleteRegional.prototype.ProcessResult = function(data) {
	var formattedResult = [];
	data.records.forEach(function(element) {
		formattedResult.push({ id: element.Id, text: element.Nome });
	});
	return {
		results: formattedResult
	};
};
Europa.Components.AutoCompleteRegional.FormatResponse = function(model) {
	if (model.loading) return model.text;
	return "<option value='" + model.id + "'>" + model.text + " - " + model.areaN + "</option>";
};

//AutoComplete Torre
Europa.Components.AutoCompleteTorre = function () { };
Europa.Components.AutoCompleteTorre.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteTorre);
Europa.Components.AutoCompleteTorre.prototype.ProcessResult = function (data) {
	var formattedResult = [];
	data.records.forEach(function (element) {
		formattedResult.push({ id: element.IdTorre, text: element.NomeTorre });
	});
	return {
		results: formattedResult
	};
};

//AutoComplete UnidadeLote
Europa.Components.AutoCompleteUnidadeLote = function () { };
Europa.Components.AutoCompleteUnidadeLote.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteUnidadeLote);
Europa.Components.AutoCompleteUnidadeLote.prototype.ProcessResult = function (data) {
	var formattedResult = [];
	data.records.forEach(function (element) {
		formattedResult.push({ id: element.Id, text: element.Nome + " - " + element.Torre.Nome + " - " + element.Torre.Empreendimento.Nome });
	});
	return {
		results: formattedResult
	};
};


//AutoComplete MotivoInteracao
Europa.Components.AutoCompleteMotivoInteracao = function () { };
Europa.Components.AutoCompleteMotivoInteracao.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteMotivoInteracao);
Europa.Components.AutoCompleteMotivoInteracao.prototype.ProcessResult = function (data) {
	var formattedResult = [];
	data.records.forEach(function (element) {
		formattedResult.push({ id: element.Id, text: element.Nome });
	});
	return {
		results: formattedResult
	};
};

//AutoComplete Usuario
Europa.Components.AutoCompleteUsuario = function () { };
Europa.Components.AutoCompleteUsuario.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteUsuario).WithParamName("NomeUsuario");
Europa.Components.AutoCompleteUsuario.prototype.ProcessResult = function (data) {
	var formattedResult = [];
	data.records.forEach(function (element) {
		formattedResult.push({ id: element.Id, text: element.NomeUsuario });
	});
	return {
		results: formattedResult
	};
};


//AutoComplete Cliente
Europa.Components.AutoCompleteCliente = function () { };
Europa.Components.AutoCompleteCliente.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteCliente);
Europa.Components.AutoCompleteCliente.prototype.ProcessResult = function (data) {
	var formattedResult = [];
	data.records.forEach(function (element) {
		formattedResult.push({ id: element.Id, text: element.Nome + " " + element.Sobrenome });
	});
	return {
		results: formattedResult
	};
};

//AutoComplete Loja
Europa.Components.AutoCompleteLoja = function () { };
Europa.Components.AutoCompleteLoja.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteLoja)
	.WithParamName("nomefantasia");


// Autocomplete Situação Corretor
Europa.Components.AutoCompletePalavraChave = function () { };
Europa.Components.AutoCompletePalavraChave.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompletePalavraChave);

//AutoComplete Profissão
Europa.Components.AutoCompleteProfissao = function () { };
Europa.Components.AutoCompleteProfissao.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteProfissao);

//AutoComplete Breve Lançamento
Europa.Components.AutoCompleteBreveLancamento = function () { };
Europa.Components.AutoCompleteBreveLancamento.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteBreveLancamento);

//AutoComplete Breve Lançamento da Regional (estado)
Europa.Components.AutoCompleteBreveLancamentoRegional = function () { };
Europa.Components.AutoCompleteBreveLancamentoRegional.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteBreveLancamentoRegional);

//AutoComplete Breve Lançamento da Regional (estado)
Europa.Components.AutoCompleteBreveLancamentoRegionalSemEmpreendimento = function () { };
Europa.Components.AutoCompleteBreveLancamentoRegionalSemEmpreendimento.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteBreveLancamentoRegionalSemEmpreendimento);

//AutoComplete Corretor da Empresa de Venda
Europa.Components.AutoCompleteCorretorEmpresaVenda = function () { };
Europa.Components.AutoCompleteCorretorEmpresaVenda.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteCorretorEmpresaVenda);

//AutoComplete Ponto Venda da Empresa de Venda
Europa.Components.AutoCompletePontoVendaEmpresaVenda = function () { };
Europa.Components.AutoCompletePontoVendaEmpresaVenda.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompletePontoVendaEmpresaVenda);

//AutoComplete Corretor da Empresa de Venda
Europa.Components.AutoCompleteGerenteEmpresaVenda = function () { };
Europa.Components.AutoCompleteGerenteEmpresaVenda.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteGerenteEmpresaVenda);

//AutoComplete Breve Lançamento
Europa.Components.AutoCompleteBanco = function () { };
Europa.Components.AutoCompleteBanco.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteBanco);

//AutoComplete Perfil Portal
Europa.Components.AutoCompletePerfilPortal = function () { };
Europa.Components.AutoCompletePerfilPortal.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompletePerfilPortal);

//AutoComplete Pacote do Diretor
Europa.Components.AutoCompletePacoteDiretor = function () { };
Europa.Components.AutoCompletePacoteDiretor.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompletePacoteDiretor);
Europa.Components.AutoCompletePacoteDiretor.prototype.ProcessResult = function (data) {
	var formattedResult = [];
	data.records.forEach(function (element) {
		formattedResult.push({ id: element.Nome, text: element.Nome });
	});
	return {
		results: formattedResult
	};
};

//AutoComplete Pacote do Corretor
Europa.Components.AutoCompletePacoteCorretor = function () { };
Europa.Components.AutoCompletePacoteCorretor.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompletePacoteCorretor);
Europa.Components.AutoCompletePacoteCorretor.prototype.ProcessResult = function (data) {
	var formattedResult = [];
	data.records.forEach(function (element) {
		formattedResult.push({ id: element.Nome, text: element.Nome });
	});
	return {
		results: formattedResult
	};
};

//AutoComplete PreProposta
Europa.Components.AutoCompletePreProposta = function () { };
Europa.Components.AutoCompletePreProposta.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompletePreProposta)
.WithParamName("Codigo");
Europa.Components.AutoCompletePreProposta.prototype.ProcessResult = function (data) {
	var formattedResult = [];
	data.records.forEach(function (element) {
		formattedResult.push({ id: element.Id, text: element.Codigo });
	});
	return {
		results: formattedResult
	};
};

//AutoComplete Agentes de Venda
Europa.Components.AutoCompleteAgentesVenda = function () { };
Europa.Components.AutoCompleteAgentesVenda.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteAgentesVenda);

//Hierarquia House 

//AutoComplete Pacote do Corretor
Europa.Components.AutoCompleteAgenteVendaHouseConsulta = function () { };
Europa.Components.AutoCompleteAgenteVendaHouseConsulta.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteAgenteVendaHouseConsulta)
	.WithParamName("NomeAgenteVendaHouse");
Europa.Components.AutoCompleteAgenteVendaHouseConsulta.prototype.ProcessResult = function (data) {
	var formattedResult = [];
	data.records.forEach(function (element) {
		formattedResult.push({ id: element.Id, text: element.NomeAgenteVendaHouse });
	});
	return {
		results: formattedResult
	};
};
//AutoComplete AgrupamentoPreProposta
Europa.Components.AgrupamentoProcessoPreProposta = function () { };
Europa.Components.AgrupamentoProcessoPreProposta.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteAgrupamentoProcessoPreProposta)
	.WithParamName("Descricao");
Europa.Components.AgrupamentoProcessoPreProposta.prototype.ProcessResult = function (data) {
	
	var formattedResult = [];
	data.records.forEach(function (element) {
		formattedResult.push({ id: (element.Agrupamento?"A":"S")+element.Id, text: element.Descricao });
	});
	return {
		results: formattedResult
	};
};


//AutocoComplete Estado Indique
Europa.Components.AutoCompleteEstadoIndique = function () { };
Europa.Components.AutoCompleteEstadoIndique.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteEstadoIndique);

//AutocoComplete Cidade Indique
Europa.Components.AutoCompleteCidadeIndique = function () { };
Europa.Components.AutoCompleteCidadeIndique.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteCidadeIndique);


//AutoComplete Situacao Kanban Pre-Proposta
Europa.Components.AutoCompleteSituacaoKanbanPreProposta = function () { };
Europa.Components.AutoCompleteSituacaoKanbanPreProposta.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteSituacaoKanbanPreProposta)
	.WithParamName("Nome");
Europa.Components.AutoCompleteSituacaoKanbanPreProposta.prototype.ProcessResult = function (data) {
	var formattedResult = [];
	data.records.forEach(function (element) {
		formattedResult.push({ id: element.Id, text: element.Nome });
	});
	return {
		results: formattedResult
	};
};