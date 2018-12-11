using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models
{
    public class LibraryAsset 
    {
        public int Id { get; set; }
        [Required]
        public string Tittle { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public Status Status { get; set; }
        public int StatusId { get; set; }
        [Required]
        public decimal Cost { get; set; }
        public string ImageUrl { get; set; }
        public int NumberOfCopies { get; set; }
        public virtual LibraryBranch Location { get; set; }
        public int LocationId { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        public string Author { get; set; }
        public string Discriminator { get; set; }



    }
}
