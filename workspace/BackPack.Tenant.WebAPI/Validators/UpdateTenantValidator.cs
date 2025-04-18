using BackPack.Tenant.Library.Requests;
using FluentValidation;

namespace BackPack.Tenant.WebAPI.Validators
{
    public class UpdateTenantValidator : AbstractValidator<UpdateTenantRequest>
    {
        public UpdateTenantValidator()
        {
            RuleFor(request => request.TenantID).NotNull().NotEmpty();
            RuleFor(request => request.DomainID).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(request => request.Name).NotNull().NotEmpty();
            RuleFor(request => request.MaxStudents).NotNull().NotEmpty().GreaterThan(0);
            RuleFor(request => request.LoginName).NotNull().NotEmpty().Length(2, 100);
            RuleFor(request => request.FirstName).NotNull().NotEmpty().Length(1, 100);
            RuleFor(request => request.LastName).NotNull().NotEmpty().Length(1, 100);
            RuleFor(request => request.EmailID).NotNull().NotEmpty().EmailAddress();
        }
    }
}
