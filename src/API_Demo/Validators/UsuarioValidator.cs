using FluentValidation;

namespace API_Demo.Validators
{
    public class UsuarioValidator : AbstractValidator<RegistrarUsuarioReq>
    {
        public UsuarioValidator()
        {
            RuleFor(req => req.usuario).NotNull().NotEmpty().WithMessage("Usuario inválido");
            RuleFor(req => req.nombre).NotNull().NotEmpty().WithMessage("Nombre inválido");
            RuleFor(req => req.password).NotNull().NotEmpty().WithMessage("Password inválido");
            RuleFor(req => req.mail).NotNull().NotEmpty().EmailAddress();
        }
    }
}
