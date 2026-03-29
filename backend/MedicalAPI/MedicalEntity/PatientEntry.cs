using System;
using System.Collections.Generic;

namespace MedicalAPI.MedicalEntity;

public partial class PatientEntry
{
    public int PatientId { get; set; }

    public string PatientName { get; set; } = null!;

    public DateTime? EntryDate { get; set; }

    public string? PatientGeneder { get; set; }

    public string? Address { get; set; }

    public string? BloodGroup { get; set; }

    public int? Age { get; set; }

    public decimal? Weight { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<TblMedicineDetail> TblMedicineDetails { get; set; } = new List<TblMedicineDetail>();

    public virtual ICollection<TblSymptomDetail> TblSymptomDetails { get; set; } = new List<TblSymptomDetail>();

    public virtual TblUser User { get; set; } = null!;
}
