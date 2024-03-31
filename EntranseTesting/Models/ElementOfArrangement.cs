using System;
using System.Collections.Generic;

namespace EntranseTesting.Models;

public partial class ElementOfArrangement
{
    public int Id { get; set; }

    public int IdQuestion { get; set; }

    public string Name { get; set; } = null!;

    public int Position { get; set; }

    public virtual Question IdQuestionNavigation { get; set; } = null!;

    public virtual ICollection<UserResponseArrangement> UserResponseArrangements { get; set; } = new List<UserResponseArrangement>();
}
