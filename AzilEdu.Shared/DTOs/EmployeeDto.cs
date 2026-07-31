using System;
using System.Collections.Generic;
using System.Text;

namespace AzilEdu.Shared.DTOs;

public class EmployeeDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string EmployeeNumber { get; set; } = string.Empty;
    public DateTime? HireDate { get; set; }
    public string Notes { get; set; } = string.Empty;

    public int EmployeePositionId { get; set; }
    public int EmployeeStatusId { get; set; }
    public string PositionName { get; set; } = string.Empty;
    public string StatusName { get; set; } = string.Empty;
}