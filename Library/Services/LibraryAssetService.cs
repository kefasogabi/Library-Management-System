using Library.Data;
using Library.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Services
{
    public class LibraryAssetService : ILibraryAsset
    {
        private readonly ApplicationDbContext context;

        public LibraryAssetService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void Add(LibraryAsset newAsset)
        {
            context.Add(newAsset);
            context.SaveChanges();
        }

        public IEnumerable<LibraryAsset> GetAll()
        {
            return context.LibraryAssets
                .Include(a => a.Status).
                ToList();
        }

        public string GetAuthorOrDirector(int id)
        {
            var isBook = context.LibraryAssets.OfType<Book>()
                .Where(a => a.Id == id).Any();
            var isVideo = context.LibraryAssets.OfType<Video>()
               .Where(a => a.Id == id).Any();

            return isBook ?
                context.Books.FirstOrDefault(a => a.Id == id).Author :
                context.Videos.FirstOrDefault(a => a.Id == id).Director
                ?? "UnKnown";
               

        }

        public LibraryAsset GetById(int id)
        {
            return GetAll()
                  .SingleOrDefault(a => a.Id == id);
                
        }

        public LibraryBranch GetCurrentLocation(int id)
        {
            return GetById(id).Location;
                //context.LibraryAssets.FirstOrDefault(a => a.Id == id).Location;
        }

        public string GetIsbn(int id)
        {
            if(context.Books.Any(a => a.Id == id))
            {
                return context.Books.FirstOrDefault(a => a.Id == id).ISBN;
            }

            return "";
        }

        public string GetTitle(int id)
        {
            return context.LibraryAssets.FirstOrDefault(a => a.Id == id).Tittle;
        }

        public string GetType(int id)
        {
            var book = context.LibraryAssets.OfType<Book>().Where(b => b.Id == id);
            return book.Any() ? "Book" : "Video";
        }
    }
}
