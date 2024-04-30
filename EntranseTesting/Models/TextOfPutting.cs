using System;
using System.Collections.Generic;

namespace EntranseTesting.Models;

public partial class TextOfPutting
{
    public int Id { get; set; }

    public int IdQuestion { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<ElementOfPutting> ElementOfPuttings { get; set; } = new List<ElementOfPutting>();

    public virtual Question IdQuestionNavigation { get; set; } = null!;

    public virtual ICollection<UserResponseMultiplyAnswer> UserResponseMultiplyAnswers { get; set; } = new List<UserResponseMultiplyAnswer>();
}
