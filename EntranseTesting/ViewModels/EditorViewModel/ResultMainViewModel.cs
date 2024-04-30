using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using EntranseTesting.Models;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;

namespace EntranseTesting.ViewModels
{
    public partial class ResultMainViewModel : ObservableObject
    {
        ObservableCollection<UserSession> answers = new ObservableCollection<UserSession>();
        string group = "";
        string fio = "";
        [ObservableProperty]DateTime endDate = DateTime.Today;
        [ObservableProperty]DateTime startDate = DateTime.Today;
        DateTime selectedEndDate = DateTime.Today;
        DateTime selectedStartDate = DateTime.Today;


        public ResultMainViewModel()
        {
            EntranceTestingContext connection = new EntranceTestingContext();            
            StartDate = connection.UserSessions.OrderBy(tb => tb.Date).ToList().First().Date;
            SelectedStartDate = StartDate;
            filter();
        }

        public ObservableCollection<UserSession> Answers { get => answers; set => answers = value; }
        public string Group { get => group; set { group = value; filter(); } }
        public string Fio { get => fio; set { fio = value; filter(); } }
        public DateTime SelectedEndDate { get => selectedEndDate; set { selectedEndDate = value; filter(); } }
        public DateTime SelectedStartDate { get => selectedStartDate; set { selectedStartDate = value; filter(); } }
        private void filter()
        {
            EntranceTestingContext connection = new EntranceTestingContext();
            List<UserSession> list = connection.UserSessions
                .Include(tb=>tb.IdAppSettingsNavigation)
                .Include(tb => tb.UserResponses)
                .OrderByDescending(tb => tb.Date).ToList();
            if(!string.IsNullOrWhiteSpace(Group))
                list = list.Where(tb => tb.UserGroup.ToLower().Contains(Group.ToLower())).ToList();
            if (!string.IsNullOrWhiteSpace(Fio))
                list = list.Where(tb => tb.UserName.ToLower().Contains(Fio.ToLower())).ToList();
            list = list.Where(tb => tb.Date >= SelectedStartDate && tb.Date.Date <= SelectedEndDate).ToList();

            Answers.Clear();
            foreach (var item in list)
                Answers.Add(item);
        }
       
    }
}