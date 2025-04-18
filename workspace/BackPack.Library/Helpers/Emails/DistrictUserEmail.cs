using BackPack.Dependency.Library.Helpers;
using BackPack.Library.Constants;
using KnomadixInfrastructure.Email;
using Microsoft.Extensions.Configuration;
using Mustache;
using Serilog;

namespace BackPack.Library.Helpers.Emails 
{
    public class DistrictUserEmail(IConfiguration configuration)
    {
        public async Task DistrictUserEmailAsync(string? UserName, string? FName, string? LName, string? EmailID, string? DName, string? SCode)
        {
            var activationLink = configuration.GetSection("CommonSettings").GetSection("ApplicationBaseUrl").Value + "activation?loginName=" + UserName + "&securityCode=" + SCode + "&DomainName=" + DName + "&Module=" + "Activation&IsStudent=0";

            var emailContent = Properties.Resources.AccActivationEmailTeacher;

            var data = new
            {
                User = FName + " " + LName,
                ActivationLinkMessage = "Activate Knomadix Account",
                ActivationLink = activationLink,
            };

            string result = Template.Compile(emailContent).Render(data);
            List<string> listTo = new List<string>();
            listTo.Add(EmailID ?? "");
            Aes256Helper aes256Helper = new(configuration);
            EmailSettings emailRequest = new()
            {
                DisplayName = configuration.GetSection(ServiceConstant.EmailSettings).GetSection(ServiceConstant.DisplayName).Value,
                From = aes256Helper.Aes256Decryption(configuration.GetSection(ServiceConstant.EmailSettings).GetSection(ServiceConstant.From).Value ?? ""),
                Host = aes256Helper.Aes256Decryption(configuration.GetSection(ServiceConstant.EmailSettings).GetSection(ServiceConstant.Host).Value ?? ""),
                UserName = aes256Helper.Aes256Decryption(configuration.GetSection(ServiceConstant.EmailSettings).GetSection(ServiceConstant.UserName).Value ?? ""),
                Password = aes256Helper.Aes256Decryption(configuration.GetSection(ServiceConstant.EmailSettings).GetSection(ServiceConstant.Password).Value ?? ""),
                Port = int.Parse(aes256Helper.Aes256Decryption(configuration.GetSection(ServiceConstant.EmailSettings).GetSection(ServiceConstant.Port).Value ?? "").ToString()),
                To = listTo,
                Subject = "Knomadix Account Registration",
                Content = result
            };

            string response = await Email.SendAsyncMail(emailRequest);
            Log.Warning("{Message}-{ErrorType}-{LoginName}-{UserFirstName}-{UserLastName}-{EmailId}-{ActivationLink}-{ErrorMessage}", response, "Email", UserName, FName, LName, EmailID, activationLink, response);
        }
    }
}
