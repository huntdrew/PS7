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
    public class GradeTypeController : BaseController<GradeType>, iBaseController<GradeType>
    {


        public GradeTypeController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
                : base(context, httpContextAccessor)
        {

        }

        [HttpGet]
        [Route("GetGradeType")]
        public async Task<IActionResult> Get()
        {
            List<GradeType> itmGradeType = await _context.GradeTypes.ToListAsync();
            return Ok(itmGradeType);
        }

        [HttpGet]
        [Route("GetGradeType/{pSchoolId}")]
        public async Task<IActionResult> Get(int pSchoolId)
        {
            GradeType itmGradeType = await _context.GradeTypes.Where(x => x.SchoolId == pSchoolId).FirstOrDefaultAsync();
            var trans = _context.Database.BeginTransaction();
            trans.Rollback();
            return StatusCode(StatusCodes.Status500InternalServerError, "Error 404, Object Not Found");

        }

        [HttpGet]
        [Route("GetGradeType/{pSchoolId}, {pGradeTypeCode}")]

        public async Task<IActionResult> Get(int pSchoolId, int pGradeTypeCode)
        {
            GradeType itmGradeType = await _context.GradeTypes.Where(x => x.SchoolId == pSchoolId && x.GradeTypeCode == "pGradeTypeCode").FirstOrDefaultAsync();
            return Ok(itmGradeType);
        }


        [HttpDelete]
        [Route("Delete/{pGradeTypeCode}")]
        public async Task<IActionResult> Delete(int pGradeTypeCode)
        {
            GradeType itmGradeType = await _context.GradeTypes.Where(x => x.GradeTypeCode == "pGradeTypeCode").FirstOrDefaultAsync();
            _context.Remove(itmGradeType);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] GradeType _GradeType)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var _GrT = await _context.GradeTypes.Where(x => x.GradeTypeCode == _GradeType.GradeTypeCode).FirstOrDefaultAsync();
                if (_GrT == null)
                {
                    bExist = false;
                    _GrT = new GradeType();
                }
                else
                    bExist = true;
                _GrT.Description = _GradeType.Description;
                _GrT.GradeTypeCode = _GradeType.GradeTypeCode;
                _GrT.SchoolId = _GradeType.SchoolId;
                if (bExist)
                    _context.GradeTypes.Update(_GrT);
                else
                    _context.GradeTypes.Add(_GrT);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_GradeType.GradeTypeCode);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GradeType _GradeType)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var _GrT = await _context.GradeTypes.Where(x => x.GradeTypeCode == _GradeType.GradeTypeCode).FirstOrDefaultAsync();


                if (_GrT != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }



                _GrT = new GradeType();
                _GrT.Description = _GradeType.Description;
                _GrT.SchoolId = _GradeType.SchoolId;
                if (bExist)
                    _context.GradeTypes.Update(_GrT);
                else
                    _context.GradeTypes.Add(_GrT);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_GradeType.GradeTypeCode);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
