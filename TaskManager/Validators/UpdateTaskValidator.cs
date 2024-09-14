using FluentValidation;
using TaskManager.Dtos;

namespace TaskManager.Validators
{
    public class UpdateTaskValidator:AbstractValidator<UpdateTaskDto>
    {

        public UpdateTaskValidator() {
            RuleFor(x => x.Title).NotEmpty().WithMessage("El titulo de la tarea no puede estar vacio");
        RuleFor(x => x.StatusId).GreaterThanOrEqualTo(0).WithMessage("El codigo de estatus debe ser un numero no negativo");
        
        }  

    }
}
