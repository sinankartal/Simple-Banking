using Application.IServices;
using AutoMapper;
using Common.Enums;
using Common.Helpers.ReferenceNumberHelper;
using Common.RequestMessages;
using Common.ResponseMessages;
using Microsoft.Extensions.Logging;
using Persistence.IRepositories;
using Persistence.Models;

namespace Application.Services;

public class AccountTopUpService : BaseAccountActivites
{
    private readonly ITransactionFeeRepository _transactionFeeRepository;
    private readonly IAccountRepository _accountRepository;

    public AccountTopUpService(IAccountRepository accountRepository, ILogger<AccountService> logger,
        ITransactionHistoryRepository transactionHistoryRepository, ITransactionFeeRepository transactionFeeRepository)
        : base(accountRepository, logger, transactionHistoryRepository)
    {
        _header.Type = TransactionType.TOPUP;
        _transactionFeeRepository = transactionFeeRepository;
        _accountRepository = accountRepository;
    }


    public override async Task DoExecute(AccountActivitiesRequest request,
        AccountActivitiesResponse response)
    {
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
    }
}