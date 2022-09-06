﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Objects;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    
    public partial class rhinoEntities : DbContext
    {
        public rhinoEntities()
            : base("name=rhinoEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<acta_satisfaccion> acta_satisfaccion { get; set; }
        public DbSet<acta_satisfaccion_nueva> acta_satisfaccion_nueva { get; set; }
        public DbSet<area_ccosto> area_ccosto { get; set; }
        public DbSet<articulos> articulos { get; set; }
        public DbSet<articulos_af> articulos_af { get; set; }
        public DbSet<bodegasst> bodegasst { get; set; }
        public DbSet<como_dato> como_dato { get; set; }
        public DbSet<compras_articulos> compras_articulos { get; set; }
        public DbSet<consecutivos> consecutivos { get; set; }
        public DbSet<cuentas> cuentas { get; set; }
        public DbSet<depreciacion> depreciacion { get; set; }
        public DbSet<depreciacion_2021> depreciacion_2021 { get; set; }
        public DbSet<depreciacion_2022> depreciacion_2022 { get; set; }
        public DbSet<descuentos> descuentos { get; set; }
        public DbSet<detalle_act_sat> detalle_act_sat { get; set; }
        public DbSet<detalle_acta_satis> detalle_acta_satis { get; set; }
        public DbSet<detalle_depre> detalle_depre { get; set; }
        public DbSet<detalle_devolucion> detalle_devolucion { get; set; }
        public DbSet<detalle_entrega> detalle_entrega { get; set; }
        public DbSet<detalle_pedido> detalle_pedido { get; set; }
        public DbSet<detalle_presiones> detalle_presiones { get; set; }
        public DbSet<detalle_rp> detalle_rp { get; set; }
        public DbSet<detalle_traslados> detalle_traslados { get; set; }
        public DbSet<detalle_usu_alt> detalle_usu_alt { get; set; }
        public DbSet<devoluciones> devoluciones { get; set; }
        public DbSet<directivos> directivos { get; set; }
        public DbSet<empleado> empleado { get; set; }
        public DbSet<empresa> empresa { get; set; }
        public DbSet<entregas> entregas { get; set; }
        public DbSet<grupos> grupos { get; set; }
        public DbSet<histo_ajuste_presu> histo_ajuste_presu { get; set; }
        public DbSet<hv_altura> hv_altura { get; set; }
        public DbSet<insp_equi_altura> insp_equi_altura { get; set; }
        public DbSet<inspec_escalera> inspec_escalera { get; set; }
        public DbSet<iva> iva { get; set; }
        public DbSet<list_rp> list_rp { get; set; }
        public DbSet<log_user> log_user { get; set; }
        public DbSet<log_usuario> log_usuario { get; set; }
        public DbSet<objeto> objeto { get; set; }
        public DbSet<pedidos> pedidos { get; set; }
        public DbSet<permisos_usu> permisos_usu { get; set; }
        public DbSet<planillas_nomina> planillas_nomina { get; set; }
        public DbSet<presiones_presupuesto> presiones_presupuesto { get; set; }
        public DbSet<presupuesto> presupuesto { get; set; }
        public DbSet<proveedor> proveedor { get; set; }
        public DbSet<srp> srp { get; set; }
        public DbSet<sysdiagrams> sysdiagrams { get; set; }
        public DbSet<temp_detalle_presiones> temp_detalle_presiones { get; set; }
        public DbSet<temp_presiones_presupuesto> temp_presiones_presupuesto { get; set; }
        public DbSet<tipo_usuario> tipo_usuario { get; set; }
        public DbSet<traslados_presupuesto> traslados_presupuesto { get; set; }
        public DbSet<usuario> usuario { get; set; }
        public DbSet<views> views { get; set; }
        public DbSet<depreciacion_2022old> depreciacion_2022old { get; set; }
        public DbSet<jefes> jefes { get; set; }
        public DbSet<pedidos_tiempo_diff> pedidos_tiempo_diff { get; set; }
        public DbSet<amortizacion> amortizacion { get; set; }
        public DbSet<detalle_pedido_diff_acta> detalle_pedido_diff_acta { get; set; }
    
        public virtual ObjectResult<string> Depreciar_af()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("Depreciar_af");
        }
    
        public virtual int INSERT_DEPRECIACION(string nro_ped, Nullable<int> cantidadPed, string codi_articulo)
        {
            var nro_pedParameter = nro_ped != null ?
                new ObjectParameter("nro_ped", nro_ped) :
                new ObjectParameter("nro_ped", typeof(string));
    
            var cantidadPedParameter = cantidadPed.HasValue ?
                new ObjectParameter("cantidadPed", cantidadPed) :
                new ObjectParameter("cantidadPed", typeof(int));
    
            var codi_articuloParameter = codi_articulo != null ?
                new ObjectParameter("codi_articulo", codi_articulo) :
                new ObjectParameter("codi_articulo", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("INSERT_DEPRECIACION", nro_pedParameter, cantidadPedParameter, codi_articuloParameter);
        }
    
        public virtual int proximos_vencer()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("proximos_vencer");
        }
    
        public virtual int sp_alterdiagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_alterdiagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_creatediagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_creatediagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_dropdiagram(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_dropdiagram", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagramdefinition_Result> sp_helpdiagramdefinition(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagramdefinition_Result>("sp_helpdiagramdefinition", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagrams_Result> sp_helpdiagrams(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagrams_Result>("sp_helpdiagrams", diagramnameParameter, owner_idParameter);
        }
    
        public virtual int sp_renamediagram(string diagramname, Nullable<int> owner_id, string new_diagramname)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var new_diagramnameParameter = new_diagramname != null ?
                new ObjectParameter("new_diagramname", new_diagramname) :
                new ObjectParameter("new_diagramname", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_renamediagram", diagramnameParameter, owner_idParameter, new_diagramnameParameter);
        }
    
        public virtual int sp_upgraddiagrams()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_upgraddiagrams");
        }
    
        public virtual int validate_presupuesto(string nro_ped)
        {
            var nro_pedParameter = nro_ped != null ?
                new ObjectParameter("nro_ped", nro_ped) :
                new ObjectParameter("nro_ped", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("validate_presupuesto", nro_pedParameter);
        }
    
        public virtual int validate_presupuesto_AF(string nro_ped)
        {
            var nro_pedParameter = nro_ped != null ?
                new ObjectParameter("nro_ped", nro_ped) :
                new ObjectParameter("nro_ped", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("validate_presupuesto_AF", nro_pedParameter);
        }
    
        public virtual int validate_presupuesto_AFold(string nro_ped)
        {
            var nro_pedParameter = nro_ped != null ?
                new ObjectParameter("nro_ped", nro_ped) :
                new ObjectParameter("nro_ped", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("validate_presupuesto_AFold", nro_pedParameter);
        }
    
        public virtual int validate_presupuesto_diff(string nro_ped)
        {
            var nro_pedParameter = nro_ped != null ?
                new ObjectParameter("nro_ped", nro_ped) :
                new ObjectParameter("nro_ped", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("validate_presupuesto_diff", nro_pedParameter);
        }
    
        public virtual int INSERT_AMORTIZACION(string nro_ped, Nullable<int> cantidadPed, string codi_articulo)
        {
            var nro_pedParameter = nro_ped != null ?
                new ObjectParameter("nro_ped", nro_ped) :
                new ObjectParameter("nro_ped", typeof(string));
    
            var cantidadPedParameter = cantidadPed.HasValue ?
                new ObjectParameter("cantidadPed", cantidadPed) :
                new ObjectParameter("cantidadPed", typeof(int));
    
            var codi_articuloParameter = codi_articulo != null ?
                new ObjectParameter("codi_articulo", codi_articulo) :
                new ObjectParameter("codi_articulo", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("INSERT_AMORTIZACION", nro_pedParameter, cantidadPedParameter, codi_articuloParameter);
        }
    }
}