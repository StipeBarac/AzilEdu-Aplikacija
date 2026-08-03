using System;
using System.Collections.Generic;
using System.Text;

namespace AzilEdu.Shared.Models;

public class DonationStatus
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public List<Donation> Donations { get; set; } = new();
}
