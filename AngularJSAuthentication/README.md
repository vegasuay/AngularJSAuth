<h2>Token Based Authentication using ASP.NET Web API 2, Owin, and Identity</h2>

In this tutorial we’ll build SPA using <b>AngularJS</b> for the front-end, and <b>ASP.NET Web API 2</b>, <b>Owin middleware</b>, and <b>ASP.NET Identity</b> for the back-end.
The API supports CORS and accepts HTTP calls from any origin

<h3>Token Based Authentication</h3>
As I stated before we’ll use token based approach to implement authentication between the front-end application and the back-end API, with the evolution of front-end frameworks and the huge change on how we build web applications nowadays the preferred approach to authenticate users is to use signed token as this token sent to the server with each request, some of the benefits for using this approach are:<br />
<b>* Scalability of Servers:</b> The token sent to the server is self contained which holds all the user information needed for authentication, so adding more servers to your web farm is an easy task, there is no dependent on shared session stores.

<b>* Loosely Coupling:</b> Your front-end application is not coupled with specific authentication mechanism, the token is generated from the server and your API is built in a way to understand this token and do the authentication.

<b>* Mobile Friendly:</b> Cookies and browsers like each other, but storing cookies on native platforms (Android, iOS, Windows Phone) is not a trivial task, having standard way to authenticate users will simplify our life if we decided to consume the back-end API from native applications.

