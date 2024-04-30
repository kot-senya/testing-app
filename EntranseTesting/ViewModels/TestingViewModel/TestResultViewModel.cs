using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using EntranseTesting.Models;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;
using ReactiveUI;

namespace EntranseTesting.ViewModels
{
    public partial class TestResultViewModel : ObservableObject
    {
        [ObservableProperty] UserSession session = new UserSession();
        public TestResultViewModel() { }
        public TestResultViewModel(int? _n)
        {
            try
            {
                EntranceTestingContext connection = new EntranceTestingContext();
                Session = connection.UserSessions.Include(tb => tb.UserResponses).Include(tb => tb.IdAppSettingsNavigation).FirstOrDefault(tb => tb.Id == Response.userSession.Id);
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine("\n\nTestResult\n" + ex.Message);
#endif
            }

        }
    }
}