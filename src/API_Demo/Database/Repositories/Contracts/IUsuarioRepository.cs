﻿using API_Demo.Models.Requests;
using API_Demo.Models.Responses;
using System.Collections.Generic;

namespace API_Demo.Database.Repositories.Contracts
{
    public interface IUsuarioRepository
    {
        void InsertarUsuario(RegistrarUsuarioReq user);
        List<UsuarioRes> GetUsuarios();
        UsuarioRes GetUsuario(string usuario);
    }
}