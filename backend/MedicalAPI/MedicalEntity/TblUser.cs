using System;
using System.Collections.Generic;

namespace MedicalAPI.MedicalEntity;

public partial class TblUser
{
    public int UserId { get; set; }

    public int UserRoleId { get; set; }

    public string LoginName { get; set; } = null!;

    public string UserPassword { get; set; } = null!;

    public string UserFirstName { get; set; } = null!;

    public string? UserMiddleName { get; set; }

    public string UserLastName { get; set; } = null!;

    public string UserEmailId { get; set; } = null!;

    public string? UserGender { get; set; }

    public string? UserMobileNo { get; set; }

    public int? UserAge { get; set; }

    public DateTime? UserPasswordExpiryDate { get; set; }

    public string? UserAadharCard { get; set; }

    public bool? UserIsActive { get; set; }

    public DateTime? UserCreateDate { get; set; }

    public DateTime? UserModifiedDate { get; set; }

    public virtual ICollection<PatientEntry> PatientEntries { get; set; } = new List<PatientEntry>();

    public virtual ICollection<TblMedicineDetail> TblMedicineDetails { get; set; } = new List<TblMedicineDetail>();

    public virtual ICollection<TblSymptomDetail> TblSymptomDetails { get; set; } = new List<TblSymptomDetail>();

    public virtual UserRole UserRole { get; set; } = null!;

    public virtual ICollection<UserRoleRel> UserRoleRels { get; set; } = new List<UserRoleRel>();
}
