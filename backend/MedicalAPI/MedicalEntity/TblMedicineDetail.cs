using System;
using System.Collections.Generic;

namespace MedicalAPI.MedicalEntity;

public partial class TblMedicineDetail
{
    public int MedId { get; set; }

    public int PatientId { get; set; }

    public int SymptomId { get; set; }

    public string MedDetails { get; set; } = null!;

    public int UserId { get; set; }

    public virtual PatientEntry Patient { get; set; } = null!;

    public virtual TblSymptomDetail Symptom { get; set; } = null!;

    public virtual TblUser User { get; set; } = null!;
}
