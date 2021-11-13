var europaApp = angular.module('europaApp', []);
var DataTableApp = angular.module('europaApp', ['datatables', 'datatables.select', 'datatables.columnfilter', 'datatables.bootstrap']);

DataTableApp.config(function ($controllerProvider, $provide, $compileProvider) {
      DataTableApp.controller = function (name, constructor) {
          $controllerProvider.register(name, constructor);
          return (this);
      };
  });

EuropaCompileAngularControllers = function(selector, html, callback, params) {
    var rootElement = angular.element(document);
    rootElement.ready(function () {
        rootElement.injector().invoke(function ($compile) {
            var $content = $(selector),
                scope = $content.scope();

            $content.html(html);
            $compile($content.contents())(scope);
            scope.$digest();
            if (callback != undefined) {
                if (params === undefined) {
                    callback();
                } else {
                    callback(params);
                }
            }  
        });
    });
}