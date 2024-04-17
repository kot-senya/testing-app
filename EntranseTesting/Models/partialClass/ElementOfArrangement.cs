using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntranseTesting.Models
{
    public partial class ElementOfArrangement
    {
        double height = 0;
        double width = 0;
        [NotMapped]
        public double Height { get => height; set => height = value; }
        [NotMapped]
        public double Width { get => width; set => width = value; }
    }
}
