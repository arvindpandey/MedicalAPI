using System;
using System.Collections.Generic;

namespace MedicalAPI.MedicalEntity;

public partial class TblSymptomDetail
{
    public int SymptomId { get; set; }

    public int? PatientId { get; set; }

    public string? SymptomDetails { get; set; }

    public int? UserId { get; set; }
}
