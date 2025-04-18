using BackPack.Library.Requests.CourseCapsule;
using FluentValidation;

namespace BackPack.WebAPI.Validators.CourseCapsule
{
    public class UpdateCourseCapsulePodValidator : AbstractValidator<UpdateCourseCapsulePodRequest>
    {
        public UpdateCourseCapsulePodValidator()
        {
            RuleFor(request => request.CourseCapsuleFolderID).NotNull();
            RuleFor(request => request.Pods).NotNull();
        }
    }
}
