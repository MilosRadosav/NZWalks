using FluentValidation;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Validators
{
    public class UpdateWalkDificcultyRequestValidator: AbstractValidator<UpdateWalkDifficultyDto>
    {
        public UpdateWalkDificcultyRequestValidator()
        {
            RuleFor(x => x.Code).NotEmpty();
        }
    }
}
