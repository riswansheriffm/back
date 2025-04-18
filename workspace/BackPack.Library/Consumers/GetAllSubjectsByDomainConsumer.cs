
using BackPack.Dependency.Library.Helpers;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Services.Interfaces.CourseCapsule;
using BackPack.MessageContract.Library;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace BackPack.Library.Consumers
{
    public class GetAllSubjectsByDomainConsumer(
        ICourseCapsuleConsumerService courseCapsuleConsumerService,
        IConfiguration configuration
        ) : IConsumer<GetAllSubjectsByDomainEvent>
    {
        public async Task Consume(ConsumeContext<GetAllSubjectsByDomainEvent> context)
        {
            Aes256Helper aes256Helper = new(configuration);
            GlobalApplicationProperty.DBConnection = aes256Helper.Aes256Decryption(context.Message.DBConnection!);

            AllSubjectsByDomainResponseEvent response = await courseCapsuleConsumerService.GetAllSubjectsByDomainConsumerAsync(domainId: context.Message.DomainId);

            if (context.IsResponseAccepted<AllSubjectsByDomainResponseEvent>())
            {
                await context.RespondAsync(new AllSubjectsByDomainResponseEvent
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
