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
    
    public partial class entregas
    {
        public int id_ent { get; set; }
        public string ced_empl { get; set; }
        public System.DateTime fecha { get; set; }
        public string hora { get; set; }
        public int id_empresa { get; set; }
        public int cod_user { get; set; }
        public string observacion { get; set; }
        public int autoriza { get; set; }
        public int estado { get; set; }
        public int tipo_acta { get; set; }
    }
}
