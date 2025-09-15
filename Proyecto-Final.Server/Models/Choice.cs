using System;
using System.Collections.Generic;

namespace Proyecto_Final.Server.Models;

public partial class Choice
{
    public int Id { get; set; }

    public string? Label { get; set; }

    public int? Position { get; set; }

    public int? QuestionId { get; set; }

    public string? Value { get; set; }

    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    public virtual Question? Question { get; set; }

    public virtual ICollection<Answer> AnswersNavigation { get; set; } = new List<Answer>();
}
