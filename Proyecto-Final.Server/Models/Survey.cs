using System;
using System.Collections.Generic;

namespace Proyecto_Final.Server;

public partial class Survey
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public bool? IsAnonymous { get; set; }

    public string? Status { get; set; }

    public DateTime? OpensAt { get; set; }

    public DateTime? ClosesAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

    public virtual ICollection<Submission> Submissions { get; set; } = new List<Submission>();
}
