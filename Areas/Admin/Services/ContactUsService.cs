using AutoMapper;
using AutoMapper.QueryableExtensions;
using FileSharing.Areas.Admin.Models;
using FileSharing.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileSharing.Areas.Admin.Services
{
    public class ContactUsService : IContactUsService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ContactUsService(ApplicationDbContext context ,IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task ChangeStatusAsync(int Id, bool status)
        {
            var selecteditem = await _context.Contacts.FindAsync(Id);
            if (selecteditem != null)
            {
                 selecteditem.Status = status;
                _context.Update(selecteditem);
                await _context.SaveChangesAsync();
            }
        }

        public IQueryable<ContactUsViewModel> GetAll()
        {
            var result = _context.Contacts.ProjectTo<ContactUsViewModel>(_mapper.ConfigurationProvider);
            return result;
        }
    }
}
