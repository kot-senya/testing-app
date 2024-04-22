using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using EntranseTesting.Models;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;

namespace EntranseTesting.ViewModels
{
    public partial class TestResultViewModel : ObservableObject
    {
        [ObservableProperty] UserSession session = new UserSession();
        public TestResultViewModel() { }
        public TestResultViewModel(int? _n)
        {
            EntranceTestingContext connection = new EntranceTestingContext();
            session = connection.UserSessions.Include(tb => tb.UserResponses).FirstOrDefault(tb => tb.Id == Response.userSession.Id);
        }

        public double ProcentCorrectly
        {
            get => Math.Round((double)(session.UserResponses.Where(tb => tb.Correctly == true).Count()/ session.UserResponses.Count()),2);
        }
        public string CountCorrectly
        {
            get => session.UserResponses.Where(tb => tb.Correctly == true).Count() + " / "+ session.UserResponses.Count();
        }
    }
}