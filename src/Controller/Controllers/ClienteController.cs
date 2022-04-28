using Carter;
using Carter.ModelBinding;
using Carter.Request;
using Carter.Response;
using Microsoft.AspNetCore.Http;
using Model.Request;
using Repository.Contracts;

namespace Controller.Controllers
{
    public class ClienteController : CarterModule
    {
        public string HTTP_Methods { get; set; }
        public ClienteController(IClienteRepository clienteRepository) : base("API/clientes")
        {
            Delete("/delete/{id}", async (req, res) =>
            {
                var getClienteRequest = req.RouteValues.As<string>("id");
                var clienteMsg = await clienteRepository.DeleteCliente(getClienteRequest);

                if (clienteMsg == "OK")
                {
                    res.StatusCode = StatusCodes.Status200OK;
                    await res.AsJson($"Se borrado correctamente al usuario con id {getClienteRequest}");
                }
                else
                {
                    res.StatusCode = StatusCodes.Status404NotFound;
                    await res.AsJson($"No se encontro al usuario con id {getClienteRequest}");
                }
            });

            Get("/listado", async (req, res) =>
            {
                var clientes = await clienteRepository.GetClientes();

                if (clientes != null)
                {
                    res.StatusCode = StatusCodes.Status200OK;
                    await res.AsJson(clientes);
                }
                else
                {
                    res.StatusCode = StatusCodes.Status500InternalServerError;
                }
            });

            Get("/detalle/{id}", async (req, res) =>
            {
                var getClienteRequest = req.RouteValues.As<string>("id");
                var clientes = await clienteRepository.GetClientes(getClienteRequest);

                if (clientes != null && clientes.Count != 0)
                {
                    res.StatusCode = StatusCodes.Status200OK;
                    await res.AsJson(clientes);
                }
                else
                {
                    res.StatusCode = StatusCodes.Status404NotFound;
                    await res.AsJson($"No se encontro el cliente: {getClienteRequest}");
                }
            });

            Post("/create", async (req, res) =>
            {
                var cliente = await req.BindAndValidate<ClienteReq>();
                if (cliente.ValidationResult.IsValid)
                {
                    var clienteMsg = await clienteRepository.SetCliente(cliente.Data, "create");

                    if (clienteMsg != "ERROR")
                    {
                        res.StatusCode = StatusCodes.Status200OK;
                        await res.AsJson($"El cliente {cliente.Data.idCliente} se creo con exito");
                    }
                    else
                    {
                        await res.AsJson($"No se pudo crear un nuevo cliente porque ya existe {cliente.Data.idCliente}");
                    }
                }
                else
                {
                    res.StatusCode = StatusCodes.Status400BadRequest;
                    await res.AsJson(cliente.ValidationResult.Errors[0].ErrorMessage);
                }
            });

            Post("/update", async (req, res) =>
            {
                var cliente = await req.Bind<ClienteReq>();
                var clienteMsg = await clienteRepository.SetCliente(cliente, "update");

                if (clienteMsg != "ERROR")
                {
                    res.StatusCode = StatusCodes.Status200OK;
                    await res.AsJson($"El cliente {cliente.idCliente} se creo con exito");
                }
                else
                {
                    await res.AsJson($"No se pudo crear un nuevo cliente porque ya existe {cliente.idCliente}");
                }
            });
        }
    }
}
