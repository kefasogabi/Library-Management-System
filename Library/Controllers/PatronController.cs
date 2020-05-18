using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Data;
using Library.Interfaces;
using Library.Models;
using Library.ViewModel.Patron;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    [Authorize]
    public class PatronController : Controller
    {
        private readonly IPatron patron;

        public PatronController(IPatron patron)
        {
            this.patron = patron;
        }
        public IActionResult Index()
        {
            var allPatrons = patron.GetAll();

            var patronModels = allPatrons
                .Select(p => new PatronDetailModel
                {
                    Id = p.Id,
                    LastName = p.LastName ?? "No First Name Provided",
                    FirstName = p.FirstName ?? "No Last Name Provided",
                    LibraryCardId = p.LibraryCard?.Id,
                    OverdueFees = p.LibraryCard?.Fees,
                   
                }).ToList();

            var model = new PatronIndexModel
            {
                Patrons = patronModels
            };

            return View(model);
        }

        public IActionResult Detail(int id)
        {
            var patro = patron.Get(id);

            var model = new PatronDetailModel
            {
                Id = patro.Id,
                LastName = patro.LastName ?? "No Last Name Provided",
                FirstName = patro.FirstName ?? "No First Name Provided",
                Address = patro.Address ?? "No Address Provided",
                MemberSince = patro.LibraryCard?.Created,
                OverdueFees = patro.LibraryCard?.Fees,
                LibraryCardId = patro.LibraryCard?.Id,
                Telephone = string.IsNullOrEmpty(patro.TelephoneNumber) ? "No Telephone Number Provided" : patro.TelephoneNumber,
                AssetsCheckedOut = patron.GetCheckOuts(id).ToList(),
                CheckoutHistory = patron.GetCheckOutHistory(id),
                Holds = patron.GetHolds(id)
            };

            return View(model);
        }

        public IActionResult Create()
        {
         
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind] Patron patrons)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new PatronModel
                {

                };
            }

            if(patrons.Id == 0)
            {
                var card = new LibraryCard
                {
                    Created = DateTime.Now,
                    Fees = 0
                };
                patron.AddCard(card);

                patron.Complete();
                patrons.LibraryCardId = patron.GetCard();
                patrons.HomeLibraryBranch = patron.GetBranch();
                patron.Add(patrons);
            }
            else
            {
                var patronInDb = patron.Get(patrons.Id);
                patronInDb.Address = patrons.Address;
                patronInDb.DateOfBirth = patrons.DateOfBirth;
                patronInDb.FirstName = patrons.FirstName;
                patronInDb.LastName = patrons.LastName;
                patronInDb.LibraryCard = patrons.LibraryCard;
                patronInDb.TelephoneNumber = patrons.TelephoneNumber;
            }


            patron.Complete();

            return RedirectToAction("Index", "Patron");
           
        }

        
        public IActionResult Edit( int id)
        {
            var patro = patron.Get(id);

            if (patro == null)
            {
                return NotFound();
            }

            return View(patro);
        }

       
        public IActionResult Delete(int id)
        {
            var patro = patron.Get(id);

            if (patro == null)
                return NotFound();

           

            return View(patro);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult ConfirmDelete(int id)
        {
            var patro = patron.Get(id);

            patron.Remove(patro);
            patron.Complete();

            return RedirectToAction("Index", "Patron");
        }

    }
}