using Common.RequestMessages;
using Common.ResponseMessages;

namespace Application.IServices;

public interface IAccountActivitiesService
{
    Task<AccountActivitiesResponse> Execute(AccountActivitiesRequest request); 
}