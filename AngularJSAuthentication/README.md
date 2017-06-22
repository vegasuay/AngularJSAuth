# Token Based Authentication using ASP.NET Web API 2, Owin, and Identity

In this tutorial we’ll build SPA using <b>AngularJS</b> for the front-end, and <b>ASP.NET Web API 2</b>, <b>Owin middleware</b>, and <b>ASP.NET Identity</b> for the back-end.
The API supports CORS and accepts HTTP calls from any origin

<h3>Token Based Authentication</h3>
As I stated before we’ll use token based approach to implement authentication between the front-end application and the back-end API, with the evolution of front-end frameworks and the huge change on how we build web applications nowadays the preferred approach to authenticate users is to use signed token as this token sent to the server with each request, some of the benefits for using this approach are:<br />
<b>* Scalability of Servers:</b> The token sent to the server is self contained which holds all the user information needed for authentication, so adding more servers to your web farm is an easy task, there is no dependent on shared session stores.

<b>* Loosely Coupling:</b> Your front-end application is not coupled with specific authentication mechanism, the token is generated from the server and your API is built in a way to understand this token and do the authentication.

<b>* Mobile Friendly:</b> Cookies and browsers like each other, but storing cookies on native platforms (Android, iOS, Windows Phone) is not a trivial task, having standard way to authenticate users will simplify our life if we decided to consume the back-end API from native applications.

<h3>Building the Back-End API</h3>
<b>Step 1: Creating the Web API Project</b><br />

![alt text](https://github.com/vegasuay/AngularJSAuth/blob/master/AngularJSAuthentication/Images/createproject.PNG)

<b>Step 2: Installing the needed NuGet Packages:</b>
Install-Package Microsoft.AspNet.WebApi.Owin<br />
Install-Package Microsoft.Owin.Host.SystemWeb<br />
Install-Package Microsoft.AspNet.Identity.Owin<br />
Install-Package Microsoft.AspNet.Identity.EntityFramework<br />
Install-Package Microsoft.Owin.Security.OAuth<br />
Install-Package Microsoft.Owin.Cors<br />

<b>Step 3: Add Owin “Startup” Class</b><br />
this class will be fired once our server starts, notice the “assembly” attribute which states which class to fire on start-up. The “Configuration” method accepts parameter of type “IAppBuilder” this parameter will be supplied by the host at run-time. This “app” parameter is an interface which will be used to compose the application for our Owin server.

The “HttpConfiguration” object is used to configure API routes, so we’ll pass this object to method “Register” in “WebApiConfig” class.

Lastly, we’ll pass the “config” object to the extension method “UseWebApi” which will be responsible to wire up ASP.NET Web API to our Owin server pipeline.

<b>Step 4: Delete Global.asax Class</b>
<br />
<b>Step 5: Add the ASP.NET Identity System</b><br />
Now we need to add Database context class which will be responsible to communicate with our database, so add new class and name it “AuthContext”.

this class inherits from “IdentityDbContext” class, you can think about this class as special version of the traditional “DbContext” Class, it will provide all of the Entity Framework code-first mapping and DbSet properties needed to manage the identity tables in SQL Server.

Now we want to add “UserModel” which contains the properties needed to be sent once we register a user, this model is POCO class with some data annotations attributes used for the sake of validating the registration payload request. So under “Models” folder add new class named “UserModel” .

Now we need to add new connection string named “AuthContext” in our Web.Config class, so open you web.config and add 

connectionStrings
    add name="AuthContext" connectionString="Data Source=(localdb)\v11.0;Initial Catalog=AngularJSAuth;Integrated Security=SSPI" providerName="System.Data.SqlClient"
connectionStrings

<b>Step 6: Add Repository class to support ASP.NET Identity System</b><br />
Now we want to implement two methods needed in our application which they are: “RegisterUser” and “FindUser”, so add new class named “AuthRepository”.

<b>Step 7: Add our “Account” Controller</b><br>
Now it is the time to add our first Web API controller which will be used to register new users.

By looking at the “Register” method you will notice that we’ve configured the endpoint for this method to be “/api/account/register” so any user wants to register into our system must issue HTTP POST request to this URI and the pay load for this request will contain the JSON object.

Now you can run your application and issue HTTP POST request to your local URI: “http://localhost:port/api/account/register” if all went fine you will receive HTTP status code 200 and the database specified in connection string will be created automatically and the user will be inserted into table “dbo.AspNetUsers”.

![alt text](https://github.com/vegasuay/AngularJSAuth/blob/master/AngularJSAuthentication/Images/post_register.PNG)

The “GetErrorResult” method is just a helper method which is used to validate the “UserModel” and return the correct HTTP status code if the input data is invalid.

<b>Step 8: Add Secured Orders Controller</b><br />
Now we want to add another controller to serve our Orders, we’ll assume that this controller will return orders only for Authenticated users, to keep things simple we’ll return static data. So add new controller named “OrdersController” under “Controllers” folder.

Notice how we added the “Authorize” attribute on the method “Get” so if you tried to issue HTTP GET request to the end point “http://localhost:port/api/orders” you will receive HTTP status code 401 unauthorized because the request you send till this moment doesn’t contain valid authorization header.

<b>Step 9: Add support for OAuth Bearer Tokens Generation</b><br>
Till this moment we didn’t configure our API to use OAuth authentication workflow.

Here we’ve created new instance from class “OAuthAuthorizationServerOptions” and set its option as the below:

* The path for generating tokens will be as :”http://localhost:port/token”. We’ll see how we will issue HTTP POST request to generate token in the next steps.
* We’ve specified the expiry for token to be 24 hours, so if the user tried to use the same token for authentication after 24 hours from the issue time, his request will be rejected and HTTP status code 401 is returned.
* We’ve specified the implementation on how to validate the credentials for users asking for tokens in custom class named “SimpleAuthorizationServerProvider”.

Now we passed this options to the extension method “UseOAuthAuthorizationServer” so we’ll add the authentication middleware to the pipeline.

![alt text](https://github.com/vegasuay/AngularJSAuth/blob/master/AngularJSAuthentication/Images/post_token.PNG)<br />
![alt text](https://github.com/vegasuay/AngularJSAuth/blob/master/AngularJSAuthentication/Images/post_token_response.PNG)<br />

<b>Step 10: Implement the “SimpleAuthorizationServerProvider” class</b><br />
Add new folder named “Providers” then add new class named “SimpleAuthorizationServerProvider”.

As you notice this class inherits from class “OAuthAuthorizationServerProvider”, we’ve overridden two methods “ValidateClientAuthentication” and “GrantResourceOwnerCredentials”. The first method is responsible for validating the “Client”, in our case we have only one client so we’ll always return that its validated successfully.

The second method “GrantResourceOwnerCredentials” is responsible to validate the username and password sent to the authorization server’s token endpoint, so we’ll use the “AuthRepository” class we created earlier and call the method “FindUser” to check if the username and password are valid.

If the credentials are valid we’ll create “ClaimsIdentity” class and pass the authentication type to it, in our case “bearer token”, then we’ll add two claims (“sub”,”role”) and those will be included in the signed token. You can add different claims here but the token size will increase for sure.

Now generating the token happens behind the scenes when we call “context.Validated(identity)”.

To allow CORS on the token middleware provider we need to add the header “Access-Control-Allow-Origin” to Owin context, if you forget this, generating the token will fail when you try to call it from your browser. Not that this allows CORS for token middleware provider not for ASP.NET Web API which we’ll add on the next step.

<b>Step 11: Allow CORS for ASP.NET Web API</b><br />
open class “Startup” again and add the highlighted line of code (line 8) to the method “Configuration” as the below:

app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

<b>Step 12: Testing the Back-end API</b>
Assuming that you registered the username “XXX” with password “SuperPass” in the step below, we’ll use the same username to generate token, so to test this out open your favorite REST client application in order to issue HTTP requests to generate token for user “XXX”. For me I’ll be using Advance REST client.

![alt text](https://github.com/vegasuay/AngularJSAuth/blob/master/AngularJSAuthentication/Images/get_orders.PNG)<br />
![alt text](https://github.com/vegasuay/AngularJSAuth/blob/master/AngularJSAuthentication/Images/get_orders_response.PNG)<br />

If all is correct we’ll receive HTTP status 200 along with the secured data in the response body, if you try to change any character with signed token you directly receive HTTP status code 401 unauthorized.


