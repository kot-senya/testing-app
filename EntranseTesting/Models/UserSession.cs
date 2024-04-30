using System;
using System.Collections.Generic;

namespace EntranseTesting.Models;

public partial class UserSession
{
    public int Id { get; set; }

    public string UserGroup { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public DateTime Date { get; set; }

    public TimeSpan Time { get; set; }

    public int CountHint { get; set; }

    public int? IdAppSettings { get; set; }

    public virtual AppSetting? IdAppSettingsNavigation { get; set; }

    public virtual ICollection<UserResponse> UserResponses { get; set; } = new List<UserResponse>();
}
