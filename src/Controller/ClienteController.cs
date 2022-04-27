using Carter;
using Carter.Request;
using Carter.Response;
using Microsoft.AspNetCore.Http;
using Repository.Contracts;

namespace Controller
{
    public class ClienteController : CarterModule
    {
        public ClienteController(IClienteRepository clienteRepository) : base("API/clientes")
        {
            Get("/listado", async (req, res) =>
            {
                var procesos = await clienteRepository.GetClientes();

                if (procesos != null)
                {
                    res.StatusCode = StatusCodes.Status200OK;
                    await res.AsJson(procesos);
                }
                else
                {
                    res.StatusCode = StatusCodes.Status500InternalServerError;
                }
            });

            Get("/detalle/{id}", async (req, res) =>
            {
                var getClienteRequest = req.RouteValues.As<string>("id");
                var procesos = await clienteRepository.GetClientes(getClienteRequest);

                if (procesos != null && procesos.Count != 0)
                {
                    res.StatusCode = StatusCodes.Status200OK;
                    await res.AsJson(procesos);
                }
                else
                {
                    res.StatusCode = StatusCodes.Status404NotFound;
                    await res.WriteAsync($"No se encontro el proceso: {getClienteRequest}");
                }
            });
        }
    }
}
