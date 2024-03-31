using System;
using System.Collections.Generic;

namespace EntranseTesting.Models;

public partial class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? IdOrientation { get; set; }

    public virtual ElementOrientation? IdOrientationNavigation { get; set; }

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
