let InfiniteScroll = function () {
    this.loading = false;
    this.currentPage = 0;
    this.pageSize = 0;
    this.loaderId = undefined;
    this.target = undefined;
    this.bottomRef = undefined;
    this.url = undefined;
    this.filterFunction = undefined;
    this.onResponse = undefined;
    this.customContent = undefined;
    this.canLoadMore = true;
    this.callback = undefined;
    this.skipFirstPage = false;
    this.scrollElement = undefined;
    this.order = undefined;
};

InfiniteScroll.prototype = {
    WithPageSize: function (value) {
        this.pageSize = value;
        return this;
    },
    WithOrder: function (value) {
        this.order = value;
        return this;
    },
    WithLoaderId: function (value) {
        this.loaderId = value;
        return this;
    },
    WithScrollElement: function (value) {
        this.scrollElement = value;
        return this;
    },
    WithTarget: function (value) {
        this.target = value;
        return this;
    },
    WithBottomRef: function (value) {
        this.bottomRef = value;
        return this;
    },
    WithUrl: function (value) {
        this.url = value;
        return this;
    },
    WithFilterFunction: function (value) {
        this.filterFunction = value;
        return this;
    },
    WithOnResponse: function (value) {
        this.onResponse = value;
        return this;
    },
    WithCallBack: function (callback) {
        this.callback = callback;
        return this;
    },
    SkipFirstPage: function () {
        this.skipFirstPage = true;
        return this;
    },
    WithCustomContent: function (value) {
        this.customContent = value;
        return this;
    },

    Complete: function () {
        let instance = this;
        if (!instance.skipFirstPage) {
            instance.Request();
        }
        let scroll = window;
        if (instance.scrollElement) {
            scroll = instance.scrollElement;
        }
        $(scroll).off('scroll');
        $(scroll).scroll(function () {
            if (!instance.loading && $(instance.bottomRef).isInViewport()) {
                let hT = $(instance.bottomRef).offset().top,
                    hH = $(instance.bottomRef).outerHeight(),
                    wH = $(window).height(),
                    wS = $(window).scrollTop();
                if (wS >= Math.floor(hT + hH - wH)) {
                    instance.NextPage();
                }
            }
        });
        return instance;
    },

    Reload: function () {
        this.currentPage = 0;
        this.canLoadMore = true;
        this.Request();
    },
    GoToPage: function (page) {
        this.currentPage = page;
        this.Request();
    },
    NextPage: function () {
        if (this.canLoadMore) {
            this.GoToPage(this.currentPage + 1);
        }
    },
    Request: function () {
        let instance = this;

        instance.loading = true;
        $(instance.loaderId).show();

        if (instance.currentPage === 0) {
            $(instance.target).empty();
        }

        let filter = instance.filterFunction();
        filter["pageSize"] = instance.pageSize;
        filter["start"] = instance.pageSize * instance.currentPage;
        filter["order"] = instance.order ? [{value: "asc", column: instance.order}] : [];
        $.post(instance.url, filter, function (res) {
            if (instance.onResponse !== undefined) {
                instance.onResponse(res, filter);
            }
            if (instance.customContent) {
                instance.customContent(instance, res);
            } else {
                if (instance.currentPage === 0) {
                    $(instance.target).html(res.Content);
                } else {
                    $(instance.target).append(res.Content);
                }
            }
            if (res.NumElementReturned < instance.pageSize) {
                instance.canLoadMore = false;
            }
            $(instance.loaderId).hide();
            instance.loading = false;
            if (instance.callback) {
                instance.callback(instance.canLoadMore);
            }
        });
    }
};