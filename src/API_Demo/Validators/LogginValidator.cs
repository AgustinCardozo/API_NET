using API_Demo.Models.Requests;
using FluentValidation;

namespace API_Demo.Validators
{
    public class LogginValidator : AbstractValidator<RegistrarUsuarioReq>
    {
        public LogginValidator()
        {
            RuleFor(req => req.usuario).NotNull().NotEmpty();
            RuleFor(req => req.nombre).NotNull().NotEmpty();
            RuleFor(req => req.password).NotNull().NotEmpty();
            RuleFor(req => req.mail).NotNull().NotEmpty().EmailAddress();
        }
    }
}
