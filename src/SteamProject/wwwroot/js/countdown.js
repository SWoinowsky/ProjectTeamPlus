var Ringer = function (element, countdown_to, competition_start_date) {
    this.countdown_to = countdown_to;
    this.competition_start_date = competition_start_date;
    this.element = document.getElementById(element);

    var competitionLength = Math.ceil((new Date(this.countdown_to).getTime() - new Date(this.competition_start_date).getTime()) / 86400000);

    this.rings = {
        'DAYS': {
            s: 86400000, // mseconds in a day,
            max: competitionLength
        },
        'HOURS': {
            s: 3600000, // mseconds per hour,
            max: 24
        },
        'MINUTES': {
            s: 60000, // mseconds per minute
            max: 60
        },
        'SECONDS': {
            s: 1000,
            max: 60
        },
    };
    this.r_count = 4;
    this.r_spacing = 25; // px
    this.r_size = 100; // px
    this.r_thickness = 10; // px
    this.update_interval = 11; // ms
};

Ringer.prototype.init = function () {
    this.ctx = this.element.getContext('2d');
    this.size = {
        w: (this.r_size + this.r_thickness) * this.r_count + (this.r_spacing * (this.r_count - 1)),
        h: (this.r_size + this.r_thickness)
    };
    this.element.setAttribute('width', this.size.w);
    this.element.setAttribute('height', this.size.h);
    this.ctx.textAlign = 'center';
    this.actual_size = this.r_size + this.r_thickness;
    this.countdown_to_time = new Date(this.countdown_to).getTime();
    this.element.style.cssText = "width: " + this.size.w + "px; height: " + this.size.h + "px";
    this.go();
};

Ringer.prototype.go = function () {
    var idx = 0;
    this.time = (new Date().getTime()) - this.countdown_to_time;
    for (var r_key in this.rings) this.unit(idx++, r_key, this.rings[r_key]);
    setTimeout(this.go.bind(this), this.update_interval);
};

Ringer.prototype.unit = function (idx, label, ring) {
    var x, y, value, ring_secs = ring.s;
    value = parseFloat(this.time / ring_secs);
    this.time -= Math.round(parseInt(value)) * ring_secs;
    value = Math.abs(value);

    x = (this.r_size * 0.5 + this.r_thickness * 0.5);
    x += +(idx * (this.r_size + this.r_spacing + this.r_thickness));
    y = this.r_size * 0.5;
    y += this.r_thickness * 0.5;

    // calculate arc angles
    var degrees = (value / ring.max) * 360.0;
    var angle = degrees * (Math.PI / 180);
    var startAngle, endAngle;

    startAngle = -0.5 * Math.PI; // this will start at the top of the circle
    endAngle = startAngle + angle; // this will fill up the circle in clockwise direction

    this.ctx.save();

    this.ctx.translate(x, y);
    this.ctx.clearRect(this.actual_size * -0.5, this.actual_size * -0.5, this.actual_size, this.actual_size);

    // first circle
    this.ctx.strokeStyle = "rgba(128, 128, 128, 0.20)";
    this.ctx.beginPath();
    this.ctx.arc(0, 0, this.r_size / 2, 0, 2 * Math.PI, 2);
    this.ctx.lineWidth = this.r_thickness;
    this.ctx.stroke();

    // second circle
    this.ctx.strokeStyle = "rgba(253, 128, 1, 0.9)";
    this.ctx.beginPath();
    this.ctx.arc(0, 0, this.r_size / 2, startAngle, endAngle, false); // start the arc at the top and draw clockwise
    this.ctx.lineWidth = this.r_thickness;
    this.ctx.stroke();

    // label
    this.ctx.fillStyle = "#ffffff";
    this.ctx.font = '12px Helvetica';
    this.ctx.fillText(label, 0, 23);

    this.ctx.font = 'bold 40px Helvetica';
    this.ctx.fillText(Math.floor(value), 0, 10);

    this.ctx.restore();
}


document.querySelectorAll('.countdown').forEach(function (countdown, index) {
    var startDate = countdown.dataset.startDate; // start date
    var endDate = countdown.dataset.endDate; // end date
    var canvas = document.createElement('canvas');
    canvas.id = 'canvas' + index; // Give each canvas a unique ID
    countdown.appendChild(canvas);
    new Ringer(canvas.id, endDate, startDate).init(); // pass end date first, then start date
});
