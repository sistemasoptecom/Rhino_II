using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Rhino.Service
{
    [ServiceContract]
    public interface IServicios
    {

        #region Rhino
        //#region CLIENTES    
        //[OperationContract]
        //[WebInvoke(
        //Method = "POST",
        //UriTemplate = "/CreateClientes",
        //ResponseFormat = WebMessageFormat.Json,
        //RequestFormat = WebMessageFormat.Json,
        //BodyStyle = WebMessageBodyStyle.WrappedRequest
        //)]
        //string CreateClientes(string json);

        //[OperationContract]
        //[WebInvoke(
        //Method = "POST",
        //UriTemplate = "/UpdateClientes",
        //ResponseFormat = WebMessageFormat.Json,
        //RequestFormat = WebMessageFormat.Json,
        //BodyStyle = WebMessageBodyStyle.WrappedRequest
        //)]
        //string UpdateClientes(string json);

        //[OperationContract]
        //[WebInvoke(
        //Method = "POST",
        //UriTemplate = "/ReadClientes",
        //ResponseFormat = WebMessageFormat.Json,
        //RequestFormat = WebMessageFormat.Json,
        //BodyStyle = WebMessageBodyStyle.WrappedRequest
        //)]
        //string ReadClientes(string json);

        //[OperationContract]
        //[WebInvoke(
        //Method = "POST",
        //UriTemplate = "/DeleteClientes",
        //ResponseFormat = WebMessageFormat.Json,
        //RequestFormat = WebMessageFormat.Json,
        //BodyStyle = WebMessageBodyStyle.WrappedRequest
        //)]
        //string DeleteClientes(string json);


        //[OperationContract]
        //[WebInvoke(
        //Method = "POST",
        //UriTemplate = "/ValidateClientes",
        //ResponseFormat = WebMessageFormat.Json,
        //RequestFormat = WebMessageFormat.Json,
        //BodyStyle = WebMessageBodyStyle.WrappedRequest
        //)]
        //string ValidateClientes(string json);
        //#endregion // SE ME DESCARGO EL TELEFONO Y NO ME DI CUENTA XD. bueno si puedes nos conectamos mas ahorita | No, Espera un momento y ya


        [OperationContract]
        [WebInvoke(
             Method = "POST",
             UriTemplate = "/ReporteDetallePedido",
             ResponseFormat = WebMessageFormat.Json,
             RequestFormat = WebMessageFormat.Json,
             BodyStyle = WebMessageBodyStyle.WrappedRequest
             )]
        string ReporteDetallePedido();


        [OperationContract]
        [WebInvoke(
       Method = "POST",
       UriTemplate = "/PreloadViews",
       ResponseFormat = WebMessageFormat.Json,
       RequestFormat = WebMessageFormat.Json,
       BodyStyle = WebMessageBodyStyle.WrappedRequest
       )]
        string PreloadViews(string json);

        #region USUARIOS

        [OperationContract]
        [WebInvoke(
           Method = "POST",
           UriTemplate = "/PreloadUsuario",
           ResponseFormat = WebMessageFormat.Json,
           RequestFormat = WebMessageFormat.Json,
           BodyStyle = WebMessageBodyStyle.WrappedRequest
           )]
        string PreloadUsuario(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/CreateUsuarios",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string CreateUsuarios(string json);
        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/ValidateUsername",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string ValidateUsername(string json);

        [OperationContract]
         [WebInvoke(
           Method = "POST",
           UriTemplate = "/ReadUsuario",
           ResponseFormat = WebMessageFormat.Json,
           RequestFormat = WebMessageFormat.Json,
           BodyStyle = WebMessageBodyStyle.WrappedRequest
           )]
        string ReadUsuario(string json);
        [OperationContract]
        [WebInvoke(
           Method = "POST",
           UriTemplate = "/UpdateUsuario",
           ResponseFormat = WebMessageFormat.Json,
           RequestFormat = WebMessageFormat.Json,
           BodyStyle = WebMessageBodyStyle.WrappedRequest
           )]
        string UpdateUsuario(string json);
        #endregion

        //#region PRESTAMOS

        //[OperationContract]
        //[WebInvoke(
        //   Method = "POST",
        //   UriTemplate = "/PreloadPrestamo",
        //   ResponseFormat = WebMessageFormat.Json,
        //   RequestFormat = WebMessageFormat.Json,
        //   BodyStyle = WebMessageBodyStyle.WrappedRequest
        //   )]
        //string PreloadPrestamo();

        //[OperationContract]
        //[WebInvoke(
        //Method = "POST",
        //UriTemplate = "/CreatePrestamos",
        //ResponseFormat = WebMessageFormat.Json,
        //RequestFormat = WebMessageFormat.Json,
        //BodyStyle = WebMessageBodyStyle.WrappedRequest
        //)]
        //string CreatePrestamos(string json);

        //[OperationContract]
        //[WebInvoke(
        //Method = "POST",
        //UriTemplate = "/ReadPrestamos",
        //ResponseFormat = WebMessageFormat.Json,
        //RequestFormat = WebMessageFormat.Json,
        //BodyStyle = WebMessageBodyStyle.WrappedRequest
        //)]
        //string ReadPrestamos(string json);

        //[OperationContract]
        //[WebInvoke(
        //Method = "POST",
        //UriTemplate = "/ReadDatosPrestamo",
        //ResponseFormat = WebMessageFormat.Json,
        //RequestFormat = WebMessageFormat.Json,
        //BodyStyle = WebMessageBodyStyle.WrappedRequest
        //)]
        //string ReadDatosPrestamo(string json);
        //#endregion

        //#region PAGOS
        //[OperationContract]
        //[WebInvoke(
        //Method = "POST",
        //UriTemplate = "/CreatePagos",
        //ResponseFormat = WebMessageFormat.Json,
        //RequestFormat = WebMessageFormat.Json,
        //BodyStyle = WebMessageBodyStyle.WrappedRequest
        //)]
        //string CreatePagos(string json);

        //[OperationContract]
        //[WebInvoke(
        //Method = "POST",
        //UriTemplate = "/ReadPagos",
        //ResponseFormat = WebMessageFormat.Json,
        //RequestFormat = WebMessageFormat.Json,
        //BodyStyle = WebMessageBodyStyle.WrappedRequest
        //)]
        //string ReadPagos(string json);

        //[OperationContract]
        //[WebInvoke(
        //Method = "POST",
        //UriTemplate = "/ReadClientePrestamo",
        //ResponseFormat = WebMessageFormat.Json,
        //RequestFormat = WebMessageFormat.Json,
        //BodyStyle = WebMessageBodyStyle.WrappedRequest
        //)]
        //string ReadClientePrestamo(string json);

        //[OperationContract]
        //[WebInvoke(
        //Method = "POST",
        //UriTemplate = "/ReadTablaPagos",
        //ResponseFormat = WebMessageFormat.Json,
        //RequestFormat = WebMessageFormat.Json,
        //BodyStyle = WebMessageBodyStyle.WrappedRequest
        //)]
        //string ReadTablaPagos(string json);

        //[OperationContract]
        //[WebInvoke(
        //Method = "POST",
        //UriTemplate = "/CreatePagosRuta",
        //ResponseFormat = WebMessageFormat.Json,
        //RequestFormat = WebMessageFormat.Json,
        //BodyStyle = WebMessageBodyStyle.WrappedRequest
        //)]
        //string CreatePagosRuta(string json);

        //#endregion

        //#region RUTA
        //[OperationContract]
        //[WebInvoke(
        //    Method = "POST",
        //    UriTemplate = "/ReadRuta",
        //    ResponseFormat = WebMessageFormat.Json,
        //    RequestFormat = WebMessageFormat.Json,
        //    BodyStyle = WebMessageBodyStyle.WrappedRequest
        //    )]
        //string ReadRuta(string json);

        //[OperationContract]
        //[WebInvoke(
        //    Method = "POST",
        //    UriTemplate = "/ValidarFecha",
        //    ResponseFormat = WebMessageFormat.Json,
        //    RequestFormat = WebMessageFormat.Json,
        //    BodyStyle = WebMessageBodyStyle.WrappedRequest
        //    )]
        //string ValidarFecha(string json);

        //[OperationContract]
        //[WebInvoke(
        //    Method = "POST",
        //    UriTemplate = "/UpdateRuta",
        //    ResponseFormat = WebMessageFormat.Json,
        //    RequestFormat = WebMessageFormat.Json,
        //    BodyStyle = WebMessageBodyStyle.WrappedRequest
        //    )]
        //string UpdateRuta(string json);

        //[OperationContract]
        //[WebInvoke(
        //Method = "POST",
        //UriTemplate = "/CreateCierreDia",
        //ResponseFormat = WebMessageFormat.Json,
        //RequestFormat = WebMessageFormat.Json,
        //BodyStyle = WebMessageBodyStyle.WrappedRequest
        //)]
        //string CreateCierreDia(string json);

        //[OperationContract]
        //[WebInvoke(
        //Method = "POST",
        //UriTemplate = "/ReadRutaAnt",
        //ResponseFormat = WebMessageFormat.Json,
        //RequestFormat = WebMessageFormat.Json,
        //BodyStyle = WebMessageBodyStyle.WrappedRequest
        //)]
        //string ReadRutaAnt(string json);

        //[OperationContract]
        //[WebInvoke(
        //    Method = "POST",
        //    UriTemplate = "/ReadGastosDia",
        //    ResponseFormat = WebMessageFormat.Json,
        //    RequestFormat = WebMessageFormat.Json,
        //    BodyStyle = WebMessageBodyStyle.WrappedRequest
        //    )]
        //string ReadGastosDia(string json);

        //[OperationContract]
        //[WebInvoke(
        //Method = "POST",
        //UriTemplate = "/Read_Rel_Gastos",
        //ResponseFormat = WebMessageFormat.Json,
        //RequestFormat = WebMessageFormat.Json,
        //BodyStyle = WebMessageBodyStyle.WrappedRequest
        //)]
        //string Read_Rel_Gastos(string json);
        //#endregion

        //#region ESTADO CUENTA
        //[OperationContract]
        //[WebInvoke(
        //    Method = "POST",
        //    UriTemplate = "/ReadTablaPrestamo",
        //    ResponseFormat = WebMessageFormat.Json,
        //    RequestFormat = WebMessageFormat.Json,
        //    BodyStyle = WebMessageBodyStyle.WrappedRequest
        //    )]
        //string ReadTablaPrestamo(string json);
        //#endregion

        #region EMPLEADOS    
        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/CreateEmpleados",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string CreateEmpleados(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/UpdateEmpleados",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string UpdateEmpleados(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/ReadEmpleados",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string ReadEmpleados(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/PreloadEmpresa",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string PreloadEmpresa();

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/DeleteEmpleados",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string DeleteEmpleados(string json);


        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/ValidateEmpleados",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string ValidateEmpleados(string json);


        #endregion

        #region ACTIVOS FIJOS    
        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/CreateActivos",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string CreateActivos(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/UpdateActivos",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string UpdateActivos(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/ReadActivos",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string ReadActivos(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/DeleteActivos",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string DeleteActivos(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/Validateimei",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string Validateimei(string json);



        #endregion

        #region ENTREGAS
        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/create_entrega",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string create_entrega(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/update_entrega",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string update_entrega(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/ReadEntregas",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string ReadEntregas(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/Read_Acta",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string Read_Acta(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/Searchreport1",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string Searchreport1(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/ReadObjeto",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string ReadObjeto(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/ReadConsumibles",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string ReadConsumibles(string json);


        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/ReporteConsumibles",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string ReporteConsumibles(string json);
        #endregion

        #region
        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/create_acta",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string create_acta(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/update_consumibles",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string update_consumibles(string json);
        #endregion

        #region
        [OperationContract]
            [WebInvoke(
            Method = "POST",
            UriTemplate = "/ReadActa_entrega",
            ResponseFormat = WebMessageFormat.Json,
            RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest
            )]
        string ReadActa_entrega(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/ReadActa_devolucion",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string ReadActa_devolucion(string json);
        #endregion

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/preload_jefes",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string preload_jefes();

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/preload_ccosto",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string preload_ccosto();


        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/readempresa",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
            string readempresa(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/validatejefe",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string validatejefe(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/consulta",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string consulta(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/CreateInspeccionaltura",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string CreateInspeccionaltura(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/ReadIspeccionAlt",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string ReadIspeccionAlt(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/Report_Escaleras",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string Report_Escaleras(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/CreateEquipo",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string CreateEquipo(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/Create_HV_Altura",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string Create_HV_Altura(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/Report_bodegasst",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string Report_bodegasst(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/Report_bodegasstfordate",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string Report_bodegasstfordate(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/readequipoalt",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string readequipoalt(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/validatehv",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string validatehv(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/CreateInspeccionescalera",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string CreateInspeccionescalera(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/Readinspec_esacaleras",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string Readinspec_esacaleras(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/UpdateHv_equipos",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string UpdateHv_equipos(string json);


        #region COMPRAS
        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/PreloadArticulos",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string PreloadArticulos();

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/CreateArticulos",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string CreateArticulos(string json);


        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/ValidatePresupuesto",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string ValidatePresupuesto(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/CreatePresupuesto",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string CreatePresupuesto(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/PreloadPresupuesto",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string PreloadPresupuesto();
        [OperationContract]
        [WebInvoke(
       Method = "POST",
       UriTemplate = "/AjustarPresupuesto",
       ResponseFormat = WebMessageFormat.Json,
       RequestFormat = WebMessageFormat.Json,
       BodyStyle = WebMessageBodyStyle.WrappedRequest
       )]
        string AjustarPresupuesto(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
       UriTemplate = "/TrasladoPresupuesto",
       ResponseFormat = WebMessageFormat.Json,
       RequestFormat = WebMessageFormat.Json,
       BodyStyle = WebMessageBodyStyle.WrappedRequest
       )]
        string TrasladoPresupuesto(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/buscar_movimientos_pre",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string buscar_movimientos_pre(string json);

        [OperationContract]
        [WebInvoke(
           Method = "POST",
           UriTemplate = "/CreatePedidos",
           ResponseFormat = WebMessageFormat.Json,
           RequestFormat = WebMessageFormat.Json,
           BodyStyle = WebMessageBodyStyle.WrappedRequest
       )]

        string CreatePedidos(string json);


        [OperationContract]
        [WebInvoke(
           Method = "POST",
           UriTemplate = "/ListarArticulosArmortizables",
           ResponseFormat = WebMessageFormat.Json,
           RequestFormat = WebMessageFormat.Json,
           BodyStyle = WebMessageBodyStyle.WrappedRequest
       )]

        string ListarArticulosArmortizables();

        [OperationContract]
        [WebInvoke(
           Method = "POST",
           UriTemplate = "/CreatePedidos_Diff",
           ResponseFormat = WebMessageFormat.Json,
           RequestFormat = WebMessageFormat.Json,
           BodyStyle = WebMessageBodyStyle.WrappedRequest
       )]

        string CreatePedidos_Diff(string json);

        [OperationContract]
        [WebInvoke(
           Method = "POST",
           UriTemplate = "/CreatePedidos_AF",
           ResponseFormat = WebMessageFormat.Json,
           RequestFormat = WebMessageFormat.Json,
           BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]

        string CreatePedidos_AF(string json);

        [OperationContract]
        [WebInvoke(
           Method = "POST",
           UriTemplate = "/ReadPedido",
           ResponseFormat = WebMessageFormat.Json,
           RequestFormat = WebMessageFormat.Json,
           BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]

        string ReadPedido(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/PreloadNro_pedido",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]

        string PreloadNro_pedido();

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/ReadPresion",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]

        string ReadPresion(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/ReadTraslados",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string ReadTraslados(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/ReportePedidos",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string ReportePedidos(string json);
                
        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/ReportePedidos_finan",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string ReportePedidos_finan();

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/ReportePresion",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string ReportePresion();


        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/ReporteTraslado",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string ReporteTraslado();

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/TrasladosEjecutados",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string TrasladosEjecutados(string json);


        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/PresionesAprobadas",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string PresionesAprobadas(string json);






        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/Reporte_Nro_pedido",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string Reporte_Nro_pedido(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/Reporte_SRP",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string Reporte_SRP(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/Aprobar_srp",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string Aprobar_srp(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/Reporte_pte_rp",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string Reporte_pte_rp(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/ReportePedidos_contratos",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string ReportePedidos_contratos(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/verificar_srp",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string verificar_srp(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/Aprobado_financiera",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string Aprobado_financiera(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/Guardar_aprobacion_presion",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
     )]
        string Guardar_aprobacion_presion(string json);



      [OperationContract]
      [WebInvoke(
      Method = "POST",
      UriTemplate = "/Guardar_Aprobacion_traslado",
      ResponseFormat = WebMessageFormat.Json,
      RequestFormat = WebMessageFormat.Json,
      BodyStyle = WebMessageBodyStyle.WrappedRequest
      )]
      string Guardar_Aprobacion_traslado(string json);















        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/Aprobar_RP",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string Aprobar_RP(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/Validarpermiso",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string Validarpermiso(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/Update_estados",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string Update_estados(string json);


        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/Info_ped_srp",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string Info_ped_srp(string json);


        //#region GASTOS    
        //[OperationContract]
        //[WebInvoke(
        //Method = "POST",
        //UriTemplate = "/CreateGastos",
        //ResponseFormat = WebMessageFormat.Json,
        //RequestFormat = WebMessageFormat.Json,
        //BodyStyle = WebMessageBodyStyle.WrappedRequest
        //)]
        //string CreateGastos(string json);

        //[OperationContract]
        //[WebInvoke(
        //Method = "POST",
        //UriTemplate = "/UpdateGastos",
        //ResponseFormat = WebMessageFormat.Json,
        //RequestFormat = WebMessageFormat.Json,
        //BodyStyle = WebMessageBodyStyle.WrappedRequest
        //)]
        //string UpdateGastos(string json);

        //[OperationContract]
        //[WebInvoke(
        //Method = "POST",
        //UriTemplate = "/ReadGastos",
        //ResponseFormat = WebMessageFormat.Json,
        //RequestFormat = WebMessageFormat.Json,
        //BodyStyle = WebMessageBodyStyle.WrappedRequest
        //)]
        //string ReadGastos(string json);

        //[OperationContract]
        //[WebInvoke(
        //Method = "POST",
        //UriTemplate = "/DeleteGastos",
        //ResponseFormat = WebMessageFormat.Json,
        //RequestFormat = WebMessageFormat.Json,
        //BodyStyle = WebMessageBodyStyle.WrappedRequest
        //)]
        //string DeleteGastos(string json);


        //[OperationContract]
        //[WebInvoke(
        //    Method = "POST",
        //    UriTemplate = "/reademp",
        //    ResponseFormat = WebMessageFormat.Json,
        //    RequestFormat = WebMessageFormat.Json,
        //    BodyStyle = WebMessageBodyStyle.WrappedRequest
        //    )]
        //string reademp(string json);

        //#endregion

        //#region NOMINA
        //[OperationContract]
        //[WebInvoke(
        //Method = "POST",
        //UriTemplate = "/GenerateNomina",
        //ResponseFormat = WebMessageFormat.Json,
        //RequestFormat = WebMessageFormat.Json,
        //BodyStyle = WebMessageBodyStyle.WrappedRequest
        //)]
        //string GenerateNomina(string json);

        //[OperationContract]
        //[WebInvoke(
        //Method = "POST",
        //UriTemplate = "/SearchNomina",
        //ResponseFormat = WebMessageFormat.Json,
        //RequestFormat = WebMessageFormat.Json,
        //BodyStyle = WebMessageBodyStyle.WrappedRequest
        //)]
        //string SearchNomina(string json);


        //[OperationContract]
        //[WebInvoke(
        //Method = "POST",
        //UriTemplate = "/CreateNomina",
        //ResponseFormat = WebMessageFormat.Json,
        //RequestFormat = WebMessageFormat.Json,
        //BodyStyle = WebMessageBodyStyle.WrappedRequest
        //)]
        //string CreateNomina(string json);
        //#endregion

        //#region CARTERA

        //[OperationContract]
        //[WebInvoke(
        //   Method = "POST",
        //   UriTemplate = "/PreloadCartera",
        //   ResponseFormat = WebMessageFormat.Json,
        //   RequestFormat = WebMessageFormat.Json,
        //   BodyStyle = WebMessageBodyStyle.WrappedRequest
        //   )]
        //string PreloadCartera();

        //[OperationContract]
        //[WebInvoke(
        //Method = "POST",
        //UriTemplate = "/SearchCartera",
        //ResponseFormat = WebMessageFormat.Json,
        //RequestFormat = WebMessageFormat.Json,
        //BodyStyle = WebMessageBodyStyle.WrappedRequest
        //)]
        //string SearchCartera(string json);
        //#endregion

        //#region PROXIMOS A VENCER
        //[OperationContract]
        //[WebInvoke(
        //   Method = "POST",
        //   UriTemplate = "/ProximosVencer",
        //   ResponseFormat = WebMessageFormat.Json,
        //   RequestFormat = WebMessageFormat.Json,
        //   BodyStyle = WebMessageBodyStyle.WrappedRequest
        //   )]
        //string ProximosVencer();
        //#endregion

        //#region PLANILLA PRESTAMOS  
        //[OperationContract]
        //[WebInvoke(
        //   Method = "POST",
        //   UriTemplate = "/ReadPlanilla",
        //   ResponseFormat = WebMessageFormat.Json,
        //   RequestFormat = WebMessageFormat.Json,
        //   BodyStyle = WebMessageBodyStyle.WrappedRequest
        //   )]
        //string ReadPlanilla();
        //#endregion

        //#endregion

        #region
        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/Createdescuento",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string Createdescuento(string json);
        #endregion

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/Validarpermisos",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string Validarpermisos(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/Read_User",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string Read_User(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/ReadPedidos_detalle_rp",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string ReadPedidos_detalle_rp(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/Save_detalles_rp",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string Save_detalles_rp(string json);

        [OperationContract]
        [WebInvoke(
          Method = "POST",
          UriTemplate = "/PreloaDirectivos",
          ResponseFormat = WebMessageFormat.Json,
          RequestFormat = WebMessageFormat.Json,
          BodyStyle = WebMessageBodyStyle.WrappedRequest
          )]
        string PreloaDirectivos();

        [OperationContract]
        [WebInvoke(
          Method = "POST",
          UriTemplate = "/CreateActa_Satisfaccion",
          ResponseFormat = WebMessageFormat.Json,
          RequestFormat = WebMessageFormat.Json,
          BodyStyle = WebMessageBodyStyle.WrappedRequest
          )]
        string CreateActa_Satisfaccion(string json);

        [OperationContract]
        [WebInvoke(
          Method = "POST",
          UriTemplate = "/Update_ASatisfaccion",
          ResponseFormat = WebMessageFormat.Json,
          RequestFormat = WebMessageFormat.Json,
          BodyStyle = WebMessageBodyStyle.WrappedRequest
          )]
        string Update_ASatisfaccion(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/Validar_saldo_disponibe",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string Validar_saldo_disponibe(string json);

        [OperationContract]
        [WebInvoke(
      Method = "POST",
      UriTemplate = "/ReporteAS",
      ResponseFormat = WebMessageFormat.Json,
      RequestFormat = WebMessageFormat.Json,
      BodyStyle = WebMessageBodyStyle.WrappedRequest
      )]
            string ReporteAS(string json);


        [OperationContract]
        [WebInvoke(
      Method = "POST",
      UriTemplate = "/Read_Acta_Sat",
      ResponseFormat = WebMessageFormat.Json,
      RequestFormat = WebMessageFormat.Json,
      BodyStyle = WebMessageBodyStyle.WrappedRequest
      )]
        string Read_Acta_Sat(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/Read_Detalle_rp",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string Read_Detalle_rp(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/Aprobar_AS",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string Aprobar_AS(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/ReportePedidos_ptes",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string ReportePedidos_ptes(string json);


        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/verificar__aprobar_srp",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string verificar__aprobar_srp(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/convert_proveedor",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string convert_proveedor(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/execute_saldos_pdte_af",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string execute_saldos_pdte_af();

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/Reporte_planilla_nomina",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string Reporte_planilla_nomina(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/Read_planillas_aprobada_nomina",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string Read_planillas_aprobada_nomina();

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/Reporte_activos_pendientes",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string Reporte_activos_pendientes();

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/Reporte_depreciacion_af",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string Reporte_depreciacion_af();


        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/Ingreso_depreciacion_af",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string Ingreso_depreciacion_af(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/Depreciar_af",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string Depreciar_af();

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/Depreciar_af_nuevo",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string Depreciar_af_nuevo();


        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/ExportarPedidosFinanciera",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string ExportarPedidosFinanciera();

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/ExportarPedidosCompras",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string ExportarPedidosCompras();

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/recorte_pedido",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string recorte_pedido(string json);
      
        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/create_traslado2",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string create_traslado2(string json);


        #region
        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/create_presion",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string create_presion(string json);


        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/preload_comodato",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string preload_comodato();
       
         
        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/Read_comodato",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string Read_comodato(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/Update_Comodato",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string Update_Comodato(string json);

        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/CreateComodato",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string CreateComodato(string json);

       [OperationContract]
       [WebInvoke(
       Method = "POST",
       UriTemplate = "/Solicitud_create_presion",
       ResponseFormat = WebMessageFormat.Json,
       RequestFormat = WebMessageFormat.Json,
       BodyStyle = WebMessageBodyStyle.WrappedRequest
       )]
        string Solicitud_create_presion(string json);


        #endregion
        [OperationContract]
        [WebInvoke(
        Method = "POST",
        UriTemplate = "/create_traslado",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.WrappedRequest
        )]
        string create_traslado(string json);

      





    }
}
#endregion
#endregion