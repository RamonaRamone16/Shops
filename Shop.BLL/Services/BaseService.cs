using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Shop.DAL;
using Shop.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.BLL.Services
{
    public class BaseService
    {
        protected readonly ApplicationDbContext _context; 
        protected readonly IMapper _mapper; 
        protected readonly UserManager<User> _userManager; 

        protected BaseService(ApplicationDbContext context, IMapper mapper, UserManager<User> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }


    }
}
