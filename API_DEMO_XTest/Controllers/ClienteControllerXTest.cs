using API_Demo.Controllers;
using API_Demo.Models.Requests;
using API_DEMO_XTest.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Demo_XTest.Controllers;

public class ClienteControllerXTest
{
    private readonly ClienteController clienteController;
    private readonly ServiceProviderHelper serviceHelper;

    public ClienteControllerXTest()
    {
        serviceHelper = new();
        clienteController = serviceHelper.GetRequiredService<ClienteController>();
    }

    [Fact]
    public async Task GetClientes_Test()
    {

        var response = (ObjectResult)await clienteController.GetClientes();
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

        var response = (StatusCodeResult)await clienteController.UpdateCliente(req);
        Assert.True(response.StatusCode == StatusCodes.Status500InternalServerError);
    }

    [Theory]
    [InlineData("00000")]
    [InlineData("000001234")]
    public async Task GetCliente_Test(string id)
    {
        var response = (ObjectResult)await clienteController.GetCliente(id);
        Assert.NotNull(response);
        if (response.StatusCode == StatusCodes.Status404NotFound)
        {
            Assert.Equal(StatusCodes.Status404NotFound, response.StatusCode);
            return;
        }
        Assert.True(response.StatusCode == StatusCodes.Status200OK);
    }
}