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
    
    public partial class detalle_depreciacion
    {
        public int id_auto { get; set; }
        public int id_dep_art { get; set; }
        public string descripcion { get; set; }
        public int periodo { get; set; }
        public string cuenta { get; set; }
        public int deprec_acumulada { get; set; }
        public int valor_libros { get; set; }
        public string ejecutado { get; set; }
    }
}
