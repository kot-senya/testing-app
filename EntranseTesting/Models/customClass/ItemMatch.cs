using Avalonia;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntranseTesting.Models
{
    public partial class ItemMatch:ObservableObject
    {        
        [ObservableProperty] int numGroup = 0;
        [ObservableProperty] string? name1 = "";
        [ObservableProperty] string? name2 = "";
        [ObservableProperty] ElementOfGroup? elem1 = null;
        [ObservableProperty] ElementOfGroup? elem2 = null;

        public ItemMatch(){}
        public ItemMatch(string _name1, string _name2)
        {
            Name1 = _name1;
            Name2 = _name2;
        }

        public string Num { get => (NumGroup == 0) ? "" : NumGroup.ToString(); }
    }
}
