using System;
using System.Collections.Generic;

namespace EntranseTesting.Models;

public partial class UserResponseMatchTheElement
{
    public int Id { get; set; }

    public int IdResponse { get; set; }

    public int IdElement1 { get; set; }

    public int IdElement2 { get; set; }

    public virtual ElementOfGroup IdElement1Navigation { get; set; } = null!;

    public virtual ElementOfGroup IdElement2Navigation { get; set; } = null!;

    public virtual UserResponse IdResponseNavigation { get; set; } = null!;
}
