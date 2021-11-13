Europa.Controllers.Login = {
    Selectors: {
        ImgMostrarSenha: "#mostrar-senha",
        ImgEsconderSenha: "#esconder-senha",
        BtnSenha: "#btn-mostrar-senha",
        BtnEntrar: "#btn-entrar",
        BtnBack: "#btn-back",
        Form: "#login-form",
        LoginHasValue: false,
        PasswordHasValue: false
    }
};

$(function () {
    window.addEventListener('load', function () {
        let forms = document.getElementsByClassName('needs-validation');
        let validation = Array.prototype.filter.call(forms, function (form) {
            form.addEventListener('submit', function (event) {
                if (form.checkValidity() === false) {
                    event.preventDefault();
                    event.stopPropagation();
                } else {
                    Spinner.Show();
                }
                form.classList.add('was-validated');
                $(".server-validation", form).css("display", "none");

            }, false);
        });
        Europa.Controllers.Login.ValidarUsername();
        Europa.Controllers.Login.ValidarPassword();
        Europa.Controllers.Login.ToggleLoginButton();
    }, false);


    $(Europa.Controllers.Login.Selectors.BtnSenha).on("click", function () {
        $(Europa.Controllers.Login.Selectors.ImgMostrarSenha).toggle();
        $(Europa.Controllers.Login.Selectors.ImgEsconderSenha).toggle();
        let input = document.getElementById("password");
        if (input.type === "password") {
            input.type = "text";
        } else {
            input.type = "password";
        }
    });


    $(Europa.Controllers.Login.Selectors.BtnBack).on("click", function () {
        $("#box-message").hide();
        $("#box-login").show();
    });

    $("#username").keyup(function () {
        Europa.Controllers.Login.ValidarUsername();
        Europa.Controllers.Login.DisableErrorClass();
        Europa.Controllers.Login.ToggleLoginButton();
    });

    $("#password").keyup(function () {
        Europa.Controllers.Login.ValidarPassword();
        Europa.Controllers.Login.DisableErrorClass();
        Europa.Controllers.Login.ToggleLoginButton();
    });
});

Europa.Controllers.Login.ToggleLoginButton = function () {
    if (Europa.Controllers.Login.Selectors.PasswordHasValue && Europa.Controllers.Login.Selectors.LoginHasValue) {
        $("#btn-entrar").removeAttr("disabled");
    }
    else {
        $("#btn-entrar").attr("disabled", "disabled");
    }
}

Europa.Controllers.Login.ValidarUsername = function () {
    if ($("#username").val().length > 0 || $("input#username:-webkit-autofill").length > 0) {
        $("#icon-mail").addClass("blue");
        Europa.Controllers.Login.Selectors.LoginHasValue = true;
    } else {
        $("#icon-mail").removeClass("blue");
        Europa.Controllers.Login.Selectors.LoginHasValue = false;
    }
}

Europa.Controllers.Login.ValidarPassword = function () {
    if ($("#password").val().length > 0 || $("input#password:-webkit-autofill").length > 0) {
        $("#group-password").find("svg").each(function () {
            $(this).addClass("blue");
        })
        Europa.Controllers.Login.Selectors.PasswordHasValue = true;
    } else {
        $("#group-password").find("svg").each(function () {
            $(this).removeClass("blue");
        })
        Europa.Controllers.Login.Selectors.PasswordHasValue = false;
    }
}

Europa.Controllers.Login.DisableErrorClass = function () {
    let div = document.getElementsByClassName("input-error");
    if (div.length > 0) {
        div[0].classList.remove("input-error");
    }
}