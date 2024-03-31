using Avalonia.Controls;
using EntranseTesting.Models;
using ReactiveUI;
using System.Collections.Generic;

namespace EntranseTesting.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public static EntranceTestingContext baseConnection = new EntranceTestingContext();
        
        UserControl uc = new TestMain();

        //view model для работы с основными user control
        TestMainViewModel testMain = new TestMainViewModel();
        TestPageViewModel testPage = new TestPageViewModel(0);

        // главный контейнер
        public UserControl UC { get => uc; set => this.RaiseAndSetIfChanged(ref uc, value); }
        //взаимодействие с view model
        public TestMainViewModel TestMain { get => testMain; set => testMain = value; }
        public TestPageViewModel TestPages { get => testPage; set => testPage = value; }
        

        public void StartTesting(int numVariant)
        {
            TestPages = new TestPageViewModel(numVariant);
            UC = new TestPage();
        }
       
    }
}
