using AngularJSAuthentication.API.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AngularJSAuthentication.API.Controllers
{
    [RoutePrefix("api/Orders")]
    public class OrdersController : ApiController
    {
        /// <summary>
        /// if you tried to issue HTTP GET request to the end point 
        /// “http://localhost:port/api/orders” you will receive 
        /// HTTP status code 401 unauthorized
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(OrderMemory.CreateOrders());
        }
    }
}
