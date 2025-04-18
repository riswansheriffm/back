
using BackPack.Dependency.Library.Helpers;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Services.Interfaces.CourseCapsule;
using BackPack.MessageContract.Library;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace BackPack.Library.Consumers
{
    public class GetAllLPCourseLicensesConsumer(
        ICourseCapsuleConsumerService courseCapsuleConsumerService,
        IConfiguration configuration
        ) : IConsumer<GetAllLPCourseLicensesEvent>
    {
        public async Task Consume(ConsumeContext<GetAllLPCourseLicensesEvent> context)
        {
            Aes256Helper aes256Helper = new(configuration);
            GlobalApplicationProperty.DBConnection = aes256Helper.Aes256Decryption(context.Message.DBConnection!);

            LPCourseLicensesResponseEvent response = await courseCapsuleConsumerService.GetAllLPCourseLicensesConsumerAsync(domainId: context.Message.DomainId, courseId: context.Message.CourseID, courseCapsuleId: context.Message.CourseCapsuleId);

            if (context.IsResponseAccepted<LPCourseLicensesResponseEvent>())
            {
                await context.RespondAsync(new LPCourseLicensesResponseEvent
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
