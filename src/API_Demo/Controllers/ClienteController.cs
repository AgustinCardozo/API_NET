using Carter;
using Carter.ModelBinding;
using Carter.Request;
using Carter.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Model.Request;
using Repository.Contracts;

namespace API_Demo.Controllers
{
    public class ClienteController : CarterModule
    {
        public ClienteController(IClienteRepository clienteRepository, ILogger<ClienteController> logger) : base("api/clientes")
        {
            Delete("/delete/{id}", async (req, res) =>
            {
                var getClienteRequest = req.RouteValues.As<string>("id");
                logger.LogInformation($"Se borra el cliente con ID: {getClienteRequest}");
                var clienteMsg = await clienteRepository.DeleteCliente(getClienteRequest);

                if (EsValidoMensaje(clienteMsg))
                {
                    res.StatusCode = StatusCodes.Status200OK;
                    logger.LogInformation("StatusCode: " + res.StatusCode.ToString());
                    await res.AsJson($"Se borrado correctamente al usuario con id {getClienteRequest}");
                }
                else
                {
                    res.StatusCode = StatusCodes.Status404NotFound;
                    logger.LogError("StatusCode: " + res.StatusCode.ToString());
                    await res.AsJson($"No se encontro al usuario con id {getClienteRequest}");
                }
            });

            Get("/", async (req, res) =>
            {
                logger.LogInformation("Listado de clientes");
                var clientes = await clienteRepository.GetClientes();

                if (clientes != null)
                {
                    res.StatusCode = StatusCodes.Status200OK;
                    logger.LogInformation("StatusCode: "+res.StatusCode.ToString());
                    await res.AsJson(clientes);
                }
                else
                {
                    res.StatusCode = StatusCodes.Status500InternalServerError;
                    logger.LogError("StatusCode: " + res.StatusCode.ToString());
                }
            });

            Get("/{id}", async (req, res) =>
            {
                var getClienteRequest = req.RouteValues.As<string>("id");
                logger.LogInformation($"Se consulta el cliente con ID: {getClienteRequest}");
                var clientes = await clienteRepository.GetClientes(getClienteRequest);

                if (clientes != null && clientes.Count != 0)
                {
                    res.StatusCode = StatusCodes.Status200OK;
                    logger.LogInformation("StatusCode: " + res.StatusCode.ToString());
                    await res.AsJson(clientes);
                }
                else
                {
                    res.StatusCode = StatusCodes.Status404NotFound;
                    logger.LogWarning("StatusCode: " + res.StatusCode.ToString());
                    await res.AsJson($"No se encontro el cliente: {getClienteRequest}");
                }
            });

            Post("/create", async (req, res) =>
            {
                logger.LogInformation("Se crear un nuevo cliente");
                var cliente = await req.BindAndValidate<ClienteReq>();
                
                if (cliente.ValidationResult.IsValid)
                {
                    var clienteMsg = await clienteRepository.SetCliente(cliente.Data, "create");

                    if (EsValidoMensaje(clienteMsg))
                    {
                        res.StatusCode = StatusCodes.Status200OK;
                        logger.LogInformation("StatusCode: " + res.StatusCode.ToString());
                        await res.AsJson($"El cliente {cliente.Data.idCliente} se creo con exito");
                    }
                    else
                    {
                        res.StatusCode = StatusCodes.Status400BadRequest;
                        logger.LogWarning("StatusCode: " + res.StatusCode.ToString());
                        await res.AsJson($"No se pudo crear un nuevo cliente porque ya existe {cliente.Data.idCliente}");
                    }
                }
                else
                {
                    res.StatusCode = StatusCodes.Status400BadRequest;
                    logger.LogError("StatusCode: " + res.StatusCode.ToString());
                    await res.AsJson(cliente.ValidationResult.Errors[0].ErrorMessage);
                }
            });

            Post("/update", async (req, res) =>
            {
                var cliente = await req.Bind<ClienteReq>();
                logger.LogInformation($"Se modifica el cliente con ID: {cliente.idCliente}");
                var clienteMsg = await clienteRepository.SetCliente(cliente, "update");

                if (EsValidoMensaje(clienteMsg))
                {
                    res.StatusCode = StatusCodes.Status200OK;
                    logger.LogInformation("StatusCode: " + res.StatusCode.ToString());
                    await res.AsJson($"El cliente {cliente.idCliente} se modifico con exito");
                }
                else
                {
                    res.StatusCode = StatusCodes.Status404NotFound;
                    logger.LogWarning("StatusCode: " + res.StatusCode.ToString());
                    await res.AsJson($"No se encontro el cliente {cliente.idCliente}");
                }
            });
        }

        private bool EsValidoMensaje(string msg)
        {
            const string errorMsg = "ERROR";
            return !string.IsNullOrEmpty(msg) && msg != errorMsg;
        }
    }
}
