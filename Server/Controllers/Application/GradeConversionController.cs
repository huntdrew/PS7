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
    public class GradeConversionController : BaseController<GradeConversion>, iBaseController<GradeConversion>
    {


        public GradeConversionController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
                : base(context, httpContextAccessor)
        {

        }

        [HttpGet]
        [Route("GetGradeConversion")]
        public async Task<IActionResult> Get()
        {
            List<GradeConversion> itmGradeConversion = await _context.GradeConversions.ToListAsync();
            return Ok(itmGradeConversion);
        }
        [HttpGet]
        [Route("GetGradeConversion/{pSchoolId}")]
        public async Task<IActionResult> Get(int pSchoolId)
        {
            GradeConversion itmGradeConversion = await _context.GradeConversions.Where(x => x.SchoolId == pSchoolId).FirstOrDefaultAsync();
            var trans = _context.Database.BeginTransaction();
            trans.Rollback();
            return StatusCode(StatusCodes.Status500InternalServerError, "Error 404, Object Not Found");

        }

        [HttpGet]
        [Route("GetGradeConversion/{pSchoolId}, {pLetterGrade}")]

        public async Task<IActionResult> Get(int pSchoolId, int pLetterGrade)
        {
            GradeConversion itmGradeConversion = await _context.GradeConversions.Where(x => x.SchoolId == pSchoolId && x.LetterGrade == "pLetterGrade").FirstOrDefaultAsync();
            return Ok(itmGradeConversion);
        }

        [HttpDelete]
        [Route("Delete/{pSchoolId}")]
        public async Task<IActionResult> Delete(int pSchoolId)
        {
            GradeConversion itmGradeConversion = await _context.GradeConversions.Where(x => x.SchoolId == pSchoolId).FirstOrDefaultAsync();
            _context.Remove(itmGradeConversion);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] GradeConversion _GradeConversion)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var _GrConv = await _context.GradeConversions.Where(x => x.SchoolId == _GradeConversion.SchoolId).FirstOrDefaultAsync();
                if (_GrConv == null)
                {
                    bExist = false;
                    _GrConv = new GradeConversion();
                }
                else
                    bExist = true;
                _GrConv.LetterGrade = _GradeConversion.LetterGrade;
                _GrConv.GradePoint = _GradeConversion.GradePoint;
                _GrConv.MaxGrade = _GradeConversion.MaxGrade;
                _GrConv.MinGrade = _GradeConversion.MinGrade;
                _GrConv.SchoolId = _GradeConversion.SchoolId;
                if (bExist)
                    _context.GradeConversions.Update(_GrConv);
                else
                    _context.GradeConversions.Add(_GrConv);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_GradeConversion.SchoolId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GradeConversion _GradeConversion)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var _GrConv = await _context.GradeConversions.Where(x => x.SchoolId == _GradeConversion.SchoolId).FirstOrDefaultAsync();


                if (_GrConv != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }



                _GrConv = new GradeConversion();
                _GrConv.LetterGrade = _GradeConversion.LetterGrade;
                _GrConv.GradePoint = _GradeConversion.GradePoint;
                _GrConv.MaxGrade = _GradeConversion.MaxGrade;
                _GrConv.MinGrade = _GradeConversion.MinGrade;
                _GrConv.SchoolId = _GradeConversion.SchoolId;
                if (bExist)
                    _context.GradeConversions.Update(_GrConv);
                else
                    _context.GradeConversions.Add(_GrConv);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_GradeConversion.SchoolId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
