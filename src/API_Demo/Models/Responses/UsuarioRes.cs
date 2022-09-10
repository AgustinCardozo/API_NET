using System;

namespace API_Demo.Models.Responses
{
    public class UsuarioRes
    {
        public int id { get; set; }
        public string usuario { get; set; }
        public string password { get; set; }
        public string mail { get; set; }
        public string nombre { get; set; }
        public string rol { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime? updatedAt { get; set; }
        public DateTime? deletedAt { get; set; }
    }
}
