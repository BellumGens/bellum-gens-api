using BellumGens.Api.Models;
using BellumGens.Api.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace BellumGens.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/Tournament")]
    public class TournamentController : BaseController
    {
        private readonly BellumGensDbContext _dbContext = new BellumGensDbContext();

        [HttpPost]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(TournamentApplication application)
        {
            if (ModelState.IsValid)
            {
                Company c = _dbContext.Companies.Find(application.CompanyId);
                ApplicationUser user = GetAuthUser();
                if (application.Game == Game.StarCraft2)
                {
                    if (string.IsNullOrEmpty(application.BattleNetId))
                    {
                        return BadRequest("Please provide your Battle.net battle tag!");
                    }
                }
                else
                {
                    if (application.TeamId == Guid.Empty)
                    {
                        return BadRequest("Please provide the team that you're registering!");
                    }
                }
                if (c == null)
                {
                    _dbContext.Companies.Add(new Company()
                    {
                        Name = application.CompanyId
                    });
                }
                application.UniqueHash(_dbContext);
                application.UserId = user.Id;
                _dbContext.TournamentApplications.Add(application);
                
                try
                {
                    _dbContext.SaveChanges();
                }
                catch
                {
                    return BadRequest("Something went wrong...");
                }

                try
                {
                    string message = $@"Здравей, { user.UserName },
                                    <p>Успешно получихме вашата регистрация за Esports Бизнес Лигата. В регистрацията сте посочили, че текущо работите в <b>{ application.CompanyId }</p>. Регистрация ще бъде потвърдена след като преведете таксата за участие (60лв. с ДДС за лигата по StarCraft II, или 300лв. с ДДС за лигата по CS:GO).</p>
                                    <p>Банковата ни сметка е</p>
                                    <ul>
                                        <li>Име на Банката: <b>{ AppInfo.Config.bank }</b></li>
                                        <li>Титуляр: <b>{ AppInfo.Config.bankAccountOwner }</b></li>
                                        <li>Сметка: <b>{ AppInfo.Config.bankAccount }</b></span></li>
                                        <li>BIC: <b>{ AppInfo.Config.bic }</b></li>
                                    </ul>
                                    <p>Моля при превода да сложите в основанието уникалния код, който сме генерирали за вашата регистрация: <b>{ application.Hash }</b>. Можете да намерите кода и през вашият профил на сайта ни.</p>
                                    <p>Ако ви е нужна фактура, моля да се свържете с нас на <a href='mailto:info@eb-league.com'>info@eb-league.com</a>!</p>
                                    <p>Поздрави от екипа на Bellum Gens!</p>
                                    <a href='https://eb-league.com' target='_blank'>https://eb-league.com</a>";
                    await EmailServiceProvider.SendNotificationEmail(application.Email, "Регистрацията ви е получена", message);
                }
                catch
                {

                }
                return Ok(application);
            }
            return BadRequest("The application didn't validate");
        }

        [HttpGet]
        [Route("Registrations")]
        public IHttpActionResult GetRegistrations()
        {
            ApplicationUser user = GetAuthUser();
            return Ok(_dbContext.TournamentApplications.Where(a => a.UserId == user.Id).ToList());
        }

        [HttpDelete]
        [Route("Delete")]
        public IHttpActionResult DeleteRegistraion(Guid id)
        {
            ApplicationUser user = GetAuthUser();
            TournamentApplication application = _dbContext.TournamentApplications.Find(id);
            if (application != null)
            {
                if (application.UserId == user.Id)
                {
                    _dbContext.TournamentApplications.Remove(application);
                    try
                    {
                        _dbContext.SaveChanges();
                    }
                    catch
                    {
                        return BadRequest("Something went wrong!");
                    }
                    return Ok(id);
                }
                return NotFound();
            }
            return NotFound();
        }
    }
}
