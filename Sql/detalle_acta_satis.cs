//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Rhino.Sql
{
    using System;
    using System.Collections.Generic;
    
    public partial class detalle_acta_satis
    {
        public int id { get; set; }
        public int consecutivo { get; set; }
        public int nro_pedido { get; set; }
        public Nullable<int> nro_srp { get; set; }
        public string ceco { get; set; }
        public string cuenta { get; set; }
        public string nro_rp { get; set; }
        public double valor_max { get; set; }
        public double valor { get; set; }
        public double otros { get; set; }
        public double iva { get; set; }
        public double subtotal { get; set; }
        public double saldo_disponible { get; set; }
        public string activo { get; set; }
    }
}
