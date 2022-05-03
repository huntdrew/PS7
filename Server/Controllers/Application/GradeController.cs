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
    public class GradeController : BaseController<Grade>, iBaseController<Grade>
    {


        public GradeController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
                : base(context, httpContextAccessor)
        {

        }

        [HttpGet]
        [Route("GetGrade")]
        public async Task<IActionResult> Get()
        {
            List<Grade> itmGrade = await _context.Grades.ToListAsync();
            return Ok(itmGrade);
        }

        [HttpGet]
        [Route("GetGrade/{pStudentId}")]
        public async Task<IActionResult> Get(int pStudentId)
        {
            Grade itmGrade = await _context.Grades.Where(x => x.StudentId == pStudentId).FirstOrDefaultAsync();
            var trans = _context.Database.BeginTransaction();
            trans.Rollback();
            return StatusCode(StatusCodes.Status500InternalServerError, "Error 404, Object Not Found");

        }

        [HttpGet]
        [Route("GetGrade/{pSectionId},{pStudentId},{pSchoolId}, {pGradeTypeCode}, {pGradeCodeOccurrence}")]

        public async Task<IActionResult> Get(int pSectionId, int pStudentId, int pSchoolId, int pGradeTypeCode,  int pGradeCodeOccurrence)
        {
            Grade itmGrade = await _context.Grades.Where(x => x.SectionId == pSectionId && x.StudentId == pStudentId && x.SchoolId == pSchoolId 
            && x.GradeTypeCode == "pGradeTypeCode" && x.GradeCodeOccurrence == pGradeCodeOccurrence).FirstOrDefaultAsync();
            return Ok(itmGrade);
        }


        [HttpDelete]
        [Route("Delete/{pSchoolId}")]
        public async Task<IActionResult> Delete(int pSchoolId)
        {
            Grade itmGrade = await _context.Grades.Where(x => x.SchoolId == pSchoolId).FirstOrDefaultAsync();
            _context.Remove(itmGrade);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Grade _Grade)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Gr = await _context.Grades.Where(x => x.SchoolId == _Grade.SchoolId).FirstOrDefaultAsync();
                if (_Gr == null)
                {
                    bExist = false;
                    _Gr = new Grade();
                }
                else
                    bExist = true;
                _Gr.StudentId = _Grade.StudentId;
                _Gr.SectionId = _Grade.SectionId;
                _Gr.SchoolId = _Grade.SchoolId;
                _Gr.GradeTypeCode = _Grade.GradeTypeCode;
                _Gr.GradeCodeOccurrence = _Grade.GradeCodeOccurrence;
                _Gr.NumericGrade = _Grade.NumericGrade;
                _Gr.Comments = _Grade.Comments;

                if (bExist)
                    _context.Grades.Update(_Gr);
                else
                    _context.Grades.Add(_Gr);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Grade.SchoolId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Grade _Grade)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Gr = await _context.Grades.Where(x => x.SchoolId == _Grade.SchoolId).FirstOrDefaultAsync();


                if (_Gr != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }



                _Gr = new Grade();
                _Gr.StudentId = _Grade.StudentId;
                _Gr.SectionId = _Grade.SectionId;
                _Gr.SchoolId = _Grade.SchoolId;
                _Gr.GradeTypeCode = _Grade.GradeTypeCode;
                _Gr.GradeCodeOccurrence = _Grade.GradeCodeOccurrence;
                _Gr.NumericGrade = _Grade.NumericGrade;
                _Gr.Comments = _Grade.Comments;

                if (bExist)
                    _context.Grades.Update(_Gr);
                else
                    _context.Grades.Add(_Gr);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Grade.SchoolId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
