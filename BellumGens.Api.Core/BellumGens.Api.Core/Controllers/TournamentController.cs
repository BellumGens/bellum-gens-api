using BellumGens.Api.Core;
using BellumGens.Api.Core.Models;
using BellumGens.Api.Core.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BellumGens.Api.Controllers
{
    [Authorize]
    public class TournamentController : BaseController
    {
        private readonly AppConfiguration _appInfo;
        public TournamentController(AppConfiguration appInfo, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, IEmailSender sender, BellumGensDbContext context)
            : base(userManager, roleManager, signInManager, sender, context)
        {
            _appInfo = appInfo;
        }

        #region TOURNAMENTS AND REGISTRATIONS
        [AllowAnonymous]
        [Route("ActiveTournament")]
        public IActionResult GetActiveTournament()
        {
            var tournament = _dbContext.Tournaments.FirstOrDefault(t => t.Active);
            return Ok(tournament);
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(TournamentApplication application)
        {
            if (ModelState.IsValid)
            {
                Company c = _dbContext.Companies.Find(application.CompanyId);
                ApplicationUser user = await GetAuthUser();
                if (application.Game == Game.StarCraft2)
                {
                    if (string.IsNullOrEmpty(application.BattleNetId))
                    {
                        return BadRequest("Моля попълнете Battle.net battle tag!");
                    }
                    if (_dbContext.TournamentApplications.Where(a => a.BattleNetId == application.BattleNetId && a.TournamentId == application.TournamentId).SingleOrDefault() != null)
                    {
                        return BadRequest($"Вече има направена регистрация с battle tag {application.BattleNetId}!");
                    }
                    if (user.BattleNetId == null || user.BattleNetId != application.BattleNetId)
                    {
                        _dbContext.Users.Find(user.Id).BattleNetId = application.BattleNetId;
                    }
                }
                else
                {
                    if (application.TeamId == Guid.Empty)
                    {
                        return BadRequest("Моля попълнете отбор във формата за регистрация!");
                    }
                    if (_dbContext.TournamentApplications.Where(a => a.TeamId == application.TeamId && a.TournamentId == application.TournamentId).SingleOrDefault() != null)
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
                                        <li>Име на Банката: <b>{ _appInfo.Config.bank }</b></li>
                                        <li>Титуляр: <b>{ _appInfo.Config.bankAccountOwner }</b></li>
                                        <li>Сметка: <b>{ _appInfo.Config.bankAccount }</b></span></li>
                                        <li>BIC: <b>{ _appInfo.Config.bic }</b></li>
                                    </ul>
                                    <p>Моля при превода да сложите в основанието уникалния код, който сме генерирали за вашата регистрация: <b>{ application.Hash }</b>. Можете да намерите кода и през вашият профил на сайта ни.</p>
                                    <p>Ако ви е нужна фактура, моля да се свържете с нас на <a href='mailto:info@eb-league.com'>info@eb-league.com</a>!</p>
                                    <p>Заповядайте и в нашият <a href='https://discord.gg/bnTcpa9'>дискорд сървър</a>!</p>
                                    <p>Поздрави от екипа на Bellum Gens!</p>
                                    <a href='https://eb-league.com' target='_blank'>https://eb-league.com</a>";
                    await _sender.SendEmailAsync(application.Email, "Регистрацията ви е получена", message).ConfigureAwait(false);
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
        public async Task<IActionResult> GetUserRegistrations()
        {
            ApplicationUser user = await GetAuthUser();
            return Ok(_dbContext.TournamentApplications.Where(a => a.UserId == user.Id && a.Tournament.Active).ToList());
        }

        [AllowAnonymous]
        [Route("RegCount")]
        public IActionResult GetTotalRegistrationsCount(Guid tournamentId)
        {
            List<TournamentApplication> registrations = _dbContext.TournamentApplications.Where(a => a.Tournament.ID == tournamentId).ToList();
            List<RegistrationCountViewModel> model = new List<RegistrationCountViewModel>()
            {
                new RegistrationCountViewModel(registrations, Game.CSGO),
                new RegistrationCountViewModel(registrations, Game.StarCraft2)
            };
            return Ok(model);
        }

        [Route("AllRegistrations")]
        public async Task<IActionResult> GetAllApplications()
        {
            return await UserIsInRole("admin") ? Ok(_dbContext.TournamentApplications.Where(a => a.Tournament.Active).ToList()) : Unauthorized();
        }

        [HttpPut]
        [Route("Confirm")]
        public async Task<IActionResult> ConfirmRegistration(Guid id, TournamentApplication application)
        {
            if (await UserIsInRole("admin"))
            {
                TournamentApplication entity = _dbContext.TournamentApplications.Find(id);
                if (entity != null)
                {
                    _dbContext.Entry(entity).CurrentValues.SetValues(application);

                    try
                    {
                        _dbContext.SaveChanges();
                    }
                    catch (DbUpdateException e)
                    {
                        System.Diagnostics.Trace.TraceError("Tournament registration update error: " + e.Message);
                        return BadRequest("Something went wrong!");
                    }
                    return Ok(application);
                }
                return NotFound();
            }
            return Unauthorized();
        }

        [AllowAnonymous]
        [Route("CSGORegs")]
        public IActionResult GetCSGORegistrations(Guid? tournamentId = null)
        {
            List<TournamentApplication> entities = tournamentId != null ?
                _dbContext.TournamentApplications.Where(r => r.Game == Game.CSGO && r.TournamentId == tournamentId).ToList() :
                _dbContext.TournamentApplications.Where(r => r.Game == Game.CSGO && r.Tournament.Active).ToList();

            List<TournamentCSGOMatch> matches = tournamentId != null ?
                _dbContext.TournamentCSGOMatches.Where(m => m.TournamentId == tournamentId).ToList() :
                _dbContext.TournamentCSGOMatches.Where(m => m.Tournament.Active).ToList();

            List<TournamentCSGOParticipant> registrations = new List<TournamentCSGOParticipant>();
            foreach (TournamentApplication app in entities)
            {
                registrations.Add(new TournamentCSGOParticipant(app, matches.FindAll(m => m.Team1Id == app.TeamId || m.Team2Id == app.TeamId)));
            }
            return Ok(registrations);
        }

        [AllowAnonymous]
        [Route("SC2Regs")]
        public IActionResult GetSC2sRegistrations(Guid? tournamentId = null)
        {
            List<TournamentApplication> entities = tournamentId != null ?
                _dbContext.TournamentApplications.Where(r => r.Game == Game.StarCraft2 && r.TournamentId == tournamentId).ToList() :
                _dbContext.TournamentApplications.Where(r => r.Game == Game.StarCraft2 && r.Tournament.Active).ToList();

            List<TournamentSC2Match> matches = tournamentId != null ?
                _dbContext.TournamentSC2Matches.Where(m => m.TournamentId == tournamentId).ToList() :
                _dbContext.TournamentSC2Matches.Where(m => m.Tournament.Active).ToList();

            List<TournamentSC2Participant> registrations = new List<TournamentSC2Participant>();
            
            foreach (TournamentApplication app in entities)
            {
                registrations.Add(new TournamentSC2Participant(app, matches.FindAll(m => m.Player1Id == app.UserId || m.Player2Id == app.UserId)));
            }
            return Ok(registrations);
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> DeleteRegistraion(Guid id)
        {
            ApplicationUser user = await GetAuthUser();
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
        [AllowAnonymous]
        [Route("Tournaments")]
        public IActionResult GetTournaments()
        {
            return Ok(_dbContext.Tournaments.ToList());
        }


        [HttpPut]
        [Route("Create")]
        public async Task<IActionResult> CreateTournament(Tournament tournament)
        {
            if (await UserIsInRole("admin"))
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
        #endregion

        #region GROUPS
        [AllowAnonymous]
        [Route("CSGOGroups")]
        public IActionResult GetCSGOGroups(Guid? tournamentId = null)
        {
            List<TournamentCSGOGroup> groups = tournamentId != null ?
                _dbContext.TournamentCSGOGroups.Where(g => g.TournamentId == tournamentId).ToList() :
                _dbContext.TournamentCSGOGroups.Where(g => g.Tournament.Active).ToList();
            return Ok(groups);
        }

        [AllowAnonymous]
        [Route("SC2Groups")]
        public IActionResult GetSC2Groups(Guid? tournamentId = null)
        {
            List<TournamentSC2Group> groups = tournamentId != null ?
                _dbContext.TournamentSC2Groups.Where(g => g.TournamentId == tournamentId).ToList() :
                _dbContext.TournamentSC2Groups.Where(g => g.Tournament.Active).ToList();
            return Ok(groups);
        }

        [HttpPut]
        [Route("csgogroup")]
        public async Task<IActionResult> SubmitCSGOGroup(Guid? id, TournamentCSGOGroup group)
        {
            if (await UserIsInRole("event-admin"))
            {
                TournamentCSGOGroup entity = _dbContext.TournamentCSGOGroups.Find(id);
                if (entity != null)
                {
                    _dbContext.Entry(entity).CurrentValues.SetValues(group);
                }
                else
                {
                    group.TournamentId = _dbContext.Tournaments.Where(t => t.Active).First().ID;
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

        [HttpPut]
        [Route("sc2group")]
        public async Task<IActionResult> SubmitSC2Group(Guid? id, TournamentSC2Group group)
        {
            if (await UserIsInRole("event-admin"))
            {
                TournamentSC2Group entity = _dbContext.TournamentSC2Groups.Find(id);
                if (entity != null)
                {
                    _dbContext.Entry(entity).CurrentValues.SetValues(group);
                }
                else
                {
                    group.TournamentId = _dbContext.Tournaments.Where(t => t.Active).First().ID;
                    _dbContext.TournamentSC2Groups.Add(group);
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
        [Route("group")]
        public async Task<IActionResult> DeleteGroup(Guid id)
        {
            TournamentGroup entity;
            if (await UserIsInRole("event-admin"))
            {
                entity = _dbContext.TournamentCSGOGroups.Find(id);
                if (entity != null)
                {
                    foreach (var par in entity.Participants)
                    {
                        par.TournamentCSGOGroupId = null;
                    }
                    _dbContext.TournamentCSGOGroups.Remove(entity as TournamentCSGOGroup);
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
                entity = _dbContext.TournamentSC2Groups.Find(id);
                if (entity != null)
                {
                    foreach (var par in entity.Participants)
                    {
                        par.TournamentSC2GroupId = null;
                    }
                    _dbContext.TournamentSC2Groups.Remove(entity as TournamentSC2Group);
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
        public async Task<IActionResult> AddToGroup(Guid id, TournamentApplication participant)
        {
            TournamentGroup entity;
            if (await UserIsInRole("event-admin"))
            {
                entity = _dbContext.TournamentCSGOGroups.Find(id);
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
        public async Task<IActionResult> RemoveFromGroup(Guid id)
        {
            if (await UserIsInRole("event-admin"))
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
        #endregion

        #region MATCHES
        [AllowAnonymous]
        [Route("csgomatches")]
        public IActionResult GetCSGOMatches()
        {
            return Ok(_dbContext.TournamentCSGOMatches.OrderBy(m => m.StartTime).ToList());
        }

        [AllowAnonymous]
        [Route("csgomatch")]
        public IActionResult GetCSGOMatch(Guid id)
        {
            TournamentCSGOMatch match = _dbContext.TournamentCSGOMatches.Find(id);
            if (match != null)
                return Ok(match);
            return NotFound();
        }

        [AllowAnonymous]
        [Route("sc2matches")]
        public IActionResult GetSC2Matches()
        {
            return Ok(_dbContext.TournamentSC2Matches.OrderBy(m => m.StartTime).ToList());
        }

        [AllowAnonymous]
        [Route("sc2match")]
        public IActionResult GetSC2Match(Guid id)
        {
            TournamentSC2Match match = _dbContext.TournamentSC2Matches.Find(id);
            if (match != null)
                return Ok(match);
            return NotFound();
        }

        [HttpPut]
        [Route("csgomatch")]
        public async Task<IActionResult> SubmitCSGOMatch(Guid? id, TournamentCSGOMatch match)
        {
            if (await UserIsInRole("event-admin"))
            {
                TournamentCSGOMatch entity = _dbContext.TournamentCSGOMatches.Find(id);

                if (entity != null)
                {
                    _dbContext.Entry(entity).CurrentValues.SetValues(match);
                    match = entity;
                }
                else
                {
                    if (match.TournamentId == null)
                    {
                        match.TournamentId = _dbContext.Tournaments.Where(t => t.Active).First().ID;
                    }
                    _dbContext.TournamentCSGOMatches.Add(match);
                }

                try
                {
                    _dbContext.SaveChanges();
                }
                catch (DbUpdateException e)
                {
                    System.Diagnostics.Trace.TraceError("Tournament CS:GO match update exception: " + e.Message);
                    return BadRequest("Something went wrong...");
                }
                if (entity == null)
                {
                    await _dbContext.Entry(match).Reference(m => m.Team1).LoadAsync().ConfigureAwait(false);
                    await _dbContext.Entry(match).Reference(m => m.Team2).LoadAsync().ConfigureAwait(false);
                }

                return Ok(match);
            }
            return Unauthorized();
        }

        [HttpDelete]
        [Route("csgomatch")]
        public async Task<IActionResult> DeleteCSGOMatch(Guid? id)
        {
            if (await UserIsInRole("event-admin"))
            {
                TournamentCSGOMatch entity = _dbContext.TournamentCSGOMatches.Find(id);
                _dbContext.TournamentCSGOMatches.Remove(entity);

                try
                {
                    _dbContext.SaveChanges();
                }
                catch (DbUpdateException e)
                {
                    System.Diagnostics.Trace.TraceError("Tournament CS:GO match delete exception: " + e.Message);
                    return BadRequest("Something went wrong...");
                }
                return Ok(entity);
            }
            return Unauthorized();
        }

        [HttpPut]
        [Route("csgomatchmap")]
        public async Task<IActionResult> SubmitCSGOMatchMap(Guid? id, CSGOMatchMap map)
        {
            if (await UserIsInRole("event-admin"))
            {
                CSGOMatchMap entity = _dbContext.TournamentCSGOMatchMaps.Find(id);
                if (entity != null)
                {
                    _dbContext.Entry(entity).CurrentValues.SetValues(map);
                }
                else
                {
                    _dbContext.TournamentCSGOMatchMaps.Add(map);
                }

                try
                {
                    _dbContext.SaveChanges();
                }
                catch (DbUpdateException e)
                {
                    System.Diagnostics.Trace.TraceError("Tournament CS:GO match map update exception: " + e.Message);
                    return BadRequest("Something went wrong...");
                }
                return Ok(map);
            }
            return Unauthorized();
        }

        [HttpDelete]
        [Route("csgomatchmap")]
        public async Task<IActionResult> DeleteCSGOMatchMap(Guid? id)
        {
            if (await UserIsInRole("event-admin"))
            {
                CSGOMatchMap entity = _dbContext.TournamentCSGOMatchMaps.Find(id);
                _dbContext.TournamentCSGOMatchMaps.Remove(entity);

                try
                {
                    _dbContext.SaveChanges();
                }
                catch (DbUpdateException e)
                {
                    System.Diagnostics.Trace.TraceError("Tournament CS:GO match map delete exception: " + e.Message);
                    return BadRequest("Something went wrong...");
                }
                return Ok(entity);
            }
            return Unauthorized();
        }

        [HttpPut]
        [Route("sc2match")]
        public async Task<IActionResult> SubmitSC2Match(Guid? id, TournamentSC2Match match)
        {
            if (await UserIsInRole("event-admin"))
            {
                TournamentSC2Match entity = _dbContext.TournamentSC2Matches.Find(id);
                if (entity != null)
                {
                    _dbContext.Entry(entity).CurrentValues.SetValues(match);
                    match = entity;
                }
                else
                {
                    if (match.TournamentId == null)
                    {
                        match.TournamentId = _dbContext.Tournaments.Where(t => t.Active).First().ID;
                    }
                    _dbContext.TournamentSC2Matches.Add(match);
                }

                try
                {
                    _dbContext.SaveChanges();
                }
                catch (DbUpdateException e)
                {
                    System.Diagnostics.Trace.TraceError("Tournament StarCraft II match update exception: " + e.Message);
                    return BadRequest("Something went wrong...");
                }
                if (entity == null)
                {
                    await _dbContext.Entry(match).Reference(m => m.Player1).LoadAsync().ConfigureAwait(false);
                    await _dbContext.Entry(match).Reference(m => m.Player2).LoadAsync().ConfigureAwait(false);
                }
                return Ok(match);
            }
            return Unauthorized();
        }



        [HttpDelete]
        [Route("sc2match")]
        public async Task<IActionResult> DeleteSC2Match(Guid? id)
        {
            if (await UserIsInRole("event-admin"))
            {
                TournamentSC2Match entity = _dbContext.TournamentSC2Matches.Find(id);
                _dbContext.TournamentSC2Matches.Remove(entity);

                try
                {
                    _dbContext.SaveChanges();
                }
                catch (DbUpdateException e)
                {
                    System.Diagnostics.Trace.TraceError("Tournament StarCraft II match delete exception: " + e.Message);
                    return BadRequest("Something went wrong...");
                }
                return Ok(entity);
            }
            return Unauthorized();
        }

        [HttpPut]
        [Route("sc2matchmap")]
        public async Task<IActionResult> SubmitSC2MatchMap(Guid? id, SC2MatchMap map)
        {
            if (await UserIsInRole("event-admin"))
            {
                SC2MatchMap entity = _dbContext.TournamentSC2MatchMaps.Find(id);
                if (entity != null)
                {
                    _dbContext.Entry(entity).CurrentValues.SetValues(map);
                }
                else
                {
                    _dbContext.TournamentSC2MatchMaps.Add(map);
                }

                try
                {
                    _dbContext.SaveChanges();
                }
                catch (DbUpdateException e)
                {
                    System.Diagnostics.Trace.TraceError("Tournament CS:GO match map update exception: " + e.Message);
                    return BadRequest("Something went wrong...");
                }
                return Ok(map);
            }
            return Unauthorized();
        }

        [HttpDelete]
        [Route("sc2matchmap")]
        public async Task<IActionResult> DeleteSC2MatchMap(Guid? id)
        {
            if (await UserIsInRole("event-admin"))
            {
                SC2MatchMap entity = _dbContext.TournamentSC2MatchMaps.Find(id);
                _dbContext.TournamentSC2MatchMaps.Remove(entity);

                try
                {
                    _dbContext.SaveChanges();
                }
                catch (DbUpdateException e)
                {
                    System.Diagnostics.Trace.TraceError("Tournament CS:GO match map delete exception: " + e.Message);
                    return BadRequest("Something went wrong...");
                }
                return Ok(entity);
            }
            return Unauthorized();
        }
        #endregion
    }
}
