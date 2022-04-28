using FluentValidation;
using Model.Request;

namespace Controller.Validators
{
    public class ClienteValidator : AbstractValidator<ClienteReq>
    {
        public ClienteValidator()
        {
            RuleFor(req => req.idCliente).Must(IsNumeric).NotEmpty().NotNull().MaximumLength(6);
        }

        private static bool IsNumeric(string id)
        {
            int result;
            return int.TryParse(id, out result);
        }
    }
}
