using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EntranseTesting.Models;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;

namespace EntranseTesting.ViewModels
{
    public partial class TestSettingsViewModel : ObservableObject
    {       
        [ObservableProperty]AppSetting? settings;
        [ObservableProperty]int countQuestion = 0;
        public TestSettingsViewModel()
        {
            EntranceTestingContext baseConnection = new EntranceTestingContext();
            if (baseConnection.AppSettings.ToList().Count > 0)
                Settings = baseConnection.AppSettings.ToList().Last();
            else
            {
                Settings = new AppSetting() 
                {                     
                    Time = new TimeSpan(0, 45, 0),
                    CountOfQuestions = 30,
                    HintVisibility = false,
                    CountOfHints = 0, 
                    Raiting5 = 30,
                    Raiting4 = 23,
                    Raiting3 = 16
                };
            }

            CountQuestion = baseConnection.Questions.Where(tb => tb.InTest == true).Count();
        }
    }
}