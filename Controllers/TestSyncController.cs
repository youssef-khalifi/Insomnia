using BackEnd.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestSyncController : ControllerBase
{
    private readonly SqliteDbContext _sqliteDbContext;
    private readonly SqlServerDbContext _sqlServerDbContext;

    public TestSyncController(SqliteDbContext sqliteDbContext, SqlServerDbContext sqlServerDbContext)
    {
        _sqliteDbContext = sqliteDbContext;
        _sqlServerDbContext = sqlServerDbContext;
    }

    [HttpGet("localCollection")]
    public async Task<ActionResult> GetLocal()
    {
        var collections = await _sqliteDbContext.Collections.Include(c => c.Requests).ToListAsync();
        return Ok(collections);
    }
    
    [HttpGet("localRequest")]
    public async Task<ActionResult> GetLocalRequest()
    {
        var collections = await _sqliteDbContext.Requests.ToListAsync();
        return Ok(collections);
    }
    
    [HttpGet("RemoteCollection")]
    public async Task<ActionResult> GetRemote()
    {
        var collections = await _sqlServerDbContext.Collections.Include(c => c.Requests).ToListAsync();
        return Ok(collections);
    }
    
    [HttpGet("RemoteRequest")]
    public async Task<ActionResult> GetRemoteRequest()
    {
        var collections = await _sqlServerDbContext.Requests.ToListAsync();
        return Ok(collections);
    }
}