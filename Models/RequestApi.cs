using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BackEnd.Models;

public class RequestApi
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public ResponseApi Response { get; set; } = new ResponseApi();
    [JsonIgnore]
    public Collection Collection { get; set; }
    [ForeignKey("CollectionId")]
    public int CollectionId { get; set; } // Foreign Key
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    [Required]
    public bool IsDeleted { get; set; } = false;
    [Required]
    public bool IsSynced { get; set; } = false;
    public DateTime? LastSyncedAt { get; set; }
    [Required]
    public Guid SyncId { get; set; } = Guid.NewGuid();
}