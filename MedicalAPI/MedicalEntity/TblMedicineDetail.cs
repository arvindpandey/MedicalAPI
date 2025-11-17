using System;
using System.Collections.Generic;

namespace MedicalAPI.MedicalEntity;

public partial class TblMedicineDetail
{
    public int MedId { get; set; }

    public int? PatientId { get; set; }

    public int? SymptomId { get; set; }

    public string? MedDetails { get; set; }

    public int? UserId { get; set; }
}
