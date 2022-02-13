using FileSharing.Areas.Admin.Models;
using System.Linq;
using System.Threading.Tasks;

namespace FileSharing.Areas.Admin.Services
{
    public interface IUserService
    {
        //Get  All User
        IQueryable<AdminUserViewModel> GetAll();

        //Get Bocked User
        IQueryable<AdminUserViewModel> GetBlokedUsers();

        //Search on User by username or email 
        IQueryable<AdminUserViewModel> Search(string term);

        Task<OperationResult> ToggleBlockUserAsync(string userId);

        //Get Count Of User Register
        Task<int> UserRegistrationCountAsync();

        //Get Count Of User Register
        Task<int> UserRegistrationCountAsync(int month);

        // create roles
        Task InitializeAsync();
    }
}
