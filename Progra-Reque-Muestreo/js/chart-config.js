var reportManager;
var GoogleAnalytics = {
    Packages: ['corechart', 'table', 'geochart', 'gauge', 'map', 'orgchart', 'timeline', 'treemap'],
    Debug: false
};

function ReportManager() {
    var _charts = [];
    this.addChart = function (initFuncName) {
        if (GoogleAnalytics.Debug)
            console.log('adding function name to _charts', initFuncName);

        _charts.push(initFuncName);
    }
    this.initCharts = function () {
        if (GoogleAnalytics.Debug)
            console.log('starting initCharts, using function list', _charts);
        for (i = 0; i < _charts.length; i++) {
            if (GoogleAnalytics.Debug)
                console.log('calling report render for ', _charts[i]);
            _charts[i]();
        }
    }
}

Sys.Application.add_load(function () {
    reportManager = new ReportManager();
    if (GoogleAnalytics.Debug)
        console.log('report manager initialized as "reportManager"', reportManager);
});


