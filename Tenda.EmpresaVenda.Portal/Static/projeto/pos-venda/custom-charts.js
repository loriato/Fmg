Chart.defaults.PrePropostasFinalizadasDoughnutChart = Chart.defaults.doughnut;

Chart.controllers.PrePropostasFinalizadasDoughnutChart = Chart.controllers.doughnut.extend({
    draw: function (ease) {
        Chart.controllers.doughnut.prototype.draw.call(this, ease);

        var total = 0;
        for (var i = 0; i < this._data.length && i < 3; i++) {
            total += this._data[i];
        }

        if (total <= 0 && this.chart.config.data.datasets[0].data.length === 3) {
            this.chart.config.data.datasets[0].data.push(1);
            if (this.chart.config.data.datasets[0].backgroundColor instanceof Array && this.chart.config.data.datasets[0].backgroundColor.length === 3) {
                this.chart.config.data.datasets[0].backgroundColor.push("#dddddd");
            }
            this.chart.update();
            return;
        } else if (this.chart.config.data.datasets[0].data.length > 3 && total > 0) {
            this.chart.config.data.datasets = [this.chart.config.data.datasets[0]];
        }

        var title = "Total";
        var value = total;
        if (this.chart.selectedIndex !== undefined && this.chart.selectedIndex !== null && this.chart.selectedIndex < this._data.length) {
            var tmpValue = this._data[this.chart.selectedIndex];
            if (tmpValue > 0) {
                value = tmpValue;
                if (this.chart.selectedIndex < this.chart.legend.legendItems.length) {
                    title = this.chart.legend.legendItems[this.chart.selectedIndex].text;
                }
            }
        }

        var progress = value * ease;
        if (this.alreadyAnimated) {
            progress = value;
        }
        progress = Math.round(progress);

        var radius = this.innerRadius;

        var canvas = this.chart.canvas;
        var context = canvas.getContext("2d");

        var width = $(canvas).width();
        var height = $(canvas).height();
        var px = width / 2;
        var py = height / 2;

        var textOffset = -3;
        if (this.chart.options.legend.display) {
            textOffset = 13;
        }

        context.font = "bold 28px AktivGrotesk";
        context.fillStyle = "black";
        context.textAlign = "center";
        context.textBaseline = "middle";
        context.fillText(progress + "", px, py + textOffset, radius * 2 - 10);
        context.font = "normal 12px AktivGrotesk";
        context.fillStyle = "black";
        context.textAlign = "center";
        context.textBaseline = "middle";
        context.fillText(title, px, py + 20 + textOffset, radius * 2 - 10);

        if (ease === 1) {
            this.alreadyAnimated = true;
        }
    }
});