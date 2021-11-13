"use strict"

Europa.Mask = {};
Europa.Mask.Behavior = {};
Europa.Mask.Options = {};
Europa.Mask.FORMAT_CEP = "00000-000";
Europa.Mask.FORMAT_CPF = "000.000.000-00";
Europa.Mask.FORMAT_CNPJ = "00.000.000/0000-00";
Europa.Mask.FORMAT_TELEFONE_9 = "(00) 00000-0000";
Europa.Mask.FORMAT_TELEFONE_8 = "(00) 0000-0000";
Europa.Mask.FORMAT_MONEY = "#.##0,00";
Europa.Mask.FORMAT_DECIMAL = "##0,00";
Europa.Mask.FORMAT_DATE = "00/00/0000";
Europa.Mask.FORMAT_INTEIRO = "0000000";
Europa.Mask.FORMAT_INT = "######";

Europa.Mask.Telefone = function (input, dontClear) {
	$(input).unmask().mask(Europa.Mask.Behavior.Telefones, Europa.Mask.Options.Telefones(dontClear));
};

Europa.Mask.Dinheiro = function (input) {
	$(input).unmask().mask(Europa.Mask.FORMAT_MONEY, { reverse: true });
};

Europa.Mask.Decimal = function (input) {
	$(input).unmask().mask(Europa.Mask.FORMAT_DECIMAL, { reverse: true });
};

Europa.Mask.CpfCnpj = function (input, clearIfNotMatch) {
	$(input).unmask().mask(Europa.Mask.Behavior.CpfCnpj, Europa.Mask.Options.CpfCnpj(clearIfNotMatch));
};

Europa.Mask.Int = function (input) {
	$(input).unmask().mask(Europa.Mask.FORMAT_INT, { reverse: true });
};

Europa.Mask.Apply = function (input, mask, dontClear) {
	$(input).unmask().mask(mask, { clearIfNotMatch: dontClear == true ? false : true });
};

Europa.Mask.ApplyWithOptions = function (input, mask, options) {
	$(input).unmask().mask(mask, options);
};

Europa.Mask.ApplyByClass = function (classe, mask, clearIfNotMatch, reverse) {
    var options = {};
    if (clearIfNotMatch != undefined)
        options.clearIfNotMatch = clearIfNotMatch;
    if (reverse != undefined)
        options.reverse = reverse;
    $("." + classe).unmask().mask(mask, options);
}

Europa.Mask.GetMaskedValue = function (val, mask) {
	if (mask instanceof Function) {
		mask = mask(val);
	}
	return $('<div></div>').mask(mask).masked(val);
};

Europa.Mask.Behavior.Telefones = function (val) {
	return val.replace(/\D/g, "").length === 11 ? Europa.Mask.FORMAT_TELEFONE_9 : Europa.Mask.FORMAT_TELEFONE_8 + '9';
};

Europa.Mask.Behavior.CpfCnpj = function (val) {
	return val.replace(/\D/g, "").length > 11 ? Europa.Mask.FORMAT_CNPJ : Europa.Mask.FORMAT_CPF + '9';
};

Europa.Mask.Options.CpfCnpj = function (clearIfNotMatch) {
	return {
		onKeyPress: function (val, e, field, options) {
			field.mask(Europa.Mask.Behavior.CpfCnpj(val), options);
		},
		clearIfNotMatch: clearIfNotMatch
	};
};

Europa.Mask.Options.Telefones = function (dontClear) {
	return {
		onKeyPress: function (val, e, field, options) {
			field.mask(Europa.Mask.Behavior.Telefones.apply({}, arguments), options);
		},
		clearIfNotMatch: dontClear == true ? false : true
	};
};

Europa.Mask.Options.CompletaZeroEsquerda = function (input, qtdZero) {
	qtdZero = qtdZero ? qtdZero : 0; ///se não informar qtdZero então não coloca zero a esquerda
	var zero = '';
	for (var i = 1; i <= qtdZero; i++) { zero += '0'; }
	return input.value = (zero + input.value).slice(-qtdZero);
};

Europa.Mask.Options.RemoveZeroEsquerda = function (input) {
	if (parseInt(input.value, 10) > 9)
		return input.value = parseInt(input.value, 10);

}
