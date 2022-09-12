using Rhino.Sql;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.Objects;
using System.Data.Objects.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Web.Script.Serialization;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Rhino.Models;
using GemBox.Spreadsheet;

namespace Rhino.Service
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehaviorAttribute(IncludeExceptionDetailInFaults = true)]
    public class Servicios : IServicios
    {

        JavaScriptSerializer jss = new JavaScriptSerializer();
        rhinoEntities conn = new rhinoEntities();

        //#region clientes
        //public string CreateClientes(string json)
        //{
        //    dynamic data = jss.DeserializeObject(json);
        //    string _json = data["json"];
        //    try
        //    {
        //        clientes c = jss.Deserialize<clientes>(_json);
        //        conn.clientes.Add(c);
        //        int rs = conn.SaveChanges();
        //        return jss.Serialize(rs > 0);
        //    }
        //    catch (DbEntityValidationException e)
        //    {
        //        string err = "";
        //        foreach (var eve in e.EntityValidationErrors)
        //        {
        //            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
        //                eve.Entry.Entity.GetType().Name, eve.Entry.State);
        //            foreach (var ve in eve.ValidationErrors)
        //            {
        //                err += ve.ErrorMessage;
        //                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
        //                    ve.PropertyName, ve.ErrorMessage);
        //            }
        //        }
        //        return jss.Serialize(err);
        //    }
        //}
        //public string UpdateClientes(string json)
        //{
        //    dynamic data = jss.DeserializeObject(json);
        //    string _json = data["json"];
        //    int rs = 0;
        //    clientes c = jss.Deserialize<clientes>(_json);
        //    conn.Entry(c).State = System.Data.EntityState.Modified;
        //    rs = conn.SaveChanges();
        //    //logoUsuario02("MEDICOS", "ACTUALIZAR", codigo_usuario, rs);
        //    return jss.Serialize(rs > 0);
        //}
        //public string ReadClientes(string json)
        //{
        //    dynamic data = jss.DeserializeObject(json);
        //    int id = data["id"];
        //    clientes c = conn.clientes.Where(a => a.id_cliente.Equals(id)).FirstOrDefault();
        //    usuario u = conn.usuario.Where(a => a.id.Equals(c.usu_id)).FirstOrDefault();
        //    c.usu_id = Convert.ToInt32(u.codigo);
        //    return jss.Serialize(c);
        //}
        //public string DeleteClientes(string json)
        //{
        //    dynamic data = jss.DeserializeObject(json);
        //    int id = data["id"];
        //    clientes clientes = conn.clientes.Where(a => a.id_cliente.Equals(id)).FirstOrDefault();
        //    conn.clientes.Remove(clientes);
        //    int rs = conn.SaveChanges();
        //    return jss.Serialize(rs);
        //}
        //public string ValidateClientes(string json)
        //{
        //    bool b = false;
        //    dynamic data = jss.DeserializeObject(json);
        //    string username = data["clientes"];
        //    bool u = false;
        //    u = conn.clientes.Where(z => z.codigo.Equals(username)).Count() > 0;
        //    return jss.Serialize(u);
        //}
        //#endregion

        public string PreloadViews(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            string user = data["usuario"];
            List<object> arr = new List<object>();
            List<views> v = conn.views.ToList();
            foreach(var item in v)
            {
                permisos_usu p = conn.permisos_usu.Where(z => z.id_view.Equals(item.id) && z.usuario.Equals(user)).FirstOrDefault();
                if(p != null)
                {       
                    if(p.autorizacion == "1") { 
                        item.visible = "true";
                    }
                    else
                    {
                        item.visible = "false";
                    }
                }
                else
                {
                    item.visible = "false";
                }
            }
            arr.Add(v);
            return jss.Serialize(arr);
        }

        #region usuario
        public string PreloadUsuario(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            string ced = data["cedula"];
            List<object> arr = new List<object>();
            List<tipo_usuario> tu = conn.tipo_usuario.ToList();
            usuario u = conn.usuario.Where(a => a.codigo.Equals(ced)).FirstOrDefault();
            var views = conn.views.ToList();
            arr.Add(tu);
            arr.Add(u);
            arr.Add(views);


            return jss.Serialize(arr);
        }
        public string CreateUsuarios(string json)
        {
            try
            {
                dynamic data = jss.DeserializeObject(json);
                string _json = data["json"];
                dynamic list = jss.DeserializeObject(data["tabla"]);
                string codigo_usuario = data["codigo_usuario"];
                usuario usuario = jss.Deserialize<usuario>(_json);
                string password = usuario.password;
                usuario.password = encriptar(password);
                foreach (dynamic item in list)
                {
                    permisos_usu pu = new permisos_usu();

                    pu.id_view = item["id"];
                    pu.id_usuario = 0;
                    pu.usuario = usuario.username;
                    pu.autorizacion = item["autorizado"]+"";
                    pu.pe1 = Convert.ToString(item["pe1"]);
                    pu.pe2 = Convert.ToString(item["pe2"]);

                    conn.permisos_usu.Add(pu);

                }
                conn.usuario.Add(usuario);
                int rs = conn.SaveChanges();
                //log_User("CREAR USUARIO", "GUARDAR", codigo_usuario, rs);
                return jss.Serialize(rs > 0);
            }
            catch (DbEntityValidationException e)
            {
                string err = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        err += ve.ErrorMessage;
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return jss.Serialize("{err:" + err + "}");
            }
        }
        public string ValidateUsername(string json)
        {
            bool b = false;
            dynamic data = jss.DeserializeObject(json);
            string username = data["username"];
            bool u = false;
            u = conn.usuario.Where(z => z.username.Equals(username)).Count() > 0;
            return jss.Serialize(u);
        }
        public string ReadUsuario(string json)
        {
            List<object> arr = new List<object>();
            dynamic data = jss.DeserializeObject(json);
            string documento = data["usuario"];
            usuario usuario = conn.usuario.Where(a => a.codigo == documento).FirstOrDefault();
            var permisos = conn.permisos_usu.Where(a => a.usuario.Equals(usuario.username)).ToList();
            arr.Add(usuario);
            arr.Add(permisos);
            return jss.Serialize(arr);

        }
        public string UpdateUsuario(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            string _json = data["json"];
            dynamic list = jss.DeserializeObject(data["tabla"]);
            int rs = 0;
            usuario c = jss.Deserialize<usuario>(_json);
            string password = c.password;
            c.password = encriptar(password);
            conn.Entry(c).State = System.Data.EntityState.Modified;
            var p = conn.permisos_usu.Where(a => a.usuario == c.username).ToList();
            foreach (dynamic item in p)
            {
                conn.permisos_usu.Remove(item);
                rs = conn.SaveChanges();
            }

            foreach (dynamic item in list)
            {
                permisos_usu pu = new permisos_usu();

                pu.id_view = item["id"];
                pu.id_usuario = 0;
                pu.usuario = c.username;
                pu.autorizacion = item["autorizado"] + "";
                pu.pe1 = Convert.ToString(item["pe1"]);
                pu.pe2 = Convert.ToString(item["pe2"]);

                conn.permisos_usu.Add(pu);

            }
            rs = conn.SaveChanges();
            return jss.Serialize(rs > 0);
        }
        #endregion

        //#region Prestamos
        //public string PreloadPrestamo()
        //{
        //    List<object> arr = new List<object>();
        //    List<tipo_interes> ti = conn.tipo_interes.ToList();
        //    arr.Add(ti);


        //    return jss.Serialize(arr);
        //}
        //public string CreatePrestamos(string json)
        //{
        //    dynamic data = jss.DeserializeObject(json);
        //    string _json = data["json"];
        //    try
        //    {
        //        prestamo c = jss.Deserialize<prestamo>(_json);
        //        //c.fecha_corte = c.fecha_prestamo;
        //        c.t_prestado = Convert.ToInt32(c.total_pagar);
        //        c.estado = 1;
        //        conn.prestamo.Add(c);
        //        int rs = conn.SaveChanges();
        //        return jss.Serialize(rs > 0);
        //    }
        //    catch (DbEntityValidationException e)
        //    {
        //        string err = "";
        //        foreach (var eve in e.EntityValidationErrors)
        //        {
        //            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
        //                eve.Entry.Entity.GetType().Name, eve.Entry.State);
        //            foreach (var ve in eve.ValidationErrors)
        //            {
        //                err += ve.ErrorMessage;
        //                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
        //                    ve.PropertyName, ve.ErrorMessage);
        //            }
        //        }
        //        return jss.Serialize(err);
        //    }
        //}
        //public string ReadPrestamos(string json)
        //{
        //    dynamic data = jss.DeserializeObject(json);
        //    int id = data["id"];
        //    prestamo c = conn.prestamo.Where(a => a.id_prestamo.Equals(id)).FirstOrDefault();
        //    clientes x = conn.clientes.Where(p => p.id_cliente.Equals(c.cliente_id)).FirstOrDefault();
        //    c.cliente_id = Convert.ToInt32(x.codigo);
        //    return jss.Serialize(c);
        //}
        //public string ReadDatosPrestamo(string json)
        //{
        //    dynamic data = jss.DeserializeObject(json);
        //    int id = data["id"];
        //    List<object> arr = new List<object>();
        //    prestamo c = conn.prestamo.Where(a => a.id_prestamo.Equals(id)).FirstOrDefault();
        //    clientes x = conn.clientes.Where(p => p.id_cliente.Equals(c.cliente_id)).FirstOrDefault();
        //    tipo_interes i = conn.tipo_interes.Where(b => b.id_tipo_inte == c.tipo_interes_id).FirstOrDefault();
        //    arr.Add(c);
        //    arr.Add(x);
        //    arr.Add(i);
        //    return jss.Serialize(arr);
        //}
        //#endregion

        //#region Pagos
        //public string CreatePagos(string json)
        //{
        //    dynamic data = jss.DeserializeObject(json);
        //    string _json = data["json"];
        //    int id_prestamo = data["idprestamo"];
        //    int total = Convert.ToInt32(data["total"]);
        //    int matarp = data["matarp"];
        //    string tipo = Convert.ToString(data["tipo"]);
        //    dynamic head = jss.DeserializeObject(data["json"]);
        //    try
        //    {
        //        if (tipo == "1")
        //        {
        //            pagos c = jss.Deserialize<pagos>(_json);
        //            c.prestamo_id = id_prestamo;
        //            prestamo p = conn.prestamo.Where(x => x.id_prestamo.Equals(id_prestamo)).FirstOrDefault();
        //            DateTime fechaaux = c.fecha_pago;
        //            p.fecha_corte = fechaaux;
        //            p.monto = p.monto - total;
        //            p.t_prestado = p.t_prestado - (head["valor_pagar_capital"] + head["valor_pagar_interes"]);
        //            c.resta = p.t_prestado;
        //            p.dia_cobro = Convert.ToInt32(head["dia_cob"]);
        //            if (p.t_prestado > 0)
        //            {
        //                conn.Entry(p).State = System.Data.EntityState.Modified;
        //                conn.pagos.Add(c);
        //                if (matarp == 1)
        //                {
        //                    p.estado = 0;
        //                    conn.Entry(p).State = System.Data.EntityState.Modified;
        //                }
        //            }
        //            if (c.resta <= 0)
        //            {
        //                p.estado = 0;
        //                conn.Entry(p).State = System.Data.EntityState.Modified;
        //                //conn.pagos.Add(c);
        //            }
        //        }
        //        if (tipo == "0")
        //        {
        //            pagos c = jss.Deserialize<pagos>(_json);
        //            c.prestamo_id = id_prestamo;
        //            prestamo p = conn.prestamo.Where(x => x.id_prestamo.Equals(id_prestamo)).FirstOrDefault();
        //            DateTime fechaaux = c.fecha_pago;
        //            p.fecha_corte = fechaaux;
        //            p.monto = p.monto - total;
        //            c.resta = p.monto;
        //            p.dia_cobro = Convert.ToInt32(head["dia_cob"]);
        //            if (p.monto > 0)
        //            {
        //                conn.Entry(p).State = System.Data.EntityState.Modified;
        //                conn.pagos.Add(c);
        //                if (matarp == 1)
        //                {
        //                    p.estado = 0;
        //                    conn.Entry(p).State = System.Data.EntityState.Modified;
        //                }
        //            }
        //            if (c.resta <= 0)
        //            {
        //                p.estado = 0;
        //                conn.Entry(p).State = System.Data.EntityState.Modified;
        //                //conn.pagos.Add(c);
        //            }
        //        }
        //        int rs = conn.SaveChanges();
        //        return jss.Serialize(rs > 0);
        //    }
        //    catch (DbEntityValidationException e)
        //    {
        //        string err = "";
        //        foreach (var eve in e.EntityValidationErrors)
        //        {
        //            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
        //                eve.Entry.Entity.GetType().Name, eve.Entry.State);
        //            foreach (var ve in eve.ValidationErrors)
        //            {
        //                err += ve.ErrorMessage;
        //                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
        //                    ve.PropertyName, ve.ErrorMessage);
        //            }
        //        }
        //        return jss.Serialize(err);
        //    }
        //}
        //public string ReadPagos(string json)
        //{
        //    dynamic data = jss.DeserializeObject(json);
        //    int id = data["id"];
        //    prestamo c = conn.prestamo.Where(a => a.id_prestamo.Equals(id)).FirstOrDefault();
        //    clientes x = conn.clientes.Where(p => p.id_cliente.Equals(c.cliente_id)).FirstOrDefault();
        //    c.cliente_id = Convert.ToInt32(x.codigo);
        //    return jss.Serialize(c);
        //}
        //public string ReadClientePrestamo(string json)
        //{
        //    dynamic data = jss.DeserializeObject(json);
        //    int id = Convert.ToInt32(data["id"]);
        //    List<object> arr = new List<object>();

        //    prestamo x = conn.prestamo.Where(p => p.id_prestamo.Equals(id)).FirstOrDefault();
        //    clientes c = conn.clientes.Where(a => a.id_cliente.Equals(x.cliente_id)).FirstOrDefault();
        //    tipo_interes ti = conn.tipo_interes.Where(i => i.id_tipo_inte.Equals(x.tipo_interes_id)).FirstOrDefault();
        //    arr.Add(c);
        //    arr.Add(x);
        //    arr.Add(ti);
        //    return jss.Serialize(arr);
        //}
        //public string ReadTablaPagos(string json)
        //{
        //    dynamic data = jss.DeserializeObject(json);
        //    int id = data["id"];
        //    List<object> arr = new List<object>();
        //    var pg = conn.pagos.Where(z => z.prestamo_id.Equals(id)).ToList();
        //    return jss.Serialize(pg);
        //}
        //public string CreatePagosRuta(string json)
        //{
        //    dynamic data = jss.DeserializeObject(json);
        //    string _json = data["json"];

        //    dynamic head = jss.DeserializeObject(data["json"]);
        //    int id_prestamo = Convert.ToInt32(head["Id_prestamo"]);
        //    DateTime f_pago = Convert.ToDateTime(head["fecha_pago"]);
        //    int id_presta = Convert.ToInt32(head["Id_prestamo"]);
        //    pagos c = new pagos();
        //    c.prestamo_id = Convert.ToInt32(head["Id_prestamo"]);
        //    prestamo p = conn.prestamo.Where(x => x.id_prestamo.Equals(id_prestamo)).FirstOrDefault();
        //    try
        //    {
        //        if (head["Tipo_prestamo"] == 1)
        //        {
        //            p.monto = head["Valor_pagar_capital"];
        //            p.t_prestado = head["Resta"];
        //            if (p.monto > 0)
        //            {
        //                conn.Entry(p).State = System.Data.EntityState.Modified;
        //                pagos pa = conn.pagos.Where(a => a.prestamo_id.Equals(id_presta) && a.fecha_pago == f_pago).FirstOrDefault();
        //                if (pa != null)
        //                {
        //                    pa.valor_pagar = Convert.ToInt32(head["pago"]);
        //                    pa.valor_pagar_capital = head["Valor_pagar_capital"];
        //                    pa.valor_pagar_interes = head["Valor_pagar_interes"];
        //                    conn.Entry(pa).State = System.Data.EntityState.Modified;
        //                }
        //                else
        //                {
        //                    c.hora = head["hora"];
        //                    c.valor_pagar = Convert.ToInt32(head["pago"]);
        //                    c.valor_pagar_interes = head["Valor_pagar_interes"];
        //                    c.valor_pagar_capital = head["Valor_pagar_capital"];
        //                    c.cliente_id = head["Id_cliente"];
        //                    c.matarp = 0;
        //                    c.usu_id = head["usu_id"];
        //                    c.resta = head["Resta"];
        //                    c.fecha_pago = Convert.ToDateTime(head["fecha_pago"]);
        //                    if (head["Fecha_plazo"].Equals("/Date(-62135578800000)/"))
        //                    {
        //                        c.fecha_plazo = null;
        //                    }
        //                    else
        //                    {
        //                        c.fecha_plazo = Convert.ToDateTime(head["Fecha_plazo"]);
        //                    }
        //                    conn.pagos.Add(c);
        //                }

        //            }
        //            if (c.resta <= 0)
        //            {
        //                p.estado = 0;
        //                conn.Entry(p).State = System.Data.EntityState.Modified;
        //            }
        //        }
        //        if (head["Tipo_prestamo"] == 0)
        //        {
        //            p.monto = head["Valor_pagar_capital"];
        //            if (p.monto > 0)
        //            {
        //                conn.Entry(p).State = System.Data.EntityState.Modified;
        //                pagos pa = conn.pagos.Where(a => a.prestamo_id.Equals(id_presta) && a.fecha_pago == f_pago).FirstOrDefault();
        //                if (pa != null)
        //                {
        //                    pa.valor_pagar = Convert.ToInt32(head["pago"]);
        //                    pa.valor_pagar_interes = head["Valor_pagar_interes"];
        //                    pa.valor_pagar_capital = head["Valor_pagar_capital"];
        //                    pa.hora = head["hora"];
        //                    pa.cliente_id = head["Id_cliente"];
        //                    pa.usu_id = head["usu_id"];
        //                    pa.resta = head["Resta"];
        //                    pa.fecha_pago = Convert.ToDateTime(head["fecha_pago"]);
        //                    conn.Entry(pa).State = System.Data.EntityState.Modified;
        //                }
        //                else
        //                {
        //                    c.hora = head["hora"];
        //                    c.valor_pagar = Convert.ToInt32(head["pago"]);
        //                    c.valor_pagar_interes = head["Valor_pagar_interes"];
        //                    c.valor_pagar_capital = head["Valor_pagar_capital"];
        //                    c.cliente_id = head["Id_cliente"];
        //                    c.matarp = 0;
        //                    c.usu_id = head["usu_id"];
        //                    c.resta = head["Resta"];
        //                    c.fecha_pago = Convert.ToDateTime(head["fecha_pago"]);
        //                    if (head["Fecha_plazo"].Equals("/Date(-62135578800000)/"))
        //                    {
        //                        c.fecha_plazo = null;
        //                    }
        //                    else
        //                    {
        //                        c.fecha_plazo = Convert.ToDateTime(head["Fecha_plazo"]);
        //                    }
        //                    conn.pagos.Add(c);
        //                }

        //            }
        //            if (head["Resta"] <= 0)
        //            {
        //                p.estado = 0;
        //                conn.Entry(p).State = System.Data.EntityState.Modified;
        //                //conn.pagos.Add(c);
        //            }
        //        }

        //        int rs = conn.SaveChanges();
        //        return jss.Serialize(rs > 0);
        //    }
        //    catch (DbEntityValidationException e)
        //    {
        //        string err = "";
        //        foreach (var eve in e.EntityValidationErrors)
        //        {
        //            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
        //                eve.Entry.Entity.GetType().Name, eve.Entry.State);
        //            foreach (var ve in eve.ValidationErrors)
        //            {
        //                err += ve.ErrorMessage;
        //                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
        //                    ve.PropertyName, ve.ErrorMessage);
        //            }
        //        }
        //        return jss.Serialize("{err:" + err + "}");
        //    }
        //}
        //#endregion

        //#region RUTA
        ////public string ReadRuta(string json)
        ////{
        ////    jss.MaxJsonLength = 500000000;
        ////    conn.Configuration.ValidateOnSaveEnabled = false;
        ////    conn.Configuration.AutoDetectChangesEnabled = false;
        ////    conn.Configuration.LazyLoadingEnabled = false;
        ////    conn.Configuration.ProxyCreationEnabled = false;

        ////    dynamic data = jss.DeserializeObject(json);
        ////    int iduser,idadmin;
        ////    iduser = Convert.ToInt32(data["id_user"]);
        ////    idadmin = Convert.ToInt32(data["admin"]);
        ////    string fecha = data["fecha"];

        ////    string stringCon = System.Configuration.ConfigurationManager.ConnectionStrings["rhinoEntities"].ConnectionString;
        ////    string[] vec = stringCon.Split(';');
        ////    string preStrCon = vec[2].Substring(28) + ";" + vec[3] + ";" + vec[4] + ";" + vec[5] + ";" + vec[6];
        ////    using (SqlConnection con = new SqlConnection(preStrCon))
        ////    {
        ////        try
        ////        {
        ////            int rs = 0;
        ////            con.Open();
        ////            var commandStr = "";
        ////            if (idadmin != 0) { 
        ////            commandStr = "SELECT DATEADD(day, dias, p.fecha_corte) as fecha,FORMAT(p.fecha_corte, 'yyyy/MM/dd', 'en-US' ) as fecha_corte,* FROM prestamo as p INNER JOIN clientes as c on p.cliente_id = c.id_cliente  INNER JOIN tipo_interes as ti on p.tipo_interes_id = ti.id_tipo_inte LEFT JOIN pagos as pa on p.id_prestamo = pa.prestamo_id where p.estado =1 and c.usu_id = '" + iduser + "'and DATEADD(day, dias, p.fecha_corte) = '" + fecha + "' or fecha_plazo ='" + fecha + "'";
        ////                //and(fecha_pago = '" + fecha + "' or fecha_pago IS NULL)
        ////            }
        ////            else {
        ////                commandStr = "SELECT DATEADD(day, dias, p.fecha_corte) as fecha,p.fecha_corte as fecha_corte,* FROM prestamo as p INNER JOIN clientes as c on p.cliente_id = c.id_cliente  INNER JOIN tipo_interes as ti on p.tipo_interes_id = ti.id_tipo_inte LEFT JOIN pagos as pa on p.id_prestamo = pa.prestamo_id where p.estado =1 and DATEADD(day, dias, p.fecha_corte) = '" + fecha + "' or fecha_plazo ='" + fecha + "' ";
        ////            }
        ////            SqlCommand sql = new SqlCommand(commandStr, con);
        ////            SqlDataReader read = sql.ExecuteReader();
        ////            string str = WriteReaderToJSON(read);
        ////            con.Close();
        ////            return (str);
        ////        }
        ////        catch (DbEntityValidationException e)
        ////        {
        ////            string err = "";
        ////            foreach (var eve in e.EntityValidationErrors)
        ////            {
        ////                Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
        ////                    eve.Entry.Entity.GetType().Name, eve.Entry.State);
        ////                foreach (var ve in eve.ValidationErrors)
        ////                {
        ////                    err += ve.ErrorMessage;
        ////                    Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
        ////                        ve.PropertyName, ve.ErrorMessage);
        ////                }
        ////            }
        ////            return jss.Serialize(err);
        ////        }
        ////    }
        ////}
        //public string ReadRuta(string json)
        //{
        //    jss.MaxJsonLength = 500000000;
        //    conn.Configuration.ValidateOnSaveEnabled = false;
        //    conn.Configuration.AutoDetectChangesEnabled = false;
        //    conn.Configuration.LazyLoadingEnabled = false;
        //    conn.Configuration.ProxyCreationEnabled = false;
        //    try
        //    {
        //        dynamic data = jss.DeserializeObject(json);
        //        int iduser = Convert.ToInt32(data["id_user"]);
        //        int idadmin = Convert.ToInt32(data["admin"]);
        //        string fecha = data["fecha"];
        //        DateTime fechad = Convert.ToDateTime(fecha);
        //        DateTime fecha_actual = DateTime.Today;
        //        List<prestamo> arr = new List<prestamo>();
        //        List<Ruta> ruta = new List<Ruta>();
        //        int rt = conn.ruta.Where(x => x.fecha_ruta.Equals(fechad)).Count();
        //        if (rt == 0)
        //        {
        //            var p = conn.prestamo.Where(y => y.estado.Equals(1)).ToList();
        //            foreach (var i in p)
        //            {
        //                if (i.tipo_interes_id == 2)
        //                {
        //                    if (i.fecha_corte.AddDays(1) == fechad)
        //                    {
        //                        arr.Add(i);
        //                    }
        //                }
        //                if (i.tipo_interes_id == 3)
        //                {
        //                    if (i.fecha_corte.AddDays(7) == fechad)
        //                    {
        //                        arr.Add(i);
        //                    }
        //                }
        //                if (i.tipo_interes_id == 4)
        //                {
        //                    if (i.fecha_corte.AddDays(15) == fechad)
        //                    {
        //                        arr.Add(i);
        //                    }
        //                }
        //                if (i.tipo_interes_id == 5)
        //                {
        //                    if (i.fecha_corte.AddDays(30) == fechad)
        //                    {
        //                        arr.Add(i);
        //                    }
        //                }
        //                clientes c = conn.clientes.Where(x => x.id_cliente.Equals(i.cliente_id)).FirstOrDefault();
        //                pagos pago = conn.pagos.OrderByDescending(y => y.id_pagos).Where(y => y.prestamo_id.Equals(i.id_prestamo) && y.fecha_pago.Equals(fechad)).FirstOrDefault();
        //                pagos pago2 = conn.pagos.Where(r => r.prestamo_id.Equals(i.id_prestamo) && r.fecha_plazo != null).FirstOrDefault();
        //                if (pago2 != null && pago2.fecha_plazo.Equals(fechad))
        //                {
        //                    arr.Add(i);
        //                }
        //                tipo_interes t = conn.tipo_interes.Where(z => z.id_tipo_inte.Equals(i.tipo_interes_id)).FirstOrDefault();
        //                foreach (prestamo aux in arr)
        //                {
        //                    if (aux.id_prestamo.Equals(i.id_prestamo))
        //                    {
        //                        Ruta r = new Ruta();
        //                        r.Apellidos = c.apellidos;
        //                        r.Nombres = c.nombres;
        //                        r.Codigo = c.codigo;
        //                        r.Direccion = c.direccion;
        //                        r.Fecha_corte = Convert.ToDateTime(aux.fecha_corte);
        //                        r.Fecha_prestamo = Convert.ToDateTime(aux.fecha_prestamo);
        //                        r.Id_cliente = c.id_cliente;
        //                        r.Id_prestamo = aux.id_prestamo;
        //                        r.Interes = aux.interes;
        //                        r.Monto = Convert.ToDecimal(aux.monto);
        //                        r.Num_cuotas = aux.num_cuotas;
        //                        r.Obs = aux.observacion;
        //                        r.Tipo_intereses_id = aux.tipo_interes_id;
        //                        r.Tipo_prestamo = aux.tipo_prestamo;
        //                        r.T_prestado = Convert.ToDecimal(aux.t_prestado);
        //                        r.Total_pagar = Convert.ToString(aux.total_pagar);
        //                        r.Vlr_cuotas = aux.vlr_cuotas;
        //                        r.Num_meses = Convert.ToInt32(aux.num_meses);
        //                        if (aux.tipo_prestamo.Equals(0))
        //                        {
        //                            r.Valor_pagar = calcularvlr_pagar(r.Monto, r.Interes, r.Fecha_corte, t.dias, fechad);
        //                        }
        //                        else
        //                        {
        //                            r.Valor_pagar = aux.vlr_cuotas;
        //                            Double inte = Convert.ToDouble(aux.interes) / 100;

        //                            r.Total_interes = Convert.ToDecimal((aux.vlr_prestado * inte) * aux.num_meses);
        //                            r.Inte_x_cuotas = r.Total_interes / r.Num_cuotas;
        //                        }
        //                        r.Vlr_prestado = aux.vlr_prestado;
        //                        if (pago != null)
        //                        {
        //                            r.Id_pagos = pago.id_pagos;
        //                            r.pago = pago.valor_pagar;
        //                            r.hora = pago.hora;
        //                            r.Fecha_plazo = Convert.ToDateTime(pago.fecha_plazo);
        //                            r.Resta = pago.resta;

        //                        }
        //                        else
        //                        {
        //                            r.Id_pagos = 0;
        //                            if (aux.tipo_prestamo.Equals(0))
        //                            {
        //                                r.Resta = Convert.ToDecimal(r.Valor_pagar + r.Monto);
        //                            }
        //                            else
        //                            {
        //                                r.Resta = aux.t_prestado;
        //                            }
        //                        }
        //                        if (pago2 != null)
        //                        {
        //                            r.Id_pagos = pago2.id_pagos;
        //                            r.Resta = pago2.resta;
        //                            r.pago = pago2.valor_pagar;
        //                            r.hora = pago2.hora;
        //                            r.Fecha_plazo = Convert.ToDateTime(pago2.fecha_plazo);
        //                        }
        //                        else
        //                        {
        //                            r.Id_pagos = 0;
        //                            if (aux.tipo_prestamo.Equals(0))
        //                            {
        //                                r.Resta = Convert.ToDecimal(r.Valor_pagar + r.Monto);
        //                            }
        //                            else
        //                            {
        //                                r.Resta = aux.t_prestado;
        //                            }
        //                        }
        //                        int q = 0;
        //                        foreach (var item in ruta)
        //                        {
        //                            if (item.Id_prestamo.Equals(i.id_prestamo))
        //                            {
        //                                q = 1;
        //                            }
        //                        }

        //                        if (q.Equals(0))
        //                        {
        //                            ruta.Add(r);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            var rta = conn.ruta.Where(x => x.fecha_ruta.Equals(fechad)).ToList();

        //            foreach (var item in rta)
        //            {
        //                Ruta r = new Ruta();
        //                clientes c = conn.clientes.Where(x => x.id_cliente.Equals(item.id_cliente)).FirstOrDefault();
        //                prestamo p = conn.prestamo.Where(y => y.id_prestamo.Equals(item.id_prestamo)).FirstOrDefault();
        //                r.Apellidos = c.apellidos;
        //                r.Nombres = c.nombres;
        //                r.Codigo = c.codigo;
        //                r.Direccion = c.direccion;
        //                r.Fecha_prestamo = item.fecha_prestamo;
        //                r.Id_cliente = c.id_cliente;
        //                r.Id_prestamo = item.id_prestamo;
        //                r.Monto = Convert.ToDecimal(item.monto);
        //                r.Resta = Convert.ToDecimal(item.resta);
        //                r.Fecha_plazo = Convert.ToDateTime(item.fecha_plazo);
        //                r.Vlr_prestado = p.vlr_prestado;
        //                r.Vlr_cuotas = item.vlr_cuota;
        //                r.Valor_pagar = item.vlr_cuota;
        //                r.Total_pagar = Convert.ToString(p.total_pagar);
        //                r.pago = item.abono;
        //                r.Resta =  Convert.ToDecimal(item.resta);
        //                r.hora = item.hora;

        //                ruta.Add(r);
        //            }
        //        }
        //        return jss.Serialize(ruta);
        //    }
        //    catch (DbEntityValidationException e)
        //    {
        //        string err = "";
        //        foreach (var eve in e.EntityValidationErrors)
        //        {
        //            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
        //                eve.Entry.Entity.GetType().Name, eve.Entry.State);
        //            foreach (var ve in eve.ValidationErrors)
        //            {
        //                err += ve.ErrorMessage;
        //                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
        //                    ve.PropertyName, ve.ErrorMessage);
        //            }
        //        }
        //        return jss.Serialize(err);
        //    }
        //}

        //private decimal calcularvlr_pagar(decimal monto, int interes, DateTime fecha_corte, int dias, DateTime fechad)
        //{
        //    //DateTime f = DateTime.Now;
        //    try
        //    {
        //        decimal total = 0;
        //        TimeSpan ts = fechad - fecha_corte;
        //        int days = ts.Days;
        //        Decimal subtotal = Convert.ToDecimal(Convert.ToDecimal(interes) / 100);
        //        decimal cuasitottal = Convert.ToDecimal(monto * subtotal);
        //        decimal subtotal2 = Convert.ToDecimal(cuasitottal / dias);
        //        total = subtotal2 * days;
        //        //decimal total = ((monto * (interes / 100)) / dias) * days;
        //        return total;
        //    }
        //    catch (Exception e)
        //    {

        //        string ret = "";
        //        if (e.InnerException != null)
        //        {
        //            if (e.InnerException.Message != null)
        //            {
        //                ret = (e.InnerException.Message + e.InnerException.InnerException.Message);
        //                //ret = (e.InnerException.Message);

        //            }
        //        }
        //        return 0;
        //    }

        //}

        //public string UpdateRuta(string json)
        //{
        //    dynamic data = jss.DeserializeObject(json);
        //    string _json = data["registro"];
        //    dynamic head = jss.DeserializeObject(data["registro"]);
        //    try
        //    {
        //        ruta c = jss.Deserialize<ruta>(_json);
        //        int id_prest = head["id_prestamo"];
        //        DateTime fech_ruta = head["id_prestamo"];
        //        ruta r = conn.ruta.Where(a => a.id_prestamo.Equals(id_prest) && a.fecha_ruta.Equals(fech_ruta)).FirstOrDefault();
        //        if (r != null)
        //        {
        //            conn.ruta.Add(c);
        //        }
        //        else
        //        {
        //            conn.Entry(c).State = System.Data.EntityState.Modified;
        //        }

        //        int rs = conn.SaveChanges();
        //        return jss.Serialize(rs > 0);
        //    }
        //    catch (DbEntityValidationException e)
        //    {
        //        string err = "";
        //        foreach (var eve in e.EntityValidationErrors)
        //        {
        //            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
        //                eve.Entry.Entity.GetType().Name, eve.Entry.State);
        //            foreach (var ve in eve.ValidationErrors)
        //            {
        //                err += ve.ErrorMessage;
        //                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
        //                    ve.PropertyName, ve.ErrorMessage);
        //            }
        //        }
        //        return jss.Serialize(err);
        //    }
        //}
        //public string CreateCierreDia(string json)
        //{
        //    dynamic data = jss.DeserializeObject(json);
        //    string _json = data["json"];
        //    dynamic list = jss.DeserializeObject(data["grilla"]);
        //    dynamic head = jss.DeserializeObject(data["json"]);
        //    try
        //    {
        //        ruta c = jss.Deserialize<ruta>(_json);
        //        foreach (dynamic item in list)
        //        {
        //            ruta r = new ruta();

        //            r.id_usuario = Convert.ToInt32(head["usu_id"]);
        //            r.id_cliente = Convert.ToInt32(item["Id_cliente"]);
        //            r.id_prestamo = Convert.ToInt32(item["Id_prestamo"]);
        //            r.fecha_prestamo = Convert.ToDateTime(item["Fecha_prestamo"]);
        //            r.monto = Convert.ToInt32(item["Monto"]);
        //            r.vlr_cuota = Convert.ToInt32(item["Vlr_cuotas"]);
        //            r.abono = Convert.ToInt32(item["pago"]);
        //            r.resta = Convert.ToString(item["Resta"]);
        //            r.fecha_ruta = Convert.ToDateTime(head["fecha"]);
        //            if (item["Total_pagar"] != null && item["Total_pagar"] != "")
        //            {
        //                r.total_pagar = Convert.ToInt32(item["Total_pagar"]);
        //            }
        //            else
        //            {
        //                r.total_pagar = Convert.ToInt32(r.resta);
        //            }
        //            r.hora = item["hora"];
        //            if (item["Fecha_plazo"] != "/Date(-62135578800000)/")
        //            {
        //                r.fecha_plazo = Convert.ToDateTime(item["Fecha_plazo"]);
        //            }
        //            else
        //            {
        //                r.fecha_plazo = null;
        //            }
        //            r.observacion = "";
        //            int id_prest = Convert.ToInt32(item["Id_prestamo"]);
        //            prestamo p = conn.prestamo.Where(x => x.id_prestamo.Equals(id_prest)).FirstOrDefault();
        //            tipo_interes ti = conn.tipo_interes.Where(z => z.id_tipo_inte.Equals(p.tipo_interes_id)).FirstOrDefault();
        //            p.fecha_corte = p.fecha_corte.AddDays(ti.dias);
        //            conn.Entry(p).State = System.Data.EntityState.Modified;
        //            conn.ruta.Add(r);

        //        }

        //        int rs = conn.SaveChanges();
        //        return jss.Serialize(rs > 0);
        //    }
        //    catch (DbEntityValidationException e)
        //    {
        //        string err = "";
        //        foreach (var eve in e.EntityValidationErrors)
        //        {
        //            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
        //                eve.Entry.Entity.GetType().Name, eve.Entry.State);
        //            foreach (var ve in eve.ValidationErrors)
        //            {
        //                err += ve.ErrorMessage;
        //                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
        //                    ve.PropertyName, ve.ErrorMessage);
        //            }
        //        }
        //        return jss.Serialize(err);
        //    }
        //}
        //public string ValidarFecha(string json)
        //{
        //    dynamic data = jss.DeserializeObject(json);
        //    string fecha = data["fecha"];
        //    int id = Convert.ToInt32(data["id_prestamo"]);
        //    DateTime f = Convert.ToDateTime(fecha);

        //    int x = conn.pagos.Where(r => r.fecha_pago.Equals(f) && r.prestamo_id.Equals(id)).Count();
        //    return jss.Serialize(x > 0);
        //}
        //public string ReadRutaAnt(string json)
        //{
        //    dynamic data = jss.DeserializeObject(json);
        //    string fecha = data["fecha"];

        //    string stringCon = System.Configuration.ConfigurationManager.ConnectionStrings["rhinoEntities"].ConnectionString;
        //    string[] vec = stringCon.Split(';');
        //    string preStrCon = vec[2].Substring(28) + ";" + vec[3] + ";" + vec[4] + ";" + vec[5] + ";" + vec[6];
        //    using (SqlConnection con = new SqlConnection(preStrCon))
        //    {
        //        try
        //        {
        //            int rs = 0;
        //            con.Open();
        //            var commandStr = "";
        //            commandStr = "select vlr_cuota as vlr_cuotas,monto as vlr_prestado,r.abono as pago, * from ruta as r INNER JOIN clientes as c on  r.id_cliente = c.id_cliente where r.fecha_ruta = '" + fecha + "'";
        //            SqlCommand sql = new SqlCommand(commandStr, con);
        //            SqlDataReader read = sql.ExecuteReader();
        //            string str = WriteReaderToJSON(read);
        //            con.Close();
        //            return (str);
        //        }
        //        catch (DbEntityValidationException e)
        //        {
        //            string err = "";
        //            foreach (var eve in e.EntityValidationErrors)
        //            {
        //                Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
        //                    eve.Entry.Entity.GetType().Name, eve.Entry.State);
        //                foreach (var ve in eve.ValidationErrors)
        //                {
        //                    err += ve.ErrorMessage;
        //                    Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
        //                        ve.PropertyName, ve.ErrorMessage);
        //                }
        //            }
        //            return jss.Serialize(err);
        //        }
        //    }
        //}
        //public string ReadGastosDia(string json)
        //{
        //    dynamic data = jss.DeserializeObject(json);
        //    int id = data["id"];
        //    DateTime fecha = Convert.ToDateTime(data["fecha"]);
        //    gastos c = conn.gastos.Where(a => a.usu_id.Equals(id) && a.fecha.Equals(fecha)).FirstOrDefault();
        //    return jss.Serialize(c);
        //}
        //public string Read_Rel_Gastos(string json)
        //{
        //    dynamic data = jss.DeserializeObject(json);
        //    string[] ff = data["fecha"].Split('T');
        //    DateTime aux = Convert.ToDateTime(ff[0]);
        //    string aux2 = Convert.ToString(aux);
        //    string[] f = Convert.ToString(aux2).Split(' ');
        //    f = f[0].Split('/');
        //    string fecha = f[2] + '/' + f[1] + '/' + f[0];
        //    string query = "Select CONCAT(e.nombres,' ',e.apellidos) as empleado,costo,observacion,(CASE g.tipo_gasto WHEN 1 THEN 'No operacional' WHEN 2 THEN 'operacional'  END) as t_gasto,(select SUM(valor_pagar)  from pagos where fecha_pago ='" + fecha + "') as total   FROM gastos as g INNER JOIN empleado as e on e.id = g.emp_id where g.fecha ='" + fecha + "'";
        //    return consulta(query);
        //}
        //#endregion

        //#region ESTADO CUENTA
        //public string ReadTablaPrestamo(string json)
        //{
        //    dynamic data = jss.DeserializeObject(json);
        //    int id;
        //    id = Convert.ToInt32(data["id"]);

        //    string stringCon = System.Configuration.ConfigurationManager.ConnectionStrings["rhinoEntities"].ConnectionString;
        //    string[] vec = stringCon.Split(';');
        //    string preStrCon = vec[2].Substring(28) + ";" + vec[3] + ";" + vec[4] + ";" + vec[5] + ";" + vec[6];
        //    using (SqlConnection con = new SqlConnection(preStrCon))
        //    {
        //        try
        //        {
        //            int rs = 0;
        //            con.Open();
        //            var commandStr = "select fecha_prestamo,vlr_prestado,interes,ti.nombre as forma_pago,num_meses,total_pagar,vlr_cuotas from prestamo as p inner join tipo_interes ti on p.tipo_interes_id = ti.id_tipo_inte WHERE p.id_prestamo='" + id + "'";
        //            SqlCommand sql = new SqlCommand(commandStr, con);
        //            SqlDataReader read = sql.ExecuteReader();
        //            string str = WriteReaderToJSON(read);
        //            con.Close();
        //            return (str);
        //        }
        //        catch (DbEntityValidationException e)
        //        {
        //            string err = "";
        //            foreach (var eve in e.EntityValidationErrors)
        //            {
        //                Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
        //                    eve.Entry.Entity.GetType().Name, eve.Entry.State);
        //                foreach (var ve in eve.ValidationErrors)
        //                {
        //                    err += ve.ErrorMessage;
        //                    Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
        //                        ve.PropertyName, ve.ErrorMessage);
        //                }
        //            }
        //            return jss.Serialize(err);
        //        }
        //    }
        //}
        //#endregion

        #region empleados
        public string CreateEmpleados(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            string _json = data["json"];
            try
            {
                empleado c = jss.Deserialize<empleado>(_json);
                conn.empleado.Add(c);
                int rs = conn.SaveChanges();
                return jss.Serialize(rs > 0);
            }
            catch (DbEntityValidationException e)
            {
                string err = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        err += ve.ErrorMessage;
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return jss.Serialize(err);
            }
        }
        public string UpdateEmpleados(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            string _json = data["json"];
            int rs = 0;
            empleado c = jss.Deserialize<empleado>(_json);
            conn.Entry(c).State = System.Data.EntityState.Modified;
            rs = conn.SaveChanges();
            //logoUsuario02("MEDICOS", "ACTUALIZAR", codigo_usuario, rs);
            return jss.Serialize(rs > 0);
        }
        public string ReadEmpleados(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            int id = data["id"];
            empleado c = conn.empleado.Where(a => a.id.Equals(id)).FirstOrDefault();
            return jss.Serialize(c);
        }
        public string DeleteEmpleados(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            int id = data["id"];
            empleado empleado = conn.empleado.Where(a => a.id.Equals(id)).FirstOrDefault();
            conn.empleado.Remove(empleado);
            int rs = conn.SaveChanges();
            return jss.Serialize(rs);
        }
        public string ValidateEmpleados(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            string cedula = Convert.ToString(data["cedula"]);
            bool u = false;
            u = conn.empleado.Where(z => z.cedula_emp.Equals(cedula)).Count() > 0;
            return jss.Serialize(u);
        }
        public string PreloadEmpresa()
        {
            List<object> arr = new List<object>();
            List<empresa> emp = conn.empresa.ToList();
            arr.Add(emp);
            return jss.Serialize(arr);
        }
        public string preload_jefes()
        {
            List<object> arr = new List<object>();
            List<jefes> j = conn.jefes.ToList();
            arr.Add(j);
            return jss.Serialize(arr);
        }
        public string preload_ccosto()
        {
            List<object> arr = new List<object>();
            List<area_ccosto> cc = conn.area_ccosto.ToList();
            arr.Add(cc);
            return jss.Serialize(arr);
        }

        #endregion

        #region ACTIVOS FIJOS
        public string CreateActivos(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            string _json = data["json"];
            dynamic head = jss.DeserializeObject(data["json"]);

            try
            {
                objeto c = jss.Deserialize<objeto>(_json);
                c.estado = 1;
                conn.objeto.Add(c);
                if (Convert.ToInt32(head["tipo_articulo"]) == 2){
                    int id = Convert.ToInt32(head["id"]);
                    int id_pedido = Convert.ToInt32(head["id_pedido"]);
                    depreciacion d = conn.depreciacion.Where(a => a.id.Equals(id) && a.id_pedido.Equals(id_pedido)).FirstOrDefault();
                    if(d != null) { 
                    d.inventario = 1;
                    d.placa_af = head["af"];
                        d.cuota = Convert.ToInt32(head["valor"]) / Convert.ToInt32(head["v_util"]);
                        conn.Entry(d).State = System.Data.EntityState.Modified;
                    }
                }

                int rs = conn.SaveChanges();
                return jss.Serialize(rs > 0);
            }
            catch (DbEntityValidationException e)
            {
                string err = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        err += ve.ErrorMessage;
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return jss.Serialize(err);
            }
        }
        public string UpdateActivos(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            string _json = data["json"];
            dynamic head = jss.DeserializeObject(data["json"]);
            int rs = 0;
            objeto c = jss.Deserialize<objeto>(_json);
            conn.Entry(c).State = System.Data.EntityState.Modified;
            if (Convert.ToInt32(head["tipo_articulo"]) == 2)
            {
                string id = head["af"];
                depreciacion d = conn.depreciacion.Where(a => a.placa_af.Equals(id)).FirstOrDefault();
                if (d != null)
                {
                    d.valor = Convert.ToInt32(head["valor"]);
                    d.cuota = Convert.ToInt32(head["valor"]) / d.v_util;
                    conn.Entry(d).State = System.Data.EntityState.Modified;
                }
            }
            rs = conn.SaveChanges();

            return jss.Serialize(rs > 0);
        }

        /* ingresado por shi*/
        public string UpdateComodato(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            string _json = data["json"];
            dynamic head = jss.DeserializeObject(data["json"]);
            int rs = 0;
            objeto c = jss.Deserialize<objeto>(_json);
            conn.Entry(c).State = System.Data.EntityState.Modified;
            if (Convert.ToInt32(head["tipo_articulo"]) == 2)
            {
                string id = head["af"];
                depreciacion d = conn.depreciacion.Where(a => a.placa_af.Equals(id)).FirstOrDefault();
                if (d != null)
                {
                    d.valor = Convert.ToInt32(head["valor"]);
                    d.cuota = Convert.ToInt32(head["valor"]) / d.v_util;
                    conn.Entry(d).State = System.Data.EntityState.Modified;
                }
            }
            rs = conn.SaveChanges();

            return jss.Serialize(rs > 0);
        }


        /*hasta aqui*/

        public string ReadActivos(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            int id = data["id"];
            objeto c = conn.objeto.Where(a => a.id.Equals(id)).FirstOrDefault();
            return jss.Serialize(c);
        }
        public string DeleteActivos(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            int id = data["id"];
            empleado empleado = conn.empleado.Where(a => a.id.Equals(id)).FirstOrDefault();
            conn.empleado.Remove(empleado);
            int rs = conn.SaveChanges();
            return jss.Serialize(rs);
        }
        public string Validateimei(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            string imei = Convert.ToString(data["imei"]);
            bool u = false;
            u = conn.objeto.Where(z => z.imei.Equals(imei)).Count() > 0;
            return jss.Serialize(u);
        }
        #endregion

        #region ENTREGAS
        public string create_entrega(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            string _json = data["json"];
            dynamic list = jss.DeserializeObject(data["json2"]);
            string tipo = Convert.ToString(data["tipo"]);
            dynamic head = jss.DeserializeObject(data["json"]);
            int id_usuario = data["log_usuario"]; 
            int rs = 0;
            try
            {
                if(tipo == "1" || tipo == "5") { 
                entregas c = jss.Deserialize<entregas>(_json);
                entregas e = conn.entregas.OrderByDescending(y => y.id_ent).FirstOrDefault();
                if(e == null)
                {
                    c.id_ent = 0;
                }
                else { 
                c.id_ent = e.id_ent + 1;
                }
                    c.estado = Convert.ToInt32(tipo);
                    conn.entregas.Add(c);
                    rs = conn.SaveChanges();
                    foreach (dynamic item in list)
                {
                    detalle_entrega r = new detalle_entrega();

                    r.id_ent = c.id_ent;
                    r.elemento = item["elemento"];
                    r.marca = item["marca"];
                    r.placa_af = item["placa_af"];
                    r.estado = item["estado"];
                    r.cantidad = item["cantidad"];
                    r.observacion = item["observacion"];
                    r.imei_inv = item["imei_inv"];
                        objeto o = conn.objeto.Where(p => p.af.Equals(r.placa_af)).FirstOrDefault();
                        if(o != null){
                            o.estado = 0;
                            conn.Entry(o).State = System.Data.EntityState.Modified;
                        }
                        else 
                        {
                            objeto obj = conn.objeto.Where(p => p.imei.Equals(r.placa_af)).FirstOrDefault();
                            obj.estado = 0;
                            conn.Entry(obj).State = System.Data.EntityState.Modified;
                        }
                   
                     int a  = conn.SaveChanges();

                     conn.detalle_entrega.Add(r);
                }
                    log_user(1, c.id_ent, id_usuario, head["hora"]);
                    return jss.Serialize(rs > 0);
                }
                else if(tipo == "2")
                {
                    devoluciones x = jss.Deserialize<devoluciones>(_json);
                    devoluciones e = conn.devoluciones.OrderByDescending(y => y.id_dev).FirstOrDefault();
                    if (e == null)
                    {
                        x.id_dev = 0;
                    }
                    else
                    {
                        x.id_dev = e.id_dev + 1;
                    }
                    x.estado = 1;
                    foreach (dynamic item in list)
                    {
                        detalle_devolucion r = new detalle_devolucion();

                        r.id_dev = x.id_dev;
                        r.elemento = item["elemento"];
                        r.marca = item["marca"];
                        r.placa_af = item["placa_af"];
                        r.estado = item["estado"];
                        r.cantidad = item["cantidad"];
                        r.observacion = item["observacion"];
                        r.imei_inv = item["imei_inv"];

                        objeto o = conn.objeto.Where(p => p.af.Equals(r.placa_af)).FirstOrDefault();
                        o.estado = 1;
                        conn.Entry(o).State = System.Data.EntityState.Modified;

                        conn.detalle_devolucion.Add(r);
                    }
                    conn.devoluciones.Add(x);
                     rs = conn.SaveChanges();
                    log_user(2, x.id_dev, id_usuario, head["hora"]);
                    return jss.Serialize(rs > 0);
                }
                return jss.Serialize(rs > 0);
            }
            catch (DbEntityValidationException e)
            {
                string err = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        err += ve.ErrorMessage;
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return jss.Serialize(err);
            }
        }
        public string ReadEntregas(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            string cedula = data["ced_empl"];
            int tipo_objeto = data["tipo_objeto"];
            entregas e = conn.entregas.Where(a => a.ced_empl.Equals(cedula)).FirstOrDefault();
            var ent = conn.entregas.Where(a => a.ced_empl.Equals(cedula)).ToList();
            int rs = 0;
            foreach (var item in ent)
            {
                
                detalle_entrega de = conn.detalle_entrega.Where(a => a.id_ent.Equals(item.id_ent)).FirstOrDefault();
                if (de != null)
                {
                    objeto o = conn.objeto.Where(a => a.af.Equals(de.placa_af)).FirstOrDefault();

                    if (o != null)
                    {
                        if (o.tipo == tipo_objeto)
                        {
                            rs = 1;
                            return jss.Serialize(rs);
                        }
                    }
                }
            }

            return jss.Serialize(rs);
        }
        public string Searchreport1(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            List<object> arr = new List<object>();
            int tipo_objeto = Convert.ToInt32(data["tipo"]);
            var obj = conn.objeto.ToList();
            //if (tipo_objeto != 5) { 
              obj = conn.objeto.Where(a => a.tipo_articulo == 2).ToList();
            //}
            int sum = 0;
            foreach (var item in obj)
            {

                reporte_activos r1 = new reporte_activos();
                if (item.estado == 0)
                {
                    r1.Estado = "ENTREGADO";

                    detalle_entrega de = conn.detalle_entrega.OrderByDescending(y => y.id_ent).Where(a => a.placa_af.Equals(item.af)).FirstOrDefault();

                    entregas e = conn.entregas.Where(a => a.id_ent.Equals(de.id_ent)).FirstOrDefault();

                    if (e != null) { 
                    empleado emp = conn.empleado.Where(a => a.cedula_emp.Equals(e.ced_empl)).FirstOrDefault();

                    r1.Usuario = emp.nombre + " " + emp.snombre + " " + emp.ppellido + " " + emp.spellido;
                    r1.Cedula = emp.cedula_emp;
                    }
                }
                else if (item.estado == 1)
                {
                    r1.Estado = "DISPONIBLE";
                }
                else if (item.estado == 3)
                {
                    r1.Estado = "BAJA";
                }
                else if (item.estado == 4)
                {
                    r1.Estado = "HURTO";
                }
                else if (item.estado == 5)
                {
                    r1.Estado = "REPARACION";
                }
                //VALIDAR TIPO DE ACTIVO
                r1.Serial = item.imei;
                r1.Placa = item.af;
                r1.Descripcion = item.descripcion;
                r1.Cod_articulo = item.cod_articulo;
                r1.Factura = item.factura;
                r1.Valor = item.valor;
                r1.Centro_costo = item.centro_costo;
                r1.Observacion = item.observacion;
                r1.Causacion = Convert.ToDateTime(item.causacion);
                r1.Fecha_estado = Convert.ToDateTime(item.fecha_estado);
                if(item.tipo == 5)
                {
                    r1.Tipo = "COMPUTO";
                }
                else if(item.tipo == 4)
                {
                    r1.Tipo = "HERRAMIENTAS";
                }
                else if (item.tipo == 6)
                {
                    r1.Tipo = "MUEBLES Y ENSERES";
                }
                else if (item.tipo == 7)
                {
                    r1.Tipo = "COMUNICACION";
                }
                else if (item.tipo == 8)
                {
                    r1.Tipo = "EDIFICACIONES";
                }
                else if (item.tipo == 9)
                {
                    r1.Tipo = "LICENCIAS";
                }
                arr.Add(r1);

            }


            return jss.Serialize(arr);

        }
        public string Read_Acta(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            int tipo = data["tipo"];
            int id = data["id"];

            List<object> arr = new List<object>();
            if(tipo == 1)
            {
                entregas e = conn.entregas.Where(a => a.id_ent.Equals(id)).FirstOrDefault();
                if (e != null) { 
                var de = conn.detalle_entrega.Where(a => a.id_ent.Equals(e.id_ent)).ToList();
                    arr.Add(e);
                    arr.Add(de);
                }
            }
            else if (tipo == 2)
            {
                devoluciones d = conn.devoluciones.Where(a => a.id_dev.Equals(id)).FirstOrDefault();
                if (d != null)
                {
                    var dd = conn.detalle_devolucion.Where(a => a.id_dev.Equals(d.id_dev)).ToList();
                    arr.Add(d);
                    arr.Add(dd);
                }
            }

            return jss.Serialize(arr);
        }
        public string update_entrega(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            string _json = data["json"];
            dynamic list = jss.DeserializeObject(data["json2"]);
            string tipo = Convert.ToString(data["tipo"]);
            dynamic head = jss.DeserializeObject(data["json"]);
            int id_usuario = data["log_usuario"];
            int rs = 0;
            try
            {
                if (tipo == "1")
                {
                    entregas c = jss.Deserialize<entregas>(_json);

                    c.estado = 1;
                    conn.Entry(c).State = System.Data.EntityState.Modified;
                    var de = conn.detalle_entrega.Where(a => a.id_ent.Equals(c.id_ent)).ToList();
                    foreach (dynamic item in de)
                    {
                        conn.detalle_entrega.Remove(item);
                        rs = conn.SaveChanges();
                    }
                    foreach (dynamic item in list)
                    {
                        detalle_entrega r = new detalle_entrega();

                        r.id_ent = c.id_ent;
                        r.elemento = item["elemento"];
                        r.marca = item["marca"];
                        r.placa_af = item["placa_af"];
                        r.estado = item["estado"];
                        r.cantidad = item["cantidad"];
                        r.observacion = item["observacion"];
                        r.imei_inv = item["imei_inv"];
                        objeto o = conn.objeto.Where(p => p.af.Equals(r.placa_af)).FirstOrDefault();
                        o.estado = 0;
                        conn.Entry(o).State = System.Data.EntityState.Modified;
                        conn.detalle_entrega.Add(r);
                        int a = conn.SaveChanges();


                    }
                    log_user(1, c.id_ent, id_usuario, head["hora"]);
                    return jss.Serialize(rs > 0);
                }
                else if (tipo == "2")
                {
                    devoluciones x = jss.Deserialize<devoluciones>(_json);
                    x.estado = 1;
                    var de = conn.detalle_devolucion.Where(a => a.id_dev.Equals(x.id_dev)).ToList();
                    foreach (dynamic item in de)
                    {
                        conn.detalle_devolucion.Remove(item);
                        rs = conn.SaveChanges();
                    }
                    foreach (dynamic item in list)
                    {
                        detalle_devolucion r = new detalle_devolucion();

                        r.id_dev = x.id_dev;
                        r.elemento = item["elemento"];
                        r.marca = item["marca"];
                        r.placa_af = item["placa_af"];
                        r.estado = item["estado"];
                        r.cantidad = item["cantidad"];
                        r.observacion = item["observacion"];
                        r.imei_inv = item["imei_inv"];
                        objeto o = conn.objeto.Where(p => p.af.Equals(r.placa_af)).FirstOrDefault();
                        o.estado = 1;
                        conn.Entry(o).State = System.Data.EntityState.Modified;

                        conn.detalle_devolucion.Add(r);
                        int a = conn.SaveChanges();
                    }
                    conn.Entry(x).State = System.Data.EntityState.Modified;
                    rs = conn.SaveChanges();
                    log_user(2, x.id_dev, id_usuario, head["hora"]);
                    return jss.Serialize(rs > 0);
                }
                return jss.Serialize(rs > 0);
            }
            catch (DbEntityValidationException e)
            {
                string err = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        err += ve.ErrorMessage;
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return jss.Serialize(err);
            }
        }
        public string ReadObjeto(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            string activo = data["activo"];
            objeto c = conn.objeto.Where(a => a.af.Equals(activo)).FirstOrDefault();
            return jss.Serialize(c);
        }

        public string ReadConsumibles(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            string parametro = Convert.ToString(data["activo"]);
            objeto c = conn.objeto.Where(a => a.imei.Equals(parametro) && a.tipo_articulo == 1).FirstOrDefault();
            articulos art = conn.articulos.Where(a => a.codigo.Equals(c.cod_articulo)).FirstOrDefault();
            List<object> arr = new List<object>();


            if (c == null){
                objeto af = conn.objeto.Where(a => a.af.Equals(parametro) && a.tipo_articulo.Equals(1)).FirstOrDefault();
                articulos ar = conn.articulos.Where(a => a.codigo.Equals(c.cod_articulo)).FirstOrDefault();
                arr.Add(af);
                arr.Add(ar);
                return jss.Serialize(arr);
            }else {
                arr.Add(c);
                arr.Add(art);
                return jss.Serialize(arr);
            }
        }

        public string ReporteConsumibles(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            List<object> arr = new List<object>();
            int tipo_objeto = Convert.ToInt32(data["tipo"]);
            var obj = conn.objeto.Where(a => a.tipo_articulo == 1).ToList();
            if (tipo_objeto != 5)
            {
                obj = conn.objeto.Where(a => a.tipo.Equals(tipo_objeto) && a.tipo_articulo == 1 && a.estado != 7).ToList();
            }
            int sum = 0;
            foreach (var item in obj)
            {

                report_tablet r1 = new report_tablet();
                if (item.estado == 0 || item.estado == 4)
                {
                    if(item.estado == 0) { 
                        r1.Estado = "ENTREGADO";
                    }
                    else if(item.estado == 4)
                    {
                        r1.Estado = "HURTO";
                    }

                    detalle_entrega de = conn.detalle_entrega.OrderByDescending(y => y.id_ent).Where(a => a.imei_inv.Equals(item.imei)).FirstOrDefault();

                    if( de != null) { 
                    entregas e = conn.entregas.Where(a => a.id_ent.Equals(de.id_ent)).FirstOrDefault();

                    if (e != null)
                    {
                        empleado emp = conn.empleado.Where(a => a.cedula_emp.Equals(e.ced_empl)).FirstOrDefault();

                        r1.Usuario = emp.nombre + " " + emp.snombre + " " + emp.ppellido + " " + emp.spellido;
                        r1.Cedula = emp.cedula_emp;
                        r1.Observacion_entrega = e.observacion;
                    }
                    }
                }
                else if (item.estado == 1)
                {
                    r1.Estado = "DISPONIBLE";
                }
                else if (item.estado == 3)
                {
                    r1.Estado = "BAJA";
                }
                else if (item.estado == 5)
                {
                    r1.Estado = "REPARACION";
                }
                //VALIDAR TIPO DE ACTIVO
                r1.Imei = item.imei;
                r1.Placa = item.af;
                r1.Item = sum + 1;
                r1.Id = item.id;
                r1.Descripcion = item.descripcion;
                r1.Linea = item.linea;
                r1.Linea_activa = item.linea_activa;
                r1.Nuevo_imei = item.nuevo_imei;
                r1.Factura = item.factura;
                r1.Valor = item.valor;
                r1.Centro_costo = item.centro_costo;
                r1.Cod_articulo = item.cod_articulo;
                if(item.causacion != null) { 
                r1.Causacion = Convert.ToDateTime(item.causacion);
                }
                if (item.fecha_estado != null)
                {
                    r1.Fecha_estado = Convert.ToDateTime(item.fecha_estado);
                }
                r1.Observacion = item.observacion;
                descuentos des = conn.descuentos.Where(a => a.imei_articulo.Equals(r1.Imei)).FirstOrDefault();
                if (des != null)
                {
                    r1.Descuento = Convert.ToInt32(des.valor);
                    r1.Motivo_desc = des.motivo;
                    r1.Forma_desc = des.forma_desc;

                }
                arr.Add(r1);

            }


            return jss.Serialize(arr);

        }
        #endregion

        #region ACTA
        public string create_acta(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            string _json = data["json"];
            dynamic list = jss.DeserializeObject(data["json2"]);
            string tipo = Convert.ToString(data["tipo"]);
            dynamic head = jss.DeserializeObject(data["json"]);
            int id_usuario = data["log_usuario"];
            int rs = 0;
            try
            {
                if (tipo == "1" || tipo == "5")
                {
                    entregas c = jss.Deserialize<entregas>(_json);
                    entregas e = conn.entregas.OrderByDescending(y => y.id_ent).FirstOrDefault();
                    if (e == null)
                    {
                        c.id_ent = 0;
                    }
                    else
                    {
                        c.id_ent = e.id_ent + 1;
                    }
                    c.estado =  Convert.ToInt32(tipo);
                    conn.entregas.Add(c);
                    rs = conn.SaveChanges();
                    foreach (dynamic item in list)
                    {
                        detalle_entrega r = new detalle_entrega();

                        r.id_ent = c.id_ent;
                        r.elemento = item["elemento"];
                        r.marca = item["marca"];
                        r.placa_af = "";
                        r.estado = item["estado"];
                        r.cantidad = item["cantidad"];
                        r.observacion = "";
                        r.imei_inv = item["imei_inv"];
                        r.c = item["c"];
                        r.l = item["l"];
                        r.a = item["a"];
                        r.v = item["v"];
                        r.f = item["f"];
                        objeto o = conn.objeto.Where(p => p.imei.Equals(r.imei_inv)).FirstOrDefault();
                        if (o != null)
                        {
                            o.estado = 0;
                            conn.Entry(o).State = System.Data.EntityState.Modified;
                        }
                        else
                        {
                            objeto obj = conn.objeto.Where(p => p.imei.Equals(r.imei_inv)).FirstOrDefault();
                            obj.estado = 0;
                            conn.Entry(obj).State = System.Data.EntityState.Modified;
                        }

                        int a = conn.SaveChanges();

                        conn.detalle_entrega.Add(r);
                    }
                    log_user(1, c.id_ent, id_usuario, head["hora"]);
                    return jss.Serialize(rs > 0);
                }
                else if (tipo == "2" || tipo == "3" )
                {
                    devoluciones x = jss.Deserialize<devoluciones>(_json);
                    devoluciones e = conn.devoluciones.OrderByDescending(y => y.id_dev).FirstOrDefault();
                    if (e == null)
                    {
                        x.id_dev = 0;
                    }
                    else
                    {
                        x.id_dev = e.id_dev + 1;
                    }
                    x.estado = 1;
                    foreach (dynamic item in list)
                    {
                        detalle_devolucion r = new detalle_devolucion();

                        r.id_dev = x.id_dev;
                        r.elemento = item["elemento"];
                        r.marca = item["marca"];
                        r.placa_af = "";
                        r.estado = item["estado"];
                        r.cantidad = item["cantidad"];
                        r.observacion = "";
                        r.imei_inv = item["imei_inv"];
                        r.c = item["c"];
                        r.l = item["l"];
                        r.a = item["a"];
                        r.v = item["v"];
                        r.f = item["f"];
                        objeto o = conn.objeto.Where(p => p.imei.Equals(r.imei_inv)).FirstOrDefault();
                        o.estado = 1;
                        conn.Entry(o).State = System.Data.EntityState.Modified;

                        conn.detalle_devolucion.Add(r);
                    }
                    conn.devoluciones.Add(x);
                    rs = conn.SaveChanges();
                    if( tipo == "3")
                    {
                        entregas c = jss.Deserialize<entregas>(_json);
                        entregas en = conn.entregas.OrderByDescending(y => y.id_ent).FirstOrDefault();
                        if (en == null)
                        {
                            c.id_ent = 0;
                        }
                        else
                        {
                            c.id_ent = en.id_ent + 1;
                        }
                        c.estado = Convert.ToInt32(tipo);
                        //en.ced_empl = head["ced_empl_tras"];
                        c.ced_empl = head["ced_empl_tras"];
                        c.autoriza = head["autoriza_tras"];
                        conn.entregas.Add(c);
                        rs = conn.SaveChanges();
                        foreach (dynamic item in list)
                        {
                            detalle_entrega r = new detalle_entrega();

                            r.id_ent = c.id_ent;
                            r.elemento = item["elemento"];
                            r.marca = item["marca"];
                            r.placa_af = "";
                            r.estado = item["estado"];
                            r.cantidad = item["cantidad"];
                            r.observacion = "";
                            r.imei_inv = item["imei_inv"];
                            r.c = item["c"];
                            r.l = item["l"];
                            r.a = item["a"];
                            r.v = item["v"];
                            r.f = item["f"];
                            objeto o = conn.objeto.Where(p => p.imei.Equals(r.imei_inv)).FirstOrDefault();
                            if (o != null)
                            {
                                o.estado = 0;
                                conn.Entry(o).State = System.Data.EntityState.Modified;
                            }
                            else
                            {
                                objeto obj = conn.objeto.Where(p => p.imei.Equals(r.imei_inv)).FirstOrDefault();
                                obj.estado = 0;
                                conn.Entry(obj).State = System.Data.EntityState.Modified;
                            }

                            int a = conn.SaveChanges();

                            conn.detalle_entrega.Add(r);
                        }
                    }
                    log_user(2, x.id_dev, id_usuario, head["hora"]);
                    return jss.Serialize(rs > 0);
                }

                return jss.Serialize(rs > 0);
            }
            catch (DbEntityValidationException e)
            {
                string err = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        err += ve.ErrorMessage;
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return jss.Serialize(err);
            }
        }

        public string update_consumibles(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            string _json = data["json"];
            dynamic list = jss.DeserializeObject(data["json2"]);
            string tipo = Convert.ToString(data["tipo"]);
            dynamic head = jss.DeserializeObject(data["json"]);
            int id_usuario = data["log_usuario"];
            int rs = 0;
            try
            {
                if (tipo == "1")
                {
                    entregas c = jss.Deserialize<entregas>(_json);

                    c.estado = 1;
                    conn.Entry(c).State = System.Data.EntityState.Modified;
                    var de = conn.detalle_entrega.Where(a => a.id_ent.Equals(c.id_ent)).ToList();
                    foreach (dynamic item in de)
                    {
                        conn.detalle_entrega.Remove(item);
                        rs = conn.SaveChanges();
                    }
                    foreach (dynamic item in list)
                    {
                        detalle_entrega r = new detalle_entrega();

                        r.id_ent = c.id_ent;
                        r.elemento = item["elemento"];
                        r.marca = item["marca"];
                        r.placa_af = "";
                        r.estado = item["estado"];
                        r.cantidad = item["cantidad"];
                        r.observacion = "";
                        r.imei_inv = item["imei_inv"];
                        r.c = item["c"];
                        r.l = item["l"];
                        r.a = item["a"];
                        r.v = item["v"];
                        r.f = item["f"];

                        objeto o = conn.objeto.Where(p => p.af.Equals(r.placa_af)).FirstOrDefault();
                        o.estado = 0;
                        conn.Entry(o).State = System.Data.EntityState.Modified;
                        conn.detalle_entrega.Add(r);
                        int a = conn.SaveChanges();


                    }
                    log_user(1, c.id_ent, id_usuario, head["hora"]);
                    return jss.Serialize(rs > 0);
                }
                else if (tipo == "2")
                {
                    devoluciones x = jss.Deserialize<devoluciones>(_json);
                    x.estado = 1;
                    var de = conn.detalle_devolucion.Where(a => a.id_dev.Equals(x.id_dev)).ToList();
                    foreach (dynamic item in de)
                    {
                        conn.detalle_devolucion.Remove(item);
                        rs = conn.SaveChanges();
                    }
                    foreach (dynamic item in list)
                    {
                        detalle_devolucion r = new detalle_devolucion();

                        r.id_dev = x.id_dev;
                        r.elemento = item["elemento"];
                        r.marca = item["marca"];
                        r.placa_af = "";
                        r.estado = item["estado"];
                        r.cantidad = item["cantidad"];
                        r.observacion = "";
                        r.imei_inv = item["imei_inv"];
                        r.c = item["c"];
                        r.l = item["l"];
                        r.a = item["a"];
                        r.v = item["v"];
                        r.f = item["f"];

                        objeto o = conn.objeto.Where(p => p.af.Equals(r.placa_af)).FirstOrDefault();
                        o.estado = 1;
                        conn.Entry(o).State = System.Data.EntityState.Modified;

                        conn.detalle_devolucion.Add(r);
                        int a = conn.SaveChanges();
                    }
                    conn.Entry(x).State = System.Data.EntityState.Modified;
                    rs = conn.SaveChanges();
                    log_user(2, x.id_dev, id_usuario, head["hora"]);
                    return jss.Serialize(rs > 0);
                }
                return jss.Serialize(rs > 0);
            }
            catch (DbEntityValidationException e)
            {
                string err = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        err += ve.ErrorMessage;
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return jss.Serialize(err);
            }
        }
        #endregion

        #region
        public string ReadActa_entrega(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            string cedula = data["ced_empl"];
            DateTime fecha = Convert.ToDateTime(data["fecha"]);
            string hora = data["hora"];
            List<object> arr = new List<object>();
            empleado emp = conn.empleado.Where(a => a.cedula_emp.Equals(cedula)).FirstOrDefault();
            empresa c = conn.empresa.Where(a => a.id.Equals(emp.empresa)).FirstOrDefault();
            entregas e = conn.entregas.Where(a => a.ced_empl.Equals(cedula) && a.fecha.Equals(fecha) && a.hora.Equals(hora)).FirstOrDefault();
            var de = conn.detalle_entrega.Where(a => a.id_ent.Equals(e.id_ent)).ToList();

            arr.Add(e);
            arr.Add(de);
            arr.Add(emp);
            arr.Add(c);

            return jss.Serialize(arr);
        }

        public string ReadActa_devolucion(string json)
        {
            dynamic data = jss.DeserializeObject(json);

            int id = Convert.ToInt32(data["id"]); 
            List<object> arr = new List<object>();
            devoluciones e = conn.devoluciones.Where(a => a.id_dev.Equals(id)).FirstOrDefault();
            empleado emp = conn.empleado.Where(a => a.cedula_emp.Equals(e.ced_empl)).FirstOrDefault();
            empresa c = conn.empresa.Where(a => a.id.Equals(emp.empresa)).FirstOrDefault();
            
            var de = conn.detalle_devolucion.Where(a => a.id_dev.Equals(e.id_dev)).ToList();

            arr.Add(e);
            arr.Add(de);
            arr.Add(emp);
            arr.Add(c);

            return jss.Serialize(arr);
        }
        #endregion

        #region DESCUENTOS
        public string Createdescuento(string json)
        {
            try
            {
                dynamic data = jss.DeserializeObject(json);
                string _json = data["json"];
                descuentos des = jss.Deserialize<descuentos>(_json);
                if(des.tipo_desc == 1)
                {
                    objeto c = conn.objeto.Where(a => a.imei.Equals(des.imei_articulo)).FirstOrDefault();
                    c.observacion = "DESCUENTO POR " + des.forma_desc + " $ " + des.valor; 
                    conn.Entry(c).State = System.Data.EntityState.Modified;
                } 

                conn.descuentos.Add(des);
                int rs = conn.SaveChanges();
                //log_User("CREAR USUARIO", "GUARDAR", codigo_usuario, rs);
                return jss.Serialize(rs > 0);
            }
            catch (DbEntityValidationException e)
            {
                string err = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        err += ve.ErrorMessage;
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return jss.Serialize("{err:" + err + "}");
            }
        }
        #endregion

        public string CreateEquipo(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            string _json = data["json"];
            try
            {
                bodegasst e = jss.Deserialize<bodegasst>(_json);
                e.estado = 1;
                conn.bodegasst.Add(e);
                int rs = conn.SaveChanges();
                return jss.Serialize(rs > 0);
            }
            catch (DbEntityValidationException e)
            {
                string err = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        err += ve.ErrorMessage;
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return jss.Serialize(err);
            }
        }

        public string Create_HV_Altura(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            string _json = data["json"];
            dynamic list = jss.DeserializeObject(data["json2"]);
            try
            {
                hv_altura c = jss.Deserialize<hv_altura>(_json);
                conn.hv_altura.Add(c);
                int a = conn.SaveChanges();
                foreach (dynamic item in list)
                {
                    detalle_usu_alt r = new detalle_usu_alt();

                    r.id_hv = c.id;
                    r.cedula = Convert.ToString(item["cedula"]);
                    r.codigo = 0;
                    r.nombre = item["nombre"];
                    r.cargo = item["cargo"];
                    if(item["f_entrega"] != "")
                    { 
                        r.f_entrega = Convert.ToDateTime(item["f_entrega"]);
                    }
                    if (item["f_devolucion"] != "")
                    {
                        r.f_devolucion = Convert.ToDateTime(item["f_devolucion"]);
                    }

                    int b  = conn.SaveChanges();

                     conn.detalle_usu_alt.Add(r);
                }
                int rs = conn.SaveChanges();
                return jss.Serialize(rs > 0);
            }
            catch (DbEntityValidationException e)
            {
                string err = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        err += ve.ErrorMessage;
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return jss.Serialize(err);
            }
        }

        public string UpdateHv_equipos(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            dynamic list = jss.DeserializeObject(data["grilla"]);
            string _json = data["json"];
            int rs = 0;

            try
            {
                    hv_altura h = jss.Deserialize<hv_altura>(_json);
                    int validation = conn.hv_altura.Where(n => n.tipo.Equals(h.tipo) && n.codigo.Equals(h.codigo) && n.serial.Equals(h.serial) && n.lote.Equals(h.lote)).Count();
                hv_altura bh = conn.hv_altura.Where(n => n.tipo.Equals(h.tipo) && n.codigo.Equals(h.codigo) && n.serial.Equals(h.serial) && n.lote.Equals(h.lote)).FirstOrDefault();
                if (validation <= 0)
                    {
                    conn.hv_altura.Add(h);
                    int a = conn.SaveChanges();
                    foreach (dynamic item in list)
                    {
                        detalle_usu_alt r = new detalle_usu_alt();

                        r.id_hv = h.id;
                        r.cedula = Convert.ToString(item["cedula"]);
                        r.codigo = 0;
                        r.nombre = item["nombre"];
                        r.cargo = item["cargo"];
                        if (item["f_entrega"] != null)
                        {
                            if (item["f_entrega"] != "") { 
                                r.f_entrega = Convert.ToDateTime(item["f_entrega"]);
                            }
                        }
                        if (item["f_devolucion"] != null)
                        {
                            if (item["f_devolucion"] != "") { 
                                r.f_devolucion = Convert.ToDateTime(item["f_devolucion"]);
                            }
                        }
                       
                        
                        conn.detalle_usu_alt.Add(r);
                        bodegasst bsst = conn.bodegasst.Where(g => g.codigo.Equals(h.codigo) && g.serial.Equals(h.serial) && g.tipo == h.tipo).FirstOrDefault();
                        bsst.cedula_asig = r.cedula;
                        conn.Entry(bsst).State = System.Data.EntityState.Modified;
                       rs = conn.SaveChanges();


                    }
                } else {

                    conn.Entry(bh).State = System.Data.EntityState.Modified;
                    var du = conn.detalle_usu_alt.Where(a => a.id_hv.Equals(bh.id)).ToList();
                    if(du != null)
                    {
                        foreach(dynamic item in du)
                        {
                            conn.detalle_usu_alt.Remove(item);
                            rs = conn.SaveChanges();
                        }
                      
                    }

                    foreach (dynamic item in list)
                    {
                        detalle_usu_alt r = new detalle_usu_alt();

                        r.id_hv = bh.id;
                        r.cedula = Convert.ToString(item["cedula"]);
                        r.codigo = 0;
                        r.nombre = item["nombre"];
                        r.cargo = item["cargo"];
                        if (item["f_entrega"] != "")
                        {
                            r.f_entrega = Convert.ToDateTime(item["f_entrega"]);
                        }
                        if (item["f_devolucion"] != "")
                        {
                            r.f_devolucion = Convert.ToDateTime(item["f_devolucion"]);
                        }

                        conn.detalle_usu_alt.Add(r);
                        bodegasst bsst = conn.bodegasst.Where(g => g.codigo.Equals(h.codigo) && g.serial.Equals(h.serial) && g.tipo == h.tipo).FirstOrDefault();
                        bsst.cedula_asig = r.cedula;
                        conn.Entry(bsst).State = System.Data.EntityState.Modified;
                        rs = conn.SaveChanges();


                    }
                }

                    //log_user(2, x.id_dev, id_usuario, head["hora"]);
                    return jss.Serialize(rs > 0);
            }
            catch (DbEntityValidationException e)
            {
                string err = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        err += ve.ErrorMessage;
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return jss.Serialize(err);
            }
        }

        public string CreateInspeccionaltura(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            string _json = data["json"];
            try
            {
                insp_equi_altura c = jss.Deserialize<insp_equi_altura>(_json);
                conn.insp_equi_altura.Add(c);
                int rs = conn.SaveChanges();
                return jss.Serialize(rs > 0);
            }
            catch (DbEntityValidationException e)
            {
                string err = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        err += ve.ErrorMessage;
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return jss.Serialize(err);
            }
        }
        public string ReadIspeccionAlt(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            int id = data["id"];
            insp_equi_altura e = conn.insp_equi_altura.Where(a => a.id.Equals(id)).FirstOrDefault();
            return jss.Serialize(e);
        }

        public string readempresa(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            int id_emp = Convert.ToInt32(data["id_emp"]);
            empresa c = conn.empresa.Where(a => a.id.Equals(id_emp)).FirstOrDefault();
            return jss.Serialize(c);
        }

        public string validatejefe(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            int id = Convert.ToInt32(data["id_autoriza"]);
            jefes c = conn.jefes.Where(a => a.id.Equals(id)).FirstOrDefault();
            return jss.Serialize(c);
        }

        public string Report_bodegasst(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            List<object> arr = new List<object>();
            var obj = conn.bodegasst.ToList();
            obj = conn.bodegasst.Where(a => a.estado == 1).ToList();
            int sum = 0;
            foreach (var item in obj)
            {

                reporte_bodegasst r1 = new reporte_bodegasst();
                if (item.cedula_asig != null )
                {
                    r1.Estado_asig = "ENTREGADO";

                      empleado emp = conn.empleado.Where(a => a.cedula_emp.Equals(item.cedula_asig)).FirstOrDefault();

                    if(emp != null) { 
                            r1.Empleado = emp.nombre + " " + emp.snombre + " " + emp.ppellido + " " + emp.spellido;
                            r1.Ccosto = Convert.ToString(emp.ccosto);
                    } else {
                        r1.Empleado = "NO REGISTRADO EN BD";
                    }
                    r1.Cedula_asig = item.cedula_asig;
                    
                }else {
                        r1.Estado_asig = "DISPONIBLE";
                        r1.Ccosto = "";
                      }
                
                //VALIDAR TIPO DE ACTIVO
                r1.Id = item.id;
                r1.Codigo = item.codigo;
                r1.Marca = item.marca;
                r1.Referencia = item.referencia;
                r1.Serial = item.serial;
                r1.Lote = item.lote;
                r1.Mes_fabricacion = Convert.ToInt32(item.mes_fabricacion);
                r1.Ano_fabricacion = Convert.ToInt32(item.ano_fabricacion);
                r1.F_compra = Convert.ToDateTime(item.f_compra);
                r1.Proveedor = item.proveedor;
                r1.Tipo = Convert.ToInt32(item.tipo);
                r1.Observaciones = item.observaciones;
                r1.Estado = item.estado;
                r1.F_inspeccion = r1.F_compra.AddYears(1);
               
                r1.F_prox_inspec = Convert.ToDateTime(item.f_prox_inspec);
                if (item.tipo == 1)
                {
                    r1.Tipo_equipo = "ARNES";
                    insp_equi_altura ie = conn.insp_equi_altura.OrderByDescending(y => y.f_inspeccion).Where(r => (r.arn_codigo1.Equals(item.codigo) || r.arn_codigo2.Equals(item.codigo)) && r.arn_serial1.Equals(item.serial) || (r.arn_serial2.Equals(item.serial))).FirstOrDefault();
                    if (ie != null) { r1.F_seguimiento = ie.f_inspeccion; r1.Res_inspeccion = ie.resp_inspeccion; }
                    
                }
                else if (item.tipo == 2)
                {
                    r1.Tipo_equipo = "ESLINGA P.";
                    insp_equi_altura ie = conn.insp_equi_altura.OrderByDescending(y => y.f_inspeccion).Where(r => r.cod_erc1.Equals(item.codigo) || r.cod_erc2.Equals(item.codigo) && r.serial_erc1.Equals(item.serial) || r.serial_erc1.Equals(item.serial)).FirstOrDefault();
                    if (ie != null) { r1.F_seguimiento = ie.f_inspeccion; r1.Res_inspeccion = ie.resp_inspeccion; }
                }
                else if (item.tipo == 3)
                {
                    r1.Tipo_equipo = "ESLINGA Y.";
                    insp_equi_altura ie = conn.insp_equi_altura.OrderByDescending(y => y.f_inspeccion).Where(r => r.codigo_eai1.Equals(item.codigo) || r.codigo_eai2.Equals(item.codigo) && r.serial_eai1.Equals(item.serial) || r.serial_eai2.Equals(item.serial)).FirstOrDefault();
                    if (ie != null) { r1.F_seguimiento = ie.f_inspeccion; r1.Res_inspeccion = ie.resp_inspeccion; }
                }
                else if (item.tipo == 4)
                {
                    r1.Tipo_equipo = "TIE OFF";
                    insp_equi_altura ie = conn.insp_equi_altura.OrderByDescending(y => y.f_inspeccion).Where(r => r.codigo_eai1.Equals(item.codigo) || r.codigo_eai2.Equals(item.codigo) && r.serial_eai1.Equals(item.serial) || r.serial_eai2.Equals(item.serial)).FirstOrDefault();
                    if (ie != null) { r1.F_seguimiento = ie.f_inspeccion; r1.Res_inspeccion = ie.resp_inspeccion; }
                }
                else if (item.tipo == 5)
                {
                    r1.Tipo_equipo = "ARRESTADOR";
                    insp_equi_altura ie = conn.insp_equi_altura.OrderByDescending(y => y.f_inspeccion).Where(r => (r.cod_arrest1.Equals(item.codigo) || r.cod_arrest2.Equals(item.codigo)) && (r.serial_arrest1.Equals(item.serial) || r.serial_arrest2.Equals(item.serial))).FirstOrDefault();
                    if (ie != null) { r1.F_seguimiento = ie.f_inspeccion; r1.Res_inspeccion = ie.resp_inspeccion; }
                }
                else if (item.tipo == 6)
                {
                    r1.Tipo_equipo = "LINEA DE VIDA";
                    insp_equi_altura ie = conn.insp_equi_altura.OrderByDescending(y => y.f_inspeccion).Where(r => r.codigo_lv1.Equals(item.codigo) || r.codigo_lv2.Equals(item.codigo) && r.serial_lv1.Equals(item.serial) || r.serial_lv2.Equals(item.serial)).FirstOrDefault();
                    if (ie != null) { r1.F_seguimiento = ie.f_inspeccion; r1.Res_inspeccion = ie.resp_inspeccion; }
                }
                else if (item.tipo == 7)
                {
                    r1.Tipo_equipo = "MOSQUETON";
                    insp_equi_altura ie = conn.insp_equi_altura.OrderByDescending(y => y.f_inspeccion).Where(r => (r.codigo_mosq1.Equals(item.codigo) || r.codigo_mosq2.Equals(item.codigo)) && (r.serial_mosq1.Equals(item.serial) || r.serial_mosq2.Equals(item.serial))).FirstOrDefault();
                    if (ie != null) { r1.F_seguimiento = ie.f_inspeccion; r1.Res_inspeccion = ie.resp_inspeccion; }
                }

                arr.Add(r1);

            }


            return jss.Serialize(arr);

        }

        public string Report_bodegasstfordate(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            DateTime startdate = Convert.ToDateTime(data["startdate"]);
            DateTime end_date = Convert.ToDateTime(data["enddate"]);
            List<object> arr = new List<object>();
            var obj = conn.bodegasst.ToList();
            obj = conn.bodegasst.Where(a => a.estado == 1 && a.f_prox_inspec >= startdate && a.f_prox_inspec <= end_date).ToList();
            foreach (var item in obj)
            {

                reporte_bodegasst r1 = new reporte_bodegasst();
                if (item.cedula_asig.Length != 0)
                {
                    r1.Estado_asig = "ENTREGADO";

                    empleado emp = conn.empleado.Where(a => a.cedula_emp.Equals(item.cedula_asig)).FirstOrDefault();

                    r1.Empleado = emp.nombre + " " + emp.snombre + " " + emp.ppellido + " " + emp.spellido;
                    r1.Cedula_asig = item.cedula_asig;

                }
                else
                {
                    r1.Estado_asig = "DISPONIBLE";
                }

                //VALIDAR TIPO DE ACTIVO
                r1.Id = item.id;
                r1.Codigo = item.codigo;
                r1.Marca = item.marca;
                r1.Referencia = item.referencia;
                r1.Serial = item.serial;
                r1.Lote = item.lote;
                r1.Mes_fabricacion = Convert.ToInt32(item.mes_fabricacion);
                r1.Ano_fabricacion = Convert.ToInt32(item.ano_fabricacion);
                r1.F_compra = Convert.ToDateTime(item.f_compra);
                r1.Proveedor = item.proveedor;
                r1.Tipo = Convert.ToInt32(item.tipo);
                r1.Observaciones = item.observaciones;
                r1.Estado = item.estado;
                r1.F_inspeccion = r1.F_compra.AddYears(1);
                r1.Ccosto = item.ccosto;
                r1.F_prox_inspec = Convert.ToDateTime(item.f_prox_inspec);
                if (item.tipo == 1)
                {
                    r1.Tipo_equipo = "ARNES";
                }
                else if (item.tipo == 2)
                {
                    r1.Tipo_equipo = "ESLINGA P.";
                }
                else if (item.tipo == 3)
                {
                    r1.Tipo_equipo = "ESLINGA Y.";
                }
                else if (item.tipo == 4)
                {
                    r1.Tipo_equipo = "TIE OFF";
                }
                else if (item.tipo == 5)
                {
                    r1.Tipo_equipo = "LINEA DE VIDA";
                }
                else if (item.tipo == 6)
                {
                    r1.Tipo_equipo = "ESLINGA SEGURIDAD SENC.";
                }

                arr.Add(r1);

            }


            return jss.Serialize(arr);

        }

        public string readequipoalt(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            string codigo = data["codigo"];
            string serial = data["serial"];
            string lote = data["lote"];
            int tipo = Convert.ToInt32(data["tipo"]);
            int id = Convert.ToInt32(data["id"]);
            List<object> arr = new List<object>();
            bodegasst c = conn.bodegasst.Where(a => a.codigo.Equals(codigo) && a.serial.Equals(serial) && a.tipo == tipo && a.id == id).FirstOrDefault();
            arr.Add(c);

            if (c.tipo == 1) {
                var l = conn.insp_equi_altura.Where(r => r.arn_codigo1.Equals(c.codigo) || r.arn_codigo2.Equals(c.codigo) && r.arn_serial1.Equals(c.serial) || r.arn_serial2.Equals(c.serial) && r.arn_lote1.Equals(c.lote) ||  r.arn_lote2.Equals(c.lote)).ToList();
                arr.Add(l);
            }
            else if (c.tipo == 2)
            {
                var l = conn.insp_equi_altura.Where(r => r.cod_erc1.Equals(c.codigo) || r.cod_erc2.Equals(c.codigo) && r.serial_erc1.Equals(c.serial) || r.serial_erc1.Equals(c.serial) && r.lote_erc1.Equals(c.lote) || r.lote_erc2.Equals(c.lote)).ToList();
                arr.Add(l);
            }
            else if (c.tipo == 3)
            {
                var l = conn.insp_equi_altura.Where(r => r.codigo_eai1.Equals(c.codigo) || r.codigo_eai2.Equals(c.codigo) && r.serial_eai1.Equals(c.serial) || r.serial_eai2.Equals(c.serial) && r.lote_eai1.Equals(c.lote) || r.lote_eai2.Equals(c.lote)).ToList();
                arr.Add(l);
            }
            else if (c.tipo == 4)
            {
                var l = conn.insp_equi_altura.Where(r => r.codigo_tieoff1.Equals(c.codigo) || r.codigo_tieoff2.Equals(c.codigo) && r.serial_tieoff1.Equals(c.serial) || r.serial_tieoff2.Equals(c.serial) && r.lote_tieoff1.Equals(c.lote) || r.lote_tieoff2.Equals(c.lote)).ToList();
                arr.Add(l);
            }
            else if (c.tipo == 5)
            {
                var l = conn.insp_equi_altura.Where(r => r.cod_arrest1.Equals(c.codigo) || r.cod_arrest2.Equals(c.codigo) && r.serial_arrest1.Equals(c.serial) || r.serial_arrest2.Equals(c.serial) && r.lote_arrest1.Equals(c.lote) || r.lote_arrest2.Equals(c.lote)).ToList();
                arr.Add(l);
            }
            else if (c.tipo == 6)
            {
                var l = conn.insp_equi_altura.Where(r => r.codigo_lv1.Equals(c.codigo) || r.codigo_lv2.Equals(c.codigo) && r.serial_lv1.Equals(c.serial) || r.serial_lv2.Equals(c.serial) && r.lote_lv1.Equals(c.lote) || r.lote_lv2.Equals(c.lote)).ToList();
                arr.Add(l);
            }
            else if (c.tipo == 7)
            {
                var l = conn.insp_equi_altura.Where(r => r.codigo_mosq1.Equals(c.codigo) || r.codigo_mosq2.Equals(c.codigo) && r.serial_mosq1.Equals(c.serial) || r.serial_mosq2.Equals(c.serial) && r.lote_mosq1.Equals(c.lote) || r.lote_mosq2.Equals(c.lote)).ToList();
                arr.Add(l);
            }


            return jss.Serialize(arr);
        }

        public string validatehv(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            string codigo = data["codigo"];
            string serial = data["serial"];
            string lote = data["lote"];
            int tipo = Convert.ToInt32(data["tipo"]);
            int id = Convert.ToInt32(data["id"]);
            List<object> arr = new List<object>();
            hv_altura h = conn.hv_altura.Where(a => a.codigo.Equals(codigo) && a.serial.Equals(serial) && a.tipo.Equals(tipo) && a.lote.Equals(lote)).FirstOrDefault();
            int sw;
            if (h != null)
            {
                var de = conn.detalle_usu_alt.Where(r => r.id_hv.Equals(h.id)).ToList();
       

                arr.Add(de);
                sw = 0;

            }else {
                sw = 1;
            //bodegasst bsst = conn.bodegasst.Where(a => a.codigo.Equals(codigo) && a.serial.Equals(serial) && a.tipo == tipo).FirstOrDefault();
                bodegasst bsst = conn.bodegasst.Where(a => a.codigo.Equals(codigo) && a.serial.Equals(serial) && a.tipo == tipo && a.id == id).FirstOrDefault();
                empleado e = conn.empleado.Where(a => a.cedula_emp.Equals(bsst.cedula_asig)).FirstOrDefault();
            arr.Add(e);
            }
            arr.Add(sw);
            return jss.Serialize(arr);
        }

        public string CreateInspeccionescalera(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            string _json = data["json"];
            try
            {
                inspec_escalera c = jss.Deserialize<inspec_escalera>(_json);
                conn.inspec_escalera.Add(c);
                int rs = conn.SaveChanges();
                return jss.Serialize(rs > 0);
            }
            catch (DbEntityValidationException e)
            {
                string err = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        err += ve.ErrorMessage;
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return jss.Serialize(err);
            }
        }

        public string Readinspec_esacaleras(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            int id = Convert.ToInt32(data["id"]);
            List<object> arr = new List<object>();
            inspec_escalera c = conn.inspec_escalera.Where(a => a.id.Equals(id)).FirstOrDefault();
            usuario u = conn.usuario.Where(a => a.codigo.Equals(c.ced_usu_inspec)).FirstOrDefault();
            arr.Add(c);
            arr.Add(u);
            return jss.Serialize(arr);
        }

        public string Report_Escaleras(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            var obj = conn.inspec_escalera.ToList();

            foreach (var item in obj)
            {

                if (item.ced_empleado.Length != 0)
                {
                    item.idenfiticacion = item.idenfiticacion.ToUpper();
                    usuario usu = conn.usuario.Where(a => a.codigo.Equals(item.ced_usu_inspec)).FirstOrDefault();
                    item.respinspeccion = usu.nombre_usuario;
                    empleado emp = conn.empleado.Where(a => a.cedula_emp.Equals(item.ced_empleado)).FirstOrDefault();

                    if (emp != null)
                    {
                        item.ced_empleado = emp.nombre + " " + emp.snombre + " " + emp.ppellido + " " + emp.spellido;
                    }
                    else
                    {
                        item.ced_empleado = "NO REGISTRADO EN BD";
                    }
                }
                else
                {
                    item.ced_empleado = "NO REGISTRA EMPLEADO ASIGNADO";
                }

            }


            return jss.Serialize(obj);

        }

        #region COMPRAS 

        public string PreloadArticulos()
        {
            List<object> arr = new List<object>();
            compras_articulos c = conn.compras_articulos.OrderByDescending(a => a.codigo).FirstOrDefault();
            int codigo = 0001;
            if(c != null)
            {
                codigo = c.codigo + 1;
            }
            List<iva> iva = conn.iva.ToList();
            arr.Add(codigo);
            arr.Add(iva);
            return jss.Serialize(arr);
        }

        public string CreateArticulos(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            string _json = data["json"];
            try
            {
                compras_articulos e = jss.Deserialize<compras_articulos>(_json);
                conn.compras_articulos.Add(e);
                int rs = conn.SaveChanges();
                return jss.Serialize(rs > 0);
            }
            catch (DbEntityValidationException e)
            {
                string err = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        err += ve.ErrorMessage;
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return jss.Serialize(err);
            }
        }

        public string ValidatePresupuesto(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            string ano = data["ano"];
            string cuenta = data["cuenta"];
            string ccosto = data["ccosto"];

            int h = conn.presupuesto.Where(a => a.ano.Equals(ano) && a.cuenta.Equals(cuenta) && a.ccosto.Equals(ccosto)).Count();

            return jss.Serialize(h);
        }
        public string CreatePresupuesto(string json)
        {
            try
            {
                dynamic data = jss.DeserializeObject(json);
                dynamic list = jss.DeserializeObject(data["tabla"]);

                foreach (dynamic item in list)
                {
                    presupuesto pu = new presupuesto();

                    pu.ano = item["ano"];
                    pu.cuenta = item["cuenta"];
                    pu.rubro = item["rubro"];
                    pu.ccosto = item["ccosto"];
                    pu.total_presupuesto = item["valor"];
                    pu.total_ejecucion = 0;
                    pu.disponibilidad = item["valor"];
                    pu.indice = "";

                    conn.presupuesto.Add(pu);
                    
                }
                int rs = conn.SaveChanges();
                
                return jss.Serialize(rs > 0);
            }
            catch (DbEntityValidationException e)
            {
                string err = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        err += ve.ErrorMessage;
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return jss.Serialize("{err:" + err + "}");
            }
        }

        public string PreloadPresupuesto()
        {
            List<object> arr = new List<object>();
            List<presupuesto> p = conn.presupuesto.ToList();
            arr.Add(p);
            return jss.Serialize(arr);
        }
        public string AjustarPresupuesto(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            string _json = data["json"];
            try
            {
                presiones_presupuesto PP = conn.presiones_presupuesto.OrderByDescending(y => y.id).FirstOrDefault();
                int consecutivo1 = PP.id + 1;
                presiones_presupuesto pp = jss.Deserialize<presiones_presupuesto>(_json);
                pp.justificacion = data["observacion"];
                pp.ingresos = 0;
                pp.costos = 0;
                pp.utilidad = 0;
                pp.objeto = "";
                pp.dependencia = "";
                pp.vlr_total = data["adiccion"];
                pp.usuario = data["log_user"];
                pp.estado = "APROBADO";
                pp.vto_bueno_fina = 1;
                pp.fecha = DateTime.Now;
                conn.presiones_presupuesto.Add(pp);



                presupuesto c = jss.Deserialize<presupuesto>(_json);
                presupuesto p = conn.presupuesto.Where(a => a.id.Equals(c.id)).FirstOrDefault();
                p.disponibilidad = p.disponibilidad + data["adiccion"];
                p.indice = "UP";
                conn.Entry(p).State = System.Data.EntityState.Modified;
                histo_ajuste_presu h = new histo_ajuste_presu();
                h.fecha =  DateTime.Now;
                h.id_presupuesto = c.id;
                h.observacion = data["observacion"];
                h.valor_ant  = data["valor_ant"];
                h.nuevo_saldo = p.disponibilidad;
                conn.histo_ajuste_presu.Add(h);



                detalle_presiones r = new detalle_presiones();


                r.id_presion = consecutivo1;
                r.id_presupuesto = c.id;
                r.ccosto = c.ccosto;
                r.cuenta = c.cuenta;
                r.rubro = c.rubro;
                r.valor = Convert.ToInt32(data["adiccion"]);
                r.ejecutado = 1;
                conn.detalle_presiones.Add(r);
                int rs = conn.SaveChanges();
                return jss.Serialize(rs > 0);
            }
            catch (DbEntityValidationException e)
            {
                string err = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        err += ve.ErrorMessage;
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return jss.Serialize(err);
            }
        }

        public string TrasladoPresupuesto(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            dynamic j1 = jss.DeserializeObject(data["json1"]);
            string _json1 = data["json1"];
            string _json2 = data["json2"];
            try
            {
                presupuesto p1 = jss.Deserialize<presupuesto>(_json1);
                presupuesto p2 = jss.Deserialize<presupuesto>(_json2);
                presupuesto pr1 = conn.presupuesto.Where(a => a.id.Equals(p1.id)).FirstOrDefault();
                presupuesto pr2 = conn.presupuesto.Where(a => a.id.Equals(p2.id)).FirstOrDefault();
                if(pr1.disponibilidad >= j1["valor_traslado"]) { 
                //presupuesto INICIAL
                double vlr_ant_p1 = pr1.disponibilidad;
                double vlr_ant_p2 = pr2.disponibilidad;
                //pr1.total_presupuesto = pr1.total_presupuesto - j1["valor_traslado"];
                pr1.disponibilidad = pr1.disponibilidad - j1["valor_traslado"];
                    pr1.indice = "RIGHT";
                    //presupuesto FINAL
                    //pr2.total_presupuesto = j1["nuevo_total"];
                    pr2.disponibilidad = pr2.disponibilidad + j1["valor_traslado"];
                    pr2.indice = "UP";
                conn.Entry(pr1).State = System.Data.EntityState.Modified;
                conn.Entry(pr2).State = System.Data.EntityState.Modified;
                //HISTORICO PRESUPUESTO INICIAL
                histo_ajuste_presu h = new histo_ajuste_presu();
                h.fecha = DateTime.Now;
                h.id_presupuesto = p1.id;
                h.observacion = "TRASLADO SALDO A CUENTA: " + p2.cuenta +" JUSTIFICACION: "+"CECO: "+ p2.ccosto + j1["observacion"];
                h.valor_ant = vlr_ant_p1;
                h.nuevo_saldo = pr1.disponibilidad;
                conn.histo_ajuste_presu.Add(h);
                //HISTORICO PRESUPUESTO FINAL
                histo_ajuste_presu h2 = new histo_ajuste_presu();
                h2.fecha = DateTime.Now;
                h2.id_presupuesto = p2.id;
                h2.observacion = "RECIBE SALDO DE CUENTA: " + p1.cuenta + " JUSTIFICACION: "+"CECO: "+ p1.ccosto + j1["observacion"];
                h2.valor_ant = vlr_ant_p2;
                h2.nuevo_saldo = pr2.disponibilidad;
                conn.histo_ajuste_presu.Add(h2);
                int rs = conn.SaveChanges();
                
                return jss.Serialize(rs > 0);
                }
                else
                {
                    return jss.Serialize(0);
                }

            }
            catch (DbEntityValidationException e)
            {
                string err = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        err += ve.ErrorMessage;
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return jss.Serialize(err);
            }
        }

        public string buscar_movimientos_pre(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            int id = data["id"];
            List<object> arr = new List<object>();
            presupuesto c = conn.presupuesto.Where(a => a.id.Equals(id)).FirstOrDefault();
            var mp = conn.histo_ajuste_presu.Where(a => a.id_presupuesto.Equals(c.id)).ToList();

            arr.Add(c);
            arr.Add(mp);
            return jss.Serialize(arr);
        }
        public string PreloadNro_pedido()
        {
            int nro_pedido = 0;
            pedidos c = conn.pedidos.OrderByDescending(a => a.nro_pedido).FirstOrDefault();

            if (c != null)
            {
                nro_pedido = c.nro_pedido + 1;
            }
            return jss.Serialize(nro_pedido);
        }
        public string CreatePedidos(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            string _json = data["json"];
            dynamic list = jss.DeserializeObject(data["tabla"]);

            List<object> arr = new List<object>();
            try
            {
                pedidos p = jss.Deserialize<pedidos>(_json);
                p.vto_bueno_finan = 0;
                string pedido_nro = PreloadNro_pedido();
                p.nro_pedido = Convert.ToInt32(pedido_nro);
                int  nro_ped = p.nro_pedido;
                p.estado = "RETENIDO";
                p.aprobado_jefe = "0";
                conn.pedidos.Add(p);

                foreach (dynamic item in list)
                {
                    detalle_pedido r = new detalle_pedido();


                    r.id_pedido = p.nro_pedido;
                    r.codigo_art = item["codigo_art"];
                    r.descripcion = item["descripcion"];
                    r.cantidad = item["cantidad"];
                    r.und = item["und"];
                    r.valor = Convert.ToDouble(item["valor"]);
                    r.iva = Convert.ToDouble(item["iva"]);
                    r.subtotal = Convert.ToDouble(item["subtotal"]);
                    r.total = Convert.ToDouble(item["total"]);
                    r.cuenta = item["cuenta"];

                    conn.detalle_pedido.Add(r);
                    
                   
                }
                int rs = conn.SaveChanges();
                arr.Add(rs);

                var procedure = conn.validate_presupuesto(nro_ped + "");
                

                var du = conn.srp.Where(a => a.id_pedido == nro_ped).ToList();
                 int cant = du.Count();

                arr.Add(p.nro_pedido);

                return jss.Serialize(arr);
            }
            catch (DbEntityValidationException e)
            {
                string err = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        err += ve.ErrorMessage;
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return jss.Serialize(err);
            }
        }

        public string CreatePedidos_Diff(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            string _json = data["json"];
            JObject js = JObject.Parse(_json);
            string usuario = js["usuario"].ToString();
            dynamic list = jss.DeserializeObject(data["tabla"]);
            List<object> arr = new List<object>();
            try
            {
                pedidos p = jss.Deserialize<pedidos>(_json);
                p.vto_bueno_finan = 0;
                string pedido_nro = PreloadNro_pedido();
                p.nro_pedido = Convert.ToInt32(pedido_nro);
                int nro_ped = p.nro_pedido;
               
                p.estado = "RETENIDO";
                p.aprobado_jefe = "0";

                conn.pedidos.Add(p);

                foreach (dynamic item in list)
                {
                    detalle_pedido r = new detalle_pedido();
                    pedidos_tiempo_diff ptd = new pedidos_tiempo_diff();
                    detalle_pedido_diff_acta dpda = new detalle_pedido_diff_acta();

                    r.id_pedido = p.nro_pedido;
                    r.codigo_art = item["codigo_art"];
                    r.descripcion = item["descripcion"];
                    r.cantidad = item["cantidad"];
                    r.und = item["und"];
                    r.valor = Convert.ToDouble(item["valor"]);
                    r.iva = Convert.ToDouble(item["iva"]);
                    r.subtotal = Convert.ToDouble(item["subtotal"]);
                    r.total = Convert.ToDouble(item["total"]);
                    r.cuenta = item["cuenta"];

                    //entro en el pedido con el articulo y tiempo amortizar

                    ptd.id_pedido = p.nro_pedido;
                    ptd.cod_articulo = item["codigo_art"];
                    ptd.tiempo = Convert.ToInt32(item["tiempo"]);
                    ptd.estado = 1;
                    ptd.fechaCreacion = DateTime.Now;

                    //realizo la marcacion de pedidos diferidos con las actas de satisfaccion

                    dpda.id_pedido = nro_ped;
                    dpda.cod_articulo = item["codigo_art"];
                    //var ped = conn.pedidos.Where(x => x.nro_pedido == nro_ped).First();
                    dpda.usuario = usuario;
                    dpda.Estado = 0;
                    dpda.EsPedidoLegalizado = 0;
                    dpda.FechaCreacion = DateTime.Now;
                    dpda.FechaActualizacion = DateTime.Now;
                   

                    conn.pedidos_tiempo_diff.Add(ptd);
                    conn.detalle_pedido.Add(r);
                    conn.detalle_pedido_diff_acta.Add(dpda);

                }
                int rs = conn.SaveChanges();
                arr.Add(rs);
                arr.Add(p.nro_pedido);

                foreach (dynamic item in list)
                {
                    var procedure1 = conn.INSERT_AMORTIZACION(nro_ped + "", item["cantidad"], item["codigo_art"]);

                }
                // var proc = conn.INSERT_AMORTIZACION()

                var procedure = conn.validate_presupuesto_diff(nro_ped + "");
                return jss.Serialize(arr);
            }
            catch (DbEntityValidationException e)
            {
                string err = "";

                return jss.Serialize(err);
            }
        }

        public string CreatePedidos_AF(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            string _json = data["json"];
            dynamic list = jss.DeserializeObject(data["tabla"]);

            List<object> arr = new List<object>();
            try
            {
                pedidos p = jss.Deserialize<pedidos>(_json);
                p.vto_bueno_finan = 0;
                string pedido_nro = PreloadNro_pedido();
                p.nro_pedido = Convert.ToInt32(pedido_nro);
                int nro_ped = p.nro_pedido;
                p.estado = "RETENIDO";
                p.aprobado_jefe = "0";
                conn.pedidos.Add(p);

                foreach (dynamic item in list)
                {
                    detalle_pedido r = new detalle_pedido();


                    r.id_pedido = p.nro_pedido;
                    r.codigo_art = item["codigo_art"];
                    r.descripcion = item["descripcion"];
                    r.cantidad = item["cantidad"];
                    r.und = item["und"];
                    r.valor = Convert.ToDouble(item["valor"]);
                    r.iva = Convert.ToDouble(item["iva"]);
                    r.subtotal = Convert.ToDouble(item["subtotal"]);
                    r.total = Convert.ToDouble(item["total"]);
                    r.cuenta = item["cuenta"];

                    conn.detalle_pedido.Add(r);

                }
                int rs = conn.SaveChanges();
                arr.Add(rs);
                foreach (dynamic item in list)
                {
                    var procedure1 = conn.INSERT_DEPRECIACION(nro_ped + "", item["cantidad"], item["codigo_art"]);

                }
                var procedure = conn.validate_presupuesto_AF(nro_ped + "");

                var du = conn.srp.Where(a => a.id_pedido == nro_ped).ToList();
                int cant = du.Count();

                arr.Add(p.nro_pedido);

                return jss.Serialize(arr);
            }
            catch (DbEntityValidationException e)
            {
                string err = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        err += ve.ErrorMessage;
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return jss.Serialize(err);
            }
        }

        public string ReadPedido(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            int id = data["id"];
            List<object> arr = new List<object>();
            pedidos c = conn.pedidos.Where(a => a.id.Equals(id)).FirstOrDefault();
            var p = conn.detalle_pedido.Where(a => a.id_pedido.Equals(c.nro_pedido)).ToList();
            arr.Add(c);
            arr.Add(p);
            return jss.Serialize(arr);
        }

        public string ReportePedidos(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            int tipo = Convert.ToInt32(data["tipo"]);
            List<object> arr = new List<object>();
            DateTime fecha = DateTime.Today;
            int mes_actual = fecha.Month;
            int año_actual = fecha.Year;
            //
            if (tipo == 1)
            {
                List<pedidos> p = conn.pedidos.Where(x => x.nro_srp.Equals(null) && x.aprobado_jefe.Equals("1") && x.estado!="CANCELADO").ToList();
                arr.Add(p);
            }
            else if (tipo == 2)
            {
                List<pedidos> p = conn.pedidos.Where(x => x.nro_srp != null && x.aprobado_jefe.Equals("1") && x.estado != "CANCELADO" && x.estado != "RETENIDO" && x.fecha.Month.Equals(mes_actual) && x.fecha.Year.Equals(año_actual)).ToList();
                arr.Add(p);
            }

            
            return jss.Serialize(arr);
        }
        public string ReportePedidos_finan()
        {
            List<object> arr = new List<object>();
            List<pedidos> p = conn.pedidos.Where(x => x.nro_srp != null && x.vto_bueno_finan==0).ToList();
            // List<pedidos> p = conn.pedidos.Where(x => x.nro_srp != null).ToList();
            arr.Add(p);
            return jss.Serialize(arr);
        }

        public string ReportePresion()
        {
            List<object> arr = new List<object>();
            List<presiones_presupuesto> p = conn.presiones_presupuesto.Where(Y=>Y.estado.Equals("PENDIENTE")).ToList();
            arr.Add(p);
            return jss.Serialize(arr);
        }

        public string ReporteTraslado()
        {
            List<object> arr = new List<object>();
            List<traslados_presupuesto> p = conn.traslados_presupuesto.Where(x => x.estado.Equals("PENDIENTE")).ToList();
            arr.Add(p);
            return jss.Serialize(arr);
        }

        public string TrasladosEjecutados(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            int tipo = Convert.ToInt32(data["tipo"]);
            DateTime fecha_inicial = Convert.ToDateTime(data["fechainicio"]);
            DateTime fecha_final = Convert.ToDateTime(data["fechafin"]);

            List<object> arr = new List<object>();
            if (tipo == 1)
            {
                List<traslados_presupuesto> p = conn.traslados_presupuesto.Where(x => x.estado.Equals("APROBADO")&& x.fecha >= fecha_inicial && x.fecha <= fecha_final).ToList();
                arr.Add(p);
            }
            else if (tipo == 2)
            {
                List<traslados_presupuesto> p = conn.traslados_presupuesto.Where(x => x.estado.Equals("PENDIENTE") && x.fecha >= fecha_inicial && x.fecha <= fecha_final).ToList();
                arr.Add(p);
            }



            return jss.Serialize(arr);
        }

        public string PresionesAprobadas(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            int tipo = Convert.ToInt32(data["tipo"]);
            DateTime fecha_inicial = Convert.ToDateTime(data["fechainicio"]);
            DateTime fecha_final = Convert.ToDateTime(data["fechafin"]);

            List<object> arr = new List<object>();
            if (tipo == 1)
            {
                List<presiones_presupuesto> p = conn.presiones_presupuesto.Where(x => x.estado.Equals("APROBADO") && x.fecha >= fecha_inicial && x.fecha <= fecha_final).ToList();
                arr.Add(p);
            }
            else if (tipo == 2)
            {
                List<presiones_presupuesto> p = conn.presiones_presupuesto.Where(x => x.estado.Equals("PENDIENTE") && x.fecha >= fecha_inicial && x.fecha <= fecha_final).ToList();
                arr.Add(p);
            }



            return jss.Serialize(arr);
        }
        public string ReadPresion(string json)
        {
            try
            {
                dynamic data = jss.DeserializeObject(json);
                int id = Convert.ToInt32(data["id"]);

                List<object> arr = new List<object>();
                presiones_presupuesto c = conn.presiones_presupuesto.Where(a => a.id.Equals(id)).FirstOrDefault();
                List<detalle_presiones> dt = conn.detalle_presiones.Where(a => a.id_presion.Equals(id)).ToList();
                arr.Add(c);
                arr.Add(dt);

                return jss.Serialize(arr);
            }
            catch (Exception e)
            {
                return jss.Serialize("Error " + e.Message.ToString());
            }

        }

        public string ReadTraslados(string json)
        {
            try
            {
                dynamic data = jss.DeserializeObject(json);
                int id = Convert.ToInt32(data["id"]);

                List<object> arr = new List<object>();
                traslados_presupuesto c = conn.traslados_presupuesto.Where(a => a.id.Equals(id)).FirstOrDefault();
                List<detalle_traslados> dt = conn.detalle_traslados.Where(a => a.id_traslado.Equals(id)).ToList();
                arr.Add(c);
                arr.Add(dt);

                return jss.Serialize(arr);
            }
            catch (Exception e)
            {
                return jss.Serialize("Error "+ e.Message.ToString());
            }
            
        }

        public string Reporte_Nro_pedido(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            int numero_ped = Convert.ToInt32(data["numero_ped"]);
            List<object> arr = new List<object>();
            pedidos p = conn.pedidos.Where(a => a.nro_pedido.Equals(numero_ped)).FirstOrDefault();
            var dp = conn.detalle_pedido.Where(x => x.id_pedido.Equals(p.nro_pedido)).ToList();

            arr.Add(p);
            arr.Add(dp);
            return jss.Serialize(arr);
        }

        public string Reporte_SRP(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            int numero_ped = Convert.ToInt32(data["numero_ped"]);
            List<object> arr = new List<object>();
            pedidos p = conn.pedidos.Where(a => a.nro_pedido.Equals(numero_ped)).FirstOrDefault();
            var dp = conn.srp.Where(x => x.id_pedido.Equals(p.nro_pedido)).ToList();

            arr.Add(p);
            arr.Add(dp);
            return jss.Serialize(arr);
        }
        public string Reporte_pte_rp(string json)
        {

            dynamic data = jss.DeserializeObject(json);
            int tipo = Convert.ToInt32(data["tipo"]);
            List<object> arr = new List<object>();
            DateTime fecha_inicial = DateTime.Today;
            DateTime fecha_final = DateTime.Today;

            if (data["fechaini"] != null)
            {
                fecha_inicial = Convert.ToDateTime(data["fechaini"]);
            }

            if (data["fechafin"] != null)
            {
                fecha_final = Convert.ToDateTime(data["fechafin"]);

            }

            if (tipo == 1)
            {
                 List<pedidos> p = conn.pedidos.Where(x => x.nro_srp != null &&  x.vto_bueno_finan == 1 && x.nro_rp == null && x.fecha>=fecha_inicial && x.fecha<=fecha_final).ToList();
                arr.Add(p);
            }else if (tipo == 2)
            {
                List<pedidos> p = conn.pedidos.Where(x => x.nro_srp != null && x.vto_bueno_finan == 1 && x.nro_rp != null && x.fecha >= fecha_inicial && x.fecha <= fecha_final).ToList();
                arr.Add(p);
            }

           

            return jss.Serialize(arr);
        }

        public string ReportePedidos_contratos(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            string user = data["usuario"];
            string username = data["username"];
            int tipo = Convert.ToInt32(data["tipo"]);
            DateTime fecha_inicial = DateTime.Today;
            DateTime fecha_final = DateTime.Today;

            if (data["fechaini"]!=null)
            {
               fecha_inicial = Convert.ToDateTime(data["fechaini"]);
            }

            if (data["fechafin"]!=null) 
            {
              fecha_final = Convert.ToDateTime(data["fechafin"]);

            }
            List<object> arr = new List<object>();
            views v = conn.views.Where(a => a.name_module.Equals("Pedidos Aprobados")).FirstOrDefault();
            permisos_usu per = conn.permisos_usu.Where(x => x.id_view.Equals(v.id) && x.usuario.Equals(username)).FirstOrDefault();
            
            if(per.pe1.Equals("1"))
            {  
                if(tipo == 0)
                {
                    List<pedidos> p = conn.pedidos.Where(x => x.estado.Equals("APROBADO") && x.fecha >= fecha_inicial && x.fecha <= fecha_final).ToList();
                    arr.Add(p);
                }
                else if( tipo == 1)
                {
                    List<pedidos> p = conn.pedidos.Where(x => x.estado.Equals("APROBADO") && x.nro_contrato != null && x.fecha >= fecha_inicial && x.fecha <= fecha_final).ToList();
                    arr.Add(p);
                }
                else if(tipo == 2)
                {
                    List<pedidos> p = conn.pedidos.Where(x => x.estado.Equals("APROBADO") && x.nro_contrato == null && x.fecha >= fecha_inicial && x.fecha <= fecha_final).ToList();
                    arr.Add(p);
                }
                
            }
            else
            {
                if (tipo == 0)
                {
                    List<pedidos> p = conn.pedidos.Where(x => x.estado.Equals("APROBADO") && x.usuario.Equals(user)).ToList();
                    arr.Add(p);

                }
                else if (tipo == 1)
                {
                    List<pedidos> p = conn.pedidos.Where(x => x.estado.Equals("APROBADO") && x.usuario.Equals(user) && x.nro_contrato != null).ToList();
                    arr.Add(p);
                }
                else if (tipo == 2)
                {
                    List<pedidos> p = conn.pedidos.Where(x =>  x.usuario.Equals(user) && x.fecha >= fecha_inicial && x.fecha <= fecha_final && x.estado !="APROBADO").ToList();
                    arr.Add(p);
                }
            }

            
            return jss.Serialize(arr);
        }

        public string Aprobar_srp(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            int nro_ped = data["nro_ped"];
            try
            {
                consecutivos c = conn.consecutivos.FirstOrDefault();
                pedidos p = conn.pedidos.Where(a => a.nro_pedido.Equals(nro_ped)).FirstOrDefault();
                c.consecutivo_srp = c.consecutivo_srp + 1;
                p.nro_srp = c.consecutivo_srp;
                p.fecha_vto_bueno = DateTime.Now;
                conn.Entry(c).State = System.Data.EntityState.Modified;
                conn.Entry(p).State = System.Data.EntityState.Modified;
                int rs = conn.SaveChanges();
                return jss.Serialize(p.nro_srp);
            }
            catch (DbEntityValidationException e)
            {
                string err = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        err += ve.ErrorMessage;
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return jss.Serialize(err);
            }
        }
        public string verificar_srp(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            int nro_ped = data["nro_ped"];
            try
            {
                List<object> arr = new List<object>();
                var du = conn.srp.Where(a => a.id_pedido == nro_ped).ToList();
                pedidos p = conn.pedidos.Where(a => a.nro_pedido.Equals(nro_ped)).FirstOrDefault();
                int cant = du.Count();
                DateTime fecha = DateTime.Today;
                string anio = fecha.Year.ToString();
                int contador = 0;
                if (du != null)
                {
                    foreach (var item in du)
                    {
                        presupuesto x = conn.presupuesto.Where(a => a.ccosto.Equals(item.ccosto) && a.cuenta.Equals(item.cuenta) && a.ano.Equals(anio)).FirstOrDefault();
                        if (x != null)
                        {
                            if (item.total <= x.disponibilidad && (x.disponibilidad - item.total >= 0))
                            {
                                item.comprobado = 1;
                                contador = contador + 1;
                                conn.Entry(item).State = System.Data.EntityState.Modified;
                                x.disponibilidad = (Convert.ToDouble(x.disponibilidad - item.total));
                                x.total_ejecucion = (Convert.ToDouble(x.total_ejecucion + item.total));
                                conn.Entry(x).State = System.Data.EntityState.Modified;
                            }
                        }
                    }

                    var dp = conn.depreciacion.Where(a => a.id_pedido == nro_ped).ToList();

                    if(dp != null)
                    {
                        foreach (var item in dp)
                        {
                            DateTime fecha_actual = DateTime.Today;
                            int meses_restantes_año = 12 - fecha_actual.Month;
                            item.valor_ejecutado = item.valor_ejecutado + meses_restantes_año * item.cuota;
                            item.v_util_restante = item.v_util_restante - meses_restantes_año;
                            conn.Entry(item).State = System.Data.EntityState.Modified;

                        }
                    }
                        
                    arr.Add(p.nro_pedido);
                    if (cant == contador)
                    {
                        p.estado = "PENDIENTE";
                        arr.Add(p.estado);
                        conn.Entry(p).State = System.Data.EntityState.Modified;
                        int y = conn.SaveChanges();
                    }
                    arr.Add(p.estado);

                }
                return jss.Serialize(arr);
            }
            catch (DbEntityValidationException e)
            {
                string err = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        err += ve.ErrorMessage;
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return jss.Serialize(err);
            }
        }


        public string Guardar_aprobacion_presion(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            int id= data["id"];
            presiones_presupuesto x = conn.presiones_presupuesto.Where(a => a.id.Equals(id)).FirstOrDefault();
            List<detalle_presiones> dp = conn.detalle_presiones.Where(a => a.id_presion.Equals(id)).ToList();
            List<object> arr = new List<object>();
            int rs = 0;
            int l = 0;

            try
            {
                if (x.vto_bueno_fina != 1)
                {
                  
                    foreach (detalle_presiones item in dp)
                    {
                        detalle_presiones dd = new detalle_presiones();
                        dd = item;
                        
                        presupuesto o = conn.presupuesto.Where(p => p.id.Equals(item.id_presupuesto)).FirstOrDefault();

                        if (dd.ejecutado != 1)
                        {
                      
                            if (o != null && x.vto_bueno_fina != 1)
                            {
                                histo_ajuste_presu h = new histo_ajuste_presu();
                                Double valor_ant = Convert.ToDouble(o.disponibilidad);

                                o.disponibilidad = o.disponibilidad + Convert.ToInt64(item.valor);
                                o.indice = "UP";
                                conn.Entry(o).State = System.Data.EntityState.Modified;
                                h.fecha = DateTime.Now;
                                h.id_presupuesto = Convert.ToInt32(item.id_presupuesto);
                                h.nuevo_saldo = o.disponibilidad;
                                h.valor_ant = valor_ant;

                                h.observacion = x.justificacion + " Consecutivo " + item.id_presion + " RESPONSABLE " + x.usuario;
                                conn.histo_ajuste_presu.Add(h);
                            }

                            dd.ejecutado = 1;
                            conn.Entry(dd).State = System.Data.EntityState.Modified;
                            l = conn.SaveChanges();
                          
                        }
                        else if(dd.ejecutado==1)
                        {
                            x.estado = "APROBADO";
                            x.vto_bueno_fina = 1;
                            conn.Entry(x).State = System.Data.EntityState.Modified;
                            rs = conn.SaveChanges();
                            break;
                        }
                        else
                        {
                            dd.ejecutado = 0;
                            conn.Entry(dd).State = System.Data.EntityState.Modified;

                        }


                    }
                  
                    if (l>0)
                    {
                        x.estado = "APROBADO";
                        x.vto_bueno_fina = 1;
                        conn.Entry(x).State = System.Data.EntityState.Modified;
                       

                    }
                    rs = conn.SaveChanges();


                }
                string k = rs + "";
                return jss.Serialize(k);

            }
            catch (DbEntityValidationException e)
            {
                string err = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        err += ve.ErrorMessage;
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }

                return jss.Serialize(err);
            }
            


        }

        /*SHILEY MOLINA*/
        public String Guardar_Aprobacion_traslado(String json){
            dynamic data = jss.DeserializeObject(json);
            int id = Convert.ToInt32(data["id"]);
            List<object> arr = new List<object>();
            traslados_presupuesto n = conn.traslados_presupuesto.Where(a => a.id.Equals(id)).FirstOrDefault();
            List<detalle_traslados> dt = conn.detalle_traslados.Where(a => a.id_traslado.Equals(id)).ToList();


            if (dt != null)
            {
                ////
                ///
                try
                {
               
                    double vlr_ant_p1 = 0;
                    double vlr_ant_p2 = 0;


                    foreach (detalle_traslados item in dt)
                    {
                        detalle_traslados nu = item;
                        presupuesto pr1 = new presupuesto();
                        presupuesto pr2 = new presupuesto();

                        pr1 = conn.presupuesto.Where(y => y.id.Equals(item.id_presu_ini)).FirstOrDefault();
                        pr2 = conn.presupuesto.Where(y => y.id.Equals(item.id_presu_fin)).FirstOrDefault();
                        double disponibilidad = Convert.ToInt64(pr1.disponibilidad);
                        if (pr1 != null)
                        {
                            if (Convert.ToInt64(pr1.disponibilidad) >= item.valor_traslado )
                            {
                                //presupuesto INICIAL

                                if (item.ejecutado != 1)
                                {
                                    vlr_ant_p1 = Convert.ToInt64(pr1.disponibilidad);
                                    vlr_ant_p2 = Convert.ToUInt64(pr2.disponibilidad);
                                    pr1.disponibilidad = pr1.disponibilidad - Convert.ToDouble(item.valor_traslado);
                                    pr1.indice = "RIGHT";
                                    pr2.disponibilidad = pr2.disponibilidad + Convert.ToDouble(item.valor_traslado);
                                    pr2.indice = "UP";
                                    conn.Entry(pr1).State = System.Data.EntityState.Modified;
                                    conn.Entry(pr2).State = System.Data.EntityState.Modified;
                                    //HISTORICO PRESUPUESTO INICIAL
                                    histo_ajuste_presu h = new histo_ajuste_presu();

                                    h.fecha = DateTime.Now;
                                    h.id_presupuesto = item.id_presu_ini;
                                    h.observacion = "TRASLADO SALDO A CUENTA: " + item.cuenta_fin + " CECO: " + item.ccosto_fin + " JUSTIFICACION " + n.justificacion + " CONSECUTIVO " + item.id_traslado + " RESPONSABLE " + n.usuario;
                                    h.valor_ant = vlr_ant_p1;
                                    h.nuevo_saldo = pr1.disponibilidad;
                                    conn.histo_ajuste_presu.Add(h);
                                    //HISTORICO PRESUPUESTO FINAL
                                    histo_ajuste_presu h2 = new histo_ajuste_presu();
                                    h2.fecha = DateTime.Now;
                                    h2.id_presupuesto = item.id_presu_fin;
                                    h2.observacion = "RECIBE SALDO DE CUENTA: " + item.cuenta_ini + "CECO: " + item.ccosto_ini + " JUSTIFICACION " + n.justificacion + " CONSECUTIVO " + item.id_traslado + " RESPONSABLE " + n.usuario;
                                    h2.valor_ant = vlr_ant_p2;
                                    h2.nuevo_saldo = pr2.disponibilidad;
                                    conn.histo_ajuste_presu.Add(h2);
                                    nu.ejecutado = 1;
                                    conn.Entry(nu).State = System.Data.EntityState.Modified;
                                }
                                
                            }
                            else {
                                nu.ejecutado = 0;
                                conn.Entry(nu).State = System.Data.EntityState.Modified;
                            

                            }



                            // no   conn.detalle_traslados.Add(r);


                        }
                        int a = conn.SaveChanges();
                        arr.Add(a);


                    }

                   
                }
                catch (DbEntityValidationException e)
                {
                    string err = "";
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            err += ve.ErrorMessage;
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    return jss.Serialize(err);
                }



                //////


            }

            if (n != null)
             {
                n.estado = "APROBADO";
                n.vto_bueno_finan = 1;


            }
            conn.Entry(n).State = System.Data.EntityState.Modified;
            int  rs = conn.SaveChanges();
             return jss.Serialize(rs > 0);

        }

        public string Aprobado_financiera(string json)
        {
            
            dynamic data = jss.DeserializeObject(json);
            dynamic list =  jss.DeserializeObject(data["tabla"]);
            

            foreach (dynamic item in list)
            {
                pedidos p = new pedidos();
                int ped = item["nro_pedido"];
                pedidos x = conn.pedidos.Where(a => a.nro_pedido.Equals(ped)).FirstOrDefault();
                x.vto_bueno_finan = item["vto_bueno_finan"];
                x.fecha_vto_finan = DateTime.Now;
                //x.estado = "APROBADO";
                conn.Entry(x).State = System.Data.EntityState.Modified;
                
            }
            int rs = conn.SaveChanges();
            return jss.Serialize(rs > 0);
        }

        public string Aprobar_RP(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            dynamic list = jss.DeserializeObject(data["tabla"]);

            try
            {
                foreach (dynamic item in list)
                {
                    if(item["nro_rp"] != null) { 
                    consecutivos c = conn.consecutivos.FirstOrDefault();
                    int nro_ped = item["nro_pedido"];
                    pedidos p = conn.pedidos.Where(a => a.nro_pedido.Equals(nro_ped)).FirstOrDefault();
                    c.consecutivo_rp = item["nro_rp"];
                    p.nro_rp = item["nro_rp"];
                    p.estado = "APROBADO";
                    conn.Entry(c).State = System.Data.EntityState.Modified;
                    conn.Entry(p).State = System.Data.EntityState.Modified;
                    }
                }
                int rs = conn.SaveChanges();
                return jss.Serialize(rs > 0);
            }

            catch (DbEntityValidationException e)
            {
                string err = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        err += ve.ErrorMessage;
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return jss.Serialize(err);
            }
        }

        public string Validarpermiso(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            string username = data["username"];

            views v = conn.views.Where(a => a.name_module.Equals("Pedidos Aprobados")).FirstOrDefault();
            permisos_usu per = conn.permisos_usu.Where(x => x.id_view.Equals(v.id) && x.usuario.Equals(username)).FirstOrDefault();


            return jss.Serialize(per.pe1);
        }

        #endregion

        public string Update_estados(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            string _json = data["json"];
            try
            {
                pedidos c = jss.Deserialize<pedidos>(_json);
                //pedidos p = conn.pedidos.Where(a => a.nro_pedido.Equals(nro_ped)).FirstOrDefault();
                c.vto_bueno_presu = 1;
                conn.Entry(c).State = System.Data.EntityState.Modified;
                int rs = conn.SaveChanges();
                return jss.Serialize(rs > 0);
            }
            catch (DbEntityValidationException e)
            {
                string err = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        err += ve.ErrorMessage;
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }

                return jss.Serialize(err);
            }
        }

        public string ReadPedidos_detalle_rp(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            int id = Convert.ToInt32(data["id"]);
            List<object> arr = new List<object>();
            //pedidos x = conn.pedidos.Where(a => a.id == id).FirstOrDefault();
            var r = conn.pedidos.Where(a => a.nro_rp == id).ToList();
            double y = 0;

            foreach (dynamic item in r)
            {
                int nro_ped = Convert.ToInt32(item.nro_pedido);

                y = y + Convert.ToDouble(conn.srp.Where(e => e.id_pedido == nro_ped).Select(a => a.total).Sum());
            }
            string rp = Convert.ToString(id);

            var mp = conn.detalle_rp.Where(a => a.nro_rp.Equals(rp)).ToList();
            arr.Add(y);
            arr.Add(mp);
            return jss.Serialize(arr);
        }

        public string Save_detalles_rp(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            string nro_rp = Convert.ToString(data["nro_rp"]);
            string username = data["username"];
            string hora = data["hora"];
            string _json = data["json"];
            //dynamic list = jss.DeserializeObject(data["detalle_rp"]);

            //var p = conn.detalle_rp.Where(a => a.nro_rp.Equals(nro_rp)).ToList();
            //foreach (dynamic item in p)
            //{
            //    conn.detalle_rp.Remove(item);
            //   int r = conn.SaveChanges();
            //}

            List<object> arr = new List<object>();
            try
            {

                detalle_rp dr = jss.Deserialize<detalle_rp>(_json);
                dr.nro_rp = nro_rp;
                dr.log_fecha = DateTime.Now;
                dr.log_hora = hora;
                dr.log_user = username;

                conn.detalle_rp.Add(dr);

                //foreach (dynamic item in list)
                //{
                //    detalle_rp r = new detalle_rp();

                //    r.nro_rp = nro_rp;
                //    r.factura = item["factura"];
                //    r.fecha = Convert.ToDateTime(item["fecha"]);
                //    r.valor =  Convert.ToString(item["valor"]);
                //    r.log_fecha = DateTime.Now;
                //    r.log_hora = hora;
                //    r.log_user = username;


                //    conn.detalle_rp.Add(r);


                //}
                int rs = conn.SaveChanges();
                return jss.Serialize(arr);
            }
            catch (DbEntityValidationException e)
            {
                string err = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        err += ve.ErrorMessage;
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return jss.Serialize("?" + err);
            }

            catch (Exception e)
            {

                string ret = "";
                if (e.InnerException != null)
                {
                    if (e.InnerException.Message != null)
                    {
                        ret = (e.InnerException.Message);
                        if (e.InnerException.InnerException != null)
                        {
                            ret += e.InnerException.InnerException.Message + " " + e.Message;
                        }
                        else
                        {
                            ret += e.Message;
                        }
                    }
                }
                else
                {
                    ret = (e.Message);
                }
                return jss.Serialize("?" + ret);
            }
        }

        public string PreloaDirectivos()
        {
            List<object> arr = new List<object>();
            List<directivos> D = conn.directivos.ToList();
            arr.Add(D);
            return jss.Serialize(arr);
        }

        public string CreateActa_Satisfaccion(string json)
        {
            try
            {
                dynamic data = jss.DeserializeObject(json);
                string _json = data["json"];
                dynamic tabla = jss.DeserializeObject(data["tabla"]);
                dynamic list_rp = jss.DeserializeObject(data["lista_rp"]);
                List<object> arr = new List<object>();
                acta_satisfaccion_nueva act = jss.Deserialize<acta_satisfaccion_nueva>(_json);
                consecutivos c = conn.consecutivos.FirstOrDefault();
                act.consecutivo = c.consecutivo_as;
                foreach (dynamic it in tabla)
                {
                    detalle_acta_satis das = new detalle_acta_satis();
                    das.consecutivo = c.consecutivo_as;
                    das.nro_pedido = Convert.ToInt32(it["nro_pedido"]);
                    das.nro_srp = Convert.ToInt32(it["nro_srp"]);
                    das.ceco = it["ceco"];
                    das.cuenta = it["cuenta"];
                    das.nro_rp = it["nro_rp"];
                    das.valor_max = Convert.ToDouble(it["valor_max"]);
                    das.valor = Convert.ToInt32(it["valor"]);
                    das.otros = Convert.ToInt32(it["otros"]);
                    das.iva = Convert.ToInt32(it["iva"]);
                    das.subtotal = Convert.ToInt32(it["subtotal"]);
                    das.saldo_disponible = Convert.ToInt32(it["saldo_disponible"]);
                    das.activo = it["activo"];

                    conn.detalle_acta_satis.Add(das);
                    ValidarPedidoDiferido(Convert.ToInt32(it["nro_pedido"]), c.consecutivo_as);
                }

                c.consecutivo_as = c.consecutivo_as + 1;
                conn.Entry(c).State = System.Data.EntityState.Modified;
                conn.acta_satisfaccion_nueva.Add(act);
                int rs = conn.SaveChanges();
                arr.Add(rs > 0);
                arr.Add(act.consecutivo);
                return jss.Serialize(arr);
            }
            catch (DbEntityValidationException e)
            {
                string err = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        err += ve.ErrorMessage;
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return jss.Serialize("{err:" + err + "}");
            }
        }

        public void ValidarPedidoDiferido(int pedido, int consecutivo)
        {
            var pedidoDiff = conn.detalle_pedido_diff_acta.Where(x => x.id_pedido == pedido).ToList();
            if(pedidoDiff != null)
            {
                foreach(detalle_pedido_diff_acta dpa in pedidoDiff)
                {
                    dpa.consecutivo = consecutivo;
                    //conn.detalle_pedido_diff_acta.
                    conn.Entry(dpa).State = System.Data.EntityState.Modified;
                }
                conn.SaveChanges();
            }
        }

        public string Update_ASatisfaccion(string json)
        {
            try
            {
                dynamic data = jss.DeserializeObject(json);
                string _json = data["json"];
                dynamic tabla = jss.DeserializeObject(data["tabla"]);
                dynamic list_rp = jss.DeserializeObject(data["lista_rp"]);

                acta_satisfaccion_nueva act = jss.Deserialize<acta_satisfaccion_nueva>(_json);
                conn.Entry(act).State = System.Data.EntityState.Modified;
                int rs = 0;
                //BUSCAR PARA ELIMINAR LOS DETALLES
                var dt = conn.detalle_acta_satis.Where(x => x.consecutivo == act.consecutivo).ToList();
                var rp = conn.list_rp.Where(x => x.consecutivo_as == act.consecutivo).ToList();

                foreach (dynamic i in dt)
                {
                    conn.detalle_acta_satis.Remove(i);
                    rs = conn.SaveChanges();
                }
                foreach (dynamic it in tabla)
                {
                    detalle_acta_satis das = new detalle_acta_satis();
                    das.consecutivo = Convert.ToInt32(act.consecutivo);
                    das.nro_pedido = Convert.ToInt32(it["nro_pedido"]);
                    das.nro_srp = Convert.ToInt32(it["nro_srp"]);
                    das.ceco = it["ceco"];
                    das.cuenta = it["cuenta"];
                    das.nro_rp = it["nro_rp"];
                    das.valor_max = Convert.ToDouble(it["valor_max"]);
                    das.valor = Convert.ToDouble(it["valor"]);
                    das.otros = Convert.ToInt32(it["otros"]);
                    das.iva = Convert.ToInt32(it["iva"]);
                    das.subtotal = Convert.ToInt32(it["subtotal"]);
                    das.saldo_disponible = Convert.ToInt64(it["saldo_disponible"]);
                    das.activo = it["activo"];
                    conn.detalle_acta_satis.Add(das);

                }

                 rs = conn.SaveChanges();
                return jss.Serialize(rs > 0);
            }
            catch (DbEntityValidationException e)
            {
                string err = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        err += ve.ErrorMessage;
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return jss.Serialize("{err:" + err + "}");
            }
        }

        public string Validar_saldo_disponibe(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            int nro_pedido = data["pedido"];
            string ceco = data["ceco"];
            string cuenta = data["cuenta"];
            var das = conn.detalle_acta_satis.Where(x => x.nro_pedido == nro_pedido && x.ceco.Equals(ceco) && x.cuenta.Equals(cuenta)).ToList();
            Double disponible = 0;
            foreach (var item in das)
            {
                detalle_acta_satis I = new detalle_acta_satis();

                disponible = disponible + Convert.ToDouble(item.subtotal);

            }

            return jss.Serialize(disponible);
        }

        public string ReporteAS(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            int tipo = Convert.ToInt32(data["tipo"]);
            string usuario = data["usuario"];
            List<object> arr = new List<object>();
            if (tipo == 1)
            {
                List<acta_satisfaccion_nueva> p = conn.acta_satisfaccion_nueva.Where(x => x.aprobado != "1" && x.asignado_a.Equals(usuario)).ToList();
                arr.Add(p);
            }
            else if (tipo == 2)
            {
                List<acta_satisfaccion_nueva> p = conn.acta_satisfaccion_nueva.Where(x => x.aprobado == "1" && x.asignado_a.Equals(usuario)).ToList();
                arr.Add(p);
            }


            return jss.Serialize(arr);
        }

        public string Read_Acta_Sat(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            int conse = data["consecutivo"];
            List<object> arr = new List<object>();
            acta_satisfaccion_nueva p = conn.acta_satisfaccion_nueva.Where(a => a.consecutivo == conse).FirstOrDefault();
            var das = conn.detalle_acta_satis.Where(x => x.consecutivo == conse).ToList();
            var rp = conn.list_rp.Where(x => x.consecutivo_as == conse).ToList();
            arr.Add(p);
            arr.Add(das);
            arr.Add(rp);
            return jss.Serialize(arr);
        }

        public string Read_Detalle_rp(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            string nro_rp = data["nro_rp"];
            string factura = data["factura"];
            detalle_rp p = conn.detalle_rp.Where(a => a.nro_rp == nro_rp && a.factura.Equals(factura)).FirstOrDefault();

            return jss.Serialize(p);
        }

        public string Aprobar_AS(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            int cons = data["consecutivo"];
            string usuario = data["usuario"];
            string log_user = data["log_user"];
            try
            {

                acta_satisfaccion_nueva p = conn.acta_satisfaccion_nueva.Where(a => a.consecutivo == cons).FirstOrDefault();

                p.aprobado = "1";
                p.aprobado_por = usuario;
                p.ruta_firma = "firmas/" + log_user + ".png"; 
                conn.Entry(p).State = System.Data.EntityState.Modified; 
                int rs = conn.SaveChanges();

                validateCambioEstadoPedidoActaDiferido(cons);
                return jss.Serialize(rs);
            }
            catch (DbEntityValidationException e)
            {
                string err = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        err += ve.ErrorMessage;
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return jss.Serialize(err);
            }
        }

        public void validateCambioEstadoPedidoActaDiferido(int consecutivo)
        {
            var pedidoActaDiferido = conn.detalle_pedido_diff_acta.Where(x => x.consecutivo == consecutivo).ToList();
            if(pedidoActaDiferido != null)
            {
                if(pedidoActaDiferido.Count() > 0)
                {
                    foreach(detalle_pedido_diff_acta i in pedidoActaDiferido)
                    {
                        i.Estado = 1;
                        i.EsPedidoLegalizado = 1;
                        conn.Entry(i).State = System.Data.EntityState.Modified;
                    }
                    conn.SaveChanges();
                }
            }
        }

        public string ReportePedidos_ptes(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            int tipo = Convert.ToInt32(data["tipo"]);
            string usuario = data["usuario"];
            List<object> arr = new List<object>();
            if (tipo == 1)
            {
                List<pedidos> p = conn.pedidos.Where(x => x.aprobado_jefe != "1" && x.asignado_a.Equals(usuario) && x.estado != "CANCELADO").ToList();
                arr.Add(p);

            }
            else if (tipo == 2)
            {
                List<pedidos> p = conn.pedidos.Where(x => x.aprobado_jefe == "1" && x.asignado_a.Equals(usuario) && x.estado != "CANCELADO").ToList();
                arr.Add(p);
            }


            return jss.Serialize(arr);
        }

        public string convert_proveedor(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            string nit = data["nit"];
            proveedor p = conn.proveedor.Where(a => a.nit.Equals(nit)).FirstOrDefault();

            return jss.Serialize(p.razon_social);
        }

        #region INFORMES
        public string Info_ped_srp(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            string user = data["usuario"];
            string username = data["username"];
            int tipo = Convert.ToInt32(data["tipo"]);
            List<object> arr = new List<object>();
            views v = conn.views.Where(a => a.name_module.Equals("informe pedidos srp")).FirstOrDefault();
            permisos_usu per = conn.permisos_usu.Where(x => x.id_view.Equals(v.id) && x.usuario.Equals(username)).FirstOrDefault();



            if (per.pe1.Equals("1"))
            {

                List<pedidos> p = conn.pedidos.ToList();

                foreach (var item in p)
                {
                    Informe_ped_srp I = new Informe_ped_srp();
                    I.Id = item.id+"";
                    I.Nro_ped = item.nro_pedido+"";
                    I.Nro_srp = item.nro_srp + "";
                    I.Nro_rp = item.nro_rp + "";
                    I.Nro_contrato = item.nro_contrato +"";
                    I.CECO = Convert.ToInt32(item.ccosto);
                    area_ccosto c = conn.area_ccosto.Where(a => a.ccosto == I.CECO).FirstOrDefault();
                    I.Nombre_ceco = c.area;
                    I.Usuario = item.usuario;
                    I.Asignado = "";
                    List<srp> s = conn.srp.Where(a => a.id_pedido.Equals(item.nro_pedido)).ToList();
                    I.Valor_srp = 0;
                    foreach (var x in s)
                    {
                        I.Valor_srp = Convert.ToDouble(I.Valor_srp + x.total);
                    }
                    if (item.vto_bueno_finan == 1)
                    {
                        I.Apro_finan = "SI";
                    }
                    else
                    {
                        I.Apro_finan = "PENDIENTE";
                    }
                    I.Fecha = item.fecha;
                    I.Estado = item.estado;
                    arr.Add(I);
                }
                

            }
            else
            {

                    List<pedidos> p = conn.pedidos.Where(x =>  x.usuario.Equals(user)).ToList();

                foreach (var item in p)
                {
                    Informe_ped_srp I = new Informe_ped_srp();
                    I.Id = item.id + "";
                    I.Nro_ped = item.nro_pedido + "";
                    I.Nro_srp = item.nro_srp + "";
                    I.Nro_rp = item.nro_rp + "";
                    I.Nro_contrato = item.nro_contrato + "";
                    I.CECO = Convert.ToInt32(item.ccosto);
                    area_ccosto c = conn.area_ccosto.Where(a => a.ccosto == I.CECO).FirstOrDefault();
                    I.Nombre_ceco = c.area;
                    I.Usuario = item.usuario;
                    I.Asignado = "";
                    List<srp> s = conn.srp.Where(a => a.id_pedido.Equals(item.id)).ToList();
                    I.Valor_srp = 0;
                    foreach (var x in s)
                    {
                        I.Valor_srp = Convert.ToDouble(x.total + x.total);
                    }
                    if (item.vto_bueno_finan == 1)
                    {
                        I.Apro_finan = "SI";
                    }
                    else
                    {
                        I.Apro_finan = "PENDIENTE";
                    }
                    I.Fecha = item.fecha;
                    I.Estado = item.estado;

                    arr.Add(I);
                }


            }


            return jss.Serialize(arr);
        }
        #endregion

        public string verificar__aprobar_srp(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            int nro_ped = data["nro_ped"];
            try
            {
                List<object> arr = new List<object>();
                var du = conn.srp.Where(a => a.id_pedido == nro_ped).ToList();
                pedidos p = conn.pedidos.Where(a => a.nro_pedido.Equals(nro_ped)).FirstOrDefault();
                int cant = du.Count();
                int contador = 0;
                DateTime fecha = DateTime.Today;
                string anio = fecha.Year.ToString();
                if (du != null)
                {
                    foreach (var item in du)
                    {
                        presupuesto x = conn.presupuesto.Where(a => a.ccosto.Equals(item.ccosto) && a.cuenta.Equals(item.cuenta) && a.ano.Equals(anio)).FirstOrDefault();
                        if (x != null)
                        {
                            if (item.total <= x.disponibilidad && (x.disponibilidad - item.total >= 0))
                            {
                                item.comprobado = 1;
                                contador = contador + 1;
                                conn.Entry(item).State = System.Data.EntityState.Modified;
                                x.disponibilidad = (Convert.ToDouble(x.disponibilidad - item.total));
                                x.total_ejecucion = (Convert.ToDouble(x.total_ejecucion + item.total));
                                conn.Entry(x).State = System.Data.EntityState.Modified;
                            }
                        }
                    }
                    var dp = conn.depreciacion.Where(a => a.id_pedido == nro_ped).ToList();

                    if (dp != null)
                    {
                        foreach (var item in dp)
                        {
                            DateTime fecha_actual = DateTime.Today;
                            int meses_restantes_año = 12 - fecha_actual.Month;
                            item.valor_ejecutado = item.valor_ejecutado + meses_restantes_año * item.cuota;
                            item.v_util_restante = item.v_util_restante - meses_restantes_año;
                            conn.Entry(item).State = System.Data.EntityState.Modified;

                        }
                    }
                    arr.Add(p.nro_pedido);
                    p.aprobado_jefe = "1";
                    if (cant == contador)
                    {
                        p.estado = "PENDIENTE";
                        arr.Add(p.estado);
                    }
                    conn.Entry(p).State = System.Data.EntityState.Modified;
                    int y = conn.SaveChanges();
                    arr.Add(p.estado);
                }
                return jss.Serialize(arr);
            }
            catch (DbEntityValidationException e)
            {
                string err = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        err += ve.ErrorMessage;
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return jss.Serialize(err);
            }
        }

        //#region gastos
        //public string CreateGastos(string json)
        //{
        //    dynamic data = jss.DeserializeObject(json);
        //    string _json = data["json"];
        //    try
        //    {
        //        gastos c = jss.Deserialize<gastos>(_json);
        //        c.fecha = DateTime.Today;
        //        conn.gastos.Add(c);
        //        int rs = conn.SaveChanges();
        //        return jss.Serialize(rs > 0);
        //    }
        //    catch (DbEntityValidationException e)
        //    {
        //        string err = "";
        //        foreach (var eve in e.EntityValidationErrors)
        //        {
        //            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
        //                eve.Entry.Entity.GetType().Name, eve.Entry.State);
        //            foreach (var ve in eve.ValidationErrors)
        //            {
        //                err += ve.ErrorMessage;
        //                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
        //                    ve.PropertyName, ve.ErrorMessage);
        //            }
        //        }
        //        return jss.Serialize(err);
        //    }
        //}
        //public string UpdateGastos(string json)
        //{
        //    dynamic data = jss.DeserializeObject(json);
        //    string _json = data["json"];
        //    int rs = 0;
        //    gastos c = jss.Deserialize<gastos>(_json);
        //    conn.Entry(c).State = System.Data.EntityState.Modified;
        //    rs = conn.SaveChanges();
        //    //logoUsuario02("MEDICOS", "ACTUALIZAR", codigo_usuario, rs);
        //    return jss.Serialize(rs > 0);
        //}
        //public string ReadGastos(string json)
        //{
        //    dynamic data = jss.DeserializeObject(json);
        //    int id = data["id"];
        //    gastos c = conn.gastos.Where(a => a.id.Equals(id)).FirstOrDefault();
        //    empleado x = conn.empleado.Where(p => p.id.Equals(c.emp_id)).FirstOrDefault();
        //    c.emp_id = Convert.ToInt32(x.codigo);
        //    return jss.Serialize(c);
        //}
        //public string DeleteGastos(string json)
        //{
        //    dynamic data = jss.DeserializeObject(json);
        //    int id = data["id"];
        //    empleado empleado = conn.empleado.Where(a => a.id.Equals(id)).FirstOrDefault();
        //    conn.empleado.Remove(empleado);
        //    int rs = conn.SaveChanges();
        //    return jss.Serialize(rs);
        //}
        //public string reademp(string json)
        //{
        //    dynamic data = jss.DeserializeObject(json);
        //    int id = data["iduser"];
        //    empleado c = conn.empleado.Where(a => a.usu_id.Equals(id)).FirstOrDefault();
        //    return jss.Serialize(c);
        //}
        //#endregion

        //#region nomina
        //public string GenerateNomina(string json)
        //{
        //    dynamic data = jss.DeserializeObject(json);
        //    List<object> arr = new List<object>();
        //    string fechaaux = data["fecha"];
        //    DateTime fecha = Convert.ToDateTime(fechaaux);
        //    int year = DateTime.Now.Year;
        //    int validation = conn.nomina.Where(n => n.fecha_ini <= fecha && n.fecha_fin >= fecha).Count();
        //    if (validation == 0)
        //    {
        //        year y = conn.year.Where(a => a.year1.Equals(year)).FirstOrDefault();
        //        if (y != null)
        //        {
        //            var m = conn.mes.Where(a => a.year_id.Equals(y.id)).ToList();
        //            if (m != null)
        //            {
        //                var quincena1 = m.Where(a => a.dia_ini1 <= fecha && a.dia_fin1 >= fecha).FirstOrDefault();
        //                var quincena2 = m.Where(a => a.dia_ini2 <= fecha && a.dia_fin2 >= fecha).FirstOrDefault();
        //                if (quincena1 != null)
        //                {
        //                    mesaux mesaux1 = new mesaux();
        //                    mesaux1.Id = quincena1.id;
        //                    mesaux1.Year = y.year1;
        //                    mesaux1.Nombre = quincena1.mes1;
        //                    mesaux1.Fecha_ini = Convert.ToDateTime(quincena1.dia_ini1);
        //                    mesaux1.Fecha_fin = Convert.ToDateTime(quincena1.dia_fin1);
        //                    arr.Add(mesaux1);
        //                    var empleados = conn.empleado.ToList();
        //                    if (empleados != null)
        //                    {
        //                        arr.Add(empleados);
        //                    }
        //                    var gastos = conn.gastos.Where(g => g.tipo_gasto == 1 && g.fecha >= quincena1.dia_ini1 && g.fecha <= quincena1.dia_fin1).ToList();
        //                    if (gastos != null)
        //                    {
        //                        arr.Add(gastos);
        //                    }
        //                }
        //                if (quincena2 != null)
        //                {
        //                    mesaux mesaux1 = new mesaux();
        //                    mesaux1.Id = quincena2.id;
        //                    mesaux1.Year = y.year1;
        //                    mesaux1.Nombre = quincena2.mes1;
        //                    mesaux1.Fecha_ini = Convert.ToDateTime(quincena2.dia_ini2);
        //                    mesaux1.Fecha_fin = Convert.ToDateTime(quincena2.dia_fin2);
        //                    arr.Add(mesaux1);
        //                    var empleados = conn.empleado.ToList();
        //                    if (empleados != null)
        //                    {
        //                        arr.Add(empleados);
        //                    }
        //                    var gastos = conn.gastos.Where(g => g.tipo_gasto == 1 && g.fecha >= quincena2.dia_ini2 && g.fecha <= quincena2.dia_fin2).ToList();
        //                    if (gastos != null)
        //                    {
        //                        arr.Add(gastos);
        //                    }
        //                }
        //            }
        //        }
        //        return jss.Serialize(arr);
        //    }
        //    else
        //    {
        //        return jss.Serialize(false);
        //    }
        //}

        //public string SearchNomina(string json)
        //{
        //    dynamic data = jss.DeserializeObject(json);
        //    List<object> arr = new List<object>();
        //    string fechaaux = data["fecha"];
        //    DateTime fecha = Convert.ToDateTime(fechaaux);
        //    int year = DateTime.Now.Year;
        //    int validation = conn.nomina.Where(n => n.fecha_ini <= fecha && n.fecha_fin >= fecha).Count();
        //    if (validation > 0)
        //    {
        //        year y = conn.year.Where(a => a.year1.Equals(year)).FirstOrDefault();
        //        if (y != null)
        //        {
        //            var m = conn.mes.Where(a => a.year_id.Equals(y.id)).ToList();
        //            if (m != null)
        //            {
        //                var quincena1 = m.Where(a => a.dia_ini1 <= fecha && a.dia_fin1 >= fecha).FirstOrDefault();
        //                var quincena2 = m.Where(a => a.dia_ini2 <= fecha && a.dia_fin2 >= fecha).FirstOrDefault();
        //                if (quincena1 != null)
        //                {
        //                    mesaux mesaux1 = new mesaux();
        //                    mesaux1.Id = quincena1.id;
        //                    mesaux1.Year = y.year1;
        //                    mesaux1.Nombre = quincena1.mes1;
        //                    mesaux1.Fecha_ini = Convert.ToDateTime(quincena1.dia_ini1);
        //                    mesaux1.Fecha_fin = Convert.ToDateTime(quincena1.dia_fin1);
        //                    arr.Add(mesaux1);
        //                    var empleados = conn.empleado.ToList();
        //                    if (empleados != null)
        //                    {
        //                        arr.Add(empleados);
        //                    }
        //                    var gastos = conn.gastos.Where(g => g.tipo_gasto == 1 && g.fecha >= quincena1.dia_ini1 && g.fecha <= quincena1.dia_fin1).ToList();
        //                    if (gastos != null)
        //                    {
        //                        arr.Add(gastos);
        //                    }
        //                }
        //                if (quincena2 != null)
        //                {
        //                    mesaux mesaux1 = new mesaux();
        //                    mesaux1.Id = quincena2.id;
        //                    mesaux1.Year = y.year1;
        //                    mesaux1.Nombre = quincena2.mes1;
        //                    mesaux1.Fecha_ini = Convert.ToDateTime(quincena2.dia_ini2);
        //                    mesaux1.Fecha_fin = Convert.ToDateTime(quincena2.dia_fin2);
        //                    arr.Add(mesaux1);
        //                    var empleados = conn.empleado.ToList();
        //                    if (empleados != null)
        //                    {
        //                        arr.Add(empleados);
        //                    }
        //                    var gastos = conn.gastos.Where(g => g.tipo_gasto == 1 && g.fecha >= quincena2.dia_ini2 && g.fecha <= quincena2.dia_fin2).ToList();
        //                    if (gastos != null)
        //                    {
        //                        arr.Add(gastos);
        //                    }
        //                }
        //            }
        //        }
        //        return jss.Serialize(arr);
        //    }
        //    else
        //    {
        //        return jss.Serialize(false);
        //    }
        //}

        //public string CreateNomina(string json)
        //{
        //    dynamic data = jss.DeserializeObject(json);
        //    dynamic list = jss.DeserializeObject(data["grilla"]);
        //    int rs = 0;
        //    try
        //    {
        //        foreach (dynamic item in list)
        //        {
        //            nomina h = new nomina();
        //            if (item["fecha_ini"] != null)
        //            {
        //                h.fecha_ini = Convert.ToDateTime(item["fecha_ini"]);
        //            }
        //            if (item["fecha_fin"] != null)
        //            {
        //                h.fecha_fin = Convert.ToDateTime(item["fecha_fin"]);
        //            }
        //            if (item["monto_pagar"] != null)
        //            {
        //                h.monto_pagar = Convert.ToInt32(item["monto_pagar"]);
        //            }
        //            if (item["monto_desc"] != null)
        //            {
        //                h.monto_desc = Convert.ToInt32(item["monto_desc"]);
        //            }
        //            if (item["emp_id"] != null)
        //            {
        //                h.emp_id = Convert.ToInt32(item["emp_id"]);
        //            }
        //            if (item["mes_id"] != null)
        //            {
        //                h.mes_id = Convert.ToInt32(item["mes_id"]);
        //            }
        //            if (item["usu_id"] != null)
        //            {
        //                h.usu_id = Convert.ToInt32(item["usu_id"]);
        //            }
        //            h.fecha_pago = DateTime.Today;
        //            conn.nomina.Add(h);
        //        }

        //        rs = conn.SaveChanges();
        //        return jss.Serialize(rs > 0);
        //    }
        //    catch (DbEntityValidationException e)
        //    {
        //        string err = "";
        //        foreach (var eve in e.EntityValidationErrors)
        //        {
        //            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
        //                eve.Entry.Entity.GetType().Name, eve.Entry.State);
        //            foreach (var ve in eve.ValidationErrors)
        //            {
        //                err += ve.ErrorMessage;
        //                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
        //                    ve.PropertyName, ve.ErrorMessage);
        //            }
        //        }
        //        return jss.Serialize(err);
        //    }
        //}

        //#endregion

        //#region CARTERA
        //public string PreloadCartera()
        //{
        //    List<object> arr = new List<object>();
        //    decimal d = 0;
        //    var prestamos = conn.prestamo.Where(p => p.estado == 1).Count();
        //    var clientes = conn.clientes.Count();
        //    var pg = conn.prestamo.ToList();
        //    foreach (var item in pg)
        //    {
        //        d += Convert.ToDecimal(item.monto);
        //    }
        //    var pg2 = conn.pagos.ToList();
        //    foreach (var item in pg2)
        //    {
        //        d += Convert.ToDecimal(item.valor_pagar);
        //    }

        //    arr.Add(prestamos);
        //    arr.Add(d);
        //    arr.Add(clientes);


        //    return jss.Serialize(arr);
        //}

        //public string SearchCartera(string json)
        //{
        //    dynamic data = jss.DeserializeObject(json);
        //    string fecha1 = data["fecha1"];
        //    string fecha2 = data["fecha2"];
        //    int op = data["op"];

        //    DateTime f1 = Convert.ToDateTime(fecha1);
        //    DateTime f2 = Convert.ToDateTime(fecha2);
        //    if (op == 1)
        //    {
        //        var c = conn.prestamo.Where(pr => pr.fecha_prestamo >= f1 && pr.fecha_prestamo <= f2).ToList();
        //        if (c != null)
        //        {
        //            return jss.Serialize(c);
        //        }
        //    }
        //    if (op == 2)
        //    {
        //        var c = conn.pagos.Where(pr => pr.fecha_pago >= f1 && pr.fecha_pago <= f2).ToList();
        //        if (c != null)
        //        {
        //            return jss.Serialize(c);
        //        }
        //    }
        //    if (op == 3)
        //    {
        //        List<object> arr = new List<object>();
        //        var c = conn.pagos.Where(pr => pr.fecha_pago >= f1 && pr.fecha_pago <= f2).ToList();
        //        if (c != null)
        //        {
        //            arr.Add(c);
        //        }
        //        var p = conn.prestamo.Where(pr => pr.fecha_prestamo >= f1 && pr.fecha_prestamo <= f2).ToList();
        //        if (p != null)
        //        {
        //            arr.Add(p);
        //        }
        //        return jss.Serialize(arr);
        //    }
        //    else
        //    {
        //        return jss.Serialize(false);
        //    }
        //}
        //#endregion

        //#region PROXIMOS A VENCER
        //public string ProximosVencer()
        //{
        //    var proximos = conn.proximos_vencer();

        //    return jss.Serialize(proximos);
        //}
        //#endregion

        //#region PLANILLA PRESTAMOS
        //public string ReadPlanilla()
        //{
        //    jss.MaxJsonLength = 500000000;
        //    conn.Configuration.ValidateOnSaveEnabled = false;
        //    conn.Configuration.AutoDetectChangesEnabled = false;
        //    conn.Configuration.LazyLoadingEnabled = false;
        //    conn.Configuration.ProxyCreationEnabled = false;

        //    string stringCon = System.Configuration.ConfigurationManager.ConnectionStrings["rhinoEntities"].ConnectionString;
        //    string[] vec = stringCon.Split(';');
        //    string preStrCon = vec[2].Substring(28) + ";" + vec[3] + ";" + vec[4] + ";" + vec[5] + ";" + vec[6];
        //    using (SqlConnection con = new SqlConnection(preStrCon))
        //    {
        //        try
        //        {
        //            con.Open();
        //            var commandStr = "select id_prestamo,tipo_interes_id, c.codigo, CONCAT(UPPER(apellidos),' ',nombres) as cliente,c.direccion,FORMAT( fecha_prestamo, 'dd/MM/yyyy', 'en-US' ) as fecha_prestamo,FORMAT(p.fecha_corte, 'dd/MM/yyyy', 'en-US' ) as fecha_corte,vlr_prestado,interes,p.vlr_cuotas,ti.nombre as forma_pago, case p.tipo_prestamo when 0 then 'Compuesto' when 1 then 'Simple' end as tprestamo,dias from prestamo as p INNER JOIN tipo_interes as ti on p.tipo_interes_id = ti.id_tipo_inte INNER JOIN clientes as c on p.cliente_id = c.id_cliente where p.estado = 1";
        //            SqlCommand sql = new SqlCommand(commandStr, con);
        //            SqlDataReader read = sql.ExecuteReader();
        //            string str = WriteReaderToJSON(read);
        //            con.Close();
        //            return (str);
        //        }
        //        catch (DbEntityValidationException e)
        //        {
        //            string err = "";
        //            foreach (var eve in e.EntityValidationErrors)
        //            {
        //                Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
        //                    eve.Entry.Entity.GetType().Name, eve.Entry.State);
        //                foreach (var ve in eve.ValidationErrors)
        //                {
        //                    err += ve.ErrorMessage;
        //                    Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
        //                        ve.PropertyName, ve.ErrorMessage);
        //                }
        //            }
        //            return jss.Serialize(err);
        //        }
        //    }
        //}

        //#endregion


        //ESTRUCTURA PARA REALIZAR UNA CONSULTA
        public string consulta(string json)
        {
            jss.MaxJsonLength = 500000000;
            conn.Configuration.ValidateOnSaveEnabled = false;
            conn.Configuration.AutoDetectChangesEnabled = false;
            conn.Configuration.LazyLoadingEnabled = false;
            conn.Configuration.ProxyCreationEnabled = false;
            dynamic data = jss.DeserializeObject(json);

            string stringCon = System.Configuration.ConfigurationManager.ConnectionStrings["rhinoEntities"].ConnectionString;
            string[] vec = stringCon.Split(';');
            string preStrCon = vec[2].Substring(28) + ";" + vec[3] + ";" + vec[4] + ";" + vec[5] + ";" + vec[6];
            using (SqlConnection con = new SqlConnection(preStrCon))
            {
                try
                {
                    con.Open();
                    var commandStr = data["query"];
                    SqlCommand sql = new SqlCommand(commandStr, con);
                    SqlDataReader read = sql.ExecuteReader();
                    string str = WriteReaderToJSON(read);
                    con.Close();
                    return (str);
                }
                catch (DbEntityValidationException e)
                {
                    string err = "";
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            err += ve.ErrorMessage;
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    return jss.Serialize(err);
                }
            }
        }

        private string WriteReaderToJSON(IDataReader reader)
        {
            StringBuilder sb = new StringBuilder();

            if (reader == null || reader.FieldCount == 0)
            {
                sb.Append("null");
                return "";
            }

            int rowCount = 0;

            sb.Append("[");

            while (reader.Read())
            {
                sb.Append("{");
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    sb.Append("\"" + reader.GetName(i) + "\":");
                    sb.Append("\"" + reader[i] + "\"");
                    sb.Append(i == (reader.FieldCount - 1) ? "" : ",");
                }
                sb.Append("},");
                rowCount++;
            }
            if (rowCount > 0)
            {
                int index = sb.ToString().LastIndexOf(",");
                sb.Remove(index, 1);
            }
            sb.Append("]");
            return sb.ToString();
        }
        public string encriptar(string text)
        {
            string enc = text.Trim().ToUpper();
            int lon = enc.Length;
            string j = "";
            int k = 0;
            string res = "";

            if (lon > 0)
            {
                for (int i = 0; i < lon; i++)
                {
                    j = enc.Substring(i, 1);
                    k = ((((Encoding.ASCII.GetBytes(j)[0]) + i) * lon) % 255);
                    res += "" + (char)k;
                }
            }

            return res;
        }
        public class combo
        {
            public string key { set; get; }
            public string values { set; get; }
        }
        public string log_user(int tipo, int id_tipo, int id_user, string hora )
        {
            log_user l = new log_user();

            l.tipo = tipo;
            l.id_tipo = id_tipo;
            l.fecha = DateTime.Today;
            l.hora = hora;
            l.id_usuario = id_user;

            conn.log_user.Add(l);
            int rs = conn.SaveChanges();

            return jss.Serialize(rs > 0);
        }

        public string Validarpermisos(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            string username = data["username"];
            string name_view = data["name_view"];

            views v = conn.views.Where(a => a.name_module.Equals(name_view)).FirstOrDefault();
            permisos_usu per = conn.permisos_usu.Where(x => x.id_view.Equals(v.id) && x.usuario.Equals(username)).FirstOrDefault();


            return jss.Serialize(per);
        }

        public string Read_User(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            string codigo = data["cedula"];

            empleado v = conn.empleado.Where(a => a.cedula_emp.Equals(codigo)).FirstOrDefault();

            return jss.Serialize(v);
        }

        public string execute_saldos_pdte_af()
        {
            List<object> arr = new List<object>();
            var dp = conn.depreciacion.ToList();
            DateTime fecha_actual = DateTime.Now;
            foreach (dynamic item in dp)
            {
                string cuenta = item.cuenta;
                string ano = item.ano;
                string ccosto = item.ccosto;
                int valor_desc = 0;
                int nueva_vida = 0;
                presupuesto p = conn.presupuesto.Where(a => a.cuenta.Equals(cuenta) && a.ccosto.Equals(ccosto) && a.ano.Equals(ano)).FirstOrDefault();
                if (item.v_util_restante >= 12)
                {
                    valor_desc = item.cuota * 12;
                    nueva_vida = item.v_util_restante - 12;
                }
                else
                {
                    valor_desc = item.cuota * item.v_util_restante;
                    nueva_vida = 0;
                }
                item.valor = valor_desc;
                if (p != null){
                    if (Convert.ToInt32(item.ano) != fecha_actual.Year) { 
                     if(p.disponibilidad >= valor_desc)
                        {
                            p.disponibilidad = p.disponibilidad - valor_desc;
                            p.total_ejecucion = p.total_ejecucion + valor_desc;
                            item.valor_ejecutado = item.valor_ejecutado + valor_desc;
                            item.fecha_ejecucion = DateTime.Now;
                            item.v_util_restante = nueva_vida;
                            item.ano = (fecha_actual.Year)+"";
                            conn.Entry(p).State = System.Data.EntityState.Modified;
                            conn.Entry(item).State = System.Data.EntityState.Modified;
                        }
                        else
                        {
                            arr.Add(item);
                        }
                        int rs = conn.SaveChanges();
                    }
                    else
                    {
                        arr.Add(item);
                    }
                }
                else
                {
                     arr.Add(item);
                }


            }
            return jss.Serialize(arr);
        }

        public string Reporte_planilla_nomina(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            int filtro = Convert.ToInt32(data["tipo"]);
            List<object> arr = new List<object>();
            if(filtro == 1){
                var p = conn.planillas_nomina.Where(a => a.estado.Equals("PENDIENTE")).ToList();
                arr.Add(p);
            }
            else if (filtro == 2) {
                var p = conn.planillas_nomina.Where(a => a.estado.Equals("APROBADO")).ToList();
                arr.Add(p);
            }
            else if (filtro == 3)
            {
                var p = conn.planillas_nomina.Where(a => a.estado.Equals("RETENIDO")).ToList();
                arr.Add(p);
            }
            else if (filtro == 4)
            {
                var p = conn.planillas_nomina.ToList();
                arr.Add(p);
            }



            return jss.Serialize(arr);
        }

        public string Read_planillas_aprobada_nomina()
        {
            List<object> arr = new List<object>();

                var p = conn.planillas_nomina.Where(a => a.estado.Equals("APROBADO")).ToList();
                arr.Add(p);

            return jss.Serialize(arr);
        }

        public string Reporte_activos_pendientes()
        {
            //dynamic data = jss.DeserializeObject(json);
            List<object> arr = new List<object>();
                var p = conn.depreciacion.Where(a => a.inventario != 3).ToList();
                arr.Add(p);

            return jss.Serialize(arr);
        }

        public string Ingreso_depreciacion_af(string json)
        {
            try
            {
                dynamic data = jss.DeserializeObject(json);
                string _json = data["json"];
                detalle_depre dp = new detalle_depre();
                dynamic head = jss.DeserializeObject(data["json"]);
                dp.id_pedido = head["id_pedido"];
                dp.descripcion = head["descripcion"];
                dp.cuenta = head["cuenta"];
                dp.valor_activo = head["valor"];
                dp.v_util = head["v_util"];
                dp.depre_acumulada = 0;
                dp.saldo_libros = head["valor"];
                dp.cuota = head["cuota"];
                dp.v_util_restante = head["v_util"];
                dp.rubro = head["rubro"];
                dp.ccosto = head["ccosto"];
                dp.placa_af = head["placa_af"];

                conn.detalle_depre.Add(dp);
                int id_ref = head["id"];
                string  placad=head["placa_af"];
               
                depreciacion d = conn.depreciacion.Where(a => a.id == id_ref).FirstOrDefault();
                d.inventario = 3;
                int rs2 = conn.SaveChanges();
     
                // detalle_depre e = conn.detalle_depre.OrderByDescending(y => y.id_depreciacion).FirstOrDefault();
                depreciacion_2022 dy = new depreciacion_2022();
               // if (e.placa_af.Equals(head["placa_af"]))
                //{
                    // if (e.placa_af.Equals(head["placa_af"])) {
                    dy.id_depre = dp.id_depreciacion;
               // dy.id_depre = e.id_depreciacion;
                dy.placa = head["placa_af"];
                dy.cuota = Convert.ToString(head["cuota"]);
                dy.mes_ult_eje = 0;
                conn.depreciacion_2022.Add(dy);

                int rs = conn.SaveChanges();
               // }

                return jss.Serialize(rs2);
            }
            catch (DbEntityValidationException e)
            {
                string err = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        err += ve.ErrorMessage;
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return jss.Serialize("{err:" + err + "}");
            }
        }

        public string Reporte_depreciacion_af()
        {
            //dynamic data = jss.DeserializeObject(json);
            List<object> arr = new List<object>();
            var p = conn.detalle_depre.ToList();
            arr.Add(p);

            return jss.Serialize(arr);
        }

        public string Depreciar_af()
        {
            //dynamic data = jss.DeserializeObject(json);
            List<object> arr = new List<object>();
            var p = conn.detalle_depre.ToList();

            DateTime fecha = DateTime.Today;
            int mes_actual = fecha.Month;

            foreach (dynamic item in p)
            {
                if(item.mes_ult_ejecucion != mes_actual)
                {
                    if (item.v_util_restante > 0) {
                        item.depre_acumulada = item.depre_acumulada + item.cuota;
                        item.saldo_libros = item.saldo_libros - item.cuota;
                        item.v_util_restante = item.v_util_restante - 1;
                        item.mes_ult_ejecucion = mes_actual;
                        item.fecha_ejecucion = fecha;
                        if (item.fecha_ini_dep == null)
                        {
                            item.fecha_ini_dep = fecha;
                        }
                        string af = item.placa_af;
                        depreciacion_2021 d = conn.depreciacion_2021.Where(a => a.placa == af).FirstOrDefault();
                        DateTime fechaActual = DateTime.Now;
                        string mes = fechaActual.ToString("MMMM");
                        string query = "UPDATE depreciacion_2021 set " + mes + "= cuota where placa = '" + af + "'";
                        createobject js = new createobject();
                        js.query = query;
                        string result = consulta(jss.Serialize(js));
                        conn.Entry(item).State = System.Data.EntityState.Modified;
                    }
                }

                 
            }
            int rs = conn.SaveChanges();

            return jss.Serialize(rs);


        }

        /*shirley se mejora la consulta  22_12_2021
         */
        public string Depreciar_af_nuevo()
        {
            DateTime fecha = DateTime.Today;
            int mes_actual = fecha.Month;
            List<object> arr = new List<object>();
            var procedure = conn.Depreciar_af();
            int rs = conn.SaveChanges();

            return jss.Serialize(rs);


        }

        public class createobject
        {
            public string query { get; set; }
        }

        public string ExportarPedidosFinanciera()
        {

            List<object> arr = new List<object>();

            var p = conn.pedidos.Where(a => a.nro_srp != null  && a.vto_bueno_finan == 0 ).ToList();

            foreach (var item in p)
            {

                Report_financiera r1 = new Report_financiera();

                r1.Nro_pedido = item.nro_pedido;
                r1.Fecha = item.fecha.ToString("dd/MM/yyyy");
                r1.Ceco = item.ccosto;
                r1.Usuario = item.usuario;
                r1.Valor = item.vlr_total;
                string nit = item.proveedor;
                proveedor d = conn.proveedor.Where(a => a.nit.Equals(nit)).FirstOrDefault();
                if (d != null)
                {
                    r1.Proveedor = d.razon_social;
                }
                r1.Estado = "PENDIENTE";
                r1.Justificacion = item.justificacion;

                arr.Add(r1);

            }

            return jss.Serialize(arr);

        }

        public string ExportarPedidosCompras()
        {

            List<object> arr = new List<object>();

            var p = conn.pedidos.Where(a => a.estado.Equals("APROBADO")).ToList();

            foreach (var item in p)
            {

                Report_compras r1 = new Report_compras();

                r1.Nro_pedido = item.nro_pedido;
                r1.Fecha = item.fecha.ToString("dd/MM/yyyy");
                r1.Ceco = item.ccosto;
                r1.Usuario = item.usuario;
                r1.Valor = item.vlr_total;
                string nit = item.proveedor;
                proveedor d = conn.proveedor.Where(a => a.nit.Equals(nit)).FirstOrDefault();
                if(d != null)
                {
                    r1.Proveedor = d.razon_social;
                }
                r1.Estado = item.estado;
                r1.Justificacion = item.justificacion;
                r1.Nro_srp = Convert.ToString(item.nro_srp);
                r1.Nro_rp = Convert.ToString(item.nro_rp);
                r1.Nro_contrato = item.nro_contrato;
                r1.Obs_compras = item.obs_compras;
                arr.Add(r1);

            }

            return jss.Serialize(arr);

        }

        #region RECORTE_PEDIDOS
        public string recorte_pedido(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            int pedido = data["nro_pedido"];
            string cuenta = data["cuenta"];
            string ceco = data["ceco"];
            int disponible = data["disponible"];
            int saldo_reducir = data["saldo_reducir"];
            string justificacion = data["justificacion"];
            DateTime fecha = DateTime.Today;
            string anio = fecha.Year.ToString();
            presupuesto p = conn.presupuesto.Where(a => a.cuenta.Equals(cuenta) && a.ccosto.Equals(ceco) && a.ano.Equals(anio)).FirstOrDefault();

            histo_ajuste_presu ha = new histo_ajuste_presu();
            ha.id_presupuesto = p.id;
            ha.fecha = fecha;
            ha.valor_ant = p.disponibilidad;
            ha.observacion = "DEVOLUCION POR REDUCCION DE PEDIDO CORRESPONDE AL PEDIDO NRO " + pedido;
            ha.nuevo_saldo = p.disponibilidad + saldo_reducir;


            p.disponibilidad = p.disponibilidad + saldo_reducir;
            p.total_ejecucion = p.total_ejecucion - saldo_reducir;

            srp s = conn.srp.Where(a => a.id_pedido.Equals(pedido) && a.cuenta.Equals(cuenta) && a.ccosto.Equals(ceco)).FirstOrDefault();
            s.total = s.total - saldo_reducir;

            conn.Entry(p).State = System.Data.EntityState.Modified;
            conn.Entry(s).State = System.Data.EntityState.Modified;
            conn.histo_ajuste_presu.Add(ha);

            int rs = conn.SaveChanges();
            return jss.Serialize(rs);
        }

        #endregion

        #region PRESUPUESTO T Y P
        public string create_presion(string json)
        {
             dynamic data = jss.DeserializeObject(json);
             string _json = data["json"];
             dynamic list = jss.DeserializeObject(data["tabla"]);
             dynamic head = jss.DeserializeObject(data["json"]);
             int rs = 0;
             List<object> arr = new List<object>();
             try
             {


                 presiones_presupuesto PP = conn.presiones_presupuesto.OrderByDescending(y => y.id).FirstOrDefault();
                 int consecutivo1 = PP.id + 1;

                 presiones_presupuesto c = jss.Deserialize<presiones_presupuesto>(_json);

                     conn.presiones_presupuesto.Add(c);
                     foreach (dynamic item in list)
                     {
                         detalle_presiones r = new detalle_presiones();

                         r.id_presion = consecutivo1;
                         r.id_presupuesto = Convert.ToInt32(item["id_presupuesto"]);
                         r.ccosto = item["ccosto"];
                         r.cuenta = item["cuenta"];
                         r.rubro = item["rubro"];
                         r.valor = Convert.ToInt32(item["valor"]);
                         presupuesto o = conn.presupuesto.Where(p => p.id.Equals(r.id_presupuesto)).FirstOrDefault();
                         Double valor_ant = Convert.ToDouble(o.disponibilidad);
                         if (o != null)
                         {
                         o.disponibilidad = o.disponibilidad + Convert.ToInt32(r.valor);
                         o.indice = "UP";
                             conn.Entry(o).State = System.Data.EntityState.Modified;
                         }
                         histo_ajuste_presu h = new histo_ajuste_presu();
                        // presiones_presupuesto PP = conn.presiones_presupuesto.OrderByDescending(y => y.id).FirstOrDefault();
                         h.fecha = DateTime.Now;
                         h.id_presupuesto = Convert.ToInt32(item["id_presupuesto"]);
                         h.nuevo_saldo = o.disponibilidad;

                         h.observacion =  c.justificacion+ " Consecutivo "+consecutivo1 + " RESPONSABLE " + c.usuario;
                         h.valor_ant = valor_ant;
                         conn.histo_ajuste_presu.Add(h);

                     conn.detalle_presiones.Add(r);


                     }


                 int a = conn.SaveChanges();
                 arr.Add(a);
                 if (a > 0) { 
                     PP = conn.presiones_presupuesto.OrderByDescending(y => y.id).Where(y => y.usuario.Equals(c.usuario)).FirstOrDefault();
                     arr.Add(PP.id);

                 }
                 return jss.Serialize(arr);
             }
             catch (DbEntityValidationException e)
             {
                 string err = "";
                 foreach (var eve in e.EntityValidationErrors)
                 {
                     Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                         eve.Entry.Entity.GetType().Name, eve.Entry.State);
                     foreach (var ve in eve.ValidationErrors)
                     {
                         err += ve.ErrorMessage;
                         Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                             ve.PropertyName, ve.ErrorMessage);
                     }
                 }

                return jss.Serialize(err);
            }
          
        }
        public string Solicitud_create_presion(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            string _json = data["json"];
            dynamic list = jss.DeserializeObject(data["tabla"]);
            dynamic head = jss.DeserializeObject(data["json"]);
            int rs = 0;
            List<object> arr = new List<object>();
            try
            {

                presiones_presupuesto c = jss.Deserialize<presiones_presupuesto>(_json);
                c.estado = "PENDIENTE";
                c.vto_bueno_fina = 0;
                conn.presiones_presupuesto.Add(c);
                presiones_presupuesto PP = conn.presiones_presupuesto.OrderByDescending(y => y.id).FirstOrDefault();
                int consecutivo = PP.id + 1;
                foreach (dynamic item in list)
                {
                    detalle_presiones r = new detalle_presiones();

                    r.id_presion = consecutivo;
                    r.id_presupuesto = Convert.ToInt32(item["id_presupuesto"]);
                    r.ccosto = item["ccosto"];
                    r.cuenta = item["cuenta"];
                    r.rubro = item["rubro"];
                    r.valor = Convert.ToInt64(item["valor"]);
                    r.ejecutado=0;
                    conn.detalle_presiones.Add(r);


                }


                int a = conn.SaveChanges();
                arr.Add(a);
                if (a > 0)
                {
                 
                    PP = conn.presiones_presupuesto.OrderByDescending(y => y.id).Where(y => y.usuario.Equals(c.usuario)).FirstOrDefault();
                    arr.Add(PP.id);



                    return jss.Serialize(arr);
                }
                return jss.Serialize(arr);

            }

            catch (DbEntityValidationException e)
            {
                string err = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        err += ve.ErrorMessage;
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return jss.Serialize(err);
            }
        }





        public string create_traslado(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            string _json = data["json"];
            dynamic list = jss.DeserializeObject(data["json2"]);
            dynamic head = jss.DeserializeObject(data["json"]);
            int rs = 0;
            List<object> arr = new List<object>();
            try
            {

                traslados_presupuesto c = jss.Deserialize<traslados_presupuesto>(_json);

                conn.traslados_presupuesto.Add(c);
                foreach (dynamic item in list)
                {
                    detalle_traslados r = new detalle_traslados();

                    r.id_presu_ini = Convert.ToInt32(item["id_presu_ini"]);
                    r.id_presu_fin = Convert.ToInt32(item["id_presu_fin"]);
                    r.ccosto_ini = item["ccosto_ini"];
                    r.cuenta_ini = item["cuenta_ini"];
                    r.rubro_ini = item["rubro_ini"];
                    r.disponible_ini = Convert.ToInt32(item["disponible_fin"]);
                    r.ccosto_fin = item["ccosto_fin"];
                    r.cuenta_fin = item["cuenta_fin"];
                    r.rubro_fin = item["rubro_fin"];
                    r.disponible_fin = Convert.ToInt32(item["disponible_fin"]);
                    r.valor_traslado = item["valor_traslado"];
                    presupuesto pr1 = conn.presupuesto.Where(y => y.id.Equals(r.id_presu_ini)).FirstOrDefault();
                    presupuesto pr2 = conn.presupuesto.Where(y => y.id.Equals(r.id_presu_fin)).FirstOrDefault();
                    Double disponibilidad = Convert.ToDouble(pr1.disponibilidad);
                    if (pr1 != null)
                    {
                        if (Convert.ToInt32(pr1.disponibilidad) >= r.valor_traslado)
                        {
                            //presupuesto INICIAL
                            double vlr_ant_p1 = pr1.disponibilidad;
                            double vlr_ant_p2 = pr2.disponibilidad;
                            //pr1.total_presupuesto = pr1.total_presupuesto - j1["valor_traslado"];
                            pr1.disponibilidad = Convert.ToDouble(disponibilidad - Convert.ToDouble(r.valor_traslado));
                            pr1.indice = "RIGHT";
                            //presupuesto FINAL
                            //pr2.total_presupuesto = j1["nuevo_total"];
                            pr2.disponibilidad = pr2.disponibilidad + Convert.ToDouble(r.valor_traslado);
                            pr2.indice = "UP";
                            conn.Entry(pr1).State = System.Data.EntityState.Modified;
                            conn.Entry(pr2).State = System.Data.EntityState.Modified;
                            //HISTORICO PRESUPUESTO INICIAL
                            histo_ajuste_presu h = new histo_ajuste_presu();
                            traslados_presupuesto TP = conn.traslados_presupuesto.OrderByDescending(y => y.id).FirstOrDefault();
                            int consecutivo = TP.id + 1;
                            h.fecha = DateTime.Now;
                            h.id_presupuesto = r.id_presu_ini;
                            h.observacion = "TRASLADO SALDO A CUENTA: " + r.cuenta_fin + " CECO: " + r.ccosto_fin +" JUSTIFICACION "+ c.justificacion+" CONSECUTIVO "+consecutivo +" RESPONSABLE "+ c.usuario;
                            h.valor_ant = vlr_ant_p1;
                            h.nuevo_saldo = pr1.disponibilidad;
                            conn.histo_ajuste_presu.Add(h);
                            //HISTORICO PRESUPUESTO FINAL
                            histo_ajuste_presu h2 = new histo_ajuste_presu();
                            h2.fecha = DateTime.Now;
                            h2.id_presupuesto = r.id_presu_fin;
                            h2.observacion = "RECIBE SALDO DE CUENTA: " + r.cuenta_ini + "CECO: " + r.ccosto_ini + " JUSTIFICACION " + " CONSECUTIVO " + consecutivo + " RESPONSABLE " + c.usuario;
                            h2.valor_ant = vlr_ant_p2;
                            h2.nuevo_saldo = pr2.disponibilidad;
                            conn.histo_ajuste_presu.Add(h2);
                        }




                        conn.detalle_traslados.Add(r);


                    }
                    int a = conn.SaveChanges();
                    arr.Add(a);
                    if (a > 0)
                    {
                        traslados_presupuesto PP = conn.traslados_presupuesto.OrderByDescending(y => y.id).Where(y => y.usuario.Equals(c.usuario)).FirstOrDefault();
                      
  
                        arr.Add(PP.id);

                    }

                }

                return jss.Serialize(arr);
            }
            catch (DbEntityValidationException e)
            {
                string err = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        err += ve.ErrorMessage;
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return jss.Serialize(err);
            }
        }

        public string create_traslado2(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            string _json = data["json"];
            dynamic list = jss.DeserializeObject(data["json2"]);
            dynamic head = jss.DeserializeObject(data["json"]);
            int rs = 0;
            List<object> arr = new List<object>();
            try
            {

                traslados_presupuesto c = jss.Deserialize<traslados_presupuesto>(_json);
                c.vto_bueno_finan = 0;
                c.estado = "PENDIENTE";
                traslados_presupuesto TP = conn.traslados_presupuesto.OrderByDescending(y => y.id).FirstOrDefault();
                int consecutivo = TP.id + 1;

                conn.traslados_presupuesto.Add(c);
                foreach (dynamic item in list)
                {
                    detalle_traslados r = new detalle_traslados();
                    r.id_traslado = consecutivo;
                    r.id_presu_ini = Convert.ToInt32(item["id_presu_ini"]);
                    r.id_presu_fin = Convert.ToInt32(item["id_presu_fin"]);
                    r.ccosto_ini = item["ccosto_ini"];
                    r.cuenta_ini = item["cuenta_ini"];
                    r.rubro_ini = item["rubro_ini"];
                    r.disponible_ini = Convert.ToDouble(item["disponible_ini"]);
                    r.ccosto_fin = item["ccosto_fin"];
                    r.cuenta_fin = item["cuenta_fin"];
                    r.rubro_fin = item["rubro_fin"];
                    r.disponible_fin = Convert.ToInt32(item["disponible_fin"]);
                    r.valor_traslado = item["valor_traslado"];
                    presupuesto pr1 = conn.presupuesto.Where(y => y.id.Equals(r.id_presu_ini)).FirstOrDefault();
                    presupuesto pr2 = conn.presupuesto.Where(y => y.id.Equals(r.id_presu_fin)).FirstOrDefault();
                    Double disponibilidad = Convert.ToDouble(pr1.disponibilidad);
                   if (pr1 != null)
                    {
                        if (Convert.ToInt64(pr1.disponibilidad) >= r.valor_traslado)
                        {
                            //presupuesto INICIAL
                            double vlr_ant_p1 = pr1.disponibilidad;
                            double vlr_ant_p2 = pr2.disponibilidad;
                           // pr1.disponibilidad = Convert.ToDouble(disponibilidad - Convert.ToDouble(r.valor_traslado));
                           // pr1.indice = "RIGHT";
                            //pr2.disponibilidad = pr2.disponibilidad + Convert.ToDouble(r.valor_traslado);
                            //pr2.indice = "UP";
                           // conn.Entry(pr1).State = System.Data.EntityState.Modified;
                           // conn.Entry(pr2).State = System.Data.EntityState.Modified;
                            //HISTORICO PRESUPUESTO INICIAL
                           // histo_ajuste_presu h = new histo_ajuste_presu();
                            
                           // h.fecha = DateTime.Now;
                            //h.id_presupuesto = r.id_presu_ini;
                           // h.observacion = "TRASLADO SALDO A CUENTA: " + r.cuenta_fin + " CECO: " + r.ccosto_fin + " JUSTIFICACION " + c.justificacion + " CONSECUTIVO " + consecutivo + " RESPONSABLE " + c.usuario;
                           // h.valor_ant = vlr_ant_p1;
                           // h.nuevo_saldo = pr1.disponibilidad;
                          //  conn.histo_ajuste_presu.Add(h);
                            //HISTORICO PRESUPUESTO FINAL
                            //histo_ajuste_presu h2 = new histo_ajuste_presu();
                            //h2.fecha = DateTime.Now;
                            //h2.id_presupuesto = r.id_presu_fin;
                            //h2.observacion = "RECIBE SALDO DE CUENTA: " + r.cuenta_ini + "CECO: " + r.ccosto_ini + " JUSTIFICACION " + " CONSECUTIVO " + consecutivo + " RESPONSABLE " + c.usuario;
                            //h2.valor_ant = vlr_ant_p2;
                            //h2.nuevo_saldo = pr2.disponibilidad;
                            //conn.histo_ajuste_presu.Add(h2);
                        }




                        conn.detalle_traslados.Add(r);


                    }
                    int a = conn.SaveChanges();
                    arr.Add(a);
                    if (a > 0)
                    {
                        traslados_presupuesto PP = conn.traslados_presupuesto.OrderByDescending(y => y.id).Where(y => y.usuario.Equals(c.usuario)).FirstOrDefault();


                        arr.Add(PP.id);

                    }

                }

                return jss.Serialize(arr);
            }
            catch (DbEntityValidationException e)
            {
                string err = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        err += ve.ErrorMessage;
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return jss.Serialize(err);
            }
        }

        public string preload_comodato()
        {
            List<object> arr = new List<object>();
            List<como_dato> cd = conn.como_dato.ToList();

            arr.Add(cd);

            return jss.Serialize(arr);
        }
        
        public string ReporteDetallePedido()
        {
            
            List<object> arr = new List<object>();
            List<detalle_pedido> cd = conn.detalle_pedido.ToList();
            List<pedidos> pd =conn.pedidos.Where(a => a.estado.Equals("PENDIENTE")).ToList();
          
            arr.Add(cd);
            arr.Add(pd);
          


            return jss.Serialize(arr);
        }
        
        public string Read_comodato(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            int codigo =data["id"];

            como_dato v = conn.como_dato.Where(a => a.codigo == codigo).FirstOrDefault();

            return jss.Serialize(v);
        }

        public string Update_Comodato(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            string _json = data["json"];
            int rs = 0;
            como_dato c = jss.Deserialize<como_dato>(_json);
            conn.Entry(c).State = System.Data.EntityState.Modified;
            rs = conn.SaveChanges();
            //logoUsuario02("MEDICOS", "ACTUALIZAR", codigo_usuario, rs);
            return jss.Serialize(rs > 0);
        }

        public string CreateComodato(string json)
        {
            dynamic data = jss.DeserializeObject(json);
            string _json = data["json"];
            try
            {
                como_dato c = jss.Deserialize<como_dato>(_json);
                conn.como_dato.Add(c);
                int rs = conn.SaveChanges();
                return jss.Serialize(rs > 0);
            }
            catch (DbEntityValidationException e)
            {
                string err = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        err += ve.ErrorMessage;
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return jss.Serialize(err);
            }

        }


        #endregion

        #region LISTAR ARTICULOS AMORTIZABLES
        public string ListarArticulosArmortizables()
        {
            string s = "";
            List<listaPedidos> listPedidos = new List<listaPedidos>();
            var connection = System.Configuration.ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
            string query = "select p.nro_pedido, af.descripcion, dp.total, ptd.tiempo, s.total as TotalAmortizable, asn.nro_factura, das.ceco, das.cuenta, c.rubro \n" +
                            "from pedidos p inner join detalle_pedido dp on p.nro_pedido = dp.id_pedido \n" +
                            "inner join pedidos_tiempo_diff ptd on p.nro_pedido = ptd.id_pedido \n" +
                            "inner join articulos_diff af on dp.codigo_art = af.codigo \n" +
                            "inner join detalle_pedido_diff_acta dpda on p.nro_pedido = dpda.id_pedido \n" +
                            "inner join detalle_acta_satis das on p.nro_pedido = das.nro_pedido \n" +
                            "inner join acta_satisfaccion_nueva asn on das.consecutivo = asn.consecutivo \n" +
                            "inner join srp s on p.nro_pedido = s.id_pedido \n" +
                            "inner join cuentas c on das.cuenta = c.cuenta \n" +
                            "where dpda.Estado = 1 and dpda.EsPedidoLegalizado = 1 and dpda.EsActivado = 0";
            using (SqlConnection con = new SqlConnection(connection.ToString()))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader srd = cmd.ExecuteReader())
                    {
                        while (srd.Read())
                        {
                            listPedidos.Add(new listaPedidos
                            {
                                nro_pedido = Convert.ToInt32(srd["nro_pedido"]),
                                codigo_art = srd["codigo_art"]+"",
                                descripcion = srd["descripcion"]+"",
                                total = Convert.ToDouble(srd["total"]),
                                tiempo = Convert.ToInt32(srd["tiempo"]),
                                TotalAmortizable = Convert.ToDouble(srd["TotalAmortizable"]),
                                nro_factura = srd["nro_factura"]+"",
                                ceco = srd["ceco"]+"",
                                cuenta = srd["cuenta"]+"",
                                rubro = srd["rubro"]+""
                            });
                        }
                    }
                    con.Close();
                }
            }

                return jss.Serialize(listPedidos);
        }

        public string ListarpedidosAmortizar()
        {
            string s = "";
            List<listarPedidoAmortizacion> pedidosAmortizacion = new List<listarPedidoAmortizacion>();
            var connection = System.Configuration.ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
            string query = "select a.id_pedido, a.cod_art, a.descripcion, a.valor as valorSrp,a.v_util,a.cuota, \n"+
                            "isnull((select top(1)nro_factura from acta_satisfaccion_nueva asn inner join detalle_acta_satis das on asn.consecutivo = das.consecutivo where das.nro_pedido = a.id_pedido), 'SIN FACTURA') as NumeroFac, \n"+
                            "a.ccosto, a.cuenta,a.rubro, count(a.id_pedido) as cantidadpedido, \n"+
                            "(select count(*) from amortizacion where EsIngresada = 1 and id_pedido = a.id_pedido) as Amortizar from amortizacion a \n"+
                            "group by a.id_pedido,a.cod_art,a.descripcion,a.valor,a.v_util,a.cuota,a.ccosto,a.cuenta,a.rubro";
            using (SqlConnection con = new SqlConnection(connection.ToString()))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader srd = cmd.ExecuteReader())
                    {
                        while (srd.Read())
                        {
                            pedidosAmortizacion.Add(new listarPedidoAmortizacion
                            {
                                id_pedido = Convert.ToInt32(srd["id_pedido"]),
                                cod_art = srd["cod_art"] + "",
                                descripcion = srd["descripcion"] + "",
                                valorSrp = Convert.ToDecimal(srd["valorSrp"]),
                                v_util = Convert.ToInt32(srd["v_util"]),
                                cuota = Convert.ToInt32(srd["cuota"]),
                                NumeroFac = srd["NumeroFac"]+"",
                                ccosto = srd["ccosto"]+"",
                                cuenta = srd["cuenta"]+"",
                                rubro = srd["rubro"]+"",
                                cantidadpedido = Convert.ToInt32(srd["cantidadpedido"]),
                                Amortizar = Convert.ToInt32(srd["Amortizar"])
                            });
                        }
                    }
                    con.Close();
                }
            }
                
            return jss.Serialize(pedidosAmortizacion);
        }

        public string SaveFileArticulosDiff(string json)
        {
            string s = "";
            List<object> arr = new List<object>();
            dynamic data = jss.DeserializeObject(json);
            string dfile = data["file"];
            string usuario = data["usuario"];
            string name = "archivo_excel";
            string file = dfile.Split(',')[1];
            string ext = (dfile.Split(';')[0]).Split('/')[1];
            int asignaConsecutivoPedido = 0;
            byte[] toDecodeByte = Convert.FromBase64String(file);

            string filename = "_" + DateTime.Now.ToString("yyyy-MM-dd-HHmmss");

            if (ext.Equals("vnd.openxmlformats-officedocument.wordprocessingml.document"))
            {
                ext = "docx";
            }
            if (ext.Equals("vnd.openxmlformats-officedocument.spreadsheetml.sheet"))
            {
                ext = "xlsx";
            }

            if (ext.Equals("pdf"))
            {

                ext = "pdf";      //
            }

            String path = AppDomain.CurrentDomain.BaseDirectory;
            string ruta = path + "ArchivosExcel/";

            if (!Directory.Exists(ruta))
            {
                Directory.CreateDirectory(ruta);
            }

            if (!File.Exists(ruta + filename + "." + ext))
            {
                var fs = new BinaryWriter(new FileStream(ruta + name + "." + ext, FileMode.Append, FileAccess.Write));
                fs.Write(toDecodeByte);
                fs.Close();
                //fs.ToString();
               
            }
            string excel = ruta + name + "." + ext;
            //Boolean EsArchivoProcesado = ProcesarArchivoExcel(excel, usuario, out Esprocesodo);
            Boolean Esprocesodo;
            int totalRegExcel = 0;
            List<temp_entrada_producto_aux> temp_pedidos = ProcesarArchivoExcel(excel, usuario,conn, out Esprocesodo, out totalRegExcel);
            List<temp_entrada_producto> temp_pedido_ = new List<temp_entrada_producto>();
            if (Esprocesodo)
            {
                //
                File.Delete(ruta + filename + "." + ext);
                var fs = new BinaryWriter(new FileStream(ruta + filename + "." + ext, FileMode.Append, FileAccess.Write));
                fs.Write(toDecodeByte);
                fs.Close();
            }
            int pedidoValidar = temp_pedidos[0].pedido;
            string cod_articulo = temp_pedidos[0].cod_articulo;

            //validar pedido legalizado 

            var detallePediodo = conn.detalle_pedido.Where(x => x.id_pedido == pedidoValidar && x.codigo_art.Equals(cod_articulo)).First();
            var pedidoAmort = conn.amortizacion.Where(x => x.id_pedido == pedidoValidar && x.cod_art.Equals(cod_articulo) && x.EsIngresada == 0).ToList();
            
            if(detallePediodo != null)
            {
                if (totalRegExcel - 1 > pedidoAmort.Count())
                {

                    arr.Add(0);
                    arr.Add("Error la cantidad de registros en el excel supera la cantida del pedido con el articulo");
                }
                else
                {
                    foreach(temp_entrada_producto_aux tem_aux in temp_pedidos)
                    {
                        CrearConsecutivoPedidoAmort(conn, out asignaConsecutivoPedido);
                        temp_entrada_producto temp_entrada = new temp_entrada_producto();
                        temp_entrada.tipo = tem_aux.tipo;
                        temp_entrada.pedido = tem_aux.pedido;
                        temp_entrada.asignacion = asignaConsecutivoPedido + "";
                        temp_entrada.cod_articulo = tem_aux.cod_articulo;
                        temp_entrada.descripcion = tem_aux.descripcion;
                        temp_entrada.observacion = tem_aux.observacion;
                        temp_entrada.ccosto = tem_aux.ccosto;
                        temp_entrada.causacion = tem_aux.causacion;
                        temp_entrada.Estado = tem_aux.Estado;
                        temp_entrada.FechaCreacion = tem_aux.FechaCreacion;
                        temp_entrada.usuario = tem_aux.usuario;
                        temp_entrada.EsProcesado = 0;

                        conn.temp_entrada_producto.Add(temp_entrada);
                        conn.SaveChanges();

                    }
                    
                }
            }
            else
            {
                arr.Add(0);
                arr.Add("Error al procesar el Excel");
            }

            temp_pedido_ = conn.temp_entrada_producto.Where(x => x.pedido == pedidoValidar).ToList();
            //var detallePedido = 
            arr.Add(1);
            arr.Add(temp_pedido_);
            return jss.Serialize(arr);
        }

        public string setIngresoAmortizacion(string json)
        {
            string s = "";
            //dynamic data = jss.DeserializeObject(json);

            //string arrayTemp_entrada = data["registros"];

            JObject data = JObject.Parse(json);
            string registros = Convert.ToString(data["registros"]);

            List<temp_entrada_producto> array_tem_obj = JsonConvert.DeserializeObject<List<temp_entrada_producto>>(registros);

            try
            {
                foreach (temp_entrada_producto tempP in array_tem_obj)
                {
                    entrada_producto_diferido prod_diff = new entrada_producto_diferido();

                    prod_diff.tipo = tempP.tipo;
                    prod_diff.pedido = tempP.pedido;
                    prod_diff.asignacion = tempP.asignacion;
                    prod_diff.cod_articulo = tempP.cod_articulo;
                    prod_diff.descripcion = tempP.descripcion;
                    prod_diff.observacion = tempP.observacion;
                    prod_diff.ccosto = tempP.ccosto;
                    prod_diff.causacion = tempP.causacion;
                    prod_diff.Estado = tempP.Estado;
                    prod_diff.FechaCreacion = DateTime.Now;
                    prod_diff.usuario = tempP.usuario;
                    prod_diff.EsProcesado = 0;
                    conn.entrada_producto_diferido.Add(prod_diff);
                    int GuardaProductoDiff = conn.SaveChanges();

                   if(GuardaProductoDiff > 0)
                    {
                        tempP.EsProcesado = 1;
                        conn.Entry(tempP).State = System.Data.EntityState.Modified;
                        conn.SaveChanges();
                        amortizacion amort = conn.amortizacion.Where(x => x.id_pedido == tempP.pedido &&
                                                                        x.cod_art == tempP.cod_articulo &&
                                                                        x.ccosto == tempP.ccosto &&
                                                                        x.EsIngresada == 0 &&
                                                                        x.Imei == null).First();
                        
                        if (amort != null)
                        {
                            amort.EsIngresada = 1;
                            amort.Imei = tempP.asignacion;
                            conn.Entry(amort).State = System.Data.EntityState.Modified;
                            conn.SaveChanges();
                        }
                    }
                    
                    
                }
                //conn.SaveChanges();
                s = "si";
                return jss.Serialize(s);
            }
            catch (DbEntityValidationException e)
            {
                string err = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        err += ve.ErrorMessage;
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return jss.Serialize(err);
            }
        }

        public static List<temp_entrada_producto_aux> ProcesarArchivoExcel(string path, string usuario, rhinoEntities Rinho , out Boolean Esprocesado , out int cantidaExcel)
        {
            
            Esprocesado = false;
            cantidaExcel = 0;
            List<temp_entrada_producto_aux> temp_produc = new List<temp_entrada_producto_aux>();
            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            ExcelFile workbook = ExcelFile.Load(path);
            var sb = new StringBuilder();
            foreach (var worksheet in workbook.Worksheets)
            {
                sb.AppendLine();
                sb.AppendFormat("{0} {1} {0}", new string('-', 25), worksheet.Name);
                int counter = 0;
                cantidaExcel = worksheet.Rows.Count();
                
                
                foreach (var row in worksheet.Rows)
                {
                    if (counter >= 1)
                    {
                       
                        
                        //Excel e = new Excel();
                        sb.AppendLine();
                        temp_produc.Add(new temp_entrada_producto_aux
                        {
                            
                            tipo = Convert.ToInt32(row.AllocatedCells[0].StringValue),
                            pedido = Convert.ToInt32(row.AllocatedCells[1].StringValue),
                            
                            cod_articulo = row.AllocatedCells[3].StringValue,
                            descripcion = row.AllocatedCells[4].StringValue,
                            observacion = row.AllocatedCells[5].StringValue,
                            ccosto = row.AllocatedCells[6].StringValue,
                            causacion = DateTime.Now,
                            Estado = 1,
                            FechaCreacion = DateTime.Now,
                            usuario = usuario,
                            EsProcesado = 0
                        }); ; ;

                        
                        
                        
                    }
                    counter++;
                }
                
            }
            if(temp_produc.Count() == cantidaExcel - 1)
            {
                Esprocesado = true;
            }
            return temp_produc;
        }

        private static void CrearConsecutivoPedidoAmort(rhinoEntities Rinho,out int consecutivo)
        {

            consecutivo = 1000;


            string consecutivoAsing = ultimoConsecutivoBaseDato();
            if(consecutivoAsing != "")
            {
                consecutivo = Convert.ToInt32(consecutivoAsing) + 1;
            }
           
        }

        public static string ultimoConsecutivoBaseDato()
        {
            string consecutvo = "";
            var connection = System.Configuration.ConfigurationManager.ConnectionStrings["conexion"].ConnectionString;
            string query = "select max(asignacion) as asignacion from temp_entrada_producto";
            using (SqlConnection con = new SqlConnection(connection.ToString()))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader srd = cmd.ExecuteReader())
                    {
                        while (srd.Read())
                        {
                            if(srd["asignacion"] != null)
                            {
                                consecutvo = srd["asignacion"] + "";
                            }
                        }
                    }
                    con.Close();
                }
            }

            return consecutvo;
        }
        public static string Base64Encode(string base64)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(base64);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        #endregion

    }
}
