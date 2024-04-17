using System;
using System.Collections.Generic;

namespace EntranseTesting.Models;

public partial class UserResponseChooseAnswer
{
    public int Id { get; set; }

    public int IdResponse { get; set; }

    public int IdElement { get; set; }

    public bool? Usercorrectly { get; set; }

    public virtual ElementOfChoose IdElementNavigation { get; set; } = null!;

    public virtual UserResponse IdResponseNavigation { get; set; } = null!;
}
