using Avalonia.Media.Imaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntranseTesting.Models
{
    public partial class HintImage
    {
        public Bitmap hImage
        {
            get
            {
                Stream stream = new MemoryStream(Image);
                return new Bitmap(stream);
            }
        }
    }
}
