using Application.IServices;
using AutoMapper;
using Common.DTOs;
using Common.Enums;
using Common.Helpers;
using Common.Helpers.ReferenceNumberHelper;
using Common.RequestMessages;
using Common.ResponseMessages;
using Microsoft.Extensions.Logging;
using Persistence.IRepositories;
using Persistence.Models;

namespace Application.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IAccountHolderRepository _accountHolderRepository;
    private readonly IIBANStoreRepository _ibanStoreRepository;
    private readonly ITransactionFeeRepository _transactionFeeRepository;
    private readonly ITransactionHistoryRepository _transactionHistoryRepository;
    private readonly ILogger<AccountService> _logger;
    private readonly IMapper _mapper;

    public AccountService(IAccountRepository accountRepository, ILogger<AccountService> logger,
        IAccountHolderRepository accountHolderRepository, IIBANStoreRepository ibanStoreRepository, IMapper mapper,
        ITransactionFeeRepository transactionFeeRepository, ITransactionHistoryRepository transactionHistoryRepository)
    {
        _accountRepository = accountRepository;
        _accountHolderRepository = accountHolderRepository;
        _logger = logger;
        _ibanStoreRepository = ibanStoreRepository;
        _transactionHistoryRepository = transactionHistoryRepository;
        _mapper = mapper;
        _transactionFeeRepository = transactionFeeRepository;
    }

    public async Task<AccountCreateResponse> CreateAsync(AccountCreateRequest accountCreateRequest)
    {
        IBANStore ibanObj = await _ibanStoreRepository.GetNonActiveIban();

        if (ibanObj is null || ibanObj.Id == 0)
        {
            throw new AppException("There is no active iban");
        }

        var holder = _accountHolderRepository.FindByBSNAsync(accountCreateRequest.BSN);
        if (holder is null)
        {
            holder = await CreateAccountHolder(accountCreateRequest);
        }

        Account account = new Account();
        account.HolderId = holder.Id;
        account.Balance = Decimal.Zero;
        account.AccountNumber = ibanObj.AccountNumber;
        account.Holder = holder;
        account.IBAN = ibanObj.IBAN;
        await _accountRepository.InsertAsync(account);
        _ibanStoreRepository.UpdateActiveFlag(ibanObj.Id, true);
        _accountRepository.SaveAsync();
        AccountCreateResponse response = new AccountCreateResponse
        {
            AccountId = account.Id,
            Message = "The account is created successfully."
        };
        return response;
    }

    private async Task<AccountHolder> CreateAccountHolder(AccountCreateRequest accountCreateRequest)
    {
        AccountHolder accountHolder = new AccountHolder();
        accountHolder.Name = accountCreateRequest.Name;
        accountHolder.Surname = accountCreateRequest.Surname;
        accountHolder.BSN = accountCreateRequest.BSN;
        accountHolder.PhoneNumber = accountCreateRequest.PhoneNumber;
        accountHolder.Id = await _accountHolderRepository.InsertAsync(accountHolder);
        return accountHolder;
    }

    public async Task<AccountDTO> GetAsync(string id)
    {
        Account account = await _accountRepository.FindAsync(id);
        if (account is null)
        {
            throw new KeyNotFoundException("Account cannot be found");
        }

        AccountDTO dto = new AccountDTO();
        return _mapper.Map<Account, AccountDTO>(account);
    }

    public List<AccountDTO> GetAccountsByBsn(string bsn)
    {
        List<Account> accounts = _accountRepository.GetAccountsByBsn(bsn);
        if (!accounts.Any())
        {
            throw new KeyNotFoundException($"Accounts cannot be found for {bsn}");
        }

        List<AccountDTO> accountDtos = new List<AccountDTO>();
        return _mapper.Map(accounts, accountDtos);
    }

    public async Task<AccountTopUpResponse> TopUp(AccountTopUpRequest accountTopUpRequest)
    {
        Account holderAccount =
            _accountRepository.GetHolderAccount(accountTopUpRequest.AccountHolderId, accountTopUpRequest.AccountNumber);
        if (holderAccount is null || string.IsNullOrEmpty(holderAccount.Id))
        {
            throw new AppException($"User does not have account for {accountTopUpRequest.AccountNumber} number");
        }

        TransactionFee transactionFee = _transactionFeeRepository.GetFeeByType(TransactionFeeType.ACCOUNT_TOPUP);
        String referenceNumberTopUp = ReferenceNumberGenerator.Generate();
        TransactionHistory topUpHistory = new TransactionHistory();
        SetHistory(accountTopUpRequest.AccountHolderId, topUpHistory, referenceNumberTopUp,
            TransactionType.TOPUP, accountTopUpRequest);

        await _transactionHistoryRepository.InsertAsync(topUpHistory);

        if (transactionFee is null || string.IsNullOrEmpty(transactionFee.Id))
        {
            holderAccount.Balance += accountTopUpRequest.Amount;
        }
        else
        {
            holderAccount.Balance += accountTopUpRequest.Amount -
                                     (accountTopUpRequest.Amount *  transactionFee.Percentage / 100);

            String referenceNumberFee = ReferenceNumberGenerator.Generate();
            SetHistory(accountTopUpRequest.AccountHolderId, topUpHistory, referenceNumberFee, TransactionType.FEE,
                transactionFee);
            await _transactionHistoryRepository.InsertAsync(topUpHistory);
        }

        _accountRepository.Update(holderAccount);
        _accountRepository.SaveAsync();
        AccountTopUpResponse response = new AccountTopUpResponse()
        {
            AccountId = holderAccount.Id,
            Message = "Top up is successful.",
            ReferenceNumber = referenceNumberTopUp,
        };

        return response;
    }

    private static void SetHistory(string accountHolderId, TransactionHistory history,
        string referenceNumberTopUp, TransactionType type, object data)
    {
        history.Type = type;
        history.ReferenceNumber = referenceNumberTopUp;
        history.AccountHolderId = accountHolderId;
        history.Data = Newtonsoft.Json.JsonConvert.SerializeObject(data);
    }
}