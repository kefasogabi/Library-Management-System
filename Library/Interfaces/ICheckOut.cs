using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Interfaces
{
    public interface ICheckOut
    {
        IEnumerable<CheckOut> GetAll();
        CheckOut Get(int id);
        void Add(CheckOut newCheckOut);
        IEnumerable<CheckOutHistory> GetCheckOutHistory(int id);
        void PlaceHold(int assetId, int libraryCardId);
        void CheckOutItem(int id, int libraryCardId);
        void CheckInItem(int id);
        CheckOut GetLatestCheckOut(int id);
        int GetNumberOfCopies(int id);
        bool IsCheckedOut(int id);

        string GetCurrentHoldPatron(int id);
        string GetCurrentHoldPlaced(int id);
        string GetCurrentPatron(int id);
        IEnumerable<Hold> GetCurrentHolds(int id);

        void MarkLost(int id);
        void MarkFound(int id);
    }
}
