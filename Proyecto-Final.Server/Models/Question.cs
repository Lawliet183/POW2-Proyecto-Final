using System;
using System.Collections.Generic;

namespace Proyecto_Final.Server;

public partial class Question
{
    public int Id { get; set; }

    public int? SurveyId { get; set; }

    public int? Position { get; set; }

    public string? Text { get; set; }

    public string? Type { get; set; }

    public bool? IsRequired { get; set; }

    public float? MinValue { get; set; }

    public float? MaxValue { get; set; }

    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    public virtual ICollection<Choice> Choices { get; set; } = new List<Choice>();

    public virtual Survey? Survey { get; set; }
}
