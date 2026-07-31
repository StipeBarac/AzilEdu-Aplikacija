using AzilEdu.Api.Data;
using AzilEdu.Shared.DTOs;
using AzilEdu.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AzilEdu.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DonorsController : ControllerBase
{
    private readonly AzilEduDbContext _context;

    public DonorsController(AzilEduDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<DonorDto>>> GetDonors()
    {
        var donors = await _context.Donors
            .OrderBy(d => d.LastName)
            .Select(d => new DonorDto
            {
                Id = d.Id,
                FirstName = d.FirstName,
                LastName = d.LastName,
                OrganizationName = d.OrganizationName,
                Email = d.Email,
                Phone = d.Phone,
                Address = d.Address,
                City = d.City,
                Notes = d.Notes,
                CreatedAt = d.CreatedAt,
                DonorTypeId = d.DonorTypeId,
                DonorStatusId = d.DonorStatusId,
                StatusName = d.DonorStatus!.Name,
                TypeName = d.DonorType!.Name
            })
            .ToListAsync();

        return Ok(donors);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DonorDto>> GetDonorById(int id)
    {
        var d = await _context.Donors
            .Include(donor => donor.DonorStatus)
            .Include(donor => donor.DonorType)
            .FirstOrDefaultAsync(donor => donor.Id == id);

        if (d is null)
            return NotFound();

        var dto = new DonorDto
        {
            Id = d.Id,
            FirstName = d.FirstName,
            LastName = d.LastName,
            OrganizationName = d.OrganizationName,
            Email = d.Email,
            Phone = d.Phone,
            Address = d.Address,
            City = d.City,
            Notes = d.Notes,
            CreatedAt = d.CreatedAt,
            DonorTypeId = d.DonorTypeId,
            DonorStatusId = d.DonorStatusId,
            StatusName = d.DonorStatus!.Name,
            TypeName = d.DonorType!.Name
        };

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<DonorDto>> CreateDonor(SaveDonorDto dto)
    {
        var donor = new Donor
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            OrganizationName = dto.OrganizationName,
            Email = dto.Email,
            Phone = dto.Phone,
            Address = dto.Address,
            City = dto.City,
            Notes = dto.Notes,
            CreatedAt = DateTime.Now,
            DonorTypeId = dto.DonorTypeId,
            DonorStatusId = dto.DonorStatusId
        };

        _context.Donors.Add(donor);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetDonorById), new { id = donor.Id }, null);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDonor(int id, SaveDonorDto dto)
    {
        var donor = await _context.Donors.FindAsync(id);

        if (donor is null)
            return NotFound();

        donor.FirstName = dto.FirstName;
        donor.LastName = dto.LastName;
        donor.OrganizationName = dto.OrganizationName;
        donor.Email = dto.Email;
        donor.Phone = dto.Phone;
        donor.Address = dto.Address;
        donor.City = dto.City;
        donor.Notes = dto.Notes;
        donor.DonorTypeId = dto.DonorTypeId;
        donor.DonorStatusId = dto.DonorStatusId;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDonor(int id)
    {
        var donor = await _context.Donors.FindAsync(id);

        if (donor is null)
            return NotFound();

        _context.Donors.Remove(donor);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}