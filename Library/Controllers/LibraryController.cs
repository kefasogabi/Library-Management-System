using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Interfaces;
using Library.ViewModel.Branch;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    [Authorize]
    public class LibraryController : Controller
    {
        private readonly ILibraryBranch branch;

        public LibraryController( ILibraryBranch branch)
        {
            this.branch = branch;
        }

        public IActionResult Index()
        {
            var branchModels = branch.GetAll()
                 .Select(br => new BranchDetailModel
                 {
                     Id = br.Id,
                     BranchName = br.Name,
                     NumberOfAssets = branch.GetAssetCount(br.Id),
                     NumberOfPatrons = branch.GetPatronCount(br.Id),
                     IsOpen = branch.IsBranchOpen(br.Id),
                     Description = br.Description,
                     Address = br.Address,
                     Telephone = br.Telephone,
                     BranchOpenedDate = br.OpenDate.ToString("yyyy-MM-dd"),
                     TotalAssetValue = branch.GetAssetsValue(br.Id),
                     ImageUrl = br.ImageUrl,
                     HoursOpen = branch.GetBranchHours(br.Id),
                     
                    
                        
                 }).ToList();

            var model = new BranchIndexModel
            {
                Branches = branchModels
            };

            return View(model);
        }

        public IActionResult Detail(int id)
        {
            var branchs = branch.Get(id);
            var model = new BranchDetailModel
            {
                BranchName = branchs.Name,
                Description = branchs.Description,
                Address = branchs.Address,
                Telephone = branchs.Telephone,
                BranchOpenedDate = branchs.OpenDate.ToString("yyyy-MM-dd"),
                NumberOfPatrons = branch.GetPatronCount(id),
                NumberOfAssets = branch.GetAssetCount(id),
                TotalAssetValue = branch.GetAssetsValue(id),
                ImageUrl = branchs.ImageUrl,
                HoursOpen = branch.GetBranchHours(id)
            };

            return View(model);
        }
    }
}