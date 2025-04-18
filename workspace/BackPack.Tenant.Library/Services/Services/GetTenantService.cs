
using BackPack.MessageContract.Library;
using BackPack.Tenant.Library.Repositories.Interfaces;
using BackPack.Tenant.Library.Responses;
using BackPack.Tenant.Library.Services.Interfaces;
using MassTransit;

namespace BackPack.Tenant.Library.Services.Services
{
    public class GetTenantService(
        IGetAllTenantRepository getAllTenantRepository,
        IGetAllPublicActiveTenantRepository getAllPublicActiveTenantRepository,        
        IRequestClient<GetDomainEvent> requestGetDomainClient,
        IGetTenantByTenantIDRepository getTenantByTenantIDRepository,
        IGetAllActiveTenantRepository getAllActiveTenantRepository
        ) : IGetTenantService
    {
        #region GetAllTenantsAsync
        public async Task<GetAllDistrictResponse> GetAllTenantsAsync()
        {
            GetAllDistrictResponse response = await getAllTenantRepository.GetAllTenantsAsync();

            return response;
        }
        #endregion

        #region GetAllPublicActiveTenantsAsync
        public async Task<AllPublicActiveDomainsResultResponse> GetAllPublicActiveTenantsAsync()
        {
            AllPublicActiveDomainsResultResponse response = await getAllPublicActiveTenantRepository.GetAllPublicActiveTenantsAsync();

            return response;
        }
        #endregion

        #region GetAllActiveTenantsAsync
        public async Task<GetAllActiveDomainsResponse> GetAllActiveTenantsAsync()
        {
            GetAllActiveDomainsResponse response = await getAllActiveTenantRepository.GetAllActiveTenantsAsync();

            return response;
        }
        #endregion

        #region GetTenantAsync
        public async Task<GetDomainAcceptedEvent> GetTenantAsync(int domainID, Guid tenantID)
        {
            GetTenantByTenantNameResponse dbResponse = await getTenantByTenantIDRepository.GetTenantDBConnection(tenantID: tenantID);

            Response<GetDomainAcceptedEvent> consumerResponse = await requestGetDomainClient!.GetResponse<GetDomainAcceptedEvent>(new GetDomainEvent()
            {
                DomainID = domainID,
                DBConnection = dbResponse.Data.DBConnection
            }, timeout: TimeSpan.FromSeconds(60));

            return new GetDomainAcceptedEvent()
            {
                Success = consumerResponse.Message.Success,
                StatusCode = consumerResponse.Message.StatusCode,
                Data = consumerResponse.Message.Data,
                StatusMessage = consumerResponse.Message.StatusMessage,
                ExceptionMessage = consumerResponse.Message.ExceptionMessage,
                ExceptionType = consumerResponse.Message.ExceptionType,
            };
        }
        #endregion
    }
}
