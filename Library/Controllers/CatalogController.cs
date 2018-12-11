using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Library.Data;
using Library.Interfaces;
using Library.Models;
using Library.ViewModel;
using Library.ViewModel.Catalog;
using Library.ViewModel.CheckOutModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    [Authorize]
    public class CatalogController : Controller
    {
        private readonly ILibraryAsset assets;
        private readonly ICheckOut checkOut;
        private readonly ApplicationDbContext context;
        private readonly IHostingEnvironment ho;

        public CatalogController( ILibraryAsset assets,
                                    ICheckOut checkOut,
                                    ApplicationDbContext context,
                                    IHostingEnvironment ho)
        {
            this.assets = assets;
            this.checkOut = checkOut;
            this.context = context;
            this.ho = ho;
        }
        public IActionResult Index()
        {
            var assetModels = assets.GetAll();
            var listResult = assetModels
                .Select(result => new AssetIndexListingModel
                {
                    Id = result.Id,
                    ImageUrl = result.ImageUrl,
                    AuthorOrDirector = assets.GetAuthorOrDirector(result.Id),
                    Tittle = result.Tittle,
                    Type = assets.GetType(result.Id),
                   
                });
            var model = new AssetIndexModel()
            {
                Assets = listResult
            };
            return View(model);
        }

        public IActionResult Detail(int id)
        {
            var asset = assets.GetById(id);

            var currentHolds = checkOut.GetCurrentHolds(id)
                .Select(a => new AssetHoldModel
                {
                    HoldPlaced = checkOut.GetCurrentHoldPlaced(a.Id),
                    PatronName = checkOut.GetCurrentHoldPatron(a.Id)
                });

            var model = new AssetDetailModel
            {
                AssetId = id,
                Title = asset.Tittle,
                Type = assets.GetType(id),
                Year = asset.Year,
                Cost = asset.Cost,
                Status = asset.Status.Name,
                ImageUrl = asset.ImageUrl,
                AuthorOrDirector = assets.GetAuthorOrDirector(id),
                CheckOutHistory = checkOut.GetCheckOutHistory(id),
                ISBN = assets.GetIsbn(id),
                LastChechOut = checkOut.GetLatestCheckOut(id),
                PatronName = checkOut.GetCurrentPatron(id),
                CurrentHolds = currentHolds
            }; 

            return View(model);
        }

        public IActionResult Checkout(int id)
        {
            var asset = assets.GetById(id);

            var model = new CheckOutModels
            {
                AssetId = id,
                ImageUrl = asset.ImageUrl,
                Title = asset.Tittle,
                LibraryCardId = "",
                IsCheckedOut = checkOut.IsCheckedOut(id)
            };
            return View(model);
        }

        public IActionResult Hold(int id)
        {
            var asset = assets.GetById(id);

            var model = new CheckOutModels
            {
                AssetId = id,
                ImageUrl = asset.ImageUrl,
                Title = asset.Tittle,
                LibraryCardId = "",
                HoldCount = checkOut.GetCurrentHolds(id).Count()
            };
            return View(model);
        }

        public IActionResult MarkLost(int id)
        {
            checkOut.MarkLost(id);
            return RedirectToAction("Detail", new { id });
        }
        public IActionResult MarkFound(int id)
        {
            checkOut.MarkFound(id);
            return RedirectToAction("Detail", new { id });
        }

        public IActionResult CheckIn(int id)
        {
            checkOut.CheckInItem(id);
            return RedirectToAction("Detail", new { id });
        }

        [HttpPost]
        public IActionResult PlaceCheckOut(int assetId, int libraryCardId)
        {
            checkOut.CheckOutItem(assetId, libraryCardId);
            return RedirectToAction("Detail", new { id = assetId });
        }

        [HttpPost]
        public IActionResult PlaceHold(int assetId, int libraryCardId)
        {
            checkOut.PlaceHold(assetId, libraryCardId);
            return RedirectToAction("Detail", new { id = assetId });
        }

        public IActionResult Create()
        {
            var viewModel = new AssetModel
            {

            };
            return View("Create", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind] LibraryAsset libraryAsset, IFormFile pic)
        {
            if(!ModelState.IsValid)
            {
                var viewModel = new AssetModel(libraryAsset)
                {

                };
            }

            if(libraryAsset.Id == 0)
            {
                var fileName = Path.Combine(ho.WebRootPath + "\\images\\", Path.GetFileName(pic.FileName));
                pic.CopyTo(new FileStream(fileName, FileMode.Create));

                libraryAsset.ImageUrl = "/images/" + Path.GetFileName(pic.FileName);
                libraryAsset.LocationId = 1;
                libraryAsset.StatusId = 3;
                libraryAsset.Discriminator = "Book";
                context.Add(libraryAsset);
            }
            else
            {

                var fileName = Path.Combine(ho.WebRootPath + "\\images\\", Path.GetFileName(pic.FileName));
                pic.CopyTo(new FileStream(fileName, FileMode.Create));

                var assetInDb = context.LibraryAssets.SingleOrDefault(a => a.Id == libraryAsset.Id);
                assetInDb.Author = libraryAsset.Author;
                assetInDb.ISBN = libraryAsset.ISBN;
                assetInDb.Cost = libraryAsset.Cost;
                assetInDb.NumberOfCopies = libraryAsset.NumberOfCopies;
                assetInDb.Tittle = libraryAsset.Tittle;
                assetInDb.Year = libraryAsset.Year;
                assetInDb.ImageUrl = "/images/" + Path.GetFileName(pic.FileName);
            }
            

            await context.SaveChangesAsync();

            return RedirectToAction("Index", "Catalog");
        }

        public IActionResult Edit(int id)
        {
            var asset = context.LibraryAssets.SingleOrDefault(a => a.Id == id);
            
            if(asset == null)
            {
                return NotFound();
            }

            var viewModel = new AssetModel
            {
                Id = asset.Id,
                Author = asset.Author,
                Tittle = asset.Tittle,
                Year = asset.Year,
                Cost = asset.Cost,
                ImageUrl = asset.ImageUrl.ToString(),
                NumberOfCopies = asset.NumberOfCopies,
                ISBN = asset.ISBN
            };

            return View("Edit", viewModel);
        }

        public IActionResult Delete(int? id)
        {
            var asset = context.LibraryAssets.SingleOrDefault(a => a.Id == id);

            if (asset == null)
            {
                return NotFound();
            }

            var viewModel = new AssetModel
            {
                Id = asset.Id,
                Author = asset.Author,
                Tittle = asset.Tittle,
                Year = asset.Year,
                Cost = asset.Cost,
                ImageUrl = asset.ImageUrl,
                NumberOfCopies = asset.NumberOfCopies,
                ISBN = asset.ISBN
            };

            return View("delete", viewModel);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult ConfirmDelete( int id)
        {
            var asset = context.LibraryAssets.SingleOrDefault(a => a.Id == id);

            if (asset == null)
                return NotFound();
           


            

            context.Remove(asset);
            context.SaveChanges();

            return RedirectToAction("Index", "Catalog");
        }




    }
}