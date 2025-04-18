using BackPack.Library.Requests.CourseCapsule;
using FluentValidation;

namespace BackPack.WebAPI.Validators.CourseCapsule
{
    public class SaveCourseCapsuleFolderValidator : AbstractValidator<SaveCourseCapsuleFolderRequest>
    {
        public SaveCourseCapsuleFolderValidator()
        {
            RuleFor(request => request.LoginID).NotNull().NotEmpty();
            RuleFor(request => request.CourseCapsuleID).NotNull();
        }
    }
}
