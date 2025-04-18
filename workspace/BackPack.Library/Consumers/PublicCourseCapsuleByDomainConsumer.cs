
using BackPack.Dependency.Library.Helpers;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Services.Interfaces.CourseCapsule;
using BackPack.Library.Services.Services.CourseCapsule;
using BackPack.MessageContract.Library;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace BackPack.Library.Consumers
{
    public class PublicCourseCapsuleByDomainConsumer(
        ICourseCapsuleConsumerService courseCapsuleConsumerService,
        IConfiguration configuration
        ) : IConsumer<GetCourseCapsuleEvent>
    {
        public async Task Consume(ConsumeContext<GetCourseCapsuleEvent> context)
        {
            Aes256Helper aes256Helper = new(configuration);
            GlobalApplicationProperty.DBConnection = aes256Helper.Aes256Decryption(context.Message.DBConnection!);

            PublicCourseCapsuleByDomainResponseEvent response = await courseCapsuleConsumerService.PublicCourseCapsuleByDomainAndSubjectConsumerAsync(domainId: context.Message.DomainId, subjectId: context.Message.SubjectId);
                        
            if (context.IsResponseAccepted<PublicCourseCapsuleByDomainResponseEvent>())
            {
                await context.RespondAsync(new PublicCourseCapsuleByDomainResponseEvent
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
