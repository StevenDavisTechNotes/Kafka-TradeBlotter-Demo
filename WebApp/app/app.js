"use strict";
var App = (function () {
    function App() {
        this.counterValue = 5;
    }
    Object.defineProperty(App.prototype, "counter", {
        get: function () {
            return this.counterValue;
        },
        set: function (val) {
            this.counterValue = val;
        },
        enumerable: true,
        configurable: true
    });
    App.prototype.decrement = function () {
        this.counter--;
    };
    App.prototype.increment = function () {
        this.counter++;
    };
    return App;
}());
exports.App = App;
//# sourceMappingURL=app.js.map