using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using BellumGens.Api.Models;
using Newtonsoft.Json;

namespace BellumGens.Api.Controllers
{
    public class CompaniesController : BaseController
    {

        // GET: api/Companies
        public List<string> GetCompanies()
        {
            return _dbContext.Companies.Select(c => c.Name).ToList();
        }

        //// GET: api/Companies/5
        //[ResponseType(typeof(Company))]
        //public async Task<IHttpActionResult> GetCompany(string id)
        //{
        //    Company company = await db.Companies.FindAsync(id);
        //    if (company == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(company);
        //}

        //// PUT: api/Companies/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PutCompany(string id, Company company)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != company.Name)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(company).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CompanyExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        //// POST: api/Companies
        //[ResponseType(typeof(Company))]
        //public async Task<IHttpActionResult> PostCompany(Company company)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Companies.Add(company);

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (CompanyExists(company.Name))
        //        {
        //            return Conflict();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return CreatedAtRoute("ActionApi", new { id = company.Name }, company);
        //}

        //// DELETE: api/Companies/5
        //[ResponseType(typeof(Company))]
        //public async Task<IHttpActionResult> DeleteCompany(string id)
        //{
        //    Company company = await db.Companies.FindAsync(id);
        //    if (company == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Companies.Remove(company);
        //    await db.SaveChangesAsync();

        //    return Ok(company);
        //}

        //private bool CompanyExists(string id)
        //{
        //    return _dbContext.Companies.Count(e => e.Name == id) > 0;
        //}
    }
}