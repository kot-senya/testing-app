using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using EntranseTesting.Models;
using ReactiveUI;

namespace EntranseTesting.ViewModels
{
	public class TestMainViewModel : ReactiveObject
	{
		public List<int> Variant { get; } = MainWindowViewModel.baseConnection.QuestionVariants.Select(tb => tb.Variant).Distinct().OrderBy(x => x).ToList();

    }
}