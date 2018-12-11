using Library.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library.ViewModel.Catalog
{
    public class AssetModel
    {
        public int Id { get; set; }
        [Required]
        public string Tittle { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public Status Status { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public decimal Cost { get; set; }
        public string ImageUrl { get; set; }
        public int NumberOfCopies { get; set; }
        public virtual LibraryBranch Location { get; set; }

        public AssetModel()
        {
            Id = 0;
        }

        public AssetModel(LibraryAsset asset)
        {
            Id = asset.Id;
            Author = asset.Author;
            ISBN = asset.ISBN;
            Cost = asset.Cost;
            ImageUrl = asset.ImageUrl;
            NumberOfCopies = asset.NumberOfCopies;
            Tittle = asset.Tittle;
            Year = asset.Year;
            
        }


       
        
    }
}
