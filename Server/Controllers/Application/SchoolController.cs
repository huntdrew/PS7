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
    public class SchoolController : BaseController<School>, iBaseController<School>
    {


        public SchoolController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
                : base(context, httpContextAccessor)
        {

        }

        [HttpGet]
        [Route("GetSchool")]
        public async Task<IActionResult> Get()
        {
            List<School> itmSchool = await _context.Schools.ToListAsync();
            return Ok(itmSchool);
        }

        [HttpGet]
        [Route("GetSchool/{pSchoolId}")]
        public async Task<IActionResult> Get(int pSchoolId)
        {
            School itmSchool = await _context.Schools.Where(x => x.SchoolId == pSchoolId).FirstOrDefaultAsync();
            return Ok(itmSchool);
        }

        [HttpDelete]
        [Route("Delete/{pSchoolId}")]
        public async Task<IActionResult> Delete(int pSchoolId)
        {
            School itmSchool = await _context.Schools.Where(x => x.SchoolId == pSchoolId).FirstOrDefaultAsync();
            _context.Remove(itmSchool);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] School _School)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Sch = await _context.Schools.Where(x => x.SchoolId == _School.SchoolId).FirstOrDefaultAsync();
                if (_Sch == null)
                {
                    bExist = false;
                    _Sch = new School();
                }
                else
                    bExist = true;
                _Sch.SchoolName = _School.SchoolName;
                if (bExist)
                    _context.Schools.Update(_Sch);
                else
                    _context.Schools.Add(_Sch);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_School.SchoolId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] School _School)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Sch = await _context.Schools.Where(x => x.SchoolId == _School.SchoolId).FirstOrDefaultAsync();


                if (_Sch != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }



                _Sch = new School();
                _Sch.SchoolName = _School.SchoolName;
                if (bExist)
                    _context.Schools.Update(_Sch);
                else
                    _context.Schools.Add(_Sch);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_School.SchoolId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
