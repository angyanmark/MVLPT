using AutoMapper;
using ImageStore.Backend.Common.Constants;
using ImageStore.Backend.Common.Dtos.Account;
using ImageStore.Backend.Common.Exceptions;
using ImageStore.Backend.Dal;
using ImageStore.Backend.Dal.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ImageStore.Backend.Bll.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ImageStoreDbContext _dbContext;
        
        private readonly IMapper _mapper;

        public AccountService(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager, SignInManager<User> signInManager, ImageStoreDbContext dbContext, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task LoginAsync(LoginDto dto)
        {
            var signInResult = await _signInManager.PasswordSignInAsync(dto.Username, dto.Password, false, false);

            if (!signInResult.Succeeded)
                throw new ImageStoreException("Wrong username or password");
        }

        public async Task RegisterAsync(RegisterDto dto)
        {
            User userEntity = _mapper.Map<User>(dto);

            if (await _dbContext.Users.AnyAsync(u => u.NormalizedUserName == dto.UserName.ToUpper()))
                throw new ImageStoreException("The selected username is taken");

            var createdUser = await _userManager.CreateAsync(userEntity, dto.Password);
            if (!createdUser.Succeeded)
                throw new ImageStoreException("Error during registration");

            await _userManager.AddToRoleAsync(userEntity, Roles.User);
        }
    }
}
