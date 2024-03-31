using System;
using System.Collections.Generic;

namespace EntranseTesting.Models;

public partial class ElementOfEquality
{
    public int Id { get; set; }

    public int IdQuestion { get; set; }

    public string Name { get; set; } = null!;

    public virtual Question IdQuestionNavigation { get; set; } = null!;

    public virtual ICollection<RatioOfElementEquality> RatioOfElementEqualityIdElement1Navigations { get; set; } = new List<RatioOfElementEquality>();

    public virtual ICollection<RatioOfElementEquality> RatioOfElementEqualityIdElement2Navigations { get; set; } = new List<RatioOfElementEquality>();

    public virtual ICollection<UserResponseMatchTheValue> UserResponseMatchTheValueIdElement1Navigations { get; set; } = new List<UserResponseMatchTheValue>();

    public virtual ICollection<UserResponseMatchTheValue> UserResponseMatchTheValueIdElement2Navigations { get; set; } = new List<UserResponseMatchTheValue>();
}
