using System;
using System.Collections.Generic;

namespace EntranseTesting.Models;

public partial class UserResponseArrangement
{
    public int Id { get; set; }

    public int IdResponse { get; set; }

    public int IdElement { get; set; }

    public int Position { get; set; }

    public virtual ElementOfArrangement IdElementNavigation { get; set; } = null!;

    public virtual UserResponse IdResponseNavigation { get; set; } = null!;
}
