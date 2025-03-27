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

        //
        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee(Employee emp)
        {
            _appDbcontext.Employees.Add(emp);
            await _appDbcontext.SaveChangesAsync();

            return CreatedAtAction("GetEmployeeById", new { id = emp.Id }, emp);
        }
        //https://localhost:7206/api/Employees
        [HttpGet("{id}")]
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

        //https://localhost:7206/api/Employees
        [HttpPut("{id}")]
        public async Task<ActionResult<Employee>> UpdateEmployeeById(int id, Employee emp)
        {
            if (id != emp.Id)
            {
                BadRequest();
            }
            _appDbcontext.Entry(emp).State = EntityState.Modified;

            try
            {
                await _appDbcontext.SaveChangesAsync(); // calistirilacak kod blogu
            }
            catch (Exception ex) 
            {
                if (!EmployeeExists(id))
                {
                    return NotFound(ex);
                }
                else
                {
                    throw; //bos gecerli bir hata firlatir
                }

            }
            return NoContent();
        }
        private  bool EmployeeExists(int id)
        {
            return _appDbcontext.Employees.Any(e => e.Id == id);
            //any veritabani sorgulari icin kullanilir
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Employee>> DeleteEmployeeById(int id)
        {
            var emp = await _appDbcontext.Employees.FindAsync(id);
            if (emp == null)
            {
                return NotFound();
            }
            _appDbcontext.Employees.Remove(emp);
            await _appDbcontext.SaveChangesAsync();
            return NoContent();
        }
    }
}
