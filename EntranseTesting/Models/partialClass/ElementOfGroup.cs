using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntranseTesting.Models
{
    public partial class ElementOfGroup:INotifyPropertyChanged
    {
       
        private bool isActive = false;        
        int numGroup = 0;
        [NotMapped]
        public bool VisibleNum { get => (NumGroup == 0) ? false : true; }
        [NotMapped] 
        public bool IsActive { get => isActive; set { isActive = value; OnPropertyChanged("IsActive"); } }
        [NotMapped]
        public int NumGroup { get => numGroup; set { numGroup = value; OnPropertyChanged("NumGroup"); OnPropertyChanged("VisibleNum"); } }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
