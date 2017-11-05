using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encryption
{
    public class PhotoContext : DbContext
    {
        public DbSet<Photo> Photos { get; set; }
    }
}
