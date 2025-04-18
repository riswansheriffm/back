using BackPack.Dependency.Library.Helpers;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Requests.District;
using BackPack.Library.Services.Interfaces.District;
using BackPack.MessageContract.Library;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace BackPack.Library.Consumers
{
    public class UpdateDomainConsumer(IDistrictService districtService, IConfiguration configuration) : IConsumer<UpdateDomainEvent>
    {
        public async Task Consume(ConsumeContext<UpdateDomainEvent> context)
        {
            Aes256Helper aes256Helper = new(configuration);
            GlobalApplicationProperty.DBConnection = aes256Helper.Aes256Decryption(context.Message.DBConnection!);

            UpdateDistrictRequest request = new()
            {
                ID = context.Message.DomainID!,
                Name = context.Message.Name!,
                Desc = context.Message.Desc!,
                StreetAddress = context.Message.StreetAddress!,
                City = context.Message.City!,
                State = context.Message.State!,
                ZipCode = context.Message.ZipCode!,
                MaxStudents = context.Message.MaxStudents!,
                MaxTeachers = context.Message.MaxTeachers!,
                ActivityBy = context.Message.ActivityBy!,
                EmailID = context.Message.EmailID!,
                FirstName = context.Message.FirstName!,
                LastName = context.Message.LastName!,
                LoginName = context.Message.LoginName!,
                PhoneNo = context.Message.PhoneNo!,
                AccessToken = context.Message.AccessToken!,
                ApplicationID = context.Message.ApplicationID!,
                SourceID = context.Message.SourceID!,
            };

            BaseResponse response = await districtService.UpdateDistrictAsync(request);

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
