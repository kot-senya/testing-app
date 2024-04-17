using System;
using System.Collections.Generic;

namespace EntranseTesting.Models;

public partial class UserResponse
{
    public int Id { get; set; }

    public int IdSession { get; set; }

    public int IdQuestion { get; set; }

    public bool Correctly { get; set; }

    public bool HintApply { get; set; }

    public virtual Question IdQuestionNavigation { get; set; } = null!;

    public virtual UserSession IdSessionNavigation { get; set; } = null!;

    public virtual ICollection<UserResponseArrangement> UserResponseArrangements { get; set; } = new List<UserResponseArrangement>();

    public virtual ICollection<UserResponseChooseAnswer> UserResponseChooseAnswers { get; set; } = new List<UserResponseChooseAnswer>();

    public virtual ICollection<UserResponseMatchTheElement> UserResponseMatchTheElements { get; set; } = new List<UserResponseMatchTheElement>();

    public virtual ICollection<UserResponseMatchTheValue> UserResponseMatchTheValues { get; set; } = new List<UserResponseMatchTheValue>();
}
