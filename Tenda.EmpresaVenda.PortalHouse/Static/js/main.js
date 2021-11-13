(function () {
    Europa.AjaxConfigure();
    if ($.fn.select2) {
        $.fn.select2.defaults.set("language", "pt-BR");
    }
    /**
     * @return {boolean}
     */
    Europa.Form.Valid = function (formId) {
        let formSelector = Europa.String.RemoveHashtag(formId);
        let form = document.getElementById(formSelector);
        form.classList.add('was-validated');
        $(".server-validation", form).css("display", "none");
        return form.checkValidity();
    };

    $(document).on('hidden.bs.modal', function () {
        if ($('.modal.show').length) {
            $('body').addClass('modal-open');
        }
    });



})();
  
