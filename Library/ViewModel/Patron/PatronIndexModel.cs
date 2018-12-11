using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.ViewModel.Patron
{
    public class PatronIndexModel
    {
        public IEnumerable<PatronDetailModel> Patrons { get; set; }
    }
}
