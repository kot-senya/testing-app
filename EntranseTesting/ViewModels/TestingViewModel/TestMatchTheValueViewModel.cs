using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using CommunityToolkit.Mvvm.ComponentModel;
using EntranseTesting.Models;
using EntranseTesting.Models.customClass;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;

namespace EntranseTesting.ViewModels
{
	public partial class TestMatchTheValueViewModel : ObservableObject
	{
        EntranceTestingContext baseConnection = new EntranceTestingContext();
        [ObservableProperty] int numberTask;
        [ObservableProperty] string question;
        [ObservableProperty] List<ItemMatchTheValue> elements = new List<ItemMatchTheValue>();
        [ObservableProperty] List<ElementOfEquality> info = new List<ElementOfEquality>();

        public TestMatchTheValueViewModel(int numberTask)
        {
            this.numberTask = numberTask;

            Question = baseConnection.Questions.FirstOrDefault(tb => tb.Id == numberTask).Name;

            Info = baseConnection.ElementOfEqualities.Include(tb => tb.RatioOfElementEqualityIdElement1Navigations).Include(tb => tb.RatioOfElementEqualityIdElement2Navigations).ToList();

            for (int i = 0; i < Info.Count/2; i++)
                Elements.Add(new ItemMatchTheValue(Info.Select(tb => tb.Name).ToList()));            
        }
    }
}