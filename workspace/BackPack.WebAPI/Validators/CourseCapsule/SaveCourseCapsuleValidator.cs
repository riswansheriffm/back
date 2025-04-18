using BackPack.Library.Requests.CourseCapsule;
using FluentValidation;

namespace BackPack.WebAPI.Validators.CourseCapsule
{
    public class SaveCourseCapsuleValidator : AbstractValidator<SaveCourseCapsuleRequest>
    {
        public SaveCourseCapsuleValidator()  
        {
            RuleFor(request => request.LoginID).NotNull().NotEmpty();
            RuleFor(request => request.CourseCapsuleID).NotNull();
        }
    }
}
