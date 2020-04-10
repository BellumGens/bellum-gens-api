using BellumGens.Api.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BellumGens.Api.Controllers
{
    [RoutePrefix("api/Shop")]
    public class ShopController : BaseController
    {
        [HttpPost]
        [Route("Order")]
        public IHttpActionResult SubmitOrder(JerseyOrder order)
        {
            if (ModelState.IsValid)
            {
                _dbContext.JerseyOrders.Add(order);

                try
                {
                    _dbContext.SaveChanges();
                }
                catch (DbUpdateException e)
                {
                    System.Diagnostics.Trace.TraceError("Order update exception: " + e.Message);
                    return BadRequest("Something went wrong...");
                }
                return Ok(order);
            }
            return BadRequest("Order couldn't be validated");
        }
    }
}
