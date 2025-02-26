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

    [HttpGet("sqlite")]
    public async Task<ActionResult> GetLocal()
    {
        var collections = await _sqliteDbContext.Collections.ToListAsync();
        return Ok(collections);
    }
    
    [HttpGet("sqlserver")]
    public async Task<ActionResult> GetRemote()
    {
        var collections = await _sqlServerDbContext.Collections.ToListAsync();
        return Ok(collections);
    }
}