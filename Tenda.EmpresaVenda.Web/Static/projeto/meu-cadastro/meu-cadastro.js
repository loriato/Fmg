$(function () {
    $("#text_indisponivel").hide();
    setTimeout(function () {
        Europa.Mask.Apply($("#EmpresaVenda_CNPJ"), Europa.Mask.FORMAT_CNPJ);
        Europa.Mask.Apply($("#EmpresaVenda_Corretor_CPF"), Europa.Mask.FORMAT_CPF);

        console.log($('#url-foto').val());
        var urlFoto = $('#url-foto').val();

        if (urlFoto !== undefined && urlFoto !== null && urlFoto !== "") {
            $("#img_foto_fachada").attr("src", $('#url-foto').val());
            $("#img_foto_fachada").show();
        } else {
            $("#img_foto_fachada").hide();
            $("#text_indisponivel").show();
         }
        var height = $("#div_form").height();
        $("#div_image").height(height);
    }, 500);
});
