using Common.RequestMessages;
using FluentValidation;

namespace Common.CustomValidators
{
    public class MoneyTransferValidator : AbstractValidator<MoneyTransferRequest>
    {
        public MoneyTransferValidator()
        {
            RuleFor(a => a.Amount).NotNull().GreaterThan(0).WithMessage("Amount should be greater than 0");
            RuleFor(a => a.AccountNumber).NotNull().Matches(@"^\d{10}$")
                .WithMessage("Account number should be 10 digits.");
            RuleFor(a => a.IBAN).NotNull().Length(18).WithMessage("IBAN should be 18 characters");
            RuleFor(a => a.Name).NotNull().MinimumLength(2).WithMessage("Name is required.");
            RuleFor(a => a.Surname).NotNull().MinimumLength(2).WithMessage("Name is required.");
        }
    }
}