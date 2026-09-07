using AzilEdu.Api.Data;
using AzilEdu.Shared.DTOs;
using AzilEdu.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AzilEdu.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Microsoft.AspNetCore.Authorization.Authorize(
    Policy = AzilEdu.Api.Security.AuthorizationPolicies.AdminOnly)]
public class EmployeesController : ControllerBase
{
    private readonly AzilEduDbContext _context;

    public EmployeesController(AzilEduDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<EmployeeDto>>> GetEmployees()
    {
        var employees = await _context.Employees
            .OrderBy(e => e.LastName)
            .Select(e => new EmployeeDto
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                Phone = e.Phone,
                EmployeeNumber = e.EmployeeNumber,
                HireDate = e.HireDate,
                Notes = e.Notes,
                EmployeePositionId = e.EmployeePositionId,
                EmployeeStatusId = e.EmployeeStatusId,
                PositionName = e.EmployeePosition!.Name,
                StatusName = e.EmployeeStatus!.Name
            })
            .ToListAsync();

        return Ok(employees);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeDto>> GetEmployeeById(int id)
    {
        var e = await _context.Employees
            .Include(emp => emp.EmployeePosition)
            .Include(emp => emp.EmployeeStatus)
            .FirstOrDefaultAsync(emp => emp.Id == id);

        if (e is null)
            return NotFound();

        var dto = new EmployeeDto
        {
            Id = e.Id,
            FirstName = e.FirstName,
            LastName = e.LastName,
            Email = e.Email,
            Phone = e.Phone,
            EmployeeNumber = e.EmployeeNumber,
            HireDate = e.HireDate,
            Notes = e.Notes,
            EmployeePositionId = e.EmployeePositionId,
            EmployeeStatusId = e.EmployeeStatusId,
            PositionName = e.EmployeePosition!.Name,
            StatusName = e.EmployeeStatus!.Name
        };

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<EmployeeDto>> CreateEmployee(SaveEmployeeDto dto)
    {
        var employee = new Employee
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            Phone = dto.Phone,
            EmployeeNumber = dto.EmployeeNumber,
            HireDate = dto.HireDate,
            Notes = dto.Notes,
            EmployeePositionId = dto.EmployeePositionId,
            EmployeeStatusId = dto.EmployeeStatusId
        };

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.Id }, null);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployee(int id, SaveEmployeeDto dto)
    {
        var employee = await _context.Employees.FindAsync(id);

        if (employee is null)
            return NotFound();

        employee.FirstName = dto.FirstName;
        employee.LastName = dto.LastName;
        employee.Email = dto.Email;
        employee.Phone = dto.Phone;
        employee.EmployeeNumber = dto.EmployeeNumber;
        employee.HireDate = dto.HireDate;
        employee.Notes = dto.Notes;
        employee.EmployeePositionId = dto.EmployeePositionId;
        employee.EmployeeStatusId = dto.EmployeeStatusId;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var employee = await _context.Employees.FindAsync(id);

        if (employee is null)
            return NotFound();

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("lookup")]
    public async Task<ActionResult<List<LookupDto>>> GetLookup()
    {
        var employees = await _context.Employees
            .OrderBy(e => e.LastName)
            .Select(e => new LookupDto
            {
                Id = e.Id,
                Name = e.FirstName + " " + e.LastName
            })
            .ToListAsync();

        return Ok(employees);
    }
}