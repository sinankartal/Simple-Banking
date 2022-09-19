using Application.Services;
using AutoMapper;
using Common.DTOs;
using Common.Helpers;
using Common.RequestMessages;
using Common.ResponseMessages;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Persistence.IRepositories;
using Persistence.Models;
using Xunit;

namespace Application.Test;

public class AccountServiceTest
{
    private AccountService service;
    private Mock<IAccountRepository> _accountRepository;
    private Mock<IAccountHolderRepository> _accountHolderRepository;
    private Mock<IIBANStoreRepository> _ibanStoreRepository;
    private Mock<ILogger<AccountService>> _logger;
    private IMapper _mapper;

    // private Mock<IJobItemProcessRepository> _jobItemRepository;

    public AccountServiceTest()
    {
        _accountRepository = new Mock<IAccountRepository>();
        _accountHolderRepository = new Mock<IAccountHolderRepository>();
        _ibanStoreRepository = new Mock<IIBANStoreRepository>();
        _logger = new Mock<ILogger<AccountService>>();
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfile()));
        _mapper = new Mapper(configuration);
        service = new AccountService(_accountRepository.Object, _logger.Object, _accountHolderRepository.Object, _ibanStoreRepository.Object,
            _mapper);
    }

    [Fact]
    public async Task Given_UserInfo_When_Create_Account_Then_Success()
    {
        // GIVEN
        AccountCreateRequest request = new AccountCreateRequest
        {
            Name = "Sinan",
            Surname = "Kartal",
            BSN = "34576892",
            PhoneNumber = "687110269"
        };

        var iban = CreateIBAN();

        var holder = CreateAccountHolder();
        
        _ibanStoreRepository.Setup(x => x.GetNonActiveIban()).ReturnsAsync(iban);
        _accountHolderRepository.Setup(x => x.FindByBSNAsync(It.IsAny<string>())).ReturnsAsync(new AccountHolder());
        _accountHolderRepository.Setup(x => x.InsertAsync(It.IsAny<AccountHolder>())).ReturnsAsync(holder.Id);
        _accountRepository.Setup(x => x.InsertAsync(It.IsAny<Account>())).ReturnsAsync("");
        _ibanStoreRepository.Setup(x => x.UpdateActiveFlag(It.IsAny<int>(), It.IsAny<bool>())).Verifiable();
        _accountRepository.Setup(x=>x.SaveAsync()).Verifiable();
        
        // WHEN
        AccountCreateResponse response = await service.CreateAsync(request);

        // THEN
        response.Should().NotBeNull();
        response.AccountId.Should().NotBeNull();
    }
    
    [Fact]
    public async Task Given_UserInfo_When_Create_Account_Then_Fail()
    {
        // GIVEN
        AccountCreateRequest request = new AccountCreateRequest
        {
            Name = "Sinan",
            Surname = "Kartal",
            BSN = "34576892",
            PhoneNumber = "687110269"
        };

        _ibanStoreRepository.Setup(x => x.GetNonActiveIban()).ReturnsAsync(new IBANStore());
        
        // WHEN
        Func<Task> act = () => service.CreateAsync(request);

        // THEN
        var exception = await Assert.ThrowsAsync<AppException>(act);

        Assert.Equal("There is no active iban", exception.Message);
    }
    
    [Fact]
    public async Task Given_Id_When_Get_Account_Then_Success()
    {
        // GIVEN
        var account = CreateAccount("809b3b27-fd3b-4f5c-a2e4-5ab989744afb");
        _accountRepository.Setup(x => x.FindAsync(It.IsAny<string>())).ReturnsAsync(account);
        
        // WHEN
        AccountDTO dto = await service.GetAsync(account.Id);

        // THEN
        dto.Should().NotBeNull();
        Assert.Equal(dto.Id, account.Id);
    }

    private IBANStore CreateIBAN()
    {
        IBANStore iban = new IBANStore()
        {
            Id = 1,
            IBAN = "NL21ABNA8261521222",
            IsActive = false
        };
        return iban;
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
}