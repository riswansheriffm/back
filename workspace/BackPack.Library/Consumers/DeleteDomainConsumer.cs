using BackPack.Dependency.Library.Helpers;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Requests.District;
using BackPack.Library.Services.Interfaces.District;
using BackPack.MessageContract.Library;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace BackPack.Library.Consumers
{
    public class DeleteDomainConsumer(IDistrictService districtService, IConfiguration configuration) : IConsumer<DeleteDomainEvent>
    {
        public async Task Consume(ConsumeContext<DeleteDomainEvent> context)
        {
            Aes256Helper aes256Helper = new(configuration);
            GlobalApplicationProperty.DBConnection = aes256Helper.Aes256Decryption(context.Message.DBConnection!);

            DistrictStatusRequest request = new()
            {
                ID = context.Message.DomainID!,
                ActivityBy = context.Message.ActivityBy,
            };

            BaseResponse response = await districtService.DistrictStatusAsync(request);

            if (context.IsResponseAccepted<ConsumerBaseResponse>())
                await context.RespondAsync(new ConsumerBaseResponse
                {
                    Success = response.Success,
                    StatusCode = response.StatusCode,
                    StatusMessage = response.StatusMessage!,
                    ExceptionType = response.ExceptionType,
                    ExceptionMessage = response.ExceptionMessage!,
                });
        }

    }
}
