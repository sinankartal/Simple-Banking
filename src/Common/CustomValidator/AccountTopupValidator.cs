using Common.RequestMessages;
using FluentValidation;

namespace Common.CustomValidators
{
    public class AccountTopupValidator : AbstractValidator<FinancialBaseRequest>
    {
        public AccountTopupValidator()
        {
            RuleFor(a => a.Amount).NotNull().GreaterThan(0).WithMessage("Amount should be greater than 0.");
            RuleFor(a => a.AccountNumber).NotNull().Matches(@"^\d{10}$").WithMessage("Account number should be 10 digits.");
        }
    }
}