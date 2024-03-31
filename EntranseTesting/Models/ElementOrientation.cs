using System;
using System.Collections.Generic;

namespace EntranseTesting.Models;

public partial class ElementOrientation
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
}
