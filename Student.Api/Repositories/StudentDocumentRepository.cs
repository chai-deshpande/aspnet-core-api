using Azure.Cosmos;
using Core.Data.Document.Impl;

namespace Student.Api.Repositories
{
  public class StudentDocumentRepository : DocumentRepositoryBase<Models.Student>, IStudentDocumentRepository
  {
    public StudentDocumentRepository(CosmosClient cosmosClient) : base(cosmosClient)
    {
    }

    public override string DatabaseId => "SampleDB";

    public override string ContainerId => "Students";
  }
}