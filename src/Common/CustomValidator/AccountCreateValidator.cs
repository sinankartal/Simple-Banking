using Common.RequestMessages;
using FluentValidation;

namespace Common.CustomValidators
{
    public class AccountCreateValidator : AbstractValidator<AccountCreateRequest>
    {
        public AccountCreateValidator()
        {
            RuleFor(a => a.Name).NotNull().WithMessage("Name code cannot be null");
            RuleFor(a => a.Surname).NotNull().WithMessage("Surname code cannot be null");
            RuleFor(a => a.BSN).NotNull().WithMessage("BSN code cannot be null").Matches(@"^\d{8}$")
                .WithMessage("BSN should be 8 digit numbers");
            RuleFor(a => a.PhoneNumber).NotNull().WithMessage("Phone number cannot be null").Matches(@"^\d{9}$")
                .WithMessage("Phone number should be 9 digit numbers");
        }
    }
}