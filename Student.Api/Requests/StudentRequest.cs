using System;

namespace Student.Api.Requests
{
  public class StudentRequest
  {
    public string FirstName { get; set; }
    
    public string LastName { get; set; }

    public int Age { get; set; }

    public DateTime DateOfBirth { get; set; }
  }
}