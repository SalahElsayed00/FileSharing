using FileSharing.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileSharing.Areas.Admin.Services
{
    public interface IContactUsService
    {
        IQueryable<ContactUsViewModel>  GetAll();

        Task ChangeStatusAsync(int Id,bool status);
    }
}
