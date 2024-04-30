using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntranseTesting.Models
{
    public partial class Messages: ObservableObject
    {
        [ObservableProperty] 
        string warningSettingsApp = "* В случае если кто-то начал тест раньше чем были изменены настройки теста, начатый тест и его результат будет составлен по предыдущим настройкам.";
        [ObservableProperty]
        string toolTipHintCount = "Подсказка - интерактивный элемент, который облегчает задачу в ответе на вопрос. Каждая подсказка имеет цену, вы можете ограничить количество возможных подсказок дав меньшее количество подсказок";
        [ObservableProperty]
        string aboutEvaluationTest = "* Для оценивания работ необходимо проставить нижние границы оценки (в баллах)";
        [ObservableProperty]
        string infoHint = "Подсказка - интерактивный элемент, который облегчает задачу в ответе на вопрос. Каждая подсказка имеет свою цену.\n\nПодсказки могут быть исключены из теста по желанию преподавателя.";

    }
}
