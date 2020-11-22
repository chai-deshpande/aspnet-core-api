using System;
using Student.Api.Requests;

namespace Student.Api.Responses
{
  public class StudentResponse : StudentRequest
  {
    public Guid Id { get; set; }
  }
}