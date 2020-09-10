using BellumGens.Api.Common;
using BellumGens.Api.Models;
using BellumGens.Api.Providers;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace BellumGens.Api.Controllers
{
    [RoutePrefix("api/Shop")]
    public class ShopController : BaseController
    {
        private const int baseJerseyPrice = 30;

        [HttpGet]
        [Route("Promo")]
        public IHttpActionResult CheckPromo(string code)
        {
            return Ok(_dbContext.PromoCodes.Find(code.ToUpperInvariant()));
        }

        [Authorize]
        [Route("Orders")]
        public IHttpActionResult GetOrders()
        {
            if (UserIsInRole("admin"))
            {
                return Ok(_dbContext.JerseyOrders.ToList());
            }
            return Unauthorized();
        }



        [Authorize]
        [HttpPut]
        [Route("Edit")]
        public IHttpActionResult EditOrder(Guid orderId, JerseyOrder order)
        {
            if (UserIsInRole("admin"))
            {
                _dbContext.Entry(order).State = EntityState.Modified;

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
            return Unauthorized();
        }

        [HttpPost]
        [Route("Order")]
        public async Task<IHttpActionResult> SubmitOrder(JerseyOrder order)
        {
            if (ModelState.IsValid)
            {
                if (order.PromoCode != null)
                {
                    order.PromoCode = order.PromoCode.ToUpperInvariant();
                }

                _dbContext.JerseyOrders.Add(order);

                try
                {
                    _dbContext.SaveChanges();
                }
                catch (DbUpdateException e)
                {
                    System.Diagnostics.Trace.TraceError("Order submit exception: " + e.Message);
                    return BadRequest("Something went wrong...");
                }

                try
                {
                    await _dbContext.Entry(order).Reference(o => o.Promo).LoadAsync().ConfigureAwait(false);

                    decimal discount = 0;
                    if (order.Promo != null)
                    {
                        discount = order.Promo.Discount;
                    }

                    StringBuilder builder = new StringBuilder();
                    builder.Append($@"Здравейте {order.FirstName} {order.LastName},
                                <p>Успешно получихме вашата поръчка. Очаквайте обаждане на посоченият от вас телефонен номер за потвърждение!</p>
                                <p>Детайли за вашата поръчка:</p>");
                    foreach (JerseyDetails jersey in order.Jerseys)
                    {
                        builder.Append($"<p>{Util.JerseyCutNames[jersey.Cut]} тениска, размер {Util.JerseySizeNames[jersey.Size]}</p>");
                    }
                    builder.Append($"Обща цена: {order.Jerseys.Count * baseJerseyPrice * (1 - discount) + 5}лв.");
                    builder.Append(@"<p>Поздрави от екипа на Bellum Gens!</p>
                                <a href='https://eb-league.com' target='_blank'>https://eb-league.com</a>");

                    await EmailServiceProvider.SendNotificationEmail(order.Email, "Поръчката ви е получена", builder.ToString()).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Trace.TraceError("Order submit exception: " + e.Message);
                }
                return Ok(order);
            }
            return BadRequest("Order couldn't be validated...");
        }
    }
}
