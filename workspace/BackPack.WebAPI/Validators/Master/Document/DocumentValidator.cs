using BackPack.Library.Requests.Master.Document;
using FluentValidation;

namespace BackPack.WebAPI.Validators.Master.Document
{
    public class DocumentValidator : AbstractValidator<DocumentRequest>
    {
        public DocumentValidator()
        {
            RuleFor(request => request.documents).NotNull().NotEmpty();
        }
    }
}
