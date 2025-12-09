using System;
using System.Collections.Generic;

namespace MedicalAPI.MedicalEntity;

public partial class UserRole
{
    public int Urid { get; set; }

    public string? RoleName { get; set; }

    public string? RoleDescription { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }
}
