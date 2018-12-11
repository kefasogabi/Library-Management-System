using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models
{
    public class CheckOutHistory
    {
        public int Id { get; set; }
        public LibraryAsset LibraryAsset { get; set; }
        [Required]
        public LibraryCard LibraryCard { get; set; }
        public DateTime CheckedOut { get; set; }
        public DateTime? CheckedIn { get; set; }

    }
}
