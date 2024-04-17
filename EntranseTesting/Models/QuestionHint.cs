using System;
using System.Collections.Generic;

namespace EntranseTesting.Models;

public partial class QuestionHint
{
    public int Id { get; set; }

    public int IdQuestion { get; set; }

    public int IdHint { get; set; }

    public int Cost { get; set; }

    public virtual HintText IdHintNavigation { get; set; } = null!;

    public virtual Question IdQuestionNavigation { get; set; } = null!;
}
