namespace API_Demo.Models.Requests
{
    public class PageReq
    {
        //public int Page { get; set; }
        //public int PageSize { get; set; }

        //public int Offset { get; }
        //public int Next { get; }

        //public PageReq(int page, int pageSize = 10)
        //{
        //    Page = page < 1 ? 1 : page;
        //    PageSize = pageSize < 1 ? 10 : pageSize;

        //    Next = pageSize;
        //    Offset = (Page - 1) * Next;
        //}
        public int pagina { get; set; } //Indica la pag donde se encuentra el usuario, por defecto va a ser 1
        private int registroPorPagina = 10;
        private readonly int cantidadMaximaRegistroPorPagina = 50;

        public int cantidadRegistroPorPagina
        {
            get => registroPorPagina;
            set { registroPorPagina = value > cantidadMaximaRegistroPorPagina ? cantidadMaximaRegistroPorPagina : value; }
        }
    }
}
