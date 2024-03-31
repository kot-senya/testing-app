using System;
using System.Collections.Generic;

namespace EntranseTesting.Models;

public partial class ElementOfChoose
{
    public int Id { get; set; }

    public int IdQuestion { get; set; }

    public string Name { get; set; } = null!;

    public bool Correctly { get; set; }

    public virtual Question IdQuestionNavigation { get; set; } = null!;

    public virtual ICollection<UserResponseChooseAnswer> UserResponseChooseAnswers { get; set; } = new List<UserResponseChooseAnswer>();
}
