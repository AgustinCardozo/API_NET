﻿using API_Demo.Models.Requests;

namespace API_Demo.Services.Contracts
{
    public interface ILogginService
    {
        string RegistrarUsuario(RegistrarUsuarioReq usuario);
        string IniciarSeccion(UsuarioLogginReq usuario);
    }
}