(function () {
    Europa.AjaxConfigure();

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

    //bsCustomFileInput.init()

    Europa.CheckMobile = function () {
        return /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent);
    }
})();

