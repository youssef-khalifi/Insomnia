using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Text.Json;

namespace BackEnd.Models;

public class ResponseApi
{
    public int Id { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public long ResponseTime { get; set; }
    public string Body { get; set; } = string.Empty;
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