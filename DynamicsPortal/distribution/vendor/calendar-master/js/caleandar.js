var Calendar = function (model, options, date) {
    this.Options = {
        Color: "",
        LinkColor: "",
        NavShow: true,
        NavVertical: false,
        NavLocation: "",
        DateTimeShow: true,
        DateTimeFormat: "mmm, yyyy",
        DatetimeLocation: "",
        EventClick: "",
        EventTargetWholeDay: false,
        DisabledDays: [],
        ModelChange: model,
        onMonthChange: null // <-- new option
    };

    for (var key in options) {
        this.Options[key] =
            typeof options[key] == "string"
                ? options[key].toLowerCase()
                : options[key];
    }

    this.Model = model || [];
    this.Today = new Date();
    this.Today.Month = this.Today.getMonth();
    this.Today.Year = this.Today.getFullYear();

    if (date instanceof Date) {
        this.Selected = new Date(date.getFullYear(), date.getMonth(), date.getDate());
    } else {
        this.Selected = new Date(this.Today);
    }

    this.Selected.Month = this.Selected.getMonth();
    this.Selected.Year = this.Selected.getFullYear();
    this.Selected.Days = new Date(this.Selected.Year, this.Selected.Month + 1, 0).getDate();
    this.Selected.FirstDay = new Date(this.Selected.Year, this.Selected.Month, 1).getDay();
    this.Selected.LastDay = new Date(this.Selected.Year, this.Selected.Month + 1, 0).getDay();

    this.Prev = new Date(this.Selected.Year, this.Selected.Month - 1, 1);
    if (this.Selected.Month == 0) this.Prev = new Date(this.Selected.Year - 1, 11, 1);
    this.Prev.Days = new Date(this.Prev.getFullYear(), this.Prev.getMonth() + 1, 0).getDate();
};

function createCalendar(calendar, element, adjuster) {
    if (typeof adjuster !== "undefined") {
        var newDate = new Date(calendar.Selected.Year, calendar.Selected.Month + adjuster, 1);
        if (typeof calendar.Options.onMonthChange === "function") {
            calendar.Options.onMonthChange(newDate.getFullYear(), newDate.getMonth());
            return;
        }
        calendar = new Calendar(calendar.Model, calendar.Options, newDate);
        element.innerHTML = "";
    } else {
        for (var key in calendar.Options) {
            typeof calendar.Options[key] != "function" &&
                typeof calendar.Options[key] != "object" &&
                calendar.Options[key]
                ? (element.className += " " + key + "-" + calendar.Options[key])
                : 0;
        }
    }

    var months = [
        "January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"
    ];

    var mainSection = document.createElement("div");
    mainSection.className += "cld-main";

    function AddDateTime() {
        var datetime = document.createElement("div");
        datetime.className += "cld-datetime";

        if (calendar.Options.NavShow && !calendar.Options.NavVertical) {
            var rwd = document.createElement("div");
            rwd.className += " cld-rwd cld-nav";
            rwd.addEventListener("click", function () {
                var newDate = new Date(calendar.Selected.Year, calendar.Selected.Month - 1, 1);
                if (typeof calendar.Options.onMonthChange === "function") {
                    calendar.Options.onMonthChange(newDate.getFullYear(), newDate.getMonth());
                } else {
                    createCalendar(calendar, element, -1);
                }
            });
            rwd.innerHTML =
                '<svg height="15" width="15" viewBox="0 0 75 100" fill="rgba(0,0,0,0.5)"><polyline points="0,50 75,0 75,100"></polyline></svg>';
            datetime.appendChild(rwd);
        }

        var today = document.createElement("div");
        today.className += " today";
        today.innerHTML = months[calendar.Selected.Month] + ", " + calendar.Selected.Year;
        datetime.appendChild(today);

        if (calendar.Options.NavShow && !calendar.Options.NavVertical) {
            var fwd = document.createElement("div");
            fwd.className += " cld-fwd cld-nav";
            fwd.addEventListener("click", function () {
                var newDate = new Date(calendar.Selected.Year, calendar.Selected.Month + 1, 1);
                if (typeof calendar.Options.onMonthChange === "function") {
                    calendar.Options.onMonthChange(newDate.getFullYear(), newDate.getMonth());
                } else {
                    createCalendar(calendar, element, 1);
                }
            });
            fwd.innerHTML =
                '<svg height="15" width="15" viewBox="0 0 75 100" fill="rgba(0,0,0,0.5)"><polyline points="0,0 75,50 0,100"></polyline></svg>';
            datetime.appendChild(fwd);
        }

        mainSection.appendChild(datetime);
    }

    function AddLabels() {
        var labels = document.createElement("ul");
        labels.className = "cld-labels";
        var labelsList = ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"];
        for (var i = 0; i < labelsList.length; i++) {
            var label = document.createElement("li");
            label.className += "cld-label";
            label.innerHTML = labelsList[i];
            labels.appendChild(label);
        }
        mainSection.appendChild(labels);
    }

    function AddDays() {
        function DayNumber(n) {
            var number = document.createElement("p");
            number.className += "cld-number";
            number.innerHTML += n;
            return number;
        }

        var days = document.createElement("ul");
        days.className += "cld-days";

        for (var i = 0; i < calendar.Selected.FirstDay; i++) {
            var day = document.createElement("li");
            day.className += "cld-day prevMonth";
            var number = DayNumber(calendar.Prev.Days - calendar.Selected.FirstDay + (i + 1));
            day.appendChild(number);
            days.appendChild(day);
        }

        for (var i = 0; i < calendar.Selected.Days; i++) {
            var day = document.createElement("li");
            day.className += "cld-day currMonth";
            var number = DayNumber(i + 1);

            for (var n = 0; n < calendar.Model.length; n++) {
                var evDate = calendar.Model[n].Date;
                var toDate = new Date(calendar.Selected.Year, calendar.Selected.Month, i + 1);
                if (evDate.getTime() == toDate.getTime()) {
                    number.className += " eventday";
                    var title = document.createElement("span");
                    title.className += "cld-title";
                    if (calendar.Model[n].Color) number.style.color = calendar.Model[n].Color;
                    if (calendar.Model[n].BgColor) day.style.backgroundColor = calendar.Model[n].BgColor;
                    if (calendar.Model[n].CssClass) day.classList.add(calendar.Model[n].CssClass);
                    title.innerHTML += '<a href="' + (calendar.Model[n].Link || "#") + '">' + calendar.Model[n].Title + "</a>";
                    number.appendChild(title);
                }
            }

            day.appendChild(number);

            if (
                i + 1 == calendar.Today.getDate() &&
                calendar.Selected.Month == calendar.Today.Month &&
                calendar.Selected.Year == calendar.Today.Year
            ) {
                day.className += " today";
            }

            days.appendChild(day);
        }

        mainSection.appendChild(days);
    }

    if (calendar.Options.Color)
        mainSection.innerHTML += "<style>.cld-main{color:" + calendar.Options.Color + ";}</style>";
    if (calendar.Options.LinkColor)
        mainSection.innerHTML += "<style>.cld-title a{color:" + calendar.Options.LinkColor + ";}</style>";

    element.appendChild(mainSection);
    if (calendar.Options.DateTimeShow) AddDateTime();
    AddLabels();
    AddDays();
}

function caleandar(el, data, settings, date) {
    var obj = new Calendar(data, settings, date);
    createCalendar(obj, el);
}
