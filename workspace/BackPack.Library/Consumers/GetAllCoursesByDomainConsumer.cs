
using BackPack.Dependency.Library.Helpers;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Services.Interfaces.CourseCapsule;
using BackPack.MessageContract.Library;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace BackPack.Library.Consumers
{
    public class GetAllCoursesByDomainConsumer(
        ICourseCapsuleConsumerService courseCapsuleConsumerService,
        IConfiguration configuration
        ) : IConsumer<GetAllCoursesByDomainEvent>
    {
        public async Task Consume(ConsumeContext<GetAllCoursesByDomainEvent> context)
        {
            Aes256Helper aes256Helper = new(configuration);
            GlobalApplicationProperty.DBConnection = aes256Helper.Aes256Decryption(context.Message.DBConnection!);

            AllCoursesByDomainResponseEvent response = await courseCapsuleConsumerService.GetAllCoursesByDomainConsumerAsync(domainId: context.Message.DomainId);

            if (context.IsResponseAccepted<AllCoursesByDomainResponseEvent>())
            {
                await context.RespondAsync(new AllCoursesByDomainResponseEvent
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
