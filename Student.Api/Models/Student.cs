using System;
using System.Text.Json.Serialization;
using Core.Data.Document.Entities;

namespace Student.Api.Models
{
  [Serializable]
  public class Student : DocumentEntityBase
  {
    [JsonPropertyName("firstName")]
    public string FirstName { get; set; }
    
    [JsonPropertyName("lastName")]
    public string LastName { get; set; }

    [JsonPropertyName("age")]
    public int Age { get; set; }

    [JsonPropertyName("dateOfBirth")]
    public DateTime DateOfBirth { get; set; }

    [JsonPropertyName("city")]
    public string City { get; set; }
  }
}