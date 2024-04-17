using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntranseTesting.Models
{    
    public partial class ElementOfChoose
    {
        bool userCorrectly = false;
        [NotMapped]
        public bool UserCorrectly { get => userCorrectly; set => userCorrectly = value; }
       
    }
}
