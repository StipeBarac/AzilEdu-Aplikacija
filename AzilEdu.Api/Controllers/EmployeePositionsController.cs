using AzilEdu.Api.Data;
using AzilEdu.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AzilEdu.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeePositionsController : ControllerBase
{
    private readonly AzilEduDbContext _context;

    public EmployeePositionsController(AzilEduDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<LookupDto>>> GetPositions()
    {
        var positions = await _context.EmployeePositions
            .OrderBy(pos => pos.Id)
            .Select(pos => new LookupDto
            {
                Id = pos.Id,
                Name = pos.Name
            })
            .ToListAsync();

        return Ok(positions);
    }
}