using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models
{
    public class LibraryCard
    {
        public int Id { get; set; }
        public decimal Fees { get; set; }
        public DateTime Created { get; set; }
        public virtual IEnumerable<CheckOut> CheckOuts { get; set; }


    }
}
