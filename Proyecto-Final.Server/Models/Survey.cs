using System;
using System.Collections.Generic;

namespace Proyecto_Final.Server.Models;

public partial class Survey
{
    public DateTime? ClosesAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? Description { get; set; }

    public int Id { get; set; }

    public sbyte? IsAnonymous { get; set; }

    public DateTime? OpensAt { get; set; }

    public string? Status { get; set; }

    public string? Title { get; set; }

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

    public virtual ICollection<Submission> Submissions { get; set; } = new List<Submission>();
}
