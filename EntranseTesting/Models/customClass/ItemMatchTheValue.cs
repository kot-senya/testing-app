using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EntranseTesting.Models.customClass
{
    public partial class ItemMatchTheValue : ObservableObject
    {
        [ObservableProperty] List<string> values = new List<string>() { "--" };
        [ObservableProperty] string elem1 = "--";
        [ObservableProperty] string elem2 = "--";
        public ItemMatchTheValue() { }
        public ItemMatchTheValue(List<string> _list)
        {
            Random.Shared.Shuffle(CollectionsMarshal.AsSpan(_list));
            Values.AddRange(_list);
        }
        
    }
}
