using System;
using System.Collections.Generic;

namespace EntranseTesting.Models;

public partial class HintImage
{
    public int Id { get; set; }

    public int IdHint { get; set; }

    public byte[] Image { get; set; } = null!;

    public virtual HintText IdHintNavigation { get; set; } = null!;
}
