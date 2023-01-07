using FluentValidation;

namespace API_Demo.Validators
{
    public class ClienteValidator : AbstractValidator<ClienteReq>
    {
        public ClienteValidator()
        {
            RuleFor(req => req.idCliente).Must(IsNumeric).NotEmpty().NotNull().MaximumLength(6);
            RuleFor(req => req.razonSocial).NotNull().NotEmpty();
        }

        private static bool IsNumeric(string id)
        {
            return int.TryParse(id, out _);
        }
    }
}
