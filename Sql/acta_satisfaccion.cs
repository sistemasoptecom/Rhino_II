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
    
    public partial class acta_satisfaccion
    {
        public int id { get; set; }
        public System.DateTime fecha { get; set; }
        public string tipo_acta { get; set; }
        public string tipo_comp { get; set; }
        public string cual_compromiso { get; set; }
        public string nit_proveedor { get; set; }
        public string nombre_proveedor { get; set; }
        public string nro_oden { get; set; }
        public string objeto_beneficio { get; set; }
        public Nullable<System.DateTime> fecha_inicio { get; set; }
        public Nullable<System.DateTime> fecha_terminacion { get; set; }
        public string valor_orden { get; set; }
        public string valor_ejecutado { get; set; }
        public string saldo_disponible { get; set; }
        public string valor_letras { get; set; }
        public string nro_factura { get; set; }
        public string subtotal { get; set; }
        public string iva { get; set; }
        public string valor_total { get; set; }
        public string elaborado_por { get; set; }
        public string dependencia { get; set; }
        public string autoriza { get; set; }
        public string cargo { get; set; }
        public string observacion { get; set; }
        public string aprobado { get; set; }
        public string aprobado_por { get; set; }
        public string ruta_firma { get; set; }
        public string asignado_a { get; set; }
        public Nullable<int> consecutivo { get; set; }
    }
}
