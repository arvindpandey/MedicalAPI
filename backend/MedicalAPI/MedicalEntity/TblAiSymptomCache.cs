using System;
using System.Collections.Generic;

namespace MedicalAPI.MedicalEntity;

public partial class TblAiSymptomCache
{
    public int CacheId { get; set; }

    public string SymptomKey { get; set; } = null!;

    public string Aiadvice { get; set; } = null!;

    public string? Severity { get; set; }

    public string? SuggestedAction { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? HitCount { get; set; }
}
