using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using EntranseTesting.Models;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;

namespace EntranseTesting.ViewModels
{
    public partial class UserResultViewModel : ObservableObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        [ObservableProperty] UserSession session = new UserSession();
        [ObservableProperty] List<UserResponse> responseList = new List<UserResponse>();
        bool? hintVisible = null;
        bool? correctlyVisible = null;
        bool? responseVisible = null;
        string searchLine = "";
        public UserResultViewModel(int idSession)
        {
            EntranceTestingContext connection = new EntranceTestingContext();
            Session = connection.UserSessions
                .Include(tb => tb.IdAppSettingsNavigation)
                .Include(tb => tb.UserResponses)
                .FirstOrDefault(tb => tb.Id == idSession);

            filter();
        }

        public bool? HintVisible { get => hintVisible; set { hintVisible = value; filter(); OnPropertyChanged("HintVisible"); OnPropertyChanged("ResponseList"); } }
        public bool? CorrectlyVisible { get => correctlyVisible; set { correctlyVisible = value; filter(); OnPropertyChanged("CorrectlyVisible"); OnPropertyChanged("ResponseList"); } }
        public bool? ResponseVisible { get => responseVisible; set{ responseVisible = value; filter(); OnPropertyChanged("ResponseVisible"); OnPropertyChanged("ResponseList"); } }
        public string SearchLine { get => searchLine; set { searchLine = value; filter(); OnPropertyChanged("SearchLine"); OnPropertyChanged("ResponseList"); } }

        private void filter()
        {
            EntranceTestingContext connection = new EntranceTestingContext();
            List<UserResponse> _list = connection.UserResponses
                .Include(tb => tb.UserResponseArrangements)
                .Include(tb =>tb.UserResponseChooseAnswers)
                .Include(tb =>tb.UserResponseMatchTheElements)
                .Include(tb =>tb.UserResponseMatchTheValues)
                .Include(tb =>tb.UserResponseMultiplyAnswers)
                .Include(tb => tb.IdQuestionNavigation).ThenInclude(tb => tb.ElementOfArrangements)
                .Include(tb => tb.IdQuestionNavigation).ThenInclude(tb => tb.ElementOfChooses)
                .Include(tb => tb.IdQuestionNavigation).ThenInclude(tb => tb.ElementOfEqualities).ThenInclude(tb => tb.RatioOfElementEqualityIdElement1Navigations)
                .Include(tb => tb.IdQuestionNavigation).ThenInclude(tb => tb.ElementOfEqualities).ThenInclude(tb => tb.RatioOfElementEqualityIdElement2Navigations)
                .Include(tb => tb.IdQuestionNavigation).ThenInclude(tb => tb.Groups).ThenInclude(tb =>tb.ElementOfGroups)
                .Include(tb => tb.IdQuestionNavigation).ThenInclude(tb => tb.TextOfPuttings).ThenInclude(tb =>tb.ElementOfPuttings)
                .Where(tb => tb.IdSession == Session.Id).ToList();

            switch (HintVisible)
            {
                case true:
                    _list = _list.Where(tb => tb.HintApply == true).ToList();
                    break;
                case false:
                    _list = _list.Where(tb => tb.HintApply == false).ToList();
                    break;
                default: break;
            }

            switch (CorrectlyVisible)
            {
                case true:
                    _list = _list.Where(tb => tb.Correctly == true).ToList();
                    break;
                case false:
                    _list = _list.Where(tb => tb.Correctly == false).ToList();
                    break;
                default: break;
            }

            switch (ResponseVisible)
            {
                case true:
                    _list = _list.Where(tb => tb.UserResponseChooseAnswers.Count != 0 || tb.UserResponseArrangements.Count != 0 || tb.UserResponseMatchTheElements.Count != 0 || tb.UserResponseMatchTheValues.Count != 0 || tb.UserResponseMultiplyAnswers.Count != 0).ToList();
                    break;
                case false:
                    _list = _list.Where(tb => tb.UserResponseChooseAnswers.Count == 0 && tb.UserResponseArrangements.Count == 0 && tb.UserResponseMatchTheElements.Count == 0 && tb.UserResponseMatchTheValues.Count == 0 && tb.UserResponseMultiplyAnswers.Count == 0).ToList();
                    break;
                default: break;
            }

            if(!string.IsNullOrWhiteSpace(SearchLine))
            {
                _list = _list.Where(tb => tb.IdQuestionNavigation.Name.ToLower().Contains(SearchLine.ToLower())).ToList();
            }
            ResponseList = _list;
        }
    }
}