using System;
using System.Collections.Generic;

namespace EntranseTesting.Models;

public partial class Group
{
    public int Id { get; set; }

    public int IdQuestion { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<ElementOfGroup> ElementOfGroups { get; set; } = new List<ElementOfGroup>();

    public virtual Question IdQuestionNavigation { get; set; } = null!;
}
