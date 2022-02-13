using AutoMapper;
using AutoMapper.QueryableExtensions;
using FileSharing.Areas.Admin.Models;
using FileSharing.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FileSharing.Areas.Admin.Services
{
    public class UserServices : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserServices(ApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this._context = context;
            this._mapper = mapper;
            this._userManager = userManager;
            this._roleManager = roleManager;
        }
        public async Task<OperationResult> ToggleBlockUserAsync(string userId)
        {
            var existingUser = await _context.Users.FindAsync(userId);
            if (existingUser == null)
            {
                return OperationResult.NotFound();
            }
            existingUser.IsBlocked = !existingUser.IsBlocked;
            _context.Update(existingUser);
            await _context.SaveChangesAsync();
            return OperationResult.succeded();
        }

        public IQueryable<AdminUserViewModel> GetAll()
        {
            var result = _context.Users.OrderByDescending(e => e.CreateDate)
                .ProjectTo<AdminUserViewModel>(_mapper.ConfigurationProvider);
            return result;
        }

        public IQueryable<AdminUserViewModel> GetBlokedUsers()
        {
            var result = _context.Users.Where(e => e.IsBlocked)
                .OrderByDescending(e => e.CreateDate)
                .ProjectTo<AdminUserViewModel>(_mapper.ConfigurationProvider);
            return result;
        }

        public IQueryable<AdminUserViewModel> Search(string term)
        {
            var result = _context.Users
                .Where(
                e => e.Email == term
               || e.FirstName.Contains(term)
               || e.LastName.Contains(term)
               || term.Contains(e.FirstName)
               || term.Contains(e.LastName)
               || (e.FirstName + " " + e.LastName).Contains(term))
                .OrderByDescending(e => e.CreateDate)
                 .ProjectTo<AdminUserViewModel>(_mapper.ConfigurationProvider);
            return result;
        }

        public async Task<int> UserRegistrationCountAsync()
        {
            var Count = await _context.Users.CountAsync();
            return Count;
        }

        public async Task<int> UserRegistrationCountAsync(int month)
        {
            var year = DateTime.Today.Year;
            var Count = await _context.Users.Where(e => e.CreateDate.Month == month && e.CreateDate.Year == year).CountAsync();
            return Count;
        }

        public async Task InitializeAsync()
        {
            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            }

            var EmailAdmin = "Admin00@Gmail.com";
            var Password = "Admin@123";
            if (await _userManager.FindByEmailAsync(EmailAdmin) == null)
            {
                var user = new ApplicationUser
                {
                    FirstName = "Admin",
                    LastName = "role",
                    Email = EmailAdmin,
                    UserName = EmailAdmin,
                    PhoneNumber = "00201033336506"
                };
                await _userManager.CreateAsync(user, Password);
                await _userManager.AddToRoleAsync(user, UserRoles.Admin);
            }
        }
    }
}
