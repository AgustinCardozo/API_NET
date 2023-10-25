using System;

namespace API_Demo.Models.Responses
{
    public class DolarRes
    {
        public DateTime fecha { get; set; }
        public double compra { get; set; }
        public double venta { get; set; }
    }


    public class Cotizador
    {
        public Oficial oficial { get; set; }
        public Blue blue { get; set; }
        public OficialEuro oficial_euro { get; set; }
        public BlueEuro blue_euro { get; set; }
        public DateTime last_update { get; set; }
    }

    public class Oficial
    {
        public float value_avg { get; set; }
        public float value_sell { get; set; }
        public float value_buy { get; set; }
    }

    public class Blue
    {
        public float value_avg { get; set; }
        public float value_sell { get; set; }
        public float value_buy { get; set; }
    }

    public class OficialEuro
    {
        public float value_avg { get; set; }
        public float value_sell { get; set; }
        public float value_buy { get; set; }
    }

    public class BlueEuro
    {
        public float value_avg { get; set; }
        public float value_sell { get; set; }
        public float value_buy { get; set; }
    }

}
