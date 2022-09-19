using Common.RequestMessages;
using Common.ResponseMessages;

namespace Application.IServices;

public interface IFinancialServices
{
    Task<FinancialBaseResponse> Execute(FinancialBaseRequest request); 
}