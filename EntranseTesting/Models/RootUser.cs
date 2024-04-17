using System;
using System.Collections.Generic;

namespace EntranseTesting.Models;

public partial class RootUser
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string Login { get; set; } = null!;

    public int? Password { get; set; }
}
