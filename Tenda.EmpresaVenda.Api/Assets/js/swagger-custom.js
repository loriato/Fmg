$(document).ready(function () {
	desabilitarClickLogo();
	realocarTitulo();
});


function realocarTitulo() {
	var title = $('.info_title');
	title.detach();
	title.css({ 'align': 'center' });
	$('#header').find('.swagger-ui-wrap').append(title);
}

function desabilitarClickLogo() {
	$('a#logo').attr('href', '#');
}