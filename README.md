# AngularJSAuth

<h3>What we’ll build in this app?</h3>
The front-end SPA will be built using <b>HTML5, AngularJS, and Twitter Bootstrap</b>. The back-end server will be built using <b>ASP.NET Web API 2 on top of Owin middleware not directly on top of ASP.NET</b>; the reason for doing so that we’ll configure the server to issue OAuth bearer token authentication using Owin middleware too, so setting up everything on the same pipeline is better approach. In addition to this we’ll use ASP.NET Identity system which is built on top of Owin middleware and we’ll use it to register new users and validate their credentials before generating the tokens.<br />

As I mentioned before our back-end API should accept request coming from any origin, not only our front-end, so we’ll be enabling CORS (Cross Origin Resource Sharing) in Web API as well for the OAuth bearer token provider.

<h3>Use cases which will be covered in this application</h3>

* Allow users to signup (register) by providing username and password then store credentials in secure medium.
* Prevent anonymous users from viewing secured data or secured pages (views).
* Once the user is logged in successfully, the system should not ask for credentials or re-authentication for the next 24 hours 30 minutes because we are using refresh tokens.
So in this post we’ll cover step by step how to build the back-end API, and on the next post we’ll cover how we’ll build and integrate the SPA with the API.

