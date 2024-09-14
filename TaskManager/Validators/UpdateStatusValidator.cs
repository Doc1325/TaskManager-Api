using FluentValidation;
using TaskManager.Dtos;

namespace TaskManager.Validators
{
    public class UpdateStatusValidator:AbstractValidator<UpdateStatusDto>
    {
        public UpdateStatusValidator() { 
        RuleFor(x => x.StatusName).NotEmpty();
        }
    }
}
