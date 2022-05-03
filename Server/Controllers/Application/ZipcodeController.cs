using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SWARM.EF.Data;
using SWARM.EF.Models;
using SWARM.Server.Controllers.Base;
using SWARM.Server.Models;
using SWARM.Shared;
using SWARM.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Telerik.DataSource;
using Telerik.DataSource.Extensions;

namespace SWARM.Server.Application.Crse
{
    public class ZipcodeController : BaseController<Zipcode>, iBaseController<Zipcode>
    {


        public ZipcodeController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
                : base(context, httpContextAccessor)
        {

        }

        [HttpGet]
        [Route("GetZipcode")]
        public async Task<IActionResult> Get()
        {
            List<Zipcode> itmZipcode = await _context.Zipcodes.ToListAsync();
            return Ok(itmZipcode);
        }

        [HttpGet]
        [Route("GetZipcode/{pZip}")]
        public async Task<IActionResult> Get(string pZip)
        {
            Zipcode itmZipcode = await _context.Zipcodes.Where(x => x.Zip == pZip).FirstOrDefaultAsync();
            return Ok(itmZipcode);
        }

        [HttpDelete]
        [Route("Delete/{pZip}")]
        public async Task<IActionResult> Delete(string pZip)
        {
            Zipcode itmZipcode = await _context.Zipcodes.Where(x => x.Zip == pZip).FirstOrDefaultAsync();
            _context.Remove(itmZipcode);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Zipcode _Zipcode)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Zip = await _context.Zipcodes.Where(x => x.Zip == _Zipcode.Zip).FirstOrDefaultAsync();
                if (_Zip == null)
                {
                    bExist = false;
                    _Zip = new Zipcode();
                }
                else
                    bExist = true;
                _Zip.City = _Zipcode.City;
                _Zip.State = _Zipcode.State;
                if (bExist)
                    _context.Zipcodes.Update(_Zipcode);
                else
                    _context.Zipcodes.Add(_Zipcode);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Zipcode.Zip);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Zipcode _Zipcode)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Zip = await _context.Zipcodes.Where(x => x.Zip == _Zipcode.Zip).FirstOrDefaultAsync();


                if (_Zip != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }
              
                    _context.Zipcodes.Add(_Zipcode);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Zipcode.Zip);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        public Task<IActionResult> Delete(int itemID)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> Get(int itemID)
        {
            throw new NotImplementedException();
        }
    }
}
