using Application.Services;
using Common.Enums;
using Common.Helpers.ReferenceNumberHelper;
using Common.RequestMessages;
using Common.ResponseMessages;
using Microsoft.Extensions.Logging;
using Moq;
using Persistence.IRepositories;
using Persistence.Models;
using Xunit;

namespace Application.Test;

public class AccountTopUpServiceTest
{
    private AccountTopUpService service;
    private readonly Mock<IAccountRepository> _accountRepository;
    private readonly Mock<IAccountHolderRepository> _accountHolderRepository;
    private readonly Mock<IIBANStoreRepository> _ibanStoreRepository;
    private readonly Mock<ILogger<AccountService>> _logger;
    private readonly Mock<ITransactionHistoryRepository> _transactionHistoryRepository;
    private readonly Mock<ITransactionFeeRepository> _transactionFeeRepository;
    private readonly Mock<ITransactionLimitRepository> _transactionLimitRepository;


    public AccountTopUpServiceTest()
    {
        _accountRepository = new Mock<IAccountRepository>();
        _transactionHistoryRepository = new Mock<ITransactionHistoryRepository>();
        _transactionFeeRepository = new Mock<ITransactionFeeRepository>();
        _transactionLimitRepository = new Mock<ITransactionLimitRepository>();
        _logger = new Mock<ILogger<AccountService>>();
        service = new AccountTopUpService(_accountRepository.Object, _logger.Object,
            _transactionHistoryRepository.Object, _transactionFeeRepository.Object, _transactionLimitRepository.Object);
    }

    [Fact]
    public async Task Given_AccountTopUpRequest_When_TopUpExecute_Then_Success()
    {
        FinancialBaseResponse response = new FinancialBaseResponse();
        
        string accountId = "809b3b27-fd3b-4f5c-a2e4-5ab989744afb";
        Account account = CreateAccount(accountId);
        // GIVEN
        AccountTopUpRequest request = new AccountTopUpRequest
        {
            AccountHolderId = account.HolderId,
            AccountNumber = account.AccountNumber,
            Amount = 100
        };

        _accountRepository.Setup(x => x.GetHolderAccountAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(account);

        TransactionHistory history = CreateTransactionHistory(request);
        _transactionHistoryRepository.Setup(x => x.InsertAsync(history)).ReturnsAsync(history.Id);

        TransactionFee fee = CreateTransactionFee();
        _transactionFeeRepository.Setup(x => x.GetFeeByType(It.IsAny<TransactionFeeType>())).Returns(fee);

        account.Balance += request.Amount -
                           (request.Amount * fee.Percentage / 100);
        _accountRepository.Setup(x => x.Update(account)).Verifiable();

        // WHEN
        response = await service.ExecuteAsync(request);

        // THEN
        Assert.Equal(response.AccountId, account.Id);
        Assert.Equal(response.Message, "Top up is successful.");
    }

    private AccountHolder CreateAccountHolder()
    {
        AccountHolder holder = new AccountHolder()
        {
            Id = new Guid().ToString(),
            Accounts = null,
            BSN = "34576892",
            CreateDate = DateTime.Now,
            ModifyDate = DateTime.Now,
            Name = "Sinan",
            PhoneNumber = "687110269",
            Surname = "Kartal"
        };
        return holder;
    }

    private Account CreateAccount(string id)
    {
        AccountHolder holder = CreateAccountHolder();
        Account account = new Account()
        {
            Id = id,
            AccountNumber = "8261521222",
            Balance = 0,
            CreateDate = DateTime.Now,
            ModifyDate = DateTime.Now,
            Holder = holder,
            HolderId = holder.Id,
            IBAN = "NL21ABNA8261521222"
        };
        return account;
    }

    private TransactionHistory CreateTransactionHistory(AccountTopUpRequest request)
    {
        TransactionHistory history = new TransactionHistory()
        {
            Id = "c0c63cb7-3c75-45d5-b625-2ea07f6e1d95",
            AccountHolderId = CreateAccount("809b3b27-fd3b-4f5c-a2e4-5ab989744afb").HolderId,
            CreateDate = DateTime.Now,
            ModifyDate = DateTime.Now,
            ReferenceNumber = ReferenceNumberGenerator.Generate(),
            Type = TransactionType.TOPUP,
            Data = Newtonsoft.Json.JsonConvert.SerializeObject(request)
        };

        return history;
    }

    private TransactionFee CreateTransactionFee()
    {
        TransactionFee fee = new TransactionFee()
        {
            Id = "c0c63cb7-3c75-45d5-b625-2ea07f6e1d93",
            CreateDate = DateTime.Now,
            ModifyDate = DateTime.Now,
            Percentage = 1,
            Type = TransactionFeeType.ACCOUNT_TOPUP
        };

        return fee;
    }
}