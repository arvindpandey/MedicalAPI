using System;
using System.Collections.Generic;

namespace MedicalAPI.MedicalEntity;

public partial class UserRoleRel
{
    public int Urrid { get; set; }

    public int Userid { get; set; }

    public int Urid { get; set; }

    public virtual UserRole Ur { get; set; } = null!;

    public virtual TblUser User { get; set; } = null!;
}
