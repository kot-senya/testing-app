using System;
using System.Collections.Generic;

namespace EntranseTesting.Models;

public partial class UserSession
{
    public int Id { get; set; }

    public string UserGroup { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public DateTime Date { get; set; }

    public TimeOnly Time { get; set; }

    public virtual ICollection<UserResponse> UserResponses { get; set; } = new List<UserResponse>();
}
