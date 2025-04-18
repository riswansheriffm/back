
using BackPack.Dependency.Library.Helpers;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Services.Interfaces.CourseCapsule;
using BackPack.MessageContract.Library;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace BackPack.Library.Consumers
{
    public class GetCourseCapsuleByCapsuleConsumer(
        ICourseCapsuleConsumerService courseCapsuleConsumerService,
        IConfiguration configuration
        ) : IConsumer<GetCourseCapsuleByCapsuleEvent>
    {
        public async Task Consume(ConsumeContext<GetCourseCapsuleByCapsuleEvent> context)
        {
            Aes256Helper aes256Helper = new(configuration);
            GlobalApplicationProperty.DBConnection = aes256Helper.Aes256Decryption(context.Message.DBConnection!);

            CourseCapsuleByCapsuleResponseEvent response = await courseCapsuleConsumerService.GetCourseCapsuleByCapsuleConsumerAsync(courseCapsuleId: context.Message.CourseCapsuleId);

            if (context.IsResponseAccepted<CourseCapsuleByCapsuleResponseEvent>())
            {
                await context.RespondAsync(new CourseCapsuleByCapsuleResponseEvent
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
