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

public abstract class FinancialServicesBase : IFinancialServices
{
    private readonly IAccountRepository _accountRepository;
    private readonly IAccountHolderRepository _accountHolderRepository;
    private readonly ITransactionHistoryRepository _transactionHistoryRepository;
    private readonly ITransactionLimitRepository _transactionLimitRepository;
    private readonly ILogger<AccountService> _logger;
    protected Account holderAccount;
    protected TransactionHeader _header = new TransactionHeader();

    public FinancialServicesBase(IAccountRepository accountRepository, ILogger<AccountService> logger,
        ITransactionHistoryRepository transactionHistoryRepository,
        ITransactionLimitRepository transactionLimitRepository)
    {
        _accountRepository = accountRepository;
        _transactionHistoryRepository = transactionHistoryRepository;
        _transactionLimitRepository = transactionLimitRepository;
        _logger = logger;
    }

    public abstract Task DoExecute(FinancialBaseRequest request, FinancialBaseResponse response);

    public async Task<FinancialBaseResponse> Execute(FinancialBaseRequest request)
    {
        var response = InstanceCreator.GetResponseInstance(request);

        holderAccount =
            await _accountRepository.GetHolderAccountAsync(request.AccountHolderId, request.AccountNumber);
        if (holderAccount is null || string.IsNullOrEmpty(holderAccount.Id))
        {
            throw new AppException($"User does not have account for {request.AccountNumber} number");
        }

        if (_header.isLimitCheck)
        {
            TransactionLimit limit = await _transactionLimitRepository.GetByTypeAsync(_header.LimitType);
            if (request.Amount < limit.MinAmount)
            {
                throw new AppException(
                    $"The amount you have entered is less than minimum transaction amount. Please enter an amount is greater than {limit.MinAmount}");
            }
            else if (request.Amount > limit.MaxAmount)
            {
                throw new AppException(
                    $"The amount you have entered is greater than maximum transaction amount. Please enter an amount is less than {limit.MaxAmount}");
            }
        }

        await DoExecute(request, response);
        _logger.LogInformation($"FinancialServies {_header.Type.ToString()} DoExecute end");

        String referenceNumber = ReferenceNumberGenerator.Generate();
        SetHistory(request.AccountHolderId, referenceNumber,
            _header.Type, request);

        if (_header.Fee is not null)
        {
            String referenceNumberFee = ReferenceNumberGenerator.Generate();
            SetHistory(request.AccountHolderId, referenceNumberFee, TransactionType.FEE,
                _header.Fee);
        }
        
        response.ReferenceNumber = referenceNumber;
        response.AccountId = holderAccount.Id;
        _logger.LogInformation($"FinancialServies end. RefNo: {referenceNumber}");
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