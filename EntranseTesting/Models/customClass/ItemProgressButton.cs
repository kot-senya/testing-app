using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntranseTesting.Models
{
    public partial class ItemProgressButton: ObservableObject
    {
        [ObservableProperty] private int num = 0;
        [ObservableProperty] private bool check = false;
        [ObservableProperty] private bool active = false;
        public ItemProgressButton(int n)
        {
            Num = n;
        }

    }
}
