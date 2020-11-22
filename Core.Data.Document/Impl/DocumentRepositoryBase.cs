using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Azure.Cosmos;
using Core.Data.Document.Entities;

namespace Core.Data.Document.Impl
{
  public abstract class DocumentRepositoryBase<TEntity> : IDocumentRepositoryBase<TEntity> where TEntity : DocumentEntityBase
  {
    private readonly CosmosClient _cosmosClient;
    private readonly CosmosContainer _cosmosContainer;

    public abstract string DatabaseId { get; }
    public abstract string ContainerId { get; }

    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")]
    protected DocumentRepositoryBase(CosmosClient cosmosClient)
    {
      _cosmosClient = cosmosClient;
      _cosmosContainer = cosmosClient.GetContainer(DatabaseId, ContainerId);
    }

    public async Task AddAsync(TEntity entity, string partitionKey)
    {
      var itemResponse = await _cosmosContainer.CreateItemAsync(entity, new PartitionKey(partitionKey));

      entity.Id = itemResponse.Value.Id;
    }

    public async Task DeleteAsync(string id, string partitionKey)
    {
      await _cosmosContainer.DeleteItemAsync<TEntity>(id, new PartitionKey(partitionKey));
    }

    public async IAsyncEnumerable<TEntity> GetAllAsync()
    {
      await foreach (var item in _cosmosContainer.GetItemQueryIterator<TEntity>(new QueryDefinition("SELECT * FROM c")))
      {
        yield return item;
      }
    }

    public async IAsyncEnumerable<TEntity> GetAllAsync(string partitionKey)
    {
      await foreach (var item in _cosmosContainer.GetItemQueryIterator<TEntity>(new QueryDefinition("SELECT * FROM c"), null,
        new QueryRequestOptions {PartitionKey = new PartitionKey(partitionKey)}))
      {
        yield return item;
      }
    }

    public async Task<TEntity> GetByIdAsync(string id, string partitionKey)
    {
      var itemResponse = await _cosmosContainer.ReadItemAsync<TEntity>(id, new PartitionKey(partitionKey));

      return itemResponse?.Value;
    }

    public async Task UpdateAsync(TEntity entity, string partitionKey)
    {
      await _cosmosContainer.ReplaceItemAsync(entity, entity.Id, new PartitionKey(partitionKey));
    }

    public void Dispose()
    {
      _cosmosClient?.Dispose();
    }
  }
}