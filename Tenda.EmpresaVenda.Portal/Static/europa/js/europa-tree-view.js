"use strict";

/*
    Para futuras implementações acesse:   
    https://github.com/jonmiles/bootstrap-treeview
*/

Europa.Components.TreeView = function (id) {
    this.id = id;
    this.selectorId = "#" + id;
    this.data = [];
    this.expandIcon = "fa fa-plus";
    this.collapseIcon = "fa fa-minus";
    this.multiSelect = false;
    this.showCheckbox = false;
    this.checkedIcon = "fa fa-check-square-o";
    this.uncheckedIcon = "fa fa-square-o";
    this.emptyIcon = "";
    this.sendRequest = true;
    this.level = undefined;
    this.checkRootSiblings = false;
    this.selectRootSiblings = false;
    this.rowCheck = false;

    this.requestUrl = undefined;
    this.requestMethod = undefined;
    this.requestParams = undefined;

    this.selectCallBack = undefined;

    this.waitingCheckRender = false;
};

Europa.Components.TreeView.prototype = {
    WithAjax: function (method, url, params) {
        this.requestUrl = url;
        this.requestMethod = method;
        this.requestParams = params;
        return this;
    },
    WithAutoInit: function (autoInit) {
        this.sendRequest = autoInit;
        return this;
    },
    WithData: function (data) {
        this.data = data;
        return this;
    },
    WithLevel: function (level) {
        this.level = level;
        return this;
    },
    WithCheckRootSiblings: function (check) {
        this.checkRootSiblings = check;
        return this;
    },
    WithSelectRootSiblings: function (select) {
        this.selectRootSiblings = select;
        return this;
    },
    WithExpandIcon: function (expandIcon) {
        this.expandIcon = expandIcon;
        return this;
    },
    WithCollapseIcon: function (collapseIcon) {
        this.collapseIcon = collapseIcon;
        return this;
    },
    WithCheckedIcon: function (checkedIcon) {
        this.checkedIcon = checkedIcon;
        return this;
    },
    WithUncheckedIcon: function (uncheckedIcon) {
        this.checkedIcon = uncheckedIcon;
        return this;
    },
    WithEmptyIcon: function (emptyIcon) {
        this.emptyIcon = emptyIcon;
        return this;
    },
    WithMultiSelect: function (multi) {
        this.multiSelect = multi;
        return this;
    },
    WithShowCheckbox: function (show) {
        this.showCheckbox = show;
        return this;
    },
    WithRowCheck: function (rowCheck) {
        this.rowCheck = rowCheck;
        return this;
    },
    GetData: function () {
        return this.data;
    },
    GetNode: function (nodeId) {
        return $(this.selectorId).treeview('getNode', nodeId);
    },
    GetParentNode: function (nodeId) {
        return $(this.selectorId).treeview('getParent', nodeId);
    },
    GetSiblingsNodes: function (nodeId) {
        return $(this.selectorId).treeview('getSiblings', nodeId);
    },
    SelectNode: function (nodeId, silent) {
        $(this.selectorId).treeview('selectNode', [nodeId, { silent: silent }]);
    },
    UnselectNode: function (nodeId, silent) {
        $(this.selectorId).treeview('unselectNode', [nodeId, { silent: silent }]);
    },
    GetSelectedNodes: function () {
        return $(this.selectorId).treeview('getSelected');
    },
    GetUnselectedNodes: function (nodeId) {
        return $(this.selectorId).treeview('getUnselected', nodeId);
    },
    GetCheckedNodes: function () {
        return $(this.selectorId).treeview('getChecked');
    },
    CheckAllNodes: function (silent) {
        $(this.selectorId).treeview('checkAll', { silent: silent });
    },
    UncheckAllNodes: function (silent) {
        $(this.selectorId).treeview('uncheckAll', { silent: silent });
    },
    CheckNode: function (nodeId, silent) {
        $(this.selectorId).treeview('checkNode', [nodeId, { silent: silent }]);
    },
    UncheckNode: function (nodeId, silent) {
        $(this.selectorId).treeview('uncheckNode', [nodeId, { silent: silent }]);
    },
    DisableAllNodes: function (silent) {
        $(this.selectorId).treeview('disableAll', { silent: silent });
    },
    DisableNode: function (nodeId, silent) {
        $(this.selectorId).treeview('disableNode', [nodeId, { silent: silent }]);
    },
    EnableAllNodes: function (silent) {
        $(this.selectorId).treeview('enableAll', { silent: silent });
    },
    EnableNode: function (nodeId, silent) {
        $(this.selectorId).treeview('enableNode', [nodeId, { silent: silent }]);
    },
    ExpandAllNodes: function (levels, silent) {
        $(this.selectorId).treeview('expandAll', { levels: levels, silent: silent });
    },
    CollapseAllNodes: function (silent) {
        $(this.selectorId).treeview('collapseAll', { silent: silent });
    },
    CollapseNode: function (nodeId, ignoreChildren, silent) {
        $(this.selectorId).treeview('collapseNode', [nodeId, { silent: silent, ignoreChildren: ignoreChildren }]);
    },
    ExpandNode: function (nodeId, levels, silent) {
        $(this.selectorId).treeview('expandNode', [nodeId, { levels: levels, silent: silent }]);
    },

    Configure: function () {
        var self = this;
        if (this.requestUrl) {
            var data = {};
            if (self.requestParams !== undefined) {
                var params = self.requestParams();
                for (var key in params) {
                    if (params.hasOwnProperty(key)) {
                        data[key] = params[key];
                    }
                }
            }

            $.ajax({
                url: self.requestUrl,
                type: self.requestMethod,
                data: data,
                success: function (result) {
                    self.data = result;
                    self.Complete();
                }
            });
        } else {
            self.Complete();
        }
    },
    Complete: function () {
        var self = this;
        $(this.selectorId).treeview({
            data: self.data,
            level: self.level,
            expandIcon: self.expandIcon,
            collapseIcon: self.collapseIcon,
            multiSelect: self.multiSelect,
            showCheckbox: self.showCheckbox,
            checkedIcon: self.checkedIcon,
            uncheckedIcon: self.uncheckedIcon,
            emptyIcon: self.emptyIcon
        });

        if (self.rowCheck) {
            $(this.selectorId).on('nodeSelected nodeUnselected', function (event, node) {
                self.UnselectNode(node.nodeId, false);
                if (node.state.checked) {
                    self.UncheckNode(node.nodeId, false);
                } else {
                    self.CheckNode(node.nodeId, false);
                }
            });
        }

        if (self.checkRootSiblings) {
            var onCheckedUnchecked = function (event, node) {
                if (self.waitingCheckRender === false && node && node.nodes) {
                    $(self.selectorId).off('nodeChecked nodeUnchecked');
                    self.waitingCheckRender = true;
                    var getIds = function(node){
                        var ids = [node.nodeId];
                        var children = node.nodes;
                        if(children){
                            for (var i = 0; i < children.length; i++) {
                                if(children[i].selectable != "false"){
                                    var cids = getIds(children[i]);
                                    ids = ids.concat(cids);
                                }
                            }
                        }
                        return ids;
                    };
                    var ids = getIds(node);
                    if (event.type == "nodeChecked") {
                        $(self.selectorId).treeview('checkNode', [ids, { silent: self.silent }]);
                    } else {
                        $(self.selectorId).treeview('uncheckNode', [ids, { silent: self.silent }]);
                    }
                    self.waitingCheckRender = false;
                    $(self.selectorId).on('nodeChecked nodeUnchecked', onCheckedUnchecked);
                }
            };
            $(this.selectorId).on('nodeChecked nodeUnchecked', onCheckedUnchecked);
        }

        if (self.selectRootSiblings) {
            $(this.selectorId).on('nodeSelected nodeUnselected ', function (event, node) {
                if (node && node.nodes) {
                    var children = node.nodes;
                    for (var i = 0; i < children.length; i++) {
                        if (event.type == "nodeSelected") {
                            self.SelectNode(children[i].nodeId, false);
                        } else {
                            self.UnselectNode(children[i].nodeId, false);
                        }
                    }
                }
            });
        }
    },
    GetCheckedNodesByLevel: function (level) {
        var checkedNodes = this.GetCheckedNodes();
        var filteredCheckedNodes = [];

        if (checkedNodes != null || checkedNodes != undefined) {
            checkedNodes.forEach(function (obj) {
                if (obj.nodeLevel == level) {
                    filteredCheckedNodes.push(obj);
                }
            });
        }

        return filteredCheckedNodes;
    }
};
