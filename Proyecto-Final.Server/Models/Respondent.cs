using System;
using System.Collections.Generic;

namespace Proyecto_Final.Server;

public partial class Respondent
{
    public int Id { get; set; }

    public string? ExternalId { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Auth? Auth { get; set; }

    public virtual ICollection<Submission> Submissions { get; set; } = new List<Submission>();
}
