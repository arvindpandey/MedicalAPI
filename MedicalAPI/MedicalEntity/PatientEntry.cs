using System;
using System.Collections.Generic;

namespace MedicalAPI.MedicalEntity;

public partial class PatientEntry
{
    public int PatientId { get; set; }

    public string? PatientName { get; set; }

    public string? EntryDate { get; set; }

    public string? PatientGeneder { get; set; }

    public string? Address { get; set; }

    public string? BloodGroup { get; set; }

    public int? Age { get; set; }

    public string? Weight { get; set; }

    public int? UserId { get; set; }
}
