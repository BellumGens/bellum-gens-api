using BellumGens.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BellumGens.Api.Controllers
{
    [Authorize]
    public class TournamentController : BaseController
    {
        private readonly BellumGensDbContext _dbContext = new BellumGensDbContext();
        private const string emailConfirmation = "Greetings,<br /><br />You have successfully registered for the Esports Business League.<br /><br />To confirm your email address click on this <a href='{0}' target='_blank'>link</a>.<br /><br />The Bellum Gens team<br /><br /><a href='https://bellumgens.com' target='_blank'>https://bellumgens.com</a>";

        public IHttpActionResult Register(TournamentApplication application)
        {
            if (ModelState.IsValid)
            {
                Company c = _dbContext.Companies.Find(application.CompanyId);
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
                _dbContext.TournamentApplications.Add(application);
                
                try
                {
                    _dbContext.SaveChanges();
                }
                catch (Exception e)
                {

                }
                return Ok(application);
            }
            return BadRequest("The application didn't validate");
        }
    }
}
