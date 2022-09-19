using Common.Enums;
using Common.Helpers;
using Common.Helpers.ReferenceNumberHelper;
using Common.RequestMessages;
using Common.ResponseMessages;
using Microsoft.Extensions.Logging;
using Persistence.IRepositories;
using Persistence.Models;

namespace Application.Services;

public class MoneyTransferService : FinancialServicesBase
{
    private readonly IAccountRepository _accountRepository;
    private readonly ILogger _logger;

    public MoneyTransferService(IAccountRepository accountRepository, ILogger<AccountService> logger,
        ITransactionHistoryRepository transactionHistoryRepository,
        ITransactionLimitRepository transactionLimitRepository) : base(accountRepository, logger,
        transactionHistoryRepository, transactionLimitRepository)
    {
        _header.Type = TransactionType.MONEY_TRANSFER;
        _accountRepository = accountRepository;
        _logger = logger;
    }

    protected override async Task DoExecuteAsync(FinancialBaseRequest request, FinancialBaseResponse response)
    {
        _logger.LogInformation("Money Transfer DoExecute started");
        MoneyTransferRequest moneyTransferRequest = request as MoneyTransferRequest;
        MoneyTransferResponse moneyTransferResponse = response as MoneyTransferResponse;

        if (holderAccount.Balance < moneyTransferRequest.Amount)
        {
            throw new AppException("Balance is not enough to transfer. Please update the amount and try again.");
        }

        Account toAccount = await _accountRepository.FindByIBANAsync(moneyTransferRequest.IBAN);
        if (toAccount is null || !toAccount.Holder.Name.Equals(moneyTransferRequest.Name.Trim()) ||
            !toAccount.Holder.Surname.Equals(moneyTransferRequest.Surname.Trim()))
        {
            throw new KeyNotFoundException(
                "IBAN cannot be found with the information you entered. Please check name, surname and IBAN and try again.");
        }

        holderAccount.Balance -= moneyTransferRequest.Amount;
        toAccount.Balance += moneyTransferRequest.Amount;

        _accountRepository.Update(holderAccount);
        _accountRepository.Update(toAccount);

        String referenceNumber = ReferenceNumberGenerator.Generate();
        SetHistory(toAccount.HolderId, referenceNumber, TransactionType.MONEY_TRANSFER, moneyTransferRequest);
        
        moneyTransferResponse.Message = "Money transfer is successful.";
        
        _logger.LogInformation($"Money Transfer DoExecute ended.");
    }
}