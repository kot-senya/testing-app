using System;
using System.Collections.Generic;

namespace EntranseTesting.Models;

public partial class ElementOfGroup
{
    public int Id { get; set; }

    public int IdGroup { get; set; }

    public string Name { get; set; } = null!;

    public int RatioNumeri { get; set; }

    public virtual Group IdGroupNavigation { get; set; } = null!;

    public virtual ICollection<UserResponseMatchTheElement> UserResponseMatchTheElementIdElement1Navigations { get; set; } = new List<UserResponseMatchTheElement>();

    public virtual ICollection<UserResponseMatchTheElement> UserResponseMatchTheElementIdElement2Navigations { get; set; } = new List<UserResponseMatchTheElement>();
}
