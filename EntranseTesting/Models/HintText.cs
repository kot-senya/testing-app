using System;
using System.Collections.Generic;

namespace EntranseTesting.Models;

public partial class HintText
{
    public int Id { get; set; }

    public string? Text { get; set; }

    public virtual ICollection<HintImage> HintImages { get; set; } = new List<HintImage>();

    public virtual ICollection<QuestionHint> QuestionHints { get; set; } = new List<QuestionHint>();
}
