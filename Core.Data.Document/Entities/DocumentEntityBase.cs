using System;
using System.Text.Json.Serialization;

namespace Core.Data.Document.Entities
{
  public abstract class DocumentEntityBase
  {
    /// <summary>
    /// Default document entity identifier
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }

    /// <summary>
    /// Data time to live
    /// </summary>
    [JsonPropertyNameAttribute("ttl")]
    public int Ttl { get; set; }

    /// <summary>
    /// Entity
    /// </summary>
    /// <param name="generateId">Generate id</param>
    protected DocumentEntityBase(bool generateId = true)
    {
      // ReSharper disable once VirtualMemberCallInConstructor
      SetDefaultTimeToLive();

      if (generateId)
      {
        this.Id = Guid.NewGuid().ToString();
      }
    }

    /// <summary>
    /// Set a default data time to live
    /// </summary>
    public virtual void SetDefaultTimeToLive()
    {
      Ttl = -1;
    }

  }
}
