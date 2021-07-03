# ChartViewer

This is a simple ASP.NET MVC Project which displays Charts based on data retrived from the Database using Charts.js library.
This includes below functionality:
1. Login screen:
	Allows User to Login with the credentials given below. Password is stored in the database. On login Password in retreived based on the email address and decrypted to match with the entered password on screen by user. If password matches then User logs in successfully, otherwise login fails disaplying failure one of the failure messgaes on login screen:
	Failure Messages:
	1. "You Account is blocked for multiple failed login attempts. Please try again tomorrow."
	2. "No User found with this username."
	3. "Invalid username or password."
	
	User context is stored in session. Current Session timeout is 2 mins in web.config file, after 2 mins if any postback occurs then user is logged out of the application and navigated to login screen with Error Message : 
	"Your session expired. Please login again."
	SessionExpireFilterAttribute is Used to handle this functionality and decorated this on HomeController.	
	
UserName and Password for Login:
1. UserName - john.doe@internal.com  Password - Password@123
2. UserName - jane.doe@internal.com  Password - Password@456
3. UserName - mike.hill@internal.com  Password - Password@789
4. UserName - martha.hill@internal.com  Password - Password@000
	
	
2. Home screen, after successful authentication Home screen fetches data from the database and displayes Bar chart on screen with Export button.
3. Charts in the home screen, the data should come from the database. - Charts.js is used to disaply Horizontal Bar chart on Home screen. Chart shows statewise Confirmed COVID-19 	 cases retrived from the database.
4. Extraction of reports in PDF format. Export to PDF button,on click downloads chart in pdf format


Overall design Items:
1. Sql server Database is used to store and retriev data. Mdf and ldf files included in App_data folder in repository
2. Dependency injection is achived using Unity Container.

