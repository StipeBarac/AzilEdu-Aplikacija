using System;
using System.Collections.Generic;
using System.Text;

namespace AzilEdu.Shared.Models;

public class DonorType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Donor> Donors { get; set; } = new List<Donor>(); //jedan DonorType moze pripadati vise donora, zato ide ova lista
}
