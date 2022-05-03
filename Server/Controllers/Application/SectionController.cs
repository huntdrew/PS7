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
    public class SectionController : BaseController<Section>, iBaseController<Section>
    {


        public SectionController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
                : base(context, httpContextAccessor)
        {

        }

        [HttpGet]
        [Route("GetSection")]
        public async Task<IActionResult> Get()
        {
            List<Section> itmSection = await _context.Sections.ToListAsync();
            return Ok(itmSection);
        }

        [HttpGet]
        [Route("GetSection/{pSchoolId}")]
        public async Task<IActionResult> Get(int pSchoolId)
        {
            Section itmSection = await _context.Sections.Where(x => x.SchoolId == pSchoolId).FirstOrDefaultAsync();
            return Ok(itmSection);
            var trans = _context.Database.BeginTransaction();
            trans.Rollback();
            return StatusCode(StatusCodes.Status500InternalServerError, "Error 404, Object Not Found");

        }

        [HttpGet]
        [Route("GetSection/{pSchoolId}, {pSectionId}")]

        public async Task<IActionResult> Get(int pSchoolId, int pSectionId)
        {
            Section itmSection = await _context.Sections.Where(x => x.SchoolId == pSchoolId && x.SectionId == pSectionId).FirstOrDefaultAsync();
            return Ok(itmSection);
        }
        [HttpDelete]
        [Route("Delete/{pSectionId}")]
        public async Task<IActionResult> Delete(int pSectionId)
        {
            Section itmSection = await _context.Sections.Where(x => x.SectionId == pSectionId).FirstOrDefaultAsync();
            _context.Remove(itmSection);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Section _Section)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Sect = await _context.Sections.Where(x => x.SectionId == _Section.SectionId).FirstOrDefaultAsync();
                if (_Sect == null)
                {
                    bExist = false;
                    _Sect = new Section();
                }
                else
                    bExist = true;
                _Sect.CourseNo = _Section.CourseNo;
                _Sect.SectionNo = _Section.SectionNo;
                _Sect.Location = _Section.Location;
                _Sect.InstructorId = _Section.InstructorId;
                _Sect.Capacity = _Section.Capacity;
                _Sect.SchoolId = _Section.SchoolId;
                if (bExist)
                    _context.Sections.Update(_Sect);
                else
                    _context.Sections.Add(_Sect);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Section.SectionId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Section _Section)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Sect = await _context.Sections.Where(x => x.SectionId == _Section.SectionId).FirstOrDefaultAsync();


                if (_Sect != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }



                _Sect = new Section();
                _Sect.CourseNo = _Section.CourseNo;
                _Sect.SectionNo = _Section.SectionNo;
                _Sect.Location = _Section.Location;
                _Sect.InstructorId = _Section.InstructorId;
                _Sect.Capacity = _Section.Capacity;
                _Sect.SchoolId = _Section.SchoolId;
                if (bExist)
                    _context.Sections.Update(_Sect);
                else
                    _context.Sections.Add(_Sect);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Section.SectionId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
