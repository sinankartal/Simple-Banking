using Application.IServices;
using AutoMapper;
using Common.DTOs;
using Common.Helpers;
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
    private readonly ILogger<AccountService> _logger;
    private readonly IMapper _mapper;

    public AccountService(IAccountRepository accountRepository, ILogger<AccountService> logger,
        IAccountHolderRepository accountHolderRepository, IIBANStoreRepository ibanStoreRepository, IMapper mapper)
    {
        _accountRepository = accountRepository;
        _accountHolderRepository = accountHolderRepository;
        _logger = logger;
        _ibanStoreRepository = ibanStoreRepository;
        _mapper = mapper;
    }

    public async Task<AccountCreateResponse> CreateAsync(AccountCreateRequest accountCreateRequest)
    {
        _logger.LogInformation("AccountCreateService new account create started.");
        
        IBANStore ibanObj = await _ibanStoreRepository.GetNonActiveIban();

        if (ibanObj is null || ibanObj.Id == 0)
        {
            throw new AppException("There is no active iban");
        }

        var holder = await _accountHolderRepository.FindByBSNAsync(accountCreateRequest.BSN);
        if (holder is null || string.IsNullOrEmpty(holder.Id))
        {
            holder = await CreateAccountHolder(accountCreateRequest);
        }

        Account account = new Account();
        account.HolderId = holder.Id;
        account.Balance = Decimal.Zero;
        account.AccountNumber = ibanObj.AccountNumber;
        account.Holder = holder;
        account.IBAN = ibanObj.IBAN;
        account.Id = await _accountRepository.InsertAsync(account);
        _ibanStoreRepository.UpdateActiveFlag(ibanObj.Id, true);
        _accountRepository.SaveAsync();
        AccountCreateResponse response = new AccountCreateResponse
        {
            AccountId = account.Id,
            Message = "The account is created successfully."
        };
        _logger.LogInformation("AccountCreateService new account create ended.");

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

        return _mapper.Map<Account, AccountDTO>(account);
    }

    public async Task<List<AccountDTO>> GetAccountsByBsnAsync(string bsn)
    {
        List<Account> accounts = await _accountRepository.GetAccountsByBsnAsync(bsn);
        if (!accounts.Any())
        {
            throw new KeyNotFoundException($"Accounts cannot be found for {bsn}");
        }
        
        return _mapper.Map<List<Account>, List<AccountDTO>>(accounts);

    }
}