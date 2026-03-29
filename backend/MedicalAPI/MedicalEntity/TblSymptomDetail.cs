using System;
using System.Collections.Generic;

namespace MedicalAPI.MedicalEntity;

public partial class TblSymptomDetail
{
    public int SymptomId { get; set; }

    public int PatientId { get; set; }

    public string SymptomDetails { get; set; } = null!;

    public int UserId { get; set; }

    public virtual PatientEntry Patient { get; set; } = null!;

    public virtual ICollection<TblMedicineDetail> TblMedicineDetails { get; set; } = new List<TblMedicineDetail>();

    public virtual TblUser User { get; set; } = null!;
}
