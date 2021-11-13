using Europa.Web;
using System.Web.Optimization;

namespace Tenda.EmpresaVenda.PortalHouse
{
    public class BundleConfig
    {
        // Para obter mais informações sobre o agrupamento, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                 "~/node_modules/jquery/dist/jquery.min.js",
                 "~/node_modules/jquery-mask-plugin/dist/jquery.mask.min.js",
                 "~/node_modules/jquery-form/dist/jquery.form.min.js",
                 "~/node_modules/jquery-maskmoney/dist/jquery.maskMoney.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/node_modules/jquery-validation/dist/jquery.validate.min.js",
                "~/node_modules/jquery-validation-unobtrusive/dist/jquery.validate.unobstrusive.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/node_modules/bootstrap/dist/js/bootstrap.bundle.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/slick-carousel").Include(
                "~/node_modules/slick-carousel/slick/slick.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/europa").Include(
                "~/static/europa/js/europa-app.js",
                "~/static/europa/js/europa-mask.js",
                "~/static/europa/js/europa-form.js",
                "~/static/europa/js/europa-infinite-scroll.js",
                "~/static/europa/js/europa-dialogo.js",
                "~/static/europa/js/europa-commons.js",
                "~/static/europa/js/europa-autocomplete.js",
                "~/static/europa/js/europa-date.js",
                "~/static/europa/js/europa-datepicker.js",
                "~/static/europa/js/europa-exception-handler.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/moment").Include(
                "~/node_modules/moment/min/moment-with-locales.min.js"));

            bundles.Add(new StyleBundle("~/css/bootstrap").Include(
                "~/node_modules/bootstrap/dist/css/bootstrap.min.css"));

            bundles.Add(new StyleBundle("~/css/select2").Include(
                "~/node_modules/select2/dist/css/select2.css"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-daterangepicker").Include(
                "~/node_modules/bootstrap-daterangepicker/daterangepicker.js"));

            bundles.Add(new StyleBundle("~/css/bootstrap-daterangepicker").Include(
                "~/node_modules/bootstrap-daterangepicker/daterangepicker.css"));

            bundles.Add(new StyleBundle("~/css/slick-carousel").Include(
                "~/node_modules/slick-carousel/slick/slick.css",
                "~/node_modules/slick-carousel/slick/slick-theme.css"));

            bundles.Add(new ScriptBundle("~/bundles/select2").Include(
                "~/node_modules/select2/dist/js/select2.js",
                "~/node_modules/select2/dist/js/i18n/pt-BR.js"));

            var angularBundle = new Bundle("~/bundles/angular").Include(
                        "~/node_modules/angular/angular.min.js",
                        "~/static/europa/js/europa-angular-app.js");
            bundles.Add(angularBundle);

            var angularDatatableBunde = new Bundle("~/bundles/angular-datatable")
                    .Include("~/node_modules/angular-datatables/vendor/datatables/media/js/jquery.dataTables.js",
                    "~/node_modules/angular-datatables/dist/angular-datatables.js",
                    "~/node_modules/angular-datatables/vendor/datatables-select/js/dataTables.select.js",
                    "~/node_modules/angular-datatables/src/plugins/select/angular-datatables.select.js",
                    "~/node_modules/angular-datatables/vendor/datatables-columnfilter/js/dataTables.columnFilter.js",
                    "~/node_modules/angular-datatables/src/plugins/columnfilter/angular-datatables.columnfilter.js",
                    "~/node_modules/angular-datatables/dist/plugins/bootstrap/angular-datatables.bootstrap.min.js",
                    "~/node_modules/datatables.net-rowreorder/js/dataTables.rowReorder.min.js",
                    "~/static/europa/js/europa-angular-datatable.js");

            bundles.Add(angularDatatableBunde);

            //bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include(
            //    "~/static/jquery-ui/js/jquery-ui.min.js"));

            bundles.Add(new StyleBundle("~/css/angular-datatable").Include(
                "~/node_modules/angular-datatables/vendor/datatables/media/css/jquery.dataTables.min.css",
                "~/node_modules/angular-datatables/vendor/datatables/media/css/angular-datatables.css",
                "~/node_modules/angular-datatables/vendor/datatables-select/css/select.dataTables.css",
                "~/static/europa/css/rowReorder.dataTables.min.css",
                "~/static/europa/css/europa-angular-datatable.css",
                "~/static/europa/css/europa-bootstrap-nav-vertical.css"
                ));

            bundles.Add(new Bundle("~/css/font-awesome").Include(
                "~/node_modules/@fortawesome/fontawesome-free/css/all.min.css", new CssRewriteUrlTransformWrapper()
            ));
        }
    }
}
