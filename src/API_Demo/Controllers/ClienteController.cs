using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SpreadsheetLight;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
        [Route("{id}"), Authorize(Roles = Consts.ADMIN)]
        public async Task<IActionResult> DeleteCliente(string id)
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

        [AllowAnonymous]
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetClientes()
        {
            logger.LogInformation("Listado de clientes");
            var clientes = await clienteRepository.GetClientes();
            logger.LogInformation($"Status Code: {StatusCodes.Status200OK}");
            //ClienteHelper.QuitarEspacio(clientes);
            return Ok(clientes);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("csv")]
        [RequestSizeLimit(100_000_000)]
        public async Task<IActionResult> GetClientesCSV()
        {
            var clientes = await clienteRepository.GetClientes();
            //ClienteHelper.QuitarEspacio(clientes);

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

        [AllowAnonymous]
        [HttpGet]
        [Route("xlsx")]
        [RequestSizeLimit(100_000_000)]
        public async Task<IActionResult> GetClientesXls()
        {
            var clientes = await clienteRepository.GetClientes();
            //ClienteHelper.QuitarEspacio(clientes);
            //var pathFile = AppDomain.CurrentDomain.BaseDirectory + "test.xlsx"; //agrega el archivo el proyecto -> bin -> debug 
            //var pathFile = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/test.xlsx";
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
            //document.SaveAs(pathFile);
            using (var stream = new MemoryStream())
            {
                document.SaveAs(stream);
                var content = stream.ToArray();
                return File(content, "application/xlsx", "clientes.xlsx"); //application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            }
            //return Ok();
        }

        [AllowAnonymous]
        [HttpGet("read-xlsx")]
        [RequestSizeLimit(100_000_000)]
        public IActionResult GetClientesFromXLS()
        {
            int indice = 2; //Si es fuera 1, va a leer la 1er fila
            var clientes = new List<ClienteRes>();

            var file = new FileInfo("Assets/clientes.xlsx");
            var document = new SLDocument(file.FullName);

            while (!string.IsNullOrEmpty(document.GetCellValueAsString(indice, 1)))
            {
                var cliente = new ClienteRes
                {
                    clie_codigo = document.GetCellValueAsString(indice, 1),
                    clie_razon_social = document.GetCellValueAsString(indice, 2),
                    clie_telefono = document.GetCellValueAsString(indice, 3),
                    clie_domicilio = document.GetCellValueAsString(indice, 4),
                    clie_limite_credito = (float)document.GetCellValueAsDouble(indice, 5),
                    clie_vendedor = document.GetCellValueAsInt32(indice, 6)
                };

                clientes.Add(cliente);
                indice++;
            }
            return Ok(clientes);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetClientes(string id)
        {
            logger.LogInformation($"Se consulta el cliente con ID: {id}");
            var clientes = await clienteRepository.GetClientes(id);
            //ClienteHelper.QuitarEspacio(clientes);

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
        [Route("")]
        public async Task<IActionResult> CreateCliente([FromBody] ClienteReq clienteReq)
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
        [Route("")]
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
