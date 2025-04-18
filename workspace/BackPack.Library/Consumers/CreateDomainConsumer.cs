using BackPack.Dependency.Library.Helpers;
using BackPack.Dependency.Library.Responses;
using BackPack.Library.Requests.District;
using BackPack.Library.Responses.School;
using BackPack.Library.Services.Interfaces.District;
using BackPack.MessageContract.Library;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace BackPack.Library.Consumers
{
    public class CreateDomainConsumer(IDistrictService districtService, IConfiguration configuration) : IConsumer<CreateDomainEvent>
    {
        public async Task Consume(ConsumeContext<CreateDomainEvent> context)
        {
            Aes256Helper aes256Helper = new(configuration);
            GlobalApplicationProperty.DBConnection = aes256Helper.Aes256Decryption(context.Message.DBConnection!);

            CreateDistrictRequest request = new()
            {
                TenantID = context.Message.TenantID,
                Country = context.Message.Country!,
                Name = context.Message.Name!,
                Desc = context.Message.Desc!,
                StreetAddress = context.Message.StreetAddress!,
                City = context.Message.City!,
                State = context.Message.State!,
                ZipCode = context.Message.ZipCode!,
                MaxStudents = context.Message.MaxStudents!,
                MaxTeachers = context.Message.MaxTeachers!,
                ActivityBy = context.Message.ActivityBy!,
                AccessType = context.Message.AccessType!,
                EmailID = context.Message.EmailID!,
                FirstName = context.Message.FirstName!,
                LastName = context.Message.LastName!,
                LoginName = context.Message.LoginName!,
                PhoneNo = context.Message.PhoneNo!,
                AccessToken = context.Message.AccessToken!,
                ApplicationID = context.Message.ApplicationID!,
                SourceID = context.Message.SourceID!,
            };


            CreateSchoolResponse response = await districtService.CreateDistrictAsync(request);

            if (context.IsResponseAccepted<CreateDomainAcceptedEvent>())
                await context.RespondAsync(new CreateDomainAcceptedEvent
                {
                    DomainID = response.DomainID,
                    Success = response.Success,
                    StatusCode = response.StatusCode,
                    StatusMessage = response.StatusMessage!,
                });
        }

    }
}
