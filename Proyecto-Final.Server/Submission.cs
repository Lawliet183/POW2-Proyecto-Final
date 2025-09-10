using System;
using System.Collections.Generic;

namespace Proyecto_Final.Server;

public partial class Submission
{
    public int Id { get; set; }

    public int? RespondentId { get; set; }

    public DateTime? StartedAt { get; set; }

    public DateTime? SubmittedAt { get; set; }

    public int? SurveyId { get; set; }

    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    public virtual Respondent? Respondent { get; set; }

    public virtual Survey? Survey { get; set; }
}
