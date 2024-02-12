using API_Demo.Controllers;
using API_Demo.Models.Requests;
using API_Demo_XTest.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Demo_XTest.Controllers;

public class ClienteControllerXTest : CommonHelper<ClienteController>
{
    [Fact]
    public async Task GetClientes_Test()
    {

        var response = (ObjectResult)await service.GetClientes();
        Assert.NotNull(response);
        Assert.True(response.StatusCode == StatusCodes.Status200OK);
    }

    [Fact]
    public async Task UpdateClienteInvalido_Test()
    {
        var req = new ClienteReq
        {
            domicilio = "Darwin 747"
        };

        var response = (StatusCodeResult)await service.UpdateCliente(req);
        Assert.True(response.StatusCode == StatusCodes.Status500InternalServerError);
    }

    [Theory]
    [InlineData("00000")]
    [InlineData("000001234")]
    public async Task GetCliente_Test(string id)
    {
        var response = (ObjectResult)await service.GetCliente(id);
        Assert.NotNull(response);
        if (response.StatusCode == StatusCodes.Status404NotFound)
        {
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
            return;
        }
        Assert.True(response.StatusCode == StatusCodes.Status200OK);
    }
}