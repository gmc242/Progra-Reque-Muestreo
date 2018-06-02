1. Register your application at https://code.google.com/apis/console/ as a SERVICE ACCOUNT
2. Once your application is setup, change the following appSettings in web.config:
	a. Google.Analytics.AppName - enter your name of your app here
	b. Google.Analytics.UserName - enter your email address here
	c. Google.Analytics.AppKey - enter your generated Application Key here
	d. Google.Analytics.ProfileId - enter the profile id of the analytics account you want to 
		view (the g number in the google.com/analytics ) in format ga:########
	
	sample:
	<add key="Google.Analytics.AppName" value="MyAppName" />
    <add key="Google.Analytics.UserName" value="myEmailAddr@gmail.com" />
    <add key="Google.Analytics.AppKey" value="myAppKey" />
    <add key="Google.Analytics.ProfileId" value="ga:#forMyAnalyticsAcct#" />
    
3. Test that your connection works by browsing "Examples > Ex01_Simple.aspx"
4. customize away!
5.  Oh... and read the license!  For questions, comments, concerns email me at shawns@designbyssi.com or shawn.souto@gmail.com