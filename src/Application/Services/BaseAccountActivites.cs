using Application.IServices;
using Application.TransactionHelper;
using Common;
using Common.Enums;
using Common.Helpers;
using Common.Helpers.ReferenceNumberHelper;
using Common.RequestMessages;
using Common.ResponseMessages;
using Microsoft.Extensions.Logging;
using Persistence.IRepositories;
using Persistence.Models;

namespace Application.Services;

public abstract class BaseAccountActivites : IAccountActivitiesService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IAccountHolderRepository _accountHolderRepository;
    private readonly ITransactionHistoryRepository _transactionHistoryRepository;
    private readonly ILogger<AccountService> _logger;
    protected Account holderAccount;
    protected TransactionHeader _header = new TransactionHeader();

    public BaseAccountActivites(IAccountRepository accountRepository, ILogger<AccountService> logger,
        ITransactionHistoryRepository transactionHistoryRepository)
    {
        _accountRepository = accountRepository;
        _transactionHistoryRepository = transactionHistoryRepository;
        _logger = logger;
    }

    public abstract Task DoExecute(AccountActivitiesRequest request, AccountActivitiesResponse response);

    public async Task<AccountActivitiesResponse> Execute(AccountActivitiesRequest request)
    {
        var response = InstanceCreator.GetResponseInstance(request);

        holderAccount =
            _accountRepository.GetHolderAccount(request.AccountHolderId, request.AccountNumber);
        if (holderAccount is null || string.IsNullOrEmpty(holderAccount.Id))
        {
            throw new AppException($"User does not have account for {request.AccountNumber} number");
        }

        if (_header.isLimitCheck)
        {
        }

        await DoExecute(request, response);

        String referenceNumber = ReferenceNumberGenerator.Generate();
        SetHistory(request.AccountHolderId, referenceNumber,
            _header.Type, request);

        if (_header.Fee is not null)
        {
            String referenceNumberFee = ReferenceNumberGenerator.Generate();
            SetHistory(request.AccountHolderId, referenceNumberFee, TransactionType.FEE,
                _header.Fee);
        }
        
        return response;
    }

    protected async void SetHistory(string accountHolderId,
        string referenceNumberTopUp, TransactionType type, object data)
    {
        TransactionHistory history = new TransactionHistory()
        {
            Type = type,
            ReferenceNumber = referenceNumberTopUp,
            AccountHolderId = accountHolderId,
            Data = Newtonsoft.Json.JsonConvert.SerializeObject(data)
        };
        await _transactionHistoryRepository.InsertAsync(history);
    }
}