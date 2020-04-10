using BellumGens.Api.Models;
using BellumGens.Api.Providers;
using System;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;
using System.Web.Http;

namespace BellumGens.Api.Controllers
{
    [RoutePrefix("api/Shop")]
    public class ShopController : BaseController
    {
        [HttpPost]
        [Route("Order")]
        public async Task<IHttpActionResult> SubmitOrder(JerseyOrder order)
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


                string message = @"Здравейте,
                                <p>Успешно получихме вашата поръчка. Очаквайте обаждане на посоченият от вас телефонен номер за потвърждение!</p>
                                <p>Поздрави от екипа на Bellum Gens!</p>
                                <a href='https://eb-league.com' target='_blank'>https://eb-league.com</a>";
                try
                {
                    await EmailServiceProvider.SendNotificationEmail(order.Email, "Регистрацията ви е получена", message).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Trace.TraceError("Order update exception: " + e.Message);
                }
                return Ok(order);
            }
            return BadRequest("Order couldn't be validated");
        }
    }
}
