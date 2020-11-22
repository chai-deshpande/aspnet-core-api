using AutoMapper;
using Student.Api.Requests;
using Student.Api.Responses;

namespace Student.Api
{
  public class AutoMappingProfile : Profile
  {
    public AutoMappingProfile()
    {
      CreateMap<StudentRequest, Models.Student>();
      CreateMap<Models.Student, StudentResponse>();
    }
  }
}