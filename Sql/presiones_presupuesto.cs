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
    
    public partial class presiones_presupuesto
    {
        public int id { get; set; }
        public string usuario { get; set; }
        public string justificacion { get; set; }
        public decimal ingresos { get; set; }
        public decimal costos { get; set; }
        public decimal utilidad { get; set; }
        public string objeto { get; set; }
        public string dependencia { get; set; }
        public System.DateTime fecha { get; set; }
        public double vlr_total { get; set; }
        public string estado { get; set; }
        public Nullable<int> vto_bueno_fina { get; set; }
    }
}
