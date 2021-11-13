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
		formattedResult.push({ id: element.IdTorre, text: element.NomeTorre});
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

//AutoComplete Perfil Origem Contato
Europa.Components.AutoCompletePerfilOrigemContato = function () { };
Europa.Components.AutoCompletePerfilOrigemContato.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompletePerfilOrigemContato);


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
	.WithAction(Europa.Components.ActionAutoCompleteLoja);

//AutoComplete Loja disponivel (Sem EmpresaVenda associada)
Europa.Components.AutoCompleteLojaDisponivel = function () { };
Europa.Components.AutoCompleteLojaDisponivel.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteLojaDisponivel);

//AutoComplete MidiaContato
Europa.Components.AutoCompleteMidiaContato = function () { };
Europa.Components.AutoCompleteMidiaContato.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteMidiaContato);

//Autocomplete Tabulacao Atendimento
Europa.Components.AutoCompleteTabulacaoAtendimento = function () { };
Europa.Components.AutoCompleteTabulacaoAtendimento.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteTabulacaoAtendimento);

//Autocomplete Interação Pre Cliente
Europa.Components.AutoCompleteInteracaoPreCliente = function () { };
Europa.Components.AutoCompleteInteracaoPreCliente.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteInteracaoPreCliente).WithParamName("Protocolo");
Europa.Components.AutoCompleteInteracaoPreCliente.prototype.ProcessResult = function (data) {
	var formattedResult = [];
	data.records.forEach(function (element) {
		formattedResult.push({ id: element.Id, text: element.Protocolo });
	});
	return {
		results: formattedResult
	};
};

//Autocomplete Pre Cliente
Europa.Components.AutoCompletePreCliente = function () { };
Europa.Components.AutoCompletePreCliente.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompletePreCliente).WithParamName("NomeCompleto");
Europa.Components.AutoCompletePreCliente.prototype.ProcessResult = function (data) {
	var formattedResult = [];
	data.records.forEach(function (element) {
		formattedResult.push({ id: element.Id, text: element.NomeCompleto });
	});
	return {
		results: formattedResult
	};
};


//AutoComplete ExtensaoLoja
Europa.Components.AutoCompleteExtensaoLoja = function () { };
Europa.Components.AutoCompleteExtensaoLoja.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteExtensaoLoja).WithParamName("NomeComercial");
Europa.Components.AutoCompleteExtensaoLoja.prototype.ProcessResult = function (data) {
	var formattedResult = [];
	data.records.forEach(function (element) {
		formattedResult.push({ id: element.Id, text: element.NomeComercial });
	});
	return {
		results: formattedResult
	};
};

//AutoComplete Equipe
Europa.Components.AutoCompleteEquipe = function () { };
Europa.Components.AutoCompleteEquipe.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteEquipe);
Europa.Components.AutoCompleteEquipe.prototype.ProcessResult = function (data) {
	var formattedResult = [];
	data.records.forEach(function (element) {
		formattedResult.push({ id: element.Id, text: element.Nome });
	});
	return {
		results: formattedResult
	};
};

//Autocomplete Campanha
Europa.Components.AutoCompleteCampanha = function () { };
Europa.Components.AutoCompleteCampanha.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteCampanha)
	.WithParamName("Codigo");
Europa.Components.AutoCompleteCampanha.prototype.ProcessResult = function (data) {
	var formattedResult = [];
	data.records.forEach(function (element) {
		formattedResult.push({ id: element.Id, text: element.Codigo });
	});
	return {
		results: formattedResult
	};
};

//AutoComplete Consultor
Europa.Components.AutoCompleteConsultor = function () { };
Europa.Components.AutoCompleteConsultor.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteConsultor);
Europa.Components.AutoCompleteConsultor.prototype.ProcessResult = function (data) {
	var formattedResult = [];
	data.records.forEach(function (element) {
		formattedResult.push({ id: element.Id, text: element.Nome });
	});
	return {
		results: formattedResult
	};
};


//AutoComplete StatusResultadoContato
Europa.Components.AutoCompleteStatusResultadoContato = function () { };
Europa.Components.AutoCompleteStatusResultadoContato.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteResultadoContato);

//AutoComplete MeioComunicacao
Europa.Components.AutoCompleteMeioComunicacao = function () { };
Europa.Components.AutoCompleteMeioComunicacao.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteMeioComunicacao);

//AutoComplete MeioComunicacaoSemMidia
Europa.Components.AutoCompleteMeioComunicacaoSemMidia = function () { };
Europa.Components.AutoCompleteMeioComunicacaoSemMidia.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteMeioComunicacaoSemMidia);

//AutoComplete ScriptCampanha
Europa.Components.AutoCompleteScriptCampanha = function () { };
Europa.Components.AutoCompleteScriptCampanha.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteScriptCampanha);

//AutoComplete Evento
Europa.Components.AutoCompleteEvento = function () { };
Europa.Components.AutoCompleteEvento.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteEvento);

//Autocomplete Gerenciamento Mailing
Europa.Components.AutoCompleteGerenciamentoMailing = function () { };
Europa.Components.AutoCompleteGerenciamentoMailing.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteGerenciamentoMailing);

//AutoComplete ExtensaoLojaModal Associações
Europa.Components.AutoCompleteExtensaoLojaModal = function () { };
Europa.Components.AutoCompleteExtensaoLojaModal.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteExtensaoLojaModal).WithParamName("NomeComercial");
Europa.Components.AutoCompleteExtensaoLojaModal.prototype.ProcessResult = function (data) {
	var formattedResult = [];
	data.records.forEach(function (element) {
		formattedResult.push({ id: element.Id, text: element.NomeComercial });
	});
	return {
		results: formattedResult
	};
};

//AutoComplete Lojas por Consultor
Europa.Components.AutoCompleteLojasPorConsultor = function () { };
Europa.Components.AutoCompleteLojasPorConsultor.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteLojasPorConsultor).WithParamName("NomeComercial");
Europa.Components.AutoCompleteLojasPorConsultor.prototype.ProcessResult = function (data) {
	var formattedResult = [];
	data.records.forEach(function (element) {
		formattedResult.push({ id: element.Id, text: element.NomeComercial });
	});
	return {
		results: formattedResult
	};
};;

//Autocomplete Template Mensagem
Europa.Components.AutoCompleteTemplateMensagem = function () { };
Europa.Components.AutoCompleteTemplateMensagem.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteTemplateMensagem);

// Autocomplete Assunto
Europa.Components.AutoCompleteAssunto = function () { };
Europa.Components.AutoCompleteAssunto.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteAssunto);
Europa.Components.AutoCompleteAssunto.prototype.ProcessResult = function (data) {
	var formattedResult = [];
	data.records.forEach(function (element) {
		formattedResult.push({ id: element.Id, text: element.Nome });
	});
	return {
		results: formattedResult
	};
};


// Autocomplete Documento
Europa.Components.AutoCompleteDocumento = function () { };
Europa.Components.AutoCompleteDocumento.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteDocumento).WithParamName("Detalhe");
Europa.Components.AutoCompleteDocumento.prototype.ProcessResult = function (data) {
	var formattedResult = [];
	data.records.forEach(function (element) {
		formattedResult.push({ id: element.Id, text: element.Detalhe });
	});
	return {
		results: formattedResult
	};
};

// Autocomplete PalavraChave
Europa.Components.AutoCompletePalavraChave = function () { };
Europa.Components.AutoCompletePalavraChave.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompletePalavraChave);

// Autocomplete Situação Corretor
Europa.Components.AutoCompletePalavraChave = function () { };
Europa.Components.AutoCompletePalavraChave.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompletePalavraChave);


//AutoComplete EmpresaVendas
Europa.Components.AutoCompleteEmpresaVendas = function () { };
Europa.Components.AutoCompleteEmpresaVendas.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteEmpresaVendas)
	.WithParamName("NomeFantasia");
Europa.Components.AutoCompleteEmpresaVendas.prototype.ProcessResult = function (data) {
	var formattedResult = [];
	data.records.forEach(function (element) {
		formattedResult.push({ id: element.Id, text: element.NomeFantasia });
	});
	return {
		results: formattedResult
	};
};

//AutoComplete EmpresaVendas
Europa.Components.AutoCompleteEmpresaVendasCACT = function () { };
Europa.Components.AutoCompleteEmpresaVendasCACT.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteEmpresaVendasCACT)
	.WithParamName("NomeFantasia");
Europa.Components.AutoCompleteEmpresaVendasCACT.prototype.ProcessResult = function (data) {
	var formattedResult = [];
	data.records.forEach(function (element) {
		formattedResult.push({ id: element.Id, text: element.NomeFantasia });
	});
	return {
		results: formattedResult
	};
};


// Autocomplete Situação Corretor
Europa.Components.AutoCompleteCorretor = function () { };
Europa.Components.AutoCompleteCorretor.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteCorretor);

// Autocomplete Situação Corretor Gerente
Europa.Components.AutoCompleteCorretorGerente = function () { };
Europa.Components.AutoCompleteCorretorGerente.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteCorretorGerente);



//AutoComplete Breve Lançamento
Europa.Components.AutoCompleteBreveLancamento = function () { };
Europa.Components.AutoCompleteBreveLancamento.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteBreveLancamento);

//AutoComplete Breve Lançamento
Europa.Components.AutoCompleteBreveLancamentoListarPorRegionalDisponiveisParaCatalogoNoEstado = function () { };
Europa.Components.AutoCompleteBreveLancamentoListarPorRegionalDisponiveisParaCatalogoNoEstado.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteBreveLancamentoListarPorRegionalDisponiveisParaCatalogoNoEstado);
Europa.Components.AutoCompleteBreveLancamentoListarPorRegionalDisponiveisParaCatalogoNoEstado.prototype.ProcessResult = function (data) {
	var formattedResult = [];
	data.forEach(function (element) {
		formattedResult.push({ id: element.Value, text: element.Text });
	});
	return {
		results: formattedResult
	};
};

//AutoComplete Ponto Venda
Europa.Components.AutoCompletePontoVenda = function () { };
Europa.Components.AutoCompletePontoVenda.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompletePontoVenda)
	.WithParamName("Nome");
Europa.Components.AutoCompletePontoVenda.prototype.ProcessResult = function (data) {
	var formattedResult = [];
	data.records.forEach(function (element) {
		formattedResult.push({ id: element.Id, text: element.Nome });
	});
	return {
		results: formattedResult
	};
};

//AutoComplete Profissão
Europa.Components.AutoCompleteProfissao = function () { };
Europa.Components.AutoCompleteProfissao.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteProfissao);

//AutoComplete Breve Lançamento
Europa.Components.AutoCompleteBanco = function () { };
Europa.Components.AutoCompleteBanco.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteBanco);

//AutoComplete Breve Lançamento da Regional (estado)
Europa.Components.AutoCompleteBreveLancamentoRegionalSemEmpreendimento = function () { };
Europa.Components.AutoCompleteBreveLancamentoRegionalSemEmpreendimento.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteBreveLancamentoRegionalSemEmpreendimento);

//AutoComplete Corretor da Empresa de Venda
Europa.Components.AutoCompleteCorretorEmpresaVenda = function () { };
Europa.Components.AutoCompleteCorretorEmpresaVenda.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteCorretorEmpresaVenda);


//AutoComplete Viabilizador
Europa.Components.AutoCompleteViabilizador = function () { };
Europa.Components.AutoCompleteViabilizador.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteViabilizador)
	.WithParamName("Nome");
Europa.Components.AutoCompleteViabilizador.prototype.ProcessResult = function (data) {
	var formattedResult = [];
	data.records.forEach(function (element) {
		formattedResult.push({ id: element.Id, text: element.Nome + " - " + element.Login });
	});
	return {
		results: formattedResult
	};
};

//AutoComplete Perfil Portal
Europa.Components.AutoCompletePerfilPortal = function () { };
Europa.Components.AutoCompletePerfilPortal.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompletePerfilPortal);

//AutoComplete Tipo Custo 
Europa.Components.AutoCompleteTipoCusto = function () { };
Europa.Components.AutoCompleteTipoCusto.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteTipoCusto)
	.WithParamName("Descricao");
Europa.Components.AutoCompleteTipoCusto.prototype.ProcessResult = function (data) {
	var formattedResult = [];
	data.records.forEach(function (element) {
		formattedResult.push({ id: element.Id, text: element.Descricao });
	});
	return {
		results: formattedResult
	};
};

//AutoComplete Classificacao
Europa.Components.AutoCompleteClassificacao = function () { };
Europa.Components.AutoCompleteClassificacao.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteClassificacao)
	.WithParamName("Descricao");
Europa.Components.AutoCompleteClassificacao.prototype.ProcessResult = function (data) {
	var formattedResult = [];
	data.records.forEach(function (element) {
		formattedResult.push({ id: element.Id, text: element.Descricao });
	});
	return {
		results: formattedResult
	};
};



//AutoComplete Fornecedor 
Europa.Components.AutoCompleteFornecedor = function () { };
Europa.Components.AutoCompleteFornecedor.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteFornecedor)
	.WithParamName("NomeFantasia");
Europa.Components.AutoCompleteFornecedor.prototype.ProcessResult = function (data) {
	var formattedResult = [];
	data.records.forEach(function (element) {
		formattedResult.push({ id: element.Id, text: element.NomeFantasia });
	});
	return {
		results: formattedResult
	};
};

//AutoComplete Centro Custo
Europa.Components.AutoCompleteCentroCusto = function () { };
Europa.Components.AutoCompleteCentroCusto.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteCentroCusto)
	.WithParamName("Codigo");
Europa.Components.AutoCompleteCentroCusto.prototype.ProcessResult = function (data) {
	var formattedResult = [];
	data.records.forEach(function (element) {
		formattedResult.push({ id: element.Id, text: element.Codigo });
	});
	return {
		results: formattedResult
	};
};

//AutoComplete Pacote
Europa.Components.AutoCompletePacote = function () { };
Europa.Components.AutoCompletePacote.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompletePacote)
	.WithParamName("DescricaoPacote");
Europa.Components.AutoCompletePacote.prototype.ProcessResult = function (data) {
	var formattedResult = [];
	data.records.forEach(function (element) {
		formattedResult.push({ id: element.DescricaoPacote, text: element.DescricaoPacote });
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

//AutoComplete Stand Venda
Europa.Components.AutoCompleteStandVenda = function () { };
Europa.Components.AutoCompleteStandVenda.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteStandVenda);
Europa.Components.AutoCompleteStandVenda.prototype.ProcessResult = function (data) {
	var formattedResult = [];
	data.records.forEach(function (element) {
		formattedResult.push({ id: element.Id, text: element.Nome });
	});
	return {
		results: formattedResult
	};
};

//AutoComplete Titulos Banners
Europa.Components.AutoCompleteTituloBanners = function () { };
Europa.Components.AutoCompleteTituloBanners.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteTituloBanners)
	.WithParamName("Descricao");
Europa.Components.AutoCompleteTituloBanners.prototype.ProcessResult = function (data) {
	var formattedResult = [];
	data.records.forEach(function (element) {
		formattedResult.push({ id: element.Id, text: element.Descricao });
	});
	return {
		results: formattedResult
	};
};


//AutoComplete Regionais
Europa.Components.AutoCompleteRegionais = function () { };
Europa.Components.AutoCompleteRegionais.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteRegionais)
	.WithParamName("Nome");
Europa.Components.AutoCompleteRegionais.prototype.ProcessResult = function (data) {
	var formattedResult = [];
	data.records.forEach(function (element) {
		formattedResult.push({ id: element.Id, text: element.Nome });
	});
	return {
		results: formattedResult
	};
};

//AutoComplete RegionalEmpresa
Europa.Components.AutoCompleteRegionalEmpresa = function () { };
Europa.Components.AutoCompleteRegionalEmpresa.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteRegionalEmpresa)
	.WithParamName("Id");
Europa.Components.AutoCompleteRegionalEmpresa.prototype.ProcessResult = function (data) {
	var formattedResult = [];
	data.records.forEach(function (element) {
		formattedResult.push({ id: element.Id, text: element.Id});
	});
	return {
		results: formattedResult
	};
};

//AutoComplete EmpresaVendas
//AutoComplete Supervisor House
Europa.Components.AutoCompleteSupervisorHouse = function () { };
Europa.Components.AutoCompleteSupervisorHouse.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteSupervisorHouse)
	.WithParamName("NomeSupervisorHouse");
Europa.Components.AutoCompleteSupervisorHouse.prototype.ProcessResult = function (data) {
	var formattedResult = [];
	data.records.forEach(function (element) {
		formattedResult.push({ id: element.IdSupervisorHouse, text: element.NomeSupervisorHouse });
	});
	return {
		results: formattedResult
	};
};

//AutoComplete Agente Venda House
Europa.Components.AutoCompleteAgenteVendaHouse = function () { };
Europa.Components.AutoCompleteAgenteVendaHouse.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteAgenteVendaHouse)
	.WithParamName("NomeAgenteVendaHouse");
Europa.Components.AutoCompleteAgenteVendaHouse.prototype.ProcessResult = function (data) {
	var formattedResult = [];
	data.records.forEach(function (element) {
		formattedResult.push({ id: element.IdAgenteVendaHouse, text: element.NomeAgenteVendaHouse });
	});
	return {
		results: formattedResult
	};
};

//AutoComplete Agente Venda House
Europa.Components.AutoCompleteHouse = function () { };
Europa.Components.AutoCompleteHouse.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteHouse)
	.WithParamName("NomeHouse");
Europa.Components.AutoCompleteHouse.prototype.ProcessResult = function (data) {
	var formattedResult = [];
	data.records.forEach(function (element) {
		formattedResult.push({ id: element.IdHouse, text: element.NomeHouse });
	});
	return {
		results: formattedResult
	};
};

//AutoCompleteLojaPortal
Europa.Components.AutoCompleteLojaPortal = function () { };
Europa.Components.AutoCompleteLojaPortal.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteLojaPortal)
	.WithParamName("Nome");
Europa.Components.AutoCompleteLojaPortal.prototype.ProcessResult = function (data) {
	var formattedResult = [];
	data.records.forEach(function (element) {
		formattedResult.push({ id: element.Id, text: element.Nome });
	});
	return {
		results: formattedResult
	};
};

//AutoCompleteCoordenadorHouse
Europa.Components.AutoCompleteCoordenadorHouse = function () { };
Europa.Components.AutoCompleteCoordenadorHouse.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteCoordenadorHouse)
	.WithParamName("NomeCoordenadorHouse");
Europa.Components.AutoCompleteCoordenadorHouse.prototype.ProcessResult = function (data) {
	var formattedResult = [];
	data.records.forEach(function (element) {
		formattedResult.push({ id: element.IdCoordenadorHouse, text: element.NomeCoordenadorHouse + " - " + element.LoginCoordenadorHouse });
	});
	return {
		results: formattedResult
	};
};


//AutoComplete Sistemas
Europa.Components.AutoCompleteSistemas = function () { };
Europa.Components.AutoCompleteSistemas.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteSistemas)
	.WithParamName("Nome");
Europa.Components.AutoCompleteSistemas.prototype.ProcessResult = function (data) {
	var formattedResult = [];
	data.records.forEach(function (element) {
		formattedResult.push({ id: element.Id, text: element.Nome });
	});
	return {
		results: formattedResult
	};
};


//AutoComplete AgrupamentoProcessoPreProposta somente os não vinculados
Europa.Components.AutoCompleteAgrupamentoProcessoPreProposta = function () { };
Europa.Components.AutoCompleteAgrupamentoProcessoPreProposta.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteAgrupamentoProcessoPreProposta)
	.WithParamName("Nome");
Europa.Components.AutoCompleteAgrupamentoProcessoPreProposta.prototype.ProcessResult = function (data) {
	var formattedResult = [];
	data.records.forEach(function (element) {
		formattedResult.push({ id: element.Id, text: element.Nome });
	});
	return {
		results: formattedResult
	};
};

//AutoComplete StatusPreProposta
Europa.Components.AutoCompleteStatusPreProposta = function () { };
Europa.Components.AutoCompleteStatusPreProposta.prototype = new Europa.Components.AutoComplete()
	.WithAction(Europa.Components.ActionAutoCompleteStatusPreProposta)
	.WithParamName("StatusPadrao");
Europa.Components.AutoCompleteStatusPreProposta.prototype.ProcessResult = function (data) {
	var formattedResult = [];
	data.records.forEach(function (element) {		
		formattedResult.push({ id: element.Id, text: element.StatusPadrao});
	});
	return {
		results: formattedResult
	};
};


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