using System;
using System.Collections.Generic;

namespace EntranseTesting.Models;

public partial class UserResponseMultiplyAnswer
{
    public int Id { get; set; }

    public int IdResponse { get; set; }

    public int IdText { get; set; }

    public int IdElement { get; set; }

    public virtual ElementOfPutting IdElementNavigation { get; set; } = null!;

    public virtual UserResponse IdResponseNavigation { get; set; } = null!;

    public virtual TextOfPutting IdTextNavigation { get; set; } = null!;
}
