using BackPack.Dependency.Library.Helpers;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Services.Interfaces.District;
using BackPack.MessageContract.Library;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace BackPack.Library.Consumers
{
    public class GetDomainConsumer(IDistrictService districtService, IConfiguration configuration) : IConsumer<GetDomainEvent>
    {
        public async Task Consume(ConsumeContext<GetDomainEvent> context)
        {
            Aes256Helper aes256Helper = new(configuration);
            GlobalApplicationProperty.DBConnection = aes256Helper.Aes256Decryption(context.Message.DBConnection!);

            GetDomainAcceptedEvent response = await districtService.GetDistrictAsync(context.Message.DomainID);            

            if (context.IsResponseAccepted<GetDomainAcceptedEvent>())
            {
                await context.RespondAsync(new GetDomainAcceptedEvent
                {
                    Success = response.Success,
                    StatusCode = response.StatusCode,
                    Data = response.Data,
                    StatusMessage = response.StatusMessage,
                    ExceptionMessage = response.ExceptionMessage,
                    ExceptionType = response.ExceptionType,
                });
            }
        }
    }
}
