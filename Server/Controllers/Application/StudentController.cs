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
    public class StudentController : BaseController<Student>, iBaseController<Student>
    {


        public StudentController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor)
                : base(context, httpContextAccessor)
        {

        }

        [HttpGet]
        [Route("GetStudent")]
        public async Task<IActionResult> Get()
        {
            List<Student> itmStudent = await _context.Students.ToListAsync();
            return Ok(itmStudent);
        }

        [HttpGet]
        [Route("GetStudent/{pStudentId}")]
        public async Task<IActionResult> Get(int pStudentId)
        {
            Student itmStudent = await _context.Students.Where(x => x.StudentId == pStudentId).FirstOrDefaultAsync();
            return Ok(itmStudent);
        }

        [HttpDelete]
        [Route("Delete/{pStudentId}")]
        public async Task<IActionResult> Delete(int pStudentId)
        {
            Student itmStudent = await _context.Students.Where(x => x.StudentId == pStudentId).FirstOrDefaultAsync();
            _context.Remove(itmStudent);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Student _Student)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Stu = await _context.Students.Where(x => x.StudentId == _Student.StudentId).FirstOrDefaultAsync();
                if (_Stu == null)
                {
                    bExist = false;
                    _Stu = new Student();
                }
                else
                    bExist = true;
                _Stu.Salutation = _Student.Salutation;
                _Stu.FirstName = _Student.FirstName;
                _Stu.LastName = _Student.LastName;
                _Stu.StreetAddress = _Student.StreetAddress;
                _Stu.Zip = _Student.Zip;
                _Stu.Phone = _Student.Phone;
                _Stu.Employer = _Student.Employer;
                _Stu.RegistrationDate = _Student.RegistrationDate;

                if (bExist)
                    _context.Students.Update(_Stu);
                else
                    _context.Students.Add(_Stu);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Student.StudentId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Student _Student)
        {
            bool bExist = false;

            var trans = _context.Database.BeginTransaction();
            try
            {
                var _Stu = await _context.Students.Where(x => x.StudentId == _Student.StudentId).FirstOrDefaultAsync();


                if (_Stu != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Record Exists");
                }



                _Stu = new Student();
                _Stu.Salutation = _Student.Salutation;
                _Stu.FirstName = _Student.FirstName;
                _Stu.LastName = _Student.LastName;
                _Stu.StreetAddress = _Student.StreetAddress;
                _Stu.Zip = _Student.Zip;
                _Stu.Phone = _Student.Phone;
                _Stu.Employer = _Student.Employer;
                _Stu.RegistrationDate = _Student.RegistrationDate;
                if (bExist)
                    _context.Students.Update(_Stu);
                else
                    _context.Students.Add(_Stu);

                await _context.SaveChangesAsync();
                trans.Commit();

                return Ok(_Student.StudentId);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
