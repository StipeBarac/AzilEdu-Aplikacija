using System;
using System.Collections.Generic;
using System.Text;

namespace AzilEdu.Shared.DTOs;

public class SaveDonationDto
{
    public int DonorId { get; set; }
    public int DonationTypeId { get; set; }
    public int DonationStatusId { get; set; }

    public DateTime DonationDate { get; set; }
    public decimal? Amount { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public decimal? Quantity { get; set; }
    public decimal? EstimatedValue { get; set; }
    public string Notes { get; set; } = string.Empty;
}