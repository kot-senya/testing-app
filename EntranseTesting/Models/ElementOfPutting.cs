using System;
using System.Collections.Generic;

namespace EntranseTesting.Models;

public partial class ElementOfPutting
{
    public int Id { get; set; }

    public int IdText { get; set; }

    public string Name { get; set; } = null!;

    public bool Correctly { get; set; }

    public virtual TextOfPutting IdTextNavigation { get; set; } = null!;
}
