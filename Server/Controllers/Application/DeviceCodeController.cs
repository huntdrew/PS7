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
    public class DeviceCodeController : BaseController<DeviceCode>, iBaseController<DeviceCode>
    {


        public DeviceCodeController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
                : base(context, httpContextAccessor)
        {

        }

        [HttpGet]
        [Route("GetDeviceCode")]
        public async Task<IActionResult> Get()
        {
            List<DeviceCode> itmDeviceCode = await _context.DeviceCodes.ToListAsync();
            return Ok(itmDeviceCode);
        }

        [HttpGet]
        [Route("GetDeviceCode/{pUserCode}")]
        public async Task<IActionResult> Get(int pUserCode)
        {
            DeviceCode itmDeviceCode = await _context.DeviceCodes.Where(x => x.UserCode == "pUserCode").FirstOrDefaultAsync();
            return Ok(itmDeviceCode);
        }

        [HttpDelete]
        [Route("Delete/{pUserCode}")]
        public async Task<IActionResult> Delete(int pUserCode)
        {
            DeviceCode itmDeviceCode = await _context.DeviceCodes.Where(x => x.UserCode == "pUserCode").FirstOrDefaultAsync();
            _context.Remove(itmDeviceCode);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] DeviceCode _DeviceCode)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var _DevC = await _context.DeviceCodes.Where(x => x.UserCode == _DeviceCode.UserCode).FirstOrDefaultAsync();
                if (_DevC == null)
                {
                    bExist = false;
                    _DevC = new DeviceCode();
                }
                else
                    bExist = true;
                _DevC.DeviceCode1 = _DeviceCode.DeviceCode1;
                _DevC.SubjectId= _DeviceCode.SubjectId;
                _DevC.SessionId = _DeviceCode.SessionId;
                _DevC.ClientId = _DeviceCode.ClientId;
                _DevC.Description = _DeviceCode.Description;
                if (bExist)
                    _context.DeviceCodes.Update(_DevC);
                else
                    _context.DeviceCodes.Add(_DevC);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_DeviceCode.UserCode);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] DeviceCode _DeviceCode)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var _DevC = await _context.DeviceCodes.Where(x => x.UserCode == _DeviceCode.UserCode).FirstOrDefaultAsync();


                if (_DevC != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }

                _DevC = new DeviceCode();
                _DevC.DeviceCode1 = _DeviceCode.DeviceCode1;
                _DevC.SubjectId = _DeviceCode.SubjectId;
                _DevC.SessionId = _DeviceCode.SessionId;
                _DevC.ClientId = _DeviceCode.ClientId;
                _DevC.Description = _DeviceCode.Description;
                if (bExist)
                    _context.DeviceCodes.Update(_DevC);
                else
                    _context.DeviceCodes.Add(_DevC);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_DeviceCode.UserCode);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
