using EntityLayer.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ValidationRules
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("İsim Boş Geçilemez.");
            RuleFor(x => x.FirstName).MinimumLength(3).WithMessage("İsim üç karakterden fazla olmalı");

            RuleFor(x => x.LastName).NotEmpty().WithMessage("Soyisim Boş Geçilemez.");
      
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email Boş Geçilemez.");

            RuleFor(x => x.PasswordHash)
            .NotEmpty().WithMessage("Şifre Boş Geçilemez.")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+={}\[\]:;""'<>,.?/~`-]).{8,}$")
            .WithMessage("Şifre en az 8 karakter, bir büyük harf, bir küçük harf, bir rakam ve bir özel karakter içermelidir.");

        }
    }
}
