
using BackPack.Dependency.Library.Helpers;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Services.Interfaces.CourseCapsule;
using BackPack.MessageContract.Library;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace BackPack.Library.Consumers
{
    public class GetAllTeachersByClassConsumer(
        ICourseCapsuleConsumerService courseCapsuleConsumerService,
        IConfiguration configuration
        ) : IConsumer<GetAllTeacherByClassEvent>
    {
        public async Task Consume(ConsumeContext<GetAllTeacherByClassEvent> context)
        {
            Aes256Helper aes256Helper = new(configuration);
            GlobalApplicationProperty.DBConnection = aes256Helper.Aes256Decryption(context.Message.DBConnection!);

            AllTeacherByClassResponseEvent response = await courseCapsuleConsumerService.GetAllTeacherByClassConsumerAsync(domainId: context.Message.DomainId, courseId: context.Message.CourseID);

            if (context.IsResponseAccepted<AllTeacherByClassResponseEvent>())
            {
                await context.RespondAsync(new AllTeacherByClassResponseEvent
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
