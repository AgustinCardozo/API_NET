using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace API_Demo.Database.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly DapperContext dapperContext;

        public UsuarioRepository(DapperContext dapperContext)
        {
            this.dapperContext = dapperContext;
        }

        public UsuarioRes GetUsuario(string usuario)
        {
            using (var connection = dapperContext.CreateConnection())
            {
                return connection.QueryFirstOrDefault<UsuarioRes>(
                    sql: "SELECT * FROM GD2015C1.dbo.Usuarios WHERE usuario = @usuario",
                    param: new { usuario = usuario }
                );
            }
        }

        public List<UsuarioRes> GetUsuarios()
        {
            using (var connection = dapperContext.CreateConnection())
            {
                return connection.Query<UsuarioRes>("SELECT * FROM GD2015C1.dbo.Usuarios").ToList();
            }
        }

        public void InsertarUsuario(RegistrarUsuarioReq user)
        {
            //var result = new UsuarioValidator().Validate(user);
            //if (!result.IsValid)
            //{
            //    throw new LogginInvalidoException($"Datos de registros incorrectos: {result.Errors}");
            //}

            using (var connection = dapperContext.CreateConnection())
            {
                connection.ExecuteScalar(
                    sql: "INSERT INTO GD2015C1.dbo.Usuarios (usuario, password, mail, nombre, rol, createdAt) VALUES " +
                        "(@usuario, @password, @mail, @nombre, @rol, @createdAt)",
                    param: new
                    {
                        usuario = user.usuario,
                        password = user.password,
                        mail = user.mail,
                        nombre = user.nombre,
                        rol = user.esAdmin ? Consts.ADMIN : null,
                        createdAt = DateTime.Now
                    }
                );
            }
        }

        public void ModificarPassUsuario(int idUsuario, string password)
        {
            using (var connection = dapperContext.CreateConnection())
            {
                connection.ExecuteScalar(
                    sql: "UPDATE GD2015C1.dbo.Usuarios SET password = @password WHERE id = @id",
                    param: new { id = idUsuario, password = password }
                );
            }
        }
    }
}
