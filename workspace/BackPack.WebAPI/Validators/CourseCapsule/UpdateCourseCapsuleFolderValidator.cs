using BackPack.Library.Requests.CourseCapsule;
using FluentValidation;

namespace BackPack.WebAPI.Validators.CourseCapsule
{
    public class UpdateCourseCapsuleFolderValidator : AbstractValidator<UpdateCourseCapsuleFolderRequest>
    {
        public UpdateCourseCapsuleFolderValidator()
        {
            RuleFor(request => request.CourseCapsuleID).NotNull();
            RuleFor(request => request.Folders).NotNull();
        }
    }
}
