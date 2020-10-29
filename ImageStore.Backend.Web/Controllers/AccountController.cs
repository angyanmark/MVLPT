using ImageStore.Backend.Bll.Services.Account;
using ImageStore.Backend.Common.Dtos.Account;
using ImageStore.Backend.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ImageStore.Backend.Web.Controllers
{
    public class AccountController : ApiControllerBase
    {
        private readonly AccountService _accountService;
        private readonly JwtHelper _jwtHelper;

        public AccountController(AccountService accountService, JwtHelper jwtHelper)
        {
            _accountService = accountService;
            _jwtHelper = jwtHelper;
        }

        [HttpPost("login")]
        public async Task<TokenDto> Login(LoginDto userDto)
        {
            await _accountService.LoginAsync(userDto);
            return new TokenDto() { Token = await _jwtHelper.GenerateJsonWebToken(userDto) };
        }

        [HttpPost("register")]
        public async Task Register(RegisterDto registerDto)
        {
            await _accountService.RegisterAsync(registerDto);
        }
    }
}
