using System;
using System.Collections.Generic;

namespace EntranseTesting.Models;

public partial class QuestionVariant
{
    public int Id { get; set; }

    public int IdQuestion { get; set; }

    public int Variant { get; set; }

    public virtual Question IdQuestionNavigation { get; set; } = null!;
}
