using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Student.Api.Repositories;
using Student.Api.Requests;
using Student.Api.Responses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Student.Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class StudentsController : ControllerBase
  {
    private readonly ILogger<StudentsController> _logger;
    private readonly IMapper _mapper;
    private readonly IStudentDocumentRepository _studentDocumentRepository;
    public IConfiguration Configuration { get; }

    public StudentsController(IConfiguration configuration, ILogger<StudentsController> logger, IMapper mapper, IStudentDocumentRepository studentDocumentRepository)
    {
      _logger = logger;
      _mapper = mapper;
      _studentDocumentRepository = studentDocumentRepository;
      Configuration = configuration;
    }

    // GET: api/<StudentsController>
    [HttpGet]
    public async Task<IEnumerable<StudentResponse>> Get()
    {
      _logger.LogInformation("api/students list called");
      _logger.LogError("api/students list - sample error message");

      var studentsList = new List<Models.Student>();
      await foreach (var student in _studentDocumentRepository.GetAllAsync())
      {
        studentsList.Add(student);
      }

      return _mapper.Map<IEnumerable<StudentResponse>>(studentsList);
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
      var studentModel = _mapper.Map<Models.Student>(student);

      _studentDocumentRepository.AddAsync(studentModel, student.LastName);
    }

    // PUT api/<StudentsController>/5
    [HttpPut("{id}")]
    public void Put(Guid id, [FromBody] StudentRequest student)
    {
      var studentModel = _mapper.Map<Models.Student>(student);
      studentModel.Id = id.ToString();

      _studentDocumentRepository.UpdateAsync(studentModel, student.LastName);
    }

    // DELETE api/<StudentsController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
  }
}
