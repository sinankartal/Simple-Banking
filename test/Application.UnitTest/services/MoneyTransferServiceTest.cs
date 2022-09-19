using Application.Services;
using AutoMapper;
using Common.DTOs;
using Common.Enums;
using Common.Helpers;
using Common.Helpers.ReferenceNumberHelper;
using Common.RequestMessages;
using Common.ResponseMessages;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Persistence.IRepositories;
using Persistence.Models;
using Xunit;

namespace Application.Test;

public class MoneyTransferServiceTest
{
    private MoneyTransferService service;
    private readonly Mock<IAccountRepository> _accountRepository;
    private readonly Mock<IAccountHolderRepository> _accountHolderRepository;
    private readonly Mock<IIBANStoreRepository> _ibanStoreRepository;
    private readonly Mock<ILogger<AccountService>> _logger;
    private readonly Mock<ITransactionHistoryRepository> _transactionHistoryRepository;
    private readonly Mock<ITransactionLimitRepository> _transactionLimitRepository;

    public MoneyTransferServiceTest()
    {
        _accountRepository = new Mock<IAccountRepository>();
        _transactionHistoryRepository = new Mock<ITransactionHistoryRepository>();
        _transactionLimitRepository = new Mock<ITransactionLimitRepository>();
        _logger = new Mock<ILogger<AccountService>>();
        service = new MoneyTransferService(_accountRepository.Object, _logger.Object,
            _transactionHistoryRepository.Object, _transactionLimitRepository.Object);
    }

    [Fact]
    public async Task Given_MoneyTransferRequest_When_Execute_Then_Success()
    {
        FinancialBaseResponse response = new FinancialBaseResponse();
        
        // GIVEN
        string accountId = "809b3b27-fd3b-4f5c-a2e4-5ab989744afb";
        Account account = CreateAccount(accountId, "809b3b27-fd3b-4f5c-a2e4-5ab989744asd", "43456745", "683776354",
            "Sinan", "Kartal");

        Account toAccount = CreateAccount("809b3b27-fd3b-4f5c-a2e4-5ab989744afb",
            "809b3b27-fd3b-4f5c-a2e4-5ab989744a3b", "43435654", "456443245", "Sinan", "Kartal");
        
        var request = CreateMoneyTransferRequest(account);

        accountRepositoryMock(account, toAccount);

        TransactionHistory history = CreateTransactionHistory(request);
        _transactionHistoryRepository.Setup(x => x.InsertAsync(history)).ReturnsAsync(history.Id);

        TransactionLimit limit = CreateTransactionLimit();
        
        _transactionLimitRepository.Setup(x => x.GetByTypeAsync(It.IsAny<TransactionLimitType>())).ReturnsAsync(limit);
        // WHEN
        response = await service.ExecuteAsync(request);

        // THEN
        Assert.Equal(response.AccountId, account.Id);
        Assert.Equal(response.Message, "Money transfer is successful.");
    }
    
    [Fact]
    public async Task Given_MoneyTransferRequestWithZeroBalance_When_Execute_Then_Fail()
    {
        FinancialBaseResponse response = new FinancialBaseResponse();
        
        // GIVEN
        string accountId = "809b3b27-fd3b-4f5c-a2e4-5ab989744afb";
        Account account = CreateAccount(accountId, "809b3b27-fd3b-4f5c-a2e4-5ab989744asd", "43456745", "683776354",
            "Sinan", "Kartal", 0);

        Account toAccount = CreateAccount("809b3b27-fd3b-4f5c-a2e4-5ab989744afb",
            "809b3b27-fd3b-4f5c-a2e4-5ab989744a3b", "43435654", "456443245", "Sinan", "Kartal");
        
        var request = CreateMoneyTransferRequest(account);

        accountRepositoryMock(account, toAccount);

        TransactionHistory history = CreateTransactionHistory(request);
        _transactionHistoryRepository.Setup(x => x.InsertAsync(history)).ReturnsAsync(history.Id);

        TransactionLimit limit = CreateTransactionLimit();
        
        _transactionLimitRepository.Setup(x => x.GetByTypeAsync(It.IsAny<TransactionLimitType>())).ReturnsAsync(limit);
        // WHEN
        Func<Task> act = () => service.ExecuteAsync(request);
        
        // THEN
        var exception = await Assert.ThrowsAsync<AppException>(act);
        Assert.Equal("Balance is not enough to transfer. Please update the amount and try again.", exception.Message);
    }

    private static MoneyTransferRequest CreateMoneyTransferRequest(Account account)
    {
        MoneyTransferRequest request = new MoneyTransferRequest
        {
            AccountHolderId = account.HolderId,
            AccountNumber = account.AccountNumber,
            Amount = 100,
            IBAN = "NL21ABNA8261521222",
            Name = "Sinan",
            Surname = "Kartal"
        };
        return request;
    }

    private void accountRepositoryMock(Account account, Account toAccount)
    {
        _accountRepository.Setup(x => x.GetHolderAccountAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(account);
        _accountRepository.Setup(x => x.Update(account)).Verifiable();
        _accountRepository.Setup(x => x.FindByIBANAsync(It.IsAny<string>())).ReturnsAsync(toAccount);
    }

    private AccountHolder CreateAccountHolder(string id, string bsn, string phoneNumber, string name, string surname)
    {
        AccountHolder holder = new AccountHolder()
        {
            Id = id,
            Accounts = null,
            BSN = bsn,
            CreateDate = DateTime.Now,
            ModifyDate = DateTime.Now,
            Name = name,
            PhoneNumber = phoneNumber,
            Surname = surname
        };
        return holder;
    }

    private Account CreateAccount(string id, string accountHolderId, string bsn, string phoneNumber, string name,
        string surname, decimal balance = 1000)
    {
        AccountHolder holder = CreateAccountHolder(accountHolderId, bsn, phoneNumber, name, surname);
        Account account = new Account()
        {
            Id = id,
            AccountNumber = "8261521222",
            Balance = balance,
            CreateDate = DateTime.Now,
            ModifyDate = DateTime.Now,
            Holder = holder,
            HolderId = holder.Id,
            IBAN = "NL21ABNA8261521222"
        };
        return account;
    }

    private TransactionHistory CreateTransactionHistory(MoneyTransferRequest request)
    {
        TransactionHistory history = new TransactionHistory()
        {
            Id = "c0c63cb7-3c75-45d5-b625-2ea07f6e1d95",
            AccountHolderId = CreateAccount("809b3b27-fd3b-4f5c-a2e4-5ab989744afb",
                "809b3b27-fd3b-4f5c-a2e4-5ab989744asd", "43456745", "683776354",
                "Sinan", "Kartal").HolderId,
            CreateDate = DateTime.Now,
            ModifyDate = DateTime.Now,
            ReferenceNumber = ReferenceNumberGenerator.Generate(),
            Type = TransactionType.TOPUP,
            Data = Newtonsoft.Json.JsonConvert.SerializeObject(request)
        };

        return history;
    }

    private TransactionLimit CreateTransactionLimit()
    {
        TransactionLimit limit = new TransactionLimit()
        {
            Id = "c0c63cb7-3c75-45d5-b625-2ea07f6e1d93",
            CreateDate = DateTime.Now,
            ModifyDate = DateTime.Now,
            MaxAmount = 100,
            MinAmount = 1,
            Type = TransactionLimitType.MONEY_TRANSFER,
        };

        return limit;
    }
}