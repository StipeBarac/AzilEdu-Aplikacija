using AzilEdu.Api.Data;
using AzilEdu.Shared.DTOs;
using AzilEdu.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AzilEdu.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DonationsController : ControllerBase
{
    private readonly AzilEduDbContext _context;

    public DonationsController(AzilEduDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<DonationDto>>> GetDonations(
        [FromQuery] int? typeId,
        [FromQuery] int? statusId,
        [FromQuery] int? donorId,
        [FromQuery] DateTime? dateFrom,
        [FromQuery] DateTime? dateTo)
    {
        var query = _context.Donations
            .Include(d => d.Donor)
            .Include(d => d.DonationType)
            .Include(d => d.DonationStatus)
            .AsQueryable();

        if (typeId.HasValue)
            query = query.Where(d => d.DonationTypeId == typeId.Value);
        if (statusId.HasValue)
            query = query.Where(d => d.DonationStatusId == statusId.Value);
        if (donorId.HasValue)
            query = query.Where(d => d.DonorId == donorId.Value);
        if (dateFrom.HasValue)
            query = query.Where(d => d.DonationDate >= dateFrom.Value);
        if (dateTo.HasValue)
            query = query.Where(d => d.DonationDate <= dateTo.Value);

        var donations = await query
            .OrderByDescending(d => d.DonationDate)
            .ToListAsync();

        return Ok(donations.Select(ToDto).ToList());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DonationDto>> GetDonationById(int id)
    {
        var donation = await _context.Donations
            .Include(d => d.Donor)
            .Include(d => d.DonationType)
            .Include(d => d.DonationStatus)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (donation is null)
            return NotFound();

        return Ok(ToDto(donation));
    }

    [HttpPost]
    public async Task<ActionResult<DonationDto>> CreateDonation(SaveDonationDto request)
    {
        var validationError = ValidateDonation(request);
        if (validationError is not null)
            return BadRequest(validationError);

        var donation = new Donation
        {
            DonorId = request.DonorId,
            DonationTypeId = request.DonationTypeId,
            DonationStatusId = request.DonationStatusId,
            DonationDate = request.DonationDate,
            Amount = request.Amount,
            ItemName = request.ItemName,
            Quantity = request.Quantity,
            EstimatedValue = request.EstimatedValue,
            Notes = request.Notes
        };

        _context.Donations.Add(donation);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetDonationById), new { id = donation.Id }, null);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDonation(int id, SaveDonationDto request)
    {
        var validationError = ValidateDonation(request);
        if (validationError is not null)
            return BadRequest(validationError);

        var donation = await _context.Donations.FindAsync(id);
        if (donation is null)
            return NotFound();

        donation.DonorId = request.DonorId;
        donation.DonationTypeId = request.DonationTypeId;
        donation.DonationStatusId = request.DonationStatusId;
        donation.DonationDate = request.DonationDate;
        donation.Amount = request.Amount;
        donation.ItemName = request.ItemName;
        donation.Quantity = request.Quantity;
        donation.EstimatedValue = request.EstimatedValue;
        donation.Notes = request.Notes;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDonation(int id)
    {
        var donation = await _context.Donations.FindAsync(id);
        if (donation is null)
            return NotFound();

        _context.Donations.Remove(donation);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private static string? ValidateDonation(SaveDonationDto request)
    {
        if (request.DonorId <= 0)
            return "Donator je obavezan.";

        if (request.DonationTypeId <= 0)
            return "Tip donacije je obavezan.";

        if (request.DonationStatusId <= 0)
            return "Status donacije je obavezan.";

        if (request.DonationDate.Date > DateTime.Today)
            return "Datum donacije ne smije biti u budućnosti.";

        if (request.Quantity.HasValue && request.Quantity.Value < 0)
            return "Količina ne smije biti negativna.";

        if (request.EstimatedValue.HasValue && request.EstimatedValue.Value < 0)
            return "Procijenjena vrijednost ne smije biti negativna.";

        var isMoneyDonation = request.DonationTypeId == 1;

        if (isMoneyDonation)
        {
            if (!request.Amount.HasValue || request.Amount.Value <= 0)
                return "Za novčanu donaciju potrebno je upisati iznos veći od nule.";
        }
        else
        {
            if (string.IsNullOrWhiteSpace(request.ItemName))
                return "Za materijalnu donaciju potrebno je upisati naziv donacije.";

            if (!request.Quantity.HasValue || request.Quantity.Value <= 0)
                return "Za materijalnu donaciju potrebno je upisati količinu veću od nule.";
        }

        return null;
    }

    private static DonationDto ToDto(Donation d)
    {
        return new DonationDto
        {
            Id = d.Id,
            DonorId = d.DonorId,
            DonorName = d.Donor != null
                ? (!string.IsNullOrWhiteSpace(d.Donor.OrganizationName)
                    ? d.Donor.OrganizationName
                    : d.Donor.FirstName + " " + d.Donor.LastName)
                : string.Empty,
            DonationTypeId = d.DonationTypeId,
            TypeName = d.DonationType != null ? d.DonationType.Name : string.Empty,
            DonationStatusId = d.DonationStatusId,
            StatusName = d.DonationStatus != null ? d.DonationStatus.Name : string.Empty,
            DonationDate = d.DonationDate,
            Amount = d.Amount,
            ItemName = d.ItemName,
            Quantity = d.Quantity,
            EstimatedValue = d.EstimatedValue,
            Notes = d.Notes
        };
    }

   
}