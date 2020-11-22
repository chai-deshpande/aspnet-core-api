using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using Student.Api.Repositories;
using Student.Api.Requests;
using Student = Student.Api.Models.Student;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Student.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class StudentsController : ControllerBase
  {
    private readonly ILogger<StudentsController> _logger;
    private readonly IStudentDocumentRepository _studentDocumentRepository;
    public IConfiguration Configuration { get; }

    public StudentsController(IConfiguration configuration, ILogger<StudentsController> logger, IStudentDocumentRepository studentDocumentRepository)
    {
      _logger = logger;
      _studentDocumentRepository = studentDocumentRepository;
      Configuration = configuration;
    }

    // GET: api/<StudentsController>
    [HttpGet]
    public IEnumerable<string> Get()
    {
      _logger.LogInformation("api/students list called");
      _logger.LogError("api/students list - sample error message");
      return new string[] { "value1", "value2", Configuration["Exceptional:ErrorStore:ApplicationName"] , Configuration["Exceptional:ErrorStore:Type"] };
    }

    // GET api/<StudentsController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
      return "value";
    }

    // POST api/<StudentsController>
    [HttpPost]
    public void Post([FromBody] StudentRequest student)
    {
      _studentDocumentRepository.AddAsync(new Models.Student {FirstName = student.FirstName, LastName = student.LastName, Age = student.Age, DateOfBirth = student.DateOfBirth}, student.LastName);
    }

    // PUT api/<StudentsController>/5
    [HttpPut("{id}")]
    public void Put(Guid id, [FromBody] StudentRequest student)
    {
      _studentDocumentRepository.UpdateAsync(new Models.Student {Id = id.ToString(), FirstName = student.FirstName, LastName = student.LastName, Age = student.Age, DateOfBirth = student.DateOfBirth}, student.LastName);
    }

    // DELETE api/<StudentsController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
  }
}
