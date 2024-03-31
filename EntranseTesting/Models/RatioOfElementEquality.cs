using System;
using System.Collections.Generic;

namespace EntranseTesting.Models;

public partial class RatioOfElementEquality
{
    public int Id { get; set; }

    public int IdElement1 { get; set; }

    public int IdElement2 { get; set; }

    public virtual ElementOfEquality IdElement1Navigation { get; set; } = null!;

    public virtual ElementOfEquality IdElement2Navigation { get; set; } = null!;
}
