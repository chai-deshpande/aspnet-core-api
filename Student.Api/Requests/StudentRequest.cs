using System;

namespace Student.Api.Requests
{
  /// <summary>
  /// Student request
  /// </summary>
  public class StudentRequest
  {
    /// <summary>
    /// First name of the Student
    /// </summary>
    public string FirstName { get; set; }
    
    /// <summary>
    /// Last name of the student
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Age of the student
    /// </summary>
    public int Age { get; set; }

    /// <summary>
    /// Date of birth
    /// </summary>
    public DateTime DateOfBirth { get; set; }
  }
}