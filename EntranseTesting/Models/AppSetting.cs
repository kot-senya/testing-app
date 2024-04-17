using System;
using System.Collections.Generic;

namespace EntranseTesting.Models;

public partial class AppSetting
{
    public int Id { get; set; }

    public int CountOfHints { get; set; }

    public TimeSpan Time { get; set; }

    public int CountOfQuestions { get; set; }

    public DateTime DateOfChanging { get; set; }

    public int HalfCost { get; set; }

    public bool HalfVisibility { get; set; }

    public int Raiting5 { get; set; }

    public int Raiting4 { get; set; }

    public int Raiting3 { get; set; }

    public bool HintVisibility { get; set; }

    public bool ResultVisibiliry { get; set; }

    public virtual ICollection<UserSession> UserSessions { get; set; } = new List<UserSession>();
}
