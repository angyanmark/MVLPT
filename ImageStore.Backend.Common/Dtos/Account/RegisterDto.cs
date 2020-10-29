using FluentValidation;

namespace ImageStore.Backend.Common.Dtos.Account
{
    public class RegisterDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RegisterUserDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterUserDtoValidator()
        {
            RuleFor(x => x.UserName).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(4);
        }
    }
}
