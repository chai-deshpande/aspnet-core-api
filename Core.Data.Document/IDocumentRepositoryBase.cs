using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Data.Document.Entities;

namespace Core.Data.Document
{
  public interface IDocumentRepositoryBase<TEntity> : IDisposable where TEntity : DocumentEntityBase
  {
    public string DatabaseId { get; }

    public string ContainerId { get; }

    Task AddAsync(TEntity entity, string partitionKey);

    Task DeleteAsync(string id, string partitionKey);

    IAsyncEnumerable<TEntity> GetAllAsync();

    IAsyncEnumerable<TEntity> GetAllAsync(string partitionKey);

    Task<TEntity> GetByIdAsync(string id, string partitionKey);

    Task UpdateAsync(TEntity entity, string partitionKey);
  }
}