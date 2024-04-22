using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntranseTesting.Models
{
    public partial class QuestionHint : INotifyPropertyChanged
    {
        private bool hintPurchased = false;
        [NotMapped]
        public bool HintPurchased
        {
            get => hintPurchased;
            set
            {
                hintPurchased = value;
                OnPropertyChanged("HintPurchased");
                OnPropertyChanged("NoHintPurchased");
            }
        }
        [NotMapped]
        public bool NoHintPurchased
        {
            get => !hintPurchased;
        }
        [NotMapped]
        public string CostLine
        {
            get
            {
                switch (Cost%10)
                {
                    case 1:
                        {
                            return Cost + " очко";
                        }
                    case 2:
                    case 3:
                    case 4:
                        {
                            return Cost + " очка";
                        }
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                    case 0:
                        {
                            return Cost + " очков";
                        }
                }
                return Cost + "очков";
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
