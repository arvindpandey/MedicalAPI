using System;
using System.Collections.Generic;

namespace MedicalAPI.MedicalEntity;

public partial class UserRole
{
    public int Urid { get; set; }

    public string RoleName { get; set; } = null!;

    public string? RoleDescription { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<TblUser> TblUsers { get; set; } = new List<TblUser>();

    public virtual ICollection<UserRoleRel> UserRoleRels { get; set; } = new List<UserRoleRel>();
}
