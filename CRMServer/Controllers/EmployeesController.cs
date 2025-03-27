using CRMServer.Data;
using CRMServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRMServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        //DI
        private readonly AppDbContext _appDbcontext;
        public EmployeesController(AppDbContext appDbContext)
        {
            _appDbcontext = appDbContext;
            //_appDbcontext.Employees
        }
        //GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            return await _appDbcontext.Employees.ToListAsync();
        }
        //https://localhost:7206/api/Employees
        [HttpGet("{Id}")]
        public async Task<ActionResult<Employee>> GetEmployeeById(int id)
        {
           var emp = await _appDbcontext.Employees.FindAsync(id);
            //var emp = await _appDbcontext.Employees.FirstOrDefaultAsync(e => e.Id == id);
            if (emp == null)
            {
                return NotFound();
            }
            return emp;
            // return Ok(emp);
        }

    }
}
