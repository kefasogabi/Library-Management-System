using Library.Data;
using Library.Interfaces;
using Library.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Services
{
    public class CheckOutService : ICheckOut
    {
        private readonly ApplicationDbContext context;

        public CheckOutService( ApplicationDbContext context)
        {
            this.context = context;
        }

        public void Add(CheckOut newCheckOut)
        {
            context.Add(newCheckOut);
            context.SaveChanges();
        }
        public CheckOut Get(int id)
        {
            return context.CheckOuts.Include(c => c.LibraryAsset)
                                     .Include(c => c.LibraryCard)
                                    .FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<CheckOut> GetAll()
        {
            return context.CheckOuts;
        }
        public IEnumerable<CheckOutHistory> GetCheckOutHistory(int id)
        {
            return context.CheckOutHistories
                .Include(a => a.LibraryAsset)
                .Include(a => a.LibraryCard)
                .Where(a => a.LibraryAsset.Id == id);
        }
      
     
        public IEnumerable<Hold> GetCurrentHolds(int id)
        {
            return context.Holds
                .Include(h => h.LibraryAsset)
                .Where(a => a.LibraryAsset.Id == id);
        }

        public string GetCurrentPatron(int id)
        {
            var checkout = context.CheckOuts
                .Include(a => a.LibraryAsset)
                .Include(a => a.LibraryCard)
                .FirstOrDefault(a => a.LibraryAsset.Id == id);

            if (checkout == null) return "Not checked out.";

            var cardId = checkout.LibraryCard.Id;

            var patron = context.Patrons
                .Include(p => p.LibraryCard)
                .First(c => c.LibraryCard.Id == cardId);

            return patron.FirstName + " " + patron.LastName;
        }

        public CheckOut GetLatestCheckOut(int id)
        {
            return context.CheckOuts
                .Where(c => c.LibraryAsset.Id == id)
                .OrderByDescending(c => c.Since)
                .FirstOrDefault();
        }

        public int GetNumberOfCopies(int id)
        {
            return context.LibraryAssets
               .First(a => a.Id == id)
               .NumberOfCopies;
        }

        public bool IsCheckedOut(int id)
        {
            var isCheckedOut = context.CheckOuts.Any(a => a.LibraryAsset.Id == id);

            return isCheckedOut;
        }

        public void MarkFound(int id)
        {
            var item = context.LibraryAssets
                .First(a => a.Id == id);

            context.Update(item);
            item.Status = context.Statuses.FirstOrDefault(a => a.Name == "Available");
            var now = DateTime.Now;

            // remove any existing checkouts on the item
            var checkout = context.CheckOuts
                .FirstOrDefault(a => a.LibraryAsset.Id == id);
            if (checkout != null) context.Remove(checkout);

            // close any existing checkout history
            var history = context.CheckOutHistories
                .FirstOrDefault(h =>
                    h.LibraryAsset.Id == id
                    && h.CheckedIn == null);
            if (history != null)
            {
                context.Update(history);  
                history.CheckedIn = now;
            }

            context.SaveChanges();
        }

        public void MarkLost(int id)
        {
            var item = context.LibraryAssets
                .First(a => a.Id == id);

            context.Update(item);

            item.Status = context.Statuses.FirstOrDefault(a => a.Name == "Lost");

            context.SaveChanges();
        }

        public void PlaceHold(int assetId, int libraryCardId)
        {
            var now = DateTime.Now;

            var asset = context.LibraryAssets
                .Include(a => a.Status)
                .First(a => a.Id == assetId);

            var card = context.LibraryCards
                .First(a => a.Id == libraryCardId);

            context.Update(asset);

            if (asset.Status.Name == "Available")
                asset.Status = context.Statuses.FirstOrDefault(a => a.Name == "On Hold");

            var hold = new Hold
            {
                HoldPlaced = now,
                LibraryAsset = asset,
                LibraryCard = card
            };

            context.Add(hold);
            context.SaveChanges();
        }

        public void CheckInItem(int id)
        {
            var now = DateTime.Now;

            var item = context.LibraryAssets
                .First(a => a.Id == id);

            context.Update(item);

            // remove any existing checkouts on the item
            var checkOut = context.CheckOuts
                .Include(c => c.LibraryAsset)
                .Include(c => c.LibraryCard)
                .FirstOrDefault(a => a.LibraryAsset.Id == id);
            if (checkOut != null) context.Remove(checkOut);

            // close any existing checkout history
            var history = context.CheckOutHistories
                .Include(h => h.LibraryAsset)
                .Include(h => h.LibraryCard)
                .FirstOrDefault(h =>
                    h.LibraryAsset.Id == id
                    && h.CheckedIn == null);
            if (history != null)
            {
                context.Update(history);
                history.CheckedIn = now;
            }

            // look for current holds
            var currentHolds = context.Holds
                .Include(a => a.LibraryAsset)
                .Include(a => a.LibraryCard)
                .Where(a => a.LibraryAsset.Id == id);

            // if there are current holds, check out the item to the earliest
            if (currentHolds.Any())
            {
                CheckOutToEarliestHold(id, currentHolds);
                return;
            }

            // otherwise, set item status to available
            item.Status = context.Statuses.FirstOrDefault(a => a.Name == "Available");

            context.SaveChanges();
        }
        private void CheckOutToEarliestHold(int assetId, IEnumerable<Hold> currentHolds)
        {
            var earliestHold = currentHolds.OrderBy(a => a.HoldPlaced).FirstOrDefault();
            if (earliestHold == null) return;
            var card = earliestHold.LibraryCard;
            context.Remove(earliestHold);
            context.SaveChanges();

            CheckOutItem(assetId, card.Id);
        }

        public void CheckOutItem(int id, int libraryCardId)
        {
            if (IsCheckedOut(id)) return;

            var item = context.LibraryAssets
                .Include(a => a.Status)
                .First(a => a.Id == id);

            context.Update(item);

            item.Status = context.Statuses
                .FirstOrDefault(a => a.Name == "Checked Out");

            var now = DateTime.Now;

            var libraryCard = context.LibraryCards
                .Include(c => c.CheckOuts)
                .FirstOrDefault(a => a.Id == libraryCardId);

            var checkOut = new CheckOut
            {
                LibraryAsset = item,
                LibraryCard = libraryCard,
                Since = now,
                Until = GetDefaultCheckOutTime(now)
            };

            context.Add(checkOut);

            var checkoutHistory = new CheckOutHistory
            {
                CheckedOut = now,
                LibraryAsset = item,
                LibraryCard = libraryCard
            };

            context.Add(checkoutHistory);
            context.SaveChanges();
        }

        private DateTime GetDefaultCheckOutTime(DateTime now)
        {
            return now.AddDays(30);
        }

        public string GetCurrentHoldPatron(int id)
        {
            var hold = context.Holds
                .Include(a => a.LibraryAsset)
                .Include(a => a.LibraryCard)
                .Where(v => v.Id == id);

            var cardId = hold
                .Include(a => a.LibraryCard)
                .Select(a => a.LibraryCard.Id)
                .FirstOrDefault();

            var patron = context.Patrons
                .Include(p => p.LibraryCard)
                .First(p => p.LibraryCardId == cardId);

            return patron.FirstName + " " + patron.LastName;
        }

        public string GetCurrentHoldPlaced(int id)
        {
            var hold = context.Holds
               .Include(a => a.LibraryAsset)
               .Include(a => a.LibraryCard)
               .Where(v => v.Id == id);

            return hold.Select(a => a.HoldPlaced)
                .FirstOrDefault().ToString(CultureInfo.InvariantCulture);
        }

    }
}
