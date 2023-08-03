using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DockeroDummy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        // GET: api/<EmployeeController>
        [HttpGet]
        [Route("getemployeelist")]
        public IEnumerable<EmployeeModel> GetEmployeeList()
        {

            return Employee.Employees();
        }

        // GET api/<EmployeeController>/5
        [HttpPost]
        [Route("getemployee")]
        public EmployeeModel GetEmployee(int EmployeeId)
        {
            return Employee.Employees().FirstOrDefault(x => x.EmployeeId == EmployeeId);
        }

        // POST api/<EmployeeController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<EmployeeController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<EmployeeController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
