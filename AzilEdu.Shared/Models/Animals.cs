using System;
using System.Collections.Generic;
using System.Text;

namespace AzilEdu.Shared.Models;

public class Animal
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Species { get; set; } = string.Empty;
    public string Breed { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public int? Age { get; set; }   //Ovaj upitnik znaci da vrijednost kod koje se nalazi smije biti null, odnosno da je opcionalna
    public DateTime? ArrivalDate { get; set; }
    public bool IsAdopted { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

}