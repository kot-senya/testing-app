using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntranseTesting.Models
{
    public partial class UserSession
    {
        [NotMapped]
        public double ProcentCorrectly
        {
            get => Math.Round((double)UserResponses.Where(tb => tb.Correctly == true).Count() / (double)UserResponses.Count() * 100, 2);
        }
        [NotMapped]
        public string CountCorrectly
        {
            get => UserResponses.Where(tb => tb.Correctly == true).Count() + " / " + UserResponses.Count();
        }
        [NotMapped]
        public string Raiting
        {
            get
            {
                if (IdAppSettingsNavigation != null)
                {
                    int correctlyCount = UserResponses.Where(tb => tb.Correctly == true).Count();
                    if (correctlyCount >= IdAppSettingsNavigation.Raiting5)
                    {
                        return "5";
                    }
                    else if (correctlyCount >= IdAppSettingsNavigation.Raiting4)
                    {
                        return "4";
                    }
                    else if (correctlyCount >= IdAppSettingsNavigation.Raiting3)
                    {
                        return "3";
                    }
                    else
                    {
                        return "Незачет";
                    }
                }
                else
                    return "Не определено";
            }
        }

        public string CountHintLine
        {
            get
            {
                if (IdAppSettingsNavigation.CountOfHints == 0)
                    return "Подсказки отключены";
                return "Потрачено " + CountHint + " очков";
            }
        }
    }
}
