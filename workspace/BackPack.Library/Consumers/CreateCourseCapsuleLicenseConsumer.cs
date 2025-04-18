
using BackPack.Dependency.Library.Helpers;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Requests.CourseCapsule;
using BackPack.Library.Services.Interfaces.CourseCapsule;
using BackPack.MessageContract.Library;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace BackPack.Library.Consumers
{
    public class CreateCourseCapsuleLicenseConsumer(
        ICourseCapsuleLicenseService courseCapsuleLicenseService,
        IConfiguration configuration
        ) : IConsumer<CreateCourseCapsuleLicenseEvent>
    {
        public async Task Consume(ConsumeContext<CreateCourseCapsuleLicenseEvent> context)
        {
            Aes256Helper aes256Helper = new(configuration);
            GlobalApplicationProperty.DBConnection = aes256Helper.Aes256Decryption(context.Message.DBConnection!);

            GlobalApplicationProperty.UserID = context.Message.ActivityBy;
            GlobalApplicationProperty.UserTypeID = 3;

            CreateCourseCapsuleLicenseRequest request = new()
            {
                DomainID = context.Message.DomainID,
                LoginID = context.Message.LoginID,
                CourseCapsuleID = context.Message.CourseCapsuleID,
                CourseID = context.Message.CourseID,
                StudentIDs = context.Message.StudentIDs,
                Duration = context.Message.Duration,
                WhomToLicense = context.Message.WhomToLicense,
                StartDate = context.Message.StartDate,
                LicenseAction = context.Message.LicenseAction,
                LicenseType = context.Message.LicenseType
            };

            BaseResponse response = await courseCapsuleLicenseService.CreateCourseCapsuleLicenseAsync(request);

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
