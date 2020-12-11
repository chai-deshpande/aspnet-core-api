using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Student.Api.Repositories;
using Student.Api.Requests;
using Student.Api.Responses;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Student.Api.Controllers
{
  [Consumes("application/json")]
  [Produces("application/json", "application/xml")]
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

    /// <summary>
    /// Gets all the Students in the school
    /// </summary>
    /// <returns></returns>
    [HttpGet(Name = "GetStudents")]
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

    [HttpGet("{id}", Name = "GetStudent")]
    public async Task<ActionResult<StudentResponse>> Get(Guid id, [FromQuery]string lastName)
    {
      var student = await _studentDocumentRepository.GetByIdAsync(id.ToString(), lastName);

      if (student == null)
      {
        return NotFound();
      }

      return Ok(_mapper.Map<StudentResponse>(student));
    }

    [HttpPost(Name = "CreateStudent")]
    public async Task<ActionResult<StudentResponse>> Post([FromBody] StudentRequest student)
    {
      var studentModel = _mapper.Map<Models.Student>(student);

      await _studentDocumentRepository.AddAsync(studentModel, student.LastName);

      await PostAuditMessage("create", studentModel);

      return CreatedAtRoute("GetStudent", new {studentModel.Id, studentModel.LastName}, _mapper.Map<StudentResponse>(studentModel));
    }

    private async Task PostAuditMessage(string action, Models.Student studentModel)
    {
      var queueClient = new QueueClient(Configuration["connectionStrings:service-bus"], "student-audit");

      try
      {
        var messageBody = JsonConvert.SerializeObject(studentModel);
        var message = new Message(Encoding.UTF8.GetBytes(messageBody))
        {
          MessageId = action == "create" ? $"{studentModel.Id}" : $"{studentModel.Id}-{DateTime.UtcNow.Ticks}",
          Label = action
        };

        // Send the message
        await queueClient.SendAsync(message);
      }
      finally
      {
        // Close the client
        await queueClient.CloseAsync();
      }
    }

    [HttpPut("{id}", Name = "UpdateStudent")]
    public async Task<ActionResult<StudentResponse>> Put(Guid id, [FromBody] StudentRequest student)
    {
      var studentModel = _mapper.Map<Models.Student>(student);
      studentModel.Id = id.ToString();

      await _studentDocumentRepository.UpdateAsync(studentModel, student.LastName);

      await PostAuditMessage("update", studentModel);

      return Ok(_mapper.Map<StudentResponse>(studentModel));
    }

    // DELETE api/<StudentsController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
  }
}
