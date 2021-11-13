"use strict"

var Europa = {};
Europa.Controllers = {};
Europa.Components = {};
Europa.Components.Datatable = {};
Europa.Components.Modal = {};
Europa.i18n = {};

Europa.WindowFunction = function (functionName, context, args) {
	var namespaces = functionName.split(".");
	var func = namespaces.pop();
	for (var i = 0; i < namespaces.length; i++) {
		context = context[namespaces[i]];
	}
	return context[func].apply(args);
}

Europa.Ready = function () {
	Europa.Components.AutoComplete.AdjustWidth();
	Europa.Components.DatePicker.AutoApply();
	Europa.AutoCompleteFix();
	Europa.Initialize();
};

Europa.Initialize = function () {
	if ($('#corpo-conteudo-layout').children().length == 0) {
		$("#block_content_page").addClass("block-content-transp");
		$("#title-bar-display").addClass("hidden");
	} else {
		$("#block_content_page").addClass("block-content");
		$("#title-bar-display").removeClass("hidden");
	}

	var windowSize = $(window).height();
	var scrollTop = $('.block-content').position();
	if (scrollTop) {
		scrollTop = scrollTop.top;
	} else {
		scrollTop = 0;
	}

	var block =  $('.block-content');
	var padding = parseInt(block.css('paddingTop').replace('px', ''));
	if($("#title-bar-display").hasClass("hidden")){
		scrollTop += 46;
	}
	block.height(windowSize - scrollTop + padding);
}

//Implementação do timer do AjaxStatus
Europa.AjaxStart = function (event) {
	if (event.delegateTarget.activeElement.className.indexOf("ajax-global-false") <= -1
		&& $(event.delegateTarget.activeElement).parents().closest(".ajax-global-false").length <= 0) {
		Europa.AjaxTest();
	}
};
Europa.AjaxTest = function () {
	Spinner.Show();
};
Europa.AjaxStop = function () {
	Europa.DisableLabelButtonClickFix();
	Spinner.Hide();
};

Europa.AjaxConfigure = function () {
	Europa.DisableLabelButtonClickFix();
	$(document).ajaxStart(function (event) {
		Europa.AjaxStart(event);
	}).ajaxStop(function () {
		Europa.AjaxStop();
		//Europa.Ready();
	});
};


Europa.AjustTitleAndMenuAcitons = function () {
	var titlebarName = $('#titlebar-name');
	var titlebarButtons = $('#titlebar-buttons');
	var titlebarExtra = $('#titlebar-extra');

	var titleTarget = $('#titlebar-name-target');
	var buttonsTarget = $('#titlebar-buttons-target');
	var extraTarget = $('#titlebar-extra-target');


	titleTarget.html(titlebarName.show().html());
	buttonsTarget.html(titlebarButtons.show().html());

	if(titlebarExtra){
		extraTarget.html(titlebarExtra.show().html());
		titlebarExtra.remove();
	}

	titlebarName.remove();
	titlebarButtons.remove();
};

$(document).ready(function () {
	//Verifia se o jquery-ui foi carregado
	if (typeof jQuery.ui != 'undefined') {
		$(".modal").draggable({
			handle: ".modal-header"
		});
	}
});

Europa.Components.Cep = {};
//A variável abaixo é definida no /Shared/_AutoCompleteAction.cshtml;
Europa.Components.Cep.Action = undefined;
Europa.Components.Cep.UrlFor = function (cep) {
	return Europa.Components.Cep.Action + "?cep=" + cep;
}
Europa.Components.Cep.Search = function (cep, callback) {
	$.getJSON(Europa.Components.Cep.UrlFor(cep), function (dados) {
		if (dados.Sucesso != undefined && dados.Sucesso == false) {
			Europa.Informacao.ChangeHeaderAndContent('Atenção', dados.Objeto);
			Europa.Informacao.Show();
		} else {
			callback(dados);
		}
	});
}

Europa.AutoCompleteFix = function () {

	if (navigator.userAgent.indexOf('Safari') !== -1 && navigator.userAgent.indexOf('Chrome') === -1) {
		//Safari
		$('select.select2-container').change(function () {
			fillInEntry($(this));
		});

		$('select.select2-container').each(function () {
			fillInEntry($(this));
		});

	}

	function fillInEntry($elem) {
		var $selected = $elem.find('option:selected');

		if ($selected !== undefined) {
			setTimeout(function () {
				$("#select2-" + $elem.attr("id") + "-container").append($selected.text());
			}, 100);
		}
	}
}

Europa.NavbarScrollControl = function (menuId, mainDivId) {
	var topMenu = $("#" + menuId);
	var menuItems = topMenu.children().children("a");
	var scrollItems = menuItems.map(function () {
		var item = $($(this).attr("href"));
		if (item.length) {
			return item;
		}
	});

	$("#" + mainDivId).scroll(function () {
		var topDistance = $(this).position().top;
		var fromTop = $(this).scrollTop() + topDistance;
		var curr = scrollItems.map(function (i) {
			if (i == 1) {
			}
			if (this.position().top + topDistance <= fromTop) {
				return this;
			}
		});
		curr = curr[curr.length - 1];
		var id = curr && curr.length ? curr[0].id : "";
		menuItems
			.parent().removeClass("active")
			.end().filter("[href='#" + id + "']").parent().addClass("active");
		;
	});
}

Europa.OnTabChange = function (el, tabId, divId) {
	$("#" + tabId + " li").removeClass("active");
	$(el).addClass("active");
	$("#" + divId).animate(
		{ scrollTop: $($(el).find("a").attr("href")).position().top },
		500);
}


Europa.GridClickLink = function () {
	$('.europa_detail').off().on("click",
		function (event) {
			var input = $(this);
			var url = input.data('href');
			if (event.metaKey || event.ctrlKey) {
				window.open(url, '_blank');
			} else {
				if (input.hasClass("as_tab")) {
					window.open(url, '_blank');
				} else {
					window.location.href = url;
				}
			}
		});
};

Europa.MaskCpfCnpj = function (id, form) {
	var cpfCnpj = function (val) {
			return val.length > 14 ? '00.000.000/0000-00' : '000.000.000-009';
		},
		optionsDocumento = {
			onKeyPress: function (val, e, field, options) {
				field.mask(cpfCnpj(val), options);
			},
			clearIfNotMatch: false
		};
	$("#" + id,  form).unmask().mask(cpfCnpj, optionsDocumento);
};

Europa.MaskTelefone = function (id, form) {
	var spMaskBehavior = function (val) {
			return val.replace(/\D/g, "").length === 11 ? "(00) 00000-0000" : "(00) 0000-00009";
		},
		spOptions = {
			onKeyPress: function (val, e, field, options) {
				field.mask(spMaskBehavior.apply({}, arguments), options);
			},
			clearIfNotMatch: true
		};

	$("#" + id, form).unmask().mask(spMaskBehavior, spOptions);
};

Europa.DisableLabelButtonClickFix = function () {
	$('.btn-default[disabled].active, .btn-default[disabled]').click(function (event) {
		event.stopPropagation();
	});
};

Europa.RemoveSubstituteFieldOf = function (input) {
	input = $(input);
	input.parent().find(".replaced-input").remove();
};

Europa.AddSubstituteFieldTo = function (input) {
	if ($(input).is("[aria-controls^='DataTables_Table']")) {
		return true;
	}
	input = $(input);
	var text;
	var isSelect = false;
	var isArea = false;
	var isInput = false;
	if (input.is('input')) {
		isInput = true;
		text = input.val();
	} else if (input.is('textarea')) {
		text = input.val();
		isArea = true;
	} else {
		text = input.find(':selected').text();
		isSelect = true;
	}
	text = $("<div></div>").text(text).html();

	if (input.attr('data-entity') !== undefined) {

		var entity = input.data('entity');
		var url = Europa.Components.DetailAction.Links[entity];
		var id = input.attr('data-id');
		var clickTel = "";
		var idParent = "";
		if (url) {
			if (id) {
				if (input.attr('data-entity') != "CallCliente") {
					url = url + "/" + id;
				} else {
					if (input.attr('data-id-parent') != undefined) {
						idParent = input.attr('data-id-parent');
					}
					clickTel = ' onclick="Europa.IniciarAtendimentoTelefone(' + idParent + ')" ';
					url = url + id;
				}
			}
			var title = '';
			if (input.data('title') !== undefined) {
				title = 'title="' + input.data('title') + '"';
			}
            text = '<a href="' + url + '" ' + title + clickTel + 'rel="noopener noreferrer" target="_blank">' + text + '</a>';
		}
	}
	var aux = "<div readonly class='form-control replaced-input' style='position:absolute; padding: 6px 6px;z-index:101;'> " + text + "</div>";
	aux = $.parseHTML(aux);
	aux = $(aux);
	if (isArea) {
		aux.css("height", input.outerHeight());
		aux.css("width", input.outerWidth());
	}

	if (isInput) {
		aux.addClass(input.attr('class'));
		if (entity == "CallCliente") {
			aux.addClass("europa_detail_skype");
			aux.css("text-decoration", "none");
		}
		aux.css("white-space", "nowrap");
		aux.css("overflow", "hidden");

		aux.each(function () {
			this.style.setProperty('width', input.outerWidth() + 'px', 'important');
		});
	}

	if (isArea) {
		aux.css("white-space", "pre-wrap");
		aux.css("word-wrap", "break-word");
		aux.css("overflow", "auto");
	}

	if (input.data('entity') && input.val()) {
		if (entity == "CallCliente") {
			aux.addClass("europa_detail_skype");
		} else {
			aux.addClass('europa_div_detail');
		}
		aux.attr('data-entity', input.data('entity'));
		aux.attr('data-id', isSelect ? input.val() : input.data('id'));
	}

	if (!input.hasClass('select2-container')) {
		aux.css("display", input.css('display'));
	}

	if (input.parent().find('.replaced-input').length > 0) {
		input.parent().find('.replaced-input').remove();
	}



	$(input).before($(aux).prop("outerHTML"));
};

Europa.AddSubstituteFields = function () {
	Europa.RemoveSubstituteFields();
	$("fieldset[disabled] input[type='text'], " +
		"fieldset[disabled] select, " +
		"fieldset[disabled] textarea").each(function (index, input) {
		Europa.AddSubstituteFieldTo(input);
	});
};


Europa.RemoveSubstituteFields = function () {
	$(".replaced-input").remove();
};

Europa.HideSubstituteFields = function () {
	$(".replaced-input").hide();
};

Europa.ShowSubstituteFields = function () {
	$(".replaced-input").show();
};

Europa.IniciarAtendimentoTelefone = function(idUsuario) {
	$.get(Europa.UrlListarPerfisUsuarioLogado,
		null,
		function(res) {
			if (idUsuario != undefined && idUsuario > 0) {
				if (res.IsOperador) {
					$("#form_redirect_atendimento_layout").addHiddenInputData({ id: idUsuario });
					$("#form_redirect_atendimento_layout").submit();
				}
				if (res.IsVendedor) {
					$("#form_redirect_atendimento_venda_layout").addHiddenInputData({ id: idUsuario });
					$("#form_redirect_atendimento_venda_layout").submit();
				}
			}
		});
};

Europa.DisableElement = function (input, timeout) {
	var target = $($(input).get()[0]);
	if (target.is("i")) {
		target = target.parent();
	}
	target.attr("disabled", true);
	setTimeout(function () {
		target.removeAttr("disabled");
	}, timeout != undefined ? timeout : 2000);
};


Europa.Components.DownloadFile = function (url, params) {
    $.ajax({
        type: 'POST',
        url: url,
        data: params,
        xhrFields: {
            responseType: 'blob'
        },
        beforeSend: function () {
            Spinner.Show();
        },
        error: function () {
            Spinner.Hide();
        },
        success: function (result, text, response) {
            var disposition = response.getResponseHeader('Content-Disposition');
            var filename = '';
            var filenameRegex = /filename\*?=['"]?(?:UTF-\d['"]*)?([^;\r\n"']*)['"]?;?/
                , matches = filenameRegex.exec(disposition);

            if (matches !== null && matches[1]) {
                filename = decodeURI(matches[1]);
            }

            var blob = new Blob([result], { type: result.ContentType });
            var downloadUrl = URL.createObjectURL(blob);
            var a = document.createElement("a");
            a.href = downloadUrl;
            a.download = filename;
            document.body.appendChild(a);
            a.click();
            a.remove();
            Spinner.Hide();
        }
    });
}