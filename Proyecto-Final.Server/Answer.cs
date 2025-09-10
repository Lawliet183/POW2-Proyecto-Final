using System;
using System.Collections.Generic;

namespace Proyecto_Final.Server;

public partial class Answer
{
    public DateTime? AnswerDate { get; set; }

    public float? AnswerNumber { get; set; }

    public string? AnswerText { get; set; }

    public int Id { get; set; }

    public int? QuestionId { get; set; }

    public int? SelectedChoiceId { get; set; }

    public int? SubmissionId { get; set; }

    public virtual Question? Question { get; set; }

    public virtual Choice? SelectedChoice { get; set; }

    public virtual Submission? Submission { get; set; }

    public virtual ICollection<Choice> Choices { get; set; } = new List<Choice>();
}
