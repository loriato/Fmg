Europa.Components.CircularProgress = function () {
    this.data = [];
    this.animate = true;
    this.clockwise = true;
    this.prefix = "progress_";
    this.targetSuffix = "";
    this.strokeWidth = undefined;
    this.animationProgress = 0;
    this.focusIndex = undefined;
};

Europa.Components.CircularProgress.prototype.WithData = function (data) {
    if (!(data instanceof Array)) {
        data = [data];
    }
    this.data = data;
    return this;
};

Europa.Components.CircularProgress.prototype.SetFocusIndex = function (index) {
    if (this.focusIndex === index) {
        this.focusIndex = undefined;
    } else {
        this.focusIndex = index;
    }
    this.Redraw();
    return this;
};

Europa.Components.CircularProgress.prototype.WithAnimation = function (value) {
    this.animate = value;
    if (value) {
        this.animationProgress = 0;
    } else {
        this.animationProgress = 100;
    }
    return this;
};

Europa.Components.CircularProgress.prototype.WithClockwise = function (value) {
    this.clockwise = value;
    return this;
};

Europa.Components.CircularProgress.prototype.WithTargetSuffix = function (value) {
    this.targetSuffix = value;
    return this;
};

Europa.Components.CircularProgress.prototype.WithStrokeWidth = function (value) {
    this.strokeWidth = value;
    return this;
};

Europa.Components.CircularProgress.prototype.Configure = function () {
    var canvas = this.GetCanvas();

    var self = this;

    $(window).resize(function (e) {
        if (self.animationProgress === 100) {
            self.animate = false;
        }
        self.Redraw();
    });

    if (self.animate) {
        self.animationProgress = 0;
    }

    this.Redraw();
    return this;
};

Europa.Components.CircularProgress.prototype.Redraw = function () {
    var canvas = this.GetCanvas();
    canvas.attr("width", canvas.parent().width());

    var context = canvas[0].getContext("2d");
    context.clearRect(0, 0, canvas.width(), canvas.height());

    this.DrawAnimation();
};

Europa.Components.CircularProgress.prototype.DrawAnimation = function () {
    var canvas = this.GetCanvas();
    var self = this;

    var animate = function () {
        canvas[0].getContext('2d').clearRect(0, 0, canvas.width(), canvas.height());

        self.DrawArc(canvas[0]);
        self.DrawTotal(canvas[0]);

        if (self.animationProgress < 100) {
            self.animationProgress += 3.5;
            if (self.animationProgress > 100) {
                self.animationProgress = 100;
                self.animate = false;
            }
            requestAnimationFrame(animate);
        }
    };
    animate();
};

Europa.Components.CircularProgress.prototype.DrawTotal = function (canvas) {
    var total = 0;
    var self = this;
    var size = this.SmallestSize();
    var context = canvas.getContext('2d');

    var width = $(canvas).width();
    var height = $(canvas).height();
    var px = width / 2;
    var py = height / 2;

    var strokeWidth = 5;
    if (self.strokeWidth) {
        strokeWidth = self.strokeWidth;
    }

    var maxRadius = size / 2 - strokeWidth;

    var radius = maxRadius - (strokeWidth * 1.5 * self.data.length) - 15;
    var maxWidth = radius * 2;

    if (self.focusIndex === undefined || self.focusIndex >= self.data.length) {
        this.data.forEach(function (data) {
            total += data.value;
        });
    } else {
        total = self.data[self.focusIndex].value;
    }

    var currentProgress = total * (self.animationProgress / 100);
    currentProgress = Math.round(currentProgress);

    context.font = "bold 28px AktivGrotesk";
    context.fillStyle = "black";
    context.textAlign = "center";
    context.textBaseline = "middle";
    context.fillText("" + currentProgress, px, py - 1, maxWidth);
    context.font = "12px AktivGrotesk";
    context.fillStyle = "black";
    context.textAlign = "center";
    context.textBaseline = "middle";
    var title = "Total";
    if (self.focusIndex !== undefined && self.focusIndex < self.data.length) {
        title = self.data[self.focusIndex].title;
    }
    context.fillText(title, px, py + 19, maxWidth);
};

Europa.Components.CircularProgress.prototype.DrawArc = function (canvas) {
    var itemCount = 0;
    var self = this;
    var context = canvas.getContext('2d');
    var width = $(canvas).width();
    var height = $(canvas).height();

    var startRad = 1.5 * Math.PI;
    var endRadVar = self.clockwise ? 2 : -2;
    var px = width / 2;
    var py = height / 2;

    var strokeWidth = 5;
    if (self.strokeWidth) {
        strokeWidth = self.strokeWidth;
    }

    this.data.forEach(function (data) {
        var itemId = itemCount++;
        var min = data.minValue;
        if (min === undefined) min = 0;
        var max = data.maxValue;
        if (max === undefined) max = 100;
        var current = data.value;
        if (current === undefined) current = 0;

        var percentage = ((current - min) * 100) / (max - min);
        percentage = Math.round(percentage);

        var maxRadius = self.SmallestSize() / 2 - strokeWidth;

        var radius = maxRadius - (strokeWidth * 1.5 * itemId);

        var color = "#dddddd";

        if (data.color) {
            if (self.focusIndex === itemId || self.focusIndex === undefined || self.focusIndex + 1 > self.data.length) {
                color = data.color;
            }
        }

        var currentProgress = percentage * (self.animationProgress / 100);

        var endRad = (1.5 + (endRadVar * (currentProgress / 100))) * Math.PI;

        context.beginPath();
        context.arc(px, py, radius, startRad, endRad, !self.clockwise);
        context.lineCap = "round";
        context.lineWidth = strokeWidth;
        context.strokeStyle = color;
        context.stroke();
    });
};

Europa.Components.CircularProgress.prototype.SmallestSize = function () {
    var canvas = this.GetCanvas();
    var width = canvas.width();
    var height = canvas.height();
    if (width > height) {
        return height;
    }
    return width;
};


Europa.Components.CircularProgress.prototype.GetCanvas = function () {
    var target = $("#" + this.prefix + this.targetSuffix);
    var canvas;
    if (target.is("canvas")) {
        canvas = target;
    } else {
        var canvasId = this.targetSuffix + "-canvas";
        var a = target.append("<canvas id='" + canvasId + "'></canvas>");
        canvas = target.find("#" + canvasId);
    }
    return canvas;
};