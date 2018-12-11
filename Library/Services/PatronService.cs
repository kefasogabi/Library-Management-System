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
    public class PatronService : IPatron
    {
        private readonly ApplicationDbContext context;

        public PatronService( ApplicationDbContext context)
        {
            this.context = context;
        }

        public void Add(Models.Patron newPatron)
        {
            context.Add(newPatron);
            context.SaveChanges();
        }

        public Models.Patron Get(int id)
        {
            return context.Patrons
                .Include(a => a.LibraryCard)
                .Include(a => a.HomeLibraryBranch)
                .FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Models.Patron> GetAll()
        {
            return context.Patrons
               .Include(a => a.LibraryCard)
               .Include(a => a.HomeLibraryBranch);
            // Eager load this data.
        }

        public IEnumerable<CheckOutHistory> GetCheckOutHistory(int patronId)
        {
            var cardId = context.Patrons
                 .Include(a => a.LibraryCard)
                 .FirstOrDefault(a => a.Id == patronId)?
                 .LibraryCard.Id;

            return context.CheckOutHistories
                .Include(a => a.LibraryCard)
                .Include(a => a.LibraryAsset)
                .Where(a => a.LibraryCard.Id == cardId)
                .OrderByDescending(a => a.CheckedOut);
        }

        public IEnumerable<CheckOut> GetCheckOuts(int id)
        {
            var patronCardId = Get(id).LibraryCard.Id;
            return context.CheckOuts
                .Include(a => a.LibraryCard)
                .Include(a => a.LibraryAsset)
                .Where(v => v.LibraryCard.Id == patronCardId);
        }

        public IEnumerable<Hold> GetHolds(int patronId)
        {
            var cardId = context.Patrons
                .Include(a => a.LibraryCard)
                .FirstOrDefault(a => a.Id == patronId)?
                .LibraryCard.Id;

            return context.Holds
                .Include(a => a.LibraryCard)
                .Include(a => a.LibraryAsset)
                .Where(a => a.LibraryCard.Id == cardId)
                .OrderByDescending(a => a.HoldPlaced);
        }
    }
}
