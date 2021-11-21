using Europa.Web;
using System.Web.Optimization;

namespace Europa.Fmg.Portal.App_Start
{
    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //  "~/node_modules/jquery/dist/jquery.min.js",
            //  "~/node_modules/jquery-mask-plugin/dist/jquery.mask.min.js",
            //  "~/node_modules/jquery-form/dist/jquery.form.min.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //    "~/node_modules/jquery-validation/dist/jquery.validate.min.js",
            //    "~/node_modules/jquery-validation-unobtrusive/dist/jquery.validate.unobstrusive.min.js"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //   "~/node_modules/bootstrap/dist/js/bootstrap.bundle.min.js",
            //   "~/node_modules/bs-custom-file-input/dist/bs-custom-file-input.js"));

            //bundles.Add(new StyleBundle("~/css/bootstrap").Include(
            //                "~/node_modules/bootstrap/dist/css/bootstrap.min.css"));


            ////bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            ////           "~/static/scripts/jquery-3.1.1.min.js",
            ////           "~/static/scripts/jquery.mask.min.js",
            ////           "~/static/scripts/jquery.form.min.js"));

            ////bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            ////            "~/static/scripts/jquery.validate.min.js",
            ////            "~/static/scripts/jquery.validate.unobstrusive.min.js"));

            //bundles.Add(new ScriptBundle("~/bundles/moment").Include(
            //           "~/static/scripts/moment-with-locates.js"));

            //// Use the development version of Modernizr to develop with and learn from. Then, when you're
            //// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/static/scripts/modernizr-2.8.3.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryNumeric").Include(
            //            "~/static/scripts/jquery.numeric.min.js"));

            //bundles.Add(new ScriptBundle("~/bundles/pdfjs")
            //    .Include("~/static/pdfjs/build/pdf.js",
            //             "~/static/pdfjs/build/pdf.worker.js"));

            //bundles.Add(new ScriptBundle("~/bundles/pdfobject")
            //    .Include("~/static/PDFObject/pdfobject.js"));

            ////bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            ////            "~/static/scripts/bootstrap.js",
            ////            "~/static/scripts/respond.js"));

            ////bundles.Add(new ScriptBundle("~/bundles/bootstrap-daterangepicker").Include(
            ////           "~/static/bootstrap-daterangepicker/js/daterangepicker.js"));

            ////bundles.Add(new ScriptBundle("~/bundles/bootstrap-validator").Include(
            ////          "~/static/bootstrap-validator/validator.min.js"));

            ////bundles.Add(new ScriptBundle("~/bundles/bootstrap-treeview")
            ////    .Include("~/static/bootstrap-treeview/js/bootstrap-treeview.min.js"));

            ////repare que aqui temos um Bundle e não um ScriptBundle. Foi decidigo não minificar por contas de erro do angular que a minificação gerava
            //var angularBundle = new Bundle("~/bundles/angular").Include(
            //            "~/static/angular/angular.min.js",
            //            "~/static/europa/js/europa-angular-app.js");
            //bundles.Add(angularBundle);

            //bundles.Add(new ScriptBundle("~/bundles/select2").Include(
            //          "~/static/select2/js/select2.full.min.js",
            //          "~/static/select2/js/i18n/pt-BR.js"));

            //bundles.Add(new StyleBundle("~/css/fullcalendar")
            //    .Include("~/static/fullcalendar/css/fullcalendar.min.css"));

            //bundles.Add(new ScriptBundle("~/bundles/fullcalendar")
            //    .Include("~/static/fullcalendar/js/fullcalendar.min.js")
            //    .Include("~/static/fullcalendar/js/locale-all.js")
            //);

            //// Slick
            //bundles.Add(new StyleBundle("~/css/slick")
            //   .Include("~/static/slick/css/slick.css",
            //            "~/static/slick/css/slick-theme.css"));
            //bundles.Add(new ScriptBundle("~/bundles/slick")
            //    .Include("~/static/slick/js/slick.js"));

            //// Notify
            //bundles.Add(new StyleBundle("~/css/notify")
            //    .Include("~/static/bootstrap-notify/css/animate.css"));

            //bundles.Add(new ScriptBundle("~/bundles/notify")
            //    .Include("~/static/bootstrap-notify/js/bootstrap-notify.min.js"));

            ////repare que aqui temos um Bundle e não um ScriptBundle. Foi decidido não minificar por contas de erro do angular que a minificação gerava
            //var angularDatatableBunde = new Bundle("~/bundles/angular-datatable")
            //        .Include("~/static/angular-datatable/js/jquery.dataTables.js",
            //        "~/static/angular-datatable/js/angular-datatables.js",
            //        "~/static/angular-datatable/js/dataTables.select.min.js",
            //        "~/static/angular-datatable/js/angular-datatables.select.js",
            //        "~/static/angular-datatable/js/dataTables.columnFilter.js",
            //        "~/static/angular-datatable/js/angular-datatables.columnfilter.js",
            //        "~/static/angular-datatable/js/angular-datatables.bootstrap.min.js",
            //        "~/static/angular-datatable/js/dataTables.rowReorder.min.js",
            //        "~/static/europa/js/europa-angular-datatable.js");
            //bundles.Add(angularDatatableBunde);

            //bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include(
            //    "~/static/jquery-ui/js/jquery-ui.min.js"));

            //bundles.Add(new StyleBundle("~/css/angular-datatable").Include(
            //            "~/static/angular-datatable/css/jquery.dataTables.min.css",
            //            "~/static/angular-datatable/css/angular-datatables.css",
            //            "~/static/angular-datatable/css/select.dataTables.min.css",
            //            "~/static/angular-datatable/css/rowReorder.dataTables.min.css"));

            //bundles.Add(new StyleBundle("~/css/jquery-ui").Include(
            //    "~/static/jquery-ui/css/jquery-ui.min.css"));

            //// Não está sendo utilizando europa-* pois alguns arquivos ref angular foram colocados em bundles separados
            //bundles.Add(new ScriptBundle("~/bundles/europa").Include(
            //             "~/static/europa/js/europa-app.js",
            //             "~/static/europa/js/europa-autocomplete.js",
            //             "~/static/europa/js/europa-commons.js",
            //             "~/static/europa/js/europa-date.js",
            //             "~/static/europa/js/europa-datepicker.js",
            //             "~/static/europa/js/europa-dialogo.js",
            //             "~/static/europa/js/europa-exception-handler.js",
            //             "~/static/europa/js/europa-form.js",
            //             "~/static/europa/js/europa-mask.js",
            //             "~/static/europa/js/europa-modal.js",
            //             "~/static/europa/js/europa-order-list.js",
            //             "~/static/europa/js/europa-pdfjs-wrapper.js",
            //             "~/static/europa/js/europa-scheduler.js",
            //             "~/static/europa/js/europa-signalr-wrapper.js",
            //             "~/static/europa/js/europa-tree-view.js",
            //             "~/static/europa/js/europa-validator.js"));

            ////Cascade Style Sheets

            //bundles.Add(new StyleBundle("~/css/bootstrap-daterangepicker").Include(
            //          "~/static/bootstrap-daterangepicker/css/daterangepicker.css"));

            //bundles.Add(new Bundle("~/css/font-awesome")
            //    .Include("~/static/font-awesome/css/font-awesome.min.css", new CssRewriteUrlTransformWrapper())
            //    .Include("~/static/font-awesome/css/font-awesome-animation.min.css"));

            //bundles.Add(new StyleBundle("~/css/europa").Include(
            //          "~/static/europa/css/europa-angular-datatable.css",
            //          "~/static/europa/css/europa-bootstrap.css",
            //          "~/static/europa/css/europa-main.css",
            //          "~/static/europa/css/europa-select2.css"));

            //bundles.Add(new StyleBundle("~/css/select2").Include("~/static/select2/css/select2.min.css",
            //          "~/static/select2/css/select2-bootstrap.css"));

            //bundles.Add(new ScriptBundle("~/bundles/tinymce")
            //    .Include("~/static/tinymce/tinymce.min.js",
            //        "~/static/tinymce/jquery.tinymce.min.js",
            //        "~/static/tinymce/themes/modern/theme.min.js",
            //        "~/static/tinymce/plugins/lists/plugin.min.js",
            //        "~/static/tinymce/plugins/advlist/plugin.min.js",
            //        "~/static/tinymce/plugins/link/plugin.min.js",
            //        "~/static/tinymce/plugins/image/plugin.min.js",
            //        "~/static/tinymce/langs/pt_BR.js"
            //        ));

            //bundles.Add(new StyleBundle("~/css/tinymce")
            //    .Include("~/static/tinymce/skins/lightgray/skin.min.css"));

            //bundles.Add(new StyleBundle("~/css/bootstrap-treeview")
            //    .Include("~/static/bootstrap-treeview/css/bootstrap-treeview.min.css"));

            //bundles.Add(new StyleBundle("~/css/chartjs")
            //    .Include("~/static/chartjs/chart.css"));

            //bundles.Add(new ScriptBundle("~/bundles/chartjs")
            //    .Include("~/static/chartjs/chart.js"));


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