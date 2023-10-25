namespace API_Demo.Models.Requests
{
    public class PageReq
    {
        public int pagina { get; set; } //Indica la pag donde se encuentra el usuario

        private int registroPorPagina = 10;
        private readonly int cantidadMaximaRegistroPorPagina = 50;

        public int cantidadRegistroPorPagina
        {
            get => registroPorPagina;
            set { registroPorPagina = value > cantidadMaximaRegistroPorPagina ? cantidadMaximaRegistroPorPagina : value; }
        }

        public bool like { get; set; } = true;
        public string error { get; set; } = "";
        public string campo { get; set; } = "";
    }
}
