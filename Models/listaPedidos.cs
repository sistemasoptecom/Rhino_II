using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rhino.Models
{
    public class listaPedidos
    {
        public int nro_pedido { get; set; }
        public string codigo_art { get; set; }
        public string descripcion { get; set; }
        public Double total { get; set; }
        public int tiempo { get; set; }
        public Double TotalAmortizable { get; set; }
        public string nro_factura { get; set; }
        public string ceco { get; set; }
        public string cuenta { get; set; }
        public string rubro { get; set; }

    }
}