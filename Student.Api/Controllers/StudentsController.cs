using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog.Context;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Student.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class StudentsController : ControllerBase
  {
    private readonly ILogger<StudentsController> _logger;
    public IConfiguration Configuration { get; }

    public StudentsController(IConfiguration configuration, ILogger<StudentsController> logger)
    {
      _logger = logger;
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
    public void Post([FromBody] string value)
    {
    }

    // PUT api/<StudentsController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<StudentsController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
  }
}
