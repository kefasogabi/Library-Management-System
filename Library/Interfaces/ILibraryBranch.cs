using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Interfaces
{
    public interface ILibraryBranch
    {
        IEnumerable<LibraryBranch> GetAll();
        IEnumerable<Patron> GetPatrons(int branchId);
        IEnumerable<LibraryAsset> GetAssets(int branchId);
        LibraryBranch Get(int branchId);
        void Add(LibraryBranch newBranch);
        IEnumerable<string> GetBranchHours(int branchId);
        bool IsBranchOpen(int branchId);
        int GetAssetCount(int branchId);
        int GetPatronCount(int branchId);
        decimal GetAssetsValue(int id);
    }
}
