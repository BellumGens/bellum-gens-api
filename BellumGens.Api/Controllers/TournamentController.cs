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
                        return BadRequest("Моля попълнете Battle.net battle tag!");
                    }
                    if (_dbContext.TournamentApplications.Where(a => a.BattleNetId == application.BattleNetId).SingleOrDefault() != null)
                    {
                        return BadRequest($"Вече има направена регистрация с battle tag {application.BattleNetId}!");
                    }
                }
                else
                {
                    if (application.TeamId == Guid.Empty)
                    {
                        return BadRequest("Моля попълнете отбор във формата за регистрация!");
                    }
                    if (_dbContext.TournamentApplications.Where(a => a.TeamId == application.TeamId).SingleOrDefault() != null)
                    {
                        return BadRequest("Вече има направена регистрация за този отбор!");
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
                    return BadRequest("Нещо се обърка...");
                }

                try
                {
                    string gameMsg = application.Game == Game.CSGO ? "Вашата регистрация е за участие в лигата по CS:GO" :
                                                                    $"Вашата регистрация е за участие в лигата по StarCraft II, с battle tag {application.BattleNetId}";
                    string message = $@"Здравей, { user.UserName },
                                    <p>Успешно получихме вашата регистрация за Esports Бизнес Лигата. В регистрацията сте посочили, че текущо работите в <b>{ application.CompanyId }</b>. {gameMsg}. Регистрация ще бъде потвърдена след като преведете таксата за участие (60лв. с ДДС за лигата по StarCraft II, или 300лв. с ДДС за лигата по CS:GO).</p>
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
                    await EmailServiceProvider.SendNotificationEmail(application.Email, "Регистрацията ви е получена", message).ConfigureAwait(false);
                }
                catch
                {

                }
                return Ok(application);
            }
            return BadRequest("Не успяхме да вилидираме информацията...");
        }

        [HttpGet]
        [Route("Registrations")]
        public IHttpActionResult GetRegistrations()
        {
            ApplicationUser user = GetAuthUser();
            return Ok(_dbContext.TournamentApplications.Where(a => a.UserId == user.Id).ToList());
        }

        [HttpGet]
        [Route("RegCount")]
        [AllowAnonymous]
        public IHttpActionResult GetRegistrationsCount()
        {
            List<TournamentApplication> registrations = _dbContext.TournamentApplications.ToList();
            List<RegistrationCountViewModel> model = new List<RegistrationCountViewModel>()
            {
                new RegistrationCountViewModel(registrations, Game.CSGO),
                new RegistrationCountViewModel(registrations, Game.StarCraft2)
            };
            return Ok(model);
        }

        [HttpGet]
        [Route("CSGORegs")]
        [AllowAnonymous]
        public IHttpActionResult GetCSGORegistrations()
        {
            List<TournamentApplication> entities = _dbContext.TournamentApplications.Where(r => r.Game == Game.CSGO).ToList();
            List<TournamentCSGOParticipant> registrations = new List<TournamentCSGOParticipant>();
            foreach (TournamentApplication app in entities)
            {
                registrations.Add(new TournamentCSGOParticipant(app));
            }
            return Ok(registrations);
        }

        [HttpGet]
        [Route("SC2Regs")]
        [AllowAnonymous]
        public IHttpActionResult GetSC2sRegistrations()
        {
            List<TournamentApplication> entities = _dbContext.TournamentApplications.Where(r => r.Game == Game.StarCraft2).ToList();
            List<TournamentSC2Participant> registrations = new List<TournamentSC2Participant>();
            foreach (TournamentApplication app in entities)
            {
                registrations.Add(new TournamentSC2Participant(app));
            }
            return Ok(registrations);
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
