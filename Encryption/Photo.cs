using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encryption
{
    public class Photo
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public byte[] ImgData { get; set; }
    }
}
