## General info
University project in ASP .NET - route establisher for electric car owners.
This web application has purpose of automatization of establishing routes for electric car owners, taking into account stops for battery charge. Application is connected to database that along with others has charging station records, which are showing up on the map if are near routes and if charging points are the same as in the car.
During calculation of real maximum distance of car, there are considered many factors, such as current season (battery performance is other in winter and summer) and initial battery level. User can easily see which charging stations are in range of his car, can choose the station he wants with possibility to check details of station (different payment methods, costs etc.) and add this station as stop on google maps route.
User can create account (identity library), add own cars with individual attributes and save them.
In the future I'm planning to make also mobile application version of this, probably in Xamarin.

## Technologies
* C#
* ASP .NET
* MVC
* Repository pattern
* Entity Framework
* DB: Code first
* Identity platform authentication libraries
* HTML
* JS
* CSS
* Bootstrap
* Razor pages
