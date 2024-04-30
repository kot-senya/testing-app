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
        [ObservableProperty] ElementOfEquality? elem1_e = null;
        [ObservableProperty] ElementOfEquality? elem2_e = null;
        public ItemMatch(){}
        public ItemMatch(string _name1, string _name2)
        {
            Name1 = _name1;
            Name2 = _name2;
        }
        public ItemMatch(ElementOfGroup elem1, ElementOfGroup elem2)
        {
            Elem1 = elem1;
            Elem2 = elem2;
            Name1 = elem1.Name;
            Name2 = elem2.Name;
        }
        public ItemMatch(ElementOfEquality elem1, ElementOfEquality elem2)
        {
            Elem1_e = elem1;
            Elem2_e = elem2;
            Name1 = elem1.Name;
            Name2 = elem2.Name;
        }
        public string Num { get => (NumGroup == 0) ? "" : NumGroup.ToString(); }
    }
}
