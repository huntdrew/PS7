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
    public class GradeTypeWeightController : BaseController<GradeTypeWeight>, iBaseController<GradeTypeWeight>
    {


        public GradeTypeWeightController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
                : base(context, httpContextAccessor)
        {

        }

        [HttpGet]
        [Route("GetGradeTypeWeight")]
        public async Task<IActionResult> Get()
        {
            List<GradeTypeWeight> itmGradeTypeWeight = await _context.GradeTypeWeights.ToListAsync();
            return Ok(itmGradeTypeWeight);
        }

        

        [HttpGet]
        [Route("GetGradeTypeWeight/{pSchoolId}")]
        public async Task<IActionResult> Get(int pSchoolId)
        {
            GradeTypeWeight itmGradeTypeWeight = await _context.GradeTypeWeights.Where(x => x.SchoolId == pSchoolId).FirstOrDefaultAsync();
            var trans = _context.Database.BeginTransaction();
            trans.Rollback();
            return StatusCode(StatusCodes.Status500InternalServerError, "Error 404, Object Not Found");

        }

        [HttpGet]
        [Route("GetGradeTypeWeight/{pSchoolId}, {pSectionId}, {pGradeTypeCode}")]

        public async Task<IActionResult> Get(int pSchoolId, int pSectionId, int pGradeTypeCode)
        {
            GradeTypeWeight itmGradeTypeWeight = await _context.GradeTypeWeights.Where(x => x.SchoolId == pSchoolId && x.SectionId == pSectionId && x.GradeTypeCode == "pGradeTypeCode").FirstOrDefaultAsync();
            return Ok(itmGradeTypeWeight);
        }


        [HttpDelete]
        [Route("Delete/{pGradeTypeCode}")]
        public async Task<IActionResult> Delete(int pGradeTypeCode)
        {
            GradeTypeWeight itmGradeTypeWeight = await _context.GradeTypeWeights.Where(x => x.GradeTypeCode == "pGradeTypeCode").FirstOrDefaultAsync();
            _context.Remove(itmGradeTypeWeight);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] GradeTypeWeight _GradeTypeWeight)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var _GrTW = await _context.GradeTypeWeights.Where(x => x.GradeTypeCode == _GradeTypeWeight.GradeTypeCode).FirstOrDefaultAsync();
                if (_GrTW == null)
                {
                    bExist = false;
                    _GrTW = new GradeTypeWeight();
                }
                else
                    bExist = true;
                _GrTW.SchoolId = _GradeTypeWeight.SchoolId;
                _GrTW.SectionId = _GradeTypeWeight.SectionId;
                _GrTW.GradeTypeCode = _GradeTypeWeight.GradeTypeCode;
                _GrTW.NumberPerSection = _GradeTypeWeight.NumberPerSection;
                _GrTW.PercentOfFinalGrade = _GradeTypeWeight.PercentOfFinalGrade;
                _GrTW.DropLowest = _GradeTypeWeight.DropLowest;
                
                if (bExist)
                    _context.GradeTypeWeights.Update(_GrTW);
                else
                    _context.GradeTypeWeights.Add(_GrTW);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_GradeTypeWeight.GradeTypeCode);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GradeTypeWeight _GradeTypeWeight)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var _GrTW = await _context.GradeTypeWeights.Where(x => x.GradeTypeCode == _GradeTypeWeight.GradeTypeCode).FirstOrDefaultAsync();


                if (_GrTW != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }



                _GrTW = new GradeTypeWeight();
                _GrTW.NumberPerSection = _GradeTypeWeight.NumberPerSection;
                _GrTW.PercentOfFinalGrade = _GradeTypeWeight.PercentOfFinalGrade;
                _GrTW.DropLowest = _GradeTypeWeight.DropLowest;
                _GrTW.SchoolId = _GradeTypeWeight.SchoolId;
                if (bExist)
                    _context.GradeTypeWeights.Update(_GrTW);
                else
                    _context.GradeTypeWeights.Add(_GrTW);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_GradeTypeWeight.GradeTypeCode);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
