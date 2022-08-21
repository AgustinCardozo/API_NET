using API_Demo.Helpers;
using API_Demo.Models.Requests;
using API_Demo.Repositories.Contracts;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SpreadsheetLight;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace API_Demo.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/clientes")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteRepository clienteRepository;
        private readonly ILogger<ClienteController> logger;
        private readonly IValidator<ClienteReq> validator;

        public ClienteController(IClienteRepository clienteRepository, ILogger<ClienteController> logger, IValidator<ClienteReq> validator)
        {
            this.clienteRepository = clienteRepository;
            this.logger = logger;
            this.validator = validator;
        }

        [HttpDelete]
        [Route("{id}/delete")]
        public async Task<IActionResult> DeleteCliente([Required]string id)
        {
            logger.LogInformation($"Se borra el cliente con ID: {id}");
            var clienteMsg = await clienteRepository.DeleteCliente(id);
            if (ClienteHelper.EsValidoMensaje(clienteMsg))
            {
                logger.LogInformation($"StatusCode: {StatusCodes.Status200OK}");
                return Ok($"Se borro correctamente al usuario con id {id}");
            }
            else
            {
                logger.LogError($"StatusCode: {StatusCodes.Status404NotFound}");
                return NotFound($"No se encontro al usuario con id {id}");
            }
        }
 
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetClientes()
        {
            logger.LogInformation("Listado de clientes");
            var clientes = await clienteRepository.GetClientes();
            logger.LogInformation($"Status Code: {StatusCodes.Status200OK}");
            ClienteHelper.QuitarEspacio(clientes);
            return Ok(clientes);
        }

        [HttpGet]
        [Route("csv")]
        [RequestSizeLimit(100_000_000)]
        public async Task<IActionResult> GetClientesCSV()
        {
            var clientes = await clienteRepository.GetClientes();
            ClienteHelper.QuitarEspacio(clientes);

            var csv = new StringBuilder();
            csv.AppendLine("ID;RAZON SOCIAL;TELEFONO;TELEFONO;LIMITE DE CRED;ID VENDEDOR");

            foreach (var item in clientes)
            {
                csv.AppendLine($"{item.clie_codigo}; {item.clie_razon_social}; {item.clie_telefono};" +
                    $"{item.clie_domicilio};{item.clie_limite_credito};{item.clie_vendedor}");
            }
            var content = Encoding.ASCII.GetBytes(csv.ToString());
            return File(content, "clientes/csv", "clientes.csv");
        }

        [HttpGet]
        [Route("xlsx")]
        //agrega el archivo el proyecto -> bin -> debug 
        public async Task<IActionResult> GetClientesXlsl()
        {
            var clientes = await clienteRepository.GetClientes();
            ClienteHelper.QuitarEspacio(clientes);
            var pathFile = AppDomain.CurrentDomain.BaseDirectory + "test.xlsx";
            var document = new SLDocument();
            var table = new DataTable();

            table.Columns.Add("ID", typeof(string));
            table.Columns.Add("RAZON SOCIAL", typeof(string));
            table.Columns.Add("TELEFONO", typeof(string));
            table.Columns.Add("LIMITE DE CRED", typeof(double));
            table.Columns.Add("ID VENDEDOR", typeof(int));

            foreach (var item in clientes)
            {
                table.Rows.Add(item.clie_codigo, item.clie_razon_social, item.clie_telefono, item.clie_limite_credito, item.clie_vendedor);
            }

            document.ImportDataTable(1, 1, table, true);
            document.SaveAs(pathFile);
            return Ok();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetClientes(string id)
        {
            logger.LogInformation($"Se consulta el cliente con ID: {id}");
            var clientes = await clienteRepository.GetClientes(id);
            ClienteHelper.QuitarEspacio(clientes);

            if (clientes != null && clientes.Count != 0)
            {
                logger.LogInformation($"StatusCode: {StatusCodes.Status200OK}");
                return Ok(clientes[0]);
            }
            else
            {
                logger.LogInformation($"StatusCode: {StatusCodes.Status404NotFound}");
                return NotFound($"No se encontro el cliente: {id}");
            }
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateCliente([FromBody]ClienteReq clienteReq)
        {
            logger.LogInformation("Se crear un nuevo cliente");
            var clienteMsg = await clienteRepository.SetCliente(clienteReq, "create");

            if (ClienteHelper.EsValidoMensaje(clienteMsg))
            {
                logger.LogInformation($"StatusCode: {StatusCodes.Status200OK}");
                return Ok($"El cliente {clienteMsg} se creo con exito");
            }
            else
            {
                logger.LogWarning($"StatusCode: {StatusCodes.Status400BadRequest}");
                return BadRequest($"No se pudo crear un nuevo cliente porque ya existe {clienteReq.idCliente}");
            }
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> UpdateCliente([FromBody] ClienteReq clienteReq)
        {
            var result = await validator.ValidateAsync(clienteReq);
            if (result.IsValid)
            {
                logger.LogInformation($"Se modifica el cliente con ID: {clienteReq.idCliente}");
                var clienteMsg = await clienteRepository.SetCliente(clienteReq, "update");

                if (ClienteHelper.EsValidoMensaje(clienteMsg))
                {
                    logger.LogInformation($"StatusCode: {StatusCodes.Status200OK}");
                    return Ok($"El cliente {clienteReq.idCliente} se modifico con exito");
                }
                else
                {
                    logger.LogWarning($"StatusCode: {StatusCodes.Status400BadRequest}");
                    return BadRequest($"No se pudo modificar el cliente{clienteReq.idCliente}");
                }
            }
            else
            {
                logger.LogError(result.Errors[0].ToString());
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
