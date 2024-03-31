using System;
using System.Collections.Generic;

namespace EntranseTesting.Models;

public partial class QuestionImage
{
    public int Id { get; set; }

    public int IdQuestion { get; set; }

    public byte[] Image { get; set; } = null!;

    public virtual Question IdQuestionNavigation { get; set; } = null!;
}
