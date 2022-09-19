using Common.Enums;
using Common.RequestMessages;
using Common.ResponseMessages;
using Microsoft.Extensions.Logging;
using Persistence.IRepositories;
using Persistence.Models;

namespace Application.Services;

public class AccountTopUpService : FinancialServicesBase
{
    private readonly ITransactionFeeRepository _transactionFeeRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly ILogger _logger;

    public AccountTopUpService(IAccountRepository accountRepository, ILogger<AccountService> logger,
        ITransactionHistoryRepository transactionHistoryRepository, ITransactionFeeRepository transactionFeeRepository,
        ITransactionLimitRepository transactionLimitRepository)
        : base(accountRepository, logger, transactionHistoryRepository, transactionLimitRepository)
    {
        _header.Type = TransactionType.TOPUP;
        _transactionFeeRepository = transactionFeeRepository;
        _accountRepository = accountRepository;
        _logger = logger;
    }


    public override async Task DoExecute(FinancialBaseRequest request,
        FinancialBaseResponse response)
    {
        _logger.LogInformation($"AccountTopUpService DoExecute started");
        
        AccountTopUpRequest accountTopUpRequest = request as AccountTopUpRequest;
        AccountTopUpResponse accountTopUpResponse = response as AccountTopUpResponse;
        TransactionFee transactionFee = _transactionFeeRepository.GetFeeByType(TransactionFeeType.ACCOUNT_TOPUP);
        _header.Fee = transactionFee;
        if (transactionFee is null || string.IsNullOrEmpty(transactionFee.Id))
        {
            holderAccount.Balance += accountTopUpRequest.Amount;
        }
        else
        {
            holderAccount.Balance += accountTopUpRequest.Amount -
                                     (accountTopUpRequest.Amount * transactionFee.Percentage / 100);
        }

        _accountRepository.Update(holderAccount);
        _accountRepository.SaveAsync();

        accountTopUpResponse.Message = "Top up is successful.";
        _logger.LogInformation($"AccountTopUpService DoExecute ended");
    }
}