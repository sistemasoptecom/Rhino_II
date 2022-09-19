using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rhino.Models
{
    public class listarPedidoAmortizacion
    {
        public int id_pedido { get; set; }
        public string cod_art { get; set; }
        public string descripcion { get; set; }
        public Decimal valorSrp { get; set; }
        public Decimal valorFactura { get; set; }
        public int v_util { get; set; }
        public int cuota { get; set; }
        public string NumeroFac { get; set; }
        public string ccosto { get; set; }
        public string cuenta { get; set; }
        public string rubro { get; set; }
        public int cantidadpedido { get; set; }
        public int Amortizar { get; set; }
        public int pedidoIngreso { get; set; }
    }
}