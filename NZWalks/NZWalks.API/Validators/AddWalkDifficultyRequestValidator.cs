using FluentValidation;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Validators
{
    public class AddWalkDifficultyRequestValidator : AbstractValidator<AddWalkDifficultyDto>
    {
        public AddWalkDifficultyRequestValidator()
        {
            RuleFor(x=>x.Code).NotEmpty();
        }
    }
}
