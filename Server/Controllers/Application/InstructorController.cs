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
    public class InstructorController : BaseController<Instructor>, iBaseController<Instructor>
    {


        public InstructorController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
                : base(context, httpContextAccessor)
        {

        }

        [HttpGet]
        [Route("GetInstructor")]
        public async Task<IActionResult> Get()
        {
            List<Instructor> itmInstructor = await _context.Instructors.ToListAsync();
            return Ok(itmInstructor);
        }

        [HttpGet]
        [Route("GetInstructor/{pSchoolId}")]
        public async Task<IActionResult> Get(int pSchoolId)
        {
            Instructor itmInstructor = await _context.Instructors.Where(x => x.SchoolId == pSchoolId).FirstOrDefaultAsync();
            var trans = _context.Database.BeginTransaction();
            trans.Rollback();
            return StatusCode(StatusCodes.Status500InternalServerError, "Error 404, Object Not Found");

        }

        [HttpGet]
        [Route("GetInstructor/{pSchoolId}, {pSectionId}, {pGradeTypeCode}")]

        public async Task<IActionResult> Get(int pSchoolId, int pInstructorId)
        {
            Instructor itmInstructor = await _context.Instructors.Where(x => x.SchoolId == pSchoolId && x.InstructorId == pInstructorId).FirstOrDefaultAsync();
            return Ok(itmInstructor);
        }

        [HttpDelete]
        [Route("Delete/{pInstructorId}")]
        public async Task<IActionResult> Delete(int pInstructorId)
        {
            Instructor itmInstructor = await _context.Instructors.Where(x => x.InstructorId == pInstructorId).FirstOrDefaultAsync();
            _context.Remove(itmInstructor);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Instructor _Instructor)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Instr = await _context.Instructors.Where(x => x.InstructorId == _Instructor.InstructorId).FirstOrDefaultAsync();
                if (_Instr == null)
                {
                    bExist = false;
                    _Instr = new Instructor();
                }
                else
                    bExist = true;
                _Instr.Salutation = _Instructor.Salutation;
                _Instr.FirstName = _Instructor.FirstName;
                _Instr.LastName = _Instructor.LastName;
                _Instr.StreetAddress = _Instructor.StreetAddress;
                _Instr.Zip = _Instructor.Zip;
                _Instr.Phone = _Instructor.Phone;
                _Instr.SchoolId = _Instructor.SchoolId;
                if (bExist)
                    _context.Instructors.Update(_Instr);
                else
                    _context.Instructors.Add(_Instr);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Instructor.InstructorId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Instructor _Instructor)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Instr = await _context.Instructors.Where(x => x.InstructorId == _Instructor.InstructorId).FirstOrDefaultAsync();


                if (_Instr != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }



                _Instr = new Instructor();
                _Instr.Salutation = _Instructor.Salutation;
                _Instr.FirstName = _Instructor.FirstName;
                _Instr.LastName = _Instructor.LastName;
                _Instr.StreetAddress = _Instructor.StreetAddress;
                _Instr.Zip = _Instructor.Zip;
                _Instr.Phone = _Instructor.Phone;
                _Instr.SchoolId = _Instructor.SchoolId;
                if (bExist)
                    _context.Instructors.Update(_Instr);
                else
                    _context.Instructors.Add(_Instr);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Instructor.InstructorId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
