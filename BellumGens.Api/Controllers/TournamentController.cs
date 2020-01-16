using BellumGens.Api.Models;
using BellumGens.Api.Providers;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
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
                catch (DbUpdateException e)
                {
                    System.Diagnostics.Trace.TraceError("Tournament registration error: " + e.Message);
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
                catch (Exception e)
                {
                    System.Diagnostics.Trace.TraceError("Tournament registration error: " + e.Message);
                }
                return Ok(application);
            }
            return BadRequest("Не успяхме да вилидираме информацията...");
        }

        [Route("Registrations")]
        public IHttpActionResult GetRegistrations()
        {
            ApplicationUser user = GetAuthUser();
            return Ok(_dbContext.TournamentApplications.Where(a => a.UserId == user.Id).ToList());
        }

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

        [Route("CSGOGroups")]
        [AllowAnonymous]
        public IHttpActionResult GetCSGOGroups()
        {
            return Ok(_dbContext.TournamentCSGOGroups.ToList());
        }

        [Route("SC2Groups")]
        [AllowAnonymous]
        public IHttpActionResult GetSC2Groups()
        {
            return Ok(_dbContext.TournamentSC2Groups.ToList());
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
                    catch (DbUpdateException e)
                    {
                        System.Diagnostics.Trace.TraceError("Tournament registration delete error: " + e.Message);
                        return BadRequest("Something went wrong!");
                    }
                    return Ok(id);
                }
                return NotFound();
            }
            return NotFound();
        }

        [HttpGet]
        [Route("Leagues")]
        [AllowAnonymous]
        public IHttpActionResult GetLeagues()
        {
            return Ok(_dbContext.Tournaments.ToList());
        }


        [HttpPost]
        [Route("Create")]
        public IHttpActionResult CreateTournament(Tournament tournament)
        {
            if (UserIsInRole("admin"))
            {
                if (ModelState.IsValid)
                {
                    var entity = _dbContext.Tournaments.Find(tournament.ID);
                    if (entity != null)
                    {
                        _dbContext.Entry(entity).CurrentValues.SetValues(tournament);
                    }
                    else
                    {
                        _dbContext.Tournaments.Add(tournament);
                    }

                    try
                    {
                        _dbContext.SaveChanges();
                    }
                    catch (DbUpdateException e)
                    {
                        System.Diagnostics.Trace.TraceError("Tournament update exception: " + e.Message);
                        return BadRequest("Something went wrong...");
                    }
                    return Ok(tournament);
                }
                return BadRequest("Invalid tournament");
            }
            return Unauthorized();
        }

        [HttpPut]
        [Route("AddApplications")]
        public IHttpActionResult AddApplicationsToTournament(Guid id)
        {
            if (UserIsInRole("admin"))
            {
                Tournament tournament = _dbContext.Tournaments.Find(id);
                if (tournament != null)
                {
                    List<TournamentApplication> applications = _dbContext.TournamentApplications.Where(a => a.TournamentId == null).ToList();
                    foreach (var application in applications)
                    {
                        application.TournamentId = id;
                    }

                    try
                    {
                        _dbContext.SaveChanges();
                    }
                    catch (DbUpdateException e)
                    {
                        System.Diagnostics.Trace.TraceError("Tournament update exception: " + e.Message);
                        return BadRequest("Something went wrong...");
                    }
                    return Ok(tournament);
                }
                return NotFound();
            }
            return Unauthorized();
        }

        [HttpPut]
        [Route("csgogroup")]
        public IHttpActionResult SubmitGroup(Guid? id, TournamentCSGOGroup group)
        {
            if (UserIsInRole("admin"))
            {
                TournamentCSGOGroup entity = _dbContext.TournamentCSGOGroups.Find(id);
                if (entity != null)
                {
                    _dbContext.Entry(entity).CurrentValues.SetValues(group);
                }
                else
                {
                    group.TournamentId = _dbContext.Tournaments.First().ID;
                    _dbContext.TournamentCSGOGroups.Add(group);
                }

                try
                {
                    _dbContext.SaveChanges();
                }
                catch (DbUpdateException e)
                {
                    System.Diagnostics.Trace.TraceError("Tournament group update exception: " + e.Message);
                    return BadRequest("Something went wrong...");
                }
                return Ok(group);
            }
            return Unauthorized();
        }

        [HttpDelete]
        [Route("csgogroup")]
        public IHttpActionResult DeleteGroup(Guid id)
        {
            if (UserIsInRole("admin"))
            {
                TournamentCSGOGroup entity = _dbContext.TournamentCSGOGroups.Find(id);
                if (entity != null)
                {
                    foreach (var par in entity.Participants)
                    {
                        par.TournamentCSGOGroupId = null;
                    }
                    _dbContext.TournamentCSGOGroups.Remove(entity);
                    try
                    {
                        _dbContext.SaveChanges();
                    }
                    catch (DbUpdateException e)
                    {
                        System.Diagnostics.Trace.TraceError("Tournament group delete exception: " + e.Message);
                        return BadRequest("Something went wrong...");
                    }
                    return Ok("deleted");
                }
                return NotFound();
            }
            return Unauthorized();
        }

        [HttpPut]
        [Route("participanttogroup")]
        public IHttpActionResult AddToGroup(Guid id, TournamentApplication participant)
        {
            if (UserIsInRole("admin"))
            {
                TournamentGroup entity = _dbContext.TournamentCSGOGroups.Find(id);
                if (entity == null)
                {
                    entity = _dbContext.TournamentSC2Groups.Find(id);
                    if (entity != null)
                    {
                        TournamentApplication app = _dbContext.TournamentApplications.Find(participant.Id);
                        if (app == null)
                        {
                            return NotFound();
                        }

                        app.TournamentSC2GroupId = id;
                        try
                        {
                            _dbContext.SaveChanges();
                        }
                        catch (DbUpdateException e)
                        {
                            System.Diagnostics.Trace.TraceError("Tournament group participant add exception: " + e.Message);
                            return BadRequest("Something went wrong...");
                        }
                        return Ok("added");
                    }
                    return NotFound();
                }
                else
                {
                    TournamentApplication app = _dbContext.TournamentApplications.Find(participant.Id);
                    if (app == null)
                    {
                        return NotFound(); 
                    }

                    app.TournamentCSGOGroupId = id;
                    try
                    {
                        _dbContext.SaveChanges();
                    }
                    catch (DbUpdateException e)
                    {
                        System.Diagnostics.Trace.TraceError("Tournament group participant add exception: " + e.Message);
                        return BadRequest("Something went wrong...");
                    }
                    return Ok("added");
                }
            }
            return Unauthorized();
        }

        [HttpDelete]
        [Route("participanttogroup")]
        public IHttpActionResult RemoveFromGroup(Guid id)
        {
            if (UserIsInRole("admin"))
            {
                TournamentApplication entity = _dbContext.TournamentApplications.Find(id);
                if (entity != null)
                {
                    entity.TournamentCSGOGroupId = null;
                    entity.TournamentSC2GroupId = null;
                    try
                    {
                        _dbContext.SaveChanges();
                    }
                    catch (DbUpdateException e)
                    {
                        System.Diagnostics.Trace.TraceError("Tournament group participant delete exception: " + e.Message);
                        return BadRequest("Something went wrong...");
                    }
                    return Ok("added");
                }
                return NotFound();
            }
            return Unauthorized();
        }
    }
}
