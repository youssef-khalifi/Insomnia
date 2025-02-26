using System.ComponentModel.DataAnnotations;

namespace BackEnd.Models;

public class Collection
{
    
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<RequestApi> Requests { get; set; } = new List<RequestApi>();
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