

google.load('visualization', '1', { 'packages': GoogleAnalytics.Packages });

Sys.Application.add_load(function () {
    if (GoogleAnalytics.Debug)
        console.log('initializing google script, google.visualization =', google.visualization);

    if (typeof google.visualization == 'undefined') {
        google.load('visualization', '1', { 'packages': GoogleAnalytics.Packages });

        google.setOnLoadCallback(reportManager.initCharts);

        if (GoogleAnalytics.Debug)
            console.log('initialized google script and set google OnLoadCallback to reportManager.initCharts');
    }
    else {
        if (GoogleAnalytics.Debug)
            console.log('calling reportManager.initCharts()');

        reportManager.initCharts();

        if (GoogleAnalytics.Debug)
            console.log('reportManager.initCharts() completed');
    }
    //google.setOnLoadCallback(renderAll);  //does this still work without this?
});