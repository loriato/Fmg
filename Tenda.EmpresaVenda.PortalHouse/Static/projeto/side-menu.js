Europa.Components.SideMenu = {}

$(function () {
    $("#not-side-menu").on("click", function (e) {
        if (!e.target.classList.contains("open-side-menu")) {
            $("#side-menu").removeClass("opened");
        }
    });
})

Europa.Components.SideMenu.OpenNav = function () {
    $("#side-menu").addClass("opened");
};

Europa.Components.SideMenu.CloseNav = function () {
  $("#side-menu").removeClass("opened");
}