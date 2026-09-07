using AzilEdu.Api.Data;
using AzilEdu.Shared.DTOs;
using AzilEdu.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AzilEdu.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Microsoft.AspNetCore.Authorization.Authorize(
    Policy = AzilEdu.Api.Security.AuthorizationPolicies.Staff)]
public class VolunteersController : ControllerBase
{
    private readonly AzilEduDbContext _context;

    public VolunteersController(AzilEduDbContext context)
    {
        _context = context;
    }

    // GET: sve volontere
    [HttpGet]
    public async Task<ActionResult<List<VolunteerDto>>> GetVolunteers()
    {
        var volunteers = await _context.Volunteers
            .OrderBy(v => v.LastName)
            .Select(v => new VolunteerDto
            {
                Id = v.Id,
                FirstName = v.FirstName,
                LastName = v.LastName,
                Email = v.Email,
                Phone = v.Phone,
                Skills = v.Skills,
                AvailableFrom = v.AvailableFrom,
                Notes = v.Notes,
                VolunteerStatusId = v.VolunteerStatusId,
                Status = v.VolunteerStatus!.Name
            })
            .ToListAsync();

        return Ok(volunteers);
    }

    // GET: jedan volonter po Id
    [HttpGet("{id}")]
    public async Task<ActionResult<VolunteerDto>> GetVolunteerById(int id)
    {
        var v = await _context.Volunteers
            .Include(vol => vol.VolunteerStatus)
            .FirstOrDefaultAsync(vol => vol.Id == id);

        if (v is null)
            return NotFound();

        var dto = new VolunteerDto
        {
            Id = v.Id,
            FirstName = v.FirstName,
            LastName = v.LastName,
            Email = v.Email,
            Phone = v.Phone,
            Skills = v.Skills,
            AvailableFrom = v.AvailableFrom,
            Notes = v.Notes,
            VolunteerStatusId = v.VolunteerStatusId,
            Status = v.VolunteerStatus!.Name
        };

        return Ok(dto);
    }

    // POST: novi volonter
    [HttpPost]
    public async Task<ActionResult<VolunteerDto>> CreateVolunteer(SaveVolunteerDto dto)
    {
        var volunteer = new Volunteer
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Phone = dto.Phone,
            Skills = dto.Skills,
            AvailableFrom = dto.AvailableFrom,
            Notes = dto.Notes,
            VolunteerStatusId = dto.VolunteerStatusId
        };

        _context.Volunteers.Add(volunteer);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetVolunteerById), new { id = volunteer.Id }, null);
    }

    // PUT: izmjena postojećeg
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateVolunteer(int id, SaveVolunteerDto dto)
    {
        var volunteer = await _context.Volunteers.FindAsync(id);

        if (volunteer is null)
            return NotFound();

        volunteer.FirstName = dto.FirstName;
        volunteer.LastName = dto.LastName;
        volunteer.Email = dto.Email;
        volunteer.Phone = dto.Phone;
        volunteer.Skills = dto.Skills;
        volunteer.AvailableFrom = dto.AvailableFrom;
        volunteer.Notes = dto.Notes;
        volunteer.VolunteerStatusId = dto.VolunteerStatusId;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: brisanje
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVolunteer(int id)
    {
        var volunteer = await _context.Volunteers.FindAsync(id);

        if (volunteer is null)
            return NotFound();

        _context.Volunteers.Remove(volunteer);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("lookup")]
    public async Task<ActionResult<List<LookupDto>>> GetLookup()
    {
        var volunteers = await _context.Volunteers
            .OrderBy(v => v.LastName)
            .Select(v => new LookupDto
            {
                Id = v.Id,
                Name = v.FirstName + " " + v.LastName
            })
            .ToListAsync();

        return Ok(volunteers);
    }
}