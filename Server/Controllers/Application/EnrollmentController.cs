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
    public class EnrollmentController : BaseController<Enrollment>, iBaseController<Enrollment>
    {


        public EnrollmentController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
                : base(context, httpContextAccessor)
        {

        }

        [HttpGet]
        [Route("GetEnrollment")]
        public async Task<IActionResult> Get()
        {
            List<Enrollment> itmEnrollment = await _context.Enrollments.ToListAsync();
            return Ok(itmEnrollment);
        }


        [HttpGet]
        [Route("GetEnrollment/{pStudentId}")]
        public async Task<IActionResult> Get(int pStudentId)
        {
            Enrollment itmEnrollment = await _context.Enrollments.Where(x => x.StudentId == pStudentId).FirstOrDefaultAsync();
            var trans = _context.Database.BeginTransaction();
            trans.Rollback();
            return StatusCode(StatusCodes.Status500InternalServerError,"Error 404, Object Not Found");
       
        }

        [HttpGet]
        [Route("GetEnrollment/{pSectionId}/{pStudentId}/{pSchoolId}")]

        public async Task<IActionResult> Get(int pSectionId, int pStudentId, int pSchoolId)
         {
            Enrollment itmEnrollment = await _context.Enrollments.Where(x => x.SectionId == pSectionId && x.StudentId == pStudentId && x.SchoolId == pSchoolId).FirstOrDefaultAsync();
            return Ok(itmEnrollment);
        }


    [HttpDelete]
        [Route("Delete/{pStudentId}")]
        public async Task<IActionResult> Delete(int pStudentId)
        {
            Enrollment itmEnrollment = await _context.Enrollments.Where(x => x.StudentId == pStudentId).FirstOrDefaultAsync();
            _context.Remove(itmEnrollment);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Enrollment _Enrollment)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Enr = await _context.Enrollments.Where(x => x.StudentId == _Enrollment.StudentId).FirstOrDefaultAsync();
                if (_Enr == null)
                {
                    bExist = false;
                    _Enr = new Enrollment();
                }
                else
                    bExist = true;
                _Enr.EnrollDate = _Enrollment.EnrollDate;
                _Enr.SectionId = _Enrollment.SectionId;
                _Enr.FinalGrade = _Enrollment.FinalGrade;
                _Enr.SchoolId = _Enrollment.SchoolId;
                if (bExist)
                    _context.Enrollments.Update(_Enr);
                else
                    _context.Enrollments.Add(_Enr);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Enrollment.StudentId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Enrollment _Enrollment)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Enr = await _context.Enrollments.Where(x => x.StudentId == _Enrollment.StudentId).FirstOrDefaultAsync();


                if (_Enr != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }



                _Enr = new Enrollment();
                _Enr.FinalGrade = _Enrollment.FinalGrade;
                _Enr.SchoolId = _Enrollment.SchoolId;
                if (bExist)
                    _context.Enrollments.Update(_Enr);
                else
                    _context.Enrollments.Add(_Enr);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Enrollment.StudentId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
