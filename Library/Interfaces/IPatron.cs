using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Interfaces
{
    public interface IPatron
    {
        IEnumerable<Patron> GetAll();
        Patron Get(int id);
        void Add(Patron newPatron);
        void AddCard(LibraryCard card);
        int GetCard();
        LibraryBranch GetBranch();
        void Complete();
        void Remove(Patron patron);
        IEnumerable<CheckOutHistory> GetCheckOutHistory(int patronId);
        IEnumerable<Hold> GetHolds(int patronId);
        IEnumerable<CheckOut> GetCheckOuts(int id);
    }
}
