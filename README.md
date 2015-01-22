# MLMS (Multi Level Management System)

#Problem

My best friend owns a martial arts studio called West Coast Martial Arts Academy. I wanted to solve an immediate
issue for him through technology. One of his biggest issues was the ability to track any leads he would get through 
different mediums. 

#Solution
This application has key features to help him enter a lead, track the leads status, schedule introduction meetings, 
schedule follow up meetings, and finally flag the lead as a customer or a dead lead. There are other features as well,
such as an admin tool and a calendar to visualize the upcoming meetings.

MLMS is a full stack application.
Front-end - HTML/CSS, Javascript(Jquery) with ajax calls
Back-end - C#
Database - MS SQL 2008 R2
The solution is hosted on a godaddy site and uses Amazon AWS RDS to host the database.

#Reasoning

I started this application about a year and a half ago. I built this on the .net stack due to comfortability and to build the
application for my friend as soon as possible. I choose this stack as well because there are several C# libraries/apis that
I could leverage to build my application quick and efficient. I also was more familiar creating a specific memberservice 
class in C# and wanted to leverage that knowledge then using the built in membership providers.

I believe that this solution needs to be re-evaluated. At the beginning of my project I was stronger at building web forms
then an MVC architected application. If I had to go back and update I would build this application as a MVC application 
instead. I would want to make the application more RESTful as well. I would also spend more time segregating or modulazing
the code. There are a few instances where best practices are not used, such as putting css in external css files, javascript
in external files, etc. Another thing I would add is a unit testing framework. This application is still in use and I am 
already starting to build a new application in MVC to handle some of the issues I stated earlier. Maintaining this side
project is fun and I enjoy helping out my friend.

https://github.com/matthewdwong/MLMS/blob/master/MLMS/User/Calendar.aspx This page I am particularly proud of. I leveraged
an opensource javascript calendar called FullCalendar.js. I had to work with their apis as well as create several endpoints
on the server to pass JSON data to fullcalendar. I would then manipulate the DOM based on the JSON data which I found 
exciting.

My linkedin profile is https://www.linkedin.com/in/matthewdwong

The application URL is http://kpayongayong.com/MLMS
The username and password you can use is mattdavidwong@gmail.com; password:12345a
You can also register and create a new account.


