using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.ViewModel.Patron
{
    public class PatronDetailModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? LibraryCardId { get; set; }
        public string Address { get; set; }
        public DateTime? MemberSince { get; set; }
        public string Telephone { get; set; }
        public string HomeLibrary { get; set; }
        public decimal? OverdueFees { get; set; }
        public IEnumerable<CheckOut> AssetsCheckedOut { get; set; }
        public IEnumerable<CheckOutHistory> CheckoutHistory { get; set; }
        public IEnumerable<Hold> Holds { get; set; }
    }
}
