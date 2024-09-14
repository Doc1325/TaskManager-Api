using FluentValidation;
using TaskManager.Dtos;

namespace TaskManager.Validators
{
    public class StatusValidator:AbstractValidator<InsertStatusDto>
    {
        public StatusValidator() { 
        RuleFor(x => x.StatusName).NotEmpty();
        }
    }
}
